using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TruckDriver.Application.Queries;
using TruckDriver.Application.Queries.Contracts;
using TruckDriver.Domain.IRepositories;
using TruckDriver.Domain.Utils;
using TruckDriver.Infrastructure.Repositories;

namespace TruckDriver.Application.Infrastructure.Extensions
{
    public static class CosmosDBConfiguration
    {
        public static void ConfigureCosmosDB(this IServiceCollection services, IConfigurationRoot configurationRoot)
        {
            services.AddSingleton<CosmosClient>(sp =>
            {
                var configuration = sp.GetRequiredService<IConfiguration>();
                var cosmosEndpoint = configurationRoot[configuration[AzureKeys.CosmosEndpoint]];
                var cosmosKey = configurationRoot[configuration[AzureKeys.CosmosKey]];

                return new CosmosClient(cosmosEndpoint, cosmosKey);
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

            services.AddScoped<ITruckDriverRepository, TruckDriverRepository>();
            services.AddScoped<ITruckDriverQuery, TruckDriverQuery>();
        }
    }
}
