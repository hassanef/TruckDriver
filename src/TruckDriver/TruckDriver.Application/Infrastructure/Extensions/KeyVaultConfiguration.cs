using Azure.Security.KeyVault.Secrets;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TruckDriver.Domain.Utils;
using TruckDriver.Infrastructure.Extensions;

namespace TruckDriver.Application.Infrastructure.Extensions
{
    public static class KeyVaultConfiguration
    {
        public static void ConfigureAzureKeyVault(this IServiceCollection services, IConfiguration configuration)
        {
            var keyVaultUri = configuration[AzureKeys.KeyVaultUri];
            if (keyVaultUri is null)
                throw new ArgumentNullException(nameof(keyVaultUri));

            //TODO: This part of code is just for testing and it must be remove from there
            var credential = AzureKeyVaultCredential.Create(configuration["Azure:TenantId"], configuration["Azure:ClientId"], configuration["Azure:SecretKey"]);
            if (credential is null)
                throw new ArgumentNullException(nameof(credential));

            services.AddSingleton(sp =>
            {
                return new SecretClient(new Uri(keyVaultUri), credential);
            });
        }
    }
}
