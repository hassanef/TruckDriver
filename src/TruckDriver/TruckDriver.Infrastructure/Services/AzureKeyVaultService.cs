using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Extensions.Configuration;
using TruckDriver.Domain.IServices;
using TruckDriver.Domain.Utils;
using TruckDriver.Infrastructure.Extensions;

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
                //credential is just for testing there
                var credential = AzureKeyVaultCredential.Create(_configuration["Azure:TenantId"], _configuration["Azure:ClientId"], _configuration["Azure:SecretKey"]);
                if(credential is null)
                    throw new ArgumentNullException(nameof(credential));

                var keyVaultUri = _configuration[AzureKeys.KeyVaultUri];
                if (keyVaultUri is null)
                    throw new ArgumentNullException(nameof(keyVaultUri));

                var secretClient = new SecretClient(new Uri(keyVaultUri), credential);
                if (secretClient is null)
                    throw new ArgumentNullException(nameof(secretClient));

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
