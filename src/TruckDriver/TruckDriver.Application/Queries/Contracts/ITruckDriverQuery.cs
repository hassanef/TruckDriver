using TruckDriver.Application.Dto;

namespace TruckDriver.Application.Queries.Contracts
{
    public interface ITruckDriverQuery
    {
        Task<IEnumerable<DriverDto>> GetAsync(string location);
    }
}
