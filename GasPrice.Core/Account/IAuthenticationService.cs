using System;

namespace GasPrice.Core.Account
{

    public interface IAuthenticationService
    {
        IUserAccountService UserAccountService { get; set; }
        void SignIn(Guid userId, bool persistent = false);
        void SignIn(UserAccount account, bool persistent = false);
        void SignIn(UserAccount account, string method, bool persistent = false);
        void SignOut();
    }
}