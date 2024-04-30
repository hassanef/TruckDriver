using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TruckDriver.Domain.Utils;

namespace TruckDriver.Application.Infrastructure.Extensions
{
    public static class KeyVaultConfiguration
    {
        public static void AddAzureKeyVault(this IServiceCollection services, IConfiguration configuration)
        {
            var keyVaultUri = configuration[AzureKeys.KeyVaultUri];
            if (keyVaultUri is null)
                throw new ArgumentNullException(nameof(keyVaultUri));

            services.AddSingleton(sp =>
            {
                return new SecretClient(new Uri(keyVaultUri), new DefaultAzureCredential());
            });
        }
    }
}
