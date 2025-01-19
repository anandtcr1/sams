using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sams.Models
{
    public class SurplusPropertiesDashboard
    {
        public int TotalAcquisition { get; set; }

        public int TotalCreatedAcquisition { get; set; }

        public int TotalClosedAcquisition { get; set; }
        public int TotalTerminatedAcquisition { get; set; }

        public int TotalCreatedDisposition { get; set; }

        public int TotalDisposition { get; set; }
        public int TotalClosedDisposition { get; set; }
        public int TotalTerminatedDisposition { get; set; }

        public int TotalLease { get; set; }

        public int TotalProperties { get; set; }

        public List<PeriodViewModel> LatestPeriodList { get; set; }

        public List<SiteDetails> SearchedPropertyList { get; set; }
        public List<NetleasePropertiesViewModel> SearchedNetleaseList { get; set; }
        public List<CStoreViewModel> SearchedCStoreList { get; set; }
        public List<NotificationModel> SurplusNotificationList { get; set; }
        public List<NotificationModel> SurplusListingExpiryList { get; set; }

        public int TotalLoi { get; set; }
        public int TotalUnderContract { get; set; }
        public int TotalClosed { get; set; }
        public int TotalTerminated { get; set; }


    }
}
