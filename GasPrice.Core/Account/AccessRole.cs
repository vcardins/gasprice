
using GasPrice.Core.Account.Enum;

namespace GasPrice.Core.Account
{
    public class AccessRole
    {
        public int BitMask
        {
            get { return (int)Name; }
        }

        public UserRoleEnum Name { get; set; }

    }
}
