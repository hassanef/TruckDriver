namespace TruckDriver.Domain.IIdentityServices
{
    public interface IAccountService
    {
        Task<string> SignIn(string key);
    }
}
