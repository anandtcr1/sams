using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sams.Models
{
    public class ModuleRolePermissionViewModel
    {
        public RoleViewModel SamsRole { get; set; }
        public List<RolePermissionViewModel> RolePermissionList { get; set; }
    }
}
