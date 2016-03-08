using GasPrice.Core.Account;

namespace GasPrice.Core.Models.Admin
{
    public class UserRole : BaseObjectState
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public virtual UserAccount User { get; set; }
        public virtual Role Role { get; set; }
        public bool Active { get; set; }

    }
}
