using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sams.Models
{
    public class NetleaseRealEstateViewModel
    {
        public List<StateDetails> StateList { get; set; }
        public List<NetleasePropertiesViewModel> NetLeasePropertyList { get; set; }
        public List<RegionViewModel> RegionList { get; set; }
    }
}
