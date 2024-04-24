using Azure.Security.KeyVault.Secrets;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TruckDriver.Domain.Utils;

namespace TruckDriver.Application.Infrastructure.Extensions
{
    public static class CosmosDBConfiguration
    {
        public static void ConfigureCosmosDB(this IServiceCollection services, IConfiguration configuration)
        {
            var secretClient = services.BuildServiceProvider().GetRequiredService<SecretClient>();
            if (secretClient is null)
                throw new ArgumentNullException(nameof(secretClient));

            var cosmosEndpoint = secretClient.GetSecret(configuration[AzureKeys.CosmosEndpoint])?.Value;
            if (cosmosEndpoint is null)
                throw new ArgumentNullException(nameof(cosmosEndpoint));

            var cosmosKey = secretClient.GetSecret(configuration[AzureKeys.CosmosKey])?.Value;
            if (cosmosKey is null)
                throw new ArgumentNullException(nameof(cosmosKey));

            services.AddSingleton<CosmosClient>(sp =>
            {
                return new CosmosClient(cosmosEndpoint?.Value, cosmosKey?.Value);
            });

            services.AddSingleton(sp =>
            {
                var cosmosClient = sp.GetRequiredService<CosmosClient>();
                var configuration = sp.GetRequiredService<IConfiguration>();
                var databaseName = configuration[AzureKeys.DatabaseName];
                var containerName = configuration[AzureKeys.ContainerName];
                var database = cosmosClient.GetDatabase(databaseName);
                return database.GetContainer(containerName);
            });

           
        }
    }
}
