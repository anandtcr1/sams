using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sams.Models
{
    public class FutureTenantCriticalDateModel
    {
        public int CriticalDateId { get; set; }
        public int FutureTenantId { get; set; }
        public int NetleasePropertyId { get; set; }
        
        public string CriticalDateMaster { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        
        public string CriticalDateNotes { get; set; }

        public int Duration
        {
            get
            {
                return (EndDate.Date - StartDate.Date).Days;
            }
        }
        public int AddedDuration { get; set; }
        public int DaysToExpire
        {
            get
            {
                return (EndDate.Date - DateTime.Now.Date).Days;
            }
        }
        public int IsFromNetLease { get; set; }

    }
}
