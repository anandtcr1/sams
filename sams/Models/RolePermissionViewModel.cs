using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sams.Models
{
    public class RolePermissionViewModel
    {
        public int RolePermissionId { get; set; }
        public int RoleId { get; set; }
        public int RoleName { get; set; }
        public int ModuleId { get; set; }
        public string ModuleName { get; set; }
        public bool CanRead { get; set; }
        public bool CanEdit { get; set; }
        public bool CanCreate { get; set; }
        public bool CanDelete { get; set; }
    }
}
