using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sams.Models
{
    public class ShoppingCenterDashboardModel
    {
        public List<NetleasePropertiesViewModel> NetLeasePropertiesList { get; set; }
        public List<NotificationModel> ShoppingCenterNotificationList { get; set; }
        public List<TenantCriticalDateModel> TenantCriticalItemList { get; set; }
    }
}
