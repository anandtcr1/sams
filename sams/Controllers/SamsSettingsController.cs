using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using sams.Models;
using System.Runtime.Intrinsics.X86;
using System.Net.Mail;
using System.Net;

namespace sams.Controllers
{
    public class SamsSettingsController : Controller
    {
        public IActionResult Index()
        {
            var sSettings = GetSamsSettings();
            return View(sSettings);
        }

        public IActionResult SaveSettings(SamsSettings sSettings)
        {
            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("SaveSamsSettings", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("settings_id", sSettings.SettingsId);
                cmd.Parameters.AddWithValue("smtp_mail_server", sSettings.SmtpServer);
                cmd.Parameters.AddWithValue("smtp_port_number", sSettings.SmtpPortNumber);
                cmd.Parameters.AddWithValue("smtp_email_address", sSettings.SmtpEmailAddress);
                cmd.Parameters.AddWithValue("smtp_password", sSettings.SmtpPassword);
                cmd.Parameters.AddWithValue("email_header", sSettings.EmailHeader);
                cmd.Parameters.AddWithValue("email_body", sSettings.EmailBody);
                cmd.Parameters.AddWithValue("real_estate_director", sSettings.RealEstateDirectorName);
                cmd.Parameters.AddWithValue("directore_email_address", sSettings.DirectorEmailAddress);
                cmd.Parameters.AddWithValue("directore_phone_number", sSettings.DirectorPhoneNumber);
                cmd.Parameters.AddWithValue("show_shopping_center_menu", sSettings.ShowShoppingCenterMenu); 

                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                sSettings.SettingsId = int.Parse(cmd.ExecuteScalar().ToString());
                con.Close();
            }


            return RedirectToAction("Index");
        }

        public static SamsSettings GetSamsSettings()
        {
            var sSettings = new SamsSettings();
            string CS = DBConnection.ConnectionString;

            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("GetSamsSettings", con);
                cmd.CommandType = CommandType.StoredProcedure;

                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {

                    sSettings.SettingsId = reader.IsDBNull(reader.GetOrdinal("settings_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("settings_id"));
                    sSettings.SmtpServer = reader.IsDBNull(reader.GetOrdinal("smtp_mail_server")) ? "" : reader.GetString(reader.GetOrdinal("smtp_mail_server"));
                    sSettings.SmtpPortNumber = reader.IsDBNull(reader.GetOrdinal("smtp_port_number")) ? "" : reader.GetString(reader.GetOrdinal("smtp_port_number"));

                    sSettings.SmtpEmailAddress = reader.IsDBNull(reader.GetOrdinal("smtp_email_address")) ? "" : reader.GetString(reader.GetOrdinal("smtp_email_address"));
                    sSettings.SmtpPassword = reader.IsDBNull(reader.GetOrdinal("smtp_password")) ? "" : reader.GetString(reader.GetOrdinal("smtp_password"));

                    sSettings.EmailHeader = reader.IsDBNull(reader.GetOrdinal("email_header")) ? "" : reader.GetString(reader.GetOrdinal("email_header"));
                    sSettings.EmailBody = reader.IsDBNull(reader.GetOrdinal("email_body")) ? "" : reader.GetString(reader.GetOrdinal("email_body"));
                    sSettings.RealEstateDirectorName = reader.IsDBNull(reader.GetOrdinal("real_estate_director")) ? "" : reader.GetString(reader.GetOrdinal("real_estate_director"));

                    sSettings.DirectorEmailAddress = reader.IsDBNull(reader.GetOrdinal("directore_email_address")) ? "" : reader.GetString(reader.GetOrdinal("directore_email_address"));
                    sSettings.DirectorPhoneNumber = reader.IsDBNull(reader.GetOrdinal("directore_phone_number")) ? "" : reader.GetString(reader.GetOrdinal("directore_phone_number"));

                    sSettings.ShowShoppingCenterMenu = reader.IsDBNull(reader.GetOrdinal("show_shopping_center_menu")) ? 0 : reader.GetInt32(reader.GetOrdinal("show_shopping_center_menu"));

                }
                con.Close();
            }

            return sSettings;
        }

        public ActionResult SendTestMail()
        {
            return RedirectToAction("Index");
        }
    }
}