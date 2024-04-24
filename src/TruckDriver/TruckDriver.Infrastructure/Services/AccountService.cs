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
            if(!string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException("Secret key is null!");
            try
            {
                var appIdUriValue = await _secretService.GetSecret(_appIdUri);
                if (appIdUriValue is null)
                    throw new ArgumentNullException(nameof(appIdUriValue), "The result of Azure keyvault for appIdUriValue is null!");

                var authorityEndpointUriValue = await _secretService.GetSecret(_authorityEndpointUri);
                if (authorityEndpointUriValue is null)
                    throw new ArgumentNullException(nameof(authorityEndpointUriValue), "The result of Azure keyvault for authorityEndpointUriValue is null!");

                var app = ConfidentialClientApplicationBuilder
                              .Create(appIdUriValue)
                              .WithClientSecret(key)
                              .WithAuthority(new Uri(authorityEndpointUriValue))
                              .Build();

                var authenticationResult = await app.AcquireTokenForClient(new[] { $"{appIdUriValue}/.default" }).ExecuteAsync();
                if (authenticationResult is null)
                    throw new ArgumentNullException(nameof(authenticationResult), "The result of service to get a token is null!");

                return authenticationResult.AccessToken;
            }
            catch (Exception ex)
            {
                // Log the exception
                throw new InvalidOperationException("Authentication failed!");
            }
        }
    }
}
