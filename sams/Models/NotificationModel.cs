using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sams.Models
{
    public class NotificationModel
    {
        public int PeriodId { get; set; }
        public string AssetId { get; set; }
        public string PeriodMaster { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string PeriodNotes { get; set; }
        public string AssetType { get; set; }
        public int SitePropertyId { get; set; }
        public string PropertyAddress { get; set; }

        public string PropertyAddressShort { get; set; }

        public int Duration
        {
            get
            {
                return (EndDate.Date - StartDate.Date).Days;
            }
        }
        public int DaysToExpire
        {
            get
            {
                return (EndDate.Date - DateTime.Now.Date).Days;
            }
        }

        public DateTime? AlertDate { get; set; }
        public string NotificationEmailAddress { get; set; }
    }
}
