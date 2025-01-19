using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sams.Models
{
    public class RegionViewModel
    {
        public int RegionId { get; set; }
        public int StateId { get; set; }
        public string StateName { get; set; }
        public string RegionName { get; set; }
        public List<StateDetails> StateList { get; set; }
    }
}
