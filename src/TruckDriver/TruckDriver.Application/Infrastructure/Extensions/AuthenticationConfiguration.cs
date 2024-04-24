using Azure.Security.KeyVault.Secrets;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TruckDriver.Domain.Utils;
using TruckDriver.Infrastructure.Extensions;

namespace TruckDriver.Application.Infrastructure.Extensions
{
    public static class AuthenticationConfiguration
    {
        public static void ConfigureAuthentication(this IServiceCollection services, WebApplicationBuilder builder)
        {
            //TODO: This part of code is just for testing and it must be remove from there
            var credential = AzureKeyVaultCredential.Create(builder.Configuration["Azure:TenantId"], builder.Configuration["Azure:ClientId"], builder.Configuration["Azure:SecretKey"]);
            if (credential is null)
                throw new ArgumentNullException(nameof(credential));

            var secretClient = new SecretClient(new Uri(builder.Configuration[AzureKeys.KeyVaultUri]), credential);
            if (secretClient is null)
                throw new ArgumentNullException(nameof(secretClient));

            var appIdUriValue = secretClient.GetSecret(builder.Configuration[AzureKeys.ApplicationIdUri]).Value;
            if (appIdUriValue is null)
                throw new ArgumentNullException(nameof(appIdUriValue));
            var authorityEndpointUriValue = secretClient.GetSecret(builder.Configuration[AzureKeys.AuthorityEndpointUri]).Value;
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
