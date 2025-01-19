using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sams.Models
{
    public class EmailNotificationViewModel
    {
        public int PeriodId { get; set; }
        public string PropertyHeader1 { get; set; }
        public string PropertyHeader2 { get; set; }
        public string AssetId { get; set; }
        public string PropertyAddress { get; set; }
        public string CriticalItemHeader { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        
        public string PeriodNotes { get; set; }
        public DateTime? AlertDate { get; set; }
        public string EmailAddress { get; set; }

        public int Duration
        {
            get
            {
                return (EndDate.Date - StartDate.Date).Days;
            }
        }

    }
}
