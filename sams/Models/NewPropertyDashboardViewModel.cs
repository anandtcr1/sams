using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace sams.Models
{
    public class NewPropertyDashboardViewModel
    {
        public int TotalProperties { get; set; }
        public int TotalResearch { get; set; }
        public int TotalUnderLoi { get; set; }

        public int TotalUnderContract { get; set; }
        public int TotalClosedAcquisitions { get; set; }
        public int TotalTerminatedAcquisitions { get; set; }

        public string SelectedPropertyType { get; set; }

        public List<SiteDetails> PropertyList { get; set; }

        public List<NotificationModel> NewPropertyNotificationList { get; set; }

    }
}
