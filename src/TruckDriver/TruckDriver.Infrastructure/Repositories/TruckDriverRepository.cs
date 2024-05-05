using TruckDriver.Domain.IRepositories;
using TruckDriver.Domain.Models;
using TruckDriver.Infrastructure.Services.Contract;

namespace TruckDriver.Infrastructure.Repositories
{
    public class TruckDriverRepository(ICosmosContainerService container) : GenericRepository<Driver>(container), ITruckDriverRepository
    {
    }
}
