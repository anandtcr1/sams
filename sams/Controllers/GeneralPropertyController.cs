using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using sams.Common;
using sams.Models;

namespace sams.Controllers
{
    public class GeneralPropertyController : Controller
    {
        public IActionResult Index()
        {
            List<SiteDetails> surplusPropertiesList = new List<SiteDetails>();

            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("GetSubittedPropertyListByCategory", con);
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

                    surplusPropertiesList.Add(steDetails);
                }
                con.Close();
            }

            return View(surplusPropertiesList);
        }

        public IActionResult EditProperty(int propertyId)
        {
            SiteDetails steDetails = new SiteDetails();

            List<StateDetails> stateList = new List<StateDetails>();
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
                SqlCommand cmd = new SqlCommand("GetSubittedPropertyItemById", con);

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


                }
                con.Close();

                steDetails.StateList = stateList;
                steDetails.MarketList = marketList;

                return View(steDetails);
            }
        }

        [HttpPost]
        public ActionResult EditProperty(SiteDetails siteDetails)
        {
            int siteDetailsId = siteDetails.SiteDetailsId;
            string CS = DBConnection.ConnectionString;

            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("SaveSubittedPropertyAdmin", con);
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
                cmd.Parameters.AddWithValue("property_type", SamsPropertyType.General);

                siteDetailsId = int.Parse(cmd.ExecuteScalar().ToString());

                siteDetails.SiteDetailsId = siteDetailsId;

                con.Close();
            }


            return View();
        }

    }
}