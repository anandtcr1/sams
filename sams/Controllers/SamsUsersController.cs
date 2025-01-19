using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using DocuSign.eSign.Model;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using sams.Common;
using sams.Models;

namespace sams.Controllers
{
    public class SamsUsersController : Controller
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        public SamsUsersController(IWebHostEnvironment hostEnvironment)
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

            

            var userList = new List<UserViewModel>();

            string CS = DBConnection.ConnectionString;


            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("GetUserList", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var user = new UserViewModel();

                    user.UserId = reader.IsDBNull(reader.GetOrdinal("user_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("user_id"));
                    user.FirstName = reader.IsDBNull(reader.GetOrdinal("first_name")) ? "" : reader.GetString(reader.GetOrdinal("first_name"));

                    user.LastName = reader.IsDBNull(reader.GetOrdinal("last_name")) ? "" : reader.GetString(reader.GetOrdinal("last_name"));
                    user.UserName = reader.IsDBNull(reader.GetOrdinal("user_name")) ? "" : reader.GetString(reader.GetOrdinal("user_name"));
                    user.RoleId = reader.IsDBNull(reader.GetOrdinal("role_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("role_id"));
                    user.RoleName = reader.IsDBNull(reader.GetOrdinal("role_name")) ? "" : reader.GetString(reader.GetOrdinal("role_name"));
                    user.EmailAddress = reader.IsDBNull(reader.GetOrdinal("email_address")) ? "" : reader.GetString(reader.GetOrdinal("email_address"));


                    userList.Add(user);
                }
            }

            return View(userList);
        }

        public IActionResult AddUser(int userId)
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

            
            var user = new UserViewModel();

            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("GetUserById", con);
                cmd.Parameters.AddWithValue("user_id", userId);

                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    

                    user.UserId = reader.IsDBNull(reader.GetOrdinal("user_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("user_id"));
                    user.FirstName = reader.IsDBNull(reader.GetOrdinal("first_name")) ? "" : reader.GetString(reader.GetOrdinal("first_name"));

                    user.LastName = reader.IsDBNull(reader.GetOrdinal("last_name")) ? "" : reader.GetString(reader.GetOrdinal("last_name"));
                    user.UserName = reader.IsDBNull(reader.GetOrdinal("user_name")) ? "" : reader.GetString(reader.GetOrdinal("user_name"));
                    user.Password = reader.IsDBNull(reader.GetOrdinal("password")) ? "" : reader.GetString(reader.GetOrdinal("password"));
                    user.RoleId = reader.IsDBNull(reader.GetOrdinal("role_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("role_id"));
                    user.RoleName = reader.IsDBNull(reader.GetOrdinal("role_name")) ? "" : reader.GetString(reader.GetOrdinal("role_name"));
                    user.EmailAddress = reader.IsDBNull(reader.GetOrdinal("email_address")) ? "" : reader.GetString(reader.GetOrdinal("email_address"));

                }
            }

            using (SqlConnection con = new SqlConnection(CS))
            {
                if(user.RoleId > 0)
                {
                    /*
                    SqlCommand cmd = new SqlCommand("GetRoleById", con);
                    cmd.Parameters.AddWithValue("role_id", user.RoleId);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    var roleDetails = new RoleViewModel();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {

                        roleDetails.RoleId = reader.IsDBNull(reader.GetOrdinal("role_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("role_id"));
                        roleDetails.RoleName = reader.IsDBNull(reader.GetOrdinal("role_name")) ? "" : reader.GetString(reader.GetOrdinal("role_name"));

                    }
                    con.Close();
                    user.RolePermission.SamsRole = roleDetails;


                    List<RolePermissionViewModel> rolePermissionList = new List<RolePermissionViewModel>();
                    SqlCommand cmdRolePermission = new SqlCommand("GetRolePermission", con);
                    cmdRolePermission.Parameters.AddWithValue("role_id", user.RoleId);
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

                    user.RolePermission.RolePermissionList = rolePermissionList;

                    con.Close();
                    */
                }
                




                List<RoleViewModel> roleList = new List<RoleViewModel>();
                SqlCommand cmdRoleList = new SqlCommand("GetRoles", con);
                cmdRoleList.CommandType = CommandType.StoredProcedure;
                con.Open();
                SqlDataReader readerRoleList = cmdRoleList.ExecuteReader();
                while (readerRoleList.Read())
                {
                    RoleViewModel roleView = new RoleViewModel();
                    roleView.RoleId = readerRoleList.IsDBNull(readerRoleList.GetOrdinal("role_id")) ? 0 : readerRoleList.GetInt32(readerRoleList.GetOrdinal("role_id"));
                    roleView.RoleName = readerRoleList.IsDBNull(readerRoleList.GetOrdinal("role_name")) ? "" : readerRoleList.GetString(readerRoleList.GetOrdinal("role_name"));
                    roleList.Add(roleView);
                }
                con.Close();
                user.RoleList = roleList;

                
            }


            

            if (user.UserName == SiteSettings.SuperAdminUser)
            {
                return RedirectToAction("Index");
            }


            return View(user);
        }

        [HttpPost]
        public IActionResult SaveUser(UserViewModel userView)
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

            if (userView.UserName == SiteSettings.SuperAdminUser)
            {
                return RedirectToAction("Index");
            }

            string CS = DBConnection.ConnectionString;
            var nRole = new RoleViewModel();
            string resetId = "";
            bool canSend = false;
            var usreName = "";
            using (SqlConnection con = new SqlConnection(CS))
            {
                usreName = userView.EmailAddress.Split("@")[0];
                SqlCommand cmd = new SqlCommand("SaveUser", con);
                cmd.Parameters.AddWithValue("user_id", userView.UserId);
                cmd.Parameters.AddWithValue("first_name", userView.FirstName);

                cmd.Parameters.AddWithValue("last_name", userView.LastName);
                cmd.Parameters.AddWithValue("user_name", usreName);
                cmd.Parameters.AddWithValue("email_address", userView.EmailAddress);

                cmd.Parameters.AddWithValue("role_id", userView.RoleId);
                
                if(userView.UserId == 0)
                {
                    resetId = Helper.GetUniqueId();
                    canSend = true;
                }

                cmd.Parameters.AddWithValue("password_reset_key", resetId);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                userView.UserId = int.Parse(cmd.ExecuteScalar().ToString());
                con.Close();
            }

            if (canSend)
            {
                SamsSettings sSettings = SamsSettingsController.GetSamsSettings();

                StringBuilder sbEmailMessage = new StringBuilder();
                //sbEmailMessage.Append("<div><b>Greetings " + customer.FirstName + " " + customer.LastName + ",</b><div>");

                sbEmailMessage.Append("<div>");
                sbEmailMessage.Append("Please find below user details. <br /><br />");
                sbEmailMessage.Append("User Name : <b>" + usreName + "</b> <br /><br />");
                sbEmailMessage.Append("Please click the link below to setup your password. <br /><br />");
                
                string hostName = Helper.HostName;
                //sbEmailMessage.Append("<a href='https://samsholdingsdevelopment.azurewebsites.net/SamsUsers/ResetPassword?s="+  resetId + "'>Reset Password</a>");
                sbEmailMessage.Append("<a href='" + hostName + "/SamsUsers/ResetPassword?s=" + resetId + "'>Reset Password</a>");
                sbEmailMessage.Append("</div>");

                sbEmailMessage.Append("<div>");
                sbEmailMessage.Append("Best Regards<br />");
                sbEmailMessage.Append("Sam's Holdings, LLC");
                sbEmailMessage.Append("</div>");

                string toEmailAddress = userView.EmailAddress + "";

                //string fromEmail = "infosh@samsholdings.com";
                string fromEmail = sSettings.SmtpEmailAddress;
                MailMessage mailMessage = new MailMessage(fromEmail, toEmailAddress, "Welcome To Sam's Holdings", sbEmailMessage.ToString());
                mailMessage.IsBodyHtml = true;

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
            }
            

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult ResetPassword(string pwd, string s)
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
                pwd = StringFunctions.Encrypt(pwd, SiteSettings.PasswordKey);
                SqlCommand cmd = new SqlCommand("ResetEmployeePassword", con);
                cmd.Parameters.AddWithValue("customer_password", pwd);
                cmd.Parameters.AddWithValue("password_reset_key", s);

                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                cmd.ExecuteNonQuery();

                con.Close();
            }
            return RedirectToAction("DoLogin", "Admin");
        }

        public IActionResult ResetPassword(string s)
        {
            ViewBag.ResetPassword = s;
            return View();
        }
        public IActionResult DeleteUser(int userId)
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
            var nRole = new RoleViewModel();
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("DeleteUser", con);
                cmd.Parameters.AddWithValue("user_id", userId);

                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                cmd.ExecuteNonQuery();
                con.Close();
            }
            //return RedirectToAction("AddRole", new { roleId = roleModel.RoleId });
            return RedirectToAction("Index");
        }

        public bool CheckDuplicateUserName(string UserName)
        {
            bool userExists = false;
            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                //GetUserForLogin
                SqlCommand cmdLogin = new SqlCommand("CheckAdminUser", con);

                cmdLogin.Parameters.AddWithValue("user_name", UserName);

                cmdLogin.CommandType = CommandType.StoredProcedure;
                con.Open();

                SqlDataReader readerMarket = cmdLogin.ExecuteReader();

                while (readerMarket.Read())
                {

                    int cId = readerMarket.IsDBNull(readerMarket.GetOrdinal("user_id")) ? 0 : readerMarket.GetInt32(readerMarket.GetOrdinal("user_id"));
                    if (cId > 0)
                    {
                        userExists = true;
                    }
                    else
                    {
                        userExists = false;
                    }
                }

                con.Close();
            }

            return userExists;
        }


        public IActionResult SendPasswordLink()
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
            var nRole = new RoleViewModel();
            string resetId = Helper.GetUniqueId();
            bool canSend = false;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("ResetAdminPassword", con);
                cmd.Parameters.AddWithValue("user_id", loggedInUser.UserId);
                cmd.Parameters.AddWithValue("password_reset_key", resetId);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                cmd.ExecuteScalar();
                con.Close();
            }

            SamsSettings sSettings = SamsSettingsController.GetSamsSettings();

            StringBuilder sbEmailMessage = new StringBuilder();
            //sbEmailMessage.Append("<div><b>Greetings " + customer.FirstName + " " + customer.LastName + ",</b><div>");

            sbEmailMessage.Append("<div>");
            sbEmailMessage.Append("Please click the link below to setup your password. <br /><br />");
            // sbEmailMessage.Append("Please find the link to put your signature. <br /><br />");
            string hostName = Helper.HostName;
            //sbEmailMessage.Append("<a href='https://samsholdingsdevelopment.azurewebsites.net/SamsUsers/ResetPassword?s="+  resetId + "'>Reset Password</a>");
            sbEmailMessage.Append("<a href='" + hostName + "/SamsUsers/ResetPassword?s=" + resetId + "'>Reset Password</a>");
            sbEmailMessage.Append("</div>");

            sbEmailMessage.Append("<div>");
            sbEmailMessage.Append("Best Regards<br />");
            sbEmailMessage.Append("Sam's Holdings, LLC");
            sbEmailMessage.Append("</div>");

            string toEmailAddress = loggedInUser.EmailAddress + "";

            //string fromEmail = "infosh@samsholdings.com";
            string fromEmail = sSettings.SmtpEmailAddress;
            MailMessage mailMessage = new MailMessage(fromEmail, toEmailAddress, "Reset Password", sbEmailMessage.ToString());
            mailMessage.IsBodyHtml = true;

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

             
            return RedirectToAction("DoLogin", "Admin");
        }

        public IActionResult SendPasswordLinkById(string userId)
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
            var nRole = new RoleViewModel();
            string resetId = Helper.GetUniqueId();
            bool canSend = false;
            
            var user = new UserViewModel();

            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("ResetAdminPasswordById", con);
                cmd.Parameters.AddWithValue("user_id", userId);
                cmd.Parameters.AddWithValue("password_reset_key", resetId);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                
                while (reader.Read())
                {


                    user.UserId = reader.IsDBNull(reader.GetOrdinal("user_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("user_id"));
                    user.FirstName = reader.IsDBNull(reader.GetOrdinal("first_name")) ? "" : reader.GetString(reader.GetOrdinal("first_name"));

                    user.LastName = reader.IsDBNull(reader.GetOrdinal("last_name")) ? "" : reader.GetString(reader.GetOrdinal("last_name"));
                    user.UserName = reader.IsDBNull(reader.GetOrdinal("user_name")) ? "" : reader.GetString(reader.GetOrdinal("user_name"));
                    user.Password = reader.IsDBNull(reader.GetOrdinal("password")) ? "" : reader.GetString(reader.GetOrdinal("password"));
                    user.RoleId = reader.IsDBNull(reader.GetOrdinal("role_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("role_id"));
                    // user.RoleName = reader.IsDBNull(reader.GetOrdinal("role_name")) ? "" : reader.GetString(reader.GetOrdinal("role_name"));
                    user.EmailAddress = reader.IsDBNull(reader.GetOrdinal("email_address")) ? "" : reader.GetString(reader.GetOrdinal("email_address"));

                }
            }

            if (user.UserId > 0)
            {
                SamsSettings sSettings = SamsSettingsController.GetSamsSettings();

                StringBuilder sbEmailMessage = new StringBuilder();
                //sbEmailMessage.Append("<div><b>Greetings " + customer.FirstName + " " + customer.LastName + ",</b><div>");

                sbEmailMessage.Append("<div>");
                sbEmailMessage.Append("Please click the link below to setup your password. <br /><br />");
                // sbEmailMessage.Append("Please find the link to put your signature. <br /><br />");
                string hostName = Helper.HostName;
                //sbEmailMessage.Append("<a href='https://samsholdingsdevelopment.azurewebsites.net/SamsUsers/ResetPassword?s="+  resetId + "'>Reset Password</a>");
                sbEmailMessage.Append("<a href='" + hostName + "/SamsUsers/ResetPassword?s=" + resetId + "'>Reset Password</a>");
                sbEmailMessage.Append("</div>");

                sbEmailMessage.Append("<div>");
                sbEmailMessage.Append("Best Regards<br />");
                sbEmailMessage.Append("Sam's Holdings, LLC");
                sbEmailMessage.Append("</div>");

                string toEmailAddress = user.EmailAddress;

                //string fromEmail = "infosh@samsholdings.com";
                string fromEmail = sSettings.SmtpEmailAddress;
                MailMessage mailMessage = new MailMessage(fromEmail, toEmailAddress, "Reset Password", sbEmailMessage.ToString());
                mailMessage.IsBodyHtml = true;

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
            }
            


            return View();
        }

        public static List<UserViewModel> GetUserList()
        {
            var userList = new List<UserViewModel>();

            string CS = DBConnection.ConnectionString;


            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("GetUserList", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var user = new UserViewModel();

                    user.UserId = reader.IsDBNull(reader.GetOrdinal("user_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("user_id"));
                    user.FirstName = reader.IsDBNull(reader.GetOrdinal("first_name")) ? "" : reader.GetString(reader.GetOrdinal("first_name"));

                    user.LastName = reader.IsDBNull(reader.GetOrdinal("last_name")) ? "" : reader.GetString(reader.GetOrdinal("last_name"));
                    user.UserName = reader.IsDBNull(reader.GetOrdinal("user_name")) ? "" : reader.GetString(reader.GetOrdinal("user_name"));
                    user.RoleId = reader.IsDBNull(reader.GetOrdinal("role_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("role_id"));
                    user.RoleName = reader.IsDBNull(reader.GetOrdinal("role_name")) ? "" : reader.GetString(reader.GetOrdinal("role_name"));
                    user.EmailAddress = reader.IsDBNull(reader.GetOrdinal("email_address")) ? "" : reader.GetString(reader.GetOrdinal("email_address"));


                    userList.Add(user);
                }
            }

            return userList;
        }
    }
}