using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using sams.Common;
using sams.Models;
using Spire.Xls;

namespace sams.Controllers
{
    public class NewPropertyDashboardController : Controller
    {
        private readonly IWebHostEnvironment webHostEnvironment;

        public NewPropertyDashboardController(IWebHostEnvironment hostEnvironment)
        {
            webHostEnvironment = hostEnvironment;
        }

        public IActionResult Index(int createdBy)
        {
            UserViewModel loggedInUser = HttpContext.Session.GetObjectFromJson<UserViewModel>("LoggedInAdmin");
            if (loggedInUser == null)
            {
                return RedirectToAction("DoLogin", "Admin");
            }
            if (loggedInUser != null && loggedInUser.UserId <= 0)
            {
                return RedirectToAction("DoLogin", "Admin");
            }

            NewPropertyDashboardViewModel newPropertyDashboard = new NewPropertyDashboardViewModel();
            List<SiteDetails> newPropertiesList = new List<SiteDetails>();

            string CS = DBConnection.ConnectionString;




            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("GetNewProertiesSummary", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    int totalR = reader.IsDBNull(reader.GetOrdinal("TotalData")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalData"));
                    string pType = reader.IsDBNull(reader.GetOrdinal("pType")) ? "" : reader.GetString(reader.GetOrdinal("pType"));

                    if (pType == "Total_Properties")
                    {
                        newPropertyDashboard.TotalProperties = totalR;
                    }
                    else if (pType == "Total_Research")
                    {
                        newPropertyDashboard.TotalResearch = totalR;
                    }
                    else if (pType == "Total_Under_Loi")
                    {
                        newPropertyDashboard.TotalUnderLoi = totalR;
                    }
                    else if (pType == "Total_Under_Contract")
                    {
                        newPropertyDashboard.TotalUnderContract = totalR;
                    }
                    else if (pType == "Total_Closed_Acquisitions")
                    {
                        newPropertyDashboard.TotalClosedAcquisitions = totalR;
                    }
                    else if (pType == "Total_Terminated_Acquisitions")
                    {
                        newPropertyDashboard.TotalTerminatedAcquisitions  = totalR;
                    }
                    
                }
                con.Close();
            }



            using (SqlConnection con = new SqlConnection(CS))
            {
                /*
                SqlCommand cmd = new SqlCommand("GetSubittedPropertyListByCategory", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("property_type", SamsPropertyType.General);
                cmd.Parameters.AddWithValue("created_by", createdBy);
                */

                SqlCommand cmd = new SqlCommand("GetInProgressPropertyListByStatus", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("new_property_status_id", 0);

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
                    
                    steDetails.SiteStateName = reader.IsDBNull(reader.GetOrdinal("site_state_name")) ? "" : reader.GetString(reader.GetOrdinal("site_state_name"));

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

                    steDetails.SelectedImageName = reader.IsDBNull(reader.GetOrdinal("image_file_name")) ? "" : reader.GetString(reader.GetOrdinal("image_file_name")); 

                    if(steDetails.SelectedImageName.Trim().Length > 0)
                    {
                        string pic = @"../../submited_files/" + steDetails.SelectedImageName;
                        steDetails.SelectedImageName = pic;
                    }
                    else
                    {
                        steDetails.SelectedImageName = "no_image.png?a=1";
                        string pic = @"../../UploadedImage/" + steDetails.SelectedImageName;

                        steDetails.SelectedImageName = pic;
                    }

                    steDetails.IsDeleted = reader.IsDBNull(reader.GetOrdinal("is_deleted")) ? 0 : reader.GetInt32(reader.GetOrdinal("is_deleted"));
                    steDetails.NewPropertyStatusName = reader.IsDBNull(reader.GetOrdinal("new_property_status_name")) ? "" : reader.GetString(reader.GetOrdinal("new_property_status_name"));
                    steDetails.AssetId = reader.IsDBNull(reader.GetOrdinal("asset_id")) ? "" : reader.GetString(reader.GetOrdinal("asset_id"));

                    newPropertiesList.Add(steDetails);

                    
                }
                con.Close();

                SqlCommand cmdNewPeroperty = new SqlCommand("NewPropertyNotificationList", con);
                cmdNewPeroperty.CommandType = CommandType.StoredProcedure;

                con.Open();
                SqlDataReader readerNewPeroperty = cmdNewPeroperty.ExecuteReader();
                newPropertyDashboard.NewPropertyNotificationList = SamsNotificationController.CreateNotificationList(readerNewPeroperty);
                con.Close();

            }

            newPropertyDashboard.PropertyList = newPropertiesList;

            return View(newPropertyDashboard);
        }


        public RedirectToActionResult HideNotification(int periodId)
        {
            UserViewModel loggedInUser = HttpContext.Session.GetObjectFromJson<UserViewModel>("LoggedInAdmin");
            if (loggedInUser == null)
            {
                return RedirectToAction("DoLogin", "Admin");
            }
            if (loggedInUser != null && loggedInUser.UserId <= 0)
            {
                return RedirectToAction("DoLogin", "Admin");
            }

            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("HideNotification", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                cmd.Parameters.AddWithValue("period_id", periodId);

                cmd.ExecuteNonQuery();


                con.Close();
                return RedirectToAction("Index");
            }
        }

        public IActionResult GetInProgressList()
        {
            UserViewModel loggedInUser = HttpContext.Session.GetObjectFromJson<UserViewModel>("LoggedInAdmin");
            if (loggedInUser == null)
            {
                return RedirectToAction("DoLogin", "Admin");
            }
            if (loggedInUser != null && loggedInUser.UserId <= 0)
            {
                return RedirectToAction("DoLogin", "Admin");
            }

            NewPropertyDashboardViewModel newPropertyDashboard = new NewPropertyDashboardViewModel();
            List<SiteDetails> newPropertiesList = new List<SiteDetails>();

            string CS = DBConnection.ConnectionString;

            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("GetNewProertiesSummary", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    int totalR = reader.IsDBNull(reader.GetOrdinal("TotalData")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalData"));
                    string pType = reader.IsDBNull(reader.GetOrdinal("pType")) ? "" : reader.GetString(reader.GetOrdinal("pType"));

                    if (pType == "Total_Properties")
                    {
                        newPropertyDashboard.TotalProperties = totalR;
                    }
                    else if (pType == "Total_Research")
                    {
                        newPropertyDashboard.TotalResearch = totalR;
                    }
                    else if (pType == "Total_Under_Loi")
                    {
                        newPropertyDashboard.TotalUnderLoi = totalR;
                    }
                    else if (pType == "Total_Under_Contract")
                    {
                        newPropertyDashboard.TotalUnderContract = totalR;
                    }
                    else if (pType == "Total_Closed_Acquisitions")
                    {
                        newPropertyDashboard.TotalClosedAcquisitions = totalR;
                    }
                    else if (pType == "Total_Terminated_Acquisitions")
                    {
                        newPropertyDashboard.TotalTerminatedAcquisitions = totalR;
                    }
                }
                con.Close();
            }

            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("GetInProgressPropertyListByCategory", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("property_type", SamsPropertyType.General);
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

                    steDetails.SiteStateName = reader.IsDBNull(reader.GetOrdinal("site_state_name")) ? "" : reader.GetString(reader.GetOrdinal("site_state_name"));

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

                    steDetails.SelectedImageName = reader.IsDBNull(reader.GetOrdinal("image_file_name")) ? "" : reader.GetString(reader.GetOrdinal("image_file_name"));

                    if (steDetails.SelectedImageName.Trim().Length > 0)
                    {
                        string pic = @"../../submited_files/" + steDetails.SelectedImageName;
                        steDetails.SelectedImageName = pic;
                    }
                    else
                    {
                        steDetails.SelectedImageName = "no_image.png?a=1";
                        string pic = @"../../UploadedImage/" + steDetails.SelectedImageName;

                        steDetails.SelectedImageName = pic;
                    }

                    steDetails.IsDeleted = reader.IsDBNull(reader.GetOrdinal("is_deleted")) ? 0 : reader.GetInt32(reader.GetOrdinal("is_deleted"));
                    steDetails.AssetId = reader.IsDBNull(reader.GetOrdinal("asset_id")) ? "" : reader.GetString(reader.GetOrdinal("asset_id"));

                    newPropertiesList.Add(steDetails);


                }
                con.Close();
            }

            newPropertyDashboard.PropertyList = newPropertiesList;
            return View(newPropertyDashboard);
        }


        public IActionResult GetClosedList()
        {
            UserViewModel loggedInUser = HttpContext.Session.GetObjectFromJson<UserViewModel>("LoggedInAdmin");
            if (loggedInUser == null)
            {
                return RedirectToAction("DoLogin", "Admin");
            }
            if (loggedInUser != null && loggedInUser.UserId <= 0)
            {
                return RedirectToAction("DoLogin", "Admin");
            }

            NewPropertyDashboardViewModel newPropertyDashboard = new NewPropertyDashboardViewModel();

            List<SiteDetails> newPropertiesList = new List<SiteDetails>();

            string CS = DBConnection.ConnectionString;

            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("GetNewProertiesSummary", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    int totalR = reader.IsDBNull(reader.GetOrdinal("TotalData")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalData"));
                    string pType = reader.IsDBNull(reader.GetOrdinal("pType")) ? "" : reader.GetString(reader.GetOrdinal("pType"));

                    if (pType == "Total_Properties")
                    {
                        newPropertyDashboard.TotalProperties = totalR;
                    }
                    else if (pType == "Total_Research")
                    {
                        newPropertyDashboard.TotalResearch = totalR;
                    }
                    else if (pType == "Total_Under_Loi")
                    {
                        newPropertyDashboard.TotalUnderLoi = totalR;
                    }
                    else if (pType == "Total_Under_Contract")
                    {
                        newPropertyDashboard.TotalUnderContract = totalR;
                    }
                    else if (pType == "Total_Closed_Acquisitions")
                    {
                        newPropertyDashboard.TotalClosedAcquisitions = totalR;
                    }
                    else if (pType == "Total_Terminated_Acquisitions")
                    {
                        newPropertyDashboard.TotalTerminatedAcquisitions = totalR;
                    }
                }
                con.Close();
            }

            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("GetClosedPropertyListByCategory", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("property_type", SamsPropertyType.General);
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

                    steDetails.SiteStateName = reader.IsDBNull(reader.GetOrdinal("site_state_name")) ? "" : reader.GetString(reader.GetOrdinal("site_state_name"));

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

                    steDetails.SelectedImageName = reader.IsDBNull(reader.GetOrdinal("image_file_name")) ? "" : reader.GetString(reader.GetOrdinal("image_file_name"));

                    if (steDetails.SelectedImageName.Trim().Length > 0)
                    {
                        string pic = @"../../submited_files/" + steDetails.SelectedImageName;
                        steDetails.SelectedImageName = pic;
                    }
                    else
                    {
                        steDetails.SelectedImageName = "no_image.png?a=1";
                        string pic = @"../../UploadedImage/" + steDetails.SelectedImageName;

                        steDetails.SelectedImageName = pic;
                    }

                    steDetails.IsDeleted = reader.IsDBNull(reader.GetOrdinal("is_deleted")) ? 0 : reader.GetInt32(reader.GetOrdinal("is_deleted"));
                    steDetails.StatusChangedDate = reader.IsDBNull(reader.GetOrdinal("closed_date")) ? DateTime.Now : reader.GetDateTime(reader.GetOrdinal("closed_date"));

                    steDetails.AssetId = reader.IsDBNull(reader.GetOrdinal("asset_id")) ? "" : reader.GetString(reader.GetOrdinal("asset_id"));

                    newPropertiesList.Add(steDetails);


                }
                con.Close();
            }

            newPropertyDashboard.PropertyList = newPropertiesList;
            return View(newPropertyDashboard);
        }



        public IActionResult ViewNewProperty(int propertyId)
        {
            UserViewModel loggedInUser = HttpContext.Session.GetObjectFromJson<UserViewModel>("LoggedInAdmin");
            if (loggedInUser == null)
            {
                return RedirectToAction("DoLogin", "Admin");
            }
            if (loggedInUser != null && loggedInUser.UserId <= 0)
            {
                return RedirectToAction("DoLogin", "Admin");
            }

            var steDetails = new SiteDetails();
            List<AssetTypeViewModel> assetTypeList = new List<AssetTypeViewModel>();
            List<NewPropertyStatusModel> newPropertyStatusList = new List<NewPropertyStatusModel>();

            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {


                SqlCommand cmdAssetType = new SqlCommand("GetAssetType", con);
                cmdAssetType.Parameters.AddWithValue("property_type", 3);
                cmdAssetType.CommandType = CommandType.StoredProcedure;
                con.Open();

                SqlDataReader readerAssetType = cmdAssetType.ExecuteReader();
                while (readerAssetType.Read())
                {
                    var assetType = new AssetTypeViewModel();
                    assetType.AssetTypeId = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("asset_type_id")) ? 0 : readerAssetType.GetInt32(readerAssetType.GetOrdinal("asset_type_id"));
                    assetType.AssetTypeName = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("asset_type_name")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("asset_type_name"));

                    assetTypeList.Add(assetType);
                }
                con.Close();

                


                SqlCommand cmd = new SqlCommand("GetSubittedPropertyListById", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("propertyId", propertyId);
                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {

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

                    steDetails.SiteStateName = reader.IsDBNull(reader.GetOrdinal("site_state_name")) ? "" : reader.GetString(reader.GetOrdinal("site_state_name"));

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

                    steDetails.SelectedImageName = reader.IsDBNull(reader.GetOrdinal("image_file_name")) ? "" : reader.GetString(reader.GetOrdinal("image_file_name"));
                    steDetails.SelectedPdfName = reader.IsDBNull(reader.GetOrdinal("pdf_file_name")) ? "" : reader.GetString(reader.GetOrdinal("pdf_file_name"));

                    steDetails.Term = reader.IsDBNull(reader.GetOrdinal("term")) ? "" : reader.GetString(reader.GetOrdinal("term"));
                    steDetails.AskingRent = reader.IsDBNull(reader.GetOrdinal("asking_rent")) ? "" : reader.GetString(reader.GetOrdinal("asking_rent"));
                    steDetails.LeaseType = reader.IsDBNull(reader.GetOrdinal("lease_type")) ? 0 : reader.GetInt32(reader.GetOrdinal("lease_type"));
                    steDetails.AssetTypeId = reader.IsDBNull(reader.GetOrdinal("asset_type_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("asset_type_id"));

                    steDetails.DiligenceType = (int)SamAssetType.PurchaseLeaseBack;
                    //steDetails.DiligenceType = reader.IsDBNull(reader.GetOrdinal("diligence_type")) ? 0 : reader.GetInt32(reader.GetOrdinal("diligence_type"));

                    if (steDetails.SelectedImageName.Trim().Length > 0)
                    {
                        string pic = @"../../submited_files/" + steDetails.SelectedImageName;
                        steDetails.SelectedImageName = pic;
                    }
                    else
                    {
                        steDetails.SelectedImageName = "no_image.png?a=1";
                        string pic = @"../../UploadedImage/" + steDetails.SelectedImageName;

                        steDetails.SelectedImageName = pic;
                    }

                    if(steDetails.SelectedPdfName.Trim().Length > 0)
                    {
                        string pic = @"../../submited_files/" + steDetails.SelectedPdfName;
                        steDetails.SelectedPdfName = pic;
                    }

                    steDetails.IsDeleted = reader.IsDBNull(reader.GetOrdinal("is_deleted")) ? 0 : reader.GetInt32(reader.GetOrdinal("is_deleted"));

                    steDetails.DiligenceType = reader.IsDBNull(reader.GetOrdinal("diligence_type")) ? 0 : reader.GetInt32(reader.GetOrdinal("diligence_type"));

                    steDetails.AcquisitionPeriodList = GetPeriodList(propertyId, "Acquisition");
                    steDetails.DispositionPeriodList = GetPeriodList(propertyId, "Disposition");
                    steDetails.LeasePeriodList = GetPeriodList(propertyId, "Lease");
                    steDetails.LeasePurchasePeriodList = GetPeriodList(propertyId, "LeaseWithPurchase");
                    steDetails.PurchaseLeaseBackPeriodList = GetPeriodList(propertyId, "PurchaseLeaseBack");
                    steDetails.LeaseWithPurchasePeriodList = GetPeriodList(propertyId, "LeaseWithPurchaseOption");
                    

                    steDetails.DiligenceAcquisitions = GetDiligenceAcquisition(propertyId);
                    steDetails.DiligenceDispositions = null;// GetDiligenceDispositions(propertyId);
                    steDetails.DiligenceLease = GetDiligenceLease(propertyId);
                    steDetails.DiligenceLeaseWithPurchase = GetDiligenceLeaseWithPurchase(propertyId);
                    steDetails.DiligenceDispositions_PurchaseLeaseBack = GetDiligenceDispositions_SaleLeaseBack(propertyId);
                    

                    steDetails.PotentialUse = reader.IsDBNull(reader.GetOrdinal("potential_use")) ? "" : reader.GetString(reader.GetOrdinal("potential_use"));

                    steDetails.CheckIfClientRepresentedByABroker = reader.IsDBNull(reader.GetOrdinal("client_represented_by_broker")) ? 0 : reader.GetInt32(reader.GetOrdinal("client_represented_by_broker"));
                    steDetails.BrokerOrFirmName = reader.IsDBNull(reader.GetOrdinal("broker_firm_name")) ? "" : reader.GetString(reader.GetOrdinal("broker_firm_name"));
                    steDetails.BrokerEmailAddress = reader.IsDBNull(reader.GetOrdinal("broker_email_address")) ? "" : reader.GetString(reader.GetOrdinal("broker_email_address"));
                    steDetails.BrokerContactNumber = reader.IsDBNull(reader.GetOrdinal("broker_contact_number")) ? "" : reader.GetString(reader.GetOrdinal("broker_contact_number"));

                    
                    steDetails.Term = reader.IsDBNull(reader.GetOrdinal("term")) ? "" : reader.GetString(reader.GetOrdinal("term"));
                    steDetails.AskingRent = reader.IsDBNull(reader.GetOrdinal("asking_rent")) ? "" : reader.GetString(reader.GetOrdinal("asking_rent"));
                    steDetails.LeaseType = reader.IsDBNull(reader.GetOrdinal("lease_type")) ? 0 : reader.GetInt32(reader.GetOrdinal("lease_type"));
                    steDetails.AssetTypeId = reader.IsDBNull(reader.GetOrdinal("asset_type_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("asset_type_id"));
                    steDetails.NewPropertyStatusId = reader.IsDBNull(reader.GetOrdinal("new_property_status_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("new_property_status_id"));
                    steDetails.NewPropertyStatusName = reader.IsDBNull(reader.GetOrdinal("new_property_status_name")) ? "" : reader.GetString(reader.GetOrdinal("new_property_status_name"));
                    steDetails.StatusChangedDate = reader.IsDBNull(reader.GetOrdinal("status_changed_date")) ? default(DateTime?) : reader.GetDateTime(reader.GetOrdinal("status_changed_date"));

                    steDetails.AssetId = reader.IsDBNull(reader.GetOrdinal("asset_id")) ? "" : reader.GetString(reader.GetOrdinal("asset_id"));

                    steDetails.PurchaseLeasebackTerm = reader.IsDBNull(reader.GetOrdinal("purchase_leaseback_term")) ? "" : reader.GetString(reader.GetOrdinal("purchase_leaseback_term"));
                    steDetails.PurchaseLeasebackRent = reader.IsDBNull(reader.GetOrdinal("purchase_leaseback_rent")) ? "" : reader.GetString(reader.GetOrdinal("purchase_leaseback_rent"));
                    steDetails.PurchaseLeasebackLeaseTypeId = reader.IsDBNull(reader.GetOrdinal("purchase_leaseback_lease_type_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("purchase_leaseback_lease_type_id"));

                    steDetails.PurchasePrice = reader.IsDBNull(reader.GetOrdinal("purchase_price")) ? "" : reader.GetString(reader.GetOrdinal("purchase_price"));
                    steDetails.PurchaseLeasebackPotentialUse = reader.IsDBNull(reader.GetOrdinal("purchase_leaseback_potential_use")) ? "" : reader.GetString(reader.GetOrdinal("purchase_leaseback_potential_use"));
                    steDetails.PurchaseLeasebackComments = reader.IsDBNull(reader.GetOrdinal("purchase_leaseback_comments")) ? "" : reader.GetString(reader.GetOrdinal("purchase_leaseback_comments"));
                    steDetails.AskingPrice = reader.IsDBNull(reader.GetOrdinal("asking_price")) ? "" : reader.GetString(reader.GetOrdinal("asking_price"));
                    steDetails.FeePotentialUse = reader.IsDBNull(reader.GetOrdinal("fee_potential_use")) ? "" : reader.GetString(reader.GetOrdinal("fee_potential_use"));
                    steDetails.FeeComments = reader.IsDBNull(reader.GetOrdinal("fee_comments")) ? "" : reader.GetString(reader.GetOrdinal("fee_comments"));
                    steDetails.TermOptionPurchase = reader.IsDBNull(reader.GetOrdinal("term_option_purchase")) ? "" : reader.GetString(reader.GetOrdinal("term_option_purchase"));
                    steDetails.AskingRentOptionPurchase = reader.IsDBNull(reader.GetOrdinal("asking_rent_option_purchase")) ? "" : reader.GetString(reader.GetOrdinal("asking_rent_option_purchase"));
                    steDetails.LeaseTypePurchase = reader.IsDBNull(reader.GetOrdinal("lease_type_purchase")) ? 0 : reader.GetInt32(reader.GetOrdinal("lease_type_purchase"));
                    steDetails.OptionPurchase = reader.IsDBNull(reader.GetOrdinal("option_purchase")) ? "" : reader.GetString(reader.GetOrdinal("option_purchase"));
                    steDetails.PotentialUseOptionPurchase = reader.IsDBNull(reader.GetOrdinal("potential_use_option_purchase")) ? "" : reader.GetString(reader.GetOrdinal("potential_use_option_purchase"));
                    steDetails.CommentsOptionPurchase = reader.IsDBNull(reader.GetOrdinal("comments_option_purchase")) ? "" : reader.GetString(reader.GetOrdinal("comments_option_purchase"));



                }
                con.Close();

                steDetails.PropertyImageList = new List<ImageViewModel>();
                SqlCommand cmdImageList = new SqlCommand("GetPropertyImageList", con);

                cmdImageList.Parameters.AddWithValue("property_id", propertyId);
                cmdImageList.Parameters.AddWithValue("property_type", SamsPropertyType.NewPropertyDashboard);

                cmdImageList.CommandType = CommandType.StoredProcedure;
                con.Open();

                SqlDataReader readerMarket = cmdImageList.ExecuteReader();
                List<ImageViewModel> propertyImageList = new List<ImageViewModel>();
                while (readerMarket.Read())
                {
                    var imageItem = new ImageViewModel();
                    imageItem.ImageId = readerMarket.IsDBNull(readerMarket.GetOrdinal("image_id")) ? 0 : readerMarket.GetInt32(readerMarket.GetOrdinal("image_id"));
                    imageItem.PropertyId = propertyId;



                    imageItem.ImageName = readerMarket.IsDBNull(readerMarket.GetOrdinal("image_name")) ? "" : readerMarket.GetString(readerMarket.GetOrdinal("image_name"));
                    string pic = @"../../UploadedImage/" + imageItem.ImageName;
                    imageItem.ImageName = pic;
                    propertyImageList.Add(imageItem);
                }
                steDetails.PropertyImageList = propertyImageList;
                con.Close();


                List<AdditionalFilesViewModel> additionalFiles = new List<AdditionalFilesViewModel>();
                SqlCommand cmdComplianceList = new SqlCommand("GetNewPropertyFiles", con);

                cmdComplianceList.Parameters.AddWithValue("property_id", propertyId);
                cmdComplianceList.CommandType = CommandType.StoredProcedure;
                con.Open();

                SqlDataReader readerComplianceList = cmdComplianceList.ExecuteReader();

                while (readerComplianceList.Read())
                {
                    var c_storeFile = new AdditionalFilesViewModel();
                    c_storeFile.FileId = readerComplianceList.IsDBNull(readerComplianceList.GetOrdinal("file_id")) ? 0 : readerComplianceList.GetInt32(readerComplianceList.GetOrdinal("file_id"));
                    c_storeFile.PropertyId = propertyId;
                    c_storeFile.FileType = readerComplianceList.IsDBNull(readerComplianceList.GetOrdinal("file_type")) ? "" : readerComplianceList.GetString(readerComplianceList.GetOrdinal("file_type"));


                    c_storeFile.FileName = readerComplianceList.IsDBNull(readerComplianceList.GetOrdinal("file_name")) ? "" : readerComplianceList.GetString(readerComplianceList.GetOrdinal("file_name"));

                    c_storeFile.FileNameWithoutPath = c_storeFile.FileName.Length < 35 ? c_storeFile.FileName : c_storeFile.FileName.Substring(0, 34) + "...";

                    string pic = @"../../property_files/" + c_storeFile.FileName;
                    c_storeFile.FileName = pic;
                    additionalFiles.Add(c_storeFile);
                }
                con.Close();
                steDetails.AdditionalFiles = additionalFiles;


                steDetails.AssetTypeList = assetTypeList;

                steDetails.DiligenceAcquisitions.TransactionStatusList = GetTransactionStatusList(steDetails.DiligenceAcquisitions.AcquisitionStatus, 0);
                //steDetails.DiligenceLease.TransactionStatusList = GetTransactionStatusList(steDetails.DiligenceLease.SelectedTransactionStatusId, 0);

                steDetails.NewPropertyStatusList = newPropertyStatusList;
                steDetails.LeaseTypeList = GetLeaseTypeList();

                steDetails.DiligenceDispositions_PurchaseLeaseBack = GetDiligenceDispositions_SaleLeaseBack(propertyId);
                steDetails.DiligenceDispositions_PurchaseLeaseBack.TransactionStatusList = GetTransactionStatusList(0, steDetails.TransactionStatusId);

                
                return View(steDetails);
            }
        }


        /*
        public IActionResult DeleteList(int propertyId)
        {

            // deleteNewProperty
            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("UpdateNewPropertyStatus", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("propertyId", propertyId);
                cmd.Parameters.AddWithValue("status", 1);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }


            return RedirectToAction("Index", "NewPropertyDashboard");
        }
        */


        public IActionResult MarkAsClosed(int propertyId)
        {
            UserViewModel loggedInUser = HttpContext.Session.GetObjectFromJson<UserViewModel>("LoggedInAdmin");
            if (loggedInUser == null)
            {
                return RedirectToAction("DoLogin", "Admin");
            }
            if (loggedInUser != null && loggedInUser.UserId <= 0)
            {
                return RedirectToAction("DoLogin", "Admin");
            }

            // deleteNewProperty
            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("UpdateNewPropertyStatus", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("propertyId", propertyId);
                cmd.Parameters.AddWithValue("status", 1);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }


            return RedirectToAction("Index", "NewPropertyDashboard");
        }

        public IActionResult DeleteList(int propertyId)
        {
            UserViewModel loggedInUser = HttpContext.Session.GetObjectFromJson<UserViewModel>("LoggedInAdmin");
            if (loggedInUser == null)
            {
                return RedirectToAction("DoLogin", "Admin");
            }
            if (loggedInUser != null && loggedInUser.UserId <= 0)
            {
                return RedirectToAction("DoLogin", "Admin");
            }

            // deleteNewProperty
            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("DeleteNewProperty", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("propertyId", propertyId);
                
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }


            return RedirectToAction("Index", "NewPropertyDashboard");
        }


        public IActionResult MarkInProgressList(int propertyId)
        {
            UserViewModel loggedInUser = HttpContext.Session.GetObjectFromJson<UserViewModel>("LoggedInAdmin");
            if (loggedInUser == null)
            {
                return RedirectToAction("DoLogin", "Admin");
            }
            if (loggedInUser != null && loggedInUser.UserId <= 0)
            {
                return RedirectToAction("DoLogin", "Admin");
            }

            // deleteNewProperty
            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("UpdateNewPropertyStatus", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("propertyId", propertyId);
                cmd.Parameters.AddWithValue("status", 2);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }


            return RedirectToAction("ViewNewProperty", new { propertyId = propertyId });
        }

        

        public IActionResult ExportExcel()
        {
            UserViewModel loggedInUser = HttpContext.Session.GetObjectFromJson<UserViewModel>("LoggedInAdmin");
            if (loggedInUser == null)
            {
                return RedirectToAction("DoLogin", "Admin");
            }
            if (loggedInUser != null && loggedInUser.UserId <= 0)
            {
                return RedirectToAction("DoLogin", "Admin");
            }

            var filePath = Path.Combine(webHostEnvironment.WebRootPath + @"/templates", "new_properties_template.xlsx");

            string fullFileName = Path.Combine(webHostEnvironment.WebRootPath + @"/templates", "new_properties_template.xlsx");
            string fullToFileName = "new_properties" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xlsx";

            Workbook wrkBook = new Workbook();
            wrkBook.LoadFromFile(fullFileName);
            Worksheet sheet = wrkBook.Worksheets[0];


            NewPropertyDashboardViewModel newPropertyDashboard = new NewPropertyDashboardViewModel();
            List<SiteDetails> newPropertiesList = new List<SiteDetails>();

            string CS = DBConnection.ConnectionString;




            



            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("ExportSubittedPropertyListByCategory", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("property_type", SamsPropertyType.General);
                con.Open();


                int i = 5;
                int cnt = 1;

                string colHeader = "A", colAddress = "B", colCity = "C", colState = "D", colZipCode = "E", colZoning = "F", colLotsize = "G";
                string colStatus = "H", colAskingPrice = "I", colAskingRent = "J", colOptionPrice = "K", colUnderContractDate = "L";
                string colPurchasePrice = "M", colRentDdp = "N", colClosingDate = "O", colDaysToClose = "P";

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    string cellHeader = colHeader + i.ToString();
                    string cellAddress = colAddress + i.ToString();
                    string cellCity = colCity + i.ToString();
                    string cellState = colState + i.ToString();
                    string cellZipCode = colZipCode + i.ToString();
                    string cellZoning = colZoning + i.ToString();

                    string cellLotsize = colLotsize + i.ToString();
                    string cellStatus = colStatus + i.ToString();

                    string cellAskingPrice = colAskingPrice + i.ToString();
                    string cellAskingRent = colAskingRent + i.ToString();

                    string cellOptionPrice = colOptionPrice + i.ToString();

                    string cellUnderContractDate = colUnderContractDate + i.ToString();
                    string cellPurchasePrice = colPurchasePrice + i.ToString();
                    string cellRentDdp = colRentDdp + i.ToString();

                    string cellClosingDate = colClosingDate + i.ToString();
                    string cellDaysToClose = colDaysToClose + i.ToString();

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

                    steDetails.SiteStateName = reader.IsDBNull(reader.GetOrdinal("site_state_name")) ? "" : reader.GetString(reader.GetOrdinal("site_state_name"));

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
                    steDetails.AssetTypeId = reader.IsDBNull(reader.GetOrdinal("asset_type_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("asset_type_id"));
                    
                    steDetails.MaxPriorityTransactionStatusName = reader.IsDBNull(reader.GetOrdinal("transaction_status_name")) ? "" : reader.GetString(reader.GetOrdinal("transaction_status_name"));
                    
                    steDetails.SelectedImageName = reader.IsDBNull(reader.GetOrdinal("image_file_name")) ? "" : reader.GetString(reader.GetOrdinal("image_file_name"));

                    if (steDetails.SelectedImageName.Trim().Length > 0)
                    {
                        string pic = @"../../submited_files/" + steDetails.SelectedImageName;
                        steDetails.SelectedImageName = pic;
                    }
                    else
                    {
                        steDetails.SelectedImageName = "no_image.png?a=1";
                        string pic = @"../../UploadedImage/" + steDetails.SelectedImageName;

                        steDetails.SelectedImageName = pic;
                    }

                    steDetails.IsDeleted = reader.IsDBNull(reader.GetOrdinal("is_deleted")) ? 0 : reader.GetInt32(reader.GetOrdinal("is_deleted"));
                    string statusName = "";
                    if (steDetails.IsDeleted == 0)
                    {
                        statusName = "New Property";
                    }
                    else if (steDetails.IsDeleted == 1)
                    {
                        statusName = "Closed";
                    }
                    else if (steDetails.IsDeleted == 2)
                    {
                        statusName = "In-Progress";
                    }




                    sheet.Range[cellHeader].Value = cnt.ToString();
                    sheet.Range[cellAddress].Value = steDetails.SiteAddress;
                    sheet.Range[cellCity].Value = steDetails.SiteCity;
                    sheet.Range[cellState].Value = steDetails.SiteStateName;

                    sheet.Range[cellZipCode].Value = steDetails.ZipCode;
                    sheet.Range[cellZoning].Value = steDetails.Zoning;
                    sheet.Range[cellLotsize].Value = steDetails.LotSize;

                    sheet.Range[cellStatus].Value = steDetails.MaxPriorityTransactionStatusName;

                    sheet.Range[cellAskingPrice].Value = steDetails.SalesPrice;
                    sheet.Range[cellAskingRent].Value = steDetails.AskingRent;




                    steDetails.DiligenceAcquisitions = new DiligenceAcquisitionViewModel();
                    steDetails.DiligenceDispositions = null;// GetDiligenceDispositions(propertyId);
                    steDetails.DiligenceLease = new DiligenceLeaseViewModel();

                    steDetails.DiligenceLeaseWithPurchase = new DiligenceLeaseWithPurchaseViewModel();
                    steDetails.DiligenceDispositions_PurchaseLeaseBack = new DiligenceDispositionsViewModel();

                    if (steDetails.AssetTypeId == (int)SamAssetType.Fee)
                    {
                        steDetails.DiligenceAcquisitions = GetDiligenceAcquisition(steDetails.SiteDetailsId);
                        sheet.Range[cellUnderContractDate].Value = steDetails.DiligenceAcquisitions.UnderContractDate == default(DateTime?) ? "" : steDetails.DiligenceAcquisitions.UnderContractDate.Value.ToString("MM/dd/yyyy");
                        sheet.Range[cellPurchasePrice].Value = steDetails.DiligenceAcquisitions.PurchasePrice;
                        sheet.Range[cellRentDdp].Value = steDetails.DiligenceAcquisitions.DueDiligenceExpairyDate == default(DateTime?) ? "" : steDetails.DiligenceAcquisitions.DueDiligenceExpairyDate.Value.ToString("MM/dd/yyyy");

                        var dtClosedDate = "";
                        int? daysToClose = null;
                        if (steDetails.DiligenceAcquisitions.ClosingDate != default(DateTime?))
                        {
                            
                            if (steDetails.DiligenceAcquisitions.ClosingDate.Value.Year > 1)
                            {
                                dtClosedDate = steDetails.DiligenceAcquisitions.ClosingDate.Value.ToString("MM/dd/yyyy");
                                daysToClose = (steDetails.DiligenceAcquisitions.ClosingDate.Value - DateTime.Now).Days;
                                if (daysToClose < 0)
                                {
                                    daysToClose = 0;
                                }
                            }
                        }
                        sheet.Range[cellClosingDate].Value = dtClosedDate;
                        if (daysToClose != null)
                        {
                            sheet.Range[cellDaysToClose].Value = daysToClose.ToString();
                        }
                        
                    }
                    else if (steDetails.AssetTypeId == (int)SamAssetType.Lease)
                    {
                        steDetails.DiligenceLease = GetDiligenceLease(steDetails.SiteDetailsId);

                        sheet.Range[cellUnderContractDate].Value = steDetails.DiligenceLease.UnderContractDate == default(DateTime?) ? "" : steDetails.DiligenceLease.UnderContractDate.Value.ToString("MM/dd/yyyy");
                        sheet.Range[cellPurchasePrice].Value = steDetails.DiligenceLease.ListingPrice;
                        //sheet.Range[cellListingPrice].Value = steDetails.DiligenceLease.ListingPrice;

                        var dtClosedDate = "";
                        int? daysToClose = null;
                        if (steDetails.DiligenceLease.ClosingDate != default(DateTime?))
                        {
                            
                            if (steDetails.DiligenceLease.ClosingDate.Value.Year > 1)
                            {
                                dtClosedDate = steDetails.DiligenceLease.ClosingDate.Value.ToString("MM/dd/yyyy");
                                daysToClose = (steDetails.DiligenceLease.ClosingDate.Value - DateTime.Now).Days;
                                if (daysToClose < 0)
                                {
                                    daysToClose = 0;
                                }
                            }
                        }
                        sheet.Range[cellAskingRent].Value = steDetails.DiligenceLease.Rent;
                        sheet.Range[cellRentDdp].Value = steDetails.DiligenceLease.DueDiligenceExpiryDate == default(DateTime?) ? "" : steDetails.DiligenceLease.DueDiligenceExpiryDate.Value.ToString("MM/dd/yyyy"); 


                        sheet.Range[cellClosingDate].Value = dtClosedDate;
                        if (daysToClose != null)
                        {
                            sheet.Range[cellDaysToClose].Value = daysToClose.ToString();
                        }
                    }
                    else if (steDetails.AssetTypeId == (int)SamAssetType.LeaseWithPurchaseOption)
                    {
                        steDetails.DiligenceLeaseWithPurchase = GetDiligenceLeaseWithPurchase(steDetails.SiteDetailsId);

                        sheet.Range[cellUnderContractDate].Value = steDetails.DiligenceLeaseWithPurchase.UnderContractDate == default(DateTime?) ? "" : steDetails.DiligenceLeaseWithPurchase.UnderContractDate.Value.ToString("MM/dd/yyyy");
                        sheet.Range[cellPurchasePrice].Value = steDetails.DiligenceLeaseWithPurchase.OptionPrice;
                        sheet.Range[cellOptionPrice].Value = steDetails.DiligenceLeaseWithPurchase.OptionPrice;
                        
                        var dtClosedDate = "";
                        int? daysToClose = null;
                        if (steDetails.DiligenceLeaseWithPurchase.ClosingDate != default(DateTime?))
                        {
                            if (steDetails.DiligenceLeaseWithPurchase.ClosingDate.Value.Year > 1)
                            {
                                dtClosedDate = steDetails.DiligenceLeaseWithPurchase.ClosingDate.Value.ToString("MM/dd/yyyy");
                                daysToClose = (steDetails.DiligenceLeaseWithPurchase.ClosingDate.Value - DateTime.Now).Days;
                                if (daysToClose < 0)
                                {
                                    daysToClose = 0;
                                }
                            }
                            
                        }
                        sheet.Range[cellClosingDate].Value = dtClosedDate;
                        if (daysToClose != null)
                        {
                            sheet.Range[cellDaysToClose].Value = daysToClose.ToString();
                        }
                        sheet.Range[cellOptionPrice].Value = steDetails.DiligenceLeaseWithPurchase.OptionPrice;

                    }
                    else if (steDetails.AssetTypeId == (int)SamAssetType.PurchaseLeaseBack)
                    {
                        steDetails.DiligenceDispositions_PurchaseLeaseBack = GetDiligenceDispositions_SaleLeaseBack(steDetails.SiteDetailsId);
                        sheet.Range[cellUnderContractDate].Value = steDetails.DiligenceDispositions_PurchaseLeaseBack.UnderContractDate == default(DateTime?) ? "" : steDetails.DiligenceDispositions_PurchaseLeaseBack.UnderContractDate.Value.ToString("MM/dd/yyyy");
                        sheet.Range[cellPurchasePrice].Value = steDetails.DiligenceDispositions_PurchaseLeaseBack.SalePrice;
                        sheet.Range[cellRentDdp].Value = steDetails.DiligenceDispositions_PurchaseLeaseBack.DueDiligenceExpairyDate == default(DateTime?) ? "" : steDetails.DiligenceDispositions_PurchaseLeaseBack.DueDiligenceExpairyDate.Value.ToString("MM/dd/yyyy");

                        var dtClosedDate = "";
                        int? daysToClose = null;
                        
                        if (steDetails.DiligenceDispositions_PurchaseLeaseBack.ClosingDate != default(DateTime?))
                        {
                            if (steDetails.DiligenceDispositions_PurchaseLeaseBack.ClosingDate.Value.Year > 1)
                            {
                                dtClosedDate = steDetails.DiligenceDispositions_PurchaseLeaseBack.ClosingDate.Value.ToString("MM/dd/yyyy");
                                daysToClose = (steDetails.DiligenceDispositions_PurchaseLeaseBack.ClosingDate.Value - DateTime.Now).Days;
                                if (daysToClose < 0)
                                {
                                    daysToClose = 0;
                                }
                            }
                            
                        }
                        sheet.Range[cellClosingDate].Value = dtClosedDate;
                        if (daysToClose != null)
                        {
                            sheet.Range[cellDaysToClose].Value = daysToClose.ToString();
                        }
                    }

                    sheet.Range[cellUnderContractDate].NumberFormat = "mm-dd-yyyy;@";
                    sheet.Range[cellRentDdp].NumberFormat = "mm-dd-yyyy;@";
                    sheet.Range[cellClosingDate].NumberFormat = "mm-dd-yyyy;@";

                    cnt++;
                    i++;

                    sheet.Range["A5:Q" + i.ToString()].BorderInside(LineStyleType.Thin, Color.Black);
                    sheet.Range["A5:Q" + i.ToString()].BorderAround(LineStyleType.Thin, Color.Black);

                }
                con.Close();
                sheet.Range["A5:P" + i.ToString()].BorderInside(LineStyleType.Thin, Color.Black);
            }

            wrkBook.SaveToFile(fullToFileName);

            byte[] fileBytes = GetFile(fullToFileName);
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fullToFileName);
        }

        byte[] GetFile(string s)
        {
            System.IO.FileStream fs = System.IO.File.OpenRead(s);
            byte[] data = new byte[fs.Length];
            int br = fs.Read(data, 0, data.Length);
            if (br != fs.Length)
                throw new System.IO.IOException(s);
            return data;
        }


        public IActionResult OpenForEditProperty(int propertyId)
        {
            UserViewModel loggedInUser = HttpContext.Session.GetObjectFromJson<UserViewModel>("LoggedInAdmin");
            if (loggedInUser == null)
            {
                return RedirectToAction("DoLogin", "Admin");
            }
            if (loggedInUser != null && loggedInUser.UserId <= 0)
            {
                return RedirectToAction("DoLogin", "Admin");
            }

            var steDetails = new SiteDetails();

            List<StateDetails> stateList = new List<StateDetails>();
            List<StateDetails> allStateList = new List<StateDetails>();
            List<AssetTypeViewModel> assetTypeList = new List<AssetTypeViewModel>();

            List<MarketDetails> marketList = new List<MarketDetails>();
            List<NewPropertyStatusModel> newPropertyStatusList = new List<NewPropertyStatusModel>();

            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {

                SqlCommand cmdStateList = new SqlCommand("GetStateList", con);
                cmdStateList.CommandType = CommandType.StoredProcedure;
                con.Open();

                SqlDataReader readerStateList = cmdStateList.ExecuteReader();
                while (readerStateList.Read())
                {
                    var stateDetails = new StateDetails();
                    stateDetails.StateId = readerStateList.IsDBNull(readerStateList.GetOrdinal("state_id")) ? 0 : readerStateList.GetInt32(readerStateList.GetOrdinal("state_id"));
                    stateDetails.StateCode = readerStateList.IsDBNull(readerStateList.GetOrdinal("state_code")) ? "" : readerStateList.GetString(readerStateList.GetOrdinal("state_code"));
                    stateDetails.StateName = readerStateList.IsDBNull(readerStateList.GetOrdinal("state_name")) ? "" : readerStateList.GetString(readerStateList.GetOrdinal("state_name"));
                    stateList.Add(stateDetails);
                }
                con.Close();

                con.Open();
                SqlCommand cmdMarket = new SqlCommand("GetMarketList", con);
                cmdMarket.CommandType = CommandType.StoredProcedure;


                SqlDataReader readerMarket = cmdMarket.ExecuteReader();
                while (readerMarket.Read())
                {
                    var marketDetails = new MarketDetails();
                    marketDetails.MarketId = readerMarket.IsDBNull(readerMarket.GetOrdinal("market_id")) ? 0 : readerMarket.GetInt32(readerMarket.GetOrdinal("market_id"));
                    marketDetails.MarketName = readerMarket.IsDBNull(readerMarket.GetOrdinal("market_name")) ? "" : readerMarket.GetString(readerMarket.GetOrdinal("market_name"));

                    marketList.Add(marketDetails);
                }

                con.Close();


                SqlCommand cmdAllState = new SqlCommand("GetAllStateList", con);
                cmdAllState.CommandType = CommandType.StoredProcedure;
                con.Open();

                SqlDataReader readerAllState = cmdAllState.ExecuteReader();
                while (readerAllState.Read())
                {
                    var allStateDetails = new StateDetails();
                    allStateDetails.StateId = readerAllState.IsDBNull(readerAllState.GetOrdinal("state_id")) ? 0 : readerAllState.GetInt32(readerAllState.GetOrdinal("state_id"));
                    allStateDetails.StateName = readerAllState.IsDBNull(readerAllState.GetOrdinal("state_name")) ? "" : readerAllState.GetString(readerAllState.GetOrdinal("state_name"));
                    allStateList.Add(allStateDetails);
                }
                con.Close();

                SqlCommand cmdAssetType = new SqlCommand("GetAssetType", con);
                cmdAssetType.Parameters.AddWithValue("property_type", 3);
                cmdAssetType.CommandType = CommandType.StoredProcedure;
                con.Open();

                SqlDataReader readerAssetType = cmdAssetType.ExecuteReader();
                while (readerAssetType.Read())
                {
                    var assetType = new AssetTypeViewModel();
                    assetType.AssetTypeId = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("asset_type_id")) ? 0 : readerAssetType.GetInt32(readerAssetType.GetOrdinal("asset_type_id"));
                    assetType.AssetTypeName = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("asset_type_name")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("asset_type_name"));

                    assetTypeList.Add(assetType);
                }
                con.Close();


                SqlCommand cmdNewPropertyStatus = new SqlCommand("GetNewPropertyStatus", con);
                cmdNewPropertyStatus.CommandType = CommandType.StoredProcedure;
                con.Open();

                SqlDataReader readerNewPropertyStatus = cmdNewPropertyStatus.ExecuteReader();
                while (readerNewPropertyStatus.Read())
                {
                    var newPropertyStatus = new NewPropertyStatusModel();
                    newPropertyStatus.StatusId = readerNewPropertyStatus.IsDBNull(readerNewPropertyStatus.GetOrdinal("new_property_status_id")) ? 0 : readerNewPropertyStatus.GetInt32(readerNewPropertyStatus.GetOrdinal("new_property_status_id"));
                    newPropertyStatus.StatusName = readerNewPropertyStatus.IsDBNull(readerNewPropertyStatus.GetOrdinal("new_property_status_name")) ? "" : readerNewPropertyStatus.GetString(readerNewPropertyStatus.GetOrdinal("new_property_status_name"));

                    newPropertyStatusList.Add(newPropertyStatus);
                }
                con.Close();


                SqlCommand cmd = new SqlCommand("GetSubittedPropertyListById", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("propertyId", propertyId);
                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {

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

                    steDetails.SiteStateName = reader.IsDBNull(reader.GetOrdinal("site_state_name")) ? "" : reader.GetString(reader.GetOrdinal("site_state_name"));

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

                    steDetails.SelectedImageName = reader.IsDBNull(reader.GetOrdinal("image_file_name")) ? "" : reader.GetString(reader.GetOrdinal("image_file_name"));
                    steDetails.SelectedPdfName = reader.IsDBNull(reader.GetOrdinal("pdf_file_name")) ? "" : reader.GetString(reader.GetOrdinal("pdf_file_name"));

                    if (steDetails.SelectedImageName.Trim().Length > 0)
                    {
                        string pic = @"../../submited_files/" + steDetails.SelectedImageName;
                        steDetails.SelectedImageName = pic;
                    }
                    else
                    {
                        steDetails.SelectedImageName = "no_image.png?a=1";
                        string pic = @"../../UploadedImage/" + steDetails.SelectedImageName;

                        steDetails.SelectedImageName = pic;
                    }

                    if (steDetails.SelectedPdfName.Trim().Length > 0)
                    {
                        string pic = @"../../submited_files/" + steDetails.SelectedPdfName;
                        steDetails.SelectedPdfName = pic;
                    }

                    steDetails.IsDeleted = reader.IsDBNull(reader.GetOrdinal("is_deleted")) ? 0 : reader.GetInt32(reader.GetOrdinal("is_deleted"));
                    steDetails.PotentialUse = reader.IsDBNull(reader.GetOrdinal("potential_use")) ? "" : reader.GetString(reader.GetOrdinal("potential_use"));

                    steDetails.CheckIfClientRepresentedByABroker = reader.IsDBNull(reader.GetOrdinal("client_represented_by_broker")) ? 0 : reader.GetInt32(reader.GetOrdinal("client_represented_by_broker"));
                    steDetails.BrokerOrFirmName = reader.IsDBNull(reader.GetOrdinal("broker_firm_name")) ? "" : reader.GetString(reader.GetOrdinal("broker_firm_name"));
                    steDetails.BrokerEmailAddress = reader.IsDBNull(reader.GetOrdinal("broker_email_address")) ? "" : reader.GetString(reader.GetOrdinal("broker_email_address"));
                    steDetails.BrokerContactNumber = reader.IsDBNull(reader.GetOrdinal("broker_contact_number")) ? "" : reader.GetString(reader.GetOrdinal("broker_contact_number"));

                    steDetails.AssetTypeId = reader.IsDBNull(reader.GetOrdinal("asset_type_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("asset_type_id"));
                    steDetails.Term = reader.IsDBNull(reader.GetOrdinal("term")) ? "" : reader.GetString(reader.GetOrdinal("term"));
                    steDetails.AskingRent = reader.IsDBNull(reader.GetOrdinal("asking_rent")) ? "" : reader.GetString(reader.GetOrdinal("asking_rent"));
                    steDetails.LeaseType = reader.IsDBNull(reader.GetOrdinal("lease_type")) ? 0 : reader.GetInt32(reader.GetOrdinal("lease_type"));
                    steDetails.AssetTypeId = reader.IsDBNull(reader.GetOrdinal("asset_type_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("asset_type_id"));
                    steDetails.NewPropertyStatusId = reader.IsDBNull(reader.GetOrdinal("new_property_status_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("new_property_status_id"));

                    steDetails.StatusChangedDate = reader.IsDBNull(reader.GetOrdinal("status_changed_date")) ? default(DateTime?) : reader.GetDateTime(reader.GetOrdinal("status_changed_date"));

                    steDetails.AssetId = reader.IsDBNull(reader.GetOrdinal("asset_id")) ? "" : reader.GetString(reader.GetOrdinal("asset_id"));

                    steDetails.PurchaseLeasebackTerm = reader.IsDBNull(reader.GetOrdinal("purchase_leaseback_term")) ? "" : reader.GetString(reader.GetOrdinal("purchase_leaseback_term"));
                    steDetails.PurchaseLeasebackRent = reader.IsDBNull(reader.GetOrdinal("purchase_leaseback_rent")) ? "" : reader.GetString(reader.GetOrdinal("purchase_leaseback_rent"));
                    steDetails.PurchaseLeasebackLeaseTypeId = reader.IsDBNull(reader.GetOrdinal("purchase_leaseback_lease_type_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("purchase_leaseback_lease_type_id"));

                    steDetails.PurchasePrice = reader.IsDBNull(reader.GetOrdinal("purchase_price")) ? "" : reader.GetString(reader.GetOrdinal("purchase_price"));
                    steDetails.PurchaseLeasebackPotentialUse = reader.IsDBNull(reader.GetOrdinal("purchase_leaseback_potential_use")) ? "" : reader.GetString(reader.GetOrdinal("purchase_leaseback_potential_use"));
                    steDetails.PurchaseLeasebackComments = reader.IsDBNull(reader.GetOrdinal("purchase_leaseback_comments")) ? "" : reader.GetString(reader.GetOrdinal("purchase_leaseback_comments"));
                    steDetails.AskingPrice = reader.IsDBNull(reader.GetOrdinal("asking_price")) ? "" : reader.GetString(reader.GetOrdinal("asking_price"));
                    steDetails.FeePotentialUse = reader.IsDBNull(reader.GetOrdinal("fee_potential_use")) ? "" : reader.GetString(reader.GetOrdinal("fee_potential_use"));
                    steDetails.FeeComments = reader.IsDBNull(reader.GetOrdinal("fee_comments")) ? "" : reader.GetString(reader.GetOrdinal("fee_comments"));
                    steDetails.TermOptionPurchase = reader.IsDBNull(reader.GetOrdinal("term_option_purchase")) ? "" : reader.GetString(reader.GetOrdinal("term_option_purchase"));
                    steDetails.AskingRentOptionPurchase = reader.IsDBNull(reader.GetOrdinal("asking_rent_option_purchase")) ? "" : reader.GetString(reader.GetOrdinal("asking_rent_option_purchase"));
                    steDetails.LeaseTypePurchase = reader.IsDBNull(reader.GetOrdinal("lease_type_purchase")) ? 0 : reader.GetInt32(reader.GetOrdinal("lease_type_purchase"));
                    steDetails.OptionPurchase = reader.IsDBNull(reader.GetOrdinal("option_purchase")) ? "" : reader.GetString(reader.GetOrdinal("option_purchase"));
                    steDetails.PotentialUseOptionPurchase = reader.IsDBNull(reader.GetOrdinal("potential_use_option_purchase")) ? "" : reader.GetString(reader.GetOrdinal("potential_use_option_purchase"));
                    steDetails.CommentsOptionPurchase = reader.IsDBNull(reader.GetOrdinal("comments_option_purchase")) ? "" : reader.GetString(reader.GetOrdinal("comments_option_purchase"));


                }
                con.Close();

                steDetails.AllStateList = allStateList;
                steDetails.StateList = stateList;
                steDetails.AssetTypeList = assetTypeList;
                steDetails.LeaseTypeList= GetLeaseTypeList();
                steDetails.NewPropertyStatusList = newPropertyStatusList;

                return View(steDetails);
            }
        }

        [HttpPost]
        public IActionResult SaveProperty(SiteDetails siteDetails)
        {
            UserViewModel loggedInUser = HttpContext.Session.GetObjectFromJson<UserViewModel>("LoggedInAdmin");
            if (loggedInUser == null)
            {
                return RedirectToAction("DoLogin", "Admin");
            }
            if (loggedInUser != null && loggedInUser.UserId <= 0)
            {
                return RedirectToAction("DoLogin", "Admin");
            }

            int siteDetailsId = siteDetails.SiteDetailsId;
            string CS = DBConnection.ConnectionString;

            var imageFileName = "";
            if (siteDetails.SelectedImage != null)
            {
                imageFileName = Helper.GetUniqueFileName(siteDetails.SelectedImage.FileName);
                var imageFilePath = Path.Combine(webHostEnvironment.WebRootPath + @"/submited_files", imageFileName);
                using (var stream = System.IO.File.Create(imageFilePath))
                {
                    siteDetails.SelectedImage.CopyTo(stream);
                }
            }

            var pdfFileName = "";
            if (siteDetails.SelectedPdf != null)
            {
                pdfFileName = Helper.GetUniqueFileName(siteDetails.SelectedPdf.FileName);
                var pdfFilePath = Path.Combine(webHostEnvironment.WebRootPath + @"/submited_files", pdfFileName);
                using (var stream = System.IO.File.Create(pdfFilePath))
                {
                    siteDetails.SelectedPdf.CopyTo(stream);
                }
            }


            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("SaveSubmittedProperty", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                cmd.Parameters.AddWithValue("site_details_id", siteDetails.SiteDetailsId);
                cmd.Parameters.AddWithValue("name_prefix", siteDetails.NamePrefix);
                cmd.Parameters.AddWithValue("first_name", siteDetails.FirstName);
                cmd.Parameters.AddWithValue("last_name", siteDetails.LastName);

                cmd.Parameters.AddWithValue("company_name", siteDetails.CompanyName);
                cmd.Parameters.AddWithValue("email_address", siteDetails.EmailAddress);
                cmd.Parameters.AddWithValue("address", siteDetails.Address);
                cmd.Parameters.AddWithValue("city_name", siteDetails.CityName);

                cmd.Parameters.AddWithValue("state_id", siteDetails.StateId);
                cmd.Parameters.AddWithValue("zip_code", siteDetails.ZipCode);
                cmd.Parameters.AddWithValue("contact_number", siteDetails.ContactNumber);
                cmd.Parameters.AddWithValue("sams_holding_employee", siteDetails.SamsHoldingEmployee);

                cmd.Parameters.AddWithValue("market_id", siteDetails.MarketId);
                cmd.Parameters.AddWithValue("site_address", siteDetails.SiteAddress);
                cmd.Parameters.AddWithValue("site_city", siteDetails.SiteCity);
                cmd.Parameters.AddWithValue("site_state_id", siteDetails.SiteStateId);

                cmd.Parameters.AddWithValue("site_county", siteDetails.SiteCounty);
                cmd.Parameters.AddWithValue("site_cross_street_name", siteDetails.SiteCrossStreetName);
                cmd.Parameters.AddWithValue("is_property_available", siteDetails.IsPropertyAvailable);
                cmd.Parameters.AddWithValue("zoning", siteDetails.Zoning);

                cmd.Parameters.AddWithValue("lot_size", siteDetails.LotSize);
                cmd.Parameters.AddWithValue("sales_price", siteDetails.SalesPrice);
                cmd.Parameters.AddWithValue("comments", siteDetails.Comments);

                cmd.Parameters.AddWithValue("image_file_name", imageFileName);
                cmd.Parameters.AddWithValue("pdf_file_name", pdfFileName);

                cmd.Parameters.AddWithValue("created_by", NewPropertyCreaedBy.BySams);
                cmd.Parameters.AddWithValue("potential_use", siteDetails.PotentialUse);

                cmd.Parameters.AddWithValue("client_represented_by_broker", siteDetails.CheckIfClientRepresentedByABroker);
                cmd.Parameters.AddWithValue("broker_firm_name", siteDetails.BrokerOrFirmName);
                cmd.Parameters.AddWithValue("broker_email_address", siteDetails.BrokerContactNumber);
                cmd.Parameters.AddWithValue("broker_contact_number", siteDetails.BrokerEmailAddress);

                cmd.Parameters.AddWithValue("term", siteDetails.Term);
                cmd.Parameters.AddWithValue("asking_rent", siteDetails.AskingRent);
                cmd.Parameters.AddWithValue("lease_type", siteDetails.LeaseType);
                cmd.Parameters.AddWithValue("asset_type_id", siteDetails.AssetTypeId);
                cmd.Parameters.AddWithValue("new_property_status_id", siteDetails.NewPropertyStatusId);
                cmd.Parameters.AddWithValue("status_changed_date", siteDetails.StatusChangedDate);
                cmd.Parameters.AddWithValue("asset_id", siteDetails.AssetId);

                cmd.Parameters.AddWithValue("purchase_leaseback_term", siteDetails.PurchaseLeasebackTerm);
                cmd.Parameters.AddWithValue("purchase_leaseback_rent", siteDetails.PurchaseLeasebackRent);
                cmd.Parameters.AddWithValue("purchase_leaseback_lease_type_id", siteDetails.PurchaseLeasebackLeaseTypeId);

                cmd.Parameters.AddWithValue("purchase_price", siteDetails.PurchasePrice);
                cmd.Parameters.AddWithValue("purchase_leaseback_potential_use", siteDetails.PurchaseLeasebackPotentialUse);
                cmd.Parameters.AddWithValue("purchase_leaseback_comments", siteDetails.PurchaseLeasebackComments);
                cmd.Parameters.AddWithValue("asking_price", siteDetails.AskingPrice);
                cmd.Parameters.AddWithValue("fee_potential_use", siteDetails.FeePotentialUse);
                cmd.Parameters.AddWithValue("fee_comments", siteDetails.FeeComments);
                cmd.Parameters.AddWithValue("term_option_purchase", siteDetails.TermOptionPurchase);
                cmd.Parameters.AddWithValue("asking_rent_option_purchase", siteDetails.AskingRentOptionPurchase);
                cmd.Parameters.AddWithValue("lease_type_purchase", siteDetails.LeaseTypePurchase);
                cmd.Parameters.AddWithValue("option_purchase", siteDetails.OptionPurchase);
                cmd.Parameters.AddWithValue("potential_use_option_purchase", siteDetails.PotentialUseOptionPurchase);
                cmd.Parameters.AddWithValue("comments_option_purchase", siteDetails.CommentsOptionPurchase);

                siteDetailsId = int.Parse(cmd.ExecuteScalar().ToString());

                siteDetails.SiteDetailsId = siteDetailsId;

                con.Close();
            }

            return RedirectToAction("ViewNewProperty", new { propertyId = siteDetailsId });
        }



        DiligenceLeaseViewModel GetDiligenceLease(int propertyId)
        {
            var diligenceLease = new DiligenceLeaseViewModel();

            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("GetDiligenceLease", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("property_id", propertyId);
                cmd.Parameters.AddWithValue("property_type", SamsPropertyType.NewPropertyDashboard);
                con.Open();

                diligenceLease.PropertyId = propertyId;
                diligenceLease.PropertyType = 1;

                SqlDataReader readerAssetType = cmd.ExecuteReader();
                while (readerAssetType.Read())
                {
                    diligenceLease.DiligenceLeaseId = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("diligence_lease_id")) ? 0 : readerAssetType.GetInt32(readerAssetType.GetOrdinal("diligence_lease_id"));
                    diligenceLease.Tenant = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("tenant_name")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("tenant_name"));

                    diligenceLease.Rent = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("rent")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("rent"));
                    diligenceLease.UnderContractDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("under_contract_date")) ? default(DateTime?) : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("under_contract_date"));
                    diligenceLease.DueDiligenceExpiryDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("due_diligence_expiry_date")) ? default(DateTime?) : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("due_diligence_expiry_date"));
                    diligenceLease.EarnestMoneyDeposit = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("earnest_money_deposit")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("earnest_money_deposit"));
                    diligenceLease.DDPExtension = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("ddp_extension")) ? default(DateTime?) : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("ddp_extension"));

                    diligenceLease.TenantAttorney = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("tenant_attorney")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("tenant_attorney"));
                    diligenceLease.TenantAgentCommission = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("tenant_agent_commission")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("tenant_agent_commission"));
                    diligenceLease.LandlordAgentCommission = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("land_lord_agent_commission")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("land_lord_agent_commission"));
                    diligenceLease.LeaseSecurityDeposit = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("lease_security_deposit")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("lease_security_deposit"));

                    diligenceLease.CreatedDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("created_date")) ? DateTime.Now : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("created_date"));
                    diligenceLease.LeaseCommencementDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("lease_commencement_date")) ? default(DateTime?) : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("lease_commencement_date"));
                    diligenceLease.ClosingDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("closing_date")) ? default(DateTime?) : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("closing_date"));
                }

                con.Close();

            }

            return diligenceLease;
        }

        [HttpPost]
        public IActionResult SaveDiligenceLease(DiligenceLeaseViewModel diligenceLease)
        {
            UserViewModel loggedInUser = HttpContext.Session.GetObjectFromJson<UserViewModel>("LoggedInAdmin");
            if (loggedInUser == null)
            {
                return RedirectToAction("DoLogin", "Admin");
            }
            if (loggedInUser != null && loggedInUser.UserId <= 0)
            {
                return RedirectToAction("DoLogin", "Admin");
            }

            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("SaveDiligenceLease", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("diligence_lease_id", diligenceLease.DiligenceLeaseId);

                cmd.Parameters.AddWithValue("property_id", diligenceLease.PropertyId);
                cmd.Parameters.AddWithValue("property_type", SamsPropertyType.NewPropertyDashboard);
                cmd.Parameters.AddWithValue("tenant_name", diligenceLease.Tenant);

                cmd.Parameters.AddWithValue("rent", diligenceLease.Rent);

                cmd.Parameters.AddWithValue("under_contract_date", diligenceLease.UnderContractDate);
                cmd.Parameters.AddWithValue("due_diligence_expiry_date", diligenceLease.DueDiligenceExpiryDate);
                cmd.Parameters.AddWithValue("earnest_money_deposit", diligenceLease.EarnestMoneyDeposit);
                cmd.Parameters.AddWithValue("ddp_extension", diligenceLease.DDPExtension);

                cmd.Parameters.AddWithValue("tenant_attorney", diligenceLease.TenantAttorney);
                cmd.Parameters.AddWithValue("tenant_agent_commission", diligenceLease.TenantAgentCommission);
                cmd.Parameters.AddWithValue("land_lord_agent_commission", diligenceLease.LandlordAgentCommission);
                cmd.Parameters.AddWithValue("lease_security_deposit", diligenceLease.LeaseSecurityDeposit);
                cmd.Parameters.AddWithValue("lease_commencement_date", diligenceLease.LeaseCommencementDate);
                cmd.Parameters.AddWithValue("closing_date", diligenceLease.ClosingDate);

                con.Open();


                diligenceLease.DiligenceLeaseId = int.Parse(cmd.ExecuteScalar().ToString());


                con.Close();

            }

            return RedirectToAction("ViewNewProperty", new { propertyId = diligenceLease.PropertyId });
        }


        List<PeriodViewModel> GetPeriodList(int propertyId, string periodType)
        {
            var periodList = new List<PeriodViewModel>();

            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("GetPeriodList", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("property_id", propertyId);
                cmd.Parameters.AddWithValue("property_type", SamsPropertyType.NewPropertyDashboard);
                cmd.Parameters.AddWithValue("period_type", periodType);

                con.Open();

                SqlDataReader readerAssetType = cmd.ExecuteReader();
                while (readerAssetType.Read())
                {
                    var periodView = new PeriodViewModel();

                    periodView.PeriodId = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("period_id")) ? 0 : readerAssetType.GetInt32(readerAssetType.GetOrdinal("period_id"));
                    periodView.PropertyId = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("property_id")) ? 0 : readerAssetType.GetInt32(readerAssetType.GetOrdinal("property_id"));
                    periodView.PropertyType = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("property_type")) ? 0 : readerAssetType.GetInt32(readerAssetType.GetOrdinal("property_type"));

                    periodView.PeriodMaster = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("period_master")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("period_master"));

                    periodView.StartDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("start_date")) ? DateTime.Now : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("start_date"));
                    periodView.EndDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("end_date")) ? DateTime.Now : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("end_date"));


                    periodView.PeriodNotes = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("period_notes")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("period_notes"));
                    periodView.PeriodType = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("period_type")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("period_type"));

                    periodView.AlertDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("alert_date")) ? default(DateTime?) : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("alert_date"));
                    periodView.OtherEmailAddress = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("other_email_address")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("other_email_address"));

                    periodList.Add(periodView);
                }

                con.Close();

            }

            return periodList;
        }

        [HttpPost]
        public IActionResult SavePeriod(PeriodViewModel period)
        {
            UserViewModel loggedInUser = HttpContext.Session.GetObjectFromJson<UserViewModel>("LoggedInAdmin");
            if (loggedInUser == null)
            {
                return RedirectToAction("DoLogin", "Admin");
            }
            if (loggedInUser != null && loggedInUser.UserId <= 0)
            {
                return RedirectToAction("DoLogin", "Admin");
            }

            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("SavePeriod", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("period_id", period.PeriodId);

                cmd.Parameters.AddWithValue("property_id", period.PropertyId);
                cmd.Parameters.AddWithValue("property_type", SamsPropertyType.NewPropertyDashboard);
                cmd.Parameters.AddWithValue("period_master", period.PeriodMaster);

                cmd.Parameters.AddWithValue("start_date", period.StartDate);
                DateTime endDate = period.StartDate.AddDays(period.AddedDuration);
                cmd.Parameters.AddWithValue("end_date", endDate);
                cmd.Parameters.AddWithValue("period_notes", period.PeriodNotes);
                cmd.Parameters.AddWithValue("period_type", period.PeriodType);

                cmd.Parameters.AddWithValue("alert_date", period.AlertDate);
                cmd.Parameters.AddWithValue("other_email_address", period.OtherEmailAddress);

                con.Open();


                period.PeriodId = int.Parse(cmd.ExecuteScalar().ToString());


                con.Close();

            }

            return RedirectToAction("ViewNewProperty", new { propertyId = period.PropertyId });
        }

        public IActionResult DeletePeriod(int periodId, int propertyId)
        {
            UserViewModel loggedInUser = HttpContext.Session.GetObjectFromJson<UserViewModel>("LoggedInAdmin");
            if (loggedInUser == null)
            {
                return RedirectToAction("DoLogin", "Admin");
            }
            if (loggedInUser != null && loggedInUser.UserId <= 0)
            {
                return RedirectToAction("DoLogin", "Admin");
            }

            var periodList = new List<PeriodViewModel>();

            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("DeletePeriod", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("period_id", periodId);

                con.Open();

                cmd.ExecuteReader();


                con.Close();

            }

            return RedirectToAction("ViewNewProperty", new { propertyId = propertyId });
        }







        public IActionResult CloseAcquisition(int diligenceAcquisitionId, int propertyId)
        {
            UserViewModel loggedInUser = HttpContext.Session.GetObjectFromJson<UserViewModel>("LoggedInAdmin");
            if (loggedInUser == null)
            {
                return RedirectToAction("DoLogin", "Admin");
            }
            if (loggedInUser != null && loggedInUser.UserId <= 0)
            {
                return RedirectToAction("DoLogin", "Admin");
            }

            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("CloseDiligenceAcquisition", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("diligence_acquisition_id", diligenceAcquisitionId);

                con.Open();
                cmd.ExecuteNonQuery();

                con.Close();
            }

            return RedirectToAction("ViewNewProperty", new { propertyId = propertyId });
        }

        public IActionResult TerminateAcquisition(int diligenceAcquisitionId, int propertyId)
        {
            UserViewModel loggedInUser = HttpContext.Session.GetObjectFromJson<UserViewModel>("LoggedInAdmin");
            if (loggedInUser == null)
            {
                return RedirectToAction("DoLogin", "Admin");
            }
            if (loggedInUser != null && loggedInUser.UserId <= 0)
            {
                return RedirectToAction("DoLogin", "Admin");
            }

            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("TerminateDiligenceAcquisition", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("diligence_acquisition_id", diligenceAcquisitionId);

                con.Open();
                cmd.ExecuteNonQuery();

                con.Close();
            }

            return RedirectToAction("ViewNewProperty", new { propertyId = propertyId });
        }


        public IActionResult CloseDisposition(int diligenceDispositionsId, int propertyId)
        {
            UserViewModel loggedInUser = HttpContext.Session.GetObjectFromJson<UserViewModel>("LoggedInAdmin");
            if (loggedInUser == null)
            {
                return RedirectToAction("DoLogin", "Admin");
            }
            if (loggedInUser != null && loggedInUser.UserId <= 0)
            {
                return RedirectToAction("DoLogin", "Admin");
            }

            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("CloseDiligenceDisposition", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("diligence_dispositions_id", diligenceDispositionsId);

                con.Open();
                cmd.ExecuteNonQuery();

                con.Close();
            }

            return RedirectToAction("ViewNewProperty", new { propertyId = propertyId });
        }

        public IActionResult TerminateDisposition(int diligenceDispositionsId, int propertyId)
        {
            UserViewModel loggedInUser = HttpContext.Session.GetObjectFromJson<UserViewModel>("LoggedInAdmin");
            if (loggedInUser == null)
            {
                return RedirectToAction("DoLogin", "Admin");
            }
            if (loggedInUser != null && loggedInUser.UserId <= 0)
            {
                return RedirectToAction("DoLogin", "Admin");
            }

            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("TerminateDiligenceDisposition", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("diligence_dispositions_id", diligenceDispositionsId);

                con.Open();
                cmd.ExecuteNonQuery();

                con.Close();
            }

            return RedirectToAction("ViewNewProperty", new { propertyId = propertyId });
        }





        DiligenceAcquisitionViewModel GetDiligenceAcquisition(int propertyId)
        {

            var diligenceAcquisition = new DiligenceAcquisitionViewModel();

            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("GetDiligenceAcquisition", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("property_id", propertyId);
                cmd.Parameters.AddWithValue("property_type", SamsPropertyType.NewPropertyDashboard);
                con.Open();

                diligenceAcquisition.PropertyId = propertyId;
                diligenceAcquisition.PropertyType = 1;

                SqlDataReader readerAssetType = cmd.ExecuteReader();
                while (readerAssetType.Read())
                {
                    diligenceAcquisition.DiligenceAcquisitionId = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("diligence_acquisition_id")) ? 0 : readerAssetType.GetInt32(readerAssetType.GetOrdinal("diligence_acquisition_id"));


                    diligenceAcquisition.PurchasePrice = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("purchase_price")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("purchase_price"));
                    diligenceAcquisition.EarnestMoney = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("earnest_money")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("earnest_money"));

                    diligenceAcquisition.Exchage1031 = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("exchange_1031")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("exchange_1031"));
                    diligenceAcquisition.Deadline1031 = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("dead_line_1031")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("dead_line_1031"));

                    diligenceAcquisition.Sellers = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("sellers")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("sellers"));
                    diligenceAcquisition.EscrowAgent = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("escrow_agent")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("escrow_agent"));
                    diligenceAcquisition.SubDivision = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("sub_division")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("sub_division")); diligenceAcquisition.Deadline1031 = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("dead_line_1031")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("dead_line_1031"));
                    diligenceAcquisition.RealEstateAgent = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("real_estate_agent")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("real_estate_agent"));

                    diligenceAcquisition.CreatedDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("created_date")) ? DateTime.Now : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("created_date"));

                    diligenceAcquisition.AcquisitionStatus = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("acquisition_status")) ? 0 : readerAssetType.GetInt32(readerAssetType.GetOrdinal("acquisition_status"));



                    diligenceAcquisition.UnderContractDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("under_contract_date")) ? default(DateTime?) : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("under_contract_date"));
                    diligenceAcquisition.DueDiligenceExpairyDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("due_diligence_expiry_date")) ? default(DateTime?) : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("due_diligence_expiry_date"));
                    diligenceAcquisition.DDPExtension = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("ddp_extension")) ? default(DateTime?) : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("ddp_extension"));

                    diligenceAcquisition.DDPExtensionOpted = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("check_if_ddp_extension_opted")) ? 0 : readerAssetType.GetInt32(readerAssetType.GetOrdinal("check_if_ddp_extension_opted"));

                    diligenceAcquisition.DDPExtensionOpted = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("check_if_ddp_extension_opted")) ? 0 : readerAssetType.GetInt32(readerAssetType.GetOrdinal("check_if_ddp_extension_opted"));
                    diligenceAcquisition.AdditionalEarnestMoneyDeposit = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("additional_earnest_money_deposit")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("additional_earnest_money_deposit"));
                    diligenceAcquisition.PermittingPeriod = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("permitting_period")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("permitting_period"));
                    diligenceAcquisition.BuyingEntity = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("buying_entity")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("buying_entity"));
                    diligenceAcquisition.BuyersAttorney = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("buyers_attorney")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("buyers_attorney"));
                    diligenceAcquisition.SellersAttorney = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("sellers_attorney")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("sellers_attorney"));
                    diligenceAcquisition.BuyersAgent = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("buyers_agent")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("buyers_agent"));
                    diligenceAcquisition.SellersAgent = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("sellers_agent")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("sellers_agent"));

                    diligenceAcquisition.SellersAgentCommission = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("sellers_agent_commission")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("sellers_agent_commission"));
                    diligenceAcquisition.BuyersAgentCommission = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("buyers_agent_commission")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("buyers_agent_commission"));
                    diligenceAcquisition.ClosingDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("closing_date")) ? default(DateTime?) : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("closing_date"));
                }

                con.Close();

            }

            return diligenceAcquisition;
        }


        DiligenceDispositionsViewModel GetDiligenceDispositions(int propertyId)
        {
            var diligenceDispositions = new DiligenceDispositionsViewModel();

            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("GetDiligenceDispositions", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("property_id", propertyId);
                cmd.Parameters.AddWithValue("property_type", SamsPropertyType.NewPropertyDashboard);
                con.Open();

                diligenceDispositions.PropertyId = propertyId;
                diligenceDispositions.PropertyType = 1;

                SqlDataReader readerAssetType = cmd.ExecuteReader();
                while (readerAssetType.Read())
                {
                    diligenceDispositions.DiligenceDispositionsId = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("diligence_dispositions_id")) ? 0 : readerAssetType.GetInt32(readerAssetType.GetOrdinal("diligence_dispositions_id"));

                    diligenceDispositions.SalePrice = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("sale_price")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("sale_price"));
                    diligenceDispositions.EarnestMoney = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("earnest_money")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("earnest_money"));

                    diligenceDispositions.Buyers = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("buyers")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("buyers"));
                    diligenceDispositions.EscrowAgent = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("escrow_agent")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("escrow_agent"));

                    diligenceDispositions.BuyersAttorney = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("buyers_attorney")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("buyers_attorney"));
                    diligenceDispositions.OptionsToExtend = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("options_to_extend")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("options_to_extend"));
                    diligenceDispositions.Commissions = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("commissions")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("commissions"));

                    diligenceDispositions.CreatedDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("created_date")) ? DateTime.Now : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("created_date"));

                    diligenceDispositions.DispositionStatus = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("disposition_status")) ? 0 : readerAssetType.GetInt32(readerAssetType.GetOrdinal("disposition_status"));

                    diligenceDispositions.ClosedDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("closed_date")) ? DateTime.Now : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("closed_date"));
                    diligenceDispositions.TerminatedDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("terminated_date")) ? DateTime.Now : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("terminated_date"));

                    diligenceDispositions.UnderContractDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("under_contract_date")) ? DateTime.Now : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("under_contract_date"));
                    diligenceDispositions.DueDiligenceExpairyDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("due_diligence_expairy_date")) ? DateTime.Now : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("due_diligence_expairy_date"));

                    diligenceDispositions.DueDiligenceAmount = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("due_diligence_amount")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("due_diligence_amount"));
                    diligenceDispositions.EMD = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("emd")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("emd"));
                    diligenceDispositions.DDPExtension = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("ddp_extension")) ? DateTime.Now : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("ddp_extension"));
                    diligenceDispositions.CreatedDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("created_date")) ? DateTime.Now : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("created_date"));

                    diligenceDispositions.DDPExtension = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("ddp_extension")) ? DateTime.Now : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("ddp_extension"));
                    diligenceDispositions.DDPExtensionOpted = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("dueDiligenceApplicableStatus")) ? 0 : readerAssetType.GetInt32(readerAssetType.GetOrdinal("dueDiligenceApplicableStatus"));

                    diligenceDispositions.SellersAttorney = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("sellersAttorney")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("sellersAttorney"));
                    diligenceDispositions.BuyersAgentCommission = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("buyers_agent_commision")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("buyers_agent_commision"));
                    diligenceDispositions.SellersAgentCommission = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("sellers_agent_commision")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("sellers_agent_commision"));
                }

                con.Close();

            }

            return diligenceDispositions;
        }

        [HttpPost]
        public IActionResult SaveDiligenceAcquisition(DiligenceAcquisitionViewModel diligenceAcquisition)
        {
            UserViewModel loggedInUser = HttpContext.Session.GetObjectFromJson<UserViewModel>("LoggedInAdmin");
            if (loggedInUser == null)
            {
                return RedirectToAction("DoLogin", "Admin");
            }
            if (loggedInUser != null && loggedInUser.UserId <= 0)
            {
                return RedirectToAction("DoLogin", "Admin");
            }

            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("SaveDiligenceAcquisition", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("diligence_acquisition_id", diligenceAcquisition.DiligenceAcquisitionId);

                cmd.Parameters.AddWithValue("property_id", diligenceAcquisition.PropertyId);
                cmd.Parameters.AddWithValue("property_type", SamsPropertyType.NewPropertyDashboard);
                cmd.Parameters.AddWithValue("purchase_price", diligenceAcquisition.PurchasePrice);
                cmd.Parameters.AddWithValue("earnest_money", diligenceAcquisition.EarnestMoney);
                cmd.Parameters.AddWithValue("exchange_1031", diligenceAcquisition.Exchage1031);
                cmd.Parameters.AddWithValue("dead_line_1031", diligenceAcquisition.Deadline1031);
                cmd.Parameters.AddWithValue("sellers", diligenceAcquisition.Sellers);
                cmd.Parameters.AddWithValue("escrow_agent", diligenceAcquisition.EscrowAgent);
                cmd.Parameters.AddWithValue("sub_division", diligenceAcquisition.SubDivision);
                cmd.Parameters.AddWithValue("real_estate_agent", diligenceAcquisition.RealEstateAgent);

                cmd.Parameters.AddWithValue("under_contract_date", diligenceAcquisition.UnderContractDate);
                cmd.Parameters.AddWithValue("due_diligence_expiry_date", diligenceAcquisition.DueDiligenceExpairyDate);


                cmd.Parameters.AddWithValue("ddp_extension", diligenceAcquisition.DDPExtension);
                cmd.Parameters.AddWithValue("check_if_ddp_extension_opted", diligenceAcquisition.DDPExtensionOpted);
                cmd.Parameters.AddWithValue("additional_earnest_money_deposit", diligenceAcquisition.AdditionalEarnestMoneyDeposit);
                cmd.Parameters.AddWithValue("permitting_period", diligenceAcquisition.PermittingPeriod);
                cmd.Parameters.AddWithValue("buying_entity", diligenceAcquisition.BuyingEntity);
                cmd.Parameters.AddWithValue("buyers_attorney", diligenceAcquisition.BuyersAttorney);
                cmd.Parameters.AddWithValue("sellers_attorney", diligenceAcquisition.SellersAttorney);
                cmd.Parameters.AddWithValue("buyers_agent", diligenceAcquisition.BuyersAgent);

                cmd.Parameters.AddWithValue("sellers_agent", diligenceAcquisition.SellersAgent);
                cmd.Parameters.AddWithValue("sellers_agent_commission", diligenceAcquisition.SellersAgentCommission);
                cmd.Parameters.AddWithValue("buyers_agent_commission", diligenceAcquisition.BuyersAgentCommission);
                cmd.Parameters.AddWithValue("closing_date", diligenceAcquisition.ClosingDate);
                cmd.Parameters.AddWithValue("acquisition_status", diligenceAcquisition.AcquisitionStatus); 

                con.Open();


                diligenceAcquisition.DiligenceAcquisitionId = int.Parse(cmd.ExecuteScalar().ToString());


                con.Close();

            }

            return RedirectToAction("ViewNewProperty", new { propertyId = diligenceAcquisition.PropertyId });
        }


        [HttpPost]
        public IActionResult SaveDiligenceDispositions(DiligenceDispositionsViewModel diligenceDispositions)
        {
            UserViewModel loggedInUser = HttpContext.Session.GetObjectFromJson<UserViewModel>("LoggedInAdmin");
            if (loggedInUser == null)
            {
                return RedirectToAction("DoLogin", "Admin");
            }
            if (loggedInUser != null && loggedInUser.UserId <= 0)
            {
                return RedirectToAction("DoLogin", "Admin");
            }

            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("SaveDiligenceDispositions", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("diligence_dispositions_id", diligenceDispositions.DiligenceDispositionsId);

                cmd.Parameters.AddWithValue("property_id", diligenceDispositions.PropertyId);
                cmd.Parameters.AddWithValue("property_type", SamsPropertyType.NewPropertyDashboard);
                cmd.Parameters.AddWithValue("sale_price", diligenceDispositions.SalePrice);
                cmd.Parameters.AddWithValue("earnest_money", diligenceDispositions.EarnestMoney);
                cmd.Parameters.AddWithValue("buyers", diligenceDispositions.Buyers);
                cmd.Parameters.AddWithValue("escrow_agent", diligenceDispositions.EscrowAgent);
                cmd.Parameters.AddWithValue("buyers_attorney", diligenceDispositions.BuyersAttorney);
                cmd.Parameters.AddWithValue("options_to_extend", diligenceDispositions.OptionsToExtend);
                cmd.Parameters.AddWithValue("commissions", diligenceDispositions.Commissions);


                
                cmd.Parameters.AddWithValue("due_diligence_expairy_date", diligenceDispositions.DueDiligenceExpairyDate);
                cmd.Parameters.AddWithValue("due_diligence_amount", diligenceDispositions.DueDiligenceAmount);
                cmd.Parameters.AddWithValue("emd", diligenceDispositions.EMD);

                
                cmd.Parameters.AddWithValue("ddp_extension", diligenceDispositions.DDPExtension);

                cmd.Parameters.AddWithValue("dueDiligenceApplicableStatus", diligenceDispositions.DDPExtensionOpted);

                cmd.Parameters.AddWithValue("sellersAttorney", diligenceDispositions.SellersAttorney);
                cmd.Parameters.AddWithValue("buyers_agent_commision", diligenceDispositions.BuyersAgentCommission);
                cmd.Parameters.AddWithValue("sellers_agent_commision", diligenceDispositions.SellersAgentCommission);

                con.Open();


                diligenceDispositions.DiligenceDispositionsId = int.Parse(cmd.ExecuteScalar().ToString());


                con.Close();

            }

            return RedirectToAction("ViewNewProperty", new { propertyId = diligenceDispositions.PropertyId });
        }

        List<LeaseTypeModel> GetLeaseTypeList()
        {
            var LeaseTypeList = new List<LeaseTypeModel>();

            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("GetLeaseTypeList", con);
                cmd.CommandType = CommandType.StoredProcedure;

                con.Open();

                SqlDataReader readerAssetType = cmd.ExecuteReader();
                while (readerAssetType.Read())
                {
                    var leaseType = new LeaseTypeModel();

                    leaseType.LeaseTypeId = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("lease_type_id")) ? 0 : readerAssetType.GetInt32(readerAssetType.GetOrdinal("lease_type_id"));
                    leaseType.LeaseType = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("lease_type")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("lease_type"));
                    LeaseTypeList.Add(leaseType);
                }

                con.Close();

            }

            return LeaseTypeList;
        }




        public IActionResult GetListByStatus(int statusId)
        {
            UserViewModel loggedInUser = HttpContext.Session.GetObjectFromJson<UserViewModel>("LoggedInAdmin");
            if (loggedInUser == null)
            {
                return RedirectToAction("DoLogin", "Admin");
            }
            if (loggedInUser != null && loggedInUser.UserId <= 0)
            {
                return RedirectToAction("DoLogin", "Admin");
            }

            NewPropertyDashboardViewModel newPropertyDashboard = new NewPropertyDashboardViewModel();
            List<SiteDetails> newPropertiesList = new List<SiteDetails>();

            string CS = DBConnection.ConnectionString;

            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("GetNewProertiesSummary", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    int totalR = reader.IsDBNull(reader.GetOrdinal("TotalData")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalData"));
                    string pType = reader.IsDBNull(reader.GetOrdinal("pType")) ? "" : reader.GetString(reader.GetOrdinal("pType"));

                    if (pType == "Total_Properties")
                    {
                        newPropertyDashboard.TotalProperties = totalR;
                    }
                    else if (pType == "Total_Research")
                    {
                        newPropertyDashboard.TotalResearch = totalR;
                    }
                    else if (pType == "Total_Under_Loi")
                    {
                        newPropertyDashboard.TotalUnderLoi = totalR;
                    }
                    else if (pType == "Total_Under_Contract")
                    {
                        newPropertyDashboard.TotalUnderContract = totalR;
                    }
                    else if (pType == "Total_Closed_Acquisitions")
                    {
                        newPropertyDashboard.TotalClosedAcquisitions = totalR;
                    }
                    else if (pType == "Total_Terminated_Acquisitions")
                    {
                        newPropertyDashboard.TotalTerminatedAcquisitions = totalR;
                    }
                }
                con.Close();
            }

            using (SqlConnection con = new SqlConnection(CS))
            {

                if(statusId == 1)
                {
                    ViewData["property_status"] = "Research/Vetting Date";
                }
                else if (statusId == 2)
                {
                    ViewData["property_status"] = "Under LOI Date";
                }
                else if (statusId == 3)
                {
                    ViewData["property_status"] = "Under Contract Date";
                }
                else if (statusId == 4)
                {
                    ViewData["property_status"] = "Closed Acquisition Date";
                }
                else if (statusId == 5)
                {
                    ViewData["property_status"] = "Terminated Acquisition Date";
                }

                SqlCommand cmd = new SqlCommand("GetInProgressPropertyListByStatus", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("new_property_status_id", statusId);
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

                    steDetails.SiteStateName = reader.IsDBNull(reader.GetOrdinal("site_state_name")) ? "" : reader.GetString(reader.GetOrdinal("site_state_name"));

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

                    steDetails.SelectedImageName = reader.IsDBNull(reader.GetOrdinal("image_file_name")) ? "" : reader.GetString(reader.GetOrdinal("image_file_name"));

                    if (steDetails.SelectedImageName.Trim().Length > 0)
                    {
                        string pic = @"../../submited_files/" + steDetails.SelectedImageName;
                        steDetails.SelectedImageName = pic;
                    }
                    else
                    {
                        steDetails.SelectedImageName = "no_image.png?a=1";
                        string pic = @"../../UploadedImage/" + steDetails.SelectedImageName;

                        steDetails.SelectedImageName = pic;
                    }

                    steDetails.IsDeleted = reader.IsDBNull(reader.GetOrdinal("is_deleted")) ? 0 : reader.GetInt32(reader.GetOrdinal("is_deleted"));
                    steDetails.StatusChangedDate = reader.IsDBNull(reader.GetOrdinal("status_changed_date")) ? default(DateTime?) : reader.GetDateTime(reader.GetOrdinal("status_changed_date"));
                    steDetails.AssetId = reader.IsDBNull(reader.GetOrdinal("asset_id")) ? "" : reader.GetString(reader.GetOrdinal("asset_id"));

                    newPropertiesList.Add(steDetails);


                }
                con.Close();
            }
            
            newPropertyDashboard.PropertyList = newPropertiesList;
            return View(newPropertyDashboard);
        }

        [HttpPost]
        public RedirectToActionResult UploadImage(ImageViewModel uploadedImge)
        {
            UserViewModel loggedInUser = HttpContext.Session.GetObjectFromJson<UserViewModel>("LoggedInAdmin");
            if (loggedInUser == null)
            {
                return RedirectToAction("DoLogin", "Admin");
            }
            if (loggedInUser != null && loggedInUser.UserId <= 0)
            {
                return RedirectToAction("DoLogin", "Admin");
            }

            var uniqueFileName = Helper.GetUniqueFileName(uploadedImge.UploadedImage.FileName);

            var filePath = Path.Combine(webHostEnvironment.WebRootPath + @"/UploadedImage", uniqueFileName);

            using (var stream = System.IO.File.Create(filePath))
            {
                uploadedImge.UploadedImage.CopyTo(stream);
            }

            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("SavePropertyImage", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                cmd.Parameters.AddWithValue("property_id", uploadedImge.PropertyId);
                cmd.Parameters.AddWithValue("image_name", uniqueFileName);
                cmd.Parameters.AddWithValue("property_type", uploadedImge.PropertyType);

                cmd.ExecuteNonQuery();


                con.Close();
            }


            return RedirectToAction("ViewNewProperty", new { propertyId = uploadedImge.PropertyId });
        }

        public RedirectToActionResult DeleteImage(int imageId, int propertyId)
        {
            UserViewModel loggedInUser = HttpContext.Session.GetObjectFromJson<UserViewModel>("LoggedInAdmin");
            if (loggedInUser == null)
            {
                return RedirectToAction("DoLogin", "Admin");
            }
            if (loggedInUser != null && loggedInUser.UserId <= 0)
            {
                return RedirectToAction("DoLogin", "Admin");
            }

            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("DeleteUploadedImage", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                cmd.Parameters.AddWithValue("image_id", imageId);

                cmd.ExecuteNonQuery();


                con.Close();
                
                return RedirectToAction("ViewNewProperty", new { propertyId = propertyId });
            }

        }


        [HttpPost]
        public RedirectToActionResult SaveAdditionalFile(AdditionalFilesViewModel uploadedFile)
        {
            UserViewModel loggedInUser = HttpContext.Session.GetObjectFromJson<UserViewModel>("LoggedInAdmin");
            if (loggedInUser == null)
            {
                return RedirectToAction("DoLogin", "Admin");
            }
            if (loggedInUser != null && loggedInUser.UserId <= 0)
            {
                return RedirectToAction("DoLogin", "Admin");
            }

            var uniqueFileName = Helper.GetUniqueFileName(uploadedFile.SelectedFile.FileName);

            var filePath = Path.Combine(webHostEnvironment.WebRootPath + @"/property_files", uniqueFileName);

            using (var stream = System.IO.File.Create(filePath))
            {
                uploadedFile.SelectedFile.CopyTo(stream);
            }

            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("SaveNewPropertyFiles", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                cmd.Parameters.AddWithValue("file_id", uploadedFile.FileId);
                cmd.Parameters.AddWithValue("property_id", uploadedFile.PropertyId);
                cmd.Parameters.AddWithValue("file_type", uploadedFile.FileType);
                cmd.Parameters.AddWithValue("file_name", uniqueFileName);


                cmd.ExecuteNonQuery();


                con.Close();
            }


            
            return RedirectToAction("ViewNewProperty", new { propertyId = uploadedFile.PropertyId });
        }


        public RedirectToActionResult DeleteFile(int fileId, int propertyId)
        {
            UserViewModel loggedInUser = HttpContext.Session.GetObjectFromJson<UserViewModel>("LoggedInAdmin");
            if (loggedInUser == null)
            {
                return RedirectToAction("DoLogin", "Admin");
            }
            if (loggedInUser != null && loggedInUser.UserId <= 0)
            {
                return RedirectToAction("DoLogin", "Admin");
            }

            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("DeleteNewPropertyFile", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                cmd.Parameters.AddWithValue("file_id", fileId);

                cmd.ExecuteNonQuery();


                con.Close();
                
                return RedirectToAction("ViewNewProperty", new { propertyId = propertyId });
            }

        }

        DiligenceLeaseWithPurchaseViewModel GetDiligenceLeaseWithPurchase(int propertyId)
        {
            var diligenceLeaseWithPurchase = new DiligenceLeaseWithPurchaseViewModel();

            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("GetDiligenceLeaseWithPurchase", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("property_id", propertyId);
                cmd.Parameters.AddWithValue("property_type", SamsPropertyType.NewPropertyDashboard);
                con.Open();

                diligenceLeaseWithPurchase.PropertyId = propertyId;
                diligenceLeaseWithPurchase.PropertyType = 1;

                SqlDataReader readerAssetType = cmd.ExecuteReader();
                while (readerAssetType.Read())
                {
                    diligenceLeaseWithPurchase.DiligenceLeaseWithPurchaseId = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("diligence_lease_with_purchase_id")) ? 0 : readerAssetType.GetInt32(readerAssetType.GetOrdinal("diligence_lease_with_purchase_id"));
                    diligenceLeaseWithPurchase.Tenant = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("tenant_name")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("tenant_name"));

                    diligenceLeaseWithPurchase.Rent = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("rent")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("rent"));
                    diligenceLeaseWithPurchase.UnderContractDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("under_contract_date")) ? default(DateTime?) : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("under_contract_date"));
                    diligenceLeaseWithPurchase.DueDiligenceExpiryDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("due_diligence_expiry_date")) ? default(DateTime?) : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("due_diligence_expiry_date"));
                    diligenceLeaseWithPurchase.EarnestMoneyDeposit = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("earnest_money_deposit")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("earnest_money_deposit"));
                    diligenceLeaseWithPurchase.DDPExtension = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("ddp_extension")) ? default(DateTime?) : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("ddp_extension"));

                    diligenceLeaseWithPurchase.TenantAttorney = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("tenant_attorney")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("tenant_attorney"));
                    diligenceLeaseWithPurchase.TenantAgentCommission = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("tenant_agent_commission")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("tenant_agent_commission"));
                    diligenceLeaseWithPurchase.LandlordAgentCommission = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("land_lord_agent_commission")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("land_lord_agent_commission"));
                    diligenceLeaseWithPurchase.LeaseSecurityDeposit = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("lease_security_deposit")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("lease_security_deposit"));

                    diligenceLeaseWithPurchase.CreatedDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("created_date")) ? DateTime.Now : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("created_date"));
                    diligenceLeaseWithPurchase.LeaseCommencementDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("lease_commencement_date")) ? default(DateTime?) : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("lease_commencement_date"));

                    diligenceLeaseWithPurchase.OptionPrice = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("option_price")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("option_price"));
                    diligenceLeaseWithPurchase.OptionPurchaseDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("option_purchase_date")) ? default(DateTime?) : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("option_purchase_date"));

                    diligenceLeaseWithPurchase.ClosingDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("closing_date")) ? default(DateTime?) : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("closing_date"));
                }

                con.Close();

            }

            return diligenceLeaseWithPurchase;
        }

        [HttpPost]
        public IActionResult SaveDiligenceLeaseWithPurchase(DiligenceLeaseWithPurchaseViewModel diligenceLeaseWithPurchase)
        {
            UserViewModel loggedInUser = HttpContext.Session.GetObjectFromJson<UserViewModel>("LoggedInAdmin");
            if (loggedInUser == null)
            {
                return RedirectToAction("DoLogin", "Admin");
            }
            if (loggedInUser != null && loggedInUser.UserId <= 0)
            {
                return RedirectToAction("DoLogin", "Admin");
            }

            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("SaveDiligenceLeaseWithPurchase", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("diligence_lease_with_purchase_id", diligenceLeaseWithPurchase.DiligenceLeaseWithPurchaseId);

                cmd.Parameters.AddWithValue("property_id", diligenceLeaseWithPurchase.PropertyId);
                cmd.Parameters.AddWithValue("property_type", SamsPropertyType.NewPropertyDashboard);
                cmd.Parameters.AddWithValue("tenant_name", diligenceLeaseWithPurchase.Tenant);
                cmd.Parameters.AddWithValue("rent", diligenceLeaseWithPurchase.Rent);

                cmd.Parameters.AddWithValue("under_contract_date", diligenceLeaseWithPurchase.UnderContractDate);
                cmd.Parameters.AddWithValue("due_diligence_expiry_date", diligenceLeaseWithPurchase.DueDiligenceExpiryDate);
                cmd.Parameters.AddWithValue("earnest_money_deposit", diligenceLeaseWithPurchase.EarnestMoneyDeposit);
                cmd.Parameters.AddWithValue("ddp_extension", diligenceLeaseWithPurchase.DDPExtension);

                cmd.Parameters.AddWithValue("tenant_attorney", diligenceLeaseWithPurchase.TenantAttorney);
                cmd.Parameters.AddWithValue("tenant_agent_commission", diligenceLeaseWithPurchase.TenantAgentCommission);
                cmd.Parameters.AddWithValue("land_lord_agent_commission", diligenceLeaseWithPurchase.LandlordAgentCommission);
                cmd.Parameters.AddWithValue("lease_security_deposit", diligenceLeaseWithPurchase.LeaseSecurityDeposit);

                cmd.Parameters.AddWithValue("disposition_terminated_status", diligenceLeaseWithPurchase.DispositionTerminatedStatus);
                cmd.Parameters.AddWithValue("disposition_terminated_date", diligenceLeaseWithPurchase.DispositionTerminatedDate);
                cmd.Parameters.AddWithValue("disposition_closed_status", diligenceLeaseWithPurchase.DispositionClosedStatus);
                cmd.Parameters.AddWithValue("disposition_closed_date", diligenceLeaseWithPurchase.DispositionClosedDate);

                cmd.Parameters.AddWithValue("selected_transaction_id", diligenceLeaseWithPurchase.SelectedTransactionStatusId);
                cmd.Parameters.AddWithValue("selected_transaction_date", diligenceLeaseWithPurchase.SelectedTransactionDate);
                cmd.Parameters.AddWithValue("lease_commencement_date", diligenceLeaseWithPurchase.LeaseCommencementDate);
                cmd.Parameters.AddWithValue("option_price", diligenceLeaseWithPurchase.OptionPrice);
                cmd.Parameters.AddWithValue("option_purchase_date", diligenceLeaseWithPurchase.OptionPurchaseDate);
                cmd.Parameters.AddWithValue("closing_date", diligenceLeaseWithPurchase.ClosingDate);

                con.Open();


                diligenceLeaseWithPurchase.DiligenceLeaseWithPurchaseId = int.Parse(cmd.ExecuteScalar().ToString());


                con.Close();


                PropertyHistoryModel propertyHistory = new PropertyHistoryModel();
                propertyHistory.PropertyId = diligenceLeaseWithPurchase.PropertyId;
                propertyHistory.StatusId = diligenceLeaseWithPurchase.SelectedTransactionStatusId;
                propertyHistory.Description = diligenceLeaseWithPurchase.TransactionDescription;
                propertyHistory.LoggedInId = loggedInUser.UserId;
                propertyHistory.TransactionId = diligenceLeaseWithPurchase.DiligenceLeaseWithPurchaseId;

                //PropertyHistory.SavePropertyHistory(propertyHistory);

            }

            return RedirectToAction("ViewNewProperty", new { propertyId = diligenceLeaseWithPurchase.PropertyId });
        }

        DiligenceDispositionsViewModel GetDiligenceDispositions_SaleLeaseBack(int propertyId)
        {
            var ddpViewModel = new DiligenceDispositionsViewModel();

            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("GetDiligenceDispositions_SaleLeaseBack", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("property_id", propertyId);
                cmd.Parameters.AddWithValue("property_type", SamsPropertyType.NewPropertyDashboard);
                con.Open();



                SqlDataReader readerAssetType = cmd.ExecuteReader();
                while (readerAssetType.Read())
                {
                    

                    ddpViewModel.PropertyId = propertyId;
                    ddpViewModel.PropertyType = (int)SamsPropertyType.Surplus;
                    ddpViewModel.DiligenceDispositionsId = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("diligence_dispositions_id")) ? 0 : readerAssetType.GetInt32(readerAssetType.GetOrdinal("diligence_dispositions_id"));

                    ddpViewModel.SalePrice = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("sale_price")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("sale_price"));
                    ddpViewModel.EarnestMoney = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("earnest_money")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("earnest_money"));
                    //ddpViewModel.EarnestMoney = Helper.FormatCurrency("$", ddpViewModel.EarnestMoney);

                    ddpViewModel.Buyers = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("buyers")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("buyers"));
                    ddpViewModel.EscrowAgent = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("escrow_agent")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("escrow_agent"));

                    ddpViewModel.BuyersAttorney = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("buyers_attorney")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("buyers_attorney"));
                    ddpViewModel.OptionsToExtend = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("options_to_extend")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("options_to_extend"));
                    ddpViewModel.Commissions = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("commissions")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("commissions"));

                    ddpViewModel.CreatedDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("created_date")) ? DateTime.Now : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("created_date"));
                    ddpViewModel.DispositionStatus = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("disposition_status")) ? 0 : readerAssetType.GetInt32(readerAssetType.GetOrdinal("disposition_status"));

                    ddpViewModel.ClosedDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("closed_date")) ? DateTime.Now : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("closed_date"));
                    ddpViewModel.TerminatedDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("terminated_date")) ? DateTime.Now : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("terminated_date"));

                    ddpViewModel.UnderContractDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("under_contract_date")) ? default(DateTime?) : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("under_contract_date"));
                    ddpViewModel.DueDiligenceExpairyDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("due_diligence_expairy_date")) ? DateTime.Now : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("due_diligence_expairy_date"));

                    ddpViewModel.DueDiligenceAmount = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("due_diligence_amount")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("due_diligence_amount"));
                    ddpViewModel.EMD = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("emd")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("emd"));
                    ddpViewModel.DDPExtension = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("ddp_extension")) ? default(DateTime?) : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("ddp_extension"));
                    ddpViewModel.CreatedDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("created_date")) ? DateTime.Now : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("created_date"));


                    ddpViewModel.UnderContractDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("under_contract_date")) ? default(DateTime?) : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("under_contract_date"));
                    ddpViewModel.DueDiligenceExpairyDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("due_diligence_expairy_date")) ? default(DateTime?) : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("due_diligence_expairy_date"));

                    ddpViewModel.DueDiligenceAmount = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("due_diligence_amount")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("due_diligence_amount"));
                    ddpViewModel.EMD = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("emd")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("emd"));
                    //ddpViewModel.EMD = Helper.FormatCurrency("$", ddpViewModel.EMD);
                    ddpViewModel.DDPExtension = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("ddp_extension")) ? default(DateTime?) : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("ddp_extension"));
                    ddpViewModel.DDPExtensionOpted = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("dueDiligenceApplicableStatus")) ? 0 : readerAssetType.GetInt32(readerAssetType.GetOrdinal("dueDiligenceApplicableStatus"));

                    ddpViewModel.SellersAttorney = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("sellersAttorney")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("sellersAttorney"));
                    ddpViewModel.BuyersAgentCommission = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("buyers_agent_commision")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("buyers_agent_commision"));
                    ddpViewModel.SellersAgentCommission = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("sellers_agent_commision")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("sellers_agent_commision"));

                    ddpViewModel.DispositionTerminatedStatus = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("disposition_terminated_status")) ? 0 : readerAssetType.GetInt32(readerAssetType.GetOrdinal("disposition_terminated_status"));
                    ddpViewModel.DispositionTerminatedDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("disposition_terminated_date")) ? default(DateTime?) : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("disposition_terminated_date"));
                    ddpViewModel.DispositionClosedStatus = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("disposition_closed_status")) ? 0 : readerAssetType.GetInt32(readerAssetType.GetOrdinal("disposition_closed_status"));
                    ddpViewModel.DispositionClosedDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("disposition_closed_date")) ? default(DateTime?) : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("disposition_closed_date"));

                    ddpViewModel.SelectedTransactionStatusId = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("selected_transaction_id")) ? 0 : readerAssetType.GetInt32(readerAssetType.GetOrdinal("selected_transaction_id"));
                    ddpViewModel.SelectedTransactionStatusName = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("transaction_status_name")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("transaction_status_name"));
                    ddpViewModel.SelectedTransactionDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("selected_transaction_date")) ? DateTime.Now : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("selected_transaction_date"));

                    ddpViewModel.Rent_SaleLeaseBack = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("rent")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("rent"));
                    ddpViewModel.Term_SaleLeaseBack = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("term")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("term"));
                    ddpViewModel.LeaseType_SaleLeaseBack = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("lease_type")) ? 0 : readerAssetType.GetInt32(readerAssetType.GetOrdinal("lease_type"));
                    ddpViewModel.LeaseCommencementDate_SaleLeaseBack = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("lease_commencement_date")) ? default(DateTime?) : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("lease_commencement_date"));
                    ddpViewModel.ClosingDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("closing_date")) ? default(DateTime?) : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("closing_date"));
                }

                con.Close();

            }

            return ddpViewModel;
        }

        List<TransactionStatusModel> GetTransactionStatusList(int currentTransactionStatusId, int propertyTransactionStatusId)
        {
            var transactionStatusList = new List<TransactionStatusModel>();

            currentTransactionStatusId = propertyTransactionStatusId;

            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("GetTransactionStatusList", con);
                cmd.CommandType = CommandType.StoredProcedure;

                con.Open();

                SqlDataReader readerAssetType = cmd.ExecuteReader();
                while (readerAssetType.Read())
                {
                    var transactionStatus = new TransactionStatusModel();

                    transactionStatus.TransactionStatusId = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("transaction_status_id")) ? 0 : readerAssetType.GetInt32(readerAssetType.GetOrdinal("transaction_status_id"));
                    transactionStatus.TransactionStatusName = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("transaction_status_name")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("transaction_status_name"));

                    transactionStatusList.Add(transactionStatus);

                    /*
                    if (currentTransactionStatusId > 0)
                    {
                        if (currentTransactionStatusId == (int)SamsTransactionStatus.Under_LOI)
                        {
                            if (transactionStatus.TransactionStatusId == (int)SamsTransactionStatus.Under_Contract ||
                                transactionStatus.TransactionStatusId == (int)SamsTransactionStatus.Terminated_Dispositions ||
                                transactionStatus.TransactionStatusId == (int)SamsTransactionStatus.Closed_Dispositions ||
                                transactionStatus.TransactionStatusId == (int)SamsTransactionStatus.Under_LOI)
                            {
                                transactionStatusList.Add(transactionStatus);
                            }
                        }
                        else if (currentTransactionStatusId == (int)SamsTransactionStatus.Under_Contract)
                        {
                            if (transactionStatus.TransactionStatusId == (int)SamsTransactionStatus.Terminated_Dispositions ||
                                transactionStatus.TransactionStatusId == (int)SamsTransactionStatus.Closed_Dispositions ||
                                transactionStatus.TransactionStatusId == (int)SamsTransactionStatus.Under_Contract)
                            {
                                transactionStatusList.Add(transactionStatus);
                            }
                        }
                        else if (currentTransactionStatusId == (int)SamsTransactionStatus.Closed_Dispositions)
                        {
                            if (transactionStatus.TransactionStatusId == (int)SamsTransactionStatus.Closed_Dispositions ||
                                transactionStatus.TransactionStatusId == (int)SamsTransactionStatus.Terminated_Dispositions)
                            {
                                transactionStatusList.Add(transactionStatus);
                            }
                        }

                        else if (currentTransactionStatusId == (int)SamsTransactionStatus.LOI_Received ||
                            currentTransactionStatusId == (int)SamsTransactionStatus.Terminated_Dispositions)
                        {
                            transactionStatusList.Add(transactionStatus);
                        }
                    }
                    else
                    {
                        transactionStatusList.Add(transactionStatus);
                    }
                    */


                }

                con.Close();

            }

            return transactionStatusList;
        }

        [HttpPost]
        public IActionResult SaveDiligenceDispositions_SaleLeaseBack(DiligenceDispositionsViewModel diligenceDispositions)
        {
            UserViewModel loggedInUser = HttpContext.Session.GetObjectFromJson<UserViewModel>("LoggedInAdmin");
            if (loggedInUser == null)
            {
                return RedirectToAction("DoLogin", "Admin");
            }
            if (loggedInUser != null && loggedInUser.UserId <= 0)
            {
                return RedirectToAction("DoLogin", "Admin");
            }

            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("SaveDiligenceDispositions_SaleLeaseBack", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("diligence_dispositions_id", diligenceDispositions.DiligenceDispositionsId);

                cmd.Parameters.AddWithValue("property_id", diligenceDispositions.PropertyId);
                cmd.Parameters.AddWithValue("property_type", SamsPropertyType.NewPropertyDashboard);
                cmd.Parameters.AddWithValue("sale_price", diligenceDispositions.SalePrice);
                cmd.Parameters.AddWithValue("earnest_money", diligenceDispositions.EarnestMoney);
                cmd.Parameters.AddWithValue("buyers", diligenceDispositions.Buyers);
                cmd.Parameters.AddWithValue("escrow_agent", diligenceDispositions.EscrowAgent);
                cmd.Parameters.AddWithValue("buyers_attorney", diligenceDispositions.BuyersAttorney);
                cmd.Parameters.AddWithValue("options_to_extend", diligenceDispositions.OptionsToExtend);
                cmd.Parameters.AddWithValue("commissions", diligenceDispositions.Commissions);

                cmd.Parameters.AddWithValue("under_contract_date", diligenceDispositions.UnderContractDate);

                cmd.Parameters.AddWithValue("due_diligence_expairy_date", diligenceDispositions.DueDiligenceExpairyDate);
                cmd.Parameters.AddWithValue("due_diligence_amount", diligenceDispositions.DueDiligenceAmount);
                cmd.Parameters.AddWithValue("emd", diligenceDispositions.EMD);

                cmd.Parameters.AddWithValue("ddp_extension", diligenceDispositions.DDPExtension);
                cmd.Parameters.AddWithValue("dueDiligenceApplicableStatus", diligenceDispositions.DDPExtensionOpted);

                cmd.Parameters.AddWithValue("sellersAttorney", diligenceDispositions.SellersAttorney);
                cmd.Parameters.AddWithValue("buyers_agent_commision", diligenceDispositions.BuyersAgentCommission);
                cmd.Parameters.AddWithValue("sellers_agent_commision", diligenceDispositions.SellersAgentCommission);

                cmd.Parameters.AddWithValue("disposition_terminated_status", diligenceDispositions.DispositionTerminatedStatus);
                cmd.Parameters.AddWithValue("disposition_terminated_date", diligenceDispositions.DispositionTerminatedDate);
                cmd.Parameters.AddWithValue("disposition_closed_status", diligenceDispositions.DispositionClosedStatus);
                cmd.Parameters.AddWithValue("disposition_closed_date", diligenceDispositions.DispositionClosedDate);

                cmd.Parameters.AddWithValue("selected_transaction_id", diligenceDispositions.SelectedTransactionStatusId);

                cmd.Parameters.AddWithValue("selected_transaction_date", diligenceDispositions.SelectedTransactionDate);
                cmd.Parameters.AddWithValue("permitting_period", diligenceDispositions.PermittingPeriod);
                cmd.Parameters.AddWithValue("rent", diligenceDispositions.Rent_SaleLeaseBack);
                cmd.Parameters.AddWithValue("term", diligenceDispositions.Term_SaleLeaseBack);
                cmd.Parameters.AddWithValue("lease_type", diligenceDispositions.LeaseType_SaleLeaseBack);
                cmd.Parameters.AddWithValue("lease_commencement_date", diligenceDispositions.LeaseCommencementDate_SaleLeaseBack);
                cmd.Parameters.AddWithValue("closing_date", diligenceDispositions.ClosingDate);
                con.Open();


                diligenceDispositions.DiligenceDispositionsId = int.Parse(cmd.ExecuteScalar().ToString());


                con.Close();



                


            }

            return RedirectToAction("ViewNewProperty", new { propertyId = diligenceDispositions.PropertyId });
            
        }

        [HttpPost]
        public IActionResult SavePeriodFromDashboard(PeriodViewModel period)
        {
            UserViewModel loggedInUser = HttpContext.Session.GetObjectFromJson<UserViewModel>("LoggedInAdmin");
            if (loggedInUser == null)
            {
                return RedirectToAction("DoLogin", "Admin");
            }
            if (loggedInUser != null && loggedInUser.UserId <= 0)
            {
                return RedirectToAction("DoLogin", "Admin");
            }

            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("UpdatePeriodFromDashboard", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("period_id", period.PeriodId);

                cmd.Parameters.AddWithValue("period_master", period.PeriodMaster);

                cmd.Parameters.AddWithValue("start_date", period.StartDate);

                DateTime endDate = period.StartDate.AddDays(period.AddedDuration);

                //cmd.Parameters.AddWithValue("end_date", period.EndDate);
                cmd.Parameters.AddWithValue("end_date", endDate);

                cmd.Parameters.AddWithValue("period_notes", period.PeriodNotes);


                cmd.Parameters.AddWithValue("alert_date", period.AlertDate);
                cmd.Parameters.AddWithValue("other_email_address", period.OtherEmailAddress);

                con.Open();


                period.PeriodId = int.Parse(cmd.ExecuteScalar().ToString());


                con.Close();

            }
            return RedirectToAction("Index");

        }
    }
}