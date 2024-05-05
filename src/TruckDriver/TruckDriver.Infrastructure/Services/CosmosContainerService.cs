using Microsoft.Azure.Cosmos;
using TruckDriver.Infrastructure.Services.Contract;

namespace TruckDriver.Infrastructure.Repositories
{
    public class CosmosContainerService : ICosmosContainerService
    {
        private readonly Container _container;

        public CosmosContainerService(Container container)
        {
            _container = container;
        }

        public Container GetContainer()
        {
            return _container;
        }
    }
}
