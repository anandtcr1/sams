using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Bcpg.OpenPgp;
using sams.Models;
using Spire.Xls;

namespace sams.Controllers
{
    public class MapCompetitorController : Controller
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        public MapCompetitorController(IWebHostEnvironment hostEnvironment)
        {

            webHostEnvironment = hostEnvironment;
        }

        public IActionResult Index()
        {
            List<MapHeaderViewModel> mapHeaderList = new List<MapHeaderViewModel>();

            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("GetMapHeaderList", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var mapHeader = new MapHeaderViewModel();
                    mapHeader.MapHeaderId = reader.IsDBNull(reader.GetOrdinal("map_header_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("map_header_id"));
                    mapHeader.MapHeaderName = reader.IsDBNull(reader.GetOrdinal("header_name")) ? "" : reader.GetString(reader.GetOrdinal("header_name"));
                    mapHeader.CreatedDate = reader.IsDBNull(reader.GetOrdinal("created_date")) ? DateTime.Now : reader.GetDateTime(reader.GetOrdinal("created_date"));

                    mapHeaderList.Add(mapHeader);
                }

            }

            return View(mapHeaderList);


            
        }

        public IActionResult ViewSavedMap(string headerId)
        {
            CompetitorViewModel objCompetitor = new CompetitorViewModel();

            List<SelectedMapCordinates> mapCordinatorList = new List<SelectedMapCordinates>();
            string CS = DBConnection.ConnectionString;

            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("GetMapCordinates", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("header_id", headerId);

                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var selectedMapCordinates = new SelectedMapCordinates();
                    selectedMapCordinates.HeaderId = reader.IsDBNull(reader.GetOrdinal("header_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("header_id"));
                    selectedMapCordinates.Latitude = reader.IsDBNull(reader.GetOrdinal("latitude")) ? "" : reader.GetString(reader.GetOrdinal("latitude"));
                    selectedMapCordinates.Longitude = reader.IsDBNull(reader.GetOrdinal("longitude")) ? "" : reader.GetString(reader.GetOrdinal("longitude"));

                    selectedMapCordinates.MarkerColor = reader.IsDBNull(reader.GetOrdinal("marker_color")) ? "" : reader.GetString(reader.GetOrdinal("marker_color"));
                    selectedMapCordinates.MarkerHeader = reader.IsDBNull(reader.GetOrdinal("marker_header")) ? "" : reader.GetString(reader.GetOrdinal("marker_header"));
                    selectedMapCordinates.MarkerAddress = reader.IsDBNull(reader.GetOrdinal("marker_address")) ? "" : reader.GetString(reader.GetOrdinal("marker_address"));

                    selectedMapCordinates.MarkerType = reader.IsDBNull(reader.GetOrdinal("marker_type")) ? "" : reader.GetString(reader.GetOrdinal("marker_type"));

                    mapCordinatorList.Add(selectedMapCordinates);
                }

            }

            objCompetitor.SelectedCordinates = mapCordinatorList;
            return View(objCompetitor);
        }

        public IActionResult ShowMapForClient(string headerId)
        {
            CompetitorViewModel objCompetitor = new CompetitorViewModel();

            List<SelectedMapCordinates> mapCordinatorList = new List<SelectedMapCordinates>();
            string CS = DBConnection.ConnectionString;

            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("GetMapCordinates", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("header_id", headerId);

                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var selectedMapCordinates = new SelectedMapCordinates();
                    selectedMapCordinates.HeaderId = reader.IsDBNull(reader.GetOrdinal("header_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("header_id"));
                    selectedMapCordinates.Latitude = reader.IsDBNull(reader.GetOrdinal("latitude")) ? "" : reader.GetString(reader.GetOrdinal("latitude"));
                    selectedMapCordinates.Longitude = reader.IsDBNull(reader.GetOrdinal("longitude")) ? "" : reader.GetString(reader.GetOrdinal("longitude"));

                    selectedMapCordinates.MarkerColor = reader.IsDBNull(reader.GetOrdinal("marker_color")) ? "" : reader.GetString(reader.GetOrdinal("marker_color"));
                    selectedMapCordinates.MarkerHeader = reader.IsDBNull(reader.GetOrdinal("marker_header")) ? "" : reader.GetString(reader.GetOrdinal("marker_header"));
                    selectedMapCordinates.MarkerAddress = reader.IsDBNull(reader.GetOrdinal("marker_address")) ? "" : reader.GetString(reader.GetOrdinal("marker_address"));

                    selectedMapCordinates.MarkerType = reader.IsDBNull(reader.GetOrdinal("marker_type")) ? "" : reader.GetString(reader.GetOrdinal("marker_type"));

                    
                    selectedMapCordinates.AddedAddress = reader.IsDBNull(reader.GetOrdinal("added_address")) ? "" : reader.GetString(reader.GetOrdinal("added_address"));
                    selectedMapCordinates.LandSize = reader.IsDBNull(reader.GetOrdinal("land_size")) ? "" : reader.GetString(reader.GetOrdinal("land_size"));
                    selectedMapCordinates.AskingPrice = reader.IsDBNull(reader.GetOrdinal("asking_price")) ? "" : reader.GetString(reader.GetOrdinal("asking_price"));
                    selectedMapCordinates.Zoning = reader.IsDBNull(reader.GetOrdinal("zoning")) ? "" : reader.GetString(reader.GetOrdinal("zoning"));


                    mapCordinatorList.Add(selectedMapCordinates);
                }

            }

            objCompetitor.SelectedCordinates = mapCordinatorList;
            return View(objCompetitor);
        }

        [HttpPost]
        public string SaveMapHeader(string jsonString)
        {
            var headerId = "";
            string CS = DBConnection.ConnectionString;

            JObject mapHeader = JObject.Parse(jsonString);
            string hId = (string)mapHeader["headerId"];
            string headerName = (string)mapHeader["headerName"];

            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("SaveMapHeader", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("map_header_id", hId);
                cmd.Parameters.AddWithValue("header_name", headerName);

                con.Open();
                headerId = cmd.ExecuteScalar().ToString();
                con.Close();
            }
                

            return headerId;
        }
        
        [HttpPost]
        public string SaveMapCordinates(string jsonString)
        {
            
            string CS = DBConnection.ConnectionString;

            JArray cordinateList = JArray.Parse(jsonString);

            foreach(var mCordinate in cordinateList)
            {
                string hId = (string)mCordinate["MapHeaderId"];
                string lat = (string)mCordinate["Latitude"];
                string lng = (string)mCordinate["Longitude"];
                
                string markerColor = (string)mCordinate["MarkerColor"];
                string markerHeader = (string)mCordinate["MarkerHeader"];
                string markedAddress = (string)mCordinate["MarkedAddress"];

                string markerType = (string)mCordinate["MarkerType"];

                using (SqlConnection con = new SqlConnection(CS))
                {
                    SqlCommand cmd = new SqlCommand("SaveMapCordinates", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    
                    cmd.Parameters.AddWithValue("header_id", hId);
                    cmd.Parameters.AddWithValue("latitude", lat);
                    cmd.Parameters.AddWithValue("longitude", lng);

                    cmd.Parameters.AddWithValue("marker_color", markerColor);
                    cmd.Parameters.AddWithValue("marker_header", markerHeader);
                    cmd.Parameters.AddWithValue("marker_address", markedAddress);

                    cmd.Parameters.AddWithValue("marker_type", markerType);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }

            }

            return "done";
            
        }

        [HttpPost]
        public string SaveSingleMapLocation(string jsonString)
        {
            var headerId = "";
            string CS = DBConnection.ConnectionString;

            JObject mapHeader = JObject.Parse(jsonString);
            string hId = (string)mapHeader["headerId"];
            string lat = (string)mapHeader["latitude"];
            string lng = (string)mapHeader["longitude"];
            string markerColor = (string)mapHeader["marker_color"];
            string markerHeader = (string)mapHeader["marker_header"];
            string markedAddress = (string)mapHeader["marker_address"];
            string markerType = (string)mapHeader["marker_type"];

            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("SaveMapCordinates", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("header_id", hId);
                cmd.Parameters.AddWithValue("latitude", lat);
                cmd.Parameters.AddWithValue("longitude", lng);

                cmd.Parameters.AddWithValue("marker_color", markerColor);
                cmd.Parameters.AddWithValue("marker_header", markerHeader);
                cmd.Parameters.AddWithValue("marker_address", markedAddress);

                cmd.Parameters.AddWithValue("marker_type", markerType);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }


            return headerId;
        }



        public IActionResult DeleteMapHeader(string headerId)
        {
            string CS = DBConnection.ConnectionString;

            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("DeleteMapMarker", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("header_id", headerId);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }

            return RedirectToAction("Index");
        }


        public IActionResult EditSavedMap(string headerId)
        {
            CompetitorViewModel objCompetitor = new CompetitorViewModel();

            List<SelectedMapCordinates> mapCordinatorList = new List<SelectedMapCordinates>();
            string CS = DBConnection.ConnectionString;

            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("GetMapCordinates", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("header_id", headerId);

                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var selectedMapCordinates = new SelectedMapCordinates();
                    selectedMapCordinates.CoordinateId = reader.IsDBNull(reader.GetOrdinal("cordinated_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("cordinated_id"));
                    selectedMapCordinates.HeaderId = reader.IsDBNull(reader.GetOrdinal("header_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("header_id"));
                    selectedMapCordinates.Latitude = reader.IsDBNull(reader.GetOrdinal("latitude")) ? "" : reader.GetString(reader.GetOrdinal("latitude"));
                    selectedMapCordinates.Longitude = reader.IsDBNull(reader.GetOrdinal("longitude")) ? "" : reader.GetString(reader.GetOrdinal("longitude"));

                    selectedMapCordinates.MarkerColor = reader.IsDBNull(reader.GetOrdinal("marker_color")) ? "" : reader.GetString(reader.GetOrdinal("marker_color"));
                    selectedMapCordinates.MarkerHeader = reader.IsDBNull(reader.GetOrdinal("marker_header")) ? "" : reader.GetString(reader.GetOrdinal("marker_header"));
                    selectedMapCordinates.MarkerAddress = reader.IsDBNull(reader.GetOrdinal("marker_address")) ? "" : reader.GetString(reader.GetOrdinal("marker_address"));

                    selectedMapCordinates.MarkerType = reader.IsDBNull(reader.GetOrdinal("marker_type")) ? "" : reader.GetString(reader.GetOrdinal("marker_type"));

                    selectedMapCordinates.AddedAddress = reader.IsDBNull(reader.GetOrdinal("added_address")) ? "" : reader.GetString(reader.GetOrdinal("added_address"));
                    selectedMapCordinates.LandSize = reader.IsDBNull(reader.GetOrdinal("land_size")) ? "" : reader.GetString(reader.GetOrdinal("land_size"));
                    selectedMapCordinates.AskingPrice = reader.IsDBNull(reader.GetOrdinal("asking_price")) ? "" : reader.GetString(reader.GetOrdinal("asking_price"));
                    selectedMapCordinates.Zoning = reader.IsDBNull(reader.GetOrdinal("zoning")) ? "" : reader.GetString(reader.GetOrdinal("zoning"));
                    
                    mapCordinatorList.Add(selectedMapCordinates);
                }

            }

            objCompetitor.SelectedCordinates = mapCordinatorList;
            return View(objCompetitor);
        }

        public IActionResult AddLocationMap(string headerId)
        {
            CompetitorViewModel objCompetitor = new CompetitorViewModel();

            List<SelectedMapCordinates> mapCordinatorList = new List<SelectedMapCordinates>();
            string CS = DBConnection.ConnectionString;

            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("GetMapCordinates", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("header_id", headerId);

                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var selectedMapCordinates = new SelectedMapCordinates();
                    selectedMapCordinates.CoordinateId = reader.IsDBNull(reader.GetOrdinal("cordinated_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("cordinated_id"));
                    selectedMapCordinates.HeaderId = reader.IsDBNull(reader.GetOrdinal("header_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("header_id"));
                    selectedMapCordinates.Latitude = reader.IsDBNull(reader.GetOrdinal("latitude")) ? "" : reader.GetString(reader.GetOrdinal("latitude"));
                    selectedMapCordinates.Longitude = reader.IsDBNull(reader.GetOrdinal("longitude")) ? "" : reader.GetString(reader.GetOrdinal("longitude"));

                    selectedMapCordinates.MarkerColor = reader.IsDBNull(reader.GetOrdinal("marker_color")) ? "" : reader.GetString(reader.GetOrdinal("marker_color"));
                    selectedMapCordinates.MarkerHeader = reader.IsDBNull(reader.GetOrdinal("marker_header")) ? "" : reader.GetString(reader.GetOrdinal("marker_header"));
                    selectedMapCordinates.MarkerAddress = reader.IsDBNull(reader.GetOrdinal("marker_address")) ? "" : reader.GetString(reader.GetOrdinal("marker_address"));

                    selectedMapCordinates.MarkerType = reader.IsDBNull(reader.GetOrdinal("marker_type")) ? "" : reader.GetString(reader.GetOrdinal("marker_type"));

                    selectedMapCordinates.AddedAddress = reader.IsDBNull(reader.GetOrdinal("added_address")) ? "" : reader.GetString(reader.GetOrdinal("added_address"));
                    selectedMapCordinates.LandSize = reader.IsDBNull(reader.GetOrdinal("land_size")) ? "" : reader.GetString(reader.GetOrdinal("land_size"));
                    selectedMapCordinates.AskingPrice = reader.IsDBNull(reader.GetOrdinal("asking_price")) ? "" : reader.GetString(reader.GetOrdinal("asking_price"));
                    selectedMapCordinates.Zoning = reader.IsDBNull(reader.GetOrdinal("zoning")) ? "" : reader.GetString(reader.GetOrdinal("zoning"));

                    mapCordinatorList.Add(selectedMapCordinates);
                }

            }

            objCompetitor.SelectedCordinates = mapCordinatorList;
            return View(objCompetitor);
        }
        


        [HttpPost]
        public string UpdateMapCordinateAddress(string jsonString)
        {

            string CS = DBConnection.ConnectionString;

            JObject cordinateList = JObject.Parse(jsonString);

            string addedAddress = (string)cordinateList["adderess"];
            string landSize = (string)cordinateList["landSize"];
            string askingPrice = (string)cordinateList["askingPrice"];
            string zoning = (string)cordinateList["zoning"];
            string markerId = (string)cordinateList["markerId"];

            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("UpdateMapCordinates", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("added_address", addedAddress);
                cmd.Parameters.AddWithValue("land_size", landSize);
                cmd.Parameters.AddWithValue("asking_price", askingPrice);

                cmd.Parameters.AddWithValue("zoning", zoning);
                cmd.Parameters.AddWithValue("cordinated_id", markerId);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }


            return "done";

        }

        public IActionResult ExportExcel(string headerId)
        {
            //string fileName = Path.GetFileNameWithoutExtension(@"\\OpsVsAdp\\Files\\Daily\\TempHours.xlsx");
            var filePath = Path.Combine(webHostEnvironment.WebRootPath + @"/templates", "map_Address_List_Template.xlsx");

            string fullFileName = Path.Combine(webHostEnvironment.WebRootPath + @"/templates", "map_Address_List_Template.xlsx");
            string fullToFileName = Path.Combine(webHostEnvironment.WebRootPath + @"/templates", "map_Address_List.xlsx");

            Workbook wrkBook = new Workbook();
            wrkBook.LoadFromFile(fullFileName);
            Worksheet sheet = wrkBook.Worksheets[0];

            List<SelectedMapCordinates> cordinatorList = new List<SelectedMapCordinates>();
            string CS = DBConnection.ConnectionString;

            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("GetMapCordinates", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("header_id", headerId);

                con.Open();

                int i = 5;
                string colLatitudeId = "A", colLongitude = "B", colHeader = "C", colAddress = "D", colLandSize = "E", colAskingPrice = "F", colZoning = "G";

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var selectedMapCordinates = new SelectedMapCordinates();
                    selectedMapCordinates.HeaderId = reader.IsDBNull(reader.GetOrdinal("header_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("header_id"));
                    selectedMapCordinates.Latitude = reader.IsDBNull(reader.GetOrdinal("latitude")) ? "" : reader.GetString(reader.GetOrdinal("latitude"));
                    selectedMapCordinates.Longitude = reader.IsDBNull(reader.GetOrdinal("longitude")) ? "" : reader.GetString(reader.GetOrdinal("longitude"));

                    selectedMapCordinates.MarkerColor = reader.IsDBNull(reader.GetOrdinal("marker_color")) ? "" : reader.GetString(reader.GetOrdinal("marker_color"));
                    selectedMapCordinates.MarkerHeader = reader.IsDBNull(reader.GetOrdinal("marker_header")) ? "" : reader.GetString(reader.GetOrdinal("marker_header"));
                    selectedMapCordinates.MarkerAddress = reader.IsDBNull(reader.GetOrdinal("marker_address")) ? "" : reader.GetString(reader.GetOrdinal("marker_address"));

                    selectedMapCordinates.MarkerType = reader.IsDBNull(reader.GetOrdinal("marker_type")) ? "" : reader.GetString(reader.GetOrdinal("marker_type"));
                    selectedMapCordinates.AddedAddress = reader.IsDBNull(reader.GetOrdinal("added_address")) ? "" : reader.GetString(reader.GetOrdinal("added_address"));

                    selectedMapCordinates.LandSize = reader.IsDBNull(reader.GetOrdinal("land_size")) ? "" : reader.GetString(reader.GetOrdinal("land_size"));
                    selectedMapCordinates.AskingPrice = reader.IsDBNull(reader.GetOrdinal("asking_price")) ? "" : reader.GetString(reader.GetOrdinal("asking_price"));
                    selectedMapCordinates.Zoning = reader.IsDBNull(reader.GetOrdinal("zoning")) ? "" : reader.GetString(reader.GetOrdinal("zoning"));


                    cordinatorList.Add(selectedMapCordinates);

                    string cellLatitudeId = colLatitudeId + i.ToString();
                    string cellLongitude = colLongitude + i.ToString();
                    string cellHeader = colHeader + i.ToString();
                    string cellAddress = colAddress + i.ToString();
                    string cellLandSize = colLandSize + i.ToString();
                    string cellAskingPrice = colAskingPrice + i.ToString();
                    string cellZoning = colZoning + i.ToString();

                    sheet.Range[cellLatitudeId].Value = selectedMapCordinates.Latitude;
                    sheet.Range[cellLongitude].Value = selectedMapCordinates.Longitude;
                    sheet.Range[cellHeader].Value = selectedMapCordinates.MarkerHeader;
                    sheet.Range[cellAddress].Value = selectedMapCordinates.AddedAddress;
                    sheet.Range[cellLandSize].Value = selectedMapCordinates.LandSize;
                    sheet.Range[cellAskingPrice].Value = selectedMapCordinates.AskingPrice;
                    sheet.Range[cellZoning].Value = selectedMapCordinates.Zoning;

                    i = i + 1;
                }

                wrkBook.SaveToFile(fullToFileName);

            }
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
    }
}