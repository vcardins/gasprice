using System.Collections.Generic;

namespace GasPrice.Core.ViewModels.Admin
{
    public class RoleViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int BitMask { get; set; }
        public IEnumerable<UserRoleViewModel> UserRoles { get; set; }
        public IEnumerable<RolePermissionViewModel> RolePermissions { get; set; }        
    }
}