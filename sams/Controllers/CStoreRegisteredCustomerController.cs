using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using sams.Common;
using sams.Models;
using Spire.Xls;

namespace sams.Controllers
{
    public class CStoreRegisteredCustomerController : Controller
    {
        private readonly IWebHostEnvironment webHostEnvironment;

        public CStoreRegisteredCustomerController(IWebHostEnvironment hostEnvironment)
        {
            webHostEnvironment = hostEnvironment;
        }

        public IActionResult Index()
        {
            var customerList = new List<CustomerViewModel>();
            string CS = DBConnection.ConnectionString;

            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("GetCstoreCustomerList", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    customerList.Add(GetCustomer(reader));
                }

                con.Close();
            }

            return View(customerList);
        }

        
        public ActionResult GetCustomer(int customerId)
        {
            string CS = DBConnection.ConnectionString;
            var user = new CustomerViewModel();
            var historyList = new List<PageHitViewModel>();

            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("GetCstoreCustomerById", con);
                cmd.Parameters.AddWithValue("customer_id", customerId);

                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    user = GetCustomer(reader);
                }

                con.Close();


                SqlCommand cmdHistory = new SqlCommand("GetCustomerHistoryById", con);
                cmdHistory.Parameters.AddWithValue("customer_id", customerId);

                cmdHistory.CommandType = CommandType.StoredProcedure;
                con.Open();

                SqlDataReader readerHistory = cmdHistory.ExecuteReader();
                while (readerHistory.Read())
                {
                    var historyItem = GetPageHitItem(readerHistory);
                    historyList.Add(historyItem);
                }
                user.PageHitList = historyList;
            }

            //return JsonConvert.SerializeObject(user);
            return View(user);

        }

        public IActionResult DeleteCustomer(int customerId)
        {
            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("DeleteCstoreCustomer", con);
                cmd.Parameters.AddWithValue("customer_id", customerId);

                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                cmd.ExecuteNonQuery();
                con.Close();
            }
            return RedirectToAction("Index");
        }

        public IActionResult ExportExcel()
        {
            
            string CS = DBConnection.ConnectionString;
            string colSlNo = "B", colFirstName = "C", colLastName = "D", colEmailAddress = "E", colContactNumber = "F", colUserName = "G", colOrganization = "H";
            string colGivenTitle = "I", colAddress = "J", colZipcode = "K", colCity = "L", colState = "M", colCell = "N", colSignId = "O";

            var filePath = Path.Combine(webHostEnvironment.WebRootPath + @"/templates", "c_store_customer_list_template.xlsx");

            string fullFileName = Path.Combine(webHostEnvironment.WebRootPath + @"/templates", "c_store_customer_list_template.xlsx");
            string fullToFileName = Path.Combine(webHostEnvironment.WebRootPath + @"/templates", "c_store_customer_list.xlsx");

            Workbook wrkBook = new Workbook();
            wrkBook.LoadFromFile(fullFileName);
            Worksheet sheet = wrkBook.Worksheets[0];

            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("GetCstoreCustomerList", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                int i = 5;
                int j = 1;
                while (reader.Read())
                {
                    var customer = GetCustomer(reader);

                    string cellSlNo = colSlNo + i.ToString();
                    string cellFirstName = colFirstName + i.ToString();
                    string cellLastName = colLastName + i.ToString();
                    string cellEmailAddress = colEmailAddress + i.ToString();
                    string cellContactNumber = colContactNumber + i.ToString();
                    string cellUserName = colUserName + i.ToString();
                    string cellOrganization = colOrganization + i.ToString();
                    string cellGivenTitle = colGivenTitle + i.ToString();
                    string cellAddress = colAddress + i.ToString();
                    string cellZipcode = colZipcode + i.ToString();
                    string cellCity = colCity + i.ToString();
                    string cellState = colState + i.ToString();
                    string cellCell = colCell + i.ToString();
                    string cellSignId = colSignId + i.ToString();

                    sheet.Range[cellSlNo].Value = j.ToString();
                    sheet.Range[cellFirstName].Value = customer.FirstName;
                    sheet.Range[cellLastName].Value = customer.LastName;
                    sheet.Range[cellEmailAddress].Value = customer.EmailAddress;
                    sheet.Range[cellContactNumber].Value = customer.ContactNumber;
                    sheet.Range[cellUserName].Value = customer.UserName;
                    sheet.Range[cellOrganization].Value = customer.Company;
                    sheet.Range[cellGivenTitle].Value = customer.GivenTitle;
                    sheet.Range[cellAddress].Value = customer.Address;
                    sheet.Range[cellZipcode].Value = customer.Zipcode;
                    sheet.Range[cellCity].Value = customer.City;
                    sheet.Range[cellState].Value = customer.StateName;
                    sheet.Range[cellCell].Value = customer.CellNumber;
                    sheet.Range[cellSignId].Value = customer.SignatureId;

                    i++;
                    j++;
                    sheet.Range["A5:O" + i.ToString()].BorderInside(LineStyleType.Thin, Color.Black);
                }

                con.Close();
            }

            wrkBook.SaveToFile(fullToFileName);

            byte[] fileBytes = GetFile(fullToFileName);
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fullToFileName);
        }

        CustomerViewModel GetCustomer(SqlDataReader reader)
        {
            var user = new CustomerViewModel();
            user.CustomerId = reader.IsDBNull(reader.GetOrdinal("customer_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("customer_id"));
            user.FirstName = reader.IsDBNull(reader.GetOrdinal("first_name")) ? "" : reader.GetString(reader.GetOrdinal("first_name"));
            user.LastName = reader.IsDBNull(reader.GetOrdinal("last_name")) ? "" : reader.GetString(reader.GetOrdinal("last_name"));
            user.EmailAddress = reader.IsDBNull(reader.GetOrdinal("email_address")) ? "" : reader.GetString(reader.GetOrdinal("email_address"));
            user.ContactNumber = reader.IsDBNull(reader.GetOrdinal("contact_number")) ? "" : reader.GetString(reader.GetOrdinal("contact_number"));
            user.SignedNDAFileName = reader.IsDBNull(reader.GetOrdinal("signed_nda_file")) ? "" : reader.GetString(reader.GetOrdinal("signed_nda_file"));
            user.UserName = reader.IsDBNull(reader.GetOrdinal("user_name")) ? "" : reader.GetString(reader.GetOrdinal("user_name"));
            user.Password = reader.IsDBNull(reader.GetOrdinal("customer_password")) ? "" : reader.GetString(reader.GetOrdinal("customer_password"));
            user.CreatedDate = reader.IsDBNull(reader.GetOrdinal("created_date")) ? DateTime.Now : reader.GetDateTime(reader.GetOrdinal("created_date"));
            user.LastLoginDate = reader.IsDBNull(reader.GetOrdinal("last_login_date")) ? DateTime.Now : reader.GetDateTime(reader.GetOrdinal("last_login_date"));
            user.CustomerSignature = reader.IsDBNull(reader.GetOrdinal("customer_sign")) ? "" : reader.GetString(reader.GetOrdinal("customer_sign"));
            user.Company = reader.IsDBNull(reader.GetOrdinal("company_name")) ? "" : reader.GetString(reader.GetOrdinal("company_name"));
            user.GivenTitle = reader.IsDBNull(reader.GetOrdinal("given_title")) ? "" : reader.GetString(reader.GetOrdinal("given_title"));
            user.Address = reader.IsDBNull(reader.GetOrdinal("address")) ? "" : reader.GetString(reader.GetOrdinal("address"));
            user.Zipcode = reader.IsDBNull(reader.GetOrdinal("zipcode")) ? "" : reader.GetString(reader.GetOrdinal("zipcode"));
            user.City = reader.IsDBNull(reader.GetOrdinal("city")) ? "" : reader.GetString(reader.GetOrdinal("city"));
            user.StateId = int.Parse(reader.IsDBNull(reader.GetOrdinal("state_id")) ? "0" : reader.GetString(reader.GetOrdinal("state_id")));
            user.StateName = reader.IsDBNull(reader.GetOrdinal("state_name")) ? "" : reader.GetString(reader.GetOrdinal("state_name"));
            user.CellNumber = reader.IsDBNull(reader.GetOrdinal("cell_number")) ? "" : reader.GetString(reader.GetOrdinal("cell_number"));
            user.CellNumber = reader.IsDBNull(reader.GetOrdinal("cell_number")) ? "" : reader.GetString(reader.GetOrdinal("cell_number"));
            user.SignedStatus = reader.IsDBNull(reader.GetOrdinal("signed_status")) ? "" : reader.GetString(reader.GetOrdinal("signed_status"));

            user.SignatureId = reader.IsDBNull(reader.GetOrdinal("sh_verification_id")) ? "" : reader.GetString(reader.GetOrdinal("sh_verification_id"));

            return user;
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

        public IActionResult EditCustomer(int customerId)
        {
            string CS = DBConnection.ConnectionString;
            var user = new CustomerViewModel();

            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("GetCstoreCustomerById", con);
                cmd.Parameters.AddWithValue("customer_id", customerId);

                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    user = GetCustomer(reader);
                }

                con.Close();
            }

            user.StateList = GetStateList();

            return View(user);
        }

        List<StateDetails> GetStateList()
        {
            List<StateDetails> stateList = new List<StateDetails>();
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
                    // stateDetails.StateCode = reader.IsDBNull(reader.GetOrdinal("state_code")) ? "" : reader.GetString(reader.GetOrdinal("state_code"));
                    stateDetails.StateName = reader.IsDBNull(reader.GetOrdinal("state_name")) ? "" : reader.GetString(reader.GetOrdinal("state_name"));
                    stateList.Add(stateDetails);
                }
                con.Close();


            }
            return stateList;
        }

        [HttpPost]
        public ActionResult RegisterCustomer(CustomerViewModel customer)
        {
            

            var imageFileName = "no_file";
            
            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {


                //GetUserForLogin
                SqlCommand cmdLogin = new SqlCommand("RegisterCustomer", con);

                cmdLogin.Parameters.AddWithValue("customer_id", customer.CustomerId);
                cmdLogin.Parameters.AddWithValue("first_name", customer.FirstName);

                cmdLogin.Parameters.AddWithValue("last_name", customer.LastName);
                cmdLogin.Parameters.AddWithValue("email_address", customer.EmailAddress);

                cmdLogin.Parameters.AddWithValue("contact_number", customer.ContactNumber);
                cmdLogin.Parameters.AddWithValue("signed_nda_file", imageFileName);
                cmdLogin.Parameters.AddWithValue("user_name", customer.EmailAddress);
                cmdLogin.Parameters.AddWithValue("customer_password", customer.Password);

                cmdLogin.Parameters.AddWithValue("company_name", customer.Company);
                cmdLogin.Parameters.AddWithValue("given_title", customer.GivenTitle);

                cmdLogin.Parameters.AddWithValue("address", customer.Address);
                cmdLogin.Parameters.AddWithValue("zipcode", customer.Zipcode);
                cmdLogin.Parameters.AddWithValue("city", customer.City);
                cmdLogin.Parameters.AddWithValue("state_id", customer.StateId);
                cmdLogin.Parameters.AddWithValue("cell_number", customer.CellNumber);

                cmdLogin.Parameters.AddWithValue("signed_status", "Not Signed NDA");

                cmdLogin.CommandType = CommandType.StoredProcedure;
                con.Open();


                customer.CustomerId = int.Parse(cmdLogin.ExecuteScalar().ToString());



                customer.UploadedNDAFile = null;

                HttpContext.Session.SetObjectAsJson("LoggedInUser", customer);
                //LoginPropertyId 
                con.Close();
            }


            /*
            SamsSettings sSettings = SamsSettingsController.GetSamsSettings();

            StringBuilder sbEmailMessage = new StringBuilder();
            
            sbEmailMessage.Append("<div>");
            sbEmailMessage.Append("Please click the link below to review and sign the confidentiality agreement to get full access to additional information. <br /><br />");
            sbEmailMessage.Append("<a href='" + Helper.HostName + "/RealEstate/GetCustomerAgreement?CustomerId=" + customer.CustomerId + "'>Confidentiality Agreement/Non-disclosure Agreement</a>");
            sbEmailMessage.Append("</div>");

            sbEmailMessage.Append("<div>");
            sbEmailMessage.Append("Best Regards<br />");
            sbEmailMessage.Append("Sam's Holdings, LLC");
            sbEmailMessage.Append("</div>");

            string fromEmail = sSettings.SmtpEmailAddress;
            MailMessage mailMessage = new MailMessage(fromEmail, customer.EmailAddress, "Non-Disclosure Agreement from Sam’s Holdings LLC.", sbEmailMessage.ToString());
            mailMessage.IsBodyHtml = true;

            SmtpClient smtpClient = new SmtpClient(sSettings.SmtpServer, int.Parse(sSettings.SmtpPortNumber));
            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            
            if (sSettings.SmtpPassword.Trim().Length == 0)
            {
                sSettings.SmtpPassword = "FMf5IY78JnSlolc2";
            }
            smtpClient.Credentials = new NetworkCredential(fromEmail, sSettings.SmtpPassword);

            smtpClient.Send(mailMessage);

            customer.EmailBody = sbEmailMessage.ToString();
            
            */
            return RedirectToAction("Index");
        }


        [HttpPost]
        public IActionResult SendResetCustomerPasswordLink(string emailAddress)
        {
            CustomerViewModel customer = new CustomerViewModel();
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
                sbEmailMessage.Append(customer.FirstName + ", <br /><br />");

                sbEmailMessage.Append("Thank you for signing/sending the confidentiality agreement to review information on our C-Stores.  We have assigned the following as your user name.");

                sbEmailMessage.Append("User Name : <b>" + customer.UserName + "</b> <br /><br />");

                sbEmailMessage.Append("Please click the link below to set up a password and use it to enter the virtual data room to view/download confidential information. <br /><br />");
                // sbEmailMessage.Append("Please find the link to put your signature. <br /><br />");
                sbEmailMessage.Append("<a href='" + Helper.HostName + "/RealEstate/ResetPasswordLink?s=" + customer.ResetPasswordId + "'>Click here to setup password </a>");
                sbEmailMessage.Append("</div>");

                sbEmailMessage.Append("<div>");
                sbEmailMessage.Append("Best Regards <br />");
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

                if (sSettings.SmtpPassword.Trim().Length == 0)
                {
                    sSettings.SmtpPassword = "FMf5IY78JnSlolc2";
                }
                //smtpClient.Credentials = new NetworkCredential(fromEmail, "FMf5IY78JnSlolc2");
                
                smtpClient.Credentials = new NetworkCredential(fromEmail, sSettings.SmtpPassword);

                smtpClient.Send(mailMessage);

            }

            return View();
        }


        [HttpPost]
        public IActionResult SendCustomerAgreement(string customerId, string emailAddress)
        {
            SamsSettings sSettings = SamsSettingsController.GetSamsSettings();

            StringBuilder sbEmailMessage = new StringBuilder();
            //sbEmailMessage.Append("<div><b>Greetings " + customer.FirstName + " " + customer.LastName + ",</b><div>");

            sbEmailMessage.Append("<div>");
            sbEmailMessage.Append("Please click the link below to review and sign the confidentiality agreement to get full access to additional information. <br /><br />");
            // sbEmailMessage.Append("Please find the link to put your signature. <br /><br />");
            //sbEmailMessage.Append("<a href='https://samsholdingsdevelopment.azurewebsites.net/RealEstate/GetCustomerAgreement?CustomerId=" + customer.CustomerId + "'>Confidentiality Agreement/Non-disclosure Agreement</a>");
            sbEmailMessage.Append("<a href='" + Helper.HostName + "/RealEstate/GetCustomerAgreement?CustomerId=" + customerId + "'>Confidentiality Agreement/Non-disclosure Agreement</a>");
            sbEmailMessage.Append("</div>");

            sbEmailMessage.Append("<div>");
            sbEmailMessage.Append("Best Regards<br />");
            sbEmailMessage.Append("Sam's Holdings, LLC");
            sbEmailMessage.Append("</div>");


            //string fromEmail = "infosh@samsholdings.com";
            string fromEmail = sSettings.SmtpEmailAddress;
            MailMessage mailMessage = new MailMessage(fromEmail, emailAddress, "Non-Disclosure Agreement from Sam’s Holdings LLC.", sbEmailMessage.ToString());
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

        PageHitViewModel GetPageHitItem(SqlDataReader reader)
        {
            var pageHit = new PageHitViewModel();
            
            pageHit.PropertyId = reader.IsDBNull(reader.GetOrdinal("property_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("property_id"));
            //pageHit.CustomerId = reader.IsDBNull(reader.GetOrdinal("customer_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("customer_id"));

            pageHit.AssetId = reader.IsDBNull(reader.GetOrdinal("asset_id")) ? "" : reader.GetString(reader.GetOrdinal("asset_id"));
            pageHit.PropertyHeader = reader.IsDBNull(reader.GetOrdinal("property_header")) ? "" : reader.GetString(reader.GetOrdinal("property_header"));
            pageHit.FirstName = reader.IsDBNull(reader.GetOrdinal("first_name")) ? "" : reader.GetString(reader.GetOrdinal("first_name"));
            pageHit.LastName = reader.IsDBNull(reader.GetOrdinal("last_name")) ? "" : reader.GetString(reader.GetOrdinal("last_name"));
            pageHit.HitHeader = reader.IsDBNull(reader.GetOrdinal("hit_header")) ? "" : reader.GetString(reader.GetOrdinal("hit_header"));
            pageHit.AssetType = reader.IsDBNull(reader.GetOrdinal("property_type_name")) ? "" : reader.GetString(reader.GetOrdinal("property_type_name"));
            pageHit.CreatedDate = reader.IsDBNull(reader.GetOrdinal("created_date")) ? DateTime.Now : reader.GetDateTime(reader.GetOrdinal("created_date"));
            return pageHit;
        }
    }
}