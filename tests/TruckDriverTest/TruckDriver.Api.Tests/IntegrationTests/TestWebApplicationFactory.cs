using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using TruckDriver.Test.TestDoubles;

namespace TruckDriver.Test.IntegrationTests
{
    public class TestWebApplicationFactory<T> : WebApplicationFactory<T> where T : Program
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {
                // Disable authentication and authorization
                services.AddAuthentication(TestAuthenticationParameter.scheme)
                    .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(TestAuthenticationParameter.scheme, options => { });

                services.AddAuthorization(options =>
                {
                    options.DefaultPolicy = new AuthorizationPolicyBuilder()
                        .RequireAuthenticatedUser()
                        .Build();
                });
            });
        }
    }
}
