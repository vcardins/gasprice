using System.Collections.Generic;

namespace GasPrice.Core.Models.Admin
{
    public class Role : BaseObjectState
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int BitMask { get; set; }
        public ICollection<UserRole> UserRoles { get; set; }
        public Role ()
        {
            UserRoles = new HashSet<UserRole>();
        }

    }
}