using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TruckDriver.Domain.Utils;

namespace TruckDriver.Application.Infrastructure.Extensions
{
    public static class KeyVaultConfiguration
    {
        public static void ConfigureAzureKeyVault(this IServiceCollection services, IConfiguration configuration)
        {
            var keyVaultUri = configuration[AzureKeys.KeyVaultUri];

            services.AddSingleton(sp =>
            {
                return new SecretClient(new Uri(keyVaultUri), new DefaultAzureCredential());
            });
        }
    }
}
