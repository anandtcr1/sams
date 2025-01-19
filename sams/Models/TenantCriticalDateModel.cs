using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sams.Models
{
    public class TenantCriticalDateModel
    {
        public int ShoppingCenterClientId { get; set; }
        public int ShoppingCenterId { get; set; }
        public string UnitSelected { get; set; }
        public string TenantName { get; set; }
        public DateTime? DateRentChanged { get; set; }
        public DateTime? CoiExpire { get; set; }
        public int DaysToExpire
        {
            get
            {
                return (DateRentChanged.Value - DateTime.Now.Date).Days;
            }
        }
    }
}
