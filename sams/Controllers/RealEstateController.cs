using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using sams.Common;
using sams.Models;
using System.Web;
using System.Net.Http;
using System.Net;
using Xceed.Words.NET;
using DocuSign.Integrations.Client;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;
using System.Text;
using System.Globalization;
using System.Runtime.Intrinsics.X86;

namespace sams.Controllers
{
    public class RealEstateController : Controller
    {
        private readonly IWebHostEnvironment webHostEnvironment;

        public RealEstateController(IWebHostEnvironment hostEnvironment)
        {

            webHostEnvironment = hostEnvironment;
        }

        public IActionResult Index()
        {
            CustomerViewModel loggedInUser = HttpContext.Session.GetObjectFromJson<CustomerViewModel>("LoggedInUser");
            

            if (loggedInUser != null)
            {
                ViewData["LoggedInUserId"] = loggedInUser.CustomerId;
                ViewData["LoggedInUserName"] = loggedInUser.FirstName + " " + loggedInUser.LastName;
            }
            else
            {
                loggedInUser = GetEmptyCustomer();
            }

            

            return View();
        }

        public ActionResult surplus_real_estate(int stateId, int regionId, int p)
        {
            // GeneratePdf();
            SurplusRealestateViewModel surplusViewModel = new SurplusRealestateViewModel();

            List<SiteDetails> surplusPropertiesList = new List<SiteDetails>();

            List<StateDetails> stateList = GetStateList();

            // string CS = ConfigurationManager.ConnectionStrings["testConnection"].ConnectionString;
            string CS = DBConnection.ConnectionString;
            int totalCount = 0;

            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("GetPropertyListByState", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("stateId", stateId);
                cmd.Parameters.AddWithValue("region_id", regionId);

                int startIndex = p * Helper.PageSize;
                if (p > 0)
                {
                    startIndex = startIndex + 1;
                    
                }

                cmd.Parameters.AddWithValue("currentPage", startIndex);
                cmd.Parameters.AddWithValue("pageSize", ((p * Helper.PageSize) + Helper.PageSize));
                
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

                    steDetails.StateName = reader.IsDBNull(reader.GetOrdinal("state_name")) ? "" : reader.GetString(reader.GetOrdinal("state_name"));

                    steDetails.ZipCode = reader.IsDBNull(reader.GetOrdinal("zip_code")) ? "" : reader.GetString(reader.GetOrdinal("zip_code"));
                    steDetails.ContactNumber = reader.IsDBNull(reader.GetOrdinal("contact_number")) ? "" : reader.GetString(reader.GetOrdinal("contact_number"));
                    steDetails.SamsHoldingEmployee = reader.IsDBNull(reader.GetOrdinal("sams_holding_employee")) ? false : reader.GetBoolean(reader.GetOrdinal("sams_holding_employee"));
                    steDetails.MarketId = reader.IsDBNull(reader.GetOrdinal("market_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("market_id"));
                    steDetails.PropertyHeader = reader.IsDBNull(reader.GetOrdinal("property_header")) ? "" : reader.GetString(reader.GetOrdinal("property_header"));

                    /*
                    if(steDetails.PropertyHeader.Length > 40)
                    {
                        steDetails.PropertyHeader = steDetails.PropertyHeader.Substring(0, 40);
                    }
                    */

                    steDetails.SiteAddress = reader.IsDBNull(reader.GetOrdinal("site_address")) ? "" : reader.GetString(reader.GetOrdinal("site_address"));

                    steDetails.SiteAddress = steDetails.SiteAddress.Replace(", USA", " " + steDetails.ZipCode);

                    /*
                    if(steDetails.SiteAddress.Length > 15)
                    {
                        steDetails.SiteAddress = steDetails.SiteAddress.Substring(0, 15) + "..";
                    }
                    */

                    steDetails.SiteCity = reader.IsDBNull(reader.GetOrdinal("site_city")) ? "" : reader.GetString(reader.GetOrdinal("site_city"));
                    steDetails.SiteStateId = reader.IsDBNull(reader.GetOrdinal("site_state_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("site_state_id"));

                    steDetails.SiteStateName = reader.IsDBNull(reader.GetOrdinal("site_state_name")) ? "" : reader.GetString(reader.GetOrdinal("site_state_name"));

                    steDetails.SiteCounty = reader.IsDBNull(reader.GetOrdinal("site_county")) ? "" : reader.GetString(reader.GetOrdinal("site_county"));
                    steDetails.SiteCrossStreetName = reader.IsDBNull(reader.GetOrdinal("site_cross_street_name")) ? "" : reader.GetString(reader.GetOrdinal("site_cross_street_name"));
                    steDetails.IsPropertyAvailable = reader.IsDBNull(reader.GetOrdinal("is_property_available")) ? true : reader.GetBoolean(reader.GetOrdinal("is_property_available"));
                    steDetails.Zoning = reader.IsDBNull(reader.GetOrdinal("zoning")) ? "" : reader.GetString(reader.GetOrdinal("zoning"));
                    steDetails.LotSize = reader.IsDBNull(reader.GetOrdinal("lot_size")) ? "" : reader.GetString(reader.GetOrdinal("lot_size"));

                    steDetails.SalesPrice = reader.IsDBNull(reader.GetOrdinal("sales_price")) ? "" : reader.GetString(reader.GetOrdinal("sales_price"));
                    steDetails.AskingRent = reader.IsDBNull(reader.GetOrdinal("asking_rent")) ? "" : reader.GetString(reader.GetOrdinal("asking_rent")); 

                    /*
                    if (steDetails.SalesPrice.Length > 10)
                    {
                        steDetails.SalesPrice = steDetails.SalesPrice.Substring(0, 10) + "..";
                    }
                    */
                    

                    steDetails.Comments = reader.IsDBNull(reader.GetOrdinal("comments")) ? "" : reader.GetString(reader.GetOrdinal("comments"));

                    steDetails.CreatedDate = reader.IsDBNull(reader.GetOrdinal("created_date")) ? DateTime.Now : reader.GetDateTime(reader.GetOrdinal("created_date"));
                    steDetails.PropertyType = reader.IsDBNull(reader.GetOrdinal("property_type")) ? 0 : reader.GetInt32(reader.GetOrdinal("property_type"));

                    steDetails.AssetTypeId = reader.IsDBNull(reader.GetOrdinal("asset_type_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("asset_type_id"));

                    if (steDetails.AssetTypeId == 1)
                    {
                        steDetails.AssetTypeName = "Lease/Build To Suit";
                    }
                    else
                    {
                        steDetails.AssetTypeName = reader.IsDBNull(reader.GetOrdinal("asset_type_name")) ? "" : reader.GetString(reader.GetOrdinal("asset_type_name"));
                    }
                    //steDetails.AssetTypeName = reader.IsDBNull(reader.GetOrdinal("asset_type_name")) ? "" : reader.GetString(reader.GetOrdinal("asset_type_name"));

                    steDetails.ImageName = reader.IsDBNull(reader.GetOrdinal("image_name")) ? "" : reader.GetString(reader.GetOrdinal("image_name"));
                    steDetails.SelectedPropertyStatus = reader.IsDBNull(reader.GetOrdinal("property_status")) ? "" : reader.GetString(reader.GetOrdinal("property_status"));
                    
                    steDetails.PropertyHeaderLine2 = reader.IsDBNull(reader.GetOrdinal("property_header_line_2")) ? "" : reader.GetString(reader.GetOrdinal("property_header_line_2"));

                    steDetails.RowNumber = reader.IsDBNull(reader.GetOrdinal("row_number")) ? 0 : reader.GetInt64(reader.GetOrdinal("row_number")); 




                    List<ImageViewModel> propertyImageList = new List<ImageViewModel>();
                    using (SqlConnection conImages = new SqlConnection(CS))
                    {
                        SqlCommand cmdImageList = new SqlCommand("GetPropertyImageList", conImages);
                        cmdImageList.Parameters.AddWithValue("property_id", steDetails.SiteDetailsId);
                        cmdImageList.Parameters.AddWithValue("property_type", SamsPropertyType.Surplus);

                        cmdImageList.CommandType = CommandType.StoredProcedure;
                        conImages.Open();

                        SqlDataReader readerMarket = cmdImageList.ExecuteReader();
                        
                        while (readerMarket.Read())
                        {
                            var imageItem = new ImageViewModel();
                            imageItem.ImageId = readerMarket.IsDBNull(readerMarket.GetOrdinal("image_id")) ? 0 : readerMarket.GetInt32(readerMarket.GetOrdinal("image_id"));
                            imageItem.PropertyId = steDetails.SiteDetailsId;



                            imageItem.ImageName = readerMarket.IsDBNull(readerMarket.GetOrdinal("image_name")) ? "" : readerMarket.GetString(readerMarket.GetOrdinal("image_name"));
                            string pic = @"../../UploadedImage/" + imageItem.ImageName;
                            imageItem.ImageName = pic;
                            propertyImageList.Add(imageItem);
                        }
                        conImages.Close();
                    }
                    if(steDetails.PropertyImageList == null)
                    {
                        var imageItem = new ImageViewModel();
                        imageItem.ImageId = 0;
                        imageItem.PropertyId = steDetails.SiteDetailsId;

                        imageItem.ImageName = "no_image.png?b=1";
                        string pic = @"../../UploadedImage/" + imageItem.ImageName;
                        imageItem.ImageName = pic;
                        propertyImageList.Add(imageItem);
                    }
                    steDetails.PropertyImageList = propertyImageList;
                    surplusPropertiesList.Add(steDetails);

                    

                }
                con.Close();



                SqlCommand cmdTotalCount = new SqlCommand("GetTotalPropertyListByState", con);
                cmdTotalCount.Parameters.AddWithValue("stateId", stateId);
                cmdTotalCount.Parameters.AddWithValue("region_id", regionId);

                cmdTotalCount.CommandType = CommandType.StoredProcedure;
                con.Open();

                SqlDataReader readerTotalCount = cmdTotalCount.ExecuteReader();
                
                while (readerTotalCount.Read())
                {
                    totalCount = readerTotalCount.IsDBNull(readerTotalCount.GetOrdinal("total_records")) ? 0 : readerTotalCount.GetInt32(readerTotalCount.GetOrdinal("total_records"));
                }
                con.Close();
            }


            ViewBag.TotalRecords = totalCount;
            ViewBag.CurrentPage = p;
            ViewBag.SelectedStateId = stateId;

            surplusViewModel.StateList = stateList;
            surplusViewModel.SurplusPropertiesList = surplusPropertiesList;

            CustomerViewModel loggedInUser = HttpContext.Session.GetObjectFromJson<CustomerViewModel>("LoggedInUser");
            if (loggedInUser != null)
            {
                ViewData["LoggedInUserId"] = loggedInUser.CustomerId;
                ViewData["LoggedInUserName"] = loggedInUser.FirstName + " " + loggedInUser.LastName;
            }
            else
            {
                loggedInUser = GetEmptyCustomer();
            }

            surplusViewModel.RegionList = GetRegionList(stateId);

            return View(surplusViewModel);
        }

        public IActionResult GetSurplusProperty(int propertyId)
        {
            
            SiteDetails steDetails = new SiteDetails();

            List<StateDetails> stateList = new List<StateDetails>();
            List<MarketDetails> marketList = new List<MarketDetails>();
            List<AdditionalFilesViewModel> additionalFiles = new List<AdditionalFilesViewModel>();
            List<LeaseTypeModel> leaseTypeList = GetLeaseTypeList();

            // string CS = ConfigurationManager.ConnectionStrings["testConnection"].ConnectionString;
            string CS = DBConnection.ConnectionString;

            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("GetStateList", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var stateDetails = new StateDetails();
                    stateDetails.StateId = reader.IsDBNull(reader.GetOrdinal("state_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("state_id"));
                    stateDetails.StateCode = reader.IsDBNull(reader.GetOrdinal("state_code")) ? "" : reader.GetString(reader.GetOrdinal("state_code"));
                    stateDetails.StateName = reader.IsDBNull(reader.GetOrdinal("state_name")) ? "" : reader.GetString(reader.GetOrdinal("state_name"));
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

                SqlCommand cmdComplianceList = new SqlCommand("GetSurplusFiles", con);

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
                    string pic = @"../../property_files/" + c_storeFile.FileName;
                    c_storeFile.FileName = pic;
                    additionalFiles.Add(c_storeFile);
                }
                con.Close();
            }


            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("GetPropertyItemById", con);

                cmd.Parameters.AddWithValue("site_details_id", propertyId);
                cmd.CommandType = CommandType.StoredProcedure;
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
                    steDetails.PropertyHeader = reader.IsDBNull(reader.GetOrdinal("property_header")) ? "" : reader.GetString(reader.GetOrdinal("property_header"));

                    steDetails.PropertyHeaderLine2 = reader.IsDBNull(reader.GetOrdinal("property_header_line_2")) ? "" : reader.GetString(reader.GetOrdinal("property_header_line_2"));

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

                    steDetails.StateName = reader.IsDBNull(reader.GetOrdinal("state_name")) ? "" : reader.GetString(reader.GetOrdinal("state_name"));
                    steDetails.SiteStateName = reader.IsDBNull(reader.GetOrdinal("site_state_name")) ? "" : reader.GetString(reader.GetOrdinal("site_state_name"));

                    steDetails.AssetTypeId = reader.IsDBNull(reader.GetOrdinal("asset_type_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("asset_type_id"));
                    if (steDetails.AssetTypeId == 1)
                    {
                        steDetails.AssetTypeName = "Lease/Build To Suit";
                    }
                    else
                    {
                        steDetails.AssetTypeName = reader.IsDBNull(reader.GetOrdinal("asset_type_name")) ? "" : reader.GetString(reader.GetOrdinal("asset_type_name"));
                    }

                    steDetails.Latitude = reader.IsDBNull(reader.GetOrdinal("property_latitude")) ? "" : reader.GetString(reader.GetOrdinal("property_latitude"));
                    steDetails.Longitude = reader.IsDBNull(reader.GetOrdinal("property_longitude")) ? "" : reader.GetString(reader.GetOrdinal("property_longitude"));

                    steDetails.SelectedPropertyStatus = reader.IsDBNull(reader.GetOrdinal("property_status")) ? "" : reader.GetString(reader.GetOrdinal("property_status"));

                    steDetails.Term = reader.IsDBNull(reader.GetOrdinal("term")) ? "" : reader.GetString(reader.GetOrdinal("term"));
                    steDetails.AskingRent = reader.IsDBNull(reader.GetOrdinal("asking_rent")) ? "" : reader.GetString(reader.GetOrdinal("asking_rent"));
                    steDetails.LeaseType = reader.IsDBNull(reader.GetOrdinal("lease_type")) ? 0 : reader.GetInt32(reader.GetOrdinal("lease_type"));

                    steDetails.AssetId = reader.IsDBNull(reader.GetOrdinal("asset_id")) ? "" : reader.GetString(reader.GetOrdinal("asset_id"));

                    steDetails.TermOptionPurchase = reader.IsDBNull(reader.GetOrdinal("term_option_purchase")) ? "" : reader.GetString(reader.GetOrdinal("term_option_purchase"));
                    steDetails.AskingRentOptionPurchase = reader.IsDBNull(reader.GetOrdinal("asking_rent_option_purchase")) ? "" : reader.GetString(reader.GetOrdinal("asking_rent_option_purchase"));
                    steDetails.LeaseTypePurchase = reader.IsDBNull(reader.GetOrdinal("lease_type_purchase")) ? 0 : reader.GetInt32(reader.GetOrdinal("lease_type_purchase"));
                    steDetails.OptionPurchasePrice = reader.IsDBNull(reader.GetOrdinal("option_purchase_price")) ? "" : reader.GetString(reader.GetOrdinal("option_purchase_price"));
                }
                con.Close();

                steDetails.StateList = stateList;
                steDetails.MarketList = marketList;
                steDetails.LeaseTypeList = leaseTypeList;

                SqlCommand cmdImageList = new SqlCommand("GetPropertyImageList", con);

                cmdImageList.Parameters.AddWithValue("property_id", propertyId);
                cmdImageList.Parameters.AddWithValue("property_type", SamsPropertyType.Surplus);
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

                if (propertyImageList.Count == 0)
                {
                    var imageItem = new ImageViewModel();
                    imageItem.ImageId = 0;
                    imageItem.PropertyId = steDetails.SiteDetailsId;

                    imageItem.ImageName = "no_image.png?b=1";
                    string pic = @"../../UploadedImage/" + imageItem.ImageName;
                    imageItem.ImageName = pic;
                    propertyImageList.Add(imageItem);
                }

                con.Close();

                CustomerViewModel loggedInUser = HttpContext.Session.GetObjectFromJson<CustomerViewModel>("LoggedInUser");
                if (loggedInUser != null)
                {
                    ViewData["LoggedInUserId"] = loggedInUser.CustomerId;
                    ViewData["LoggedInUserName"] = loggedInUser.FirstName + " " + loggedInUser.LastName;
                }
                else
                {
                    loggedInUser = GetEmptyCustomer();
                }

                SqlCommand cmdSaveHits = new SqlCommand("SavePageHitStatus", con);

                cmdSaveHits.Parameters.AddWithValue("property_id", propertyId);
                cmdSaveHits.Parameters.AddWithValue("property_type", SamsPropertyType.Surplus);

                int customerId = 0;
                if (loggedInUser != null)
                {
                    customerId = loggedInUser.CustomerId;
                }
                cmdSaveHits.Parameters.AddWithValue("customer_id", customerId);
                cmdSaveHits.Parameters.AddWithValue("hit_header", "Viewed Surplus");

                cmdSaveHits.CommandType = CommandType.StoredProcedure;
                con.Open();
                cmdSaveHits.ExecuteNonQuery();
                con.Close();


                steDetails.PropertyImageList = propertyImageList;
                steDetails.AdditionalFiles = additionalFiles;
            }

            

            SamsSettings sSettings = SamsSettingsController.GetSamsSettings();
            steDetails.MySettings = sSettings;

            return View(steDetails);
        }

        List<StateDetails> GetStateList()
        {
            List<StateDetails> stateList = new List<StateDetails>();
            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("GetStateList", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var stateDetails = new StateDetails();
                    stateDetails.StateId = reader.IsDBNull(reader.GetOrdinal("state_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("state_id"));
                    stateDetails.StateCode = reader.IsDBNull(reader.GetOrdinal("state_code")) ? "" : reader.GetString(reader.GetOrdinal("state_code"));
                    stateDetails.StateName = reader.IsDBNull(reader.GetOrdinal("state_name")) ? "" : reader.GetString(reader.GetOrdinal("state_name"));
                    stateList.Add(stateDetails);
                }
                con.Close();


            }
            return stateList;
        }



        public ActionResult net_lease_properties(int stateId, int regionId)
        {
            NetleaseRealEstateViewModel netLeaseRealEstate = new NetleaseRealEstateViewModel();
            List<NetleasePropertiesViewModel> netLeaseList = new List<NetleasePropertiesViewModel>();
            List<StateDetails> stateList = GetStateList();
            List<AdditionalFilesViewModel> additionalFiles = new List<AdditionalFilesViewModel>();

            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("GetNetleasePropertyListByState", con);
                cmd.Parameters.AddWithValue("stateId", stateId);
                cmd.Parameters.AddWithValue("region_id", regionId); 
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var steDetails = new NetleasePropertiesViewModel();
                    steDetails.NetleasePropertyId = reader.IsDBNull(reader.GetOrdinal("net_lease_property_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("net_lease_property_id"));
                    steDetails.AssetId = reader.IsDBNull(reader.GetOrdinal("asset_id")) ? "" : reader.GetString(reader.GetOrdinal("asset_id"));
                    steDetails.AssetName = reader.IsDBNull(reader.GetOrdinal("asset_name")) ? "" : reader.GetString(reader.GetOrdinal("asset_name"));
                    
                    /*
                    if(steDetails.AssetName.Length > 40)
                    {
                        steDetails.AssetName = steDetails.AssetName.Substring(0, 40);
                    }
                    */
                    

                    steDetails.StateId = reader.IsDBNull(reader.GetOrdinal("state_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("state_id"));
                    steDetails.City = reader.IsDBNull(reader.GetOrdinal("city")) ? "" : reader.GetString(reader.GetOrdinal("city"));

                    steDetails.PropertyPrice = reader.IsDBNull(reader.GetOrdinal("property_price")) ? "" : reader.GetString(reader.GetOrdinal("property_price"));
                    steDetails.AskingRent = reader.IsDBNull(reader.GetOrdinal("asking_rent")) ? "" : reader.GetString(reader.GetOrdinal("asking_rent"));
                    steDetails.RentalIncome = reader.IsDBNull(reader.GetOrdinal("rental_income")) ? "" : reader.GetString(reader.GetOrdinal("rental_income"));

                    /*
                    if (steDetails.PropertyPrice.Length > 10)
                    {
                        steDetails.PropertyPrice = steDetails.PropertyPrice.Substring(0, 10) + "..";
                    }
                    */

                    steDetails.CapRate = reader.IsDBNull(reader.GetOrdinal("cap_rate")) ? 0 : reader.GetDouble(reader.GetOrdinal("cap_rate"));

                    steDetails.Term = reader.IsDBNull(reader.GetOrdinal("term")) ? "" : reader.GetString(reader.GetOrdinal("term"));

                    steDetails.PdfFileName = reader.IsDBNull(reader.GetOrdinal("detail_pdf")) ? "" : reader.GetString(reader.GetOrdinal("detail_pdf"));
                    steDetails.StateName = reader.IsDBNull(reader.GetOrdinal("state_name")) ? "" : reader.GetString(reader.GetOrdinal("state_name"));

                    steDetails.CreatedDate = reader.IsDBNull(reader.GetOrdinal("created_date")) ? DateTime.Now : reader.GetDateTime(reader.GetOrdinal("created_date"));
                    steDetails.AssetTypeId = reader.IsDBNull(reader.GetOrdinal("asset_type_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("asset_type_id"));
                    steDetails.AssetTypeName = reader.IsDBNull(reader.GetOrdinal("asset_type_name")) ? "" : reader.GetString(reader.GetOrdinal("asset_type_name"));

                    steDetails.Address = reader.IsDBNull(reader.GetOrdinal("property_address")) ? "" : reader.GetString(reader.GetOrdinal("property_address"));
                    steDetails.PropertyHeaderLine2 = reader.IsDBNull(reader.GetOrdinal("property_header_line_2")) ? "" : reader.GetString(reader.GetOrdinal("property_header_line_2"));

                    /*
                    if (steDetails.Address.Length > 15)
                    {
                        steDetails.Address = steDetails.Address.Substring(0, 15) + "..";
                    }
                    */
                    steDetails.Address = steDetails.Address.Replace(", USA", " " + steDetails.ZipCode);

                    steDetails.ZipCode = reader.IsDBNull(reader.GetOrdinal("property_zipcode")) ? "" : reader.GetString(reader.GetOrdinal("property_zipcode"));

                    steDetails.SelectedPropertyStatus = reader.IsDBNull(reader.GetOrdinal("property_status")) ? "" : reader.GetString(reader.GetOrdinal("property_status"));

                    using (SqlConnection conFiles = new SqlConnection(CS))
                    {
                        SqlCommand cmdComplianceList = new SqlCommand("GetNetLeaseFiles", conFiles);

                        cmdComplianceList.Parameters.AddWithValue("property_id", steDetails.NetleasePropertyId);
                        cmdComplianceList.CommandType = CommandType.StoredProcedure;
                        conFiles.Open();

                        SqlDataReader readerComplianceList = cmdComplianceList.ExecuteReader();

                        while (readerComplianceList.Read())
                        {
                            var c_storeFile = new AdditionalFilesViewModel();
                            c_storeFile.FileId = readerComplianceList.IsDBNull(readerComplianceList.GetOrdinal("file_id")) ? 0 : readerComplianceList.GetInt32(readerComplianceList.GetOrdinal("file_id"));
                            c_storeFile.PropertyId = steDetails.NetleasePropertyId;
                            c_storeFile.FileType = readerComplianceList.IsDBNull(readerComplianceList.GetOrdinal("file_type")) ? "" : readerComplianceList.GetString(readerComplianceList.GetOrdinal("file_type"));


                            c_storeFile.FileName = readerComplianceList.IsDBNull(readerComplianceList.GetOrdinal("file_name")) ? "" : readerComplianceList.GetString(readerComplianceList.GetOrdinal("file_name"));
                            string pic = @"../../property_files/" + c_storeFile.FileName;
                            c_storeFile.FileName = pic;
                            additionalFiles.Add(c_storeFile);
                        }
                        conFiles.Close();
                    }

                    List<ImageViewModel> propertyImageList = new List<ImageViewModel>();
                    using (SqlConnection conImage = new SqlConnection(CS))
                    {
                        SqlCommand cmdImageList = new SqlCommand("GetPropertyImageList", conImage);

                        cmdImageList.Parameters.AddWithValue("property_id", steDetails.NetleasePropertyId);
                        cmdImageList.Parameters.AddWithValue("property_type", SamsPropertyType.NetLease);

                        cmdImageList.CommandType = CommandType.StoredProcedure;
                        conImage.Open();

                        SqlDataReader readerMarket = cmdImageList.ExecuteReader();
                        
                        while (readerMarket.Read())
                        {
                            var imageItem = new ImageViewModel();
                            imageItem.ImageId = readerMarket.IsDBNull(readerMarket.GetOrdinal("image_id")) ? 0 : readerMarket.GetInt32(readerMarket.GetOrdinal("image_id"));
                            imageItem.PropertyId = steDetails.NetleasePropertyId;



                            imageItem.ImageName = readerMarket.IsDBNull(readerMarket.GetOrdinal("image_name")) ? "" : readerMarket.GetString(readerMarket.GetOrdinal("image_name"));
                            string pic = "";
                            if (imageItem.ImageName.Trim().Length > 0)
                            {
                                pic = @"../../UploadedImage/" + imageItem.ImageName;
                            }
                             
                            imageItem.ImageName = pic;
                            propertyImageList.Add(imageItem);
                        }
                        conImage.Close();
                    }
                    if (propertyImageList.Count == 0)
                    {
                        var imageItem = new ImageViewModel();
                        imageItem.ImageId = 0;
                        imageItem.PropertyId = steDetails.NetleasePropertyId;

                        imageItem.ImageName = "no_image.png?b=1";
                        string pic = @"../../UploadedImage/" + imageItem.ImageName;
                        imageItem.ImageName = pic;
                        propertyImageList.Add(imageItem);
                    }
                    steDetails.ImageList = propertyImageList;
                    steDetails.AdditionalFilesList = additionalFiles;

                    netLeaseList.Add(steDetails);
                }
                con.Close();
            }
            netLeaseRealEstate.NetLeasePropertyList = netLeaseList;
            netLeaseRealEstate.StateList = stateList;
            netLeaseRealEstate.RegionList = GetRegionList(stateId);

            CustomerViewModel loggedInUser = HttpContext.Session.GetObjectFromJson<CustomerViewModel>("LoggedInUser");
            if (loggedInUser != null)
            {
                ViewData["LoggedInUserId"] = loggedInUser.CustomerId;
                ViewData["LoggedInUserName"] = loggedInUser.FirstName + " " + loggedInUser.LastName;
            }
            else
            {
                loggedInUser = GetEmptyCustomer();
            }

            return View(netLeaseRealEstate);
        }



        public ActionResult shopping_center_list_state_wise(int stateId)
        {
            NetleaseRealEstateViewModel netLeaseRealEstate = new NetleaseRealEstateViewModel();
            List<NetleasePropertiesViewModel> netLeaseList = new List<NetleasePropertiesViewModel>();
            List<StateDetails> stateList = GetStateList();
            List<AdditionalFilesViewModel> additionalFiles = new List<AdditionalFilesViewModel>();

            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("GetNetLeaseShoppingCenterListByState", con);
                cmd.Parameters.AddWithValue("stateId", stateId);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var steDetails = new NetleasePropertiesViewModel();
                    steDetails.NetleasePropertyId = reader.IsDBNull(reader.GetOrdinal("net_lease_property_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("net_lease_property_id"));
                    steDetails.AssetId = reader.IsDBNull(reader.GetOrdinal("asset_id")) ? "" : reader.GetString(reader.GetOrdinal("asset_id"));
                    steDetails.AssetName = reader.IsDBNull(reader.GetOrdinal("asset_name")) ? "" : reader.GetString(reader.GetOrdinal("asset_name"));

                    if (steDetails.AssetName.Length > 25)
                    {
                        steDetails.AssetName = steDetails.AssetName.Substring(0, 25) + "..";
                    }


                    steDetails.StateId = reader.IsDBNull(reader.GetOrdinal("state_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("state_id"));
                    steDetails.City = reader.IsDBNull(reader.GetOrdinal("city")) ? "" : reader.GetString(reader.GetOrdinal("city"));

                    steDetails.PropertyPrice = reader.IsDBNull(reader.GetOrdinal("property_price")) ? "" : reader.GetString(reader.GetOrdinal("property_price"));
                    steDetails.AskingRent = reader.IsDBNull(reader.GetOrdinal("asking_rent")) ? "" : reader.GetString(reader.GetOrdinal("asking_rent"));

                    /*
                    if (steDetails.PropertyPrice.Length > 10)
                    {
                        steDetails.PropertyPrice = steDetails.PropertyPrice.Substring(0, 10) + "..";
                    }
                    */

                    steDetails.CapRate = reader.IsDBNull(reader.GetOrdinal("cap_rate")) ? 0 : reader.GetDouble(reader.GetOrdinal("cap_rate"));

                    steDetails.Term = reader.IsDBNull(reader.GetOrdinal("term")) ? "" : reader.GetString(reader.GetOrdinal("term"));

                    steDetails.PdfFileName = reader.IsDBNull(reader.GetOrdinal("detail_pdf")) ? "" : reader.GetString(reader.GetOrdinal("detail_pdf"));
                    steDetails.StateName = reader.IsDBNull(reader.GetOrdinal("state_name")) ? "" : reader.GetString(reader.GetOrdinal("state_name"));

                    steDetails.CreatedDate = reader.IsDBNull(reader.GetOrdinal("created_date")) ? DateTime.Now : reader.GetDateTime(reader.GetOrdinal("created_date"));
                    steDetails.AssetTypeId = reader.IsDBNull(reader.GetOrdinal("asset_type_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("asset_type_id"));
                    steDetails.AssetTypeName = reader.IsDBNull(reader.GetOrdinal("asset_type_name")) ? "" : reader.GetString(reader.GetOrdinal("asset_type_name"));
                    
                    steDetails.Address= reader.IsDBNull(reader.GetOrdinal("property_address")) ? "" : reader.GetString(reader.GetOrdinal("property_address"));

                    if (steDetails.Address.Length > 15)
                    {
                        steDetails.Address = steDetails.Address.Substring(0, 15) + "..";
                    }

                    steDetails.ZipCode = reader.IsDBNull(reader.GetOrdinal("property_zipcode")) ? "" : reader.GetString(reader.GetOrdinal("property_zipcode"));

                    using (SqlConnection conFiles = new SqlConnection(CS))
                    {
                        SqlCommand cmdComplianceList = new SqlCommand("GetNetLeaseFiles", conFiles);

                        cmdComplianceList.Parameters.AddWithValue("property_id", steDetails.NetleasePropertyId);
                        cmdComplianceList.CommandType = CommandType.StoredProcedure;
                        conFiles.Open();

                        SqlDataReader readerComplianceList = cmdComplianceList.ExecuteReader();

                        while (readerComplianceList.Read())
                        {
                            var c_storeFile = new AdditionalFilesViewModel();
                            c_storeFile.FileId = readerComplianceList.IsDBNull(readerComplianceList.GetOrdinal("file_id")) ? 0 : readerComplianceList.GetInt32(readerComplianceList.GetOrdinal("file_id"));
                            c_storeFile.PropertyId = steDetails.NetleasePropertyId;
                            c_storeFile.FileType = readerComplianceList.IsDBNull(readerComplianceList.GetOrdinal("file_type")) ? "" : readerComplianceList.GetString(readerComplianceList.GetOrdinal("file_type"));


                            c_storeFile.FileName = readerComplianceList.IsDBNull(readerComplianceList.GetOrdinal("file_name")) ? "" : readerComplianceList.GetString(readerComplianceList.GetOrdinal("file_name"));
                            string pic = @"../../property_files/" + c_storeFile.FileName;
                            c_storeFile.FileName = pic;
                            additionalFiles.Add(c_storeFile);
                        }
                        conFiles.Close();
                    }

                    List<ImageViewModel> propertyImageList = new List<ImageViewModel>();
                    using (SqlConnection conImage = new SqlConnection(CS))
                    {
                        SqlCommand cmdImageList = new SqlCommand("GetPropertyImageList", conImage);

                        cmdImageList.Parameters.AddWithValue("property_id", steDetails.NetleasePropertyId);
                        cmdImageList.Parameters.AddWithValue("property_type", SamsPropertyType.NetLease);

                        cmdImageList.CommandType = CommandType.StoredProcedure;
                        conImage.Open();

                        SqlDataReader readerMarket = cmdImageList.ExecuteReader();

                        while (readerMarket.Read())
                        {
                            var imageItem = new ImageViewModel();
                            imageItem.ImageId = readerMarket.IsDBNull(readerMarket.GetOrdinal("image_id")) ? 0 : readerMarket.GetInt32(readerMarket.GetOrdinal("image_id"));
                            imageItem.PropertyId = steDetails.NetleasePropertyId;



                            imageItem.ImageName = readerMarket.IsDBNull(readerMarket.GetOrdinal("image_name")) ? "" : readerMarket.GetString(readerMarket.GetOrdinal("image_name"));
                            string pic = @"../../UploadedImage/" + imageItem.ImageName;
                            imageItem.ImageName = pic;
                            propertyImageList.Add(imageItem);
                        }
                        conImage.Close();
                    }
                    if (propertyImageList.Count == 0)
                    {
                        var imageItem = new ImageViewModel();
                        imageItem.ImageId = 0;
                        imageItem.PropertyId = steDetails.NetleasePropertyId;

                        imageItem.ImageName = "no_image.png?b=1";
                        string pic = @"../../UploadedImage/" + imageItem.ImageName;
                        imageItem.ImageName = pic;
                        propertyImageList.Add(imageItem);
                    }
                    steDetails.ImageList = propertyImageList;
                    steDetails.AdditionalFilesList = additionalFiles;

                    
                    netLeaseList.Add(steDetails);
                }
                con.Close();
            }
            netLeaseRealEstate.NetLeasePropertyList = netLeaseList;
            netLeaseRealEstate.StateList = stateList;

            CustomerViewModel loggedInUser = HttpContext.Session.GetObjectFromJson<CustomerViewModel>("LoggedInUser");
            if (loggedInUser != null)
            {
                ViewData["LoggedInUserId"] = loggedInUser.CustomerId;
                ViewData["LoggedInUserName"] = loggedInUser.FirstName + " " + loggedInUser.LastName;
            }
            else
            {
                loggedInUser = GetEmptyCustomer();
            }

            return View(netLeaseRealEstate);
        }



        public IActionResult ViewNetLeaseProperty(int propertyId)
        {
            var steDetails = new NetleasePropertiesViewModel();

            List<StateDetails> stateList = GetStateList();
            List<AdditionalFilesViewModel> additionalFiles = new List<AdditionalFilesViewModel>();
            
            CustomerViewModel loggedInUser = HttpContext.Session.GetObjectFromJson<CustomerViewModel>("LoggedInUser");
            if (loggedInUser != null)
            {
                ViewData["LoggedInUserId"] = loggedInUser.CustomerId;
                ViewData["LoggedInUserName"] = loggedInUser.FirstName + " " + loggedInUser.LastName;
            }
            else
            {
                loggedInUser = GetEmptyCustomer();
            }
            steDetails.LoggedInUser = loggedInUser;

            string CS = DBConnection.ConnectionString;

            using (SqlConnection con = new SqlConnection(CS))
            {
                


                SqlCommand cmdComplianceList = new SqlCommand("GetNetLeaseFiles", con);

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
                    string pic = @"../../property_files/" + c_storeFile.FileName;
                    c_storeFile.FileName = pic;
                    additionalFiles.Add(c_storeFile);
                }
                con.Close();
            }

            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("GetNetleasePropertyById", con);

                cmd.Parameters.AddWithValue("net_lease_property_id", propertyId);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    steDetails.NetleasePropertyId = reader.IsDBNull(reader.GetOrdinal("net_lease_property_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("net_lease_property_id"));
                    steDetails.AssetId = reader.IsDBNull(reader.GetOrdinal("asset_id")) ? "" : reader.GetString(reader.GetOrdinal("asset_id"));
                    steDetails.AssetName = reader.IsDBNull(reader.GetOrdinal("asset_name")) ? "" : reader.GetString(reader.GetOrdinal("asset_name"));
                    steDetails.NetleaseAssetName = reader.IsDBNull(reader.GetOrdinal("netlease_asset_name")) ? "" : reader.GetString(reader.GetOrdinal("netlease_asset_name"));
                    steDetails.StateId = reader.IsDBNull(reader.GetOrdinal("state_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("state_id"));
                    steDetails.City = reader.IsDBNull(reader.GetOrdinal("city")) ? "" : reader.GetString(reader.GetOrdinal("city"));

                    steDetails.PropertyPrice = reader.IsDBNull(reader.GetOrdinal("property_price")) ? "" : reader.GetString(reader.GetOrdinal("property_price"));
                    steDetails.CapRate = reader.IsDBNull(reader.GetOrdinal("cap_rate")) ? 0 : reader.GetDouble(reader.GetOrdinal("cap_rate"));


                    steDetails.Term = reader.IsDBNull(reader.GetOrdinal("term")) ? "" : reader.GetString(reader.GetOrdinal("term"));
                    steDetails.AskingRent = reader.IsDBNull(reader.GetOrdinal("asking_rent")) ? "" : reader.GetString(reader.GetOrdinal("asking_rent"));

                    steDetails.PdfFileName = reader.IsDBNull(reader.GetOrdinal("detail_pdf")) ? "" : reader.GetString(reader.GetOrdinal("detail_pdf"));
                    steDetails.StateName = reader.IsDBNull(reader.GetOrdinal("state_name")) ? "" : reader.GetString(reader.GetOrdinal("state_name"));

                    steDetails.CreatedDate = reader.IsDBNull(reader.GetOrdinal("created_date")) ? DateTime.Now : reader.GetDateTime(reader.GetOrdinal("created_date"));

                    steDetails.AssetTypeId = reader.IsDBNull(reader.GetOrdinal("asset_type_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("asset_type_id"));
                    steDetails.AssetTypeName = reader.IsDBNull(reader.GetOrdinal("asset_type_name")) ? "" : reader.GetString(reader.GetOrdinal("asset_type_name"));

                    steDetails.Address = reader.IsDBNull(reader.GetOrdinal("property_address")) ? "" : reader.GetString(reader.GetOrdinal("property_address"));
                    steDetails.ZipCode = reader.IsDBNull(reader.GetOrdinal("property_zipcode")) ? "" : reader.GetString(reader.GetOrdinal("property_zipcode"));

                    string fileName = "";
                    if (steDetails.PdfFileName.Trim().Length > 0)
                    {
                        steDetails.SelectedPdfFileName = steDetails.PdfFileName;
                        if (steDetails.PdfFileName.ToLower().Contains(".pdf"))
                        {
                            steDetails.FileType = "pdf";
                        }
                        else if (steDetails.PdfFileName.ToLower().Contains(".jpg") || steDetails.PdfFileName.ToLower().Contains(".jpeg") || steDetails.PdfFileName.ToLower().Contains(".png"))
                        {
                            steDetails.FileType = "image";
                        }
                        else
                        {
                            steDetails.FileType = "others";
                        }
                        fileName = @"../../UploadedPdf/" + steDetails.PdfFileName;

                    }
                    
                    steDetails.PdfFileName = fileName;

                    steDetails.SavedShoppingMartPlanFileName = reader.IsDBNull(reader.GetOrdinal("shopping_mart_plan_file_name")) ? "" : "OtherFiles/" + reader.GetString(reader.GetOrdinal("shopping_mart_plan_file_name"));

                    steDetails.Latitude = reader.IsDBNull(reader.GetOrdinal("property_latitude")) ? "" : reader.GetString(reader.GetOrdinal("property_latitude"));
                    steDetails.Longitude = reader.IsDBNull(reader.GetOrdinal("property_longitude")) ? "" : reader.GetString(reader.GetOrdinal("property_longitude"));

                    steDetails.SelectedPropertyStatus = reader.IsDBNull(reader.GetOrdinal("property_status")) ? "" : reader.GetString(reader.GetOrdinal("property_status"));

                    steDetails.TermRemaining = reader.IsDBNull(reader.GetOrdinal("term_remaining")) ? "" : reader.GetString(reader.GetOrdinal("term_remaining"));
                    steDetails.RentalIncome = reader.IsDBNull(reader.GetOrdinal("rental_income")) ? "" : reader.GetString(reader.GetOrdinal("rental_income"));

                    steDetails.LeaseTypeName = reader.IsDBNull(reader.GetOrdinal("lease_type_name")) ? "" : reader.GetString(reader.GetOrdinal("lease_type_name"));
                    

                    steDetails.LeaseTypeLeaseAndFee = reader.IsDBNull(reader.GetOrdinal("lease_type_net_lease")) ? 0 : reader.GetInt32(reader.GetOrdinal("lease_type_net_lease"));
                    steDetails.LeaseTypeLeaseAndFeeName = reader.IsDBNull(reader.GetOrdinal("lease_type_net_lease_name")) ? "" : reader.GetString(reader.GetOrdinal("lease_type_net_lease_name"));

                    steDetails.Details = reader.IsDBNull(reader.GetOrdinal("details")) ? "" : reader.GetString(reader.GetOrdinal("details"));

                    steDetails.IsShoppingCenter = reader.IsDBNull(reader.GetOrdinal("is_shopping_center")) ? false : reader.GetBoolean(reader.GetOrdinal("is_shopping_center"));

                    steDetails.TermOptionPurchase = reader.IsDBNull(reader.GetOrdinal("term_option_purchase")) ? "" : reader.GetString(reader.GetOrdinal("term_option_purchase"));
                    steDetails.AskingRentOptionPurchase = reader.IsDBNull(reader.GetOrdinal("asking_rent_option_purchase")) ? "" : reader.GetString(reader.GetOrdinal("asking_rent_option_purchase"));
                    steDetails.LeaseTypePurchase = reader.IsDBNull(reader.GetOrdinal("lease_type_purchase")) ? 0 : reader.GetInt32(reader.GetOrdinal("lease_type_purchase"));
                    steDetails.OptionPurchasePrice = reader.IsDBNull(reader.GetOrdinal("option_purchase_price")) ? "" : reader.GetString(reader.GetOrdinal("option_purchase_price"));

                    steDetails.PropertyHeaderLine2 = reader.IsDBNull(reader.GetOrdinal("property_header_line_2")) ? "" : reader.GetString(reader.GetOrdinal("property_header_line_2"));

                    steDetails.LeaseTypeList = GetLeaseTypeList();
                }
                con.Close();


                SqlCommand cmdImageList = new SqlCommand("GetPropertyImageList", con);

                cmdImageList.Parameters.AddWithValue("property_id", propertyId);
                cmdImageList.Parameters.AddWithValue("property_type", SamsPropertyType.NetLease);

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
                    string pic = "";
                    if (imageItem.ImageName.Trim().Length > 0)
                    {
                        pic = @"../../UploadedImage/" + imageItem.ImageName;
                    }
                    //string pic = @"../../UploadedImage/" + imageItem.ImageName;

                    imageItem.ImageName = pic;
                    propertyImageList.Add(imageItem);
                }
                con.Close();

                if (propertyImageList.Count == 0)
                {
                    var imageItem = new ImageViewModel();
                    imageItem.ImageId = 0;
                    imageItem.PropertyId = steDetails.NetleasePropertyId;
                    
                    imageItem.ImageName = "no_image.png?b=1";
                    string pic = @"../../UploadedImage/" + imageItem.ImageName;
                    imageItem.ImageName = pic;
                    propertyImageList.Add(imageItem);

                }

                

                SqlCommand cmdSaveHits = new SqlCommand("SavePageHitStatus", con);

                cmdSaveHits.Parameters.AddWithValue("property_id", propertyId);
                if (steDetails.IsShoppingCenter)
                {
                    cmdSaveHits.Parameters.AddWithValue("property_type", SamsPropertyType.ShoppingCenter);
                    cmdSaveHits.Parameters.AddWithValue("hit_header", "Viewed Shopping Center");
                }
                else
                {
                    cmdSaveHits.Parameters.AddWithValue("property_type", SamsPropertyType.NetLease);
                    cmdSaveHits.Parameters.AddWithValue("hit_header", "Viewed Netlease");
                }
                int customerId = 0;
                if (loggedInUser != null)
                {
                    customerId = loggedInUser.CustomerId;
                }
                cmdSaveHits.Parameters.AddWithValue("customer_id", customerId);
                

                cmdSaveHits.CommandType = CommandType.StoredProcedure;
                con.Open();
                cmdSaveHits.ExecuteNonQuery();
                con.Close();


                steDetails.ImageList = propertyImageList;

                
                steDetails.ImageList = propertyImageList;
                steDetails.AdditionalFilesList = additionalFiles;

                SqlCommand cmdComplianceList = new SqlCommand("GetNetleaseComplianceFiles", con);
                cmdComplianceList.Parameters.AddWithValue("property_id", propertyId);
                cmdComplianceList.CommandType = CommandType.StoredProcedure;
                con.Open();

                SqlDataReader readerComplianceList = cmdComplianceList.ExecuteReader();
                List<AdditionalFilesViewModel> confidentialFiles = new List<AdditionalFilesViewModel>();
                while (readerComplianceList.Read())
                {
                    var netleaseFile = new AdditionalFilesViewModel();
                    netleaseFile.FileId = readerComplianceList.IsDBNull(readerComplianceList.GetOrdinal("file_id")) ? 0 : readerComplianceList.GetInt32(readerComplianceList.GetOrdinal("file_id"));
                    netleaseFile.PropertyId = propertyId;
                    netleaseFile.FileType = readerComplianceList.IsDBNull(readerComplianceList.GetOrdinal("file_type")) ? "" : readerComplianceList.GetString(readerComplianceList.GetOrdinal("file_type"));


                    netleaseFile.FileName = readerComplianceList.IsDBNull(readerComplianceList.GetOrdinal("file_name")) ? "" : readerComplianceList.GetString(readerComplianceList.GetOrdinal("file_name"));
                    string pic = @"../../property_files/" + netleaseFile.FileName;
                    netleaseFile.FileName = pic;
                    confidentialFiles.Add(netleaseFile);
                }
                con.Close();
                steDetails.NDAComplaintsFilesList = confidentialFiles;
            }

            SamsSettings sSettings = SamsSettingsController.GetSamsSettings();
            steDetails.MySettings = sSettings;

            return View(steDetails);
        }

        public ActionResult c_store_list(int stateId, int regionId)
        {
            CStoreRealEstateViewModel cStoreRealEstateViewModel = new CStoreRealEstateViewModel();
            List<CStoreViewModel> cstoreList = new List<CStoreViewModel>();
            List<StateDetails> stateList = GetStateList();

            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {

                SqlCommand cmd = new SqlCommand("GetCStoreListByState", con);
                cmd.Parameters.AddWithValue("stateId", stateId);
                cmd.Parameters.AddWithValue("region_id", regionId);

                cmd.CommandType = CommandType.StoredProcedure;

                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var steDetails = new CStoreViewModel();

                    steDetails.CStoreId = reader.IsDBNull(reader.GetOrdinal("c_store_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("c_store_id"));
                    steDetails.PropertyHeader = reader.IsDBNull(reader.GetOrdinal("property_header")) ? "" : reader.GetString(reader.GetOrdinal("property_header"));
                    
                    /*
                    if(steDetails.PropertyHeader.Length > 40)
                    {
                        steDetails.PropertyHeader = steDetails.PropertyHeader.Substring(0, 40);
                    }
                    */

                    steDetails.StateId = reader.IsDBNull(reader.GetOrdinal("state_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("state_id"));
                    steDetails.StateName = reader.IsDBNull(reader.GetOrdinal("state_name")) ? "" : reader.GetString(reader.GetOrdinal("state_name"));
                    steDetails.City = reader.IsDBNull(reader.GetOrdinal("city")) ? "" : reader.GetString(reader.GetOrdinal("city"));
                    steDetails.Zipcode = reader.IsDBNull(reader.GetOrdinal("zipcode")) ? "" : reader.GetString(reader.GetOrdinal("zipcode"));

                    steDetails.County = reader.IsDBNull(reader.GetOrdinal("county")) ? "" : reader.GetString(reader.GetOrdinal("county"));
                    steDetails.AssetId = reader.IsDBNull(reader.GetOrdinal("asset_id")) ? "" : reader.GetString(reader.GetOrdinal("asset_id"));

                    

                    steDetails.PropertyTypeId = reader.IsDBNull(reader.GetOrdinal("property_type_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("property_type_id"));

                    steDetails.PropertyTypeName = reader.IsDBNull(reader.GetOrdinal("property_type_name")) ? "" : reader.GetString(reader.GetOrdinal("property_type_name"));
                    steDetails.PropertyTaxes = reader.IsDBNull(reader.GetOrdinal("property_taxes")) ? "" : reader.GetString(reader.GetOrdinal("property_taxes"));

                    steDetails.Description = reader.IsDBNull(reader.GetOrdinal("property_description")) ? "" : reader.GetString(reader.GetOrdinal("property_description"));

                    steDetails.AskingPrice = reader.IsDBNull(reader.GetOrdinal("asking_price")) ? "" : reader.GetString(reader.GetOrdinal("asking_price"));
                    steDetails.AskingPriceString = reader.IsDBNull(reader.GetOrdinal("asking_price_string")) ? "" : reader.GetString(reader.GetOrdinal("asking_price_string"));
                    steDetails.AskingRent = reader.IsDBNull(reader.GetOrdinal("asking_rent")) ? "" : reader.GetString(reader.GetOrdinal("asking_rent"));

                    steDetails.AssetTypeId = reader.IsDBNull(reader.GetOrdinal("asset_type_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("asset_type_id"));
                    steDetails.AssetTypeName = reader.IsDBNull(reader.GetOrdinal("asset_type_name")) ? "" : reader.GetString(reader.GetOrdinal("asset_type_name"));
                    steDetails.LandSize = reader.IsDBNull(reader.GetOrdinal("land_size")) ? "" : reader.GetString(reader.GetOrdinal("land_size"));
                    steDetails.BuildingArea = reader.IsDBNull(reader.GetOrdinal("building_area")) ? "" : reader.GetString(reader.GetOrdinal("building_area"));

                    steDetails.YearBuilt = reader.IsDBNull(reader.GetOrdinal("year_built")) ? "" : reader.GetString(reader.GetOrdinal("year_built"));
                    steDetails.KnownEnvironmentalConditions = reader.IsDBNull(reader.GetOrdinal("known_environmental_conditions")) ? "" : reader.GetString(reader.GetOrdinal("known_environmental_conditions"));
                    steDetails.EMVCompliance = reader.IsDBNull(reader.GetOrdinal("emv_copliance")) ? "" : reader.GetString(reader.GetOrdinal("emv_copliance"));

                    steDetails.HoursOfOperation = reader.IsDBNull(reader.GetOrdinal("hours_of_operation")) ? "" : reader.GetString(reader.GetOrdinal("hours_of_operation"));
                    steDetails.CreatedDate = reader.IsDBNull(reader.GetOrdinal("created_date")) ? DateTime.Now : reader.GetDateTime(reader.GetOrdinal("created_date"));
                    steDetails.EnvironentNDAPdfFileName = reader.IsDBNull(reader.GetOrdinal("environent_nda_pdf_filename")) ? "" : reader.GetString(reader.GetOrdinal("environent_nda_pdf_filename"));
                    steDetails.Address = reader.IsDBNull(reader.GetOrdinal("c_store_address")) ? "" : reader.GetString(reader.GetOrdinal("c_store_address"));

                    steDetails.Address = steDetails.Address.Replace(", USA", " " + steDetails.Zipcode);

                    steDetails.SelectedPropertyStatus = reader.IsDBNull(reader.GetOrdinal("property_status")) ? "" : reader.GetString(reader.GetOrdinal("property_status"));

                    steDetails.PropertyHeaderLine2 = reader.IsDBNull(reader.GetOrdinal("property_header_line_2")) ? "" : reader.GetString(reader.GetOrdinal("property_header_line_2"));

                    if (steDetails.AssetTypeId == (int)SamAssetType.Lease)
                    {
                        steDetails.AskingPrice = steDetails.AskingPriceString;
                    }

                    List<ImageViewModel> propertyImageList = new List<ImageViewModel>();
                    using (SqlConnection conImages = new SqlConnection(CS))
                    {
                        SqlCommand cmdImageList = new SqlCommand("GetPropertyImageList", conImages);
                        cmdImageList.Parameters.AddWithValue("property_id", steDetails.CStoreId);
                        cmdImageList.Parameters.AddWithValue("property_type", SamsPropertyType.C_Store);

                        cmdImageList.CommandType = CommandType.StoredProcedure;
                        conImages.Open();

                        SqlDataReader readerMarket = cmdImageList.ExecuteReader();

                        while (readerMarket.Read())
                        {
                            var imageItem = new ImageViewModel();
                            imageItem.ImageId = readerMarket.IsDBNull(readerMarket.GetOrdinal("image_id")) ? 0 : readerMarket.GetInt32(readerMarket.GetOrdinal("image_id"));
                            imageItem.PropertyId = steDetails.CStoreId;



                            imageItem.ImageName = readerMarket.IsDBNull(readerMarket.GetOrdinal("image_name")) ? "" : readerMarket.GetString(readerMarket.GetOrdinal("image_name"));
                            string pic = @"../../UploadedImage/" + imageItem.ImageName;
                            imageItem.ImageName = pic;
                            propertyImageList.Add(imageItem);
                        }
                        conImages.Close();
                    }
                    if (propertyImageList.Count == 0)
                    {
                        var imageItem = new ImageViewModel();
                        imageItem.ImageId = 0;
                        imageItem.PropertyId = steDetails.CStoreId;

                        imageItem.ImageName = "no_image.png?b=1";
                        string pic = @"../../UploadedImage/" + imageItem.ImageName;
                        imageItem.ImageName = pic;
                        propertyImageList.Add(imageItem);
                    }

                    

                    steDetails.ImageList = propertyImageList;
                    
                    cstoreList.Add(steDetails);
                }
                con.Close();
            }

            cStoreRealEstateViewModel.CStoreList = cstoreList;
            cStoreRealEstateViewModel.StateList = stateList;
            cStoreRealEstateViewModel.RegionList = GetRegionList(stateId);

            CustomerViewModel loggedInUser = HttpContext.Session.GetObjectFromJson<CustomerViewModel>("LoggedInUser");
            if (loggedInUser != null)
            {
                ViewData["LoggedInUserId"] = loggedInUser.CustomerId;
                ViewData["LoggedInUserName"] = loggedInUser.FirstName + " " + loggedInUser.LastName;
            }
            else
            {
                loggedInUser = GetEmptyCustomer();
            }

            return View(cStoreRealEstateViewModel);
        }

        public IActionResult ViewCStore(int propertyId)
        {
            var steDetails = new CStoreViewModel();

            List<StateDetails> stateList = new List<StateDetails>();
            List<AssetTypeViewModel> assetTypeList = new List<AssetTypeViewModel>();
            List<PropertyTypeViewModel> propertyTypeList = new List<PropertyTypeViewModel>();
            List<LeaseTypeModel> leaseTypeList = GetLeaseTypeList();

            //CustomerViewModel loggedInUser = TempData.Get<CustomerViewModel>("LoggedInUser");
            CustomerViewModel loggedInUser = HttpContext.Session.GetObjectFromJson<CustomerViewModel>("LoggedInUser");


            if (loggedInUser != null)
            {
                ViewData["LoggedInUserId"] = loggedInUser.CustomerId;
                ViewData["LoggedInUserName"] = loggedInUser.FirstName + " " + loggedInUser.LastName;
            }
            else
            {
                loggedInUser = GetEmptyCustomer();
            }
            steDetails.LoggedInUser = loggedInUser;

            // string CS = ConfigurationManager.ConnectionStrings["testConnection"].ConnectionString;
            string CS = DBConnection.ConnectionString;

            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("GetStateList", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var stateDetails = new StateDetails();
                    stateDetails.StateId = reader.IsDBNull(reader.GetOrdinal("state_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("state_id"));
                    stateDetails.StateCode = reader.IsDBNull(reader.GetOrdinal("state_code")) ? "" : reader.GetString(reader.GetOrdinal("state_code"));
                    stateDetails.StateName = reader.IsDBNull(reader.GetOrdinal("state_name")) ? "" : reader.GetString(reader.GetOrdinal("state_name"));
                    stateList.Add(stateDetails);
                }
                con.Close();

                SqlCommand cmdAssetType = new SqlCommand("GetAssetType", con);
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

                SqlCommand cmdPropertyType = new SqlCommand("GetPropertyType", con);
                cmdPropertyType.CommandType = CommandType.StoredProcedure;
                con.Open();

                SqlDataReader readerPropertyType = cmdPropertyType.ExecuteReader();
                while (readerPropertyType.Read())
                {
                    var propertyType = new PropertyTypeViewModel();
                    propertyType.PropertyTypeId = readerPropertyType.IsDBNull(readerPropertyType.GetOrdinal("property_type_id")) ? 0 : readerPropertyType.GetInt32(readerPropertyType.GetOrdinal("property_type_id"));
                    propertyType.PropertyTypeName = readerPropertyType.IsDBNull(readerPropertyType.GetOrdinal("property_type_name")) ? "" : readerPropertyType.GetString(readerPropertyType.GetOrdinal("property_type_name"));

                    propertyTypeList.Add(propertyType);
                }
                con.Close();

                SqlCommand cmdSaveHits = new SqlCommand("SavePageHitStatus", con);

                cmdSaveHits.Parameters.AddWithValue("property_id", propertyId);
                cmdSaveHits.Parameters.AddWithValue("property_type", SamsPropertyType.C_Store);

                int customerId = 0;
                if(loggedInUser!= null)
                {
                    customerId = loggedInUser.CustomerId;
                }
                cmdSaveHits.Parameters.AddWithValue("customer_id", customerId);
                cmdSaveHits.Parameters.AddWithValue("hit_header", "Viewed C-Store");

                cmdSaveHits.CommandType = CommandType.StoredProcedure;
                con.Open();
                cmdSaveHits.ExecuteNonQuery();
                con.Close();

            }



            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("GetCStoreById", con);

                cmd.Parameters.AddWithValue("c_store_id", propertyId);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {

                    steDetails.CStoreId = reader.IsDBNull(reader.GetOrdinal("c_store_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("c_store_id"));
                    steDetails.StateId = reader.IsDBNull(reader.GetOrdinal("state_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("state_id"));
                    steDetails.PropertyHeader = reader.IsDBNull(reader.GetOrdinal("property_header")) ? "" : reader.GetString(reader.GetOrdinal("property_header"));
                    steDetails.StateName = reader.IsDBNull(reader.GetOrdinal("state_name")) ? "" : reader.GetString(reader.GetOrdinal("state_name"));
                    steDetails.City = reader.IsDBNull(reader.GetOrdinal("city")) ? "" : reader.GetString(reader.GetOrdinal("city"));
                    steDetails.Zipcode = reader.IsDBNull(reader.GetOrdinal("zipcode")) ? "" : reader.GetString(reader.GetOrdinal("zipcode"));

                    steDetails.County = reader.IsDBNull(reader.GetOrdinal("county")) ? "" : reader.GetString(reader.GetOrdinal("county"));
                    steDetails.AssetId = reader.IsDBNull(reader.GetOrdinal("asset_id")) ? "" : reader.GetString(reader.GetOrdinal("asset_id"));
                    steDetails.PropertyTypeId = reader.IsDBNull(reader.GetOrdinal("property_type_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("property_type_id"));
                    steDetails.PropertyTaxes = reader.IsDBNull(reader.GetOrdinal("property_taxes")) ? "" : reader.GetString(reader.GetOrdinal("property_taxes"));

                    steDetails.PropertyTypeName = reader.IsDBNull(reader.GetOrdinal("property_type_name")) ? "" : reader.GetString(reader.GetOrdinal("property_type_name"));

                    steDetails.Description = reader.IsDBNull(reader.GetOrdinal("property_description")) ? "" : reader.GetString(reader.GetOrdinal("property_description"));

                    steDetails.AskingPrice = reader.IsDBNull(reader.GetOrdinal("asking_price")) ? "" : reader.GetString(reader.GetOrdinal("asking_price"));
                    steDetails.AskingPriceString = reader.IsDBNull(reader.GetOrdinal("asking_price_string")) ? "" : reader.GetString(reader.GetOrdinal("asking_price_string"));

                    steDetails.AssetTypeId = reader.IsDBNull(reader.GetOrdinal("asset_type_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("asset_type_id"));
                    steDetails.AssetTypeName = reader.IsDBNull(reader.GetOrdinal("asset_type_name")) ? "" : reader.GetString(reader.GetOrdinal("asset_type_name"));
                    steDetails.LandSize = reader.IsDBNull(reader.GetOrdinal("land_size")) ? "" : reader.GetString(reader.GetOrdinal("land_size"));
                    steDetails.BuildingArea = reader.IsDBNull(reader.GetOrdinal("building_area")) ? "" : reader.GetString(reader.GetOrdinal("building_area"));

                    steDetails.YearBuilt = reader.IsDBNull(reader.GetOrdinal("year_built")) ? "" : reader.GetString(reader.GetOrdinal("year_built"));
                    steDetails.KnownEnvironmentalConditions = reader.IsDBNull(reader.GetOrdinal("known_environmental_conditions")) ? "" : reader.GetString(reader.GetOrdinal("known_environmental_conditions"));
                    steDetails.EMVCompliance = reader.IsDBNull(reader.GetOrdinal("emv_copliance")) ? "" : reader.GetString(reader.GetOrdinal("emv_copliance"));

                    steDetails.HoursOfOperation = reader.IsDBNull(reader.GetOrdinal("hours_of_operation")) ? "" : reader.GetString(reader.GetOrdinal("hours_of_operation"));
                    steDetails.CreatedDate = reader.IsDBNull(reader.GetOrdinal("created_date")) ? DateTime.Now : reader.GetDateTime(reader.GetOrdinal("created_date"));
                    steDetails.EnvironentNDAPdfFileName = reader.IsDBNull(reader.GetOrdinal("environent_nda_pdf_filename")) ? "" : reader.GetString(reader.GetOrdinal("environent_nda_pdf_filename"));

                    steDetails.Address = reader.IsDBNull(reader.GetOrdinal("c_store_address")) ? "" : reader.GetString(reader.GetOrdinal("c_store_address"));
                    steDetails.SelectedPropertyStatus = reader.IsDBNull(reader.GetOrdinal("property_status")) ? "" : reader.GetString(reader.GetOrdinal("property_status"));

                    if (steDetails.EnvironentNDAPdfFileName.Length > 0)
                    {
                        steDetails.EnvironentNDAPdfFileName = @"../../UploadedPdf/" + steDetails.EnvironentNDAPdfFileName;
                    }

                    steDetails.Latitude = reader.IsDBNull(reader.GetOrdinal("property_latitude")) ? "" : reader.GetString(reader.GetOrdinal("property_latitude"));
                    steDetails.Longitude = reader.IsDBNull(reader.GetOrdinal("property_longitude")) ? "" : reader.GetString(reader.GetOrdinal("property_longitude"));

                    steDetails.SelectedPropertyStatusId = reader.IsDBNull(reader.GetOrdinal("property_status_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("property_status_id"));
                    steDetails.SelectedPropertyStatus = reader.IsDBNull(reader.GetOrdinal("property_status")) ? "" : reader.GetString(reader.GetOrdinal("property_status"));
                    steDetails.Rent = reader.IsDBNull(reader.GetOrdinal("rent")) ? "" : reader.GetString(reader.GetOrdinal("rent"));

                    steDetails.CheckIfPropertyListed = reader.IsDBNull(reader.GetOrdinal("check_if_property_listed")) ? 0 : reader.GetInt32(reader.GetOrdinal("check_if_property_listed"));
                    steDetails.ListingAgentName = reader.IsDBNull(reader.GetOrdinal("listing_agent_name")) ? "" : reader.GetString(reader.GetOrdinal("listing_agent_name"));
                    steDetails.ListingExpiry = reader.IsDBNull(reader.GetOrdinal("listing_expiry")) ? DateTime.Now : reader.GetDateTime(reader.GetOrdinal("listing_expiry"));
                    steDetails.ListingPrice = reader.IsDBNull(reader.GetOrdinal("listing_price")) ? "" : reader.GetString(reader.GetOrdinal("listing_price"));

                    steDetails.Term = reader.IsDBNull(reader.GetOrdinal("term")) ? "" : reader.GetString(reader.GetOrdinal("term"));
                    steDetails.LeaseType = reader.IsDBNull(reader.GetOrdinal("lease_type")) ? 0 : reader.GetInt32(reader.GetOrdinal("lease_type"));
                    steDetails.AskingRent = reader.IsDBNull(reader.GetOrdinal("asking_rent")) ? "" : reader.GetString(reader.GetOrdinal("asking_rent"));

                    steDetails.CheckIfOilSupplyContractApplicable = reader.IsDBNull(reader.GetOrdinal("check_if_oil_supply_contract_applicable")) ? 0 : reader.GetInt32(reader.GetOrdinal("check_if_oil_supply_contract_applicable"));
                    steDetails.TermOfSupplyContract = reader.IsDBNull(reader.GetOrdinal("term_of_supply_contract")) ? "" : reader.GetString(reader.GetOrdinal("term_of_supply_contract"));

                    steDetails.CheckIfOilSupplyContractApplicableLeaseAndFee = reader.IsDBNull(reader.GetOrdinal("supply_contract_applicable_lease_and_fee")) ? 0 : reader.GetInt32(reader.GetOrdinal("supply_contract_applicable_lease_and_fee"));
                    steDetails.TermOfSupplyContractLeaseAndFee = reader.IsDBNull(reader.GetOrdinal("supply_contract_term_lease_and_fee")) ? "" : reader.GetString(reader.GetOrdinal("supply_contract_term_lease_and_fee"));

                    steDetails.TermRemaining = reader.IsDBNull(reader.GetOrdinal("term_remaining")) ? "" : reader.GetString(reader.GetOrdinal("term_remaining"));
                    steDetails.RentalIncome = reader.IsDBNull(reader.GetOrdinal("rental_income")) ? "" : reader.GetString(reader.GetOrdinal("rental_income"));
                    steDetails.LeaseTypeLeaseAndFee = reader.IsDBNull(reader.GetOrdinal("lease_type_lease_and_fee")) ? 0 : reader.GetInt32(reader.GetOrdinal("lease_type_lease_and_fee"));
                    steDetails.CheckIfOilSupplyContractApplicableLeaseAndFee = reader.IsDBNull(reader.GetOrdinal("supply_contract_applicable_lease_and_fee")) ? 0 : reader.GetInt32(reader.GetOrdinal("supply_contract_applicable_lease_and_fee"));
                    steDetails.TermOfSupplyContractLeaseAndFee = reader.IsDBNull(reader.GetOrdinal("supply_contract_term_lease_and_fee")) ? "" : reader.GetString(reader.GetOrdinal("supply_contract_term_lease_and_fee"));

                    if (steDetails.AssetTypeId == (int)SamAssetType.Lease)
                    {
                        steDetails.AskingPrice = steDetails.AskingPriceString;
                    }

                    steDetails.TermOptionPurchase = reader.IsDBNull(reader.GetOrdinal("term_option_purchase")) ? "" : reader.GetString(reader.GetOrdinal("term_option_purchase"));
                    steDetails.AskingRentOptionPurchase = reader.IsDBNull(reader.GetOrdinal("asking_rent_option_purchase")) ? "" : reader.GetString(reader.GetOrdinal("asking_rent_option_purchase"));
                    steDetails.LeaseTypePurchase = reader.IsDBNull(reader.GetOrdinal("lease_type_purchase")) ? 0 : reader.GetInt32(reader.GetOrdinal("lease_type_purchase"));
                    steDetails.OptionPurchasePrice = reader.IsDBNull(reader.GetOrdinal("option_purchase_price")) ? "" : reader.GetString(reader.GetOrdinal("option_purchase_price"));

                    steDetails.PropertyHeaderLine2 = reader.IsDBNull(reader.GetOrdinal("property_header_line_2")) ? "" : reader.GetString(reader.GetOrdinal("property_header_line_2"));
                }
                con.Close();


                SqlCommand cmdImageList = new SqlCommand("GetPropertyImageList", con);

                cmdImageList.Parameters.AddWithValue("property_id", propertyId);
                cmdImageList.Parameters.AddWithValue("property_type", SamsPropertyType.C_Store);

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
                con.Close();

                if (propertyImageList.Count == 0)
                {
                    var imageItem = new ImageViewModel();
                    imageItem.ImageId = 0;
                    imageItem.PropertyId = steDetails.CStoreId;

                    imageItem.ImageName = "no_image.png?b=1";
                    string pic = @"../../UploadedImage/" + imageItem.ImageName;
                    imageItem.ImageName = pic;
                    propertyImageList.Add(imageItem);
                }

                steDetails.ImageList = propertyImageList;

                steDetails.StateList = stateList;
                steDetails.AssetTypeList = assetTypeList;
                steDetails.PropertyTypeList = propertyTypeList;
                steDetails.LeaseTypeList = leaseTypeList;




                SqlCommand cmdComplianceList = new SqlCommand("GetCstoreComplianceFiles", con);

                cmdComplianceList.Parameters.AddWithValue("property_id", propertyId);
                cmdComplianceList.CommandType = CommandType.StoredProcedure;
                con.Open();

                SqlDataReader readerComplianceList = cmdComplianceList.ExecuteReader();
                List<AdditionalFilesViewModel> c_storeFiles = new List<AdditionalFilesViewModel>();
                while (readerComplianceList.Read())
                {
                    var c_storeFile = new AdditionalFilesViewModel();
                    c_storeFile.FileId = readerComplianceList.IsDBNull(readerComplianceList.GetOrdinal("file_id")) ? 0 : readerComplianceList.GetInt32(readerComplianceList.GetOrdinal("file_id"));
                    c_storeFile.PropertyId = propertyId;
                    c_storeFile.FileType = readerComplianceList.IsDBNull(readerComplianceList.GetOrdinal("file_type")) ? "" : readerComplianceList.GetString(readerComplianceList.GetOrdinal("file_type"));


                    c_storeFile.FileName = readerComplianceList.IsDBNull(readerComplianceList.GetOrdinal("file_name")) ? "" : readerComplianceList.GetString(readerComplianceList.GetOrdinal("file_name"));
                    string pic = @"../../property_files/" + c_storeFile.FileName;
                    c_storeFile.FileName = pic;
                    c_storeFiles.Add(c_storeFile);
                }
                con.Close();

                steDetails.GeneralFilesList = new List<AdditionalFilesViewModel>();
                SqlCommand cmdGeneralFiles = new SqlCommand("GetGeneralFiles", con);

                cmdGeneralFiles.Parameters.AddWithValue("property_id", propertyId);
                cmdGeneralFiles.Parameters.AddWithValue("property_type", SamsPropertyType.C_Store);
                cmdGeneralFiles.CommandType = CommandType.StoredProcedure;
                con.Open();

                SqlDataReader readerGeneralFiles = cmdGeneralFiles.ExecuteReader();
                List<AdditionalFilesViewModel> generalFiles = new List<AdditionalFilesViewModel>();
                while (readerGeneralFiles.Read())
                {
                    var generalFile = new AdditionalFilesViewModel();
                    generalFile.FileId = readerGeneralFiles.IsDBNull(readerGeneralFiles.GetOrdinal("general_file_id")) ? 0 : readerGeneralFiles.GetInt32(readerGeneralFiles.GetOrdinal("general_file_id"));
                    generalFile.PropertyId = propertyId;
                    generalFile.FileType = readerGeneralFiles.IsDBNull(readerGeneralFiles.GetOrdinal("file_type")) ? "" : readerGeneralFiles.GetString(readerGeneralFiles.GetOrdinal("file_type"));


                    generalFile.FileName = readerGeneralFiles.IsDBNull(readerGeneralFiles.GetOrdinal("file_name")) ? "" : readerGeneralFiles.GetString(readerGeneralFiles.GetOrdinal("file_name"));
                    generalFile.FileNameWithoutPath = generalFile.FileName;
                    string pic = @"../../property_files/" + generalFile.FileName;
                    generalFile.FileName = pic;
                    generalFiles.Add(generalFile);
                }
                steDetails.GeneralFilesList = generalFiles;
                con.Close();

                steDetails.NDAComplaintsFilesList = c_storeFiles;


            }
            if (TempData["ErrorMessage"] != null)
            {
                ViewBag.ErrorMessage = TempData["ErrorMessage"];
                TempData.Remove("ErrorMessage");
            }

            SamsSettings sSettings = SamsSettingsController.GetSamsSettings();
            steDetails.MySettings = sSettings;

            return View(steDetails);
        }

        public ActionResult submit_site()
        {
            List<StateDetails> stateList = new List<StateDetails>();
            List<StateDetails> allStateList = new List<StateDetails>();
            List<MarketDetails> marketList = new List<MarketDetails>();
            

            // string CS = ConfigurationManager.ConnectionStrings["testConnection"].ConnectionString;
            string CS = DBConnection.ConnectionString;

            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("GetStateList", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var stateDetails = new StateDetails();
                    stateDetails.StateId = reader.IsDBNull(reader.GetOrdinal("state_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("state_id"));
                    stateDetails.StateCode = reader.IsDBNull(reader.GetOrdinal("state_code")) ? "" : reader.GetString(reader.GetOrdinal("state_code"));
                    stateDetails.StateName = reader.IsDBNull(reader.GetOrdinal("state_name")) ? "" : reader.GetString(reader.GetOrdinal("state_name"));
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
            }
            
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("GetAllStateList", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var allStateDetails = new StateDetails();
                    allStateDetails.StateId = reader.IsDBNull(reader.GetOrdinal("state_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("state_id"));
                    allStateDetails.StateName = reader.IsDBNull(reader.GetOrdinal("state_name")) ? "" : reader.GetString(reader.GetOrdinal("state_name"));
                    allStateList.Add(allStateDetails);
                }
                con.Close();
            }

            SiteDetails siteDetails = new SiteDetails();
            siteDetails.StateList = stateList;
            siteDetails.AllStateList = allStateList;
            siteDetails.MarketList = marketList;
            siteDetails.CaptchaImage = Helper.GetCaptcha();

            CustomerViewModel loggedInUser = HttpContext.Session.GetObjectFromJson<CustomerViewModel>("LoggedInUser");
            if (loggedInUser != null)
            {
                ViewData["LoggedInUserId"] = loggedInUser.CustomerId;
                ViewData["LoggedInUserName"] = loggedInUser.FirstName + " " + loggedInUser.LastName;
            }
            else
            {
                loggedInUser = GetEmptyCustomer();
            }

            return View(siteDetails);
        }



        [HttpPost]
        public ActionResult submit_site(SiteDetails siteDetails)
        {
            int siteDetailsId = siteDetails.SiteDetailsId;
            string CS = DBConnection.ConnectionString;
            
            

            var lastFourDigit = siteDetails.ContactNumber.Substring(siteDetails.ContactNumber.Length - 4);
            if(lastFourDigit == siteDetails.LastFourDigitNumber)
            {
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

                    cmd.Parameters.AddWithValue("created_by", NewPropertyCreaedBy.ByCustomer);

                    siteDetailsId = int.Parse(cmd.ExecuteScalar().ToString());

                    siteDetails.SiteDetailsId = siteDetailsId;

                    con.Close();
                }



                return RedirectToAction("Submit_confirmation");
            }
            else
            {
                return RedirectToAction("SaveFailed");
            }
            
        }


        public ActionResult Submit_confirmation()
        {
            return View();
        }

        public IActionResult SaveFailed()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DoLogin(CustomerViewModel customer)
        {

            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                //GetUserForLogin
                SqlCommand cmdLogin = new SqlCommand("GetUserForLogin", con);
                //customer.Password = StringFunctions.Encrypt(customer.Password, SiteSettings.PasswordKey);
                cmdLogin.Parameters.AddWithValue("user_name", customer.UserName);
                cmdLogin.Parameters.AddWithValue("customer_password", customer.Password);
                cmdLogin.CommandType = CommandType.StoredProcedure;
                con.Open();

                SqlDataReader reader = cmdLogin.ExecuteReader();
                List<AdditionalFilesViewModel> c_storeFiles = new List<AdditionalFilesViewModel>();
                while (reader.Read())
                {
                    customer.CustomerId = reader.IsDBNull(reader.GetOrdinal("customer_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("customer_id"));
                    customer.FirstName = reader.IsDBNull(reader.GetOrdinal("first_name")) ? "" : reader.GetString(reader.GetOrdinal("first_name"));

                    customer.LastName = reader.IsDBNull(reader.GetOrdinal("last_name")) ? "" : reader.GetString(reader.GetOrdinal("last_name"));
                    customer.EmailAddress = reader.IsDBNull(reader.GetOrdinal("email_address")) ? "" : reader.GetString(reader.GetOrdinal("email_address"));
                    customer.ContactNumber = reader.IsDBNull(reader.GetOrdinal("contact_number")) ? "" : reader.GetString(reader.GetOrdinal("contact_number"));
                    customer.UserName = reader.IsDBNull(reader.GetOrdinal("user_name")) ? "" : reader.GetString(reader.GetOrdinal("user_name"));
                    customer.Password = reader.IsDBNull(reader.GetOrdinal("customer_password")) ? "" : reader.GetString(reader.GetOrdinal("customer_password"));
                    customer.SignedStatus = reader.IsDBNull(reader.GetOrdinal("signed_status")) ? "" : reader.GetString(reader.GetOrdinal("signed_status"));

                    customer.CreatedDate = reader.IsDBNull(reader.GetOrdinal("created_date")) ? DateTime.Now : reader.GetDateTime(reader.GetOrdinal("created_date"));
                    customer.LastLoginDate = reader.IsDBNull(reader.GetOrdinal("last_login_date")) ? DateTime.Now : reader.GetDateTime(reader.GetOrdinal("last_login_date"));
                    
                    

                    HttpContext.Session.SetObjectAsJson("LoggedInUser", customer);
                    
                }

                con.Close();

                

            }

            if (customer.CustomerId == 0)
            {
                TempData["ErrorMessage"] = "Wrong Username/ Password";
                
            }

            return RedirectToAction("ViewCStore", new { propertyId = customer.LoginPropertyId });
        }

        public ActionResult RegisterNewCustomer()
        {


            var cust = new CustomerViewModel();

            List<StateDetails> stateList = new List<StateDetails>();

            // string CS = ConfigurationManager.ConnectionStrings["testConnection"].ConnectionString;
            string CS = DBConnection.ConnectionString;

            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("GetAllStateList", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var stateDetails = new StateDetails();
                    stateDetails.StateId = reader.IsDBNull(reader.GetOrdinal("state_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("state_id"));
                    stateDetails.StateName = reader.IsDBNull(reader.GetOrdinal("state_name")) ? "" : reader.GetString(reader.GetOrdinal("state_name"));
                    stateList.Add(stateDetails);
                }
                con.Close();
            }

            cust.StateList = stateList;

            return View(cust);
        }

        

        private CustomerViewModel GetEmptyCustomer()
        {
            var customerModel = new CustomerViewModel();
            customerModel.CustomerId = 0;
            customerModel.FirstName = "";
            customerModel.LastName = "";

            customerModel.EmailAddress = "";
            customerModel.ContactNumber = "";
            customerModel.UserName = "";

            return customerModel;
        }

        public ActionResult LogOutCustomer()
        {
            HttpContext.Session.SetObjectAsJson("LoggedInUser", new CustomerViewModel());
            ViewData["LoggedInUserName"] = null;
            return RedirectToAction("Index", "Home");
        }






        public ActionResult shopping_center_list(int stateId)
        {
            ShoppingCenterListViewModel shoppingCenterUI = new ShoppingCenterListViewModel();
            List<ShoppingCenterViewModel> shoppingCenterList = new List<ShoppingCenterViewModel>();
            List<StateDetails> stateList = GetStateList();

            shoppingCenterUI.StateList = stateList;

            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {

                SqlCommand cmd = new SqlCommand("GetShoppingCenterListByState", con);
                cmd.Parameters.AddWithValue("state_id", stateId);

                cmd.CommandType = CommandType.StoredProcedure;

                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {


                    var shoppingCenterModel = new ShoppingCenterViewModel();

                    shoppingCenterModel.ShoppingCenterId = reader.IsDBNull(reader.GetOrdinal("shopping_center_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("shopping_center_id"));

                    shoppingCenterModel.ShoppingCenterName = reader.IsDBNull(reader.GetOrdinal("shopping_center_name")) ? "" : reader.GetString(reader.GetOrdinal("shopping_center_name"));

                    if (shoppingCenterModel.ShoppingCenterName.Length > 18)
                    {
                        shoppingCenterModel.ShoppingCenterName = shoppingCenterModel.ShoppingCenterName.Substring(0, 18) + "..";
                    }

                    shoppingCenterModel.StateId = reader.IsDBNull(reader.GetOrdinal("state_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("state_id"));
                    shoppingCenterModel.StateName = reader.IsDBNull(reader.GetOrdinal("state_name")) ? "" : reader.GetString(reader.GetOrdinal("state_name"));

                    shoppingCenterModel.CityName = reader.IsDBNull(reader.GetOrdinal("city_name")) ? "" : reader.GetString(reader.GetOrdinal("city_name"));
                    shoppingCenterModel.Zipcode = reader.IsDBNull(reader.GetOrdinal("zip_code")) ? "" : reader.GetString(reader.GetOrdinal("zip_code"));

                    shoppingCenterModel.PropertyStatusId = reader.IsDBNull(reader.GetOrdinal("property_status_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("property_status_id"));
                    shoppingCenterModel.PropertyStatusName = reader.IsDBNull(reader.GetOrdinal("asset_type_name")) ? "" : reader.GetString(reader.GetOrdinal("asset_type_name"));

                    shoppingCenterModel.RentAmount = reader.IsDBNull(reader.GetOrdinal("rent_amount")) ? "" : reader.GetString(reader.GetOrdinal("rent_amount"));
                    shoppingCenterModel.PropertyTypeId = reader.IsDBNull(reader.GetOrdinal("property_type_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("property_type_id"));
                    shoppingCenterModel.Spaces = reader.IsDBNull(reader.GetOrdinal("spaces")) ? "" : reader.GetString(reader.GetOrdinal("spaces"));
                    shoppingCenterModel.SpacesAvailable = reader.IsDBNull(reader.GetOrdinal("spaces_available")) ? "" : reader.GetString(reader.GetOrdinal("spaces_available"));
                    shoppingCenterModel.BuildingSize = reader.IsDBNull(reader.GetOrdinal("building_size")) ? "" : reader.GetString(reader.GetOrdinal("building_size"));
                    shoppingCenterModel.AssetStatus = reader.IsDBNull(reader.GetOrdinal("asset_status")) ? 0 : reader.GetInt32(reader.GetOrdinal("asset_status"));

                    shoppingCenterModel.ShopDescription = reader.IsDBNull(reader.GetOrdinal("shop_description")) ? "" : reader.GetString(reader.GetOrdinal("shop_description"));

                    if (shoppingCenterModel.ShopDescription.Length > 18)
                    {
                        shoppingCenterModel.ShopDescription = shoppingCenterModel.ShopDescription.Substring(0, 18) + "..";
                    }

                    shoppingCenterModel.CreatedDate = reader.IsDBNull(reader.GetOrdinal("created_date")) ? DateTime.Now : reader.GetDateTime(reader.GetOrdinal("created_date"));

                    
                    List<ImageViewModel> propertyImageList = new List<ImageViewModel>();
                    using (SqlConnection conImages = new SqlConnection(CS))
                    {
                        SqlCommand cmdImageList = new SqlCommand("GetPropertyImageList", conImages);
                        cmdImageList.Parameters.AddWithValue("property_id", shoppingCenterModel.ShoppingCenterId);
                        cmdImageList.Parameters.AddWithValue("property_type", SamsPropertyType.ShoppingCenter);

                        cmdImageList.CommandType = CommandType.StoredProcedure;
                        conImages.Open();

                        SqlDataReader readerMarket = cmdImageList.ExecuteReader();

                        while (readerMarket.Read())
                        {
                            var imageItem = new ImageViewModel();
                            imageItem.ImageId = readerMarket.IsDBNull(readerMarket.GetOrdinal("image_id")) ? 0 : readerMarket.GetInt32(readerMarket.GetOrdinal("image_id"));
                            imageItem.PropertyId = shoppingCenterModel.ShoppingCenterId;



                            imageItem.ImageName = readerMarket.IsDBNull(readerMarket.GetOrdinal("image_name")) ? "" : readerMarket.GetString(readerMarket.GetOrdinal("image_name"));
                            string pic = @"../../UploadedImage/" + imageItem.ImageName;
                            imageItem.ImageName = pic;
                            propertyImageList.Add(imageItem);
                        }
                        conImages.Close();
                    }
                    if (propertyImageList.Count == 0)
                    {
                        var imageItem = new ImageViewModel();
                        imageItem.ImageId = 0;
                        imageItem.PropertyId = shoppingCenterModel.ShoppingCenterId;

                        imageItem.ImageName = "no_image.png?b=1";
                        string pic = @"../../UploadedImage/" + imageItem.ImageName;
                        imageItem.ImageName = pic;
                        propertyImageList.Add(imageItem);
                    }

                    
                    

                    shoppingCenterModel.ImageList = propertyImageList;

                    shoppingCenterList.Add(shoppingCenterModel);
                }
                con.Close();
            }

            shoppingCenterUI.ShoppingCenterList = shoppingCenterList;

            return View(shoppingCenterUI);
        }


        public IActionResult ViewShoppingCenter(int centerId)
        {

            var shoppingCenterModel = new ShoppingCenterViewModel();
            string CS = DBConnection.ConnectionString;

            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("GetShoppingCenterById", con);
                cmd.Parameters.AddWithValue("shopping_center_id", centerId);

                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {

                    

                    shoppingCenterModel.ShoppingCenterId = reader.IsDBNull(reader.GetOrdinal("shopping_center_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("shopping_center_id"));

                    shoppingCenterModel.ShoppingCenterName = reader.IsDBNull(reader.GetOrdinal("shopping_center_name")) ? "" : reader.GetString(reader.GetOrdinal("shopping_center_name"));
                    shoppingCenterModel.StateId = reader.IsDBNull(reader.GetOrdinal("state_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("state_id"));
                    shoppingCenterModel.StateName = reader.IsDBNull(reader.GetOrdinal("state_name")) ? "" : reader.GetString(reader.GetOrdinal("state_name"));

                    shoppingCenterModel.CityName = reader.IsDBNull(reader.GetOrdinal("city_name")) ? "" : reader.GetString(reader.GetOrdinal("city_name"));
                    shoppingCenterModel.Zipcode = reader.IsDBNull(reader.GetOrdinal("zip_code")) ? "" : reader.GetString(reader.GetOrdinal("zip_code"));

                    shoppingCenterModel.PropertyStatusId = reader.IsDBNull(reader.GetOrdinal("property_status_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("property_status_id"));
                    shoppingCenterModel.PropertyStatusName = reader.IsDBNull(reader.GetOrdinal("asset_type_name")) ? "" : reader.GetString(reader.GetOrdinal("asset_type_name"));

                    shoppingCenterModel.RentAmount = reader.IsDBNull(reader.GetOrdinal("rent_amount")) ? "" : reader.GetString(reader.GetOrdinal("rent_amount"));
                    shoppingCenterModel.PropertyTypeId = reader.IsDBNull(reader.GetOrdinal("property_type_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("property_type_id"));
                    shoppingCenterModel.Spaces = reader.IsDBNull(reader.GetOrdinal("spaces")) ? "" : reader.GetString(reader.GetOrdinal("spaces"));
                    shoppingCenterModel.SpacesAvailable = reader.IsDBNull(reader.GetOrdinal("spaces_available")) ? "" : reader.GetString(reader.GetOrdinal("spaces_available"));
                    shoppingCenterModel.BuildingSize = reader.IsDBNull(reader.GetOrdinal("building_size")) ? "" : reader.GetString(reader.GetOrdinal("building_size"));
                    shoppingCenterModel.AssetStatus = reader.IsDBNull(reader.GetOrdinal("asset_status")) ? 0 : reader.GetInt32(reader.GetOrdinal("asset_status"));

                    shoppingCenterModel.ShopDescription = reader.IsDBNull(reader.GetOrdinal("shop_description")) ? "" : reader.GetString(reader.GetOrdinal("shop_description"));

                    shoppingCenterModel.CreatedDate = reader.IsDBNull(reader.GetOrdinal("created_date")) ? DateTime.Now : reader.GetDateTime(reader.GetOrdinal("created_date"));

                }
                con.Close();


                SqlCommand cmdImageList = new SqlCommand("GetPropertyImageList", con);

                cmdImageList.Parameters.AddWithValue("property_id", centerId);
                cmdImageList.Parameters.AddWithValue("property_type", SamsPropertyType.ShoppingCenter);

                cmdImageList.CommandType = CommandType.StoredProcedure;
                con.Open();

                SqlDataReader readerMarket = cmdImageList.ExecuteReader();
                List<ImageViewModel> propertyImageList = new List<ImageViewModel>();
                while (readerMarket.Read())
                {
                    var imageItem = new ImageViewModel();
                    imageItem.ImageId = readerMarket.IsDBNull(readerMarket.GetOrdinal("image_id")) ? 0 : readerMarket.GetInt32(readerMarket.GetOrdinal("image_id"));
                    imageItem.PropertyId = centerId;



                    imageItem.ImageName = readerMarket.IsDBNull(readerMarket.GetOrdinal("image_name")) ? "" : readerMarket.GetString(readerMarket.GetOrdinal("image_name"));
                    string pic = @"../../UploadedImage/" + imageItem.ImageName;
                    imageItem.ImageName = pic;
                    propertyImageList.Add(imageItem);
                }
                con.Close();

                if (propertyImageList.Count == 0)
                {
                    var imageItem = new ImageViewModel();
                    imageItem.ImageId = 0;
                    imageItem.PropertyId = shoppingCenterModel.ShoppingCenterId;

                    imageItem.ImageName = "no_image.png?b=1";
                    string pic = @"../../UploadedImage/" + imageItem.ImageName;
                    imageItem.ImageName = pic;
                    propertyImageList.Add(imageItem);
                }

                shoppingCenterModel.ImageList = propertyImageList;

                CustomerViewModel loggedInUser = HttpContext.Session.GetObjectFromJson<CustomerViewModel>("LoggedInUser");
                SqlCommand cmdSaveHits = new SqlCommand("SavePageHitStatus", con);

                cmdSaveHits.Parameters.AddWithValue("property_id", centerId);
                cmdSaveHits.Parameters.AddWithValue("property_type", SamsPropertyType.ShoppingCenter);

                int customerId = 0;
                if (loggedInUser != null)
                {
                    customerId = loggedInUser.CustomerId;
                }
                cmdSaveHits.Parameters.AddWithValue("customer_id", customerId);
                cmdSaveHits.Parameters.AddWithValue("hit_header", "Viewed Shopping Center");

                cmdSaveHits.CommandType = CommandType.StoredProcedure;
                con.Open();
                cmdSaveHits.ExecuteNonQuery();
                con.Close();

                List<StateDetails> stateList = GetStateList();
                shoppingCenterModel.StateList = stateList;

            }
            return View(shoppingCenterModel);
        }

        public void GeneratePdf()
        {
            string fileName = @"~/../../templates/nda.docx";
            var doc = DocX.Create(fileName);
            doc.InsertParagraph("Hello Word");
            doc.Save();
        }

        public ActionResult CustomerSign(string customerId)
        {
            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd= new SqlCommand("UpdateSignedStatus", con);
                cmd.Parameters.AddWithValue("customerId", customerId);

                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                cmd.ExecuteNonQuery();

                con.Close();
            }
            return View();
        }


        [HttpPost]
        public ActionResult RegisterCustomer(CustomerViewModel customer)
        {
            /*
            var imageFileName = Helper.GetUniqueFileName(customer.UploadedNDAFile.FileName);
            var imageFilePath = Path.Combine(webHostEnvironment.WebRootPath + @"/customer_nda_files", imageFileName);
            using (var stream = System.IO.File.Create(imageFilePath))
            {
                customer.UploadedNDAFile.CopyTo(stream);
            }
            */

            var imageFileName = "no_file";
            bool newRecord = true;

            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                

                //GetUserForLogin
                SqlCommand cmdLogin = new SqlCommand("RegisterCustomer", con);

                cmdLogin.Parameters.AddWithValue("customer_id", customer.CustomerId);
                cmdLogin.Parameters.AddWithValue("first_name", customer.FirstName);

                cmdLogin.Parameters.AddWithValue("last_name", customer.LastName);

                customer.EmailAddress = customer.UserName;

                cmdLogin.Parameters.AddWithValue("email_address", customer.EmailAddress);
                

                cmdLogin.Parameters.AddWithValue("contact_number", customer.ContactNumber);
                cmdLogin.Parameters.AddWithValue("signed_nda_file", imageFileName);
                cmdLogin.Parameters.AddWithValue("user_name", customer.UserName);

                //customer.Password = StringFunctions.Encrypt(customer.Password, SiteSettings.PasswordKey);

                cmdLogin.Parameters.AddWithValue("customer_password", customer.Password);

                cmdLogin.Parameters.AddWithValue("company_name", customer.Company);
                cmdLogin.Parameters.AddWithValue("given_title", customer.GivenTitle);

                cmdLogin.Parameters.AddWithValue("address", customer.Address);
                cmdLogin.Parameters.AddWithValue("zipcode", customer.Zipcode);
                cmdLogin.Parameters.AddWithValue("city", customer.City);
                cmdLogin.Parameters.AddWithValue("state_id", customer.StateId);
                cmdLogin.Parameters.AddWithValue("cell_number", customer.CellNumber);

                cmdLogin.Parameters.AddWithValue("signed_status", "Shared NDA For Signing"); 

                cmdLogin.CommandType = CommandType.StoredProcedure;
                con.Open();

                
                if(customer.CustomerId > 0)
                {
                    newRecord = false;
                }

                customer.CustomerId = int.Parse(cmdLogin.ExecuteScalar().ToString());






                customer.UploadedNDAFile = null;

                HttpContext.Session.SetObjectAsJson("LoggedInUser", customer);
                //LoginPropertyId 
                con.Close();
            }

            SamsSettings sSettings = SamsSettingsController.GetSamsSettings();

            StringBuilder sbEmailMessage = new StringBuilder();
            //sbEmailMessage.Append("<div><b>Greetings " + customer.FirstName + " " + customer.LastName + ",</b><div>");

            sbEmailMessage.Append("<div>");
            sbEmailMessage.Append("Please click the link below to review and sign the confidentiality agreement to get full access to additional information. <br /><br />");
            // sbEmailMessage.Append("Please find the link to put your signature. <br /><br />");
            //sbEmailMessage.Append("<a href='https://samsholdingsdevelopment.azurewebsites.net/RealEstate/GetCustomerAgreement?CustomerId=" + customer.CustomerId + "'>Confidentiality Agreement/Non-disclosure Agreement</a>");
            sbEmailMessage.Append("<a href='" + Helper.HostName + "/RealEstate/GetCustomerAgreement?CustomerId=" + customer.CustomerId + "'>Confidentiality Agreement/Non-disclosure Agreement</a>");
            sbEmailMessage.Append("</div>");

            sbEmailMessage.Append("<div>");
            sbEmailMessage.Append("Best Regards<br />");
            sbEmailMessage.Append("Sam's Holdings, LLC");
            sbEmailMessage.Append("</div>");


            //string fromEmail = "infosh@samsholdings.com";
            string fromEmail = sSettings.SmtpEmailAddress;
            MailMessage mailMessage = new MailMessage(fromEmail, customer.EmailAddress, "Non-Disclosure Agreement from Sam’s Holdings LLC.", sbEmailMessage.ToString());
            mailMessage.IsBodyHtml = true;

            /*
            MailAddress copy = new MailAddress("arun@knowminal.com");
            mailMessage.CC.Add(copy);
            */

            //SmtpClient smtpClient = new SmtpClient("smtp.office365.com", 587);
            SmtpClient smtpClient = new SmtpClient(sSettings.SmtpServer, int.Parse(sSettings.SmtpPortNumber));
            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            //smtpClient.Credentials = new NetworkCredential(fromEmail, "FMf5IY78JnSlolc2");
            if (sSettings.SmtpPassword.Trim().Length == 0)
            {
                sSettings.SmtpPassword = "FMf5IY78JnSlolc2";
            }
            smtpClient.Credentials = new NetworkCredential(fromEmail, sSettings.SmtpPassword);

            smtpClient.Send(mailMessage);

            // return RedirectToAction("ViewCStore", new { propertyId = customer.LoginPropertyId });

            /*
            if (newRecord)
            {
                return View(customer);
            }
            else
            {
                return RedirectToAction("GetCustomerAgreement", new { CustomerId = customer.CustomerId });
            }

            return RedirectToAction("GetCustomerById", new { CustomerId = customer.CustomerId });
            */
            customer.EmailBody = sbEmailMessage.ToString();
            return View(customer);
        }

        void SendConrirmationEmail(string toAddress)
        {
            string host, username, password, fromUsername, toUser;
            host = "smtp.office365.com";
            username = "infosh@samsholdings.com";
            password = "FMf5IY78JnSlolc2";
            fromUsername = "infosh@samsholdings.com";
            toUser = toAddress;

            SmtpClient m_server;
            m_server = new SmtpClient(host);
            m_server.Port = 587;
            m_server.EnableSsl = true;
            m_server.UseDefaultCredentials = false;
            m_server.Credentials = new System.Net.NetworkCredential(username, password);
            m_server.Timeout = 60000;

            StringBuilder msgToSend = new StringBuilder();
            

            MailMessage msg = new MailMessage(fromUsername, toUser, "Sam's Holdings", "<div>This is a message from knowminal</div>");
            msg.IsBodyHtml = true;

            m_server.Send(msg);

        }

        [HttpPost]
        public IActionResult SaveSign(string customerSignature, string CustomerId, string directorSignature, string customerVerificationId)
        {
            CustomerViewModel customer = new CustomerViewModel();

            string fileName = "signature_"+ CustomerId+"_.png";
            string directorFileName = "director_signature_" + CustomerId + "_.png";
            string fileNameWitPath = Path.Combine(webHostEnvironment.WebRootPath + @"/signs_data", fileName);
            string directorFileNameWitPath = Path.Combine(webHostEnvironment.WebRootPath + @"/signs_data", directorFileName);

            customerSignature = customerSignature.Replace("data:image/png;base64,", "");
            directorSignature = directorSignature.Replace("data:image/png;base64,", ""); 

            using (FileStream fs = new FileStream(fileNameWitPath, FileMode.Create))
            {
                using (BinaryWriter bw = new BinaryWriter(fs))
                {
                    byte[] data = Convert.FromBase64String(customerSignature);
                    bw.Write(data);
                    bw.Close();
                }
                fs.Close();
            }

            using (FileStream fs = new FileStream(directorFileNameWitPath, FileMode.Create))
            {
                using (BinaryWriter bw = new BinaryWriter(fs))
                {
                    byte[] data = Convert.FromBase64String(directorSignature);
                    bw.Write(data);
                    bw.Close();
                }
                fs.Close();
            }

            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                //GetUserForLogin
                SqlCommand cmdLogin = new SqlCommand("SaveCustomerSignature", con);

                cmdLogin.Parameters.AddWithValue("customer_sign", fileName);
                cmdLogin.Parameters.AddWithValue("director_signature", directorFileName); 
                cmdLogin.Parameters.AddWithValue("customer_id", int.Parse(CustomerId));
                cmdLogin.Parameters.AddWithValue("sh_verification_id", customerVerificationId); 



                cmdLogin.CommandType = CommandType.StoredProcedure;
                con.Open();

                cmdLogin.ExecuteNonQuery();


                //HttpContext.Session.SetObjectAsJson("LoggedInUser", customer); RegisterNewCustomer

                con.Close();
            }



            using (SqlConnection con = new SqlConnection(CS))
            {
                //GetUserForLogin
                SqlCommand cmdLogin = new SqlCommand("GetCustomerById", con);

                cmdLogin.Parameters.AddWithValue("customer_id", CustomerId);

                cmdLogin.CommandType = CommandType.StoredProcedure;
                con.Open();

                SqlDataReader readerMarket = cmdLogin.ExecuteReader();
                

                while (readerMarket.Read())
                {

                    customer.CustomerId = readerMarket.IsDBNull(readerMarket.GetOrdinal("customer_id")) ? 0 : readerMarket.GetInt32(readerMarket.GetOrdinal("customer_id"));
                    customer.FirstName = readerMarket.IsDBNull(readerMarket.GetOrdinal("first_name")) ? "" : readerMarket.GetString(readerMarket.GetOrdinal("first_name"));
                    customer.FirstName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(customer.FirstName);
                    customer.LastName = readerMarket.IsDBNull(readerMarket.GetOrdinal("last_name")) ? "" : readerMarket.GetString(readerMarket.GetOrdinal("last_name"));
                    customer.LastName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(customer.LastName);
                    customer.EmailAddress = readerMarket.IsDBNull(readerMarket.GetOrdinal("email_address")) ? "" : readerMarket.GetString(readerMarket.GetOrdinal("email_address"));

                    customer.ContactNumber = readerMarket.IsDBNull(readerMarket.GetOrdinal("contact_number")) ? "" : readerMarket.GetString(readerMarket.GetOrdinal("contact_number"));
                    customer.UserName = readerMarket.IsDBNull(readerMarket.GetOrdinal("user_name")) ? "" : readerMarket.GetString(readerMarket.GetOrdinal("user_name"));
                    customer.LastLoginDate = readerMarket.IsDBNull(readerMarket.GetOrdinal("last_login_date")) ? DateTime.Now : readerMarket.GetDateTime(readerMarket.GetOrdinal("last_login_date"));
                    customer.CustomerSignature = readerMarket.IsDBNull(readerMarket.GetOrdinal("customer_sign")) ? "" : readerMarket.GetString(readerMarket.GetOrdinal("customer_sign"));
                    customer.DirectorSignature = readerMarket.IsDBNull(readerMarket.GetOrdinal("director_signature")) ? "" : readerMarket.GetString(readerMarket.GetOrdinal("director_signature"));

                    customer.CustomerSignature = @"../../signs_data/" + customer.CustomerSignature;
                    customer.DirectorSignature = @"../../signs_data/" + customer.DirectorSignature;

                    customer.Company = readerMarket.IsDBNull(readerMarket.GetOrdinal("company_name")) ? "" : readerMarket.GetString(readerMarket.GetOrdinal("company_name"));

                    customer.GivenTitle = readerMarket.IsDBNull(readerMarket.GetOrdinal("given_title")) ? "" : readerMarket.GetString(readerMarket.GetOrdinal("given_title"));
                    customer.Address = readerMarket.IsDBNull(readerMarket.GetOrdinal("address")) ? "" : readerMarket.GetString(readerMarket.GetOrdinal("address"));
                    customer.Zipcode = readerMarket.IsDBNull(readerMarket.GetOrdinal("zipcode")) ? "" : readerMarket.GetString(readerMarket.GetOrdinal("zipcode"));
                    customer.City = readerMarket.IsDBNull(readerMarket.GetOrdinal("city")) ? "" : readerMarket.GetString(readerMarket.GetOrdinal("city"));
                    var sId = readerMarket.IsDBNull(readerMarket.GetOrdinal("state_id")) ? "0" : readerMarket.GetString(readerMarket.GetOrdinal("state_id"));
                    customer.StateId = int.Parse(sId);
                    customer.StateName = readerMarket.IsDBNull(readerMarket.GetOrdinal("state_name")) ? "" : readerMarket.GetString(readerMarket.GetOrdinal("state_name"));

                    customer.CellNumber = readerMarket.IsDBNull(readerMarket.GetOrdinal("cell_number")) ? "" : readerMarket.GetString(readerMarket.GetOrdinal("cell_number"));
                }

                con.Close();
            }



            SamsSettings sSettings = SamsSettingsController.GetSamsSettings();


            StringBuilder sbEmailMessage = new StringBuilder();
            //sbEmailMessage.Append("<div><b>Greetings " + customer.FirstName + " " + customer.LastName + ",</b><div>");

            sbEmailMessage.Append("<div>");
            sbEmailMessage.Append("Thank you for signing the NDA/Confidentiality agreement and registering with us. Use the link below to view and print an executed copy of the confidentiality agreement.. <br /><br />");
            sbEmailMessage.Append("Please click below link to view the agreement. <br /><br />");
            // sbEmailMessage.Append("Please find the link to put your signature. <br /><br />");
            sbEmailMessage.Append("<a href='" + Helper.HostName + "/RealEstate/ViewSignedAgreement?CustomerId=" + customer.CustomerId + "'>Click here to view and print the document</a>");
            sbEmailMessage.Append("</div>");

            sbEmailMessage.Append("<div>");
            sbEmailMessage.Append("Best Regards<br />");
            sbEmailMessage.Append("Sam's Holdings, LLC");
            sbEmailMessage.Append("</div>");


            //string fromEmail = "infosh@samsholdings.com";
            string fromEmail = sSettings.SmtpEmailAddress;

            MailMessage mailMessage = new MailMessage(fromEmail, customer.EmailAddress, "Signed Confidentiality Agreement", sbEmailMessage.ToString());
            mailMessage.IsBodyHtml = true;

            /*
            MailAddress copy = new MailAddress("arun@knowminal.com");
            mailMessage.CC.Add(copy);
            */

            //SmtpClient smtpClient = new SmtpClient("smtp.office365.com", 587);
            SmtpClient smtpClient = new SmtpClient(sSettings.SmtpServer, int.Parse(sSettings.SmtpPortNumber));
            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            if (sSettings.SmtpPassword.Trim().Length == 0)
            {
                smtpClient.Credentials = new NetworkCredential(fromEmail, "FMf5IY78JnSlolc2");
            }
            else
            {
                smtpClient.Credentials = new NetworkCredential(fromEmail, sSettings.SmtpPassword);
            }
            

            smtpClient.Send(mailMessage);









            return View(customer);

            //return RedirectToAction("GetCustomerAgreement", new { CustomerId = CustomerId });
            
        }

        public IActionResult ViewSignedAgreement(string CustomerId)
        {
            CustomerViewModel customer = new CustomerViewModel();
            string CS = DBConnection.ConnectionString;

            using (SqlConnection con = new SqlConnection(CS))
            {
                //GetUserForLogin
                SqlCommand cmdLogin = new SqlCommand("GetCustomerById", con);

                cmdLogin.Parameters.AddWithValue("customer_id", CustomerId);

                cmdLogin.CommandType = CommandType.StoredProcedure;
                con.Open();

                SqlDataReader readerMarket = cmdLogin.ExecuteReader();


                while (readerMarket.Read())
                {

                    customer.CustomerId = readerMarket.IsDBNull(readerMarket.GetOrdinal("customer_id")) ? 0 : readerMarket.GetInt32(readerMarket.GetOrdinal("customer_id"));
                    customer.FirstName = readerMarket.IsDBNull(readerMarket.GetOrdinal("first_name")) ? "" : readerMarket.GetString(readerMarket.GetOrdinal("first_name"));
                    customer.FirstName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(customer.FirstName);
                    customer.LastName = readerMarket.IsDBNull(readerMarket.GetOrdinal("last_name")) ? "" : readerMarket.GetString(readerMarket.GetOrdinal("last_name"));
                    customer.LastName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(customer.LastName);
                    customer.EmailAddress = readerMarket.IsDBNull(readerMarket.GetOrdinal("email_address")) ? "" : readerMarket.GetString(readerMarket.GetOrdinal("email_address"));

                    customer.ContactNumber = readerMarket.IsDBNull(readerMarket.GetOrdinal("contact_number")) ? "" : readerMarket.GetString(readerMarket.GetOrdinal("contact_number"));
                    customer.UserName = readerMarket.IsDBNull(readerMarket.GetOrdinal("user_name")) ? "" : readerMarket.GetString(readerMarket.GetOrdinal("user_name"));
                    customer.LastLoginDate = readerMarket.IsDBNull(readerMarket.GetOrdinal("last_login_date")) ? DateTime.Now : readerMarket.GetDateTime(readerMarket.GetOrdinal("last_login_date"));
                    customer.CustomerSignature = readerMarket.IsDBNull(readerMarket.GetOrdinal("customer_sign")) ? "" : readerMarket.GetString(readerMarket.GetOrdinal("customer_sign"));
                    customer.DirectorSignature = readerMarket.IsDBNull(readerMarket.GetOrdinal("director_signature")) ? "" : readerMarket.GetString(readerMarket.GetOrdinal("director_signature"));

                    customer.CustomerSignature = @"../../signs_data/" + customer.CustomerSignature;
                    customer.DirectorSignature = @"../../signs_data/" + customer.DirectorSignature;

                    customer.Company = readerMarket.IsDBNull(readerMarket.GetOrdinal("company_name")) ? "" : readerMarket.GetString(readerMarket.GetOrdinal("company_name"));

                    customer.GivenTitle = readerMarket.IsDBNull(readerMarket.GetOrdinal("given_title")) ? "" : readerMarket.GetString(readerMarket.GetOrdinal("given_title"));
                    customer.Address = readerMarket.IsDBNull(readerMarket.GetOrdinal("address")) ? "" : readerMarket.GetString(readerMarket.GetOrdinal("address"));
                    customer.Zipcode = readerMarket.IsDBNull(readerMarket.GetOrdinal("zipcode")) ? "" : readerMarket.GetString(readerMarket.GetOrdinal("zipcode"));
                    customer.City = readerMarket.IsDBNull(readerMarket.GetOrdinal("city")) ? "" : readerMarket.GetString(readerMarket.GetOrdinal("city"));
                    var sId = readerMarket.IsDBNull(readerMarket.GetOrdinal("state_id")) ? "0" : readerMarket.GetString(readerMarket.GetOrdinal("state_id"));
                    customer.StateId = int.Parse(sId);
                    customer.StateName = readerMarket.IsDBNull(readerMarket.GetOrdinal("state_name")) ? "" : readerMarket.GetString(readerMarket.GetOrdinal("state_name"));

                    customer.CellNumber = readerMarket.IsDBNull(readerMarket.GetOrdinal("cell_number")) ? "" : readerMarket.GetString(readerMarket.GetOrdinal("cell_number"));
                }

                con.Close();
            }
            return View(customer);

            //return RedirectToAction("GetCustomerAgreement", new { CustomerId = CustomerId });

        }


        public IActionResult GetCustomerAgreement(string CustomerId)
        {
            CustomerViewModel customer = new CustomerViewModel();
            string CS = DBConnection.ConnectionString;
            bool haveSignature = false;

            using (SqlConnection con = new SqlConnection(CS))
            {
                //GetUserForLogin
                SqlCommand cmdLogin = new SqlCommand("GetCustomerById", con);

                cmdLogin.Parameters.AddWithValue("customer_id", CustomerId);

                cmdLogin.CommandType = CommandType.StoredProcedure;
                con.Open();

                SqlDataReader readerMarket = cmdLogin.ExecuteReader();

                SamsSettings sSettings = SamsSettingsController.GetSamsSettings();

                while (readerMarket.Read())
                {
                    
                    customer.CustomerId = readerMarket.IsDBNull(readerMarket.GetOrdinal("customer_id")) ? 0 : readerMarket.GetInt32(readerMarket.GetOrdinal("customer_id"));
                    customer.FirstName = readerMarket.IsDBNull(readerMarket.GetOrdinal("first_name")) ? "" : readerMarket.GetString(readerMarket.GetOrdinal("first_name"));
                    customer.FirstName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(customer.FirstName);
                    customer.LastName = readerMarket.IsDBNull(readerMarket.GetOrdinal("last_name")) ? "" : readerMarket.GetString(readerMarket.GetOrdinal("last_name"));
                    customer.LastName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(customer.LastName);
                    customer.EmailAddress = readerMarket.IsDBNull(readerMarket.GetOrdinal("email_address")) ? "" : readerMarket.GetString(readerMarket.GetOrdinal("email_address"));

                    customer.ContactNumber = readerMarket.IsDBNull(readerMarket.GetOrdinal("contact_number")) ? "" : readerMarket.GetString(readerMarket.GetOrdinal("contact_number"));
                    customer.UserName = readerMarket.IsDBNull(readerMarket.GetOrdinal("user_name")) ? "" : readerMarket.GetString(readerMarket.GetOrdinal("user_name"));
                    customer.LastLoginDate = readerMarket.IsDBNull(readerMarket.GetOrdinal("last_login_date")) ? DateTime.Now : readerMarket.GetDateTime(readerMarket.GetOrdinal("last_login_date"));
                    customer.CustomerSignature = readerMarket.IsDBNull(readerMarket.GetOrdinal("customer_sign")) ? "" : readerMarket.GetString(readerMarket.GetOrdinal("customer_sign"));

                    if(customer.CustomerSignature.Trim().Length == 0)
                    {
                        haveSignature = false;
                    }
                    else
                    {
                        haveSignature = true;
                    }

                    customer.CustomerSignature = @"../../signs_data/" + customer.CustomerSignature;

                    customer.DirectorSignature = readerMarket.IsDBNull(readerMarket.GetOrdinal("director_signature")) ? "" : readerMarket.GetString(readerMarket.GetOrdinal("director_signature"));
                    customer.DirectorSignature = @"../../signs_data/" + customer.DirectorSignature;


                    customer.Company = readerMarket.IsDBNull(readerMarket.GetOrdinal("company_name")) ? "" : readerMarket.GetString(readerMarket.GetOrdinal("company_name"));

                    customer.GivenTitle = readerMarket.IsDBNull(readerMarket.GetOrdinal("given_title")) ? "" : readerMarket.GetString(readerMarket.GetOrdinal("given_title"));
                    customer.Address = readerMarket.IsDBNull(readerMarket.GetOrdinal("address")) ? "" : readerMarket.GetString(readerMarket.GetOrdinal("address"));
                    customer.Zipcode = readerMarket.IsDBNull(readerMarket.GetOrdinal("zipcode")) ? "" : readerMarket.GetString(readerMarket.GetOrdinal("zipcode"));
                    customer.City = readerMarket.IsDBNull(readerMarket.GetOrdinal("city")) ? "" : readerMarket.GetString(readerMarket.GetOrdinal("city"));
                    var sId = readerMarket.IsDBNull(readerMarket.GetOrdinal("state_id")) ? "0" : readerMarket.GetString(readerMarket.GetOrdinal("state_id"));
                    customer.StateId = int.Parse(sId);
                    customer.StateName = readerMarket.IsDBNull(readerMarket.GetOrdinal("state_name")) ? "" : readerMarket.GetString(readerMarket.GetOrdinal("state_name"));

                    customer.CellNumber = readerMarket.IsDBNull(readerMarket.GetOrdinal("cell_number")) ? "" : readerMarket.GetString(readerMarket.GetOrdinal("cell_number"));
                    customer.RealEstateDirectorName = sSettings.RealEstateDirectorName;
                }

                

                con.Close();
            }
            if (!haveSignature)
            {
                return View(customer);
            }
            else
            {
                return RedirectToAction("ViewSignedAgreement", new { CustomerId = CustomerId });
            }
        }

        

        [HttpPost]
        public bool CheckDuplicateUserName(string UserName)
        {
            bool customerExists = false;
            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                //GetUserForLogin
                SqlCommand cmdLogin = new SqlCommand("GetCustomerByUserName", con);

                cmdLogin.Parameters.AddWithValue("user_name", UserName);

                cmdLogin.CommandType = CommandType.StoredProcedure;
                con.Open();

                SqlDataReader readerMarket = cmdLogin.ExecuteReader();

                while (readerMarket.Read())
                {
                    
                    int cId = readerMarket.IsDBNull(readerMarket.GetOrdinal("customer_id")) ? 0 : readerMarket.GetInt32(readerMarket.GetOrdinal("customer_id"));
                    if(cId > 0)
                    {
                        customerExists = true;
                    }
                    else
                    {
                        customerExists = false;
                    }
                }

                con.Close();
            }

            return customerExists;
        }


        public IActionResult GetCustomerById(string CustomerId)
        {
            CustomerViewModel customer = new CustomerViewModel();
            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                //GetUserForLogin
                SqlCommand cmdLogin = new SqlCommand("GetCustomerById", con);

                cmdLogin.Parameters.AddWithValue("customer_id", CustomerId);

                cmdLogin.CommandType = CommandType.StoredProcedure;
                con.Open();

                SqlDataReader readerMarket = cmdLogin.ExecuteReader();

                while (readerMarket.Read())
                {

                    customer.CustomerId = readerMarket.IsDBNull(readerMarket.GetOrdinal("customer_id")) ? 0 : readerMarket.GetInt32(readerMarket.GetOrdinal("customer_id"));
                    customer.FirstName = readerMarket.IsDBNull(readerMarket.GetOrdinal("first_name")) ? "" : readerMarket.GetString(readerMarket.GetOrdinal("first_name"));
                    customer.LastName = readerMarket.IsDBNull(readerMarket.GetOrdinal("last_name")) ? "" : readerMarket.GetString(readerMarket.GetOrdinal("last_name"));
                    customer.EmailAddress = readerMarket.IsDBNull(readerMarket.GetOrdinal("email_address")) ? "" : readerMarket.GetString(readerMarket.GetOrdinal("email_address"));

                    customer.ContactNumber = readerMarket.IsDBNull(readerMarket.GetOrdinal("contact_number")) ? "" : readerMarket.GetString(readerMarket.GetOrdinal("contact_number"));
                    customer.UserName = readerMarket.IsDBNull(readerMarket.GetOrdinal("user_name")) ? "" : readerMarket.GetString(readerMarket.GetOrdinal("user_name"));
                    customer.LastLoginDate = readerMarket.IsDBNull(readerMarket.GetOrdinal("last_login_date")) ? DateTime.Now : readerMarket.GetDateTime(readerMarket.GetOrdinal("last_login_date"));
                    customer.CustomerSignature = readerMarket.IsDBNull(readerMarket.GetOrdinal("customer_sign")) ? "" : readerMarket.GetString(readerMarket.GetOrdinal("customer_sign"));

                    customer.Company = readerMarket.IsDBNull(readerMarket.GetOrdinal("company_name")) ? "" : readerMarket.GetString(readerMarket.GetOrdinal("company_name"));

                    customer.GivenTitle = readerMarket.IsDBNull(readerMarket.GetOrdinal("given_title")) ? "" : readerMarket.GetString(readerMarket.GetOrdinal("given_title"));
                    customer.Address = readerMarket.IsDBNull(readerMarket.GetOrdinal("address")) ? "" : readerMarket.GetString(readerMarket.GetOrdinal("address"));
                    customer.Zipcode = readerMarket.IsDBNull(readerMarket.GetOrdinal("zipcode")) ? "" : readerMarket.GetString(readerMarket.GetOrdinal("zipcode"));
                    customer.City = readerMarket.IsDBNull(readerMarket.GetOrdinal("city")) ? "" : readerMarket.GetString(readerMarket.GetOrdinal("city"));
                    var sId = readerMarket.IsDBNull(readerMarket.GetOrdinal("state_id")) ? "0" : readerMarket.GetString(readerMarket.GetOrdinal("state_id"));
                    customer.StateId = int.Parse(sId);
                    customer.StateName = readerMarket.IsDBNull(readerMarket.GetOrdinal("state_name")) ? "" : readerMarket.GetString(readerMarket.GetOrdinal("state_name"));
                    
                }

                con.Close();
            }

            return View(customer);
        }

        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SendResetPasswordLink(string emailAddress)
        {
            CustomerViewModel customer=new CustomerViewModel();
            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                //GetUserForLogin
                SqlCommand cmdLogin = new SqlCommand("GetCstoreCustomerByEmailAddress", con);

                cmdLogin.Parameters.AddWithValue("email_address", emailAddress);
                cmdLogin.CommandType = CommandType.StoredProcedure;
                con.Open();

                SqlDataReader reader = cmdLogin.ExecuteReader();
                List<AdditionalFilesViewModel> c_storeFiles = new List<AdditionalFilesViewModel>();
                while (reader.Read())
                {
                    customer.CustomerId = reader.IsDBNull(reader.GetOrdinal("customer_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("customer_id"));
                    customer.FirstName = reader.IsDBNull(reader.GetOrdinal("first_name")) ? "" : reader.GetString(reader.GetOrdinal("first_name"));

                    customer.LastName = reader.IsDBNull(reader.GetOrdinal("last_name")) ? "" : reader.GetString(reader.GetOrdinal("last_name"));
                    customer.EmailAddress = reader.IsDBNull(reader.GetOrdinal("email_address")) ? "" : reader.GetString(reader.GetOrdinal("email_address"));
                    customer.ContactNumber = reader.IsDBNull(reader.GetOrdinal("contact_number")) ? "" : reader.GetString(reader.GetOrdinal("contact_number"));
                    customer.UserName = reader.IsDBNull(reader.GetOrdinal("user_name")) ? "" : reader.GetString(reader.GetOrdinal("user_name"));
                    customer.Password = reader.IsDBNull(reader.GetOrdinal("customer_password")) ? "" : reader.GetString(reader.GetOrdinal("customer_password"));
                    
                    customer.Password = StringFunctions.Encrypt(customer.Password, SiteSettings.PasswordKey);

                    customer.CreatedDate = reader.IsDBNull(reader.GetOrdinal("created_date")) ? DateTime.Now : reader.GetDateTime(reader.GetOrdinal("created_date"));
                    customer.LastLoginDate = reader.IsDBNull(reader.GetOrdinal("last_login_date")) ? DateTime.Now : reader.GetDateTime(reader.GetOrdinal("last_login_date"));
                    customer.ResetPasswordId = reader.IsDBNull(reader.GetOrdinal("reset_password_id")) ? "" : reader.GetString(reader.GetOrdinal("reset_password_id"));
                    //reset_password_id


                }
                con.Close();
            }

            if (customer.CustomerId > 0)
            {




                SamsSettings sSettings = SamsSettingsController.GetSamsSettings();

                StringBuilder sbEmailMessage = new StringBuilder();
                //sbEmailMessage.Append("<div><b>Greetings " + customer.FirstName + " " + customer.LastName + ",</b><div>");

                sbEmailMessage.Append("<div>");
                // sbEmailMessage.Append("Hi " + customer.FirstName + " " + customer.LastName + "<br /><br />");

                sbEmailMessage.Append("User Name : <b>" + customer.UserName + "</b> <br /><br />");

                sbEmailMessage.Append("Please click the link below to reset your password. <br /><br />");
                // sbEmailMessage.Append("Please find the link to put your signature. <br /><br />");
                sbEmailMessage.Append("<a href='" + Helper.HostName + "/RealEstate/ResetPasswordLink?s=" + customer.ResetPasswordId + "'>Click here to reset password</a>");
                sbEmailMessage.Append("</div>");

                sbEmailMessage.Append("<div>");
                sbEmailMessage.Append("Best Regards<br />");
                sbEmailMessage.Append("Sam's Holdings, LLC");
                sbEmailMessage.Append("</div>");


                //string fromEmail = "infosh@samsholdings.com";
                string fromEmail = sSettings.SmtpEmailAddress;
                MailMessage mailMessage = new MailMessage(fromEmail, customer.EmailAddress, "Reset Password request from Sam’s Holdings LLC.", sbEmailMessage.ToString());
                mailMessage.IsBodyHtml = true;

                /*
                MailAddress copy = new MailAddress("arun@knowminal.com");
                mailMessage.CC.Add(copy);
                */

                //SmtpClient smtpClient = new SmtpClient("smtp.office365.com", 587);
                SmtpClient smtpClient = new SmtpClient(sSettings.SmtpServer, int.Parse(sSettings.SmtpPortNumber));
                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = false;
                //smtpClient.Credentials = new NetworkCredential(fromEmail, "FMf5IY78JnSlolc2");
                if (sSettings.SmtpPassword.Trim().Length == 0)
                {
                    sSettings.SmtpPassword = "FMf5IY78JnSlolc2";
                }
                smtpClient.Credentials = new NetworkCredential(fromEmail, sSettings.SmtpPassword);

                smtpClient.Send(mailMessage);

                return View();
            }
            else
            {
                return RedirectToAction("ViewCStore", new { alerts = "Given email address is not registered with us" });
            }
        }

        public IActionResult ResetPasswordLink(string s)
        {
            ViewBag.resetId = s;
            return View();
        }

        [HttpPost]
        public IActionResult ResetPassword(string reset_password_id, string cPassword)
        {
            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("ResetPassword", con);
                cmd.Parameters.AddWithValue("reset_password_id", reset_password_id);
                cmd.Parameters.AddWithValue("customer_password", cPassword);

                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                cmd.ExecuteNonQuery();

                con.Close();
            }
            return View();
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

        List<RegionViewModel> GetRegionList(int stateId)
        {
            List<RegionViewModel> regionList = new List<RegionViewModel>();

            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("GetRegionList", con);
                cmd.CommandType = CommandType.StoredProcedure;

                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    RegionViewModel regionItem = new RegionViewModel();
                    regionItem.RegionId = reader.IsDBNull(reader.GetOrdinal("region_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("region_id"));
                    regionItem.RegionName = reader.IsDBNull(reader.GetOrdinal("region_name")) ? "" : reader.GetString(reader.GetOrdinal("region_name"));
                    regionItem.StateId = reader.IsDBNull(reader.GetOrdinal("state_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("state_id"));

                    if (regionItem.StateId == stateId)
                    {
                        regionList.Add(regionItem);
                    }

                }
                con.Close();
            }

            return regionList;

        }

        [HttpPost]
        public ActionResult DoLoginFromNetlease(CustomerViewModel customer)
        {

            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                //GetUserForLogin
                SqlCommand cmdLogin = new SqlCommand("GetUserForLogin", con);
                //customer.Password = StringFunctions.Encrypt(customer.Password, SiteSettings.PasswordKey);
                cmdLogin.Parameters.AddWithValue("user_name", customer.UserName);
                cmdLogin.Parameters.AddWithValue("customer_password", customer.Password);
                cmdLogin.CommandType = CommandType.StoredProcedure;
                con.Open();

                SqlDataReader reader = cmdLogin.ExecuteReader();
                List<AdditionalFilesViewModel> c_storeFiles = new List<AdditionalFilesViewModel>();
                while (reader.Read())
                {
                    customer.CustomerId = reader.IsDBNull(reader.GetOrdinal("customer_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("customer_id"));
                    customer.FirstName = reader.IsDBNull(reader.GetOrdinal("first_name")) ? "" : reader.GetString(reader.GetOrdinal("first_name"));

                    customer.LastName = reader.IsDBNull(reader.GetOrdinal("last_name")) ? "" : reader.GetString(reader.GetOrdinal("last_name"));
                    customer.EmailAddress = reader.IsDBNull(reader.GetOrdinal("email_address")) ? "" : reader.GetString(reader.GetOrdinal("email_address"));
                    customer.ContactNumber = reader.IsDBNull(reader.GetOrdinal("contact_number")) ? "" : reader.GetString(reader.GetOrdinal("contact_number"));
                    customer.UserName = reader.IsDBNull(reader.GetOrdinal("user_name")) ? "" : reader.GetString(reader.GetOrdinal("user_name"));
                    customer.Password = reader.IsDBNull(reader.GetOrdinal("customer_password")) ? "" : reader.GetString(reader.GetOrdinal("customer_password"));
                    customer.SignedStatus = reader.IsDBNull(reader.GetOrdinal("signed_status")) ? "" : reader.GetString(reader.GetOrdinal("signed_status"));

                    customer.CreatedDate = reader.IsDBNull(reader.GetOrdinal("created_date")) ? DateTime.Now : reader.GetDateTime(reader.GetOrdinal("created_date"));
                    customer.LastLoginDate = reader.IsDBNull(reader.GetOrdinal("last_login_date")) ? DateTime.Now : reader.GetDateTime(reader.GetOrdinal("last_login_date"));



                    HttpContext.Session.SetObjectAsJson("LoggedInUser", customer);

                }

                con.Close();



            }

            if (customer.CustomerId == 0)
            {
                TempData["ErrorMessage"] = "Wrong Username/ Password";

            }

            return RedirectToAction("ViewNetleaseProperty", new { propertyId = customer.LoginPropertyId });
        }

    }
}