using Azure.Security.KeyVault.Secrets;
using Microsoft.Extensions.Configuration;
using TruckDriver.Domain.IServices;
using TruckDriver.Domain.Utils;

namespace TruckDriver.Infrastructure.Services
{
    public class AzureKeyVaultService : ISecretService
    {
        private readonly IConfiguration _configuration;
        private readonly SecretClient _secretClient;

        public AzureKeyVaultService(IConfiguration configuration, SecretClient secretClient)
        {
            _configuration = configuration;
            _secretClient = secretClient;
        }

        public async Task<string> GetSecret(string secreKey)
        {
            try
            {
                var keyVaultUri = _configuration[AzureKeys.KeyVaultUri];
                if (keyVaultUri is null)
                    throw new ArgumentNullException(nameof(keyVaultUri));

                var secret = await _secretClient.GetSecretAsync(secreKey);
                if (secret is null)
                    throw new ArgumentNullException(nameof(secret), "The result of Azure keyvault is null!");

                return secret.Value.Value;
            }
            catch (Exception ex)
            {
                // Log the exception
                throw new InvalidOperationException("AzureKeyVault failed!");
            }
        }
    }
}
