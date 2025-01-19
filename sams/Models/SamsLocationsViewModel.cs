using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sams.Models
{
    public class SamsLocationsViewModel
    {
        public int LocationId { get; set; }
        public string SHAssetId { get; set; }
        public string LocationAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zipcode { get; set; }
        public string County { get; set; }
        public string BusinessName { get; set; }
        
        public string Latitude { get; set; }
        public string Longitude { get; set; }
    }
}
