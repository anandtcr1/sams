using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sams.Models
{
    public class PropertyLocationViewModel
    {
        public int PropertyId { get; set; }
        public string PropertyHeader { get; set; }
        public string PropertyLatitude { get; set; }
        public string PropertyLongitude { get; set; }
        public string PropertyType { get; set; }

        public string PropertySize { get; set; }
        public string PropertyPrice { get; set; }
        public string CapRate { get; set; }
    }
}
