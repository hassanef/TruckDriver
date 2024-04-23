using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TruckDriver.Domain.IServices
{
    public interface ISecretService
    {
        Task<string> GetSecret(string secreKey);
    }
}
