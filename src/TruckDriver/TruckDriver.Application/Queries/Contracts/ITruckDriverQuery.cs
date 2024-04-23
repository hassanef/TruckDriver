using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TruckDriver.Application.Dto;
using TruckDriver.Domain.Models;

namespace TruckDriver.Application.Queries.Contracts
{
    public interface ITruckDriverQuery
    {
        Task<IEnumerable<DriverDto>> GetAsync(string location);
    }
}
