using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using sams.Models;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace sams.Controllers
{
    public class SamsNotificationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetTopNotificationList()
        {
            var notificationList = new List<NotificationModel>();
            StringBuilder notificationItems = new StringBuilder();

            int cnt = 0;
            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("GetNotifications", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                SqlDataReader readerAssetType = cmd.ExecuteReader();
                while (readerAssetType.Read())
                {
                    var notificationModl = new NotificationModel();

                    notificationModl.AssetId = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("asset_id")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("asset_id"));
                    notificationModl.PeriodMaster = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("period_master")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("period_master"));
                    notificationModl.StartDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("start_date")) ? DateTime.Now : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("start_date"));
                    notificationModl.EndDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("end_date")) ? DateTime.Now : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("end_date"));

                    notificationModl.PeriodNotes = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("period_notes")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("period_notes"));
                    notificationModl.PeriodMaster = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("period_master")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("period_master"));
                    notificationModl.AssetType = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("asset_type")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("asset_type"));

                    notificationModl.SitePropertyId = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("site_property_id")) ? 0 : readerAssetType.GetInt32(readerAssetType.GetOrdinal("site_property_id"));

                    if (notificationModl.PropertyAddress != null && notificationModl.PropertyAddress.Length > 15)
                    {
                        notificationModl.PropertyAddressShort = notificationModl.PropertyAddress.Substring(0, 15) + "..";
                    }
                    else
                    {
                        notificationModl.PropertyAddressShort = notificationModl.PropertyAddress;
                    }

                    notificationModl.AlertDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("alert_date")) ? default(DateTime?) : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("alert_date"));
                    notificationModl.NotificationEmailAddress = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("other_email_address")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("other_email_address"));

                    notificationList.Add(notificationModl);

                }

                con.Close();

            }

            /*
            foreach (NotificationModel notificationItem in notificationList)
            {
                if(notificationItem.AssetType== "c_store")
                {
                    notificationItems.Append("<a href='#' class='dropdown-item'>");
                    notificationItems.Append("<i class='fas fa-envelope mr-2'></i> 4 new messages");
                    notificationItems.Append("<span class='float-right text-muted text-sm'>3 mins</span>");
                    notificationItems.Append("</a>");
                    notificationItems.Append("<div class='dropdown-divider'></div>");
                }
                
            }
            */

            return Content(notificationList.Count.ToString());
        }

        public IActionResult GetNotificationList()
        {
            var notificationList = new NotificationListModel();

            
            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmdSurplus = new SqlCommand("SurplusNotificationList", con);
                cmdSurplus.CommandType = CommandType.StoredProcedure;
                
                con.Open();
                SqlDataReader readerAssetType = cmdSurplus.ExecuteReader();
                notificationList.SurplusNotificationList = CreateNotificationList(readerAssetType);
                con.Close();

                SqlCommand cmdNetLease = new SqlCommand("NetLeaseNotificationList", con);
                cmdNetLease.CommandType = CommandType.StoredProcedure;

                con.Open();
                SqlDataReader readerNetLease = cmdNetLease.ExecuteReader();
                notificationList.NetLeaseNotificationList = CreateNotificationList(readerNetLease);
                con.Close();

                SqlCommand cmdCStore = new SqlCommand("CStoreNotificationList", con);
                cmdCStore.CommandType = CommandType.StoredProcedure;

                con.Open();
                SqlDataReader readerCStore = cmdCStore.ExecuteReader();
                notificationList.CStoreNotificationList = CreateNotificationList(readerCStore);
                con.Close();


                SqlCommand cmdShoppingCenter = new SqlCommand("ShoppingCenterNotificationList", con);
                cmdShoppingCenter.CommandType = CommandType.StoredProcedure;

                con.Open();
                SqlDataReader readerShoppingCenter = cmdShoppingCenter.ExecuteReader();
                notificationList.ShoppingCenterNotificationList = CreateNotificationList(readerShoppingCenter);
                con.Close();

                SqlCommand cmdSurplusListing = new SqlCommand("SurplusListingExpiry", con);
                cmdSurplusListing.CommandType = CommandType.StoredProcedure;

                con.Open();
                SqlDataReader readerSurplusListing = cmdSurplusListing.ExecuteReader();
                notificationList.SurplusPropertyExpiryList = CreatePropertyNotificationList(readerSurplusListing);
                con.Close();


                SqlCommand cmdCStoreListing = new SqlCommand("CStoreListingExpiry", con);
                cmdCStoreListing.CommandType = CommandType.StoredProcedure;

                con.Open();
                SqlDataReader readerCStoreListing = cmdCStoreListing.ExecuteReader();
                notificationList.CStorePropertyExpiryList = CreatePropertyNotificationList(readerCStoreListing);
                con.Close();


                SqlCommand cmdNetLeaseListing = new SqlCommand("NetLeaseListingExpiry", con);
                cmdNetLeaseListing.CommandType = CommandType.StoredProcedure;

                con.Open();
                SqlDataReader readerNetLeaseListing = cmdNetLeaseListing.ExecuteReader();
                notificationList.NetLeasePropertyExpiryList = CreatePropertyNotificationList(readerNetLeaseListing);
                con.Close();

                SqlCommand cmdNewPeroperty = new SqlCommand("NewPropertyNotificationList", con);
                cmdNewPeroperty.CommandType = CommandType.StoredProcedure;

                con.Open();
                SqlDataReader readerNewPeroperty = cmdNewPeroperty.ExecuteReader();
                notificationList.NewPropertyNotificationList = SamsNotificationController.CreateNotificationList(readerNewPeroperty);
                con.Close();

                SqlCommand cmdScNotifications = new SqlCommand("GetShoppingCenterClientNotifications", con);
                cmdScNotifications.CommandType = CommandType.StoredProcedure;

                con.Open();

                notificationList.TenantCriticalItemList = new List<TenantCriticalDateModel>();
                SqlDataReader readerScNotifications = cmdScNotifications.ExecuteReader();
                while (readerScNotifications.Read())
                {
                    var tenantCriticalDate = new TenantCriticalDateModel();

                    tenantCriticalDate.ShoppingCenterClientId = readerScNotifications.IsDBNull(readerScNotifications.GetOrdinal("shopping_center_client_id")) ? 0 : readerScNotifications.GetInt32(readerScNotifications.GetOrdinal("shopping_center_client_id"));
                    tenantCriticalDate.ShoppingCenterId = readerScNotifications.IsDBNull(readerScNotifications.GetOrdinal("c_store_id")) ? 0 : readerScNotifications.GetInt32(readerScNotifications.GetOrdinal("c_store_id"));
                    tenantCriticalDate.UnitSelected = readerScNotifications.IsDBNull(readerScNotifications.GetOrdinal("unit_selected")) ? "" : readerScNotifications.GetString(readerScNotifications.GetOrdinal("unit_selected"));
                    tenantCriticalDate.TenantName = readerScNotifications.IsDBNull(readerScNotifications.GetOrdinal("tenant_name")) ? "" : readerScNotifications.GetString(readerScNotifications.GetOrdinal("tenant_name"));
                    tenantCriticalDate.DateRentChanged = readerScNotifications.IsDBNull(readerScNotifications.GetOrdinal("date_rent_changed")) ? DateTime.Now : readerScNotifications.GetDateTime(readerScNotifications.GetOrdinal("date_rent_changed"));
                    tenantCriticalDate.CoiExpire = readerScNotifications.IsDBNull(readerScNotifications.GetOrdinal("coi_expire")) ? DateTime.Now : readerScNotifications.GetDateTime(readerScNotifications.GetOrdinal("coi_expire"));

                    notificationList.TenantCriticalItemList.Add(tenantCriticalDate);
                }
                con.Close();
            }

            return View(notificationList);
        }

        public static List<NotificationModel> CreateNotificationList(SqlDataReader readerAssetType)
        {
            List<NotificationModel> notificationList = new List<NotificationModel>();

            while (readerAssetType.Read())
            {
                var notificationModl = new NotificationModel();

                notificationModl.PeriodId = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("period_id")) ? 0 : readerAssetType.GetInt32(readerAssetType.GetOrdinal("period_id"));
                notificationModl.AssetId = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("asset_id")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("asset_id"));
                
                notificationModl.PeriodMaster = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("period_master")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("period_master"));
                notificationModl.StartDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("start_date")) ? DateTime.Now : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("start_date"));
                notificationModl.EndDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("end_date")) ? DateTime.Now : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("end_date"));

                notificationModl.PeriodNotes = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("period_notes")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("period_notes"));
                notificationModl.PeriodMaster = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("period_master")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("period_master"));
                notificationModl.AssetType = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("asset_type")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("asset_type"));

                notificationModl.SitePropertyId = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("site_property_id")) ? 0 : readerAssetType.GetInt32(readerAssetType.GetOrdinal("site_property_id"));
                notificationModl.PropertyAddress = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("prop_address")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("prop_address"));

                if (notificationModl.PropertyAddress.Length > 15)
                {
                    notificationModl.PropertyAddressShort = notificationModl.PropertyAddress.Substring(0, 15) + "..";
                }
                else
                {
                    notificationModl.PropertyAddressShort = notificationModl.PropertyAddress;
                }

                notificationModl.AlertDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("alert_date")) ? default(DateTime?) : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("alert_date"));
                notificationModl.NotificationEmailAddress = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("other_email_address")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("other_email_address"));

                notificationList.Add(notificationModl);
            }

            return notificationList;
        }

        public static List<NotificationModel> CreatePropertyNotificationList(SqlDataReader readerAssetType)
        {
            List<NotificationModel> notificationList = new List<NotificationModel>();

            while (readerAssetType.Read())
            {
                var notificationModl = new NotificationModel();
               
                notificationModl.AssetId = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("asset_id")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("asset_id"));
                notificationModl.EndDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("listing_expiry")) ? DateTime.Now : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("listing_expiry"));
                notificationModl.SitePropertyId = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("site_details_id")) ? 0 : readerAssetType.GetInt32(readerAssetType.GetOrdinal("site_details_id"));
                notificationModl.PropertyAddress = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("address")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("address"));
                if(notificationModl.PropertyAddress.Length > 15)
                {
                    notificationModl.PropertyAddressShort = notificationModl.PropertyAddress.Substring(0, 15) + "..";
                }
                else
                {
                    notificationModl.PropertyAddressShort = notificationModl.PropertyAddress;
                }
                notificationList.Add(notificationModl);
            }

            return notificationList;
        }


        public RedirectToActionResult HideNotification(int periodId)
        {
            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("HideNotification", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                cmd.Parameters.AddWithValue("period_id", periodId);

                cmd.ExecuteNonQuery();


                con.Close();
                return RedirectToAction("GetNotificationList");
            }
        }

        public RedirectToActionResult HideNewPropertyNotification(int periodId)
        {
            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("HideNewPropertyNotification", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                cmd.Parameters.AddWithValue("period_id", periodId);

                cmd.ExecuteNonQuery();


                con.Close();
                return RedirectToAction("GetNotificationList");
            }
        }
    }
}