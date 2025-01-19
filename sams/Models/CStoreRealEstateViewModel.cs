using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sams.Models
{
    public class CStoreRealEstateViewModel
    {
        public List<StateDetails> StateList { get; set; }
        public List<CStoreViewModel> CStoreList { get; set; }
        public List<RegionViewModel> RegionList { get; set; }
    }
}
