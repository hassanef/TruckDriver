using Microsoft.Extensions.DependencyInjection;
using TruckDriver.Application.Queries;
using TruckDriver.Application.Queries.Contracts;
using TruckDriver.Domain.IIdentityServices;
using TruckDriver.Domain.IRepositories;
using TruckDriver.Domain.IServices;
using TruckDriver.Infrastructure.IdentityServices;
using TruckDriver.Infrastructure.Repositories;
using TruckDriver.Infrastructure.Services;

namespace TruckDriver.Application.Infrastructure.Extensions
{
    public static class ServiceCollection
    {
        public static IServiceCollection AddTruckDriverServices(this IServiceCollection services)
        {
            services.AddScoped<ITruckDriverQuery, TruckDriverQuery>();
            services.AddScoped<ITruckDriverRepository, TruckDriverRepository>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<ISecretService, AzureKeyVaultService>();

            // Add more services here if needed

            return services;
        }
    }
}
