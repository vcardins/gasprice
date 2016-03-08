using System.Collections.Generic;
using System.Security.Claims;

namespace GasPrice.Core.ViewModels.UserAccount
{

    public class AuthenticatedAccount
    {
        public int UserId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string DisplayName { get; set; }

        public IList<Claim> Claims { get; set; }

        public AuthenticatedAccount()
        {
            Claims = new List<Claim>();
        }

    }
}