using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SamsSite.Models
{
    public class PropertyItem
    {
        public int PropertyItemId { get; set; }
        public string PropertyListingId { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string Pincode { get; set; }
        public int StateId { get; set; }
        public string StateName { get; set; }
        public int CityId { get; set; }
        public string CityName { get; set; }
        public int PropertyTypeId { get; set; }
        public string PropertyTypeName { get; set; }
        public int PropertyPrice { get; set; }
        public int TotalPropertyArea { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
