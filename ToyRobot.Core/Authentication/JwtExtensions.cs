using Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
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
        if (settings.Secret == null)
        {
            throw new Exception($"Missing Secret section {JwtSettings.SectionName}");
        }

        var key = Encoding.ASCII.GetBytes(settings.Secret);
        services.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(x =>
        {
            x.TokenValidationParameters = new TokenValidationParameters
            {
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidIssuer = settings.Issuer,
                ValidAudience = settings.Audience,
            };
        });

        return services;
    }
}
