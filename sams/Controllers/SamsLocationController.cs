using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using sams.Models;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Spire.Xls;
using System.Net;
using System.Text;
using System.Xml.Linq;
using Microsoft.AspNetCore.Http;
using sams.Common;

namespace sams.Controllers
{
    public class SamsLocationController : Controller
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        public SamsLocationController(IWebHostEnvironment hostEnvironment)
        {
            webHostEnvironment = hostEnvironment;
        }

        public IActionResult Index()
        {
            var samsLocationsList = new List<SamsLocationsViewModel>();
            string CS = DBConnection.ConnectionString;

            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("GetSamsLocations", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var samsLocations = new SamsLocationsViewModel();
                    samsLocations.LocationId = reader.IsDBNull(reader.GetOrdinal("location_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("location_id"));
                    samsLocations.SHAssetId = reader.IsDBNull(reader.GetOrdinal("sh_asset_id")) ? "" : reader.GetString(reader.GetOrdinal("sh_asset_id"));

                    samsLocations.LocationAddress = reader.IsDBNull(reader.GetOrdinal("location_address")) ? "" : reader.GetString(reader.GetOrdinal("location_address"));
                    samsLocations.City = reader.IsDBNull(reader.GetOrdinal("city")) ? "" : reader.GetString(reader.GetOrdinal("city"));
                    samsLocations.State = reader.IsDBNull(reader.GetOrdinal("state")) ? "" : reader.GetString(reader.GetOrdinal("state"));
                    samsLocations.Zipcode = reader.IsDBNull(reader.GetOrdinal("zipcode")) ? "" : reader.GetString(reader.GetOrdinal("zipcode"));
                    samsLocations.County= reader.IsDBNull(reader.GetOrdinal("county")) ? "" : reader.GetString(reader.GetOrdinal("county"));
                    samsLocations.BusinessName = reader.IsDBNull(reader.GetOrdinal("business_name")) ? "" : reader.GetString(reader.GetOrdinal("business_name"));
                    samsLocations.Latitude = reader.IsDBNull(reader.GetOrdinal("latitude")) ? "" : reader.GetString(reader.GetOrdinal("latitude"));
                    samsLocations.Longitude = reader.IsDBNull(reader.GetOrdinal("longitude")) ? "" : reader.GetString(reader.GetOrdinal("longitude"));

                    samsLocationsList.Add(samsLocations);

                }
            }

            return View(samsLocationsList);
        }

        
        public IActionResult GetSamsLocation(int locationId)
        {
            var samsLocation = new SamsLocationsViewModel();
            string CS = DBConnection.ConnectionString;

            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("GetSamsLocationsById", con);
                cmd.Parameters.AddWithValue("location_id", locationId);

                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    
                    samsLocation.LocationId = reader.IsDBNull(reader.GetOrdinal("location_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("location_id"));
                    samsLocation.SHAssetId = reader.IsDBNull(reader.GetOrdinal("sh_asset_id")) ? "" : reader.GetString(reader.GetOrdinal("sh_asset_id"));

                    samsLocation.LocationAddress = reader.IsDBNull(reader.GetOrdinal("location_address")) ? "" : reader.GetString(reader.GetOrdinal("location_address"));
                    samsLocation.City = reader.IsDBNull(reader.GetOrdinal("city")) ? "" : reader.GetString(reader.GetOrdinal("city"));
                    samsLocation.State = reader.IsDBNull(reader.GetOrdinal("state")) ? "" : reader.GetString(reader.GetOrdinal("state"));
                    samsLocation.Zipcode = reader.IsDBNull(reader.GetOrdinal("zipcode")) ? "" : reader.GetString(reader.GetOrdinal("zipcode"));
                    samsLocation.County = reader.IsDBNull(reader.GetOrdinal("county")) ? "" : reader.GetString(reader.GetOrdinal("county"));
                    samsLocation.BusinessName = reader.IsDBNull(reader.GetOrdinal("business_name")) ? "" : reader.GetString(reader.GetOrdinal("business_name"));

                    samsLocation.Latitude = reader.IsDBNull(reader.GetOrdinal("latitude")) ? "" : reader.GetString(reader.GetOrdinal("latitude"));
                    samsLocation.Longitude = reader.IsDBNull(reader.GetOrdinal("longitude")) ? "" : reader.GetString(reader.GetOrdinal("longitude"));

                }
            }

            return View(samsLocation);
        }

        [HttpPost]
        public IActionResult ManageSamsLocation(SamsLocationsViewModel samsLocation)
        {
            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("SaveSamsLocation", con);
                cmd.Parameters.AddWithValue("location_id", samsLocation.LocationId);

                cmd.Parameters.AddWithValue("sh_asset_id", samsLocation.SHAssetId);
                cmd.Parameters.AddWithValue("location_address", samsLocation.LocationAddress);
                cmd.Parameters.AddWithValue("city", samsLocation.City);

                cmd.Parameters.AddWithValue("state", samsLocation.State);
                cmd.Parameters.AddWithValue("zipcode", samsLocation.Zipcode);
                cmd.Parameters.AddWithValue("county", samsLocation.County);

                cmd.Parameters.AddWithValue("business_name", samsLocation.BusinessName);

                cmd.Parameters.AddWithValue("latitude", samsLocation.Latitude);
                cmd.Parameters.AddWithValue("longitude", samsLocation.Longitude);

                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                samsLocation.LocationId= int.Parse(cmd.ExecuteScalar().ToString());
                con.Close();
            }

            return RedirectToAction("Index");
        }

        public IActionResult DeleteSamsLocation(int locationId)
        {
            var samsLocation = new SamsLocationsViewModel();
            string CS = DBConnection.ConnectionString;

            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("DeleteSamsLocation", con);
                cmd.Parameters.AddWithValue("location_id", locationId);

                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                cmd.ExecuteNonQuery();
                
            }

            //return View(samsLocation);
            return RedirectToAction("Index");
        }

        public IActionResult ExportExcel()
        {

            //string fileName = Path.GetFileNameWithoutExtension(@"\\OpsVsAdp\\Files\\Daily\\TempHours.xlsx");
            var filePath = Path.Combine(webHostEnvironment.WebRootPath + @"/templates", "sh_asset_list_template.xlsx");

            string fullFileName = Path.Combine(webHostEnvironment.WebRootPath + @"/templates", "sh_asset_list_template.xlsx");
            string fullToFileName = Path.Combine(webHostEnvironment.WebRootPath + @"/templates", "sh_asset_list.xlsx");

            Workbook wrkBook = new Workbook();
            wrkBook.LoadFromFile(fullFileName);
            Worksheet sheet = wrkBook.Worksheets[0];


            var samsLocationsList = new List<SamsLocationsViewModel>();
            string CS = DBConnection.ConnectionString;

            using (SqlConnection con = new SqlConnection(CS))
            {
                string colSlNo = "B", colSxcId = "C", colLocationAddress = "D", colCity = "E", colState = "F", colZipCode = "G";
                string colCounty = "H", colStatus = "I", colYearOpened = "J", colStoreModel = "K", colSqrFeet = "L", colProtoType = "M";
                string colBusinessName = "N", colFuelBrand = "O";

                SqlCommand cmd = new SqlCommand("GetSamsLocations", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                int i = 5;
                int j = 1;
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var samsLocations = new SamsLocationsViewModel();
                    samsLocations.LocationId = reader.IsDBNull(reader.GetOrdinal("location_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("location_id"));
                    samsLocations.SHAssetId = reader.IsDBNull(reader.GetOrdinal("sh_asset_id")) ? "" : reader.GetString(reader.GetOrdinal("sh_asset_id"));

                    samsLocations.LocationAddress = reader.IsDBNull(reader.GetOrdinal("location_address")) ? "" : reader.GetString(reader.GetOrdinal("location_address"));
                    samsLocations.City = reader.IsDBNull(reader.GetOrdinal("city")) ? "" : reader.GetString(reader.GetOrdinal("city"));
                    samsLocations.State = reader.IsDBNull(reader.GetOrdinal("state")) ? "" : reader.GetString(reader.GetOrdinal("state"));
                    samsLocations.Zipcode = reader.IsDBNull(reader.GetOrdinal("zipcode")) ? "" : reader.GetString(reader.GetOrdinal("zipcode"));
                    samsLocations.County = reader.IsDBNull(reader.GetOrdinal("county")) ? "" : reader.GetString(reader.GetOrdinal("county"));
                    samsLocations.BusinessName = reader.IsDBNull(reader.GetOrdinal("business_name")) ? "" : reader.GetString(reader.GetOrdinal("business_name"));


                    string cellSlNo = colSlNo + i.ToString();
                    string cellSxcId = colSxcId + i.ToString();
                    string cellLocationAddress = colLocationAddress + i.ToString();
                    string cellCity = colCity + i.ToString();
                    string cellState  =colState + i.ToString();
                    string cellZipCode = colZipCode + i.ToString();
                    string cellCounty =colCounty + i.ToString();
                    string cellStatus =colStatus + i.ToString();
                    

                    sheet.Range[cellSlNo].Value = j.ToString();
                    sheet.Range[cellSxcId].Value = samsLocations.SHAssetId;
                    sheet.Range[cellLocationAddress].Value = samsLocations.LocationAddress;
                    sheet.Range[cellCity].Value = samsLocations.City;
                    sheet.Range[cellState].Value = samsLocations.State;
                    sheet.Range[cellZipCode].Value = samsLocations.Zipcode;
                    sheet.Range[cellCounty].Value = samsLocations.County;
                    sheet.Range[cellStatus].Value = samsLocations.BusinessName;

                    j++;
                    i++;

                }
                con.Close();
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


        public IActionResult LocationToMap()
        {
            var samsLocationsList = new List<SamsLocationsViewModel>();
            string CS = DBConnection.ConnectionString;

            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("GetSamsLocations", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var samsLocations = new SamsLocationsViewModel();
                    samsLocations.LocationId = reader.IsDBNull(reader.GetOrdinal("location_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("location_id"));
                    samsLocations.SHAssetId = reader.IsDBNull(reader.GetOrdinal("sh_asset_id")) ? "" : reader.GetString(reader.GetOrdinal("sh_asset_id"));

                    samsLocations.LocationAddress = reader.IsDBNull(reader.GetOrdinal("location_address")) ? "" : reader.GetString(reader.GetOrdinal("location_address"));
                    samsLocations.City = reader.IsDBNull(reader.GetOrdinal("city")) ? "" : reader.GetString(reader.GetOrdinal("city"));
                    samsLocations.State = reader.IsDBNull(reader.GetOrdinal("state")) ? "" : reader.GetString(reader.GetOrdinal("state"));
                    samsLocations.Zipcode = reader.IsDBNull(reader.GetOrdinal("zipcode")) ? "" : reader.GetString(reader.GetOrdinal("zipcode"));
                    samsLocations.County = reader.IsDBNull(reader.GetOrdinal("county")) ? "" : reader.GetString(reader.GetOrdinal("county"));
                    samsLocations.BusinessName = reader.IsDBNull(reader.GetOrdinal("business_name")) ? "" : reader.GetString(reader.GetOrdinal("business_name"));


                    samsLocations.Latitude = reader.IsDBNull(reader.GetOrdinal("latitude")) ? "" : reader.GetString(reader.GetOrdinal("latitude"));
                    samsLocations.Longitude = reader.IsDBNull(reader.GetOrdinal("longitude")) ? "" : reader.GetString(reader.GetOrdinal("longitude"));

                    samsLocationsList.Add(samsLocations);

                }
            }

            return View(samsLocationsList);
        }

        [HttpPost]
        public ActionResult UploadAssetFile(IFormFile FileUpload)
        {
            var uniqueFileName = Helper.GetUniqueFileName(FileUpload.FileName);
            var filePath = Path.Combine(webHostEnvironment.WebRootPath + @"/AssetList", uniqueFileName);
            using (var stream = System.IO.File.Create(filePath))
            {
                FileUpload.CopyTo(stream);
            }

            string csvData = System.IO.File.ReadAllText(filePath);

            foreach (string row in csvData.Split('\n'))
            {
                if (!string.IsNullOrEmpty(row))
                {
                    /*
                     *  samsLocations.LocationId = reader.IsDBNull(reader.GetOrdinal("location_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("location_id"));
                    samsLocations.SHAssetId = reader.IsDBNull(reader.GetOrdinal("sh_asset_id")) ? "" : reader.GetString(reader.GetOrdinal("sh_asset_id"));

                    samsLocations.LocationAddress = reader.IsDBNull(reader.GetOrdinal("location_address")) ? "" : reader.GetString(reader.GetOrdinal("location_address"));
                    samsLocations.City = reader.IsDBNull(reader.GetOrdinal("city")) ? "" : reader.GetString(reader.GetOrdinal("city"));
                    samsLocations.State = reader.IsDBNull(reader.GetOrdinal("state")) ? "" : reader.GetString(reader.GetOrdinal("state"));
                    samsLocations.Zipcode = reader.IsDBNull(reader.GetOrdinal("zipcode")) ? "" : reader.GetString(reader.GetOrdinal("zipcode"));
                    samsLocations.County = reader.IsDBNull(reader.GetOrdinal("county")) ? "" : reader.GetString(reader.GetOrdinal("county"));
                    samsLocations.BusinessName = reader.IsDBNull(reader.GetOrdinal("business_name")) ? "" : reader.GetString(reader.GetOrdinal("business_name"));


                    samsLocations.Latitude = reader.IsDBNull(reader.GetOrdinal("latitude")) ? "" : reader.GetString(reader.GetOrdinal("latitude"));
                    samsLocations.Longitude = reader.IsDBNull(reader.GetOrdinal("longitude")) ? "" : reader.GetString(reader.GetOrdinal("longitude"));

                     */

                    try
                    {
                        string shId = row.Split(',')[0];
                        string locationAddress = row.Split(',')[1];
                        string city = row.Split(',')[2];
                        string state = row.Split(',')[3];
                        string county = row.Split(',')[4];
                        string zipCode = row.Split(',')[5];
                        string businessName = row.Split(',')[6];


                        var samsLocations = new SamsLocationsViewModel();
                        samsLocations.LocationId = 0;
                        samsLocations.SHAssetId = shId;

                        samsLocations.LocationAddress = locationAddress;
                        samsLocations.City = city;
                        samsLocations.State = state;
                        samsLocations.Zipcode = zipCode;
                        samsLocations.County = county;
                        samsLocations.BusinessName = businessName;

                        string strAddress = samsLocations.LocationAddress + ", " + samsLocations.County + ", " + samsLocations.City;
                        string YOUR_API_KEY = "AIzaSyByxJE-OM4Lv77gVdAYJAfKOiDpD6H9ofg";

                        string requestUri = string.Format("https://maps.googleapis.com/maps/api/geocode/xml?key={1}&address={0}&sensor=false", Uri.EscapeDataString(strAddress), YOUR_API_KEY);

                        WebRequest request = WebRequest.Create(requestUri);
                        WebResponse response = request.GetResponse();
                        XDocument xdoc = XDocument.Load(response.GetResponseStream());

                        XElement result = xdoc.Element("GeocodeResponse").Element("result");
                        XElement locationElement = result.Element("geometry").Element("location");
                        XElement lat = locationElement.Element("lat");
                        XElement lng = locationElement.Element("lng");

                        string strLatutude = lat.Value;
                        string strLongitude = lng.Value;

                        samsLocations.Latitude = strLatutude;
                        samsLocations.Longitude = strLongitude;

                        string CS = DBConnection.ConnectionString;
                        using (SqlConnection con = new SqlConnection(CS))
                        {
                            SqlCommand cmd = new SqlCommand("SaveSamsLocation", con);
                            cmd.Parameters.AddWithValue("location_id", samsLocations.LocationId);

                            cmd.Parameters.AddWithValue("sh_asset_id", samsLocations.SHAssetId);
                            cmd.Parameters.AddWithValue("location_address", samsLocations.LocationAddress);
                            cmd.Parameters.AddWithValue("city", samsLocations.City);

                            cmd.Parameters.AddWithValue("state", samsLocations.State);
                            cmd.Parameters.AddWithValue("zipcode", samsLocations.Zipcode);
                            cmd.Parameters.AddWithValue("county", samsLocations.County);

                            cmd.Parameters.AddWithValue("business_name", samsLocations.BusinessName);

                            cmd.Parameters.AddWithValue("latitude", samsLocations.Latitude);
                            cmd.Parameters.AddWithValue("longitude", samsLocations.Longitude);

                            cmd.CommandType = CommandType.StoredProcedure;
                            con.Open();

                            samsLocations.LocationId = int.Parse(cmd.ExecuteScalar().ToString());
                            con.Close();
                        }
                    }
                    catch
                    {

                    }
                    
                        
                }
            }

            return RedirectToAction("Index", "SamsLocation");
        }

        public IActionResult ClearSamsLocations()
        {
            var samsLocationsList = new List<SamsLocationsViewModel>();
            string CS = DBConnection.ConnectionString;

            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("ClearSamsLocations", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                cmd.ExecuteNonQuery();
                con.Close();
            }

            return RedirectToAction("Index");
        }

        public IActionResult GetLatitudeAndLongitude()
        {
            var samsLocationsList = new List<SamsLocationsViewModel>();
            string CS = DBConnection.ConnectionString;

            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("GetSamsLocations", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var samsLocations = new SamsLocationsViewModel();
                    samsLocations.LocationId = reader.IsDBNull(reader.GetOrdinal("location_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("location_id"));
                    samsLocations.SHAssetId = reader.IsDBNull(reader.GetOrdinal("sh_asset_id")) ? "" : reader.GetString(reader.GetOrdinal("sh_asset_id"));

                    samsLocations.LocationAddress = reader.IsDBNull(reader.GetOrdinal("location_address")) ? "" : reader.GetString(reader.GetOrdinal("location_address"));
                    samsLocations.City = reader.IsDBNull(reader.GetOrdinal("city")) ? "" : reader.GetString(reader.GetOrdinal("city"));
                    samsLocations.State = reader.IsDBNull(reader.GetOrdinal("state")) ? "" : reader.GetString(reader.GetOrdinal("state"));
                    samsLocations.Zipcode = reader.IsDBNull(reader.GetOrdinal("zipcode")) ? "" : reader.GetString(reader.GetOrdinal("zipcode"));
                    samsLocations.County = reader.IsDBNull(reader.GetOrdinal("county")) ? "" : reader.GetString(reader.GetOrdinal("county"));
                    samsLocations.BusinessName = reader.IsDBNull(reader.GetOrdinal("business_name")) ? "" : reader.GetString(reader.GetOrdinal("business_name"));


                    string strAddress = samsLocations.LocationAddress + ", " + samsLocations.County+", " + samsLocations.City;
                    string YOUR_API_KEY = "AIzaSyByxJE-OM4Lv77gVdAYJAfKOiDpD6H9ofg";

                    string requestUri = string.Format("https://maps.googleapis.com/maps/api/geocode/xml?key={1}&address={0}&sensor=false", Uri.EscapeDataString(strAddress), YOUR_API_KEY);

                    WebRequest request = WebRequest.Create(requestUri);
                    WebResponse response = request.GetResponse();
                    XDocument xdoc = XDocument.Load(response.GetResponseStream());

                    XElement result = xdoc.Element("GeocodeResponse").Element("result");
                    XElement locationElement = result.Element("geometry").Element("location");
                    XElement lat = locationElement.Element("lat");
                    XElement lng = locationElement.Element("lng");
                    
                    string strLatutude = lat.Value;
                    string strLongitude = lng.Value;

                    samsLocations.Latitude = strLatutude;
                    samsLocations.Longitude = strLongitude;

                    samsLocationsList.Add(samsLocations);

                }
                con.Close();

                foreach(SamsLocationsViewModel samsLocation in samsLocationsList)
                {
                    SqlCommand cmd1 = new SqlCommand("UpdateSamsLocation", con);
                    cmd1.Parameters.AddWithValue("location_id", samsLocation.LocationId);
                    cmd1.Parameters.AddWithValue("latitude", samsLocation.Latitude);
                    cmd1.Parameters.AddWithValue("longitude", samsLocation.Longitude);

                    cmd1.CommandType = CommandType.StoredProcedure;
                    con.Open();

                    cmd1.ExecuteNonQuery();
                    con.Close();
                }
            }

            return RedirectToAction("LocationToMap", "SamsLocation");
        }

    }
}