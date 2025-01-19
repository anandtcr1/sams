using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace sams.Models
{
    public class SurplusRealestateViewModel
    {
        public List<StateDetails> StateList { get; set; }
        public List<SiteDetails> SurplusPropertiesList { get; set; }
        public List<RegionViewModel> RegionList { get; set; }
    }
}
