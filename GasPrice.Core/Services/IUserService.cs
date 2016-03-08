using System.Collections.Generic;
using GasPrice.Core.Account;

namespace GasPrice.Core.Services
{
    public interface IUserService : IService<UserAccount>
    {
        IEnumerable<UserAccount> GetUsers();
    }
}
