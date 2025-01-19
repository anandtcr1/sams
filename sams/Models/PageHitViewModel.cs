using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sams.Models
{
    public class PageHitViewModel
    {
        public int PropertyId { get; set; }
        public int TotalPageHit { get; set; }
        public string AssetId { get; set; }
        public string PropertyHeader { get; set; }
        public string AssetType { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int CustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string HitHeader { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
