using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sams.Models
{
    public class NotificationListModel
    {
        public List<NotificationModel> SurplusNotificationList { get; set; }
        public List<NotificationModel> NetLeaseNotificationList { get; set; }
        public List<NotificationModel> ShoppingCenterNotificationList { get; set; }
        public List<NotificationModel> CStoreNotificationList { get; set; }

        public List<NotificationModel> SurplusPropertyExpiryList { get; set; }
        public List<NotificationModel> NetLeasePropertyExpiryList { get; set; }
        public List<NotificationModel> CStorePropertyExpiryList { get; set; }
        public List<NotificationModel> NewPropertyNotificationList { get; set; }
        public List<TenantCriticalDateModel> TenantCriticalItemList { get; set; }

    }
}
