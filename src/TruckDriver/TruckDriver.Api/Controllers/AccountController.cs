using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TruckDriver.Application.Models;
using TruckDriver.Application.Queries.Contracts;
using TruckDriver.Domain.IIdentityServices;

namespace TruckDriver.Api.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class AccountController(IAccountService accountService) : ControllerBase
    {
        private readonly IAccountService _accountService = accountService;

        [AllowAnonymous]
        [HttpPost("SignIn")]
        public async Task<IActionResult> SignIn([FromBody] SignInInput input)
        {
            var token = await _accountService.SignIn(input.Key);
            if (!string.IsNullOrWhiteSpace(token))
                return Ok(token);
            return NotFound();
        }
    }
}
