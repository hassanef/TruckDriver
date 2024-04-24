using Microsoft.AspNetCore.Builder;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TruckDriver.Application.Queries;
using TruckDriver.Application.Queries.Contracts;
using TruckDriver.Domain.IRepositories;
using TruckDriver.Domain.Utils;
using TruckDriver.Infrastructure.Extensions;
using TruckDriver.Infrastructure.Repositories;

namespace TruckDriver.Application.Infrastructure.Extensions
{
    public static class CosmosDBConfiguration
    {
        public static void ConfigureCosmosDB(this IServiceCollection services, WebApplicationBuilder builder)
        {
            //TODO: This part of code is just for testing and it must be remove from there///////////////
            var credential = AzureKeyVaultCredential.Create(builder.Configuration["Azure:TenantId"], builder.Configuration["Azure:ClientId"], builder.Configuration["Azure:SecretKey"]);
            if (credential is null)
                throw new ArgumentNullException(nameof(credential));

            var configurationWithAzureKeyVault = builder.Configuration.AddAzureKeyVault(
                                    vaultUri: new Uri(builder.Configuration[AzureKeys.KeyVaultUri]),
                                    credential: credential
                                ).Build();
            ////////////////////////////////////////////////////////////////////////////////////////////
            services.AddSingleton<CosmosClient>(sp =>
            {
                var configuration = sp.GetRequiredService<IConfiguration>();
                var cosmosEndpoint = configurationWithAzureKeyVault[configuration[AzureKeys.CosmosEndpoint]];
                var cosmosKey = configurationWithAzureKeyVault[configuration[AzureKeys.CosmosKey]];

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
