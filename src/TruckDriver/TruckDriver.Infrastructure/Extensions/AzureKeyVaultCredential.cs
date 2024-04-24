using Azure.Identity;

namespace TruckDriver.Infrastructure.Extensions
{
    public static class AzureKeyVaultCredential
    {
        public static ClientSecretCredential Create(string tenantId, string clientId, string secretKey)
        {
            if (!string.IsNullOrWhiteSpace(tenantId) && !string.IsNullOrWhiteSpace(clientId) && !string.IsNullOrWhiteSpace(secretKey))
                return new ClientSecretCredential(tenantId, clientId, secretKey);
            throw new ArgumentNullException("Some parameters to create Azure keyvault credential are null!");
        }
    }
}
