using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using sams.Controllers;
using sams.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace sams.Common
{
    public class Helper
    {
        public static int PageSize
        {
            get { return 12; }
        }
        public static Bitmap GetCaptcha()
        {
            Bitmap objBitmap = new Bitmap(130, 80);
            Graphics objGraphics = Graphics.FromImage(objBitmap);
            objGraphics.Clear(Color.White);
            Random objRandom = new Random();
            objGraphics.DrawLine(Pens.Black, objRandom.Next(0, 50), objRandom.Next(10, 30), objRandom.Next(0, 200), objRandom.Next(0, 50));
            objGraphics.DrawRectangle(Pens.Blue, objRandom.Next(0, 20), objRandom.Next(0, 20), objRandom.Next(50, 80), objRandom.Next(0, 20));
            objGraphics.DrawLine(Pens.Blue, objRandom.Next(0, 20), objRandom.Next(10, 50), objRandom.Next(100, 200), objRandom.Next(0, 80));
            Brush objBrush =
                default(Brush);
            //create background style  
            HatchStyle[] aHatchStyles = new HatchStyle[]
            {
                HatchStyle.BackwardDiagonal, HatchStyle.Cross, HatchStyle.DashedDownwardDiagonal, HatchStyle.DashedHorizontal, HatchStyle.DashedUpwardDiagonal, HatchStyle.DashedVertical,
                    HatchStyle.DiagonalBrick, HatchStyle.DiagonalCross, HatchStyle.Divot, HatchStyle.DottedDiamond, HatchStyle.DottedGrid, HatchStyle.ForwardDiagonal, HatchStyle.Horizontal,
                    HatchStyle.HorizontalBrick, HatchStyle.LargeCheckerBoard, HatchStyle.LargeConfetti, HatchStyle.LargeGrid, HatchStyle.LightDownwardDiagonal, HatchStyle.LightHorizontal
            };
            //create rectangular area  
            RectangleF oRectangleF = new RectangleF(0, 0, 300, 300);
            objBrush = new HatchBrush(aHatchStyles[objRandom.Next(aHatchStyles.Length - 3)], Color.FromArgb((objRandom.Next(100, 255)), (objRandom.Next(100, 255)), (objRandom.Next(100, 255))), Color.White);
            objGraphics.FillRectangle(objBrush, oRectangleF);
            //Generate the image for captcha  
            string captchaText = string.Format("{0:X}", objRandom.Next(1000000, 9999999));
            //add the captcha value in session  
            //Session["CaptchaVerify"] = captchaText.ToLower();
            Font objFont = new Font("Courier New", 15, FontStyle.Bold);
            //Draw the image for captcha  
            objGraphics.DrawString(captchaText, objFont, Brushes.Black, 20, 20);
            //objBitmap.Save(Response.OutputStream, ImageFormat.Gif);

            /*
            using (StreamReader reader = new StreamReader(objGraphics))
            {
                string myString = reader.ReadToEnd();
            }
            */



            return objBitmap;
        }


        public static string GetUniqueFileName(string fileName)
        {
            fileName = Path.GetFileName(fileName);
            fileName = fileName.Replace(" ", String.Empty);
            return Path.GetFileNameWithoutExtension(fileName)
                      + "_"
                      + Guid.NewGuid().ToString().Substring(0, 4)
                      + Path.GetExtension(fileName);
        }

        public static string GetUniqueId()
        {
            return Guid.NewGuid().ToString();
        }

        static public string SamsLatitude {
            get
            {
                return "35.1043081";
            }
        }

        static public string SamsLongitude {
            get
            {
                return "-80.7123775";
            }
        }

        static public string PrimaryColor
        {
            get
            {
                return "bg-purple";
            }
        }

        public static string SamsConnectionString { get; set; }
        public static string SamsConnectionStringLocal { get; set; }
        public static string SamsConnectionStringSandBox { get; set; }
        public static string SamsConnectionStringQa { get; set; }
        public static string HostName { get; set; }

        public static string AvailableBackColor = "";
        public static string UnderContractBackColor = "background-color: #9166d2;";
        public static string SoldBackColor = "background-color: #ef0000d6;";

        /*
        public static string FormatCurrency(string currencySymbol, string currencyString)
        {
            int currency = 0;
            if (currencyString == null)
            {
                currencyString = "0";
            }
            var allDigits = Regex.Match(currencyString, @"\d+").Value;
            if (allDigits.Length > 0)
            {
                currency = int.Parse(allDigits);
            }


            CultureInfo FrCulture = new CultureInfo("us-US");
            Thread.CurrentThread.CurrentCulture = FrCulture;
            NumberFormatInfo LocalFormat = (NumberFormatInfo)NumberFormatInfo.CurrentInfo.Clone();
            LocalFormat.CurrencySymbol = currencySymbol + " ";
            
            return currency.ToString("c", LocalFormat);

            //return currency.ToString("c", NumberFormatInfo.CurrentInfo);


            
        }
        */
        
        public static int ShowShoppingCenter()
        {
            
            var sSettings = new SamsSettings();
            string CS = DBConnection.ConnectionString;

            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("GetSamsSettings", con);
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

            return sSettings.ShowShoppingCenterMenu;
        }

        public static void SendCriticalAlertEmail()
        {
            List<EmailNotificationViewModel> emailNotificationList = new List<EmailNotificationViewModel>();
            SamsSettings sSettings = SamsSettingsController.GetSamsSettings();
            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("GetActiveNotifications", con);
                cmd.CommandType = CommandType.StoredProcedure;

                con.Open();
                SqlDataReader readerAssetType = cmd.ExecuteReader();
                while (readerAssetType.Read())
                {
                    try
                    {
                        EmailNotificationViewModel emailNotification = new EmailNotificationViewModel();
                        emailNotification.PeriodId = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("period_id")) ? 0 : readerAssetType.GetInt32(readerAssetType.GetOrdinal("period_id"));
                        emailNotification.PropertyHeader1 = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("p_header")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("p_header"));
                        emailNotification.PropertyHeader2 = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("header_2")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("header_2"));
                        emailNotification.AssetId = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("asset_id")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("asset_id"));
                        emailNotification.PropertyAddress = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("address")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("address"));
                        emailNotification.CriticalItemHeader = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("period_master")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("period_master"));

                        emailNotification.StartDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("start_date")) ? DateTime.Now : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("start_date"));
                        emailNotification.AlertDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("alert_date")) ? default(DateTime?) : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("alert_date"));
                        emailNotification.EmailAddress = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("other_email_address")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("other_email_address"));
                        emailNotification.EndDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("end_date")) ? DateTime.Now : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("end_date"));
                        emailNotification.PeriodNotes = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("period_notes")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("period_notes"));

                        emailNotificationList.Add(emailNotification);

                        StringBuilder sbEmailMessage = new StringBuilder();
                        //sbEmailMessage.Append("<div><b>Greetings " + customer.FirstName + " " + customer.LastName + ",</b><div>");

                        sbEmailMessage.Append("<div>");
                        sbEmailMessage.Append("<h3>Critical Notification Alert For Asset " + emailNotification.AssetId + "</h3> <br />");

                        //sbEmailMessage.Append("<b>Details</b> <br/><br/>");
                        sbEmailMessage.Append("<b>Property Address : </b>" + emailNotification.PropertyAddress + "<br/>");
                        sbEmailMessage.Append("<b>Critical Item : </b>" + emailNotification.CriticalItemHeader + "<br/>");
                        sbEmailMessage.Append("<b>Start Date : </b>" + emailNotification.StartDate.ToString("MM-dd-yyyy") + "<br/>");
                        sbEmailMessage.Append("<b>Duration : </b>" + emailNotification.Duration.ToString() + " <br/>");
                        sbEmailMessage.Append("<b>Due Date : </b><font color='red'>" + emailNotification.EndDate.ToString("MM-dd-yyyy") + "</font><br/>");
                        sbEmailMessage.Append("<b>Notes : </b>" + emailNotification.PeriodNotes + "<br/><br/>");

                        sbEmailMessage.Append("</div>");

                        sbEmailMessage.Append("<div>");
                        sbEmailMessage.Append("Best Regards<br />");
                        sbEmailMessage.Append("Sam's Holdings, LLC");
                        sbEmailMessage.Append("</div>");

                        string fromEmail = sSettings.SmtpEmailAddress;
                        emailNotification.EmailAddress = emailNotification.EmailAddress.Replace(";", ",");
                        MailMessage mailMessage = new MailMessage(fromEmail, emailNotification.EmailAddress, "Critical Notification Alert For Asset " + emailNotification.AssetId, sbEmailMessage.ToString());
                        mailMessage.IsBodyHtml = true;

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
                    catch
                    {

                    }
                    



                }

                con.Close();

                foreach(EmailNotificationViewModel en in emailNotificationList)
                {
                    SqlCommand cmd1 = new SqlCommand("UpdateEmailSendStatusOnCriticalItem", con);
                    cmd1.CommandType = CommandType.StoredProcedure;
                    cmd1.Parameters.AddWithValue("period_id", en.PeriodId);

                    con.Open();
                    cmd1.ExecuteNonQuery();
                    con.Close();
                }
            }
        }
    }

    public enum SamsPropertyType
    {
        General = 0,
        Surplus = 1,
        NetLease = 2,
        C_Store = 3,
        ShoppingCenter = 4,
        NewPropertyDashboard = 5
    }

    enum NewPropertyCreaedBy
    {
        ByCustomer = 0,
        BySams = 1
    }

    public enum PeriodType
    {
        Acquisition,
        Disposition,
        Lease,
        LeaseWithPurchase,
        Netlease,
        PurchaseLeaseBack
    }

    public enum TransactionStatus
    {
        Under_LOI = 1,
        Under_Contract = 2,
        Closed_Acquisitions = 3,
        Terminated_Acquisitions = 4,
        LOI_Received = 5
    }

    public enum SamAssetType
    {
        Lease = 1,
        Fee = 2,
        FeeSubjectToLease = 3,
        NetLease = 4,
        LeaseWithPurchaseOption = 5,
        SaleLeaseBack = 6,
        PurchaseLeaseBack = 7
    }

    public enum TransactionType
    {
        Sale = 1,
        Lease = 2,
        Netlease = 4,
        LeaseWithPurchaseOption = 5,
        SaleLeaseBack = 6,
        PurchaseLeaseBack = 7
    }

    public enum SamsTransactionStatus
    {
        Under_LOI = 1,
        Under_Contract = 2,
        Closed_Dispositions = 3,
        Terminated_Dispositions = 4,
        LOI_Received = 5
    }

    public static class HtmlHelpers
    {
        public static string EncodedMultiLineText(this HtmlHelper helper, string text)
        {
            if (String.IsNullOrEmpty(text))
            {
                return String.Empty;
            }
            return Regex.Replace(helper.Encode(text), Environment.NewLine, "<br/>");
        }
    }
    
}
