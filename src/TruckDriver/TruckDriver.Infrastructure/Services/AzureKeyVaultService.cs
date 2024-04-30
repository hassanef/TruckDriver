using Azure.Security.KeyVault.Secrets;
using TruckDriver.Domain.IServices;

namespace TruckDriver.Infrastructure.Services
{
    public class AzureKeyVaultService : ISecretService
    {
        private readonly SecretClient _secretClient;

        public AzureKeyVaultService(SecretClient secretClient)
        {
            _secretClient = secretClient;
        }

        public async Task<string> GetSecret(string secreKey)
        {
            try
            {
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
