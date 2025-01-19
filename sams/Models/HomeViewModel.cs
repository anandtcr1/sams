using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace sams.Models
{
    public class HomeViewModel
    {
        public List<StateDetails> StateList { get; set; }
        public List<SamsLocationsViewModel> SamsLocationList { get; set; }

    }
}
