using System.Collections.Generic;
using GasPrice.Core.Common.Enums;

namespace GasPrice.Core.ViewModels.Admin
{

    public class PermissionViewModel
    {
        public int Id { get; set; }
        public ModelAction ActionId { get; set; }
        public int ModuleId { get; set; }
        public virtual IEnumerable<RolePermissionViewModel> RolePermissions { get; set; }
    }
}
