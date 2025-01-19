using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using sams.Common;
using sams.Models;

namespace sams.Controllers
{
    public class PageHitStatusController : Controller
    {
        private readonly IWebHostEnvironment webHostEnvironment;

        public PageHitStatusController(IWebHostEnvironment hostEnvironment)
        {

            webHostEnvironment = hostEnvironment;
        }

        public IActionResult Index()
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

            List<PageHitViewModel> pageHitList = new List<PageHitViewModel>();

            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("GetPageHitStatus", con);
                cmd.Parameters.AddWithValue("allData", 1);

                cmd.CommandType = CommandType.StoredProcedure;

                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    PageHitViewModel pageHit = new PageHitViewModel();
                    pageHit.PropertyId= reader.IsDBNull(reader.GetOrdinal("property_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("property_id"));
                    pageHit.TotalPageHit = reader.IsDBNull(reader.GetOrdinal("totalHit")) ? 0 : reader.GetInt32(reader.GetOrdinal("totalHit"));

                    pageHit.AssetId = reader.IsDBNull(reader.GetOrdinal("asset_id")) ? "" : reader.GetString(reader.GetOrdinal("asset_id"));
                    pageHit.PropertyHeader = reader.IsDBNull(reader.GetOrdinal("property_header")) ? "" : reader.GetString(reader.GetOrdinal("property_header"));
                    pageHit.AssetType = reader.IsDBNull(reader.GetOrdinal("property_type_name")) ? "" : reader.GetString(reader.GetOrdinal("property_type_name"));

                    pageHitList.Add(pageHit);
                }
            }

            return View(pageHitList);
        }

        [HttpPost]
        public IActionResult GetHitsOnDates(DateTime fromDate, DateTime toDate)
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

            List<PageHitViewModel> pageHitList = new List<PageHitViewModel>();

            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("GetPageHitStatus", con);
                cmd.Parameters.AddWithValue("allData", 0);
                cmd.Parameters.AddWithValue("from_date", fromDate);
                cmd.Parameters.AddWithValue("to_date", toDate);

                cmd.CommandType = CommandType.StoredProcedure;

                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    PageHitViewModel pageHit = new PageHitViewModel();
                    pageHit.PropertyId = reader.IsDBNull(reader.GetOrdinal("property_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("property_id"));
                    pageHit.TotalPageHit = reader.IsDBNull(reader.GetOrdinal("totalHit")) ? 0 : reader.GetInt32(reader.GetOrdinal("totalHit"));

                    pageHit.AssetId = reader.IsDBNull(reader.GetOrdinal("asset_id")) ? "" : reader.GetString(reader.GetOrdinal("asset_id"));
                    pageHit.PropertyHeader = reader.IsDBNull(reader.GetOrdinal("property_header")) ? "" : reader.GetString(reader.GetOrdinal("property_header"));
                    pageHit.AssetType = reader.IsDBNull(reader.GetOrdinal("property_type_name")) ? "" : reader.GetString(reader.GetOrdinal("property_type_name"));

                    pageHitList.Add(pageHit);
                }
            }
            ViewData["FromDate"] = fromDate;
            ViewData["ToDate"] = toDate;
            return View(pageHitList);
        }
        
    }
}