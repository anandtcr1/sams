using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using sams.Models;
using System.Web;
using Newtonsoft.Json;
using sams.Common;
using Microsoft.Office.Interop.Excel;
using Microsoft.AspNetCore.Http.Extensions;



namespace sams.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index(string propertyType)
        {
            
            Helper.HostName = Request.HttpContext.Request.Host.Value;

            UserViewModel loggedInUser = HttpContext.Session.GetObjectFromJson<UserViewModel>("LoggedInAdmin");
            if (loggedInUser != null && loggedInUser.UserId > 0)
            {
                var dashboardSummary = new DashBoardSummaryViewModel();

                dashboardSummary.LoggedInUser = loggedInUser;

                List<PropertySummaryViewModel> pDashboardList = GetPropertyTypeDashboard();
                foreach (var item in pDashboardList)
                {
                    if (item.PropertyTypeName == "surplus")
                    {
                        dashboardSummary.TotalSurplusProperties = item.TotalCount;
                    }
                    else if (item.PropertyTypeName == "net_lease")
                    {
                        dashboardSummary.TotalNetleaseProperties = item.TotalCount;
                    }
                    else if (item.PropertyTypeName == "c_store")
                    {
                        dashboardSummary.TotalCstores = item.TotalCount;
                    }
                    else if (item.PropertyTypeName == "from_web")
                    {
                        dashboardSummary.TotalFromCustomers = item.TotalCount;
                    }
                }

                dashboardSummary.SurplusInStock = GetInStockSurplusList(0);
                foreach (var sMonthlySales in dashboardSummary.SurplusInStock)
                {
                    dashboardSummary.MonthNames = dashboardSummary.MonthNames + "'" + sMonthlySales.MonthName + "',";
                    dashboardSummary.InStockSurplusData = dashboardSummary.InStockSurplusData + sMonthlySales.TotalRecords.ToString() + ",";
                }

                dashboardSummary.SurplusSold = GetInStockSurplusList(1);
                foreach (var sMonthlySales in dashboardSummary.SurplusSold)
                {
                    dashboardSummary.SoldSurplusData = dashboardSummary.SoldSurplusData + sMonthlySales.TotalRecords.ToString() + ",";
                }




                dashboardSummary.NetLeaseInStock = GetInStockNetLeaseList(0);
                foreach (var sMonthlySales in dashboardSummary.NetLeaseInStock)
                {
                    //dashboardSummary.MonthNames = dashboardSummary.MonthNames + "'" + sMonthlySales.MonthName + "',";
                    dashboardSummary.InStockNetLeaseData = dashboardSummary.InStockNetLeaseData + sMonthlySales.TotalRecords.ToString() + ",";
                }

                dashboardSummary.NetLeaseSold = GetInStockNetLeaseList(1);
                foreach (var sMonthlySales in dashboardSummary.NetLeaseSold)
                {
                    dashboardSummary.SoldNetLeaseData = dashboardSummary.SoldNetLeaseData + sMonthlySales.TotalRecords.ToString() + ",";
                }



                dashboardSummary.CStoresInStock = GetInStockNetLeaseList(0);
                foreach (var sMonthlySales in dashboardSummary.CStoresInStock)
                {
                    //dashboardSummary.MonthNames = dashboardSummary.MonthNames + "'" + sMonthlySales.MonthName + "',";
                    dashboardSummary.InStockCStoresData = dashboardSummary.InStockCStoresData + sMonthlySales.TotalRecords.ToString() + ",";
                }

                dashboardSummary.CStoresSold = GetInStockNetLeaseList(1);
                foreach (var sMonthlySales in dashboardSummary.CStoresSold)
                {
                    dashboardSummary.SoldCStoresData = dashboardSummary.SoldCStoresData + sMonthlySales.TotalRecords.ToString() + ",";
                }


                List<PropertyLocationViewModel> propertyLocationList = new List<PropertyLocationViewModel>();
                string CS = DBConnection.ConnectionString;
                using (SqlConnection con = new SqlConnection(CS))
                {
                    SqlCommand cmd = new SqlCommand("GetPropertyPosition", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("property_type", propertyType);
                    con.Open();

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        var propertyLocation = new PropertyLocationViewModel();
                        propertyLocation.PropertyId = reader.IsDBNull(reader.GetOrdinal("property_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("property_id"));
                        propertyLocation.PropertyHeader = reader.IsDBNull(reader.GetOrdinal("header")) ? "" : reader.GetString(reader.GetOrdinal("header"));
                        propertyLocation.PropertyLatitude= reader.IsDBNull(reader.GetOrdinal("property_latitude")) ? "" : reader.GetString(reader.GetOrdinal("property_latitude"));
                        propertyLocation.PropertyLongitude = reader.IsDBNull(reader.GetOrdinal("property_longitude")) ? "" : reader.GetString(reader.GetOrdinal("property_longitude"));
                        propertyLocation.PropertyType = reader.IsDBNull(reader.GetOrdinal("property_type")) ? "" : reader.GetString(reader.GetOrdinal("property_type"));

                        propertyLocation.PropertySize = reader.IsDBNull(reader.GetOrdinal("property_size")) ? "" : reader.GetString(reader.GetOrdinal("property_size"));
                        propertyLocation.PropertyPrice = reader.IsDBNull(reader.GetOrdinal("property_price")) ? "" : reader.GetString(reader.GetOrdinal("property_price"));
                        //double cpRate = reader.IsDBNull(reader.GetOrdinal("cap_rate")) ? 0 : reader.GetDouble(reader.GetOrdinal("cap_rate"));
                        
                        propertyLocation.CapRate = reader.IsDBNull(reader.GetOrdinal("cap_rate")) ? "" : reader.GetString(reader.GetOrdinal("cap_rate"));

                        if(propertyLocation.PropertyLatitude.Trim().Length > 0 && propertyLocation.PropertyLongitude.Trim().Length > 0)
                        {
                            propertyLocationList.Add(propertyLocation);
                        }
                    }
                }

                dashboardSummary.PropertyLocationList = propertyLocationList;

                return View(dashboardSummary);
            }
            else
            {
                return RedirectToAction("DoLogin");
            }
        }

        

        public IActionResult DoLogin()
        {
            var userDetails = new UserViewModel();
            return View(userDetails);
        }

        List<SiteDetails> GetListByCategory(SamsPropertyType pType)
        {
            
            List<SiteDetails> propertyList = new List<SiteDetails>();

            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("GetPropertyListByCategory", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("property_type", pType);
                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var steDetails = new SiteDetails();
                    steDetails.SiteDetailsId = reader.IsDBNull(reader.GetOrdinal("site_details_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("site_details_id"));
                    steDetails.NamePrefix = reader.IsDBNull(reader.GetOrdinal("name_prefix")) ? "" : reader.GetString(reader.GetOrdinal("name_prefix"));
                    steDetails.FirstName = reader.IsDBNull(reader.GetOrdinal("first_name")) ? "" : reader.GetString(reader.GetOrdinal("first_name"));

                    steDetails.LastName = reader.IsDBNull(reader.GetOrdinal("last_name")) ? "" : reader.GetString(reader.GetOrdinal("last_name"));
                    steDetails.CompanyName = reader.IsDBNull(reader.GetOrdinal("company_name")) ? "" : reader.GetString(reader.GetOrdinal("company_name"));
                    steDetails.EmailAddress = reader.IsDBNull(reader.GetOrdinal("email_address")) ? "" : reader.GetString(reader.GetOrdinal("email_address"));
                    steDetails.Address = reader.IsDBNull(reader.GetOrdinal("address")) ? "" : reader.GetString(reader.GetOrdinal("address"));
                    steDetails.CityName = reader.IsDBNull(reader.GetOrdinal("city_name")) ? "" : reader.GetString(reader.GetOrdinal("city_name"));
                    steDetails.StateId = reader.IsDBNull(reader.GetOrdinal("state_id")) ? "" : reader.GetString(reader.GetOrdinal("state_id"));
                    steDetails.ZipCode = reader.IsDBNull(reader.GetOrdinal("zip_code")) ? "" : reader.GetString(reader.GetOrdinal("zip_code"));
                    steDetails.ContactNumber = reader.IsDBNull(reader.GetOrdinal("contact_number")) ? "" : reader.GetString(reader.GetOrdinal("contact_number"));
                    steDetails.SamsHoldingEmployee = reader.IsDBNull(reader.GetOrdinal("sams_holding_employee")) ? false : reader.GetBoolean(reader.GetOrdinal("sams_holding_employee"));
                    steDetails.MarketId = reader.IsDBNull(reader.GetOrdinal("market_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("market_id"));
                    steDetails.SiteAddress = reader.IsDBNull(reader.GetOrdinal("site_address")) ? "" : reader.GetString(reader.GetOrdinal("site_address"));
                    steDetails.SiteCity = reader.IsDBNull(reader.GetOrdinal("site_city")) ? "" : reader.GetString(reader.GetOrdinal("site_city"));
                    steDetails.SiteStateId = reader.IsDBNull(reader.GetOrdinal("site_state_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("site_state_id"));
                    steDetails.SiteCounty = reader.IsDBNull(reader.GetOrdinal("site_county")) ? "" : reader.GetString(reader.GetOrdinal("site_county"));
                    steDetails.SiteCrossStreetName = reader.IsDBNull(reader.GetOrdinal("site_cross_street_name")) ? "" : reader.GetString(reader.GetOrdinal("site_cross_street_name"));
                    steDetails.IsPropertyAvailable = reader.IsDBNull(reader.GetOrdinal("is_property_available")) ? true : reader.GetBoolean(reader.GetOrdinal("is_property_available"));
                    steDetails.Zoning = reader.IsDBNull(reader.GetOrdinal("zoning")) ? "" : reader.GetString(reader.GetOrdinal("zoning"));
                    steDetails.LotSize = reader.IsDBNull(reader.GetOrdinal("lot_size")) ? "" : reader.GetString(reader.GetOrdinal("lot_size"));

                    steDetails.SalesPrice = reader.IsDBNull(reader.GetOrdinal("sales_price")) ? "" : reader.GetString(reader.GetOrdinal("sales_price"));
                    steDetails.Comments = reader.IsDBNull(reader.GetOrdinal("comments")) ? "" : reader.GetString(reader.GetOrdinal("comments"));

                    steDetails.CreatedDate = reader.IsDBNull(reader.GetOrdinal("created_date")) ? DateTime.Now : reader.GetDateTime(reader.GetOrdinal("created_date"));
                    steDetails.PropertyType = reader.IsDBNull(reader.GetOrdinal("property_type")) ? 0 : reader.GetInt32(reader.GetOrdinal("property_type"));

                    steDetails.ImageName = reader.IsDBNull(reader.GetOrdinal("image_name")) ? "" : reader.GetString(reader.GetOrdinal("image_name"));

                    propertyList.Add(steDetails);
                }
                con.Close();
            }

            return propertyList;
        }

        [HttpPost]
        public IActionResult CheckUser(UserViewModel userData)
        {


            UserViewModel userDetails = new UserViewModel();
            ModuleRolePermissionViewModel moduleRolePermission = new ModuleRolePermissionViewModel();

            moduleRolePermission.SamsRole = new RoleViewModel();
            moduleRolePermission.RolePermissionList = new List<RolePermissionViewModel>();

            string CS = DBConnection.ConnectionString;

            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("GetUserDetails", con);
                cmd.CommandType = CommandType.StoredProcedure;
                
                userData.Password = StringFunctions.Encrypt(userData.Password, SiteSettings.PasswordKey);

                cmd.Parameters.AddWithValue("userName", userData.UserName);
                cmd.Parameters.AddWithValue("password", userData.Password);


                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {

                    userDetails.UserId = reader.IsDBNull(reader.GetOrdinal("user_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("user_id"));
                    userDetails.FirstName = reader.IsDBNull(reader.GetOrdinal("first_name")) ? "" : reader.GetString(reader.GetOrdinal("first_name"));
                    userDetails.LastName = reader.IsDBNull(reader.GetOrdinal("last_name")) ? "" : reader.GetString(reader.GetOrdinal("last_name"));
                    userDetails.RoleId = reader.IsDBNull(reader.GetOrdinal("role_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("role_id"));
                    userDetails.UserName = userData.UserName;
                    userDetails.EmailAddress = reader.IsDBNull(reader.GetOrdinal("email_address")) ? "" : reader.GetString(reader.GetOrdinal("email_address")); 





                }
                con.Close();

                if (userDetails.RoleId > 0)
                {


                    SqlCommand cmdUserRole = new SqlCommand("GetRoleById", con);
                    cmdUserRole.Parameters.AddWithValue("role_id", userDetails.RoleId);
                    cmdUserRole.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    var roleDetails = new RoleViewModel();
                    SqlDataReader readerUserRole = cmdUserRole.ExecuteReader();
                    while (readerUserRole.Read())
                    {

                        roleDetails.RoleId = readerUserRole.IsDBNull(readerUserRole.GetOrdinal("role_id")) ? 0 : readerUserRole.GetInt32(readerUserRole.GetOrdinal("role_id"));
                        roleDetails.RoleName = readerUserRole.IsDBNull(readerUserRole.GetOrdinal("role_name")) ? "" : readerUserRole.GetString(readerUserRole.GetOrdinal("role_name"));
                        roleDetails.CanPublishListings = readerUserRole.IsDBNull(readerUserRole.GetOrdinal("can_publish_listing")) ? false : readerUserRole.GetBoolean(readerUserRole.GetOrdinal("can_publish_listing")); 
                    }
                    con.Close();
                    moduleRolePermission.SamsRole = roleDetails;


                    List<RolePermissionViewModel> rolePermissionList = new List<RolePermissionViewModel>();
                    SqlCommand cmdRolePermission = new SqlCommand("GetRolePermission", con);
                    cmdRolePermission.Parameters.AddWithValue("role_id", userDetails.RoleId);
                    cmdRolePermission.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader readerRolePermission = cmdRolePermission.ExecuteReader();
                    while (readerRolePermission.Read())
                    {
                        RolePermissionViewModel rolePermission = new RolePermissionViewModel();
                        rolePermission.ModuleId = readerRolePermission.IsDBNull(readerRolePermission.GetOrdinal("module_id")) ? 0 : readerRolePermission.GetInt32(readerRolePermission.GetOrdinal("module_id"));
                        rolePermission.ModuleName = readerRolePermission.IsDBNull(readerRolePermission.GetOrdinal("module_name")) ? "" : readerRolePermission.GetString(readerRolePermission.GetOrdinal("module_name"));
                        rolePermission.RolePermissionId = readerRolePermission.IsDBNull(readerRolePermission.GetOrdinal("role_permission_id")) ? 0 : readerRolePermission.GetInt32(readerRolePermission.GetOrdinal("role_permission_id"));
                        rolePermission.RoleId = readerRolePermission.IsDBNull(readerRolePermission.GetOrdinal("role_id")) ? 0 : readerRolePermission.GetInt32(readerRolePermission.GetOrdinal("role_id"));

                        rolePermission.CanRead = readerRolePermission.IsDBNull(readerRolePermission.GetOrdinal("can_read")) ? false : readerRolePermission.GetBoolean(readerRolePermission.GetOrdinal("can_read"));
                        rolePermission.CanEdit = readerRolePermission.IsDBNull(readerRolePermission.GetOrdinal("can_edit")) ? false : readerRolePermission.GetBoolean(readerRolePermission.GetOrdinal("can_edit"));
                        rolePermission.CanCreate = readerRolePermission.IsDBNull(readerRolePermission.GetOrdinal("can_create")) ? false : readerRolePermission.GetBoolean(readerRolePermission.GetOrdinal("can_create"));
                        rolePermission.CanDelete = readerRolePermission.IsDBNull(readerRolePermission.GetOrdinal("can_delete")) ? false : readerRolePermission.GetBoolean(readerRolePermission.GetOrdinal("can_delete"));
                        rolePermissionList.Add(rolePermission);
                    }

                    moduleRolePermission.RolePermissionList = rolePermissionList;



                    con.Close();

                }
                userDetails.RolePermission = moduleRolePermission;
                TempData["userDetails"] = JsonConvert.SerializeObject(userDetails);
            }
            if (userDetails.UserId == 0)
            {

                // 
                //ViewData["LoginStatus"] = "Wrong Username/ Password";
                ViewBag.Name = "Wrong Username/ Password";
                return View("DoLogin");
            }
            else
            {
                HttpContext.Session.SetObjectAsJson("LoggedInAdmin", userDetails);

                return RedirectToAction("Index", new { propertyType = "all" });

            }

        }

        public IActionResult LogoutUser()
        {
            var userData = new UserViewModel();
            HttpContext.Session.SetObjectAsJson("LoggedInAdmin", userData);
            return View("DoLogin");
        }

        List<PropertySummaryViewModel> GetPropertyTypeDashboard()
        {
            List<PropertySummaryViewModel> propertyList = new List<PropertySummaryViewModel>();

            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("GetPropertyDashboard", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var steDetails = new PropertySummaryViewModel();
                    steDetails.TotalCount = reader.IsDBNull(reader.GetOrdinal("totalCount")) ? 0 : reader.GetInt32(reader.GetOrdinal("totalCount"));
                    steDetails.PropertyTypeName = reader.IsDBNull(reader.GetOrdinal("pType")) ? "" : reader.GetString(reader.GetOrdinal("pType"));
                    
                    propertyList.Add(steDetails);
                }
                con.Close();
            }

            return propertyList;
        }

        List<AssetMonthlySalesViewModel> GetInStockSurplusList(int assetStatus)
        {
            List<AssetMonthlySalesViewModel> propertyList = new List<AssetMonthlySalesViewModel>();

            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("GetSurplusMonthlyData", con);
                cmd.Parameters.AddWithValue("asset_status", assetStatus);

                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var steDetails = new AssetMonthlySalesViewModel();
                    steDetails.MonthId = reader.IsDBNull(reader.GetOrdinal("month_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("month_id"));
                    steDetails.MonthName = reader.IsDBNull(reader.GetOrdinal("month_name")) ? "" : reader.GetString(reader.GetOrdinal("month_name"));
                    steDetails.SelectedYear = reader.IsDBNull(reader.GetOrdinal("createdYear")) ? 0 : reader.GetInt32(reader.GetOrdinal("createdYear"));
                    steDetails.TotalRecords = reader.IsDBNull(reader.GetOrdinal("totalRecords")) ? 0 : reader.GetInt32(reader.GetOrdinal("totalRecords"));

                    propertyList.Add(steDetails);
                }
                con.Close();
            }

            return propertyList;
        }


        List<AssetMonthlySalesViewModel> GetInStockNetLeaseList(int assetStatus)
        {
            List<AssetMonthlySalesViewModel> propertyList = new List<AssetMonthlySalesViewModel>();

            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("GetNetLeaseMonthlyData", con);
                cmd.Parameters.AddWithValue("asset_status", assetStatus);

                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var steDetails = new AssetMonthlySalesViewModel();
                    steDetails.MonthId = reader.IsDBNull(reader.GetOrdinal("month_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("month_id"));
                    steDetails.MonthName = reader.IsDBNull(reader.GetOrdinal("month_name")) ? "" : reader.GetString(reader.GetOrdinal("month_name"));
                    steDetails.SelectedYear = reader.IsDBNull(reader.GetOrdinal("createdYear")) ? 0 : reader.GetInt32(reader.GetOrdinal("createdYear"));
                    steDetails.TotalRecords = reader.IsDBNull(reader.GetOrdinal("totalRecords")) ? 0 : reader.GetInt32(reader.GetOrdinal("totalRecords"));

                    propertyList.Add(steDetails);
                }
                con.Close();
            }

            return propertyList;
        }


        List<AssetMonthlySalesViewModel> GetInStockCStoreList(int assetStatus)
        {
            List<AssetMonthlySalesViewModel> propertyList = new List<AssetMonthlySalesViewModel>();

            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("GetCStoreMonthlyData", con);
                cmd.Parameters.AddWithValue("asset_status", assetStatus);

                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var steDetails = new AssetMonthlySalesViewModel();
                    steDetails.MonthId = reader.IsDBNull(reader.GetOrdinal("month_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("month_id"));
                    steDetails.MonthName = reader.IsDBNull(reader.GetOrdinal("month_name")) ? "" : reader.GetString(reader.GetOrdinal("month_name"));
                    steDetails.SelectedYear = reader.IsDBNull(reader.GetOrdinal("createdYear")) ? 0 : reader.GetInt32(reader.GetOrdinal("createdYear"));
                    steDetails.TotalRecords = reader.IsDBNull(reader.GetOrdinal("totalRecords")) ? 0 : reader.GetInt32(reader.GetOrdinal("totalRecords"));

                    propertyList.Add(steDetails);
                }
                con.Close();
            }

            return propertyList;
        }





    }
}