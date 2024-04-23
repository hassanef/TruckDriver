using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Extensions.Configuration;
using TruckDriver.Domain.IServices;
using TruckDriver.Domain.Utils;

namespace TruckDriver.Infrastructure.Services
{
    public class AzureKeyVaultService : ISecretService
    {
        private readonly IConfiguration _configuration;

        public AzureKeyVaultService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<string> GetSecret(string secreKey)
        {
            try
            {
                var keyVaultUri = _configuration[AzureKeys.KeyVaultUri];
                var secretClient = new SecretClient(new Uri(keyVaultUri), new DefaultAzureCredential());
                var secret = await secretClient.GetSecretAsync(secreKey);
                
                return secret.Value.Value;
            }
            catch (Exception ex)
            {
                // Log the exception
                throw new Exception("AzureKeyVault failed!");
            }
        }
    }
}
