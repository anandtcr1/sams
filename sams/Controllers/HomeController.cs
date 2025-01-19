using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using sams.Common;
using sams.Models;
using System.Web;
using DocuSign.Integrations.Client;
using System.IO;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using System.Runtime.InteropServices;
using System.Net.Mail;
using System.Net;
using DocuSign.eSign.Model;
using System.Configuration;
using System.Collections.Specialized;
using Microsoft.AspNetCore.Http;

namespace sams.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        

        public IActionResult Index()
        {
            Helper.HostName = Request.HttpContext.Request.Host.Value;

            /*
            var fromAddress = new MailAddress("anand.tcr@gmail.com", "Anand");
            var toAddress = new MailAddress("anand@knowminal.com", "KAnand");
            const string fromPassword = "sreehari123";
            const string subject = "Subject";
            const string body = "Body";

            var smtp1 = new SmtpClient();
            smtp1.Host = "smtp.gmail.com";
            smtp1.Port = 587;
            smtp1.EnableSsl = false;
            smtp1.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp1.UseDefaultCredentials = false;
            smtp1.Credentials = new NetworkCredential("anand.tcr@gmail.com", "sreehari123");

            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            })
            {
                smtp1.Send(message);
            }
            */



            /*
            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = false,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            })
            {
                smtp.Send(message);
            }
            */

            string CS1 = ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString;


            List<StateDetails> stateList = new List<StateDetails>();
            string CS = DBConnection.ConnectionString;
            // CS = ConfigurationManager.ConnectionStrings["testConnection"].ConnectionString;
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

            var samsLocationsList = new List<SamsLocationsViewModel>();
            
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

                con.Close();
            }


            var homeView = new HomeViewModel();
            homeView.StateList = stateList;
            homeView.SamsLocationList = samsLocationsList;

            CustomerViewModel loggedInUser = HttpContext.Session.GetObjectFromJson<CustomerViewModel>("LoggedInUser");
            if (loggedInUser != null)
            {
                ViewData["LoggedInUserId"] = loggedInUser.CustomerId;
                ViewData["LoggedInUserName"] = loggedInUser.FirstName + " " + loggedInUser.LastName;
            }


            return View(homeView);
        }

        public ActionResult About()
        {
            CustomerViewModel loggedInUser = HttpContext.Session.GetObjectFromJson<CustomerViewModel>("LoggedInUser");
            if (loggedInUser != null)
            {
                ViewData["LoggedInUserId"] = loggedInUser.CustomerId;
                ViewData["LoggedInUserName"] = loggedInUser.FirstName + " " + loggedInUser.LastName;
            }

            return View();
        }

        public ActionResult ContactUs()
        {
            CustomerViewModel loggedInUser = HttpContext.Session.GetObjectFromJson<CustomerViewModel>("LoggedInUser");
            if (loggedInUser != null)
            {
                ViewData["LoggedInUserId"] = loggedInUser.CustomerId;
                ViewData["LoggedInUserName"] = loggedInUser.FirstName + " " + loggedInUser.LastName;
            }
            var option = new CookieOptions();
            option.Expires = DateTime.Now.AddDays(1);
            Response.Cookies.Append("ContactUs", "true", option);

            return View();
        }


        public IActionResult Privacy()
        {
            CustomerViewModel loggedInUser = HttpContext.Session.GetObjectFromJson<CustomerViewModel>("LoggedInUser");
            if (loggedInUser != null)
            {
                ViewData["LoggedInUserId"] = loggedInUser.CustomerId;
                ViewData["LoggedInUserName"] = loggedInUser.FirstName + " " + loggedInUser.LastName;
            }

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            CustomerViewModel loggedInUser = HttpContext.Session.GetObjectFromJson<CustomerViewModel>("LoggedInUser");
            if (loggedInUser != null)
            {
                ViewData["LoggedInUserId"] = loggedInUser.CustomerId;
                ViewData["LoggedInUserName"] = loggedInUser.FirstName + " " + loggedInUser.LastName;
            }

            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public ActionResult LogOutCustomer()
        {
            HttpContext.Session.SetObjectAsJson("LoggedInUser", new CustomerViewModel());
            ViewData["LoggedInUserName"] = null;
            return RedirectToAction("Index", "Home");
        }

        public IActionResult GetRegionByStateId(string startDate, int duration)
        {
            //return Json(startDate);
            
            DateTime newDate = Convert.ToDateTime(startDate);
            //newDate = Convert.ToDateTime(startDate);
            newDate = newDate.AddDays(duration);
            return Json(newDate.ToString("MM-dd-yyyy"));
            
        }
    }
}
