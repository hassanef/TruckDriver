using Azure.Security.KeyVault.Secrets;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TruckDriver.Domain.Utils;

namespace TruckDriver.Application.Infrastructure.Extensions
{
    public static class AuthenticationConfiguration
    {
        public static void AddAuthentication(this IServiceCollection services, IConfiguration configuratio)
        {
            var secretClient = services.BuildServiceProvider().GetRequiredService<SecretClient>();
            if (secretClient is null)
                throw new ArgumentNullException(nameof(secretClient));

            var appIdUriValue = secretClient.GetSecret(configuratio[AzureKeys.ApplicationIdUri]).Value;
            if (appIdUriValue is null)
                throw new ArgumentNullException(nameof(appIdUriValue));

            var authorityEndpointUriValue = secretClient.GetSecret(configuratio[AzureKeys.AuthorityEndpointUri]).Value;
            if (authorityEndpointUriValue is null)
                throw new ArgumentNullException(nameof(authorityEndpointUriValue));

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = authorityEndpointUriValue.Value;
                    options.Audience = appIdUriValue.Value;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidAudience = appIdUriValue.Value,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Jwt:SecretKey"))
                    };
                });

            services.AddAuthorization();
        }
    }
}
