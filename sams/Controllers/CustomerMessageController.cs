using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sams.Models;

namespace sams.Controllers
{
    public class CustomerMessageController : Controller
    {
        
        public IActionResult Index()
        {
            
            List<CustomerMessageViewModel> customerMessageList = new List<CustomerMessageViewModel>();

            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("GetCustomerMessageList", con);
                
                cmd.CommandType = CommandType.StoredProcedure;

                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    string custMessage = "";
                    var cMessage = new CustomerMessageViewModel();

                    cMessage.CustomerMessageId = reader.IsDBNull(reader.GetOrdinal("contact_us_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("contact_us_id"));
                    cMessage.CustomerName = reader.IsDBNull(reader.GetOrdinal("custumer_name")) ? "" : reader.GetString(reader.GetOrdinal("custumer_name"));
                    cMessage.CustomerEmail = reader.IsDBNull(reader.GetOrdinal("customer_email")) ? "" : reader.GetString(reader.GetOrdinal("customer_email"));
                    cMessage.EmailSubject = reader.IsDBNull(reader.GetOrdinal("customer_subject")) ? "" : reader.GetString(reader.GetOrdinal("customer_subject"));
                    custMessage = reader.IsDBNull(reader.GetOrdinal("customer_message")) ? "" : reader.GetString(reader.GetOrdinal("customer_message"));
                    cMessage.CreatedDate = reader.IsDBNull(reader.GetOrdinal("created_date")) ? DateTime.Now : reader.GetDateTime(reader.GetOrdinal("created_date"));

                    if(custMessage.Length > 12)
                    {
                        custMessage = custMessage.Substring(0, 11) + "...";
                    }
                    cMessage.CustomerMessage = custMessage;
                    customerMessageList.Add(cMessage);
                }
                con.Close();
            }
            

            return View(customerMessageList);

        }

        [HttpPost]
        public IActionResult SaveCustomerMessage(CustomerMessageViewModel cMessage)
        {
            if (Request.Cookies != null)
            {
                var cookiesContactUs = Request.Cookies["ContactUs"];
                if (cookiesContactUs == "true")
                {
                    if (cMessage.CaptchaEntered == "1")
                    {
                        string CS = DBConnection.ConnectionString;
                        using (SqlConnection con = new SqlConnection(CS))
                        {
                            SqlCommand cmd = new SqlCommand("SaveCustomerMessage", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            con.Open();

                            cmd.Parameters.AddWithValue("custumer_name", cMessage.CustomerName);

                            cmd.Parameters.AddWithValue("customer_email", cMessage.CustomerEmail);
                            cmd.Parameters.AddWithValue("customer_subject", cMessage.EmailSubject);
                            cmd.Parameters.AddWithValue("customer_message", cMessage.CustomerMessage);

                            cmd.ExecuteNonQuery();
                            con.Close();

                        }

                    }
                }
            }

            var option = new CookieOptions();
            option.Expires = DateTime.Now.AddDays(-1);
            Response.Cookies.Append("ContactUs", "false", option);

            return View();
        }

        public IActionResult DeleteCustomerMessage(string customerMessageId)
        {
            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("DeleteCustomerMessage", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                cmd.Parameters.AddWithValue("contact_us_id", customerMessageId);
                cmd.ExecuteNonQuery();
                con.Close();
            }

            return RedirectToAction("Index");
        }

        public IActionResult ShowCustomerMessage(int customerMessageId)
        {

            var cMessage = new CustomerMessageViewModel();

            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("GetCustomerMessageById", con);
                cmd.Parameters.AddWithValue("contact_us_id", customerMessageId);

                cmd.CommandType = CommandType.StoredProcedure;

                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    string custMessage = "";
                    
                    cMessage.CustomerMessageId = reader.IsDBNull(reader.GetOrdinal("contact_us_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("contact_us_id"));
                    cMessage.CustomerName = reader.IsDBNull(reader.GetOrdinal("custumer_name")) ? "" : reader.GetString(reader.GetOrdinal("custumer_name"));
                    cMessage.CustomerEmail = reader.IsDBNull(reader.GetOrdinal("customer_email")) ? "" : reader.GetString(reader.GetOrdinal("customer_email"));
                    cMessage.EmailSubject = reader.IsDBNull(reader.GetOrdinal("customer_subject")) ? "" : reader.GetString(reader.GetOrdinal("customer_subject"));
                    custMessage = reader.IsDBNull(reader.GetOrdinal("customer_message")) ? "" : reader.GetString(reader.GetOrdinal("customer_message"));
                    cMessage.CreatedDate = reader.IsDBNull(reader.GetOrdinal("created_date")) ? DateTime.Now : reader.GetDateTime(reader.GetOrdinal("created_date"));
                    /*
                    if (custMessage.Length > 12)
                    {
                        custMessage = custMessage.Substring(0, 11) + "...";
                    }
                    */
                    cMessage.CustomerMessage = custMessage;
                    
                }
                con.Close();
            }


            return View(cMessage);

        }

    }
}