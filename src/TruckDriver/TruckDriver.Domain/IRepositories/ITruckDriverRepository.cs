using TruckDriver.Domain.Models;

namespace TruckDriver.Domain.IRepositories
{
    public interface ITruckDriverRepository
    {
        Task<IEnumerable<Driver>> GetAsync(string location);
    }
}
