using Microsoft.Azure.Cosmos;

namespace TruckDriver.Infrastructure.Services.Contract
{
    public interface ICosmosContainerService
    {
        Container GetContainer();
    }
}
