﻿using JWT.Algorithms;
using JWT.Builder;
using JWT.Exceptions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Authentication;
public class JwtService : IJwtService
{
    private readonly JwtSettings jwtSettings;
    private readonly ILogger<JwtService> logger;
    public JwtService(IOptions<JwtSettings> jwtSettings, ILogger<JwtService> logger)
    {
        this.jwtSettings = jwtSettings.Value;
        this.logger = logger;
    }

    public IDictionary<string, object> Decode(string token)
    {
        IDictionary<string, object>? values = null;
        try
        {
            this.logger.LogTrace("Decoding token {token}", token);
            values = JwtBuilder.Create()
                .WithAlgorithm(new HMACSHA256Algorithm())
                .WithSecret(this.jwtSettings.Secret)
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
                    this.logger.LogTrace("Decoded claims {values}", values);
                }
                else
                {
                    this.logger.LogTrace("Decoded without claims {token}", token);
                }
            }
        }
        return values;
    }

    public string CreateToken(IDictionary<string,object> claims)
    {
        var builder = JwtBuilder.Create()
            .WithAlgorithm(new HMACSHA256Algorithm())
            .WithSecret(this.jwtSettings.Secret);
        if (this.jwtSettings.ExpirationMinutes.HasValue)
        {
            builder.AddClaim("exp", DateTimeOffset.UtcNow.AddMinutes(this.jwtSettings.ExpirationMinutes.Value));
        }
        foreach(var kv in claims)
        {
            builder.AddClaim(kv.Key, kv.Value);
        }
        return builder.Encode();
    }
}
