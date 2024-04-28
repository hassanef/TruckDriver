using TruckDriver.Application.Dto;
using TruckDriver.Application.Queries.Contracts;
using TruckDriver.Domain.IRepositories;

namespace TruckDriver.Application.Queries
{
    public class TruckDriverQuery(ITruckDriverRepository truckDriverRepository) : ITruckDriverQuery
    {
        private readonly ITruckDriverRepository _truckDriverRepository = truckDriverRepository;
        public async Task<IEnumerable<DriverDto>> GetAsync(string location)
        {
            var drivers = await _truckDriverRepository.GetAsync(driver => driver.Location.ToLower() == location.ToLower());
            if (drivers is null)
                throw new ArgumentNullException(nameof(drivers), "truck drivers query is null!");

            var driverDtos = drivers.Select(driver => new DriverDto(driver.Id,
                                                                    driver.FirstName,
                                                                    driver.LastName,
                                                                    driver.Location));

            return driverDtos;
        }
    }
}
