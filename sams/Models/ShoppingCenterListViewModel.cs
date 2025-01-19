using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sams.Models
{
    public class ShoppingCenterListViewModel
    {
        public List<StateDetails> StateList { get; set; }
        public List<ShoppingCenterViewModel> ShoppingCenterList { get; set; }
    }
}
