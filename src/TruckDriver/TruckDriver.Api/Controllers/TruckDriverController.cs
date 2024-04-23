using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using TruckDriver.Application.Dto;
using TruckDriver.Application.Queries.Contracts;

namespace TruckDriver.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class TruckDriverController(ITruckDriverQuery truckDriverQuery) : ControllerBase
    {
        private readonly ITruckDriverQuery _truckDriverQuery = truckDriverQuery;

        /// <summary>
        /// Gets truck drivers by location.
        /// </summary>
        /// <param name="location">The location of the truck drivers.</param>
        /// <returns>A list of truck drivers.</returns>
        [HttpGet("{location}")]
        [ProducesResponseType(typeof(IEnumerable<DriverDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(string location)
        {
            var result = await _truckDriverQuery.GetAsync(location);
            if (!result.IsNullOrEmpty())
                return Ok(result);
            return NotFound();
        }
    }
}
