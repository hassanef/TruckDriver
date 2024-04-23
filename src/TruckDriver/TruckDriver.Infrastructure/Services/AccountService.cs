using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client;
using TruckDriver.Domain.IIdentityServices;
using TruckDriver.Domain.IServices;
using TruckDriver.Domain.Utils;

namespace TruckDriver.Infrastructure.IdentityServices
{
    public class AccountService : IAccountService
    {
        private readonly ISecretService _secretService;
        private readonly string _appIdUri;
        private readonly string _authorityEndpointUri;

        public AccountService(ISecretService secretService, IConfiguration configuration)
        {
            _secretService = secretService;
            _appIdUri = configuration[AzureKeys.ApplicationIdUri] ?? throw new ArgumentNullException(AzureKeys.ApplicationIdUri);
            _authorityEndpointUri = configuration[AzureKeys.AuthorityEndpointUri] ?? throw new ArgumentNullException(AzureKeys.AuthorityEndpointUri);
        }
        public async Task<string> SignIn(string key)
        {
            try
            {
                var appIdUriValue = await _secretService.GetSecret(_appIdUri);
                var authorityEndpointUriValue = await _secretService.GetSecret(_authorityEndpointUri);

                var app = ConfidentialClientApplicationBuilder
                              .Create(appIdUriValue)
                              .WithClientSecret(key)
                              .WithAuthority(new Uri(authorityEndpointUriValue))
                              .Build();

                var result = await app.AcquireTokenForClient(new[] { $"{appIdUriValue}/.default" }).ExecuteAsync();

                return result.AccessToken;
            }
            catch (Exception ex)
            {
                // Log the exception
                throw new Exception("Authentication failed!");
            }
        }
    }
}
