using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sams.Models
{
    public class RoleViewModel
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }

        public bool CanPublishListings { get; set; }

    }
}
