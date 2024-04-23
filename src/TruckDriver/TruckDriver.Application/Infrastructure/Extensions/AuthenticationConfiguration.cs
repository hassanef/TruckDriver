using Azure.Identity;
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
        public static void ConfigureAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var secretClient = new SecretClient(new Uri(configuration[AzureKeys.KeyVaultUri]), new DefaultAzureCredential());
            var appIdUriValue = secretClient.GetSecret(configuration[AzureKeys.ApplicationIdUri]).Value;

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = "https://login.microsoftonline.com/914a7823-10b4-403f-8482-e4ce36daffe4";
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
