using JWT.Algorithms;
using JWT.Builder;
using JWT.Exceptions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace Authentication;
public class JwtService : IJwtService
{
    private readonly JwtSettings jwtSettings;
    private readonly ILogger<JwtService> logger;
    private readonly RSA rsaPublic;
    private readonly RSA rsaPrivate;

    public JwtService(IOptions<JwtSettings> jwtSettings, ILogger<JwtService> logger)
    {
        this.jwtSettings = jwtSettings.Value;
        this.logger = logger;

        var x509 = new X509Certificate2(File.ReadAllBytes(this.jwtSettings.CertificatePath), this.jwtSettings.CertificatePassword);
        var rsaPublic = x509.GetRSAPublicKey();
        if (rsaPublic == null)
            throw new Exception("Invalid rsa key");

        var rsaPrivate = x509.GetRSAPrivateKey();
        if (rsaPrivate == null)
            throw new Exception("Invalid rsa key");

        this.rsaPublic = rsaPublic;
        this.rsaPrivate =  rsaPrivate;
    }

    public IDictionary<string, object> Decode(string token)
    {
        IDictionary<string, object>? values = null;
        try
        {
            this.logger.LogTrace("Decoding token {token}", token);

            values = JwtBuilder.Create()
                .WithAlgorithm(new RS256Algorithm(this.rsaPublic))
                .MustVerifySignature()
                .Decode<IDictionary<string, object>>(token);
        }
        catch (TokenExpiredException ex)
        {
            this.logger.LogWarning(ex, "TokenExpiredException");
            throw;
        }
        catch (SignatureVerificationException ex)
        {
            this.logger.LogWarning(ex, "SignatureVerificationException");
            throw;
        }
        catch(Exception ex)
        {
            this.logger.LogError(ex, "Generic Exception");
            throw;
        }
        finally
        {
            if (logger.IsEnabled(LogLevel.Trace))
            {
                if (values != null)
                {
                    var stValues = JsonConvert.SerializeObject(values);
                    this.logger.LogTrace("Decoded claims {values}", stValues);
                }
                else
                {
                    this.logger.LogTrace("Decoded without claims {token}", token);
                }
            }
        }
        return values;
    }
    public string CreateToken()
    {
        return CreateToken(new Dictionary<string, object>());
    }
    public string CreateToken(IDictionary<string,object> claims)
    {
        try
        {
            if (logger.IsEnabled(LogLevel.Trace))
            {
                if (claims.Count > 0)
                {
                    var stValues = JsonConvert.SerializeObject(claims);
                    this.logger.LogTrace("CreateToken claims {values}", stValues);
                }
                else
                {
                    this.logger.LogTrace("CreateToken without claims");
                }
            }

            var builder = JwtBuilder.Create()
                .WithAlgorithm(new RS256Algorithm(this.rsaPublic, this.rsaPrivate))
                .Issuer(this.jwtSettings.Issuer)
                .Audience(this.jwtSettings.Audience);
            if (this.jwtSettings.ExpirationMinutes.HasValue)
            {
                builder.AddClaim("exp", DateTimeOffset.UtcNow.AddMinutes(this.jwtSettings.ExpirationMinutes.Value));
            }
            foreach (var kv in claims)
            {
                builder.AddClaim(kv.Key, kv.Value);
            }
            var value = builder.Encode();
            logger.LogTrace("Token created {value}", value);
            return value;
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "Error creating token");
            throw;
        }
    }
}
