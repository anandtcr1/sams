using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using sams.Models;

namespace sams.Controllers
{
    public class SignUpCustomerController : Controller
    {
        public IActionResult Index()
        {

            List<SignupCustomerViewModel> signupCustomerList = new List<SignupCustomerViewModel>();

            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("GetSignedUpCustomerList", con);

                cmd.CommandType = CommandType.StoredProcedure;

                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var sCustomer = new SignupCustomerViewModel();

                    sCustomer.SignupCustomerId = reader.IsDBNull(reader.GetOrdinal("custimer_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("custimer_id"));
                    sCustomer.FirstName = reader.IsDBNull(reader.GetOrdinal("first_name")) ? "" : reader.GetString(reader.GetOrdinal("first_name"));
                    sCustomer.LastName = reader.IsDBNull(reader.GetOrdinal("last_name")) ? "" : reader.GetString(reader.GetOrdinal("last_name"));
                    sCustomer.EmailAddress = reader.IsDBNull(reader.GetOrdinal("email_address")) ? "" : reader.GetString(reader.GetOrdinal("email_address"));
                    sCustomer.ContactNumber = reader.IsDBNull(reader.GetOrdinal("contact_number")) ? "" : reader.GetString(reader.GetOrdinal("contact_number"));
                    sCustomer.CreatedDate = reader.IsDBNull(reader.GetOrdinal("created_date")) ? DateTime.Now : reader.GetDateTime(reader.GetOrdinal("created_date"));
                    sCustomer.Subscribe = reader.IsDBNull(reader.GetOrdinal("subscribe_status")) ? true : reader.GetBoolean(reader.GetOrdinal("subscribe_status"));

                    signupCustomerList.Add(sCustomer);
                }
                con.Close();
            }


            return View(signupCustomerList);

        }


        public IActionResult ViewCustomer(int customerId)
        {

            var sCustomer = new SignupCustomerViewModel();

            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("GetSignedUpCustomerById", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("custimer_id", customerId);

                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    sCustomer.SignupCustomerId = reader.IsDBNull(reader.GetOrdinal("custimer_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("custimer_id"));
                    sCustomer.FirstName = reader.IsDBNull(reader.GetOrdinal("first_name")) ? "" : reader.GetString(reader.GetOrdinal("first_name"));
                    sCustomer.LastName = reader.IsDBNull(reader.GetOrdinal("last_name")) ? "" : reader.GetString(reader.GetOrdinal("last_name"));
                    sCustomer.EmailAddress = reader.IsDBNull(reader.GetOrdinal("email_address")) ? "" : reader.GetString(reader.GetOrdinal("email_address"));
                    sCustomer.ContactNumber = reader.IsDBNull(reader.GetOrdinal("contact_number")) ? "" : reader.GetString(reader.GetOrdinal("contact_number"));
                    sCustomer.CreatedDate = reader.IsDBNull(reader.GetOrdinal("created_date")) ? DateTime.Now : reader.GetDateTime(reader.GetOrdinal("created_date"));
                    sCustomer.Subscribe = reader.IsDBNull(reader.GetOrdinal("subscribe_status")) ? true : reader.GetBoolean(reader.GetOrdinal("subscribe_status"));
                }
                con.Close();
            }


            return View(sCustomer);

        }

        [HttpPost]
        public IActionResult SaveSignedUpCustomer(SignupCustomerViewModel customer)
        {
            string CS = DBConnection.ConnectionString;

            var lastFourDigit = customer.ContactNumber.Substring(customer.ContactNumber.Length - 4);
            if (lastFourDigit == customer.LastFourDigitNumber)
            {
                using (SqlConnection con = new SqlConnection(CS))
                {
                    SqlCommand cmd = new SqlCommand("SaveSignedupCustomer", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();

                    cmd.Parameters.AddWithValue("first_name", customer.FirstName);
                    cmd.Parameters.AddWithValue("last_name", customer.LastName);
                    cmd.Parameters.AddWithValue("email_address", customer.EmailAddress);
                    cmd.Parameters.AddWithValue("contact_number", customer.ContactNumber);

                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }

            return View();
        }

        [HttpPost]
        public IActionResult SaveSignedUpCustomerFromAdmin(SignupCustomerViewModel customer)
        {
            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("UpdateSignedupCustomer", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                cmd.Parameters.AddWithValue("custimer_id", customer.SignupCustomerId);
                cmd.Parameters.AddWithValue("first_name", customer.FirstName);
                cmd.Parameters.AddWithValue("last_name", customer.LastName);
                cmd.Parameters.AddWithValue("email_address", customer.EmailAddress);
                cmd.Parameters.AddWithValue("contact_number", customer.ContactNumber);
                cmd.Parameters.AddWithValue("subscribe_status", customer.Subscribe);

                cmd.ExecuteNonQuery();
                con.Close();
            }

            return RedirectToAction("Index");
        }

        public IActionResult DeleteSignedUpCustomer(string signedUpCustomerId)
        {
            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("DeleteSignedUpCustomer", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                cmd.Parameters.AddWithValue("custimer_id", signedUpCustomerId);

                cmd.ExecuteNonQuery();
                con.Close();
            }

            return RedirectToAction("Index");
        }


    }
}