using Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection;

public static class JwtExtensions
{
    public static IServiceCollection AddJwtTokenAuthentication(this IServiceCollection services, IConfiguration config)
    {
        services.Configure<JwtSettings>(config.GetSection(JwtSettings.SectionName));
        services.AddSingleton<IJwtService, JwtService>();
        var settings = config.GetSection(JwtSettings.SectionName).Get<JwtSettings>();
        if (settings == null)
        {
            throw new Exception($"Missing section {JwtSettings.SectionName}");
        }

        var x509 = new X509Certificate2(File.ReadAllBytes(settings.CertificatePath), settings.CertificatePassword);
        var rsaPublic = x509.GetRSAPublicKey();
        if (rsaPublic == null)
            throw new Exception("Invalid rsa key");

        services.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(x =>
        {
            x.TokenValidationParameters = new TokenValidationParameters
            {                
                IssuerSigningKey = new RsaSecurityKey(rsaPublic),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidIssuer = settings.Issuer,
                ValidAudience = settings.Audience,
                RequireExpirationTime = false
            };
        });

        return services;
    }
}
