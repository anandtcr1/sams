using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Xml;
using DocuSign.Integrations.Client;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sams.Common;
using sams.Models;
using Spire.Xls;
using Spire.Xls.Core.Spreadsheet.AutoFilter;

namespace sams.Controllers
{
    public class SurplusPropertiesController : Controller
    {

        private readonly IWebHostEnvironment webHostEnvironment;
        //private readonly ApplicationDbContext dbContext;

        public SurplusPropertiesController(IWebHostEnvironment hostEnvironment)
        {
            
            webHostEnvironment = hostEnvironment;
        }

        public IActionResult Index()
        {
            UserViewModel loggedInUser = HttpContext.Session.GetObjectFromJson<UserViewModel>("LoggedInAdmin");
            if(loggedInUser == null)
            {
                return RedirectToAction("DoLogin", "Admin");
            }
            if (loggedInUser != null && loggedInUser.UserId <= 0)
            {
                return RedirectToAction("DoLogin", "Admin");
            }


            List<SiteDetails> surplusPropertiesList = new List<SiteDetails>();

            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("GetPropertyListByCategory", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("asset_status", 0);
                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var steDetails = new SiteDetails();
                    steDetails.SiteDetailsId = reader.IsDBNull(reader.GetOrdinal("site_details_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("site_details_id"));
                    steDetails.NamePrefix = reader.IsDBNull(reader.GetOrdinal("name_prefix")) ? "" : reader.GetString(reader.GetOrdinal("name_prefix"));
                    steDetails.FirstName = reader.IsDBNull(reader.GetOrdinal("first_name")) ? "" : reader.GetString(reader.GetOrdinal("first_name"));

                    steDetails.LastName = reader.IsDBNull(reader.GetOrdinal("last_name")) ? "" : reader.GetString(reader.GetOrdinal("last_name"));
                    steDetails.CompanyName = reader.IsDBNull(reader.GetOrdinal("company_name")) ? "" : reader.GetString(reader.GetOrdinal("company_name"));
                    steDetails.EmailAddress = reader.IsDBNull(reader.GetOrdinal("email_address")) ? "" : reader.GetString(reader.GetOrdinal("email_address"));
                    steDetails.Address = reader.IsDBNull(reader.GetOrdinal("address")) ? "" : reader.GetString(reader.GetOrdinal("address"));
                    steDetails.CityName = reader.IsDBNull(reader.GetOrdinal("city_name")) ? "" : reader.GetString(reader.GetOrdinal("city_name"));
                    steDetails.StateId = reader.IsDBNull(reader.GetOrdinal("state_id")) ? "" : reader.GetString(reader.GetOrdinal("state_id"));

                    steDetails.StateName = reader.IsDBNull(reader.GetOrdinal("state_name")) ? "" : reader.GetString(reader.GetOrdinal("state_name"));

                    steDetails.ZipCode = reader.IsDBNull(reader.GetOrdinal("zip_code")) ? "" : reader.GetString(reader.GetOrdinal("zip_code"));
                    steDetails.ContactNumber = reader.IsDBNull(reader.GetOrdinal("contact_number")) ? "" : reader.GetString(reader.GetOrdinal("contact_number"));
                    steDetails.SamsHoldingEmployee = reader.IsDBNull(reader.GetOrdinal("sams_holding_employee")) ? false : reader.GetBoolean(reader.GetOrdinal("sams_holding_employee"));
                    steDetails.MarketId = reader.IsDBNull(reader.GetOrdinal("market_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("market_id"));
                    steDetails.PropertyHeader = reader.IsDBNull(reader.GetOrdinal("property_header")) ? "" : reader.GetString(reader.GetOrdinal("property_header"));
                    steDetails.SiteAddress = reader.IsDBNull(reader.GetOrdinal("site_address")) ? "" : reader.GetString(reader.GetOrdinal("site_address"));
                    steDetails.SiteCity = reader.IsDBNull(reader.GetOrdinal("site_city")) ? "" : reader.GetString(reader.GetOrdinal("site_city"));
                    steDetails.SiteStateId = reader.IsDBNull(reader.GetOrdinal("site_state_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("site_state_id"));

                    steDetails.SiteStateName = reader.IsDBNull(reader.GetOrdinal("site_state_name")) ? "" : reader.GetString(reader.GetOrdinal("site_state_name"));

                    steDetails.SiteCounty = reader.IsDBNull(reader.GetOrdinal("site_county")) ? "" : reader.GetString(reader.GetOrdinal("site_county"));
                    steDetails.SiteCrossStreetName = reader.IsDBNull(reader.GetOrdinal("site_cross_street_name")) ? "" : reader.GetString(reader.GetOrdinal("site_cross_street_name"));
                    steDetails.IsPropertyAvailable = reader.IsDBNull(reader.GetOrdinal("is_property_available")) ? true : reader.GetBoolean(reader.GetOrdinal("is_property_available"));
                    steDetails.Zoning = reader.IsDBNull(reader.GetOrdinal("zoning")) ? "" : reader.GetString(reader.GetOrdinal("zoning"));
                    steDetails.LotSize = reader.IsDBNull(reader.GetOrdinal("lot_size")) ? "" : reader.GetString(reader.GetOrdinal("lot_size"));

                    steDetails.SalesPrice = reader.IsDBNull(reader.GetOrdinal("sales_price")) ? "" : reader.GetString(reader.GetOrdinal("sales_price"));
                    //steDetails.SalesPrice = Helper.FormatCurrency("$", steDetails.SalesPrice);

                    steDetails.Comments = reader.IsDBNull(reader.GetOrdinal("comments")) ? "" : reader.GetString(reader.GetOrdinal("comments"));

                    steDetails.CreatedDate = reader.IsDBNull(reader.GetOrdinal("created_date")) ? DateTime.Now : reader.GetDateTime(reader.GetOrdinal("created_date"));
                    steDetails.PropertyType = reader.IsDBNull(reader.GetOrdinal("property_type")) ? 0 : reader.GetInt32(reader.GetOrdinal("property_type"));

                    steDetails.ImageName = reader.IsDBNull(reader.GetOrdinal("image_name")) ? "" : reader.GetString(reader.GetOrdinal("image_name"));

                    steDetails.AssetTypeId = reader.IsDBNull(reader.GetOrdinal("asset_type_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("asset_type_id"));
                    steDetails.AssetTypeName = reader.IsDBNull(reader.GetOrdinal("asset_type_name")) ? "" : reader.GetString(reader.GetOrdinal("asset_type_name"));
                    steDetails.AssetStatus = reader.IsDBNull(reader.GetOrdinal("asset_status")) ? 0 : reader.GetInt32(reader.GetOrdinal("asset_status"));
                    steDetails.AssetId = reader.IsDBNull(reader.GetOrdinal("asset_id")) ? "" : reader.GetString(reader.GetOrdinal("asset_id"));

                    steDetails.SelectedPropertyStatusId= reader.IsDBNull(reader.GetOrdinal("property_status_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("property_status_id"));
                    steDetails.SelectedPropertyStatus = reader.IsDBNull(reader.GetOrdinal("property_status")) ? "" : reader.GetString(reader.GetOrdinal("property_status"));
                    steDetails.PropertyHeaderLine2 = reader.IsDBNull(reader.GetOrdinal("property_header_line_2")) ? "" : reader.GetString(reader.GetOrdinal("property_header_line_2"));
                    

                    if (steDetails.SiteAddress.Length > 15)
                    {
                        steDetails.SiteAddressSmall = steDetails.SiteAddress.Substring(0, 15) + "..";
                    }
                    else
                    {
                        steDetails.SiteAddressSmall = steDetails.SiteAddress;
                    }

                    if (steDetails.AssetStatus == 0)
                    {
                        steDetails.AssetStatusName = "Available";
                    }
                    else
                    {
                        steDetails.AssetStatusName = "Sold";
                    }

                    steDetails.TransactionStatusName = "";

                    steDetails.DiligenceDispositions = GetDiligenceDispositions(steDetails.SiteDetailsId);
                    
                    steDetails.SelectedDiligenceDispositions = new DiligenceDispositionsViewModel();

                    int saleLoi = 0, saleUnderContract = 0, saleTerminated = 0, saleClosed = 0;



                    steDetails.DiligenceAcquisitions = GetDiligenceAcquisition(steDetails.SiteDetailsId);
                    steDetails.DiligenceDispositions = GetDiligenceDispositions(steDetails.SiteDetailsId);
                    steDetails.DiligenceLease = GetDiligenceLease(steDetails.SiteDetailsId);
                    steDetails.DiligenceLeaseList = GetDiligenceLeaseList(steDetails.SiteDetailsId);
                    steDetails.DiligenceDispositions_SaleLeaseBack = GetDiligenceDispositions_SaleLeaseBack(steDetails.SiteDetailsId);

                    // steDetails.DiligenceDispositions = GetDiligenceDispositions(steDetails.SiteDetailsId);

                    steDetails.SelectedDiligenceDispositions = new DiligenceDispositionsViewModel();
                    DateTime? transactionClosedDate = default(DateTime?);

                    steDetails.DiligenceLeaseWithPurchase = new DiligenceLeaseWithPurchaseViewModel();
                    steDetails.SelectedDiligenceLease = new DiligenceLeaseViewModel();
                    steDetails.DiligenceLeaseWithPurchase = new DiligenceLeaseWithPurchaseViewModel();

                    steDetails.ShowInListing = reader.IsDBNull(reader.GetOrdinal("can_publish")) ? false : reader.GetBoolean(reader.GetOrdinal("can_publish"));

                    if (steDetails.AssetTypeId == (int)SamAssetType.Fee || steDetails.AssetTypeId == (int)SamAssetType.FeeSubjectToLease)
                    {
                        foreach (DiligenceDispositionsViewModel ddm in steDetails.DiligenceDispositions)
                        {
                            
                            if (steDetails.MaxPriorityTransactionStatusId == 0)
                            {
                                steDetails.MaxPriorityTransactionStatusId = ddm.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = ddm.SelectedTransactionStatusName;
                                steDetails.SelectedDiligenceDispositions = ddm;
                                transactionClosedDate = ddm.ClosingDate;
                            }



                            if ((ddm.SelectedTransactionStatusId == (int)TransactionStatus.LOI_Received ||
                                ddm.SelectedTransactionStatusId == (int)TransactionStatus.Terminated_Acquisitions) &&
                                (ddm.SelectedTransactionStatusId != (int)SamsTransactionStatus.Under_LOI &&
                                steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Under_Contract &&
                                steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Closed_Dispositions
                                ))
                            {
                                steDetails.MaxPriorityTransactionStatusId = ddm.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = ddm.SelectedTransactionStatusName;
                                steDetails.StatusChangedDate = ddm.SelectedTransactionDate;
                                steDetails.SelectedDiligenceDispositions = ddm;
                                transactionClosedDate = ddm.ClosingDate;
                            }

                            if (ddm.SelectedTransactionStatusId == (int)SamsTransactionStatus.Under_LOI &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Under_Contract) &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Closed_Dispositions))
                            {
                                steDetails.MaxPriorityTransactionStatusId = ddm.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = ddm.SelectedTransactionStatusName;
                                steDetails.StatusChangedDate = ddm.SelectedTransactionDate;
                                steDetails.SelectedDiligenceDispositions = ddm;
                                transactionClosedDate = ddm.ClosingDate;
                            }

                            if (ddm.SelectedTransactionStatusId == (int)SamsTransactionStatus.Under_Contract &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Closed_Dispositions))
                            {
                                steDetails.MaxPriorityTransactionStatusId = ddm.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = ddm.SelectedTransactionStatusName;
                                steDetails.StatusChangedDate = ddm.SelectedTransactionDate;
                                steDetails.SelectedDiligenceDispositions = ddm;
                                transactionClosedDate = ddm.ClosingDate;
                            }

                            if (ddm.SelectedTransactionStatusId == (int)SamsTransactionStatus.Closed_Dispositions)
                            {
                                steDetails.MaxPriorityTransactionStatusId = ddm.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = ddm.SelectedTransactionStatusName;
                                steDetails.StatusChangedDate = ddm.SelectedTransactionDate;
                                steDetails.SelectedDiligenceDispositions = ddm;
                                transactionClosedDate = ddm.ClosingDate;
                            }

                        }


                        var dtClosedDate = "";
                        var daysToClose = 0;
                        if (transactionClosedDate != default(DateTime?))
                        {
                            dtClosedDate = transactionClosedDate.Value.ToString("MM/dd/yyyy");
                            daysToClose = (transactionClosedDate.Value - DateTime.Now).Days;
                            if (daysToClose < 0)
                            {
                                daysToClose = 0;
                            }
                        }
                    }
                    else if (steDetails.AssetTypeId == (int)SamAssetType.Lease)
                    {
                        steDetails.DiligenceLeaseList = GetDiligenceLeaseList(steDetails.SiteDetailsId);
                        int leaseLoi = 0, leaseUnderContract = 0, leaseTerminated = 0, leaseClosed = 0;
                        foreach (DiligenceLeaseViewModel dl in steDetails.DiligenceLeaseList)
                        {
                            if (dl.SelectedTransactionStatusId == (int)TransactionStatus.Under_LOI)
                            {
                                leaseLoi = leaseLoi + 1;
                            }
                            else if (dl.SelectedTransactionStatusId == (int)TransactionStatus.Under_Contract)
                            {
                                leaseUnderContract = leaseUnderContract + 1;
                            }
                            else if (dl.SelectedTransactionStatusId == (int)TransactionStatus.Terminated_Acquisitions)
                            {
                                leaseTerminated = leaseTerminated + 1;
                            }
                            else if (dl.SelectedTransactionStatusId == (int)TransactionStatus.Closed_Acquisitions)
                            {
                                leaseClosed = leaseClosed + 1;
                            }

                            if (steDetails.MaxPriorityTransactionStatusId == 0)
                            {
                                steDetails.MaxPriorityTransactionStatusId = dl.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = dl.SelectedTransactionStatusName;
                                steDetails.SelectedDiligenceLease = dl;
                                transactionClosedDate = dl.ClosingDate;
                            }



                            if ((dl.SelectedTransactionStatusId == (int)TransactionStatus.LOI_Received ||
                                dl.SelectedTransactionStatusId == (int)TransactionStatus.Terminated_Acquisitions) &&
                                (dl.SelectedTransactionStatusId != (int)SamsTransactionStatus.Under_LOI &&
                                steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Under_Contract &&
                                steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Closed_Dispositions
                                ))
                            {
                                steDetails.MaxPriorityTransactionStatusId = dl.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = dl.SelectedTransactionStatusName;
                                steDetails.StatusChangedDate = dl.SelectedTransactionDate;
                                steDetails.SelectedDiligenceLease = dl;
                                transactionClosedDate = dl.ClosingDate;
                            }

                            if (dl.SelectedTransactionStatusId == (int)SamsTransactionStatus.Under_LOI &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Under_Contract) &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Closed_Dispositions))
                            {
                                steDetails.MaxPriorityTransactionStatusId = dl.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = dl.SelectedTransactionStatusName;
                                steDetails.StatusChangedDate = dl.SelectedTransactionDate;
                                steDetails.SelectedDiligenceLease = dl;
                                transactionClosedDate = dl.ClosingDate;
                            }

                            if (dl.SelectedTransactionStatusId == (int)SamsTransactionStatus.Under_Contract &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Closed_Dispositions))
                            {
                                steDetails.MaxPriorityTransactionStatusId = dl.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = dl.SelectedTransactionStatusName;
                                steDetails.StatusChangedDate = dl.SelectedTransactionDate;
                                steDetails.SelectedDiligenceLease = dl;
                                transactionClosedDate = dl.ClosingDate;
                            }

                            if (dl.SelectedTransactionStatusId == (int)SamsTransactionStatus.Closed_Dispositions)
                            {
                                steDetails.MaxPriorityTransactionStatusId = dl.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = dl.SelectedTransactionStatusName;
                                steDetails.StatusChangedDate = dl.SelectedTransactionDate;
                                steDetails.SelectedDiligenceLease = dl;
                                transactionClosedDate = dl.ClosingDate;
                            }
                        }


                        if (steDetails.SelectedDiligenceLease != null)
                        {
                            var dtClosedDate = "";
                            var daysToClose = 0;
                            if (transactionClosedDate != default(DateTime?))
                            {
                                dtClosedDate = transactionClosedDate.Value.ToString("MM/dd/yyyy");
                                daysToClose = (transactionClosedDate.Value - DateTime.Now).Days;
                                if (daysToClose < 0)
                                {
                                    daysToClose = 0;
                                }
                            }
                        }

                    }
                    else if (steDetails.AssetTypeId == (int)SamAssetType.LeaseWithPurchaseOption)
                    {
                        steDetails.DiligenceLeaseWithPurchaseList = GetDiligenceLeaseWithPurchaseList(steDetails.SiteDetailsId);

                        foreach (DiligenceLeaseWithPurchaseViewModel dl in steDetails.DiligenceLeaseWithPurchaseList)
                        {

                            steDetails.DiligenceLeaseWithPurchase = dl;
                            if (steDetails.MaxPriorityTransactionStatusId == 0)
                            {
                                steDetails.MaxPriorityTransactionStatusId = dl.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = dl.SelectedTransactionStatusName;

                                steDetails.DiligenceLeaseWithPurchase = dl;
                                transactionClosedDate = dl.ClosingDate;
                            }



                            if ((dl.SelectedTransactionStatusId == (int)TransactionStatus.LOI_Received ||
                                dl.SelectedTransactionStatusId == (int)TransactionStatus.Terminated_Acquisitions) &&
                                (dl.SelectedTransactionStatusId != (int)SamsTransactionStatus.Under_LOI &&
                                steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Under_Contract &&
                                steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Closed_Dispositions
                                ))
                            {
                                steDetails.MaxPriorityTransactionStatusId = dl.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = dl.SelectedTransactionStatusName;
                                steDetails.DiligenceLeaseWithPurchase = dl;
                                transactionClosedDate = dl.ClosingDate;
                            }

                            if (dl.SelectedTransactionStatusId == (int)SamsTransactionStatus.Under_LOI &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Under_Contract) &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Closed_Dispositions))
                            {
                                steDetails.MaxPriorityTransactionStatusId = dl.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = dl.SelectedTransactionStatusName;
                                steDetails.DiligenceLeaseWithPurchase = dl;
                                transactionClosedDate = dl.ClosingDate;
                            }

                            if (dl.SelectedTransactionStatusId == (int)SamsTransactionStatus.Under_Contract &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Closed_Dispositions))
                            {
                                steDetails.MaxPriorityTransactionStatusId = dl.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = dl.SelectedTransactionStatusName;
                                steDetails.DiligenceLeaseWithPurchase = dl;
                                transactionClosedDate = dl.ClosingDate;
                            }

                            if (dl.SelectedTransactionStatusId == (int)SamsTransactionStatus.Closed_Dispositions)
                            {
                                steDetails.MaxPriorityTransactionStatusId = dl.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = dl.SelectedTransactionStatusName;
                                steDetails.DiligenceLeaseWithPurchase = dl;
                                transactionClosedDate = dl.ClosingDate;
                            }
                        }



                        if (steDetails.DiligenceLeaseWithPurchase != null)
                        {

                            var dtClosedDate = "";
                            var daysToClose = 0;
                            if (transactionClosedDate != default(DateTime?))
                            {
                                dtClosedDate = transactionClosedDate.Value.ToString("MM/dd/yyyy");
                                daysToClose = (transactionClosedDate.Value - DateTime.Now).Days;
                                if (daysToClose < 0)
                                {
                                    daysToClose = 0;
                                }
                            }

                        }
                    }
                    else if (steDetails.AssetTypeId == (int)SamAssetType.SaleLeaseBack)
                    {
                        foreach (DiligenceDispositionsViewModel ddv in steDetails.DiligenceDispositions_SaleLeaseBack)
                        {

                            if (ddv.SelectedTransactionStatusId == (int)TransactionStatus.Closed_Acquisitions || ddv.SelectedTransactionStatusId == (int)TransactionStatus.Under_Contract || ddv.SelectedTransactionStatusId == (int)TransactionStatus.Under_LOI)
                            {
                                steDetails.CanAddTransactions = false;
                            }

                            if (ddv.SelectedTransactionStatusId == (int)TransactionStatus.Closed_Acquisitions)
                            {
                                steDetails.IsTransactionClosed = true;
                            }

                            if (steDetails.MaxPriorityTransactionStatusId == 0)
                            {
                                steDetails.MaxPriorityTransactionStatusId = ddv.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = ddv.SelectedTransactionStatusName;
                            }
                            steDetails.SelectedDiligenceDispositions = ddv;
                            transactionClosedDate = ddv.ClosingDate;


                            if ((ddv.SelectedTransactionStatusId == (int)TransactionStatus.LOI_Received ||
                                ddv.SelectedTransactionStatusId == (int)TransactionStatus.Terminated_Acquisitions) &&
                                (ddv.SelectedTransactionStatusId != (int)SamsTransactionStatus.Under_LOI &&
                                steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Under_Contract &&
                                steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Closed_Dispositions
                                ))
                            {
                                steDetails.MaxPriorityTransactionStatusId = ddv.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = ddv.SelectedTransactionStatusName;
                                steDetails.SelectedDiligenceDispositions = ddv;
                                transactionClosedDate = ddv.ClosingDate;
                            }

                            if (ddv.SelectedTransactionStatusId == (int)SamsTransactionStatus.Under_LOI &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Under_Contract) &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Closed_Dispositions))
                            {
                                steDetails.MaxPriorityTransactionStatusId = ddv.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = ddv.SelectedTransactionStatusName;
                                steDetails.SelectedDiligenceDispositions = ddv;
                                transactionClosedDate = ddv.ClosingDate;
                            }

                            if (ddv.SelectedTransactionStatusId == (int)SamsTransactionStatus.Under_Contract &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Closed_Dispositions))
                            {
                                steDetails.MaxPriorityTransactionStatusId = ddv.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = ddv.SelectedTransactionStatusName;
                                steDetails.SelectedDiligenceDispositions = ddv;
                                transactionClosedDate = ddv.ClosingDate;
                            }

                            if (ddv.SelectedTransactionStatusId == (int)SamsTransactionStatus.Closed_Dispositions)
                            {
                                steDetails.MaxPriorityTransactionStatusId = ddv.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = ddv.SelectedTransactionStatusName;
                                steDetails.SelectedDiligenceDispositions = ddv;
                                transactionClosedDate = ddv.ClosingDate;
                            }


                        }

                        var dtClosedDate = "";
                        var daysToClose = 0;
                        if (transactionClosedDate != default(DateTime?))
                        {
                            dtClosedDate = transactionClosedDate.Value.ToString("MM/dd/yyyy");
                            daysToClose = (transactionClosedDate.Value - DateTime.Now).Days;
                            if (daysToClose < 0)
                            {
                                daysToClose = 0;
                            }
                        }

                    }

                    /*
                    if (steDetails.AssetTypeId == (int)SamAssetType.Fee)
                    {
                        foreach (DiligenceDispositionsViewModel ddm in steDetails.DiligenceDispositions)
                        {
                            if (ddm.SelectedTransactionStatusId == (int)TransactionStatus.Under_LOI)
                            {
                                saleLoi = saleLoi + 1;
                            }
                            else if (ddm.SelectedTransactionStatusId == (int)TransactionStatus.Under_Contract)
                            {
                                saleUnderContract = saleUnderContract + 1;
                            }
                            else if (ddm.SelectedTransactionStatusId == (int)TransactionStatus.Terminated_Acquisitions)
                            {
                                saleTerminated = saleTerminated + 1;
                            }
                            else if (ddm.SelectedTransactionStatusId == (int)TransactionStatus.Closed_Acquisitions)
                            {
                                saleClosed = saleClosed + 1;
                            }


                            if (steDetails.MaxPriorityTransactionStatusId == 0)
                            {
                                steDetails.MaxPriorityTransactionStatusId = ddm.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = ddm.SelectedTransactionStatusName;
                                steDetails.SelectedDiligenceDispositions = ddm;
                            }



                            if ((ddm.SelectedTransactionStatusId == (int)TransactionStatus.LOI_Received ||
                                ddm.SelectedTransactionStatusId == (int)TransactionStatus.Terminated_Acquisitions) &&
                                (ddm.SelectedTransactionStatusId != (int)SamsTransactionStatus.Under_LOI &&
                                steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Under_Contract &&
                                steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Closed_Dispositions
                                ))
                            {
                                steDetails.MaxPriorityTransactionStatusId = ddm.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = ddm.SelectedTransactionStatusName;
                                steDetails.StatusChangedDate = ddm.SelectedTransactionDate;
                                steDetails.SelectedDiligenceDispositions = ddm;
                            }

                            if (ddm.SelectedTransactionStatusId == (int)SamsTransactionStatus.Under_LOI &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Under_Contract) &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Closed_Dispositions))
                            {
                                steDetails.MaxPriorityTransactionStatusId = ddm.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = ddm.SelectedTransactionStatusName;
                                steDetails.StatusChangedDate = ddm.SelectedTransactionDate;
                                steDetails.SelectedDiligenceDispositions = ddm;
                            }

                            if (ddm.SelectedTransactionStatusId == (int)SamsTransactionStatus.Under_Contract &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Closed_Dispositions))
                            {
                                steDetails.MaxPriorityTransactionStatusId = ddm.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = ddm.SelectedTransactionStatusName;
                                steDetails.StatusChangedDate = ddm.SelectedTransactionDate;
                                steDetails.SelectedDiligenceDispositions = ddm;
                            }

                            if (ddm.SelectedTransactionStatusId == (int)SamsTransactionStatus.Closed_Dispositions)
                            {
                                steDetails.MaxPriorityTransactionStatusId = ddm.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = ddm.SelectedTransactionStatusName;
                                steDetails.StatusChangedDate = ddm.SelectedTransactionDate;
                                steDetails.SelectedDiligenceDispositions = ddm;
                            }

                        }

                        
                    }
                    else if (steDetails.AssetTypeId == (int)SamAssetType.Lease)
                    {
                        steDetails.DiligenceLeaseList = GetDiligenceLeaseList(steDetails.SiteDetailsId);
                        int leaseLoi = 0, leaseUnderContract = 0, leaseTerminated = 0, leaseClosed = 0;
                        foreach (DiligenceLeaseViewModel dl in steDetails.DiligenceLeaseList)
                        {
                            if (dl.SelectedTransactionStatusId == (int)TransactionStatus.Under_LOI)
                            {
                                leaseLoi = leaseLoi + 1;
                            }
                            else if (dl.SelectedTransactionStatusId == (int)TransactionStatus.Under_Contract)
                            {
                                leaseUnderContract = leaseUnderContract + 1;
                            }
                            else if (dl.SelectedTransactionStatusId == (int)TransactionStatus.Terminated_Acquisitions)
                            {
                                leaseTerminated = leaseTerminated + 1;
                            }
                            else if (dl.SelectedTransactionStatusId == (int)TransactionStatus.Closed_Acquisitions)
                            {
                                leaseClosed = leaseClosed + 1;
                            }

                            if (steDetails.MaxPriorityTransactionStatusId == 0)
                            {
                                steDetails.MaxPriorityTransactionStatusId = dl.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = dl.SelectedTransactionStatusName;
                            }



                            if ((dl.SelectedTransactionStatusId == (int)TransactionStatus.LOI_Received ||
                                dl.SelectedTransactionStatusId == (int)TransactionStatus.Terminated_Acquisitions) &&
                                (dl.SelectedTransactionStatusId != (int)SamsTransactionStatus.Under_LOI &&
                                steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Under_Contract &&
                                steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Closed_Dispositions
                                ))
                            {
                                steDetails.MaxPriorityTransactionStatusId = dl.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = dl.SelectedTransactionStatusName;
                                steDetails.StatusChangedDate = dl.SelectedTransactionDate;
                            }

                            if (dl.SelectedTransactionStatusId == (int)SamsTransactionStatus.Under_LOI &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Under_Contract) &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Closed_Dispositions))
                            {
                                steDetails.MaxPriorityTransactionStatusId = dl.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = dl.SelectedTransactionStatusName;
                                steDetails.StatusChangedDate = dl.SelectedTransactionDate;
                            }

                            if (dl.SelectedTransactionStatusId == (int)SamsTransactionStatus.Under_Contract &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Closed_Dispositions))
                            {
                                steDetails.MaxPriorityTransactionStatusId = dl.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = dl.SelectedTransactionStatusName;
                                steDetails.StatusChangedDate = dl.SelectedTransactionDate;
                            }

                            if (dl.SelectedTransactionStatusId == (int)SamsTransactionStatus.Closed_Dispositions)
                            {
                                steDetails.MaxPriorityTransactionStatusId = dl.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = dl.SelectedTransactionStatusName;
                                steDetails.StatusChangedDate = dl.SelectedTransactionDate;
                            }
                        }
                        string leaseData = "";
                        if (leaseLoi > 0)
                        {
                            leaseData = leaseData + "Under LOI : " + leaseLoi + "; ";
                        }

                        if (leaseUnderContract > 0)
                        {
                            leaseData = leaseData + "Under Contract : " + leaseUnderContract + "; ";
                        }
                        if (leaseTerminated > 0)
                        {
                            leaseData = leaseData + "Terminated : " + leaseTerminated + "; ";
                        }
                        if (leaseClosed > 0)
                        {
                            leaseData = leaseData + "Closed : " + leaseClosed + "; ";
                        }

                        if (leaseData.Trim().Length > 0)
                        {
                            leaseData = " Lease : " + leaseData;
                        }
                    }
                    */

                    steDetails.TodoList = GetTodoList(steDetails.SiteDetailsId);
                    if(steDetails.TodoList.Count > 0)
                    {
                        steDetails.LatestComment = steDetails.TodoList[0].TodoText;
                    }
                    

                    surplusPropertiesList.Add(steDetails);
                }
                con.Close();
            }

            return View(surplusPropertiesList);
        }


        public IActionResult GetAcquisitions(int acquisitionStatus)
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

            List<SiteDetails> surplusPropertiesList = new List<SiteDetails>();

            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("GetAllSurplusPropertyAcquisition", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("asset_status", 0);
                cmd.Parameters.AddWithValue("acquisition_status", acquisitionStatus);
                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var steDetails = new SiteDetails();
                    steDetails.SiteDetailsId = reader.IsDBNull(reader.GetOrdinal("site_details_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("site_details_id"));
                    steDetails.NamePrefix = reader.IsDBNull(reader.GetOrdinal("name_prefix")) ? "" : reader.GetString(reader.GetOrdinal("name_prefix"));
                    steDetails.FirstName = reader.IsDBNull(reader.GetOrdinal("first_name")) ? "" : reader.GetString(reader.GetOrdinal("first_name"));

                    steDetails.LastName = reader.IsDBNull(reader.GetOrdinal("last_name")) ? "" : reader.GetString(reader.GetOrdinal("last_name"));
                    steDetails.CompanyName = reader.IsDBNull(reader.GetOrdinal("company_name")) ? "" : reader.GetString(reader.GetOrdinal("company_name"));
                    steDetails.EmailAddress = reader.IsDBNull(reader.GetOrdinal("email_address")) ? "" : reader.GetString(reader.GetOrdinal("email_address"));
                    steDetails.Address = reader.IsDBNull(reader.GetOrdinal("address")) ? "" : reader.GetString(reader.GetOrdinal("address"));
                    steDetails.CityName = reader.IsDBNull(reader.GetOrdinal("city_name")) ? "" : reader.GetString(reader.GetOrdinal("city_name"));
                    steDetails.StateId = reader.IsDBNull(reader.GetOrdinal("state_id")) ? "" : reader.GetString(reader.GetOrdinal("state_id"));

                    steDetails.StateName = reader.IsDBNull(reader.GetOrdinal("state_name")) ? "" : reader.GetString(reader.GetOrdinal("state_name"));

                    steDetails.ZipCode = reader.IsDBNull(reader.GetOrdinal("zip_code")) ? "" : reader.GetString(reader.GetOrdinal("zip_code"));
                    steDetails.ContactNumber = reader.IsDBNull(reader.GetOrdinal("contact_number")) ? "" : reader.GetString(reader.GetOrdinal("contact_number"));
                    steDetails.SamsHoldingEmployee = reader.IsDBNull(reader.GetOrdinal("sams_holding_employee")) ? false : reader.GetBoolean(reader.GetOrdinal("sams_holding_employee"));
                    steDetails.MarketId = reader.IsDBNull(reader.GetOrdinal("market_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("market_id"));
                    steDetails.PropertyHeader = reader.IsDBNull(reader.GetOrdinal("property_header")) ? "" : reader.GetString(reader.GetOrdinal("property_header"));
                    steDetails.SiteAddress = reader.IsDBNull(reader.GetOrdinal("site_address")) ? "" : reader.GetString(reader.GetOrdinal("site_address"));
                    steDetails.SiteCity = reader.IsDBNull(reader.GetOrdinal("site_city")) ? "" : reader.GetString(reader.GetOrdinal("site_city"));
                    steDetails.SiteStateId = reader.IsDBNull(reader.GetOrdinal("site_state_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("site_state_id"));

                    steDetails.SiteStateName = reader.IsDBNull(reader.GetOrdinal("site_state_name")) ? "" : reader.GetString(reader.GetOrdinal("site_state_name"));

                    steDetails.SiteCounty = reader.IsDBNull(reader.GetOrdinal("site_county")) ? "" : reader.GetString(reader.GetOrdinal("site_county"));
                    steDetails.SiteCrossStreetName = reader.IsDBNull(reader.GetOrdinal("site_cross_street_name")) ? "" : reader.GetString(reader.GetOrdinal("site_cross_street_name"));
                    steDetails.IsPropertyAvailable = reader.IsDBNull(reader.GetOrdinal("is_property_available")) ? true : reader.GetBoolean(reader.GetOrdinal("is_property_available"));
                    steDetails.Zoning = reader.IsDBNull(reader.GetOrdinal("zoning")) ? "" : reader.GetString(reader.GetOrdinal("zoning"));
                    steDetails.LotSize = reader.IsDBNull(reader.GetOrdinal("lot_size")) ? "" : reader.GetString(reader.GetOrdinal("lot_size"));

                    steDetails.SalesPrice = reader.IsDBNull(reader.GetOrdinal("sales_price")) ? "" : reader.GetString(reader.GetOrdinal("sales_price"));
                    //steDetails.SalesPrice = Helper.FormatCurrency("$", steDetails.SalesPrice);

                    steDetails.Comments = reader.IsDBNull(reader.GetOrdinal("comments")) ? "" : reader.GetString(reader.GetOrdinal("comments"));

                    steDetails.CreatedDate = reader.IsDBNull(reader.GetOrdinal("created_date")) ? DateTime.Now : reader.GetDateTime(reader.GetOrdinal("created_date"));
                    steDetails.PropertyType = reader.IsDBNull(reader.GetOrdinal("property_type")) ? 0 : reader.GetInt32(reader.GetOrdinal("property_type"));

                    steDetails.ImageName = reader.IsDBNull(reader.GetOrdinal("image_name")) ? "" : reader.GetString(reader.GetOrdinal("image_name"));

                    steDetails.AssetTypeId = reader.IsDBNull(reader.GetOrdinal("asset_type_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("asset_type_id"));
                    steDetails.AssetTypeName = reader.IsDBNull(reader.GetOrdinal("asset_type_name")) ? "" : reader.GetString(reader.GetOrdinal("asset_type_name"));
                    steDetails.AssetStatus = reader.IsDBNull(reader.GetOrdinal("asset_status")) ? 0 : reader.GetInt32(reader.GetOrdinal("asset_status"));
                    steDetails.AssetId = reader.IsDBNull(reader.GetOrdinal("asset_id")) ? "" : reader.GetString(reader.GetOrdinal("asset_id"));

                    steDetails.SelectedPropertyStatusId = reader.IsDBNull(reader.GetOrdinal("property_status_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("property_status_id"));
                    steDetails.SelectedPropertyStatus = reader.IsDBNull(reader.GetOrdinal("property_status")) ? "" : reader.GetString(reader.GetOrdinal("property_status"));

                    if (steDetails.SiteAddress.Length > 15)
                    {
                        steDetails.SiteAddressSmall = steDetails.SiteAddress.Substring(0, 15) + "..";
                    }
                    else
                    {
                        steDetails.SiteAddressSmall = steDetails.SiteAddress;
                    }

                    if (steDetails.AssetStatus == 0)
                    {
                        steDetails.AssetStatusName = "Available";
                    }
                    else
                    {
                        steDetails.AssetStatusName = "Sold";
                    }
                    surplusPropertiesList.Add(steDetails);
                }
                con.Close();
            }

            return View(surplusPropertiesList);
        }


        public IActionResult GetDispositions(int dispositionStatus)
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

            List<SiteDetails> surplusPropertiesList = new List<SiteDetails>();

            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("GetAllSurplusPropertyDisposition", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("asset_status", 0);
                cmd.Parameters.AddWithValue("disposition_status", dispositionStatus);
                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var steDetails = new SiteDetails();
                    steDetails.SiteDetailsId = reader.IsDBNull(reader.GetOrdinal("site_details_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("site_details_id"));
                    steDetails.NamePrefix = reader.IsDBNull(reader.GetOrdinal("name_prefix")) ? "" : reader.GetString(reader.GetOrdinal("name_prefix"));
                    steDetails.FirstName = reader.IsDBNull(reader.GetOrdinal("first_name")) ? "" : reader.GetString(reader.GetOrdinal("first_name"));

                    steDetails.LastName = reader.IsDBNull(reader.GetOrdinal("last_name")) ? "" : reader.GetString(reader.GetOrdinal("last_name"));
                    steDetails.CompanyName = reader.IsDBNull(reader.GetOrdinal("company_name")) ? "" : reader.GetString(reader.GetOrdinal("company_name"));
                    steDetails.EmailAddress = reader.IsDBNull(reader.GetOrdinal("email_address")) ? "" : reader.GetString(reader.GetOrdinal("email_address"));
                    steDetails.Address = reader.IsDBNull(reader.GetOrdinal("address")) ? "" : reader.GetString(reader.GetOrdinal("address"));
                    steDetails.CityName = reader.IsDBNull(reader.GetOrdinal("city_name")) ? "" : reader.GetString(reader.GetOrdinal("city_name"));
                    steDetails.StateId = reader.IsDBNull(reader.GetOrdinal("state_id")) ? "" : reader.GetString(reader.GetOrdinal("state_id"));

                    steDetails.StateName = reader.IsDBNull(reader.GetOrdinal("state_name")) ? "" : reader.GetString(reader.GetOrdinal("state_name"));

                    steDetails.ZipCode = reader.IsDBNull(reader.GetOrdinal("zip_code")) ? "" : reader.GetString(reader.GetOrdinal("zip_code"));
                    steDetails.ContactNumber = reader.IsDBNull(reader.GetOrdinal("contact_number")) ? "" : reader.GetString(reader.GetOrdinal("contact_number"));
                    steDetails.SamsHoldingEmployee = reader.IsDBNull(reader.GetOrdinal("sams_holding_employee")) ? false : reader.GetBoolean(reader.GetOrdinal("sams_holding_employee"));
                    steDetails.MarketId = reader.IsDBNull(reader.GetOrdinal("market_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("market_id"));
                    steDetails.PropertyHeader = reader.IsDBNull(reader.GetOrdinal("property_header")) ? "" : reader.GetString(reader.GetOrdinal("property_header"));
                    steDetails.SiteAddress = reader.IsDBNull(reader.GetOrdinal("site_address")) ? "" : reader.GetString(reader.GetOrdinal("site_address"));
                    steDetails.SiteCity = reader.IsDBNull(reader.GetOrdinal("site_city")) ? "" : reader.GetString(reader.GetOrdinal("site_city"));
                    steDetails.SiteStateId = reader.IsDBNull(reader.GetOrdinal("site_state_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("site_state_id"));

                    steDetails.SiteStateName = reader.IsDBNull(reader.GetOrdinal("site_state_name")) ? "" : reader.GetString(reader.GetOrdinal("site_state_name"));

                    steDetails.SiteCounty = reader.IsDBNull(reader.GetOrdinal("site_county")) ? "" : reader.GetString(reader.GetOrdinal("site_county"));
                    steDetails.SiteCrossStreetName = reader.IsDBNull(reader.GetOrdinal("site_cross_street_name")) ? "" : reader.GetString(reader.GetOrdinal("site_cross_street_name"));
                    steDetails.IsPropertyAvailable = reader.IsDBNull(reader.GetOrdinal("is_property_available")) ? true : reader.GetBoolean(reader.GetOrdinal("is_property_available"));
                    steDetails.Zoning = reader.IsDBNull(reader.GetOrdinal("zoning")) ? "" : reader.GetString(reader.GetOrdinal("zoning"));
                    steDetails.LotSize = reader.IsDBNull(reader.GetOrdinal("lot_size")) ? "" : reader.GetString(reader.GetOrdinal("lot_size"));

                    steDetails.SalesPrice = reader.IsDBNull(reader.GetOrdinal("sales_price")) ? "" : reader.GetString(reader.GetOrdinal("sales_price"));
                    //steDetails.SalesPrice = Helper.FormatCurrency("$", steDetails.SalesPrice);

                    steDetails.Comments = reader.IsDBNull(reader.GetOrdinal("comments")) ? "" : reader.GetString(reader.GetOrdinal("comments"));

                    steDetails.CreatedDate = reader.IsDBNull(reader.GetOrdinal("created_date")) ? DateTime.Now : reader.GetDateTime(reader.GetOrdinal("created_date"));
                    steDetails.PropertyType = reader.IsDBNull(reader.GetOrdinal("property_type")) ? 0 : reader.GetInt32(reader.GetOrdinal("property_type"));

                    steDetails.ImageName = reader.IsDBNull(reader.GetOrdinal("image_name")) ? "" : reader.GetString(reader.GetOrdinal("image_name"));

                    steDetails.AssetTypeId = reader.IsDBNull(reader.GetOrdinal("asset_type_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("asset_type_id"));
                    steDetails.AssetTypeName = reader.IsDBNull(reader.GetOrdinal("asset_type_name")) ? "" : reader.GetString(reader.GetOrdinal("asset_type_name"));
                    steDetails.AssetStatus = reader.IsDBNull(reader.GetOrdinal("asset_status")) ? 0 : reader.GetInt32(reader.GetOrdinal("asset_status"));
                    steDetails.AssetId = reader.IsDBNull(reader.GetOrdinal("asset_id")) ? "" : reader.GetString(reader.GetOrdinal("asset_id"));

                    steDetails.SelectedPropertyStatusId = reader.IsDBNull(reader.GetOrdinal("property_status_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("property_status_id"));
                    steDetails.SelectedPropertyStatus = reader.IsDBNull(reader.GetOrdinal("property_status")) ? "" : reader.GetString(reader.GetOrdinal("property_status"));

                    if (steDetails.SiteAddress.Length > 15)
                    {
                        steDetails.SiteAddressSmall = steDetails.SiteAddress.Substring(0, 15) + "..";
                    }
                    else
                    {
                        steDetails.SiteAddressSmall = steDetails.SiteAddress;
                    }


                    if (steDetails.AssetStatus == 0)
                    {
                        steDetails.AssetStatusName = "Available";
                    }
                    else
                    {
                        steDetails.AssetStatusName = "Sold";
                    }
                    surplusPropertiesList.Add(steDetails);
                }
                con.Close();
            }

            return View(surplusPropertiesList);
        }


        public IActionResult GetSoldoutProperties()
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

            List<SiteDetails> surplusPropertiesList = new List<SiteDetails>();

            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("GetPropertyListByCategory", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("asset_status", 1);
                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var steDetails = new SiteDetails();
                    steDetails.SiteDetailsId = reader.IsDBNull(reader.GetOrdinal("site_details_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("site_details_id"));
                    steDetails.NamePrefix = reader.IsDBNull(reader.GetOrdinal("name_prefix")) ? "" : reader.GetString(reader.GetOrdinal("name_prefix"));
                    steDetails.FirstName = reader.IsDBNull(reader.GetOrdinal("first_name")) ? "" : reader.GetString(reader.GetOrdinal("first_name"));

                    steDetails.LastName = reader.IsDBNull(reader.GetOrdinal("last_name")) ? "" : reader.GetString(reader.GetOrdinal("last_name"));
                    steDetails.CompanyName = reader.IsDBNull(reader.GetOrdinal("company_name")) ? "" : reader.GetString(reader.GetOrdinal("company_name"));
                    steDetails.EmailAddress = reader.IsDBNull(reader.GetOrdinal("email_address")) ? "" : reader.GetString(reader.GetOrdinal("email_address"));
                    steDetails.Address = reader.IsDBNull(reader.GetOrdinal("address")) ? "" : reader.GetString(reader.GetOrdinal("address"));
                    steDetails.CityName = reader.IsDBNull(reader.GetOrdinal("city_name")) ? "" : reader.GetString(reader.GetOrdinal("city_name"));
                    steDetails.StateId = reader.IsDBNull(reader.GetOrdinal("state_id")) ? "" : reader.GetString(reader.GetOrdinal("state_id"));

                    steDetails.StateName = reader.IsDBNull(reader.GetOrdinal("state_name")) ? "" : reader.GetString(reader.GetOrdinal("state_name"));

                    steDetails.ZipCode = reader.IsDBNull(reader.GetOrdinal("zip_code")) ? "" : reader.GetString(reader.GetOrdinal("zip_code"));
                    steDetails.ContactNumber = reader.IsDBNull(reader.GetOrdinal("contact_number")) ? "" : reader.GetString(reader.GetOrdinal("contact_number"));
                    steDetails.SamsHoldingEmployee = reader.IsDBNull(reader.GetOrdinal("sams_holding_employee")) ? false : reader.GetBoolean(reader.GetOrdinal("sams_holding_employee"));
                    steDetails.MarketId = reader.IsDBNull(reader.GetOrdinal("market_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("market_id"));
                    steDetails.PropertyHeader = reader.IsDBNull(reader.GetOrdinal("property_header")) ? "" : reader.GetString(reader.GetOrdinal("property_header"));
                    steDetails.SiteAddress = reader.IsDBNull(reader.GetOrdinal("site_address")) ? "" : reader.GetString(reader.GetOrdinal("site_address"));
                    steDetails.SiteCity = reader.IsDBNull(reader.GetOrdinal("site_city")) ? "" : reader.GetString(reader.GetOrdinal("site_city"));
                    steDetails.SiteStateId = reader.IsDBNull(reader.GetOrdinal("site_state_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("site_state_id"));

                    steDetails.SiteStateName = reader.IsDBNull(reader.GetOrdinal("site_state_name")) ? "" : reader.GetString(reader.GetOrdinal("site_state_name"));

                    steDetails.SiteCounty = reader.IsDBNull(reader.GetOrdinal("site_county")) ? "" : reader.GetString(reader.GetOrdinal("site_county"));
                    steDetails.SiteCrossStreetName = reader.IsDBNull(reader.GetOrdinal("site_cross_street_name")) ? "" : reader.GetString(reader.GetOrdinal("site_cross_street_name"));
                    steDetails.IsPropertyAvailable = reader.IsDBNull(reader.GetOrdinal("is_property_available")) ? true : reader.GetBoolean(reader.GetOrdinal("is_property_available"));
                    steDetails.Zoning = reader.IsDBNull(reader.GetOrdinal("zoning")) ? "" : reader.GetString(reader.GetOrdinal("zoning"));
                    steDetails.LotSize = reader.IsDBNull(reader.GetOrdinal("lot_size")) ? "" : reader.GetString(reader.GetOrdinal("lot_size"));

                    steDetails.SalesPrice = reader.IsDBNull(reader.GetOrdinal("sales_price")) ? "" : reader.GetString(reader.GetOrdinal("sales_price"));
                    //steDetails.SalesPrice = Helper.FormatCurrency("$", steDetails.SalesPrice);

                    steDetails.Comments = reader.IsDBNull(reader.GetOrdinal("comments")) ? "" : reader.GetString(reader.GetOrdinal("comments"));

                    steDetails.CreatedDate = reader.IsDBNull(reader.GetOrdinal("created_date")) ? DateTime.Now : reader.GetDateTime(reader.GetOrdinal("created_date"));
                    steDetails.PropertyType = reader.IsDBNull(reader.GetOrdinal("property_type")) ? 0 : reader.GetInt32(reader.GetOrdinal("property_type"));

                    steDetails.ImageName = reader.IsDBNull(reader.GetOrdinal("image_name")) ? "" : reader.GetString(reader.GetOrdinal("image_name"));

                    steDetails.AssetTypeId = reader.IsDBNull(reader.GetOrdinal("asset_type_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("asset_type_id"));
                    steDetails.AssetTypeName = reader.IsDBNull(reader.GetOrdinal("asset_type_name")) ? "" : reader.GetString(reader.GetOrdinal("asset_type_name"));
                    steDetails.AssetStatus = reader.IsDBNull(reader.GetOrdinal("asset_status")) ? 0 : reader.GetInt32(reader.GetOrdinal("asset_status"));
                    steDetails.AssetId = reader.IsDBNull(reader.GetOrdinal("asset_id")) ? "" : reader.GetString(reader.GetOrdinal("asset_id"));

                    steDetails.SelectedPropertyStatusId = reader.IsDBNull(reader.GetOrdinal("property_status_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("property_status_id"));
                    steDetails.SelectedPropertyStatus = reader.IsDBNull(reader.GetOrdinal("property_status")) ? "" : reader.GetString(reader.GetOrdinal("property_status"));
                    steDetails.StatusChangedDate = reader.IsDBNull(reader.GetOrdinal("status_changed_date")) ? DateTime.Now : reader.GetDateTime(reader.GetOrdinal("status_changed_date"));

                    if (steDetails.SiteAddress.Length > 15)
                    {
                        steDetails.SiteAddressSmall = steDetails.SiteAddress.Substring(0, 15) + "..";
                    }
                    else
                    {
                        steDetails.SiteAddressSmall = steDetails.SiteAddress;
                    }



                    if (steDetails.AssetStatus == 0)
                    {
                        steDetails.AssetStatusName = "Available";
                    }
                    else
                    {
                        steDetails.AssetStatusName = "Sold";
                    }
                    surplusPropertiesList.Add(steDetails);
                }
                con.Close();
            }

            return View(surplusPropertiesList);
        }

        public IActionResult ViewSurplusProperty(int propertyId)
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

            SiteDetails steDetails = new SiteDetails();
            
            List<StateDetails> stateList = new List<StateDetails>();
            List<MarketDetails> marketList = new List<MarketDetails>();
            List<AdditionalFilesViewModel> additionalFiles = new List<AdditionalFilesViewModel>();
            List<LeaseTypeModel> leaseTypeList = GetLeaseTypeList();

            // string CS = ConfigurationManager.ConnectionStrings["testConnection"].ConnectionString;
            string CS = DBConnection.ConnectionString;

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

                con.Open();
                SqlCommand cmdMarket = new SqlCommand("GetMarketList", con);
                cmdMarket.CommandType = CommandType.StoredProcedure;


                SqlDataReader readerMarket = cmdMarket.ExecuteReader();
                while (readerMarket.Read())
                {
                    var marketDetails = new MarketDetails();
                    marketDetails.MarketId = readerMarket.IsDBNull(readerMarket.GetOrdinal("market_id")) ? 0 : readerMarket.GetInt32(readerMarket.GetOrdinal("market_id"));
                    marketDetails.MarketName = readerMarket.IsDBNull(readerMarket.GetOrdinal("market_name")) ? "" : readerMarket.GetString(readerMarket.GetOrdinal("market_name"));

                    marketList.Add(marketDetails);
                }

                con.Close();

                SqlCommand cmdComplianceList = new SqlCommand("GetSurplusFiles", con);

                cmdComplianceList.Parameters.AddWithValue("property_id", propertyId);
                cmdComplianceList.CommandType = CommandType.StoredProcedure;
                con.Open();

                SqlDataReader readerComplianceList = cmdComplianceList.ExecuteReader();

                while (readerComplianceList.Read())
                {
                    var surplusFile = new AdditionalFilesViewModel();
                    surplusFile.FileId = readerComplianceList.IsDBNull(readerComplianceList.GetOrdinal("file_id")) ? 0 : readerComplianceList.GetInt32(readerComplianceList.GetOrdinal("file_id"));
                    surplusFile.PropertyId = propertyId;
                    surplusFile.FileType = readerComplianceList.IsDBNull(readerComplianceList.GetOrdinal("file_type")) ? "" : readerComplianceList.GetString(readerComplianceList.GetOrdinal("file_type"));


                    surplusFile.FileName = readerComplianceList.IsDBNull(readerComplianceList.GetOrdinal("file_name")) ? "" : readerComplianceList.GetString(readerComplianceList.GetOrdinal("file_name"));
                    surplusFile.FileNameWithoutPath = surplusFile.FileName.Length < 35 ? surplusFile.FileName : surplusFile.FileName.Substring(0, 34) + "...";

                    string pic = @"../../property_files/" + surplusFile.FileName;
                    surplusFile.FileName = pic;

                    additionalFiles.Add(surplusFile);
                }
                con.Close();
            }


            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("GetPropertyItemById", con);

                cmd.Parameters.AddWithValue("site_details_id", propertyId);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {

                    steDetails.SiteDetailsId = reader.IsDBNull(reader.GetOrdinal("site_details_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("site_details_id"));
                    steDetails.NamePrefix = reader.IsDBNull(reader.GetOrdinal("name_prefix")) ? "" : reader.GetString(reader.GetOrdinal("name_prefix"));
                    steDetails.FirstName = reader.IsDBNull(reader.GetOrdinal("first_name")) ? "" : reader.GetString(reader.GetOrdinal("first_name"));

                    steDetails.LastName = reader.IsDBNull(reader.GetOrdinal("last_name")) ? "" : reader.GetString(reader.GetOrdinal("last_name"));
                    steDetails.CompanyName = reader.IsDBNull(reader.GetOrdinal("company_name")) ? "" : reader.GetString(reader.GetOrdinal("company_name"));
                    steDetails.EmailAddress = reader.IsDBNull(reader.GetOrdinal("email_address")) ? "" : reader.GetString(reader.GetOrdinal("email_address"));
                    steDetails.Address = reader.IsDBNull(reader.GetOrdinal("address")) ? "" : reader.GetString(reader.GetOrdinal("address"));
                    steDetails.CityName = reader.IsDBNull(reader.GetOrdinal("city_name")) ? "" : reader.GetString(reader.GetOrdinal("city_name"));
                    steDetails.StateId = reader.IsDBNull(reader.GetOrdinal("state_id")) ? "" : reader.GetString(reader.GetOrdinal("state_id"));
                    steDetails.ZipCode = reader.IsDBNull(reader.GetOrdinal("zip_code")) ? "" : reader.GetString(reader.GetOrdinal("zip_code"));
                    steDetails.ContactNumber = reader.IsDBNull(reader.GetOrdinal("contact_number")) ? "" : reader.GetString(reader.GetOrdinal("contact_number"));
                    steDetails.SamsHoldingEmployee = reader.IsDBNull(reader.GetOrdinal("sams_holding_employee")) ? false : reader.GetBoolean(reader.GetOrdinal("sams_holding_employee"));
                    steDetails.MarketId = reader.IsDBNull(reader.GetOrdinal("market_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("market_id"));
                    steDetails.PropertyHeader = reader.IsDBNull(reader.GetOrdinal("property_header")) ? "" : reader.GetString(reader.GetOrdinal("property_header"));
                    steDetails.SiteAddress = reader.IsDBNull(reader.GetOrdinal("site_address")) ? "" : reader.GetString(reader.GetOrdinal("site_address"));
                    steDetails.SiteCity = reader.IsDBNull(reader.GetOrdinal("site_city")) ? "" : reader.GetString(reader.GetOrdinal("site_city"));
                    steDetails.SiteStateId = reader.IsDBNull(reader.GetOrdinal("site_state_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("site_state_id"));
                    steDetails.SiteCounty = reader.IsDBNull(reader.GetOrdinal("site_county")) ? "" : reader.GetString(reader.GetOrdinal("site_county"));
                    steDetails.SiteCrossStreetName = reader.IsDBNull(reader.GetOrdinal("site_cross_street_name")) ? "" : reader.GetString(reader.GetOrdinal("site_cross_street_name"));
                    steDetails.IsPropertyAvailable = reader.IsDBNull(reader.GetOrdinal("is_property_available")) ? true : reader.GetBoolean(reader.GetOrdinal("is_property_available"));
                    steDetails.Zoning = reader.IsDBNull(reader.GetOrdinal("zoning")) ? "" : reader.GetString(reader.GetOrdinal("zoning"));
                    steDetails.LotSize = reader.IsDBNull(reader.GetOrdinal("lot_size")) ? "" : reader.GetString(reader.GetOrdinal("lot_size"));

                    steDetails.SalesPrice = reader.IsDBNull(reader.GetOrdinal("sales_price")) ? "" : reader.GetString(reader.GetOrdinal("sales_price"));
                    //steDetails.SalesPrice = Helper.FormatCurrency("$", steDetails.SalesPrice);

                    steDetails.Comments = reader.IsDBNull(reader.GetOrdinal("comments")) ? "" : reader.GetString(reader.GetOrdinal("comments"));

                    steDetails.CreatedDate = reader.IsDBNull(reader.GetOrdinal("created_date")) ? DateTime.Now : reader.GetDateTime(reader.GetOrdinal("created_date"));
                    steDetails.PropertyType = reader.IsDBNull(reader.GetOrdinal("property_type")) ? 0 : reader.GetInt32(reader.GetOrdinal("property_type"));

                    steDetails.ImageName = reader.IsDBNull(reader.GetOrdinal("image_name")) ? "" : reader.GetString(reader.GetOrdinal("image_name"));

                    steDetails.StateName = reader.IsDBNull(reader.GetOrdinal("state_name")) ? "" : reader.GetString(reader.GetOrdinal("state_name"));
                    steDetails.SiteStateName = reader.IsDBNull(reader.GetOrdinal("site_state_name")) ? "" : reader.GetString(reader.GetOrdinal("site_state_name"));

                    steDetails.AssetTypeId = reader.IsDBNull(reader.GetOrdinal("asset_type_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("asset_type_id"));
                    steDetails.AssetTypeName = reader.IsDBNull(reader.GetOrdinal("asset_type_name")) ? "" : reader.GetString(reader.GetOrdinal("asset_type_name"));

                    steDetails.AssetStatus = reader.IsDBNull(reader.GetOrdinal("asset_status")) ? 0 : reader.GetInt32(reader.GetOrdinal("asset_status"));

                    steDetails.AssetId = reader.IsDBNull(reader.GetOrdinal("asset_id")) ? "" : reader.GetString(reader.GetOrdinal("asset_id"));

                    steDetails.DiligenceType = reader.IsDBNull(reader.GetOrdinal("diligence_type")) ? 0 : reader.GetInt32(reader.GetOrdinal("diligence_type"));

                    if (steDetails.AssetStatus == 0)
                    {
                        steDetails.AssetStatusName = "Available";
                    }
                    else
                    {
                        steDetails.AssetStatusName = "Sold";
                    }

                    steDetails.Latitude = reader.IsDBNull(reader.GetOrdinal("property_latitude")) ? "" : reader.GetString(reader.GetOrdinal("property_latitude"));
                    steDetails.Longitude = reader.IsDBNull(reader.GetOrdinal("property_longitude")) ? "" : reader.GetString(reader.GetOrdinal("property_longitude"));

                    steDetails.SelectedPropertyStatusId = reader.IsDBNull(reader.GetOrdinal("property_status_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("property_status_id"));
                    steDetails.SelectedPropertyStatus = reader.IsDBNull(reader.GetOrdinal("property_status")) ? "" : reader.GetString(reader.GetOrdinal("property_status"));

                    steDetails.CheckIfPropertyListed = reader.IsDBNull(reader.GetOrdinal("check_if_property_listed")) ? 0 : reader.GetInt32(reader.GetOrdinal("check_if_property_listed"));
                    steDetails.ListingAgentName = reader.IsDBNull(reader.GetOrdinal("listing_agent_name")) ? "" : reader.GetString(reader.GetOrdinal("listing_agent_name"));
                    steDetails.ListingExpiry = reader.IsDBNull(reader.GetOrdinal("listing_expiry")) ? default(DateTime?) : reader.GetDateTime(reader.GetOrdinal("listing_expiry"));
                    steDetails.ListingPrice = reader.IsDBNull(reader.GetOrdinal("listing_price")) ? "" : reader.GetString(reader.GetOrdinal("listing_price"));
                    
                    //steDetails.ListingPrice = Helper.FormatCurrency("$", steDetails.ListingPrice);

                    steDetails.Term = reader.IsDBNull(reader.GetOrdinal("term")) ? "" : reader.GetString(reader.GetOrdinal("term"));
                    steDetails.AskingRent = reader.IsDBNull(reader.GetOrdinal("asking_rent")) ? "" : reader.GetString(reader.GetOrdinal("asking_rent"));
                    //steDetails.AskingRent = Helper.FormatCurrency("$", steDetails.AskingRent);

                    steDetails.LeaseType = reader.IsDBNull(reader.GetOrdinal("lease_type")) ? 0 : reader.GetInt32(reader.GetOrdinal("lease_type"));
                    //steDetails.ClosedDate = reader.IsDBNull(reader.GetOrdinal("closed_date")) ? DateTime.Now : reader.GetDateTime(reader.GetOrdinal("closed_date"));
                    steDetails.StatusChangedDate = reader.IsDBNull(reader.GetOrdinal("status_changed_date")) ? default(DateTime?) : reader.GetDateTime(reader.GetOrdinal("status_changed_date"));
                    steDetails.IsClosed = reader.IsDBNull(reader.GetOrdinal("is_closed")) ? 0 : reader.GetInt32(reader.GetOrdinal("is_closed"));
                    steDetails.ShowInListing = reader.IsDBNull(reader.GetOrdinal("can_publish")) ? false : reader.GetBoolean(reader.GetOrdinal("can_publish"));

                    steDetails.TermOptionPurchase = reader.IsDBNull(reader.GetOrdinal("term_option_purchase")) ? "" : reader.GetString(reader.GetOrdinal("term_option_purchase"));
                    steDetails.AskingRentOptionPurchase = reader.IsDBNull(reader.GetOrdinal("asking_rent_option_purchase")) ? "" : reader.GetString(reader.GetOrdinal("asking_rent_option_purchase"));
                    steDetails.LeaseTypePurchase = reader.IsDBNull(reader.GetOrdinal("lease_type_purchase")) ? 0 : reader.GetInt32(reader.GetOrdinal("lease_type_purchase"));
                    steDetails.OptionPurchasePrice = reader.IsDBNull(reader.GetOrdinal("option_purchase_price")) ? "" : reader.GetString(reader.GetOrdinal("option_purchase_price"));
                    steDetails.PotentialUse = reader.IsDBNull(reader.GetOrdinal("potential_use")) ? "" : reader.GetString(reader.GetOrdinal("potential_use"));

                    steDetails.RegionId = reader.IsDBNull(reader.GetOrdinal("region_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("region_id"));
                    steDetails.RegionName = reader.IsDBNull(reader.GetOrdinal("region_name")) ? "" : reader.GetString(reader.GetOrdinal("region_name"));

                    steDetails.PropertyHeaderLine2 = reader.IsDBNull(reader.GetOrdinal("property_header_line_2")) ? "" : reader.GetString(reader.GetOrdinal("property_header_line_2"));

                }
                con.Close();

                steDetails.StateList = stateList;
                steDetails.MarketList = marketList;

                SqlCommand cmdImageList = new SqlCommand("GetPropertyImageList", con);

                cmdImageList.Parameters.AddWithValue("property_id", propertyId);
                cmdImageList.Parameters.AddWithValue("property_type", SamsPropertyType.Surplus); 
                cmdImageList.CommandType = CommandType.StoredProcedure;
                con.Open();

                SqlDataReader readerMarket = cmdImageList.ExecuteReader();
                List<ImageViewModel> propertyImageList = new List<ImageViewModel>();
                while (readerMarket.Read())
                {
                    var imageItem = new ImageViewModel();
                    imageItem.ImageId = readerMarket.IsDBNull(readerMarket.GetOrdinal("image_id")) ? 0 : readerMarket.GetInt32(readerMarket.GetOrdinal("image_id"));
                    imageItem.PropertyId = propertyId;

                    

                    imageItem.ImageName = readerMarket.IsDBNull(readerMarket.GetOrdinal("image_name")) ? "" : readerMarket.GetString(readerMarket.GetOrdinal("image_name"));
                    string pic = @"../../UploadedImage/" + imageItem.ImageName;
                    imageItem.ImageName = pic;
                    propertyImageList.Add(imageItem);
                }
                con.Close();
                steDetails.PropertyImageList = propertyImageList;
                steDetails.AdditionalFiles = additionalFiles;
            }

            steDetails.TodoList = GetTodoList(steDetails.SiteDetailsId);

            steDetails.DiligenceAcquisitions = GetDiligenceAcquisition(propertyId);
            steDetails.DiligenceDispositions = GetDiligenceDispositions(propertyId);
            steDetails.DiligenceLease = GetDiligenceLease(propertyId);
            steDetails.DiligenceLeaseList = GetDiligenceLeaseList(propertyId);
            steDetails.DiligenceDispositions_SaleLeaseBack = GetDiligenceDispositions_SaleLeaseBack(propertyId);

            //steDetails.DiligenceLeaseWithPurchaseList = GetDiligenceLeaseWithPurchaseList(propertyId);

            steDetails.DispositionPeriodList = GetPeriodList(propertyId, "Disposition");
            steDetails.LeasePeriodList = GetPeriodList(propertyId, "Lease");
            steDetails.LeaseTypeList = leaseTypeList;

            steDetails.CanAddTransactions = true;
            steDetails.IsTransactionClosed = false;
            steDetails.MaxPriorityTransactionStatusId = 0;
            steDetails.MaxPriorityTransactionStatusName = "";

            if (steDetails.AssetTypeId == (int)SamAssetType.Fee || steDetails.AssetTypeId == (int)SamAssetType.FeeSubjectToLease)
            {
                foreach (DiligenceDispositionsViewModel ddv in steDetails.DiligenceDispositions)
                {

                    if (ddv.SelectedTransactionStatusId == (int)TransactionStatus.Closed_Acquisitions || ddv.SelectedTransactionStatusId == (int)TransactionStatus.Under_Contract || ddv.SelectedTransactionStatusId == (int)TransactionStatus.Under_LOI)
                    {
                        steDetails.CanAddTransactions = false;
                    }

                    if (ddv.SelectedTransactionStatusId == (int)TransactionStatus.Closed_Acquisitions)
                    {
                        steDetails.IsTransactionClosed = true;
                    }

                    if (steDetails.MaxPriorityTransactionStatusId == 0)
                    {
                        steDetails.MaxPriorityTransactionStatusId = ddv.SelectedTransactionStatusId;
                        steDetails.MaxPriorityTransactionStatusName = ddv.SelectedTransactionStatusName;
                    }



                    if ((ddv.SelectedTransactionStatusId == (int)TransactionStatus.LOI_Received ||
                        ddv.SelectedTransactionStatusId == (int)TransactionStatus.Terminated_Acquisitions) &&
                        (ddv.SelectedTransactionStatusId != (int)SamsTransactionStatus.Under_LOI &&
                        steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Under_Contract &&
                        steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Closed_Dispositions
                        ))
                    {
                        steDetails.MaxPriorityTransactionStatusId = ddv.SelectedTransactionStatusId;
                        steDetails.MaxPriorityTransactionStatusName = ddv.SelectedTransactionStatusName;
                    }

                    if (ddv.SelectedTransactionStatusId == (int)SamsTransactionStatus.Under_LOI &&
                        (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Under_Contract) &&
                        (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Closed_Dispositions))
                    {
                        steDetails.MaxPriorityTransactionStatusId = ddv.SelectedTransactionStatusId;
                        steDetails.MaxPriorityTransactionStatusName = ddv.SelectedTransactionStatusName;
                    }

                    if (ddv.SelectedTransactionStatusId == (int)SamsTransactionStatus.Under_Contract &&
                        (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Closed_Dispositions))
                    {
                        steDetails.MaxPriorityTransactionStatusId = ddv.SelectedTransactionStatusId;
                        steDetails.MaxPriorityTransactionStatusName = ddv.SelectedTransactionStatusName;
                    }

                    if (ddv.SelectedTransactionStatusId == (int)SamsTransactionStatus.Closed_Dispositions)
                    {
                        steDetails.MaxPriorityTransactionStatusId = ddv.SelectedTransactionStatusId;
                        steDetails.MaxPriorityTransactionStatusName = ddv.SelectedTransactionStatusName;
                    }

                }
            }
            else if (steDetails.AssetTypeId == (int)SamAssetType.Lease)
            {
                steDetails.DiligenceLeaseList = GetDiligenceLeaseList(steDetails.SiteDetailsId);
                int leaseLoi = 0, leaseUnderContract = 0, leaseTerminated = 0, leaseClosed = 0;
                foreach (DiligenceLeaseViewModel dl in steDetails.DiligenceLeaseList)
                {
                    if (dl.SelectedTransactionStatusId == (int)TransactionStatus.Closed_Acquisitions || dl.SelectedTransactionStatusId == (int)TransactionStatus.Under_Contract|| dl.SelectedTransactionStatusId == (int)TransactionStatus.Under_LOI)
                    {
                        steDetails.CanAddTransactions = false;
                    }

                    if (dl.SelectedTransactionStatusId == (int)TransactionStatus.Under_LOI)
                    {
                        leaseLoi = leaseLoi + 1;
                    }
                    else if (dl.SelectedTransactionStatusId == (int)TransactionStatus.Under_Contract)
                    {
                        leaseUnderContract = leaseUnderContract + 1;
                    }
                    else if (dl.SelectedTransactionStatusId == (int)TransactionStatus.Terminated_Acquisitions)
                    {
                        leaseTerminated = leaseTerminated + 1;
                    }
                    else if (dl.SelectedTransactionStatusId == (int)TransactionStatus.Closed_Acquisitions)
                    {
                        leaseClosed = leaseClosed + 1;
                    }

                    if (steDetails.MaxPriorityTransactionStatusId == 0)
                    {
                        steDetails.MaxPriorityTransactionStatusId = dl.SelectedTransactionStatusId;
                        steDetails.MaxPriorityTransactionStatusName = dl.SelectedTransactionStatusName;
                    }



                    if ((dl.SelectedTransactionStatusId == (int)TransactionStatus.LOI_Received ||
                        dl.SelectedTransactionStatusId == (int)TransactionStatus.Terminated_Acquisitions) &&
                        (dl.SelectedTransactionStatusId != (int)SamsTransactionStatus.Under_LOI &&
                        steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Under_Contract &&
                        steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Closed_Dispositions
                        ))
                    {
                        steDetails.MaxPriorityTransactionStatusId = dl.SelectedTransactionStatusId;
                        steDetails.MaxPriorityTransactionStatusName = dl.SelectedTransactionStatusName;
                    }

                    if (dl.SelectedTransactionStatusId == (int)SamsTransactionStatus.Under_LOI &&
                        (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Under_Contract) &&
                        (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Closed_Dispositions))
                    {
                        steDetails.MaxPriorityTransactionStatusId = dl.SelectedTransactionStatusId;
                        steDetails.MaxPriorityTransactionStatusName = dl.SelectedTransactionStatusName;
                    }

                    if (dl.SelectedTransactionStatusId == (int)SamsTransactionStatus.Under_Contract &&
                        (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Closed_Dispositions))
                    {
                        steDetails.MaxPriorityTransactionStatusId = dl.SelectedTransactionStatusId;
                        steDetails.MaxPriorityTransactionStatusName = dl.SelectedTransactionStatusName;
                    }

                    if (dl.SelectedTransactionStatusId == (int)SamsTransactionStatus.Closed_Dispositions)
                    {
                        steDetails.MaxPriorityTransactionStatusId = dl.SelectedTransactionStatusId;
                        steDetails.MaxPriorityTransactionStatusName = dl.SelectedTransactionStatusName;
                    }
                }
                string leaseData = "";
                if (leaseLoi > 0)
                {
                    leaseData = leaseData + "Under LOI : " + leaseLoi + "; ";
                }

                if (leaseUnderContract > 0)
                {
                    leaseData = leaseData + "Under Contract : " + leaseUnderContract + "; ";
                }
                if (leaseTerminated > 0)
                {
                    leaseData = leaseData + "Terminated : " + leaseTerminated + "; ";
                }
                if (leaseClosed > 0)
                {
                    leaseData = leaseData + "Closed : " + leaseClosed + "; ";
                }

                if (leaseData.Trim().Length > 0)
                {
                    leaseData = " Lease : " + leaseData;
                }
            }
            else if (steDetails.AssetTypeId == (int)SamAssetType.LeaseWithPurchaseOption)
            {
                steDetails.DiligenceLeaseWithPurchaseList = GetDiligenceLeaseWithPurchaseList(steDetails.SiteDetailsId);
                int leaseLoi = 0, leaseUnderContract = 0, leaseTerminated = 0, leaseClosed = 0;
                foreach (DiligenceLeaseWithPurchaseViewModel dl in steDetails.DiligenceLeaseWithPurchaseList)
                {
                    if (dl.SelectedTransactionStatusId == (int)TransactionStatus.Closed_Acquisitions || dl.SelectedTransactionStatusId == (int)TransactionStatus.Under_Contract || dl.SelectedTransactionStatusId == (int)TransactionStatus.Under_LOI)
                    {
                        steDetails.CanAddTransactions = false;
                    }

                    if (dl.SelectedTransactionStatusId == (int)TransactionStatus.Under_LOI)
                    {
                        leaseLoi = leaseLoi + 1;
                    }
                    else if (dl.SelectedTransactionStatusId == (int)TransactionStatus.Under_Contract)
                    {
                        leaseUnderContract = leaseUnderContract + 1;
                    }
                    else if (dl.SelectedTransactionStatusId == (int)TransactionStatus.Terminated_Acquisitions)
                    {
                        leaseTerminated = leaseTerminated + 1;
                    }
                    else if (dl.SelectedTransactionStatusId == (int)TransactionStatus.Closed_Acquisitions)
                    {
                        leaseClosed = leaseClosed + 1;
                    }

                    if (steDetails.MaxPriorityTransactionStatusId == 0)
                    {
                        steDetails.MaxPriorityTransactionStatusId = dl.SelectedTransactionStatusId;
                        steDetails.MaxPriorityTransactionStatusName = dl.SelectedTransactionStatusName;
                    }



                    if ((dl.SelectedTransactionStatusId == (int)TransactionStatus.LOI_Received ||
                        dl.SelectedTransactionStatusId == (int)TransactionStatus.Terminated_Acquisitions) &&
                        (dl.SelectedTransactionStatusId != (int)SamsTransactionStatus.Under_LOI &&
                        steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Under_Contract &&
                        steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Closed_Dispositions
                        ))
                    {
                        steDetails.MaxPriorityTransactionStatusId = dl.SelectedTransactionStatusId;
                        steDetails.MaxPriorityTransactionStatusName = dl.SelectedTransactionStatusName;
                    }

                    if (dl.SelectedTransactionStatusId == (int)SamsTransactionStatus.Under_LOI &&
                        (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Under_Contract) &&
                        (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Closed_Dispositions))
                    {
                        steDetails.MaxPriorityTransactionStatusId = dl.SelectedTransactionStatusId;
                        steDetails.MaxPriorityTransactionStatusName = dl.SelectedTransactionStatusName;
                    }

                    if (dl.SelectedTransactionStatusId == (int)SamsTransactionStatus.Under_Contract &&
                        (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Closed_Dispositions))
                    {
                        steDetails.MaxPriorityTransactionStatusId = dl.SelectedTransactionStatusId;
                        steDetails.MaxPriorityTransactionStatusName = dl.SelectedTransactionStatusName;
                    }

                    if (dl.SelectedTransactionStatusId == (int)SamsTransactionStatus.Closed_Dispositions)
                    {
                        steDetails.MaxPriorityTransactionStatusId = dl.SelectedTransactionStatusId;
                        steDetails.MaxPriorityTransactionStatusName = dl.SelectedTransactionStatusName;
                    }
                }
                string leaseData = "";
                if (leaseLoi > 0)
                {
                    leaseData = leaseData + "Under LOI : " + leaseLoi + "; ";
                }

                if (leaseUnderContract > 0)
                {
                    leaseData = leaseData + "Under Contract : " + leaseUnderContract + "; ";
                }
                if (leaseTerminated > 0)
                {
                    leaseData = leaseData + "Terminated : " + leaseTerminated + "; ";
                }
                if (leaseClosed > 0)
                {
                    leaseData = leaseData + "Closed : " + leaseClosed + "; ";
                }

                if (leaseData.Trim().Length > 0)
                {
                    leaseData = " Lease : " + leaseData;
                }
            }
            else if (steDetails.AssetTypeId == (int)SamAssetType.SaleLeaseBack)
            {
                foreach (DiligenceDispositionsViewModel ddv in steDetails.DiligenceDispositions_SaleLeaseBack)
                {

                    if (ddv.SelectedTransactionStatusId == (int)TransactionStatus.Closed_Acquisitions || ddv.SelectedTransactionStatusId == (int)TransactionStatus.Under_Contract || ddv.SelectedTransactionStatusId == (int)TransactionStatus.Under_LOI)
                    {
                        steDetails.CanAddTransactions = false;
                    }

                    if (ddv.SelectedTransactionStatusId == (int)TransactionStatus.Closed_Acquisitions)
                    {
                        steDetails.IsTransactionClosed = true;
                    }

                    if (steDetails.MaxPriorityTransactionStatusId == 0)
                    {
                        steDetails.MaxPriorityTransactionStatusId = ddv.SelectedTransactionStatusId;
                        steDetails.MaxPriorityTransactionStatusName = ddv.SelectedTransactionStatusName;
                    }



                    if ((ddv.SelectedTransactionStatusId == (int)TransactionStatus.LOI_Received ||
                        ddv.SelectedTransactionStatusId == (int)TransactionStatus.Terminated_Acquisitions) &&
                        (ddv.SelectedTransactionStatusId != (int)SamsTransactionStatus.Under_LOI &&
                        steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Under_Contract &&
                        steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Closed_Dispositions
                        ))
                    {
                        steDetails.MaxPriorityTransactionStatusId = ddv.SelectedTransactionStatusId;
                        steDetails.MaxPriorityTransactionStatusName = ddv.SelectedTransactionStatusName;
                    }

                    if (ddv.SelectedTransactionStatusId == (int)SamsTransactionStatus.Under_LOI &&
                        (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Under_Contract) &&
                        (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Closed_Dispositions))
                    {
                        steDetails.MaxPriorityTransactionStatusId = ddv.SelectedTransactionStatusId;
                        steDetails.MaxPriorityTransactionStatusName = ddv.SelectedTransactionStatusName;
                    }

                    if (ddv.SelectedTransactionStatusId == (int)SamsTransactionStatus.Under_Contract &&
                        (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Closed_Dispositions))
                    {
                        steDetails.MaxPriorityTransactionStatusId = ddv.SelectedTransactionStatusId;
                        steDetails.MaxPriorityTransactionStatusName = ddv.SelectedTransactionStatusName;
                    }

                    if (ddv.SelectedTransactionStatusId == (int)SamsTransactionStatus.Closed_Dispositions)
                    {
                        steDetails.MaxPriorityTransactionStatusId = ddv.SelectedTransactionStatusId;
                        steDetails.MaxPriorityTransactionStatusName = ddv.SelectedTransactionStatusName;
                    }

                }
            }

            /*
            if (steDetails.MaxPriorityTransactionStatusId == (int)TransactionStatus.Closed_Acquisitions || steDetails.MaxPriorityTransactionStatusId == (int)TransactionStatus.Under_Contract || steDetails.MaxPriorityTransactionStatusId == (int)TransactionStatus.Under_LOI)
            {
                steDetails.CanAddTransactions = false;
            }
            else
            {
                steDetails.CanAddTransactions = true;
            }
            */

            steDetails.PropertyHistoryList = PropertyHistory.GetPropertyHistoryList(steDetails.SiteDetailsId);
            return View(steDetails);
        }

        DiligenceLeaseViewModel GetDiligenceLease(int propertyId)
        {
            var diligenceLease = new DiligenceLeaseViewModel();

            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("GetDiligenceLease", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("property_id", propertyId);
                cmd.Parameters.AddWithValue("property_type", SamsPropertyType.Surplus);
                con.Open();

                diligenceLease.PropertyId = propertyId;
                diligenceLease.PropertyType = 1;

                SqlDataReader readerAssetType = cmd.ExecuteReader();
                while (readerAssetType.Read())
                {
                    diligenceLease.DiligenceLeaseId = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("diligence_lease_id")) ? 0 : readerAssetType.GetInt32(readerAssetType.GetOrdinal("diligence_lease_id"));
                    diligenceLease.Tenant = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("tenant_name")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("tenant_name"));


                    diligenceLease.Rent = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("rent")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("rent"));
                    diligenceLease.UnderContractDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("under_contract_date")) ? default(DateTime?) : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("under_contract_date"));
                    diligenceLease.DueDiligenceExpiryDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("due_diligence_expiry_date")) ? default(DateTime?) : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("due_diligence_expiry_date"));
                    diligenceLease.EarnestMoneyDeposit = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("earnest_money_deposit")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("earnest_money_deposit"));
                    diligenceLease.DDPExtension = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("ddp_extension")) ? default(DateTime?) : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("ddp_extension"));

                    diligenceLease.TenantAttorney = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("tenant_attorney")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("tenant_attorney"));
                    diligenceLease.TenantAgentCommission = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("tenant_agent_commission")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("tenant_agent_commission"));
                    diligenceLease.LandlordAgentCommission = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("land_lord_agent_commission")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("land_lord_agent_commission"));
                    diligenceLease.LeaseSecurityDeposit = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("lease_security_deposit")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("lease_security_deposit"));
                    

                    diligenceLease.CreatedDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("created_date")) ? DateTime.Now : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("created_date"));

                    diligenceLease.ClosingDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("closing_date")) ? default(DateTime?) : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("closing_date"));

                }

                con.Close();

            }
            ViewData["propertyId"] = propertyId;
            return diligenceLease;
        }

        List<DiligenceLeaseViewModel> GetDiligenceLeaseList(int propertyId)
        {
            var diligenceLeaseList = new List<DiligenceLeaseViewModel>();

            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("GetDiligenceLease", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("property_id", propertyId);
                cmd.Parameters.AddWithValue("property_type", SamsPropertyType.Surplus);
                con.Open();

                

                SqlDataReader readerAssetType = cmd.ExecuteReader();
                while (readerAssetType.Read())
                {
                    var diligenceLease = new DiligenceLeaseViewModel();

                    diligenceLease.PropertyId = propertyId;
                    diligenceLease.PropertyType = 1;

                    diligenceLease.DiligenceLeaseId = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("diligence_lease_id")) ? 0 : readerAssetType.GetInt32(readerAssetType.GetOrdinal("diligence_lease_id"));
                    diligenceLease.Tenant = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("tenant_name")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("tenant_name"));


                    diligenceLease.Rent = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("rent")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("rent"));
                    diligenceLease.UnderContractDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("under_contract_date")) ? default(DateTime?) : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("under_contract_date"));
                    diligenceLease.DueDiligenceExpiryDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("due_diligence_expiry_date")) ? default(DateTime?) : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("due_diligence_expiry_date"));
                    diligenceLease.EarnestMoneyDeposit = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("earnest_money_deposit")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("earnest_money_deposit"));
                    diligenceLease.DDPExtension = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("ddp_extension")) ? default(DateTime?) : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("ddp_extension"));

                    diligenceLease.TenantAttorney = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("tenant_attorney")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("tenant_attorney"));
                    diligenceLease.TenantAgentCommission = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("tenant_agent_commission")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("tenant_agent_commission"));
                    diligenceLease.LandlordAgentCommission = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("land_lord_agent_commission")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("land_lord_agent_commission"));
                    diligenceLease.LeaseSecurityDeposit = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("lease_security_deposit")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("lease_security_deposit"));


                    diligenceLease.CreatedDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("created_date")) ? DateTime.Now : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("created_date"));

                    diligenceLease.DispositionTerminatedStatus = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("disposition_terminated_status")) ? 0 : readerAssetType.GetInt32(readerAssetType.GetOrdinal("disposition_terminated_status"));
                    diligenceLease.DispositionTerminatedDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("disposition_terminated_date")) ? default(DateTime?) : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("disposition_terminated_date"));
                    diligenceLease.DispositionClosedStatus = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("disposition_closed_status")) ? 0 : readerAssetType.GetInt32(readerAssetType.GetOrdinal("disposition_closed_status"));
                    diligenceLease.DispositionClosedDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("disposition_closed_date")) ? default(DateTime?) : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("disposition_closed_date"));


                    diligenceLease.SelectedTransactionStatusId = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("selected_transaction_id")) ? 0 : readerAssetType.GetInt32(readerAssetType.GetOrdinal("selected_transaction_id"));
                    diligenceLease.SelectedTransactionStatusName = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("transaction_status_name")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("transaction_status_name"));
                    diligenceLease.SelectedTransactionDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("selected_transaction_date")) ? DateTime.Now : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("selected_transaction_date"));

                    diligenceLease.ClosingDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("closing_date")) ? default(DateTime?) : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("closing_date"));
                    diligenceLeaseList.Add(diligenceLease);
                }

                con.Close();

            }

            ViewData["propertyId"] = propertyId;
            return diligenceLeaseList;
        }


        public IActionResult GetDiligenceLeaseById(int diligenceLeaseId, int propertyId, int currentAssetStatusId, int assetTypeId)
        {
            var diligenceLease = new DiligenceLeaseViewModel();
            diligenceLease.PropertyId = propertyId;
            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("GetDiligenceLeaseById", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("property_id", propertyId);
                cmd.Parameters.AddWithValue("property_type", SamsPropertyType.Surplus);
                cmd.Parameters.AddWithValue("diligence_lease_id", diligenceLeaseId);
                con.Open();

                bool haveRecords = false;
                
                SqlDataReader readerAssetType = cmd.ExecuteReader();
                while (readerAssetType.Read())
                {

                    haveRecords = true;
                    diligenceLease.PropertyId = propertyId;
                    diligenceLease.PropertyType = 1;

                    diligenceLease.DiligenceLeaseId = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("diligence_lease_id")) ? 0 : readerAssetType.GetInt32(readerAssetType.GetOrdinal("diligence_lease_id"));
                    diligenceLease.Tenant = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("tenant_name")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("tenant_name"));


                    diligenceLease.Rent = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("rent")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("rent"));
                    diligenceLease.UnderContractDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("under_contract_date")) ? default(DateTime?) : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("under_contract_date"));
                    diligenceLease.DueDiligenceExpiryDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("due_diligence_expiry_date")) ? default(DateTime?) : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("due_diligence_expiry_date"));
                    diligenceLease.EarnestMoneyDeposit = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("earnest_money_deposit")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("earnest_money_deposit"));
                    diligenceLease.DDPExtension = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("ddp_extension")) ? default(DateTime?) : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("ddp_extension"));

                    diligenceLease.TenantAttorney = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("tenant_attorney")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("tenant_attorney"));
                    diligenceLease.TenantAgentCommission = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("tenant_agent_commission")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("tenant_agent_commission"));
                    diligenceLease.LandlordAgentCommission = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("land_lord_agent_commission")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("land_lord_agent_commission"));
                    diligenceLease.LeaseSecurityDeposit = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("lease_security_deposit")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("lease_security_deposit"));

                    diligenceLease.DispositionTerminatedStatus = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("disposition_terminated_status")) ? 0 : readerAssetType.GetInt32(readerAssetType.GetOrdinal("disposition_terminated_status"));
                    diligenceLease.DispositionTerminatedDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("disposition_terminated_date")) ? default(DateTime?) : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("disposition_terminated_date"));
                    diligenceLease.DispositionClosedStatus = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("disposition_closed_status")) ? 0 : readerAssetType.GetInt32(readerAssetType.GetOrdinal("disposition_closed_status"));
                    diligenceLease.DispositionClosedDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("disposition_closed_date")) ? default(DateTime?) : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("disposition_closed_date"));

                    diligenceLease.SelectedTransactionStatusId = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("selected_transaction_id")) ? 0 : readerAssetType.GetInt32(readerAssetType.GetOrdinal("selected_transaction_id"));
                    diligenceLease.SelectedTransactionDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("selected_transaction_date")) ? default(DateTime?) : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("selected_transaction_date"));

                    diligenceLease.CreatedDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("created_date")) ? DateTime.Now : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("created_date"));
                    diligenceLease.LeaseCommencementDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("lease_commencement_date")) ? default(DateTime?) : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("lease_commencement_date"));
                    diligenceLease.ClosingDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("closing_date")) ? default(DateTime?) : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("closing_date"));
                    
                }

                if (!haveRecords)
                {
                    //diligenceLease.SelectedTransactionDate = DateTime.Now;
                }

                con.Close();

                diligenceLease.TransactionFileList = new List<TransactionFilesViewModel>();
                if (diligenceLeaseId > 0)
                {
                    SqlCommand cmdGetTransactionFiles = new SqlCommand("getTransactionFiles", con);
                    cmdGetTransactionFiles.CommandType = CommandType.StoredProcedure;
                    cmdGetTransactionFiles.Parameters.AddWithValue("transaction_id", diligenceLeaseId);
                    cmdGetTransactionFiles.Parameters.AddWithValue("transaction_type", TransactionType.Lease);
                    con.Open();

                    SqlDataReader readerGetTransactionFiles = cmdGetTransactionFiles.ExecuteReader();
                    while (readerGetTransactionFiles.Read())
                    {
                        TransactionFilesViewModel transactionFiles = new TransactionFilesViewModel();
                        transactionFiles.TransactionFilesId = readerGetTransactionFiles.IsDBNull(readerGetTransactionFiles.GetOrdinal("transaction_files_id")) ? 0 : readerGetTransactionFiles.GetInt32(readerGetTransactionFiles.GetOrdinal("transaction_files_id"));
                        transactionFiles.TransactionId = readerGetTransactionFiles.IsDBNull(readerGetTransactionFiles.GetOrdinal("transaction_id")) ? 0 : readerGetTransactionFiles.GetInt32(readerGetTransactionFiles.GetOrdinal("transaction_id"));
                        transactionFiles.FileHeader = readerGetTransactionFiles.IsDBNull(readerGetTransactionFiles.GetOrdinal("file_header")) ? "" : readerGetTransactionFiles.GetString(readerGetTransactionFiles.GetOrdinal("file_header"));
                        transactionFiles.FileName = readerGetTransactionFiles.IsDBNull(readerGetTransactionFiles.GetOrdinal("file_name")) ? "" : readerGetTransactionFiles.GetString(readerGetTransactionFiles.GetOrdinal("file_name"));
                        
                        transactionFiles.FileFullName = readerGetTransactionFiles.IsDBNull(readerGetTransactionFiles.GetOrdinal("file_full_path")) ? "" : readerGetTransactionFiles.GetString(readerGetTransactionFiles.GetOrdinal("file_full_path"));
                        
                        transactionFiles.Notes = readerGetTransactionFiles.IsDBNull(readerGetTransactionFiles.GetOrdinal("notes")) ? "" : readerGetTransactionFiles.GetString(readerGetTransactionFiles.GetOrdinal("notes"));
                        transactionFiles.UploadedDate = readerGetTransactionFiles.IsDBNull(readerGetTransactionFiles.GetOrdinal("UploadedDate")) ? DateTime.Now : readerGetTransactionFiles.GetDateTime(readerGetTransactionFiles.GetOrdinal("UploadedDate"));
                        transactionFiles.UploadedByName = readerGetTransactionFiles.IsDBNull(readerGetTransactionFiles.GetOrdinal("FullName")) ? "" : readerGetTransactionFiles.GetString(readerGetTransactionFiles.GetOrdinal("FullName"));

                        diligenceLease.TransactionFileList.Add(transactionFiles);
                    }

                    con.Close();

                }


                var periodList = new List<PeriodViewModel>();

                SqlCommand cmdPeriod = new SqlCommand("GetPeriodList", con);
                cmdPeriod.CommandType = CommandType.StoredProcedure;
                cmdPeriod.Parameters.AddWithValue("property_id", propertyId);
                cmdPeriod.Parameters.AddWithValue("property_type", (int)SamsPropertyType.Surplus);
                cmdPeriod.Parameters.AddWithValue("transaction_id", diligenceLeaseId);
                cmdPeriod.Parameters.AddWithValue("period_type", "Lease");
                con.Open();

                SqlDataReader readerPeriod = cmdPeriod.ExecuteReader();
                while (readerPeriod.Read())
                {
                    var periodView = new PeriodViewModel();

                    periodView.PeriodId = readerPeriod.IsDBNull(readerPeriod.GetOrdinal("period_id")) ? 0 : readerPeriod.GetInt32(readerPeriod.GetOrdinal("period_id"));
                    periodView.PropertyId = readerPeriod.IsDBNull(readerPeriod.GetOrdinal("property_id")) ? 0 : readerPeriod.GetInt32(readerPeriod.GetOrdinal("property_id"));
                    periodView.PropertyType = readerPeriod.IsDBNull(readerPeriod.GetOrdinal("property_type")) ? 0 : readerPeriod.GetInt32(readerPeriod.GetOrdinal("property_type"));

                    periodView.PeriodMaster = readerPeriod.IsDBNull(readerPeriod.GetOrdinal("period_master")) ? "" : readerPeriod.GetString(readerPeriod.GetOrdinal("period_master"));

                    periodView.StartDate = readerPeriod.IsDBNull(readerPeriod.GetOrdinal("start_date")) ? DateTime.Now : readerPeriod.GetDateTime(readerPeriod.GetOrdinal("start_date"));
                    periodView.EndDate = readerPeriod.IsDBNull(readerPeriod.GetOrdinal("end_date")) ? DateTime.Now : readerPeriod.GetDateTime(readerPeriod.GetOrdinal("end_date"));


                    periodView.PeriodNotes = readerPeriod.IsDBNull(readerPeriod.GetOrdinal("period_notes")) ? "" : readerPeriod.GetString(readerPeriod.GetOrdinal("period_notes"));
                    periodView.PeriodType = readerPeriod.IsDBNull(readerPeriod.GetOrdinal("period_type")) ? "" : readerPeriod.GetString(readerPeriod.GetOrdinal("period_type"));

                    periodView.AlertDate = readerPeriod.IsDBNull(readerPeriod.GetOrdinal("alert_date")) ? default(DateTime?) : readerPeriod.GetDateTime(readerPeriod.GetOrdinal("alert_date"));
                    periodView.OtherEmailAddress = readerPeriod.IsDBNull(readerPeriod.GetOrdinal("other_email_address")) ? "" : readerPeriod.GetString(readerPeriod.GetOrdinal("other_email_address"));

                    periodList.Add(periodView);
                }

                con.Close();
                diligenceLease.DispositionCriticalItems = periodList;
            }

            diligenceLease.TransactionStatusList = GetTransactionStatusList(currentAssetStatusId, diligenceLease.SelectedTransactionStatusId);
            ViewData["propertyId"] = propertyId;
            ViewData["currentAssetStatusId"] = currentAssetStatusId;
            ViewData["assetTypeId"] = assetTypeId; 

            return View(diligenceLease);
        }

        [HttpPost]
        public IActionResult SaveDiligenceLease(DiligenceLeaseViewModel diligenceLease)
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
                SqlCommand cmd = new SqlCommand("SaveDiligenceLease", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("diligence_lease_id", diligenceLease.DiligenceLeaseId);

                cmd.Parameters.AddWithValue("property_id", diligenceLease.PropertyId);
                cmd.Parameters.AddWithValue("property_type", SamsPropertyType.Surplus);
                cmd.Parameters.AddWithValue("tenant_name", diligenceLease.Tenant);
                cmd.Parameters.AddWithValue("rent", diligenceLease.Rent);

                cmd.Parameters.AddWithValue("under_contract_date", diligenceLease.UnderContractDate);
                cmd.Parameters.AddWithValue("due_diligence_expiry_date", diligenceLease.DueDiligenceExpiryDate);
                cmd.Parameters.AddWithValue("earnest_money_deposit", diligenceLease.EarnestMoneyDeposit);
                cmd.Parameters.AddWithValue("ddp_extension", diligenceLease.DDPExtension);
                
                cmd.Parameters.AddWithValue("tenant_attorney", diligenceLease.TenantAttorney);
                cmd.Parameters.AddWithValue("tenant_agent_commission", diligenceLease.TenantAgentCommission);
                cmd.Parameters.AddWithValue("land_lord_agent_commission", diligenceLease.LandlordAgentCommission);
                cmd.Parameters.AddWithValue("lease_security_deposit", diligenceLease.LeaseSecurityDeposit);

                cmd.Parameters.AddWithValue("disposition_terminated_status", diligenceLease.DispositionTerminatedStatus);
                cmd.Parameters.AddWithValue("disposition_terminated_date", diligenceLease.DispositionTerminatedDate);
                cmd.Parameters.AddWithValue("disposition_closed_status", diligenceLease.DispositionClosedStatus);
                cmd.Parameters.AddWithValue("disposition_closed_date", diligenceLease.DispositionClosedDate);

                cmd.Parameters.AddWithValue("selected_transaction_id", diligenceLease.SelectedTransactionStatusId);
                cmd.Parameters.AddWithValue("selected_transaction_date", diligenceLease.SelectedTransactionDate);
                cmd.Parameters.AddWithValue("lease_commencement_date", diligenceLease.LeaseCommencementDate);
                cmd.Parameters.AddWithValue("closing_date", diligenceLease.ClosingDate);

                con.Open();


                diligenceLease.DiligenceLeaseId = int.Parse(cmd.ExecuteScalar().ToString());


                con.Close();

                
                PropertyHistoryModel propertyHistory = new PropertyHistoryModel();
                propertyHistory.PropertyId = diligenceLease.PropertyId;
                propertyHistory.StatusId = diligenceLease.SelectedTransactionStatusId;
                propertyHistory.Description = diligenceLease.TransactionDescription;
                propertyHistory.LoggedInId = loggedInUser.UserId;
                propertyHistory.TransactionId = diligenceLease.DiligenceLeaseId;

                PropertyHistory.SavePropertyHistory(propertyHistory);

            }

            return RedirectToAction("ViewSurplusProperty", new { propertyId = diligenceLease.PropertyId });
        }


        List<PeriodViewModel> GetPeriodList(int propertyId, string periodType)
        {
            var periodList = new List<PeriodViewModel>();

            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("GetPeriodList", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("property_id", propertyId);
                cmd.Parameters.AddWithValue("property_type", SamsPropertyType.Surplus);
                cmd.Parameters.AddWithValue("period_type", periodType); 
                con.Open();

                SqlDataReader readerAssetType = cmd.ExecuteReader();
                while (readerAssetType.Read())
                {
                    var periodView = new PeriodViewModel();

                    periodView.PeriodId = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("period_id")) ? 0 : readerAssetType.GetInt32(readerAssetType.GetOrdinal("period_id"));
                    periodView.PropertyId = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("property_id")) ? 0 : readerAssetType.GetInt32(readerAssetType.GetOrdinal("property_id"));
                    periodView.PropertyType = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("property_type")) ? 0 : readerAssetType.GetInt32(readerAssetType.GetOrdinal("property_type"));

                    periodView.PeriodMaster = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("period_master")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("period_master"));

                    periodView.StartDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("start_date")) ? DateTime.Now : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("start_date"));
                    periodView.EndDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("end_date")) ? DateTime.Now : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("end_date"));

                    
                    periodView.PeriodNotes = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("period_notes")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("period_notes"));
                    periodView.PeriodType = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("period_type")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("period_type"));
                    periodList.Add(periodView);
                }

                con.Close();

            }

            return periodList;
        }

        List<LeaseTypeModel> GetLeaseTypeList()
        {
            var LeaseTypeList = new List<LeaseTypeModel>();

            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("GetLeaseTypeList", con);
                cmd.CommandType = CommandType.StoredProcedure;
                
                con.Open();

                SqlDataReader readerAssetType = cmd.ExecuteReader();
                while (readerAssetType.Read())
                {
                    var leaseType = new LeaseTypeModel();

                    leaseType.LeaseTypeId = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("lease_type_id")) ? 0 : readerAssetType.GetInt32(readerAssetType.GetOrdinal("lease_type_id"));
                    leaseType.LeaseType = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("lease_type")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("lease_type"));
                    LeaseTypeList.Add(leaseType);
                }

                con.Close();

            }

            return LeaseTypeList;
        }

        [HttpPost]
        public IActionResult SavePeriod(PeriodViewModel period)
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
                SqlCommand cmd = new SqlCommand("SavePeriod", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("period_id", period.PeriodId);

                cmd.Parameters.AddWithValue("property_id", period.PropertyId);
                cmd.Parameters.AddWithValue("property_type", SamsPropertyType.Surplus);
                cmd.Parameters.AddWithValue("period_master", period.PeriodMaster);

                cmd.Parameters.AddWithValue("start_date", period.StartDate);

                DateTime endDate = period.StartDate.AddDays(period.AddedDuration);

                //cmd.Parameters.AddWithValue("end_date", period.EndDate);
                cmd.Parameters.AddWithValue("end_date", endDate);

                cmd.Parameters.AddWithValue("period_notes", period.PeriodNotes);
                cmd.Parameters.AddWithValue("period_type", period.PeriodType);
                cmd.Parameters.AddWithValue("transaction_id", period.TransactionId);

                cmd.Parameters.AddWithValue("alert_date", period.AlertDate);
                cmd.Parameters.AddWithValue("other_email_address", period.OtherEmailAddress);

                con.Open();


                period.PeriodId = int.Parse(cmd.ExecuteScalar().ToString());


                con.Close();

            }
            //GetDispositionCriticalItems int diligenceDispositionsId, int propertyId
            if (period.PeriodType == "Disposition")
            {
                //return RedirectToAction("GetDispositionCriticalItems", new { diligenceDispositionsId = period.TransactionId, propertyId = period.PropertyId });
                return RedirectToAction("GetDiligenceDispositionById", new { diligenceDispositionId = period.TransactionId, propertyId = period.PropertyId, currentAssetStatusId = period.CurrentAssetStatusId });
            }
            else if (period.PeriodType == "Lease")
            {
                //return RedirectToAction("GetLeaseCriticalItems", new { diligenceLeaseId = period.TransactionId, propertyId = period.PropertyId });
                return RedirectToAction("GetDiligenceLeaseById", new { diligenceLeaseId = period.TransactionId, propertyId = period.PropertyId, currentAssetStatusId = period.CurrentAssetStatusId});
            }
            else if(period.PeriodType== "PurchaseLeaseBack")
            {
                return RedirectToAction("GetDiligenceSaleLeaseBackById", new { saleLeaseBackId = period.TransactionId, propertyId = period.PropertyId, currentAssetStatusId = period.CurrentAssetStatusId });
            }
            else
            {
                //return RedirectToAction("GetLeaseCriticalItems", new { diligenceLeaseId = period.TransactionId, propertyId = period.PropertyId });
                return RedirectToAction("GetDiligenceLeaseWithPurchaseById", new { diligenceLeaseWithPurchaseId = period.TransactionId, propertyId = period.PropertyId, currentAssetStatusId = period.CurrentAssetStatusId });
            }

        }

        public IActionResult DeletePeriod(int periodId, int propertyId)
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

            var periodList = new List<PeriodViewModel>();

            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("DeletePeriod", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("period_id", periodId);

                con.Open();

                cmd.ExecuteReader();
                

                con.Close();

            }

            return RedirectToAction("ViewSurplusProperty", new { propertyId = propertyId });
        }


        DiligenceAcquisitionViewModel GetDiligenceAcquisition(int propertyId)
        {
            var diligenceAcquisition = new DiligenceAcquisitionViewModel();

            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("GetDiligenceAcquisition", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("property_id", propertyId);
                cmd.Parameters.AddWithValue("property_type", SamsPropertyType.Surplus);
                con.Open();

                diligenceAcquisition.PropertyId = propertyId;
                diligenceAcquisition.PropertyType = 1;

                SqlDataReader readerAssetType = cmd.ExecuteReader();
                while (readerAssetType.Read())
                {
                    diligenceAcquisition.DiligenceAcquisitionId = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("diligence_acquisition_id")) ? 0 : readerAssetType.GetInt32(readerAssetType.GetOrdinal("diligence_acquisition_id"));
                    

                    diligenceAcquisition.PurchasePrice = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("purchase_price")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("purchase_price"));
                    diligenceAcquisition.EarnestMoney = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("earnest_money")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("earnest_money"));
                    //diligenceAcquisition.EarnestMoney = Helper.FormatCurrency("$", diligenceAcquisition.EarnestMoney);

                    diligenceAcquisition.Exchage1031 = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("exchange_1031")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("exchange_1031"));
                    diligenceAcquisition.Deadline1031 = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("dead_line_1031")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("dead_line_1031"));

                    diligenceAcquisition.Sellers = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("sellers")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("sellers"));
                    diligenceAcquisition.EscrowAgent = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("escrow_agent")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("escrow_agent"));
                    diligenceAcquisition.SubDivision = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("sub_division")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("sub_division"));diligenceAcquisition.Deadline1031 = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("dead_line_1031")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("dead_line_1031"));
                    diligenceAcquisition.RealEstateAgent = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("real_estate_agent")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("real_estate_agent"));

                    diligenceAcquisition.CreatedDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("created_date")) ? DateTime.Now : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("created_date"));
                    diligenceAcquisition.AcquisitionStatus = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("acquisition_status")) ? 0 : readerAssetType.GetInt32(readerAssetType.GetOrdinal("acquisition_status"));

                    diligenceAcquisition.ClosedDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("closed_date")) ? DateTime.Now : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("closed_date"));
                    diligenceAcquisition.TerminatedDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("terminated_date")) ? DateTime.Now : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("terminated_date"));

                    diligenceAcquisition.ClosingDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("closing_date")) ? default(DateTime?) : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("closing_date"));
                }

                con.Close();
                
            }

            return diligenceAcquisition;
        }


        List<DiligenceDispositionsViewModel> GetDiligenceDispositions(int propertyId)
        {
            var diligenceDispositions = new List<DiligenceDispositionsViewModel>();

            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("GetDiligenceDispositions", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("property_id", propertyId);
                cmd.Parameters.AddWithValue("property_type", SamsPropertyType.Surplus);
                con.Open();

                

                SqlDataReader readerAssetType = cmd.ExecuteReader();
                while (readerAssetType.Read())
                {
                    var ddpViewModel = new DiligenceDispositionsViewModel();

                    ddpViewModel.PropertyId = propertyId;
                    ddpViewModel.PropertyType = (int)SamsPropertyType.Surplus;
                    ddpViewModel.DiligenceDispositionsId = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("diligence_dispositions_id")) ? 0 : readerAssetType.GetInt32(readerAssetType.GetOrdinal("diligence_dispositions_id"));

                    ddpViewModel.SalePrice = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("sale_price")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("sale_price"));
                    ddpViewModel.EarnestMoney = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("earnest_money")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("earnest_money"));
                    //ddpViewModel.EarnestMoney = Helper.FormatCurrency("$", ddpViewModel.EarnestMoney);

                    ddpViewModel.Buyers = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("buyers")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("buyers"));
                    ddpViewModel.EscrowAgent = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("escrow_agent")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("escrow_agent"));

                    ddpViewModel.BuyersAttorney = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("buyers_attorney")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("buyers_attorney"));
                    ddpViewModel.OptionsToExtend = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("options_to_extend")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("options_to_extend"));
                    ddpViewModel.Commissions = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("commissions")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("commissions"));

                    ddpViewModel.CreatedDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("created_date")) ? DateTime.Now : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("created_date"));
                    ddpViewModel.DispositionStatus = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("disposition_status")) ? 0 : readerAssetType.GetInt32(readerAssetType.GetOrdinal("disposition_status"));

                    ddpViewModel.ClosedDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("closed_date")) ? DateTime.Now : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("closed_date"));
                    ddpViewModel.TerminatedDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("terminated_date")) ? DateTime.Now : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("terminated_date"));

                    ddpViewModel.UnderContractDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("under_contract_date")) ? default(DateTime?) : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("under_contract_date"));
                    ddpViewModel.DueDiligenceExpairyDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("due_diligence_expairy_date")) ? DateTime.Now : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("due_diligence_expairy_date"));
                    
                    ddpViewModel.DueDiligenceAmount = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("due_diligence_amount")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("due_diligence_amount"));
                    ddpViewModel.EMD = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("emd")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("emd"));
                    ddpViewModel.DDPExtension = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("ddp_extension")) ? default(DateTime?) : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("ddp_extension"));
                    ddpViewModel.CreatedDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("created_date")) ? DateTime.Now : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("created_date"));
                    

                    ddpViewModel.UnderContractDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("under_contract_date")) ? default(DateTime?) : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("under_contract_date"));
                    ddpViewModel.DueDiligenceExpairyDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("due_diligence_expairy_date")) ? default(DateTime?) : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("due_diligence_expairy_date"));

                    ddpViewModel.DueDiligenceAmount = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("due_diligence_amount")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("due_diligence_amount"));
                    ddpViewModel.EMD = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("emd")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("emd"));
                    //ddpViewModel.EMD = Helper.FormatCurrency("$", ddpViewModel.EMD);
                    ddpViewModel.DDPExtension = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("ddp_extension")) ? default(DateTime?) : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("ddp_extension"));
                    ddpViewModel.DDPExtensionOpted = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("dueDiligenceApplicableStatus")) ? 0 : readerAssetType.GetInt32(readerAssetType.GetOrdinal("dueDiligenceApplicableStatus"));

                    ddpViewModel.SellersAttorney = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("sellersAttorney")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("sellersAttorney"));
                    ddpViewModel.BuyersAgentCommission = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("buyers_agent_commision")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("buyers_agent_commision"));
                    ddpViewModel.SellersAgentCommission = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("sellers_agent_commision")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("sellers_agent_commision"));

                    ddpViewModel.DispositionTerminatedStatus = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("disposition_terminated_status")) ? 0 : readerAssetType.GetInt32(readerAssetType.GetOrdinal("disposition_terminated_status"));
                    ddpViewModel.DispositionTerminatedDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("disposition_terminated_date")) ? default(DateTime?) : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("disposition_terminated_date"));
                    ddpViewModel.DispositionClosedStatus = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("disposition_closed_status")) ? 0 : readerAssetType.GetInt32(readerAssetType.GetOrdinal("disposition_closed_status"));
                    ddpViewModel.DispositionClosedDate= readerAssetType.IsDBNull(readerAssetType.GetOrdinal("disposition_closed_date")) ? default(DateTime?) : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("disposition_closed_date"));

                    ddpViewModel.SelectedTransactionStatusId = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("selected_transaction_id")) ? 0 : readerAssetType.GetInt32(readerAssetType.GetOrdinal("selected_transaction_id"));
                    ddpViewModel.SelectedTransactionStatusName = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("transaction_status_name")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("transaction_status_name"));
                    ddpViewModel.SelectedTransactionDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("selected_transaction_date")) ? DateTime.Now : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("selected_transaction_date"));

                    ddpViewModel.ClosingDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("closing_date")) ? default(DateTime?) : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("closing_date"));

                    diligenceDispositions.Add(ddpViewModel);
                }

                con.Close();

            }

            return diligenceDispositions;
        }


        public IActionResult GetDiligenceDispositionById(int diligenceDispositionId, int propertyId, int currentAssetStatusId)
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

            var ddpViewModel = new DiligenceDispositionsViewModel();
            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("GetDiligenceDispositionById", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("diligence_dispositions_id", diligenceDispositionId);
                con.Open();

                ddpViewModel.PropertyType = (int)SamsPropertyType.Surplus;
                bool haveRecords = false;

                SqlDataReader readerAssetType = cmd.ExecuteReader();
                while (readerAssetType.Read())
                {

                    haveRecords = true;
                    ddpViewModel.PropertyId = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("property_id")) ? 0 : readerAssetType.GetInt32(readerAssetType.GetOrdinal("property_id")); ;

                    ddpViewModel.DiligenceDispositionsId = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("diligence_dispositions_id")) ? 0 : readerAssetType.GetInt32(readerAssetType.GetOrdinal("diligence_dispositions_id"));

                    ddpViewModel.SalePrice = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("sale_price")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("sale_price"));
                    ddpViewModel.EarnestMoney = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("earnest_money")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("earnest_money"));
                    //ddpViewModel.EarnestMoney = Helper.FormatCurrency("$", ddpViewModel.EarnestMoney);

                    ddpViewModel.Buyers = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("buyers")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("buyers"));
                    ddpViewModel.EscrowAgent = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("escrow_agent")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("escrow_agent"));

                    ddpViewModel.BuyersAttorney = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("buyers_attorney")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("buyers_attorney"));
                    ddpViewModel.OptionsToExtend = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("options_to_extend")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("options_to_extend"));
                    ddpViewModel.Commissions = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("commissions")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("commissions"));

                    ddpViewModel.CreatedDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("created_date")) ? DateTime.Now : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("created_date"));
                    ddpViewModel.DispositionStatus = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("disposition_status")) ? 0 : readerAssetType.GetInt32(readerAssetType.GetOrdinal("disposition_status"));

                    ddpViewModel.ClosedDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("closed_date")) ? DateTime.Now : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("closed_date"));
                    ddpViewModel.TerminatedDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("terminated_date")) ? DateTime.Now : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("terminated_date"));

                    ddpViewModel.UnderContractDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("under_contract_date")) ? default(DateTime?) : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("under_contract_date"));
                    ddpViewModel.DueDiligenceExpairyDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("due_diligence_expairy_date")) ? DateTime.Now : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("due_diligence_expairy_date"));

                    ddpViewModel.DueDiligenceAmount = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("due_diligence_amount")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("due_diligence_amount"));
                    ddpViewModel.EMD = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("emd")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("emd"));
                    ddpViewModel.DDPExtension = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("ddp_extension")) ? default(DateTime?) : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("ddp_extension"));
                    ddpViewModel.CreatedDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("created_date")) ? DateTime.Now : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("created_date"));

                    ddpViewModel.UnderContractDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("under_contract_date")) ? default(DateTime?) : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("under_contract_date"));
                    ddpViewModel.DueDiligenceExpairyDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("due_diligence_expairy_date")) ? default(DateTime?) : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("due_diligence_expairy_date"));

                    ddpViewModel.DueDiligenceAmount = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("due_diligence_amount")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("due_diligence_amount"));
                    ddpViewModel.EMD = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("emd")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("emd"));
                    //ddpViewModel.EMD = Helper.FormatCurrency("$", ddpViewModel.EMD);
                    ddpViewModel.DDPExtension = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("ddp_extension")) ? default(DateTime?) : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("ddp_extension"));
                    ddpViewModel.DDPExtensionOpted = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("dueDiligenceApplicableStatus")) ? 0 : readerAssetType.GetInt32(readerAssetType.GetOrdinal("dueDiligenceApplicableStatus"));

                    ddpViewModel.SellersAttorney = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("sellersAttorney")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("sellersAttorney"));
                    ddpViewModel.BuyersAgentCommission = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("buyers_agent_commision")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("buyers_agent_commision"));
                    ddpViewModel.SellersAgentCommission = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("sellers_agent_commision")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("sellers_agent_commision"));

                    ddpViewModel.DispositionTerminatedStatus = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("disposition_terminated_status")) ? 0 : readerAssetType.GetInt32(readerAssetType.GetOrdinal("disposition_terminated_status"));
                    ddpViewModel.DispositionTerminatedDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("disposition_terminated_date")) ? default(DateTime?) : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("disposition_terminated_date"));
                    ddpViewModel.DispositionClosedStatus = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("disposition_closed_status")) ? 0 : readerAssetType.GetInt32(readerAssetType.GetOrdinal("disposition_closed_status"));
                    ddpViewModel.DispositionClosedDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("disposition_closed_date")) ? default(DateTime?) : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("disposition_closed_date"));

                    ddpViewModel.SelectedTransactionStatusId = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("selected_transaction_id")) ? 0 : readerAssetType.GetInt32(readerAssetType.GetOrdinal("selected_transaction_id"));
                    ddpViewModel.SelectedTransactionDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("selected_transaction_date")) ? default(DateTime?) : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("selected_transaction_date"));

                    ddpViewModel.PermittingPeriod = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("permitting_period")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("permitting_period"));
                    ddpViewModel.ClosingDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("closing_date")) ? default(DateTime?) : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("closing_date"));
                }



                if (!haveRecords)
                {
                    //ddpViewModel.SelectedTransactionDate = DateTime.Now;
                }
                con.Close();

                ddpViewModel.TransactionFileList = new List<TransactionFilesViewModel>();
                if (diligenceDispositionId > 0)
                {
                    SqlCommand cmdGetTransactionFiles = new SqlCommand("getTransactionFiles", con);
                    cmdGetTransactionFiles.CommandType = CommandType.StoredProcedure;
                    cmdGetTransactionFiles.Parameters.AddWithValue("transaction_id", diligenceDispositionId);
                    cmdGetTransactionFiles.Parameters.AddWithValue("transaction_type", TransactionType.Sale);
                    con.Open();

                    SqlDataReader readerGetTransactionFiles = cmdGetTransactionFiles.ExecuteReader();
                    while (readerGetTransactionFiles.Read())
                    {
                        TransactionFilesViewModel transactionFiles = new TransactionFilesViewModel();
                        transactionFiles.TransactionFilesId = readerGetTransactionFiles.IsDBNull(readerGetTransactionFiles.GetOrdinal("transaction_files_id")) ? 0 : readerGetTransactionFiles.GetInt32(readerGetTransactionFiles.GetOrdinal("transaction_files_id"));
                        transactionFiles.TransactionId = readerGetTransactionFiles.IsDBNull(readerGetTransactionFiles.GetOrdinal("transaction_id")) ? 0 : readerGetTransactionFiles.GetInt32(readerGetTransactionFiles.GetOrdinal("transaction_id"));
                        transactionFiles.FileHeader = readerGetTransactionFiles.IsDBNull(readerGetTransactionFiles.GetOrdinal("file_header")) ? "" : readerGetTransactionFiles.GetString(readerGetTransactionFiles.GetOrdinal("file_header"));
                        transactionFiles.FileName = readerGetTransactionFiles.IsDBNull(readerGetTransactionFiles.GetOrdinal("file_name")) ? "" : readerGetTransactionFiles.GetString(readerGetTransactionFiles.GetOrdinal("file_name"));

                        transactionFiles.FileFullName = readerGetTransactionFiles.IsDBNull(readerGetTransactionFiles.GetOrdinal("file_full_path")) ? "" : readerGetTransactionFiles.GetString(readerGetTransactionFiles.GetOrdinal("file_full_path"));

                        transactionFiles.Notes = readerGetTransactionFiles.IsDBNull(readerGetTransactionFiles.GetOrdinal("notes")) ? "" : readerGetTransactionFiles.GetString(readerGetTransactionFiles.GetOrdinal("notes"));
                        transactionFiles.UploadedDate = readerGetTransactionFiles.IsDBNull(readerGetTransactionFiles.GetOrdinal("UploadedDate")) ? DateTime.Now : readerGetTransactionFiles.GetDateTime(readerGetTransactionFiles.GetOrdinal("UploadedDate"));
                        transactionFiles.UploadedByName = readerGetTransactionFiles.IsDBNull(readerGetTransactionFiles.GetOrdinal("FullName")) ? "" : readerGetTransactionFiles.GetString(readerGetTransactionFiles.GetOrdinal("FullName"));

                        ddpViewModel.TransactionFileList.Add(transactionFiles);
                    }

                    con.Close();
                }



                var periodList = new List<PeriodViewModel>();

                SqlCommand cmdPeriod = new SqlCommand("GetPeriodList", con);
                cmdPeriod.CommandType = CommandType.StoredProcedure;
                cmdPeriod.Parameters.AddWithValue("property_id", propertyId);
                cmdPeriod.Parameters.AddWithValue("property_type", (int)SamsPropertyType.Surplus);
                cmdPeriod.Parameters.AddWithValue("transaction_id", ddpViewModel.DiligenceDispositionsId);
                cmdPeriod.Parameters.AddWithValue("period_type", "Disposition");
                con.Open();

                SqlDataReader readerPeriod = cmdPeriod.ExecuteReader();
                while (readerPeriod.Read())
                {
                    var periodView = new PeriodViewModel();

                    periodView.PeriodId = readerPeriod.IsDBNull(readerPeriod.GetOrdinal("period_id")) ? 0 : readerPeriod.GetInt32(readerPeriod.GetOrdinal("period_id"));
                    periodView.PropertyId = readerPeriod.IsDBNull(readerPeriod.GetOrdinal("property_id")) ? 0 : readerPeriod.GetInt32(readerPeriod.GetOrdinal("property_id"));
                    periodView.PropertyType = readerPeriod.IsDBNull(readerPeriod.GetOrdinal("property_type")) ? 0 : readerPeriod.GetInt32(readerPeriod.GetOrdinal("property_type"));

                    periodView.PeriodMaster = readerPeriod.IsDBNull(readerPeriod.GetOrdinal("period_master")) ? "" : readerPeriod.GetString(readerPeriod.GetOrdinal("period_master"));

                    periodView.StartDate = readerPeriod.IsDBNull(readerPeriod.GetOrdinal("start_date")) ? DateTime.Now : readerPeriod.GetDateTime(readerPeriod.GetOrdinal("start_date"));
                    periodView.EndDate = readerPeriod.IsDBNull(readerPeriod.GetOrdinal("end_date")) ? DateTime.Now : readerPeriod.GetDateTime(readerPeriod.GetOrdinal("end_date"));


                    periodView.PeriodNotes = readerPeriod.IsDBNull(readerPeriod.GetOrdinal("period_notes")) ? "" : readerPeriod.GetString(readerPeriod.GetOrdinal("period_notes"));
                    periodView.PeriodType = readerPeriod.IsDBNull(readerPeriod.GetOrdinal("period_type")) ? "" : readerPeriod.GetString(readerPeriod.GetOrdinal("period_type"));

                    periodView.AlertDate = readerPeriod.IsDBNull(readerPeriod.GetOrdinal("alert_date")) ? default(DateTime?) : readerPeriod.GetDateTime(readerPeriod.GetOrdinal("alert_date"));
                    periodView.OtherEmailAddress = readerPeriod.IsDBNull(readerPeriod.GetOrdinal("other_email_address")) ? "" : readerPeriod.GetString(readerPeriod.GetOrdinal("other_email_address"));

                    periodList.Add(periodView);
                }

                con.Close();
                ddpViewModel.DispositionCriticalItems = periodList;
            }

            ViewData["propertyId"] = propertyId;
            ViewData["currentAssetStatusId"] = currentAssetStatusId;


            ddpViewModel.TransactionStatusList = GetTransactionStatusList(currentAssetStatusId, ddpViewModel.SelectedTransactionStatusId);
            return View(ddpViewModel);
        }

        [HttpPost]
        public IActionResult SaveDiligenceAcquisition(DiligenceAcquisitionViewModel diligenceAcquisition)
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
                SqlCommand cmd = new SqlCommand("SaveDiligenceAcquisition", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("diligence_acquisition_id", diligenceAcquisition.DiligenceAcquisitionId);
                
                cmd.Parameters.AddWithValue("property_id", diligenceAcquisition.PropertyId);
                cmd.Parameters.AddWithValue("property_type", SamsPropertyType.Surplus);
                cmd.Parameters.AddWithValue("purchase_price", diligenceAcquisition.PurchasePrice);
                cmd.Parameters.AddWithValue("earnest_money", diligenceAcquisition.EarnestMoney);
                cmd.Parameters.AddWithValue("exchange_1031", diligenceAcquisition.Exchage1031);
                cmd.Parameters.AddWithValue("dead_line_1031", diligenceAcquisition.Deadline1031);
                cmd.Parameters.AddWithValue("sellers", diligenceAcquisition.Sellers);
                cmd.Parameters.AddWithValue("escrow_agent", diligenceAcquisition.EscrowAgent);
                cmd.Parameters.AddWithValue("sub_division", diligenceAcquisition.SubDivision);
                cmd.Parameters.AddWithValue("real_estate_agent", diligenceAcquisition.RealEstateAgent);

                con.Open();


                diligenceAcquisition.DiligenceAcquisitionId = int.Parse(cmd.ExecuteScalar().ToString());


                con.Close();

                
            }

            return RedirectToAction("ViewSurplusProperty", new { propertyId = diligenceAcquisition.PropertyId });
        }


        [HttpPost]
        public IActionResult SaveDiligenceDispositions(DiligenceDispositionsViewModel diligenceDispositions)
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
                SqlCommand cmd = new SqlCommand("SaveDiligenceDispositions", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("diligence_dispositions_id", diligenceDispositions.DiligenceDispositionsId);

                cmd.Parameters.AddWithValue("property_id", diligenceDispositions.PropertyId);
                cmd.Parameters.AddWithValue("property_type", SamsPropertyType.Surplus);
                cmd.Parameters.AddWithValue("sale_price", diligenceDispositions.SalePrice);
                cmd.Parameters.AddWithValue("earnest_money", diligenceDispositions.EarnestMoney);
                cmd.Parameters.AddWithValue("buyers", diligenceDispositions.Buyers);
                cmd.Parameters.AddWithValue("escrow_agent", diligenceDispositions.EscrowAgent);
                cmd.Parameters.AddWithValue("buyers_attorney", diligenceDispositions.BuyersAttorney);
                cmd.Parameters.AddWithValue("options_to_extend", diligenceDispositions.OptionsToExtend);
                cmd.Parameters.AddWithValue("commissions", diligenceDispositions.Commissions);

                cmd.Parameters.AddWithValue("under_contract_date", diligenceDispositions.UnderContractDate);

                cmd.Parameters.AddWithValue("due_diligence_expairy_date", diligenceDispositions.DueDiligenceExpairyDate);
                cmd.Parameters.AddWithValue("due_diligence_amount", diligenceDispositions.DueDiligenceAmount);
                cmd.Parameters.AddWithValue("emd", diligenceDispositions.EMD);
                
                cmd.Parameters.AddWithValue("ddp_extension", diligenceDispositions.DDPExtension);
                cmd.Parameters.AddWithValue("dueDiligenceApplicableStatus", diligenceDispositions.DDPExtensionOpted);

                cmd.Parameters.AddWithValue("sellersAttorney", diligenceDispositions.SellersAttorney);
                cmd.Parameters.AddWithValue("buyers_agent_commision", diligenceDispositions.BuyersAgentCommission);
                cmd.Parameters.AddWithValue("sellers_agent_commision", diligenceDispositions.SellersAgentCommission);

                cmd.Parameters.AddWithValue("disposition_terminated_status", diligenceDispositions.DispositionTerminatedStatus);
                cmd.Parameters.AddWithValue("disposition_terminated_date", diligenceDispositions.DispositionTerminatedDate);
                cmd.Parameters.AddWithValue("disposition_closed_status", diligenceDispositions.DispositionClosedStatus);
                cmd.Parameters.AddWithValue("disposition_closed_date", diligenceDispositions.DispositionClosedDate);

                cmd.Parameters.AddWithValue("selected_transaction_id", diligenceDispositions.SelectedTransactionStatusId);
                
                cmd.Parameters.AddWithValue("selected_transaction_date", diligenceDispositions.SelectedTransactionDate);
                cmd.Parameters.AddWithValue("permitting_period", diligenceDispositions.PermittingPeriod);
                cmd.Parameters.AddWithValue("closing_date", diligenceDispositions.ClosingDate);

                con.Open();


                diligenceDispositions.DiligenceDispositionsId = int.Parse(cmd.ExecuteScalar().ToString());


                con.Close();

                

                PropertyHistoryModel propertyHistory = new PropertyHistoryModel();
                propertyHistory.PropertyId = diligenceDispositions.PropertyId;
                propertyHistory.StatusId = diligenceDispositions.SelectedTransactionStatusId;
                propertyHistory.Description = diligenceDispositions.TransactionDescription;
                propertyHistory.LoggedInId = loggedInUser.UserId;
                propertyHistory.TransactionId = diligenceDispositions.DiligenceDispositionsId;

                PropertyHistory.SavePropertyHistory(propertyHistory);
                

            }

            return RedirectToAction("ViewSurplusProperty", new { propertyId = diligenceDispositions.PropertyId });
        }


        public IActionResult CloseAcquisition(int diligenceAcquisitionId, int propertyId)
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
                SqlCommand cmd = new SqlCommand("CloseDiligenceAcquisition", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("diligence_acquisition_id", diligenceAcquisitionId);

                con.Open();
                cmd.ExecuteNonQuery();

                con.Close();
            }

            return RedirectToAction("ViewSurplusProperty", new { propertyId = propertyId });
        }

        public IActionResult TerminateAcquisition(int diligenceAcquisitionId, int propertyId)
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
                SqlCommand cmd = new SqlCommand("TerminateDiligenceAcquisition", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("diligence_acquisition_id", diligenceAcquisitionId);

                con.Open();
                cmd.ExecuteNonQuery();

                con.Close();
            }

            return RedirectToAction("ViewSurplusProperty", new { propertyId = propertyId });
        }


        public IActionResult CloseDisposition(int diligenceDispositionsId, int propertyId)
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
                SqlCommand cmd = new SqlCommand("CloseDiligenceDisposition", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("diligence_dispositions_id", diligenceDispositionsId);

                con.Open();
                cmd.ExecuteNonQuery();

                con.Close();
            }

            return RedirectToAction("ViewSurplusProperty", new { propertyId = propertyId });
        }

        public IActionResult TerminateDisposition(int diligenceDispositionsId, int propertyId)
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
                SqlCommand cmd = new SqlCommand("TerminateDiligenceDisposition", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("diligence_dispositions_id", diligenceDispositionsId);

                con.Open();
                cmd.ExecuteNonQuery();

                con.Close();
            }

            return RedirectToAction("ViewSurplusProperty", new { propertyId = propertyId });
        }


        public IActionResult EditSurplusProperty(int propertyId)
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

            SiteDetails steDetails = new SiteDetails();

            List<StateDetails> stateList = new List<StateDetails>();
            List<MarketDetails> marketList = new List<MarketDetails>();
            List<AssetTypeViewModel> assetTypeList = new List<AssetTypeViewModel>();
            List<PropertyStatusModel> propertyStatusList = new List<PropertyStatusModel>();
            List<LeaseTypeModel> leaseTypeList = GetLeaseTypeList();

            
            string CS = DBConnection.ConnectionString;

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

                con.Open();
                SqlCommand cmdMarket = new SqlCommand("GetMarketList", con);
                cmdMarket.CommandType = CommandType.StoredProcedure;


                SqlDataReader readerMarket = cmdMarket.ExecuteReader();
                while (readerMarket.Read())
                {
                    var marketDetails = new MarketDetails();
                    marketDetails.MarketId = readerMarket.IsDBNull(readerMarket.GetOrdinal("market_id")) ? 0 : readerMarket.GetInt32(readerMarket.GetOrdinal("market_id"));
                    marketDetails.MarketName = readerMarket.IsDBNull(readerMarket.GetOrdinal("market_name")) ? "" : readerMarket.GetString(readerMarket.GetOrdinal("market_name"));

                    marketList.Add(marketDetails);
                }

                con.Close();

                SqlCommand cmdAssetType = new SqlCommand("GetAssetType", con);
                cmdAssetType.CommandType = CommandType.StoredProcedure;
                con.Open();

                SqlDataReader readerAssetType = cmdAssetType.ExecuteReader();
                while (readerAssetType.Read())
                {
                    var assetType = new AssetTypeViewModel();
                    assetType.AssetTypeId = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("asset_type_id")) ? 0 : readerAssetType.GetInt32(readerAssetType.GetOrdinal("asset_type_id"));

                    if(assetType.AssetTypeId == 1)
                    {
                        assetType.AssetTypeName = "Lease/Build To Suit";
                    }
                    else
                    {
                        assetType.AssetTypeName = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("asset_type_name")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("asset_type_name"));
                    }
                    
                    assetTypeList.Add(assetType);
                }
                con.Close();

                con.Open();
                SqlCommand cmdPropertyList = new SqlCommand("GetPropertyStatus", con);
                cmdPropertyList.CommandType = CommandType.StoredProcedure;


                SqlDataReader readerPropertyList = cmdPropertyList.ExecuteReader();
                while (readerPropertyList.Read())
                {
                    var propertyStatus = new PropertyStatusModel();
                    propertyStatus.PropertyStatusId = readerPropertyList.IsDBNull(readerPropertyList.GetOrdinal("property_status_id")) ? 0 : readerPropertyList.GetInt32(readerPropertyList.GetOrdinal("property_status_id"));
                    propertyStatus.PropertyStatus = readerPropertyList.IsDBNull(readerPropertyList.GetOrdinal("property_status")) ? "" : readerPropertyList.GetString(readerPropertyList.GetOrdinal("property_status"));

                    propertyStatusList.Add(propertyStatus);
                }

                con.Close();
            }

            
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("GetPropertyItemById", con);

                cmd.Parameters.AddWithValue("site_details_id", propertyId);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {

                    steDetails.SiteDetailsId = reader.IsDBNull(reader.GetOrdinal("site_details_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("site_details_id"));
                    steDetails.NamePrefix = reader.IsDBNull(reader.GetOrdinal("name_prefix")) ? "" : reader.GetString(reader.GetOrdinal("name_prefix"));
                    steDetails.FirstName = reader.IsDBNull(reader.GetOrdinal("first_name")) ? "" : reader.GetString(reader.GetOrdinal("first_name"));

                    steDetails.LastName = reader.IsDBNull(reader.GetOrdinal("last_name")) ? "" : reader.GetString(reader.GetOrdinal("last_name"));
                    steDetails.CompanyName = reader.IsDBNull(reader.GetOrdinal("company_name")) ? "" : reader.GetString(reader.GetOrdinal("company_name"));
                    steDetails.EmailAddress = reader.IsDBNull(reader.GetOrdinal("email_address")) ? "" : reader.GetString(reader.GetOrdinal("email_address"));
                    steDetails.Address = reader.IsDBNull(reader.GetOrdinal("address")) ? "" : reader.GetString(reader.GetOrdinal("address"));
                    steDetails.CityName = reader.IsDBNull(reader.GetOrdinal("city_name")) ? "" : reader.GetString(reader.GetOrdinal("city_name"));
                    steDetails.StateId = reader.IsDBNull(reader.GetOrdinal("state_id")) ? "" : reader.GetString(reader.GetOrdinal("state_id"));
                    steDetails.ZipCode = reader.IsDBNull(reader.GetOrdinal("zip_code")) ? "" : reader.GetString(reader.GetOrdinal("zip_code"));
                    steDetails.ContactNumber = reader.IsDBNull(reader.GetOrdinal("contact_number")) ? "" : reader.GetString(reader.GetOrdinal("contact_number"));
                    steDetails.SamsHoldingEmployee = reader.IsDBNull(reader.GetOrdinal("sams_holding_employee")) ? false : reader.GetBoolean(reader.GetOrdinal("sams_holding_employee"));
                    steDetails.MarketId = reader.IsDBNull(reader.GetOrdinal("market_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("market_id"));
                    steDetails.PropertyHeader = reader.IsDBNull(reader.GetOrdinal("property_header")) ? "" : reader.GetString(reader.GetOrdinal("property_header"));
                    steDetails.SiteAddress = reader.IsDBNull(reader.GetOrdinal("site_address")) ? "" : reader.GetString(reader.GetOrdinal("site_address"));
                    steDetails.SiteCity = reader.IsDBNull(reader.GetOrdinal("site_city")) ? "" : reader.GetString(reader.GetOrdinal("site_city"));
                    steDetails.SiteStateId = reader.IsDBNull(reader.GetOrdinal("site_state_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("site_state_id"));
                    steDetails.SiteCounty = reader.IsDBNull(reader.GetOrdinal("site_county")) ? "" : reader.GetString(reader.GetOrdinal("site_county"));
                    steDetails.SiteCrossStreetName = reader.IsDBNull(reader.GetOrdinal("site_cross_street_name")) ? "" : reader.GetString(reader.GetOrdinal("site_cross_street_name"));
                    steDetails.IsPropertyAvailable = reader.IsDBNull(reader.GetOrdinal("is_property_available")) ? true : reader.GetBoolean(reader.GetOrdinal("is_property_available"));
                    steDetails.Zoning = reader.IsDBNull(reader.GetOrdinal("zoning")) ? "" : reader.GetString(reader.GetOrdinal("zoning"));
                    steDetails.LotSize = reader.IsDBNull(reader.GetOrdinal("lot_size")) ? "" : reader.GetString(reader.GetOrdinal("lot_size"));

                    steDetails.SalesPrice = reader.IsDBNull(reader.GetOrdinal("sales_price")) ? "" : reader.GetString(reader.GetOrdinal("sales_price"));
                    //steDetails.SalesPrice = Helper.FormatCurrency("$", steDetails.SalesPrice);

                    steDetails.Comments = reader.IsDBNull(reader.GetOrdinal("comments")) ? "" : reader.GetString(reader.GetOrdinal("comments"));

                    steDetails.CreatedDate = reader.IsDBNull(reader.GetOrdinal("created_date")) ? DateTime.Now : reader.GetDateTime(reader.GetOrdinal("created_date"));
                    steDetails.PropertyType = reader.IsDBNull(reader.GetOrdinal("property_type")) ? 0 : reader.GetInt32(reader.GetOrdinal("property_type"));

                    steDetails.ImageName = reader.IsDBNull(reader.GetOrdinal("image_name")) ? "" : reader.GetString(reader.GetOrdinal("image_name"));

                    steDetails.AssetTypeId = reader.IsDBNull(reader.GetOrdinal("asset_type_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("asset_type_id"));
                    steDetails.AssetTypeName = reader.IsDBNull(reader.GetOrdinal("asset_type_name")) ? "" : reader.GetString(reader.GetOrdinal("asset_type_name"));

                    steDetails.AssetStatus = reader.IsDBNull(reader.GetOrdinal("asset_status")) ? 0 : reader.GetInt32(reader.GetOrdinal("asset_status"));

                    steDetails.AssetId = reader.IsDBNull(reader.GetOrdinal("asset_id")) ? "" : reader.GetString(reader.GetOrdinal("asset_id"));
                    steDetails.Latitude = reader.IsDBNull(reader.GetOrdinal("property_latitude")) ? "" : reader.GetString(reader.GetOrdinal("property_latitude"));
                    steDetails.Longitude = reader.IsDBNull(reader.GetOrdinal("property_longitude")) ? "" : reader.GetString(reader.GetOrdinal("property_longitude"));

                    if (steDetails.AssetStatus == 0)
                    {
                        steDetails.AssetStatusName = "Available";
                    }
                    else
                    {
                        steDetails.AssetStatusName = "Sold";
                    }

                    steDetails.SelectedPropertyStatusId = reader.IsDBNull(reader.GetOrdinal("property_status_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("property_status_id"));
                    steDetails.SelectedPropertyStatus = reader.IsDBNull(reader.GetOrdinal("property_status")) ? "" : reader.GetString(reader.GetOrdinal("property_status"));

                    steDetails.CheckIfPropertyListed = reader.IsDBNull(reader.GetOrdinal("check_if_property_listed")) ? 0 : reader.GetInt32(reader.GetOrdinal("check_if_property_listed"));
                    steDetails.ListingAgentName = reader.IsDBNull(reader.GetOrdinal("listing_agent_name")) ? "" : reader.GetString(reader.GetOrdinal("listing_agent_name"));
                    steDetails.ListingExpiry = reader.IsDBNull(reader.GetOrdinal("listing_expiry")) ? DateTime.Now : reader.GetDateTime(reader.GetOrdinal("listing_expiry"));
                    steDetails.ListingPrice = reader.IsDBNull(reader.GetOrdinal("listing_price")) ? "" : reader.GetString(reader.GetOrdinal("listing_price"));
                    //steDetails.ListingPrice = Helper.FormatCurrency("$", steDetails.ListingPrice);

                    steDetails.Term = reader.IsDBNull(reader.GetOrdinal("term")) ? "" : reader.GetString(reader.GetOrdinal("term"));
                    steDetails.AskingRent = reader.IsDBNull(reader.GetOrdinal("asking_rent")) ? "" : reader.GetString(reader.GetOrdinal("asking_rent"));
                    //steDetails.AskingRent = Helper.FormatCurrency("$", steDetails.AskingRent);

                    steDetails.LeaseType = reader.IsDBNull(reader.GetOrdinal("lease_type")) ? 0 : reader.GetInt32(reader.GetOrdinal("lease_type"));
                    steDetails.StatusChangedDate = reader.IsDBNull(reader.GetOrdinal("status_changed_date")) ? default(DateTime?) : reader.GetDateTime(reader.GetOrdinal("status_changed_date"));
                    steDetails.IsClosed = reader.IsDBNull(reader.GetOrdinal("is_closed")) ? 0 : reader.GetInt32(reader.GetOrdinal("is_closed"));
                    steDetails.ShowInListing = reader.IsDBNull(reader.GetOrdinal("can_publish")) ? false : reader.GetBoolean(reader.GetOrdinal("can_publish"));

                    steDetails.TermOptionPurchase = reader.IsDBNull(reader.GetOrdinal("term_option_purchase")) ? "" : reader.GetString(reader.GetOrdinal("term_option_purchase"));
                    steDetails.AskingRentOptionPurchase = reader.IsDBNull(reader.GetOrdinal("asking_rent_option_purchase")) ? "" : reader.GetString(reader.GetOrdinal("asking_rent_option_purchase"));
                    steDetails.LeaseTypePurchase = reader.IsDBNull(reader.GetOrdinal("lease_type_purchase")) ? 0 : reader.GetInt32(reader.GetOrdinal("lease_type_purchase"));
                    steDetails.OptionPurchasePrice = reader.IsDBNull(reader.GetOrdinal("option_purchase_price")) ? "" : reader.GetString(reader.GetOrdinal("option_purchase_price"));

                    steDetails.PotentialUse = reader.IsDBNull(reader.GetOrdinal("potential_use")) ? "" : reader.GetString(reader.GetOrdinal("potential_use"));

                    steDetails.RegionId = reader.IsDBNull(reader.GetOrdinal("region_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("region_id"));
                    steDetails.RegionName = reader.IsDBNull(reader.GetOrdinal("region_name")) ? "" : reader.GetString(reader.GetOrdinal("region_name"));

                    steDetails.PropertyHeaderLine2 = reader.IsDBNull(reader.GetOrdinal("property_header_line_2")) ? "" : reader.GetString(reader.GetOrdinal("property_header_line_2"));

                }
                con.Close();

                steDetails.StateList = stateList;
                steDetails.MarketList = marketList;
                steDetails.AssetTypeList = assetTypeList;
                steDetails.propertyStatusList = propertyStatusList;
                steDetails.LeaseTypeList = leaseTypeList;

                steDetails.RegionList = GetRegionList(steDetails.SiteStateId);

                return View(steDetails);
            }

        }

        [HttpPost]
        public int EditSurplusProperty(SiteDetails siteDetails)
        {
            /*
            Request.Files.All
            if (System.Web.HttpContext.Current.Request.Files.AllKeys.Any())
            {

            }
            */

            

                int siteDetailsId = siteDetails.SiteDetailsId;
            string CS = DBConnection.ConnectionString;

            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("SavePropertyAdmin", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                cmd.Parameters.AddWithValue("site_details_id", siteDetails.SiteDetailsId);
                cmd.Parameters.AddWithValue("name_prefix", siteDetails.NamePrefix);
                cmd.Parameters.AddWithValue("first_name", siteDetails.FirstName);
                cmd.Parameters.AddWithValue("last_name", siteDetails.LastName);

                cmd.Parameters.AddWithValue("company_name", siteDetails.CompanyName);
                cmd.Parameters.AddWithValue("email_address", siteDetails.EmailAddress);
                cmd.Parameters.AddWithValue("address", siteDetails.Address);
                cmd.Parameters.AddWithValue("city_name", siteDetails.CityName);

                cmd.Parameters.AddWithValue("state_id", siteDetails.StateId);
                cmd.Parameters.AddWithValue("zip_code", siteDetails.ZipCode);
                cmd.Parameters.AddWithValue("contact_number", siteDetails.ContactNumber);
                cmd.Parameters.AddWithValue("sams_holding_employee", siteDetails.SamsHoldingEmployee);

                cmd.Parameters.AddWithValue("market_id", siteDetails.MarketId);
                cmd.Parameters.AddWithValue("property_header", siteDetails.PropertyHeader);
                cmd.Parameters.AddWithValue("site_address", siteDetails.SiteAddress);
                cmd.Parameters.AddWithValue("site_city", siteDetails.SiteCity);
                cmd.Parameters.AddWithValue("site_state_id", siteDetails.SiteStateId);

                cmd.Parameters.AddWithValue("site_county", siteDetails.SiteCounty);
                cmd.Parameters.AddWithValue("site_cross_street_name", siteDetails.SiteCrossStreetName);
                cmd.Parameters.AddWithValue("is_property_available", siteDetails.IsPropertyAvailable);
                cmd.Parameters.AddWithValue("zoning", siteDetails.Zoning);

                cmd.Parameters.AddWithValue("lot_size", siteDetails.LotSize);
                cmd.Parameters.AddWithValue("sales_price", siteDetails.SalesPrice);
                cmd.Parameters.AddWithValue("comments", siteDetails.Comments);
                cmd.Parameters.AddWithValue("property_type", SamsPropertyType.Surplus);
                cmd.Parameters.AddWithValue("asset_type_id", siteDetails.AssetTypeId);
                cmd.Parameters.AddWithValue("asset_status", siteDetails.AssetStatus);
                cmd.Parameters.AddWithValue("asset_id", siteDetails.AssetId);

                cmd.Parameters.AddWithValue("property_latitude", siteDetails.Latitude);
                cmd.Parameters.AddWithValue("property_longitude", siteDetails.Longitude);

                cmd.Parameters.AddWithValue("property_status_id", siteDetails.SelectedPropertyStatusId);

                cmd.Parameters.AddWithValue("check_if_property_listed", siteDetails.CheckIfPropertyListed);
                cmd.Parameters.AddWithValue("listing_agent_name", siteDetails.ListingAgentName);
                cmd.Parameters.AddWithValue("listing_expiry", siteDetails.ListingExpiry);
                cmd.Parameters.AddWithValue("listing_price", siteDetails.ListingPrice);

                cmd.Parameters.AddWithValue("term", siteDetails.Term);
                cmd.Parameters.AddWithValue("asking_rent", siteDetails.AskingRent);
                cmd.Parameters.AddWithValue("lease_type", siteDetails.LeaseType);

                cmd.Parameters.AddWithValue("status_changed_date", siteDetails.StatusChangedDate);

                cmd.Parameters.AddWithValue("term_option_purchase", siteDetails.TermOptionPurchase);
                cmd.Parameters.AddWithValue("asking_rent_option_purchase", siteDetails.AskingRentOptionPurchase);
                cmd.Parameters.AddWithValue("lease_type_purchase", siteDetails.LeaseTypePurchase);
                cmd.Parameters.AddWithValue("option_purchase_price", siteDetails.OptionPurchasePrice);
                cmd.Parameters.AddWithValue("potential_use", siteDetails.PotentialUse);
                cmd.Parameters.AddWithValue("region_id", siteDetails.RegionId);
                cmd.Parameters.AddWithValue("property_header_line_2", siteDetails.PropertyHeaderLine2);

                siteDetailsId = int.Parse(cmd.ExecuteScalar().ToString());

                siteDetails.SiteDetailsId = siteDetailsId;

                con.Close();
            }

            return siteDetails.SiteDetailsId;
            //return View();
        }

        public IActionResult AddSurplusProperty()
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

            SiteDetails steDetails = new SiteDetails();

            List<StateDetails> stateList = new List<StateDetails>();
            List<MarketDetails> marketList = new List<MarketDetails>();
            List<AssetTypeViewModel> assetTypeList = new List<AssetTypeViewModel>();


            // string CS = ConfigurationManager.ConnectionStrings["testConnection"].ConnectionString;
            string CS = DBConnection.ConnectionString;

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

                con.Open();
                SqlCommand cmdMarket = new SqlCommand("GetMarketList", con);
                cmdMarket.CommandType = CommandType.StoredProcedure;


                SqlDataReader readerMarket = cmdMarket.ExecuteReader();
                while (readerMarket.Read())
                {
                    var marketDetails = new MarketDetails();
                    marketDetails.MarketId = readerMarket.IsDBNull(readerMarket.GetOrdinal("market_id")) ? 0 : readerMarket.GetInt32(readerMarket.GetOrdinal("market_id"));
                    marketDetails.MarketName = readerMarket.IsDBNull(readerMarket.GetOrdinal("market_name")) ? "" : readerMarket.GetString(readerMarket.GetOrdinal("market_name"));

                    marketList.Add(marketDetails);
                }

                con.Close();

                SqlCommand cmdAssetType = new SqlCommand("GetAssetType", con);
                cmdAssetType.CommandType = CommandType.StoredProcedure;
                con.Open();

                SqlDataReader readerAssetType = cmdAssetType.ExecuteReader();
                while (readerAssetType.Read())
                {
                    var assetType = new AssetTypeViewModel();
                    assetType.AssetTypeId = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("asset_type_id")) ? 0 : readerAssetType.GetInt32(readerAssetType.GetOrdinal("asset_type_id"));
                    if (assetType.AssetTypeId == 1)
                    {
                        assetType.AssetTypeName = "Lease/Build To Suit";
                    }
                    else
                    {
                        assetType.AssetTypeName = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("asset_type_name")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("asset_type_name"));
                    }
                    assetTypeList.Add(assetType);
                }
                con.Close();
            }




            steDetails.StateList = stateList;
            steDetails.MarketList = marketList;
            steDetails.AssetTypeList = assetTypeList;


            return View(steDetails);

        }

        
        [HttpPost]
        public IActionResult UploadImage(ImageViewModel uploadedImge)
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

            var uniqueFileName = Helper.GetUniqueFileName(uploadedImge.UploadedImage.FileName);
            
            uniqueFileName = uniqueFileName.Replace(" ", String.Empty);

            var filePath = Path.Combine(webHostEnvironment.WebRootPath + @"/UploadedImage", uniqueFileName);
            
            using (var stream = System.IO.File.Create(filePath))
            {
                uploadedImge.UploadedImage.CopyTo(stream);
            }

            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("SavePropertyImage", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                cmd.Parameters.AddWithValue("property_id", uploadedImge.PropertyId);
                cmd.Parameters.AddWithValue("image_name", uniqueFileName);
                cmd.Parameters.AddWithValue("property_type", uploadedImge.PropertyType);
                cmd.ExecuteNonQuery();


                con.Close();
            }


            return RedirectToAction("ViewSurplusProperty", new { propertyId = uploadedImge.PropertyId });

        }

        public RedirectToActionResult DeleteImage(int imageId, int propertyId)
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
                SqlCommand cmd = new SqlCommand("DeleteUploadedImage", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                cmd.Parameters.AddWithValue("image_id", imageId);

                cmd.ExecuteNonQuery();


                con.Close();
                return RedirectToAction("ViewSurplusProperty", new { propertyId = propertyId });
            }


            

        }


        [HttpPost]
        public RedirectToActionResult SaveAdditionalFile(AdditionalFilesViewModel uploadedFile)
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

            var uniqueFileName = Helper.GetUniqueFileName(uploadedFile.SelectedFile.FileName);

            var filePath = Path.Combine(webHostEnvironment.WebRootPath + @"/property_files", uniqueFileName);

            using (var stream = System.IO.File.Create(filePath))
            {
                uploadedFile.SelectedFile.CopyTo(stream);
            }

            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("SaveSurplusFiles", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                cmd.Parameters.AddWithValue("file_id", uploadedFile.FileId);
                cmd.Parameters.AddWithValue("property_id", uploadedFile.PropertyId);
                cmd.Parameters.AddWithValue("file_type", uploadedFile.FileType);
                cmd.Parameters.AddWithValue("file_name", uniqueFileName);


                cmd.ExecuteNonQuery();


                con.Close();
            }


            return RedirectToAction("ViewSurplusProperty", new { propertyId = uploadedFile.PropertyId });

        }

        public RedirectToActionResult DeleteFile(int fileId, int propertyId)
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
                SqlCommand cmd = new SqlCommand("DeleteSurplusFile", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                cmd.Parameters.AddWithValue("file_id", fileId);

                cmd.ExecuteNonQuery();


                con.Close();
                return RedirectToAction("ViewSurplusProperty", new { propertyId = propertyId });
            }

        }


        public RedirectToActionResult DeleteProperty(int propertyId)
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
                SqlCommand cmd = new SqlCommand("DeleteSurplusProperties", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                cmd.Parameters.AddWithValue("site_details_id", propertyId);

                cmd.ExecuteNonQuery();


                con.Close();
                return RedirectToAction("Index");
            }

        }


        public RedirectToActionResult MarkAsSoldOut(int propertyId)
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
                SqlCommand cmd = new SqlCommand("SellSuplusProperty", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                cmd.Parameters.AddWithValue("site_details_id", propertyId);

                cmd.ExecuteNonQuery();


                con.Close();
                return RedirectToAction("Index");
            }

        }


        public RedirectToActionResult SaveTodo(TodoViewModel todoModel)
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
                SqlCommand cmd = new SqlCommand("SaveTodo", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                cmd.Parameters.AddWithValue("property_id", todoModel.PropertyId);
                cmd.Parameters.AddWithValue("todo_text", todoModel.TodoText);
                cmd.Parameters.AddWithValue("property_type", SamsPropertyType.Surplus);
                cmd.Parameters.AddWithValue("created_by", loggedInUser.UserId);

                cmd.ExecuteNonQuery();


                con.Close();
                return RedirectToAction("ViewSurplusProperty", new { propertyId = todoModel.PropertyId });
            }

        }


        public List<TodoViewModel> GetTodoList(int propertyId)
        {
            var todoList = new List<TodoViewModel>();
            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("GetTodoList", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("property_id", propertyId);
                cmd.Parameters.AddWithValue("property_type", SamsPropertyType.Surplus);
                con.Open();


                SqlDataReader readerAssetType = cmd.ExecuteReader();
                while (readerAssetType.Read())
                {
                    var todoModel = new TodoViewModel();
                    todoModel.TodoId = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("todo_id")) ? 0 : readerAssetType.GetInt32(readerAssetType.GetOrdinal("todo_id"));
                    todoModel.PropertyId = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("property_id")) ? 0 : readerAssetType.GetInt32(readerAssetType.GetOrdinal("property_id"));
                    todoModel.TodoText = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("todo_text")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("todo_text"));
                    todoModel.PropertyType = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("property_type")) ? 0 : readerAssetType.GetInt32(readerAssetType.GetOrdinal("property_type"));

                    todoModel.CreatedDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("created_date")) ? DateTime.Now : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("created_date"));

                    todoModel.UpdatedDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("updated_date")) ? DateTime.Now : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("updated_date"));
                    todoModel.UpdatedById = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("updated_by")) ? 0 : readerAssetType.GetInt32(readerAssetType.GetOrdinal("updated_by"));

                    todoModel.CreatedUserName = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("created_by_user")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("created_by_user"));
                    todoModel.UpdatedUserName = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("updated_by_user")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("updated_by_user"));

                    todoModel.CompletedStatus = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("completed_status")) ? 0 : readerAssetType.GetInt32(readerAssetType.GetOrdinal("completed_status"));

                    todoList.Add(todoModel);
                }
                
                con.Close();
                return todoList;
            }

        }

        public IActionResult SaveMapLocation(string Latitude, string Longitude, int PropertyId)
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
                SqlCommand cmd = new SqlCommand("SaveMapLocation", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("propertyId", PropertyId);
                cmd.Parameters.AddWithValue("property_latitude", Latitude);
                cmd.Parameters.AddWithValue("property_longitude", Longitude);
                cmd.Parameters.AddWithValue("propertyType", SamsPropertyType.Surplus);
                con.Open();

                cmd.ExecuteNonQuery();
                
                con.Close();

                return RedirectToAction("ViewSurplusProperty", new { propertyId = PropertyId });
            }
        }

        public IActionResult ExportExcel()
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

            //string fileName = Path.GetFileNameWithoutExtension(@"\\OpsVsAdp\\Files\\Daily\\TempHours.xlsx");
            var filePath = Path.Combine(webHostEnvironment.WebRootPath + @"/templates", "surplus_properties_template.xlsx");

            string fullFileName = Path.Combine(webHostEnvironment.WebRootPath + @"/templates", "surplus_properties_template.xlsx");
            //string fullToFileName = Path.Combine(webHostEnvironment.WebRootPath + @"/templates", "surplus_properties" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xlsx");
            string fullToFileName = "surplus_properties" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xlsx";

            Workbook wrkBook = new Workbook();
            wrkBook.LoadFromFile(fullFileName);
            Worksheet sheet = wrkBook.Worksheets[0];





            
            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("GetPropertyListByCategory", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("asset_status", 0);
                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                int i = 5;
                string colSmHeader = "A", colAddress = "B", colCity="C";
                string colState = "D", colZipcode = "E", colCounty = "F", colAssetType = "G";
                string colLotSize = "H", colPrice = "I", colStatus = "J", colBuyer = "K", colSellingPrice="L";
                string colTenant = "M", colRent = "N";
                string colUnderContractDate = "O", colDdp="P", colClosingDate="Q", colDaysToClose="R";

                while (reader.Read())
                {
                    string cellSMNumber = colSmHeader + i.ToString();
                    string cellAddress = colAddress + i.ToString();
                    string cellCity = colCity + i.ToString();
                    string cellState = colState + i.ToString();

                    string cellZipcode = colZipcode + i.ToString();
                    string cellCounty = colCounty + i.ToString();
                    string cellAssetType = colAssetType + i.ToString();
                    string lotSize = colLotSize + i.ToString();
                    string cellPrice = colPrice + i.ToString();
                    string cellStatus = colStatus + i.ToString();

                    string cellBuyer = colBuyer + i.ToString();
                    string cellSellingPrice = colSellingPrice + i.ToString();

                    string cellTenant = colTenant + i.ToString();
                    string cellRent = colRent + i.ToString();

                    string cellUnderContractDate = colUnderContractDate + i.ToString();
                    string cellDdp = colDdp + i.ToString();
                    string cellClosingDate = colClosingDate + i.ToString();
                    string cellDaysToClose = colDaysToClose + i.ToString();

                    var steDetails = new SiteDetails();
                    steDetails.SiteDetailsId = reader.IsDBNull(reader.GetOrdinal("site_details_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("site_details_id"));
                    steDetails.NamePrefix = reader.IsDBNull(reader.GetOrdinal("name_prefix")) ? "" : reader.GetString(reader.GetOrdinal("name_prefix"));
                    steDetails.FirstName = reader.IsDBNull(reader.GetOrdinal("first_name")) ? "" : reader.GetString(reader.GetOrdinal("first_name"));

                    steDetails.LastName = reader.IsDBNull(reader.GetOrdinal("last_name")) ? "" : reader.GetString(reader.GetOrdinal("last_name"));
                    steDetails.CompanyName = reader.IsDBNull(reader.GetOrdinal("company_name")) ? "" : reader.GetString(reader.GetOrdinal("company_name"));
                    steDetails.EmailAddress = reader.IsDBNull(reader.GetOrdinal("email_address")) ? "" : reader.GetString(reader.GetOrdinal("email_address"));
                    steDetails.Address = reader.IsDBNull(reader.GetOrdinal("address")) ? "" : reader.GetString(reader.GetOrdinal("address"));
                    steDetails.CityName = reader.IsDBNull(reader.GetOrdinal("city_name")) ? "" : reader.GetString(reader.GetOrdinal("city_name"));
                    steDetails.StateId = reader.IsDBNull(reader.GetOrdinal("state_id")) ? "" : reader.GetString(reader.GetOrdinal("state_id"));

                    steDetails.StateName = reader.IsDBNull(reader.GetOrdinal("state_name")) ? "" : reader.GetString(reader.GetOrdinal("state_name"));

                    steDetails.ZipCode = reader.IsDBNull(reader.GetOrdinal("zip_code")) ? "" : reader.GetString(reader.GetOrdinal("zip_code"));
                    steDetails.ContactNumber = reader.IsDBNull(reader.GetOrdinal("contact_number")) ? "" : reader.GetString(reader.GetOrdinal("contact_number"));
                    steDetails.SamsHoldingEmployee = reader.IsDBNull(reader.GetOrdinal("sams_holding_employee")) ? false : reader.GetBoolean(reader.GetOrdinal("sams_holding_employee"));
                    steDetails.MarketId = reader.IsDBNull(reader.GetOrdinal("market_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("market_id"));
                    steDetails.PropertyHeader = reader.IsDBNull(reader.GetOrdinal("property_header")) ? "" : reader.GetString(reader.GetOrdinal("property_header"));
                    steDetails.SiteAddress = reader.IsDBNull(reader.GetOrdinal("site_address")) ? "" : reader.GetString(reader.GetOrdinal("site_address"));
                    steDetails.SiteCity = reader.IsDBNull(reader.GetOrdinal("site_city")) ? "" : reader.GetString(reader.GetOrdinal("site_city"));
                    steDetails.SiteStateId = reader.IsDBNull(reader.GetOrdinal("site_state_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("site_state_id"));

                    steDetails.SiteStateName = reader.IsDBNull(reader.GetOrdinal("site_state_name")) ? "" : reader.GetString(reader.GetOrdinal("site_state_name"));

                    steDetails.SiteCounty = reader.IsDBNull(reader.GetOrdinal("site_county")) ? "" : reader.GetString(reader.GetOrdinal("site_county"));
                    steDetails.SiteCrossStreetName = reader.IsDBNull(reader.GetOrdinal("site_cross_street_name")) ? "" : reader.GetString(reader.GetOrdinal("site_cross_street_name"));
                    steDetails.IsPropertyAvailable = reader.IsDBNull(reader.GetOrdinal("is_property_available")) ? true : reader.GetBoolean(reader.GetOrdinal("is_property_available"));
                    steDetails.Zoning = reader.IsDBNull(reader.GetOrdinal("zoning")) ? "" : reader.GetString(reader.GetOrdinal("zoning"));
                    steDetails.LotSize = reader.IsDBNull(reader.GetOrdinal("lot_size")) ? "" : reader.GetString(reader.GetOrdinal("lot_size"));

                    steDetails.SalesPrice = reader.IsDBNull(reader.GetOrdinal("sales_price")) ? "" : reader.GetString(reader.GetOrdinal("sales_price"));
                    //steDetails.SalesPrice = Helper.FormatCurrency("$", steDetails.SalesPrice);

                    steDetails.Comments = reader.IsDBNull(reader.GetOrdinal("comments")) ? "" : reader.GetString(reader.GetOrdinal("comments"));

                    steDetails.CreatedDate = reader.IsDBNull(reader.GetOrdinal("created_date")) ? DateTime.Now : reader.GetDateTime(reader.GetOrdinal("created_date"));
                    steDetails.PropertyType = reader.IsDBNull(reader.GetOrdinal("property_type")) ? 0 : reader.GetInt32(reader.GetOrdinal("property_type"));

                    steDetails.ImageName = reader.IsDBNull(reader.GetOrdinal("image_name")) ? "" : reader.GetString(reader.GetOrdinal("image_name"));

                    steDetails.AssetTypeId = reader.IsDBNull(reader.GetOrdinal("asset_type_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("asset_type_id"));
                    steDetails.AssetTypeName = reader.IsDBNull(reader.GetOrdinal("asset_type_name")) ? "" : reader.GetString(reader.GetOrdinal("asset_type_name"));
                    steDetails.AssetStatus = reader.IsDBNull(reader.GetOrdinal("asset_status")) ? 0 : reader.GetInt32(reader.GetOrdinal("asset_status"));
                    steDetails.AssetId = reader.IsDBNull(reader.GetOrdinal("asset_id")) ? "" : reader.GetString(reader.GetOrdinal("asset_id"));

                    steDetails.SelectedPropertyStatusId = reader.IsDBNull(reader.GetOrdinal("property_status_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("property_status_id"));
                    steDetails.SelectedPropertyStatus = reader.IsDBNull(reader.GetOrdinal("property_status")) ? "" : reader.GetString(reader.GetOrdinal("property_status"));

                    if (steDetails.SiteAddress.Length > 15)
                    {
                        steDetails.SiteAddressSmall = steDetails.SiteAddress.Substring(0, 15) + "..";
                    }
                    else
                    {
                        steDetails.SiteAddressSmall = steDetails.SiteAddress;
                    }

                    if (steDetails.AssetStatus == 0)
                    {
                        steDetails.AssetStatusName = "Available";
                    }
                    else
                    {
                        steDetails.AssetStatusName = "Sold";
                    }

                    steDetails.TransactionStatusName = "";

                    steDetails.DiligenceAcquisitions = GetDiligenceAcquisition(steDetails.SiteDetailsId);
                    steDetails.DiligenceDispositions = GetDiligenceDispositions(steDetails.SiteDetailsId);
                    steDetails.DiligenceLease = GetDiligenceLease(steDetails.SiteDetailsId);
                    steDetails.DiligenceLeaseList = GetDiligenceLeaseList(steDetails.SiteDetailsId);
                    steDetails.DiligenceDispositions_SaleLeaseBack = GetDiligenceDispositions_SaleLeaseBack(steDetails.SiteDetailsId);

                    // steDetails.DiligenceDispositions = GetDiligenceDispositions(steDetails.SiteDetailsId);

                    steDetails.SelectedDiligenceDispositions = new DiligenceDispositionsViewModel();
                    DateTime? transactionClosedDate = default(DateTime?);

                    steDetails.DiligenceLeaseWithPurchase = new DiligenceLeaseWithPurchaseViewModel();
                    steDetails.SelectedDiligenceLease = new DiligenceLeaseViewModel();
                    steDetails.DiligenceLeaseWithPurchase = new DiligenceLeaseWithPurchaseViewModel();

                    string strDate = "";

                    int saleLoi = 0, saleUnderContract = 0, saleTerminated = 0, saleClosed = 0;
                    if (steDetails.AssetTypeId == (int)SamAssetType.Fee || steDetails.AssetTypeId == (int)SamAssetType.FeeSubjectToLease)
                    {
                        foreach (DiligenceDispositionsViewModel ddm in steDetails.DiligenceDispositions)
                        {
                            if (ddm.SelectedTransactionStatusId == (int)TransactionStatus.Under_LOI)
                            {
                                saleLoi = saleLoi + 1;
                            }
                            else if (ddm.SelectedTransactionStatusId == (int)TransactionStatus.Under_Contract)
                            {
                                saleUnderContract = saleUnderContract + 1;
                            }
                            else if (ddm.SelectedTransactionStatusId == (int)TransactionStatus.Terminated_Acquisitions)
                            {
                                saleTerminated = saleTerminated + 1;
                            }
                            else if (ddm.SelectedTransactionStatusId == (int)TransactionStatus.Closed_Acquisitions)
                            {
                                saleClosed = saleClosed + 1;
                            }


                            if (steDetails.MaxPriorityTransactionStatusId == 0)
                            {
                                steDetails.MaxPriorityTransactionStatusId = ddm.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = ddm.SelectedTransactionStatusName;
                                steDetails.SelectedDiligenceDispositions = ddm;
                                transactionClosedDate = ddm.ClosingDate;
                            }



                            if ((ddm.SelectedTransactionStatusId == (int)TransactionStatus.LOI_Received ||
                                ddm.SelectedTransactionStatusId == (int)TransactionStatus.Terminated_Acquisitions) &&
                                (ddm.SelectedTransactionStatusId != (int)SamsTransactionStatus.Under_LOI &&
                                steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Under_Contract &&
                                steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Closed_Dispositions
                                ))
                            {
                                steDetails.MaxPriorityTransactionStatusId = ddm.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = ddm.SelectedTransactionStatusName;
                                steDetails.StatusChangedDate = ddm.SelectedTransactionDate;
                                steDetails.SelectedDiligenceDispositions = ddm;
                                transactionClosedDate = ddm.ClosingDate;
                            }

                            if (ddm.SelectedTransactionStatusId == (int)SamsTransactionStatus.Under_LOI &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Under_Contract) &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Closed_Dispositions))
                            {
                                steDetails.MaxPriorityTransactionStatusId = ddm.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = ddm.SelectedTransactionStatusName;
                                steDetails.StatusChangedDate = ddm.SelectedTransactionDate;
                                steDetails.SelectedDiligenceDispositions = ddm;
                                transactionClosedDate = ddm.ClosingDate;
                            }

                            if (ddm.SelectedTransactionStatusId == (int)SamsTransactionStatus.Under_Contract &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Closed_Dispositions))
                            {
                                steDetails.MaxPriorityTransactionStatusId = ddm.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = ddm.SelectedTransactionStatusName;
                                steDetails.StatusChangedDate = ddm.SelectedTransactionDate;
                                steDetails.SelectedDiligenceDispositions = ddm;
                                transactionClosedDate = ddm.ClosingDate;
                            }

                            if (ddm.SelectedTransactionStatusId == (int)SamsTransactionStatus.Closed_Dispositions)
                            {
                                steDetails.MaxPriorityTransactionStatusId = ddm.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = ddm.SelectedTransactionStatusName;
                                steDetails.StatusChangedDate = ddm.SelectedTransactionDate;
                                steDetails.SelectedDiligenceDispositions = ddm;
                                transactionClosedDate = ddm.ClosingDate;
                            }

                        }


                        sheet.Range[cellBuyer].Value = steDetails.SelectedDiligenceDispositions.Buyers;
                        sheet.Range[cellSellingPrice].Value = steDetails.SelectedDiligenceDispositions.SalePrice;

                        sheet.Range[cellUnderContractDate].Value = null;
                        sheet.Range[cellUnderContractDate].Value = steDetails.SelectedDiligenceDispositions.UnderContractDate == default(DateTime?) ? "" : steDetails.SelectedDiligenceDispositions.UnderContractDate.Value.ToString("MM/dd/yyyy");
                        sheet.Range[cellDdp].Value = null;
                        sheet.Range[cellDdp].Value = steDetails.SelectedDiligenceDispositions.DueDiligenceExpairyDate == default(DateTime?) ? "" : steDetails.SelectedDiligenceDispositions.DueDiligenceExpairyDate.Value.ToString("MM/dd/yyyy");

                        var dtClosedDate = "";
                        int? daysToClose = null;
                        if (transactionClosedDate != default(DateTime?))
                        {
                            dtClosedDate = transactionClosedDate.Value.ToString("MM/dd/yyyy");
                            daysToClose = (transactionClosedDate.Value - DateTime.Now).Days;

                            if (daysToClose < 0)
                            {
                                daysToClose = 0;
                            }
                        }
                        if (dtClosedDate.Trim().Length > 0)
                        {
                            sheet.Range[cellClosingDate].Value = null;
                            sheet.Range[cellClosingDate].Value = dtClosedDate;
                        }
                        if (daysToClose != null){
                            sheet.Range[cellDaysToClose].Value = daysToClose.ToString();
                        }
                        
                    }
                    else if (steDetails.AssetTypeId == (int)SamAssetType.Lease)
                    {
                        steDetails.DiligenceLeaseList = GetDiligenceLeaseList(steDetails.SiteDetailsId);
                        int leaseLoi = 0, leaseUnderContract = 0, leaseTerminated = 0, leaseClosed = 0;
                        foreach (DiligenceLeaseViewModel dl in steDetails.DiligenceLeaseList)
                        {
                            if (dl.SelectedTransactionStatusId == (int)TransactionStatus.Under_LOI)
                            {
                                leaseLoi = leaseLoi + 1;
                            }
                            else if (dl.SelectedTransactionStatusId == (int)TransactionStatus.Under_Contract)
                            {
                                leaseUnderContract = leaseUnderContract + 1;
                            }
                            else if (dl.SelectedTransactionStatusId == (int)TransactionStatus.Terminated_Acquisitions)
                            {
                                leaseTerminated = leaseTerminated + 1;
                            }
                            else if (dl.SelectedTransactionStatusId == (int)TransactionStatus.Closed_Acquisitions)
                            {
                                leaseClosed = leaseClosed + 1;
                            }

                            if (steDetails.MaxPriorityTransactionStatusId == 0)
                            {
                                steDetails.MaxPriorityTransactionStatusId = dl.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = dl.SelectedTransactionStatusName;
                                steDetails.SelectedDiligenceLease = dl;
                                transactionClosedDate = dl.ClosingDate;
                            }



                            if ((dl.SelectedTransactionStatusId == (int)TransactionStatus.LOI_Received ||
                                dl.SelectedTransactionStatusId == (int)TransactionStatus.Terminated_Acquisitions) &&
                                (dl.SelectedTransactionStatusId != (int)SamsTransactionStatus.Under_LOI &&
                                steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Under_Contract &&
                                steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Closed_Dispositions
                                ))
                            {
                                steDetails.MaxPriorityTransactionStatusId = dl.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = dl.SelectedTransactionStatusName;
                                steDetails.StatusChangedDate = dl.SelectedTransactionDate;
                                steDetails.SelectedDiligenceLease = dl;
                                transactionClosedDate = dl.ClosingDate;
                            }

                            if (dl.SelectedTransactionStatusId == (int)SamsTransactionStatus.Under_LOI &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Under_Contract) &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Closed_Dispositions))
                            {
                                steDetails.MaxPriorityTransactionStatusId = dl.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = dl.SelectedTransactionStatusName;
                                steDetails.StatusChangedDate = dl.SelectedTransactionDate;
                                steDetails.SelectedDiligenceLease = dl;
                                transactionClosedDate = dl.ClosingDate;
                            }

                            if (dl.SelectedTransactionStatusId == (int)SamsTransactionStatus.Under_Contract &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Closed_Dispositions))
                            {
                                steDetails.MaxPriorityTransactionStatusId = dl.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = dl.SelectedTransactionStatusName;
                                steDetails.StatusChangedDate = dl.SelectedTransactionDate;
                                steDetails.SelectedDiligenceLease = dl;
                                transactionClosedDate = dl.ClosingDate;
                            }

                            if (dl.SelectedTransactionStatusId == (int)SamsTransactionStatus.Closed_Dispositions)
                            {
                                steDetails.MaxPriorityTransactionStatusId = dl.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = dl.SelectedTransactionStatusName;
                                steDetails.StatusChangedDate = dl.SelectedTransactionDate;
                                steDetails.SelectedDiligenceLease = dl;
                                transactionClosedDate = dl.ClosingDate;
                            }
                        }
                        

                        if (steDetails.SelectedDiligenceLease != null)
                        {
                            sheet.Range[cellTenant].Value = steDetails.SelectedDiligenceLease.Tenant;
                            sheet.Range[cellRent].Value = steDetails.SelectedDiligenceLease.Rent;

                            sheet.Range[cellUnderContractDate].Value = null;
                            sheet.Range[cellUnderContractDate].Value = steDetails.SelectedDiligenceLease.UnderContractDate == default(DateTime?) ? "" : steDetails.SelectedDiligenceLease.UnderContractDate.Value.ToString("MM/dd/yyyy");

                            sheet.Range[cellDdp].Value = null;
                            sheet.Range[cellDdp].Value = steDetails.SelectedDiligenceLease.DueDiligenceExpiryDate == default(DateTime?) ? "" : steDetails.SelectedDiligenceLease.DueDiligenceExpiryDate.Value.ToString("MM/dd/yyyy");

                            var dtClosedDate = "";
                            int? daysToClose = null;
                            if (transactionClosedDate != default(DateTime?))
                            {
                                dtClosedDate = transactionClosedDate.Value.ToString("MM/dd/yyyy");
                                daysToClose = (transactionClosedDate.Value - DateTime.Now).Days;
                                if (daysToClose < 0)
                                {
                                    daysToClose = 0;
                                }
                            }
                            if (dtClosedDate.Trim().Length > 0)
                            {
                                sheet.Range[cellClosingDate].Value = null;
                                sheet.Range[cellClosingDate].Value = dtClosedDate;
                            }
                            
                            if (daysToClose != null)
                            {
                                sheet.Range[cellDaysToClose].Value = daysToClose.ToString();
                            }
                        }
                        


                    }
                    else if (steDetails.AssetTypeId == (int)SamAssetType.LeaseWithPurchaseOption)
                    {
                        steDetails.DiligenceLeaseWithPurchaseList = GetDiligenceLeaseWithPurchaseList(steDetails.SiteDetailsId);

                        foreach (DiligenceLeaseWithPurchaseViewModel dl in steDetails.DiligenceLeaseWithPurchaseList)
                        {

                            steDetails.DiligenceLeaseWithPurchase = dl;
                            if (steDetails.MaxPriorityTransactionStatusId == 0)
                            {
                                steDetails.MaxPriorityTransactionStatusId = dl.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = dl.SelectedTransactionStatusName;

                                steDetails.DiligenceLeaseWithPurchase = dl;
                                transactionClosedDate = dl.ClosingDate;
                            }



                            if ((dl.SelectedTransactionStatusId == (int)TransactionStatus.LOI_Received ||
                                dl.SelectedTransactionStatusId == (int)TransactionStatus.Terminated_Acquisitions) &&
                                (dl.SelectedTransactionStatusId != (int)SamsTransactionStatus.Under_LOI &&
                                steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Under_Contract &&
                                steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Closed_Dispositions
                                ))
                            {
                                steDetails.MaxPriorityTransactionStatusId = dl.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = dl.SelectedTransactionStatusName;
                                steDetails.DiligenceLeaseWithPurchase = dl;
                                transactionClosedDate = dl.ClosingDate;
                            }

                            if (dl.SelectedTransactionStatusId == (int)SamsTransactionStatus.Under_LOI &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Under_Contract) &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Closed_Dispositions))
                            {
                                steDetails.MaxPriorityTransactionStatusId = dl.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = dl.SelectedTransactionStatusName;
                                steDetails.DiligenceLeaseWithPurchase = dl;
                                transactionClosedDate = dl.ClosingDate;
                            }

                            if (dl.SelectedTransactionStatusId == (int)SamsTransactionStatus.Under_Contract &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Closed_Dispositions))
                            {
                                steDetails.MaxPriorityTransactionStatusId = dl.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = dl.SelectedTransactionStatusName;
                                steDetails.DiligenceLeaseWithPurchase = dl;
                                transactionClosedDate = dl.ClosingDate;
                            }

                            if (dl.SelectedTransactionStatusId == (int)SamsTransactionStatus.Closed_Dispositions)
                            {
                                steDetails.MaxPriorityTransactionStatusId = dl.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = dl.SelectedTransactionStatusName;
                                steDetails.DiligenceLeaseWithPurchase = dl;
                                transactionClosedDate = dl.ClosingDate;
                            }
                        }



                        if (steDetails.DiligenceLeaseWithPurchase != null)
                        {
                            sheet.Range[cellTenant].Value = steDetails.DiligenceLeaseWithPurchase.Tenant;
                            sheet.Range[cellRent].Value = steDetails.DiligenceLeaseWithPurchase.Rent;

                            sheet.Range[cellUnderContractDate].Value = null;
                            sheet.Range[cellUnderContractDate].Value = steDetails.DiligenceLeaseWithPurchase.UnderContractDate == default(DateTime?) ? "" : steDetails.DiligenceLeaseWithPurchase.UnderContractDate.Value.ToString("MM/dd/yyyy");
                            sheet.Range[cellDdp].Value = null;
                            sheet.Range[cellDdp].Value = steDetails.DiligenceLeaseWithPurchase.DueDiligenceExpiryDate == default(DateTime?) ? "" : steDetails.DiligenceLeaseWithPurchase.DueDiligenceExpiryDate.Value.ToString("MM/dd/yyyy");

                            var dtClosedDate = "";
                            int? daysToClose = null;
                            if (transactionClosedDate != default(DateTime?))
                            {
                                dtClosedDate = transactionClosedDate.Value.ToString("MM/dd/yyyy");
                                daysToClose = (transactionClosedDate.Value - DateTime.Now).Days;
                                if (daysToClose < 0)
                                {
                                    daysToClose = 0;
                                }
                            }
                            
                            if (dtClosedDate.Trim().Length > 0)
                            {
                                sheet.Range[cellClosingDate].Value = null;
                                sheet.Range[cellClosingDate].Value = dtClosedDate;
                            }
                            if (daysToClose != null)
                            {
                                sheet.Range[cellDaysToClose].Value = daysToClose.ToString();
                            }
                        }
                    }
                    else if (steDetails.AssetTypeId == (int)SamAssetType.SaleLeaseBack)
                    {
                        foreach (DiligenceDispositionsViewModel ddv in steDetails.DiligenceDispositions_SaleLeaseBack)
                        {

                            if (ddv.SelectedTransactionStatusId == (int)TransactionStatus.Closed_Acquisitions || ddv.SelectedTransactionStatusId == (int)TransactionStatus.Under_Contract || ddv.SelectedTransactionStatusId == (int)TransactionStatus.Under_LOI)
                            {
                                steDetails.CanAddTransactions = false;
                            }

                            if (ddv.SelectedTransactionStatusId == (int)TransactionStatus.Closed_Acquisitions)
                            {
                                steDetails.IsTransactionClosed = true;
                            }

                            if (steDetails.MaxPriorityTransactionStatusId == 0)
                            {
                                steDetails.MaxPriorityTransactionStatusId = ddv.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = ddv.SelectedTransactionStatusName;
                            }
                            steDetails.SelectedDiligenceDispositions = ddv;
                            transactionClosedDate = ddv.ClosingDate;


                            if ((ddv.SelectedTransactionStatusId == (int)TransactionStatus.LOI_Received ||
                                ddv.SelectedTransactionStatusId == (int)TransactionStatus.Terminated_Acquisitions) &&
                                (ddv.SelectedTransactionStatusId != (int)SamsTransactionStatus.Under_LOI &&
                                steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Under_Contract &&
                                steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Closed_Dispositions
                                ))
                            {
                                steDetails.MaxPriorityTransactionStatusId = ddv.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = ddv.SelectedTransactionStatusName;
                                steDetails.SelectedDiligenceDispositions = ddv;
                                transactionClosedDate = ddv.ClosingDate;
                            }

                            if (ddv.SelectedTransactionStatusId == (int)SamsTransactionStatus.Under_LOI &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Under_Contract) &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Closed_Dispositions))
                            {
                                steDetails.MaxPriorityTransactionStatusId = ddv.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = ddv.SelectedTransactionStatusName;
                                steDetails.SelectedDiligenceDispositions = ddv;
                                transactionClosedDate = ddv.ClosingDate;
                            }

                            if (ddv.SelectedTransactionStatusId == (int)SamsTransactionStatus.Under_Contract &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Closed_Dispositions))
                            {
                                steDetails.MaxPriorityTransactionStatusId = ddv.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = ddv.SelectedTransactionStatusName;
                                steDetails.SelectedDiligenceDispositions = ddv;
                                transactionClosedDate = ddv.ClosingDate;
                            }

                            if (ddv.SelectedTransactionStatusId == (int)SamsTransactionStatus.Closed_Dispositions)
                            {
                                steDetails.MaxPriorityTransactionStatusId = ddv.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = ddv.SelectedTransactionStatusName;
                                steDetails.SelectedDiligenceDispositions = ddv;
                                transactionClosedDate = ddv.ClosingDate;
                            }


                        }

                        sheet.Range[cellBuyer].Value = steDetails.SelectedDiligenceDispositions.Buyers;
                        sheet.Range[cellRent].Value = steDetails.SelectedDiligenceDispositions.Rent_SaleLeaseBack;
                        sheet.Range[cellSellingPrice].Value = steDetails.SelectedDiligenceDispositions.SalePrice;

                        sheet.Range[cellUnderContractDate].Value = null;
                        
                        sheet.Range[cellUnderContractDate].Value = steDetails.SelectedDiligenceDispositions.UnderContractDate == default(DateTime?) ? "" : @steDetails.SelectedDiligenceDispositions.UnderContractDate.Value.ToString("MM/dd/yyyy");
                        

                        sheet.Range[cellDdp].Value = null;
                        
                        sheet.Range[cellDdp].Value = steDetails.SelectedDiligenceDispositions.DueDiligenceExpairyDate == default(DateTime?) ? "" : @steDetails.SelectedDiligenceDispositions.DueDiligenceExpairyDate.Value.ToString("MM/dd/yyyy");
                        

                        var dtClosedDate = "";
                        int? daysToClose = null;
                        if (transactionClosedDate != default(DateTime?))
                        {
                            dtClosedDate = transactionClosedDate.Value.ToString("MM/dd/yyyy");
                            daysToClose = (transactionClosedDate.Value - DateTime.Now).Days;
                            if (daysToClose < 0)
                            {
                                daysToClose = 0;
                            }
                        }
                        
                        if (dtClosedDate.Trim().Length > 0)
                        {
                            sheet.Range[cellClosingDate].Value = null;
                            
                            sheet.Range[cellClosingDate].Value = dtClosedDate;

                            
                        }
                        if (daysToClose != null)
                        {
                            sheet.Range[cellDaysToClose].Value = daysToClose.ToString();
                        }
                    }

                    

                    steDetails.TodoList = GetTodoList(steDetails.SiteDetailsId);
                    if (steDetails.TodoList.Count > 0)
                    {
                        steDetails.LatestComment = steDetails.TodoList[0].TodoText;
                    }

                    
                    

                    sheet.Range[cellSMNumber].Value = steDetails.AssetId;

                    List<string> addressStings = steDetails.SiteAddress.Split(',').ToList<string>();

                    if(addressStings.Count > 0)
                    {
                        sheet.Range[cellAddress].Value = addressStings[0];
                    }
                    




                    sheet.Range[cellCity].Value = steDetails.SiteCity;
                    sheet.Range[cellState].Value = steDetails.SiteStateName;
                    
                    sheet.Range[cellZipcode].Value = steDetails.ZipCode;
                    sheet.Range[cellCounty].Value = steDetails.SiteCounty;
  
                    sheet.Range[cellAssetType].Value = steDetails.AssetTypeName;
                    sheet.Range[lotSize].Value = steDetails.LotSize;

                    sheet.Range[cellPrice].Value = steDetails.SalesPrice;
                    sheet.Range[cellStatus].Value = steDetails.MaxPriorityTransactionStatusName;

                    


                    
                    

                    i = i + 1;

                    sheet.Range[cellUnderContractDate].NumberFormat = "mm-dd-yyyy;@";
                    sheet.Range[cellDdp].NumberFormat = "mm-dd-yyyy;@";
                    sheet.Range[cellClosingDate].NumberFormat = "mm-dd-yyyy;@";
                }
                con.Close();

                sheet.Range["A5:S" + i.ToString()].BorderInside(LineStyleType.Thin, Color.Black);
                sheet.Range["A5:S" + i.ToString()].BorderAround(LineStyleType.Thin, Color.Black);
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

        public ActionResult Dashboard(string s)
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

            var surplusPropertiesDashboard = new SurplusPropertiesDashboard();
            

            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("GetSurplusDashboard", con);
                cmd.CommandType = CommandType.StoredProcedure;
                
                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    string dataType = reader.IsDBNull(reader.GetOrdinal("data_type")) ? "" : reader.GetString(reader.GetOrdinal("data_type"));
                    int totalRecords = reader.IsDBNull(reader.GetOrdinal("totalRecores")) ? 0 : reader.GetInt32(reader.GetOrdinal("totalRecores"));

                    switch(dataType)
                    {
                        case "all_surplus_properties": 
                            surplusPropertiesDashboard.TotalProperties = totalRecords;
                            break;

                        case "da_all":
                            surplusPropertiesDashboard.TotalAcquisition = totalRecords;
                            break;

                        case "da_open":
                            surplusPropertiesDashboard.TotalCreatedDisposition = totalRecords;
                            break;

                        case "da_closed":
                            surplusPropertiesDashboard.TotalClosedAcquisition = totalRecords;
                            break;

                        case "da_terminated":
                            surplusPropertiesDashboard.TotalTerminatedAcquisition = totalRecords;
                            break;

                        case "diligence_dispositions_all":
                            surplusPropertiesDashboard.TotalDisposition = totalRecords;
                            break;

                        case "diligence_dispositions_open":
                            surplusPropertiesDashboard.TotalCreatedDisposition = totalRecords;
                            break;

                        case "diligence_dispositions_closed":
                            surplusPropertiesDashboard.TotalClosedDisposition = totalRecords;
                            break;

                        case "diligence_dispositions_terminated":
                            surplusPropertiesDashboard.TotalTerminatedDisposition = totalRecords;
                            break;

                        case "lease_all":
                            surplusPropertiesDashboard.TotalLease = totalRecords;
                            break;
                    }

                    
                }
                con.Close();

                

                List<SiteDetails> surplusPropertiesList = new List<SiteDetails>();

                if (s != null && s != "all")
                {
                    using (SqlConnection con1 = new SqlConnection(CS))
                    {
                        SqlCommand cmd1 = new SqlCommand("SearchSurplusPropertyList", con1);
                        cmd1.CommandType = CommandType.StoredProcedure;
                        cmd1.Parameters.AddWithValue("searchString", s);
                        con1.Open();

                        SqlDataReader reader1 = cmd1.ExecuteReader();
                        while (reader1.Read())
                        {
                            var steDetails = new SiteDetails();
                            steDetails.SiteDetailsId = reader1.IsDBNull(reader1.GetOrdinal("site_details_id")) ? 0 : reader1.GetInt32(reader1.GetOrdinal("site_details_id"));
                            steDetails.NamePrefix = reader1.IsDBNull(reader1.GetOrdinal("name_prefix")) ? "" : reader1.GetString(reader1.GetOrdinal("name_prefix"));
                            steDetails.FirstName = reader1.IsDBNull(reader1.GetOrdinal("first_name")) ? "" : reader1.GetString(reader1.GetOrdinal("first_name"));

                            steDetails.LastName = reader1.IsDBNull(reader1.GetOrdinal("last_name")) ? "" : reader1.GetString(reader1.GetOrdinal("last_name"));
                            steDetails.CompanyName = reader1.IsDBNull(reader1.GetOrdinal("company_name")) ? "" : reader1.GetString(reader1.GetOrdinal("company_name"));
                            steDetails.EmailAddress = reader1.IsDBNull(reader1.GetOrdinal("email_address")) ? "" : reader1.GetString(reader1.GetOrdinal("email_address"));
                            steDetails.Address = reader1.IsDBNull(reader1.GetOrdinal("address")) ? "" : reader1.GetString(reader1.GetOrdinal("address"));
                            steDetails.CityName = reader1.IsDBNull(reader1.GetOrdinal("city_name")) ? "" : reader1.GetString(reader1.GetOrdinal("city_name"));
                            steDetails.StateId = reader1.IsDBNull(reader1.GetOrdinal("state_id")) ? "" : reader1.GetString(reader1.GetOrdinal("state_id"));

                            steDetails.StateName = reader1.IsDBNull(reader1.GetOrdinal("state_name")) ? "" : reader1.GetString(reader1.GetOrdinal("state_name"));

                            steDetails.ZipCode = reader1.IsDBNull(reader1.GetOrdinal("zip_code")) ? "" : reader1.GetString(reader1.GetOrdinal("zip_code"));
                            steDetails.ContactNumber = reader1.IsDBNull(reader1.GetOrdinal("contact_number")) ? "" : reader1.GetString(reader1.GetOrdinal("contact_number"));
                            steDetails.SamsHoldingEmployee = reader1.IsDBNull(reader1.GetOrdinal("sams_holding_employee")) ? false : reader1.GetBoolean(reader1.GetOrdinal("sams_holding_employee"));
                            steDetails.MarketId = reader1.IsDBNull(reader1.GetOrdinal("market_id")) ? 0 : reader1.GetInt32(reader1.GetOrdinal("market_id"));
                            steDetails.PropertyHeader = reader1.IsDBNull(reader1.GetOrdinal("property_header")) ? "" : reader1.GetString(reader1.GetOrdinal("property_header"));
                            steDetails.SiteAddress = reader1.IsDBNull(reader1.GetOrdinal("site_address")) ? "" : reader1.GetString(reader1.GetOrdinal("site_address"));
                            steDetails.SiteCity = reader1.IsDBNull(reader1.GetOrdinal("site_city")) ? "" : reader1.GetString(reader1.GetOrdinal("site_city"));
                            steDetails.SiteStateId = reader1.IsDBNull(reader1.GetOrdinal("site_state_id")) ? 0 : reader1.GetInt32(reader1.GetOrdinal("site_state_id"));

                            steDetails.SiteStateName = reader1.IsDBNull(reader1.GetOrdinal("site_state_name")) ? "" : reader1.GetString(reader1.GetOrdinal("site_state_name"));

                            steDetails.SiteCounty = reader1.IsDBNull(reader1.GetOrdinal("site_county")) ? "" : reader1.GetString(reader1.GetOrdinal("site_county"));
                            steDetails.SiteCrossStreetName = reader1.IsDBNull(reader1.GetOrdinal("site_cross_street_name")) ? "" : reader1.GetString(reader1.GetOrdinal("site_cross_street_name"));
                            steDetails.IsPropertyAvailable = reader1.IsDBNull(reader1.GetOrdinal("is_property_available")) ? true : reader1.GetBoolean(reader1.GetOrdinal("is_property_available"));
                            steDetails.Zoning = reader1.IsDBNull(reader1.GetOrdinal("zoning")) ? "" : reader1.GetString(reader1.GetOrdinal("zoning"));
                            steDetails.LotSize = reader1.IsDBNull(reader1.GetOrdinal("lot_size")) ? "" : reader1.GetString(reader1.GetOrdinal("lot_size"));

                            steDetails.SalesPrice = reader1.IsDBNull(reader1.GetOrdinal("sales_price")) ? "" : reader1.GetString(reader1.GetOrdinal("sales_price"));
                            //steDetails.SalesPrice = Helper.FormatCurrency("$", steDetails.SalesPrice);

                            steDetails.Comments = reader1.IsDBNull(reader1.GetOrdinal("comments")) ? "" : reader1.GetString(reader1.GetOrdinal("comments"));

                            steDetails.CreatedDate = reader1.IsDBNull(reader1.GetOrdinal("created_date")) ? DateTime.Now : reader1.GetDateTime(reader1.GetOrdinal("created_date"));
                            steDetails.PropertyType = reader1.IsDBNull(reader1.GetOrdinal("property_type")) ? 0 : reader1.GetInt32(reader1.GetOrdinal("property_type"));

                            steDetails.ImageName = reader1.IsDBNull(reader1.GetOrdinal("image_name")) ? "" : reader1.GetString(reader1.GetOrdinal("image_name"));

                            steDetails.AssetTypeId = reader1.IsDBNull(reader1.GetOrdinal("asset_type_id")) ? 0 : reader1.GetInt32(reader1.GetOrdinal("asset_type_id"));
                            steDetails.AssetTypeName = reader1.IsDBNull(reader1.GetOrdinal("asset_type_name")) ? "" : reader1.GetString(reader1.GetOrdinal("asset_type_name"));
                            steDetails.AssetStatus = reader1.IsDBNull(reader1.GetOrdinal("asset_status")) ? 0 : reader1.GetInt32(reader1.GetOrdinal("asset_status"));
                            steDetails.AssetId = reader1.IsDBNull(reader1.GetOrdinal("asset_id")) ? "" : reader1.GetString(reader1.GetOrdinal("asset_id"));

                            steDetails.SelectedPropertyStatusId = reader1.IsDBNull(reader1.GetOrdinal("property_status_id")) ? 0 : reader1.GetInt32(reader1.GetOrdinal("property_status_id"));
                            steDetails.SelectedPropertyStatus = reader1.IsDBNull(reader1.GetOrdinal("property_status")) ? "" : reader1.GetString(reader1.GetOrdinal("property_status"));

                            if (steDetails.SiteAddress.Length > 15)
                            {
                                steDetails.SiteAddressSmall = steDetails.SiteAddress.Substring(0, 15) + "..";
                            }
                            else
                            {
                                steDetails.SiteAddressSmall = steDetails.SiteAddress;
                            }



                            if (steDetails.AssetStatus == 0)
                            {
                                steDetails.AssetStatusName = "Available";
                            }
                            else
                            {
                                steDetails.AssetStatusName = "Sold";
                            }
                            surplusPropertiesList.Add(steDetails);
                        }
                        con1.Close();



                        
                    }

                }


                surplusPropertiesDashboard.TotalLoi = GetTotalCountByTransactionStatus((int)SamsTransactionStatus.Under_LOI);
                surplusPropertiesDashboard.TotalUnderContract = GetTotalCountByTransactionStatus((int)SamsTransactionStatus.Under_Contract);
                surplusPropertiesDashboard.TotalClosed = GetTotalCountByTransactionStatus((int)SamsTransactionStatus.Closed_Dispositions);
                surplusPropertiesDashboard.TotalTerminated = GetTotalCountByTransactionStatus((int)SamsTransactionStatus.Terminated_Dispositions);

                

                SqlCommand cmdSurplus = new SqlCommand("SurplusNotificationList", con);
                cmdSurplus.CommandType = CommandType.StoredProcedure;
                con.Open();
                SqlDataReader readerAssetType = cmdSurplus.ExecuteReader();
                surplusPropertiesDashboard.SurplusNotificationList = SamsNotificationController.CreateNotificationList(readerAssetType);
                con.Close();

                surplusPropertiesDashboard.SearchedPropertyList = surplusPropertiesList;



                SqlCommand cmdSurplusListing = new SqlCommand("SurplusListingExpiry", con);
                cmdSurplusListing.CommandType = CommandType.StoredProcedure;

                con.Open();
                SqlDataReader readerSurplusListing = cmdSurplusListing.ExecuteReader();
                surplusPropertiesDashboard.SurplusListingExpiryList = SamsNotificationController.CreatePropertyNotificationList(readerSurplusListing);
                con.Close();

            }


            return View(surplusPropertiesDashboard);
        }

        public ActionResult ExportDueDiligenceAcquisition()
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

            //string fileName = Path.GetFileNameWithoutExtension(@"\\OpsVsAdp\\Files\\Daily\\TempHours.xlsx");
            var filePath = Path.Combine(webHostEnvironment.WebRootPath + @"/templates", "Due_Diligence_Acquisitions_Template.xlsx");

            string fullFileName = Path.Combine(webHostEnvironment.WebRootPath + @"/templates", "Due_Diligence_Acquisitions_Template.xlsx");
            string fullToFileName = Path.Combine(webHostEnvironment.WebRootPath + @"/templates", "Due_Diligence_Acquisitions.xlsx");

            Workbook wrkBook = new Workbook();
            wrkBook.LoadFromFile(fullFileName);
            Worksheet sheet = wrkBook.Worksheets[0];

            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("GetSurplusAcquisitionList", con);
                cmd.CommandType = CommandType.StoredProcedure;
                
                cmd.Parameters.AddWithValue("acquisition_status", 0);
                
                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                int i = 5;
                int j = 6;
                string colMainItemHeader = "A";
                string colMainItemHeaderValue = "B";
                string colContactDate = "C";
                string colPeriodNameHeader = "D";
                string colDurationHeader = "E";
                string colStartDate = "F";
                string colEndDate = "G";
                string colDaysToExpire = "H";
                string colNotes = "I";

                while (reader.Read())
                {


                    string propertyHeader = reader.IsDBNull(reader.GetOrdinal("property_header")) ? "" : reader.GetString(reader.GetOrdinal("property_header"));
                    string purchasePrice = reader.IsDBNull(reader.GetOrdinal("purchase_price")) ? "" : reader.GetString(reader.GetOrdinal("purchase_price"));
                    string earnestMoney = reader.IsDBNull(reader.GetOrdinal("earnest_money")) ? "" : reader.GetString(reader.GetOrdinal("earnest_money"));
                    string exchage1031 = reader.IsDBNull(reader.GetOrdinal("exchange_1031")) ? "" : reader.GetString(reader.GetOrdinal("exchange_1031"));
                    string deadline1031 = reader.IsDBNull(reader.GetOrdinal("dead_line_1031")) ? "" : reader.GetString(reader.GetOrdinal("dead_line_1031"));
                    string sellers = reader.IsDBNull(reader.GetOrdinal("sellers")) ? "" : reader.GetString(reader.GetOrdinal("sellers"));
                    string escrowAgent = reader.IsDBNull(reader.GetOrdinal("escrow_agent")) ? "" : reader.GetString(reader.GetOrdinal("escrow_agent"));
                    string subDivision = reader.IsDBNull(reader.GetOrdinal("sub_division")) ? "" : reader.GetString(reader.GetOrdinal("sub_division"));
                    string realEstateAgent = reader.IsDBNull(reader.GetOrdinal("real_estate_agent")) ? "" : reader.GetString(reader.GetOrdinal("real_estate_agent"));
                    DateTime createdDate = reader.IsDBNull(reader.GetOrdinal("created_date")) ? DateTime.Now : reader.GetDateTime(reader.GetOrdinal("created_date"));
                    DateTime closedDate = reader.IsDBNull(reader.GetOrdinal("closed_date")) ? DateTime.Now : reader.GetDateTime(reader.GetOrdinal("closed_date"));
                    DateTime terminatedDate = reader.IsDBNull(reader.GetOrdinal("terminated_date")) ? DateTime.Now : reader.GetDateTime(reader.GetOrdinal("terminated_date"));


                    string cellMainItemHeader = colMainItemHeader + i.ToString();
                    string cellMainItemHeaderValue = colMainItemHeaderValue + i.ToString();
                    string cellContactDate = colContactDate + i.ToString();

                    

                    sheet.Range[cellMainItemHeader].Value = propertyHeader;
                    sheet.Range[cellContactDate].Value = createdDate.ToString("MM-dd-yyyy");

                    string cellPeriodNameHeader = colPeriodNameHeader + i.ToString();
                    sheet.Range[cellPeriodNameHeader].Value = "Period";

                    string cellDurationHeader = colDurationHeader + i.ToString();
                    sheet.Range[cellDurationHeader].Value = "Duration";

                    string cellStartDate = colStartDate + i.ToString();
                    sheet.Range[cellStartDate].Value = "Start Date";

                    string cellEndDate = colEndDate + i.ToString();
                    sheet.Range[cellEndDate].Value = "End Date";

                    string cellDaysToExpire = colDaysToExpire + i.ToString();
                    sheet.Range[cellDaysToExpire].Value = "Days to Expire";

                    string cellNotes = colNotes + i.ToString();
                    sheet.Range[cellNotes].Value = "Notes";

                    sheet.Range[cellMainItemHeader + ":" + cellNotes].Style.Color = Color.LightBlue;
                    sheet.Range[cellMainItemHeader + ":" + cellNotes].Style.Font.IsBold = true;
                    

                    i = i + 1;
                    cellMainItemHeader = colMainItemHeader + i.ToString();
                    cellMainItemHeaderValue = colMainItemHeaderValue + i.ToString();


                    sheet.Range[cellMainItemHeader].Value = "Purchase Price:";
                    sheet.Range[cellMainItemHeaderValue].Value = purchasePrice;

                    i = i + 1;
                    cellMainItemHeader = colMainItemHeader + i.ToString();
                    cellMainItemHeaderValue = colMainItemHeaderValue + i.ToString();

                    sheet.Range[cellMainItemHeader].Value = "Earnest Money:";
                    sheet.Range[cellMainItemHeaderValue].Value = earnestMoney;

                    i = i + 1;
                    cellMainItemHeader = colMainItemHeader + i.ToString();
                    cellMainItemHeaderValue = colMainItemHeaderValue + i.ToString();

                    sheet.Range[cellMainItemHeader].Value = "1031 Exchange:";
                    sheet.Range[cellMainItemHeaderValue].Value = exchage1031;

                    i = i + 1;
                    cellMainItemHeader = colMainItemHeader + i.ToString();
                    cellMainItemHeaderValue = colMainItemHeaderValue + i.ToString();

                    sheet.Range[cellMainItemHeader].Value = "1031 Deadline:";
                    sheet.Range[cellMainItemHeaderValue].Value = deadline1031;

                    i = i + 1;
                    cellMainItemHeader = colMainItemHeader + i.ToString();
                    cellMainItemHeaderValue = colMainItemHeaderValue + i.ToString();

                    sheet.Range[cellMainItemHeader].Value = "Sellers:";
                    sheet.Range[cellMainItemHeaderValue].Value = sellers;

                    i = i + 1;
                    cellMainItemHeader = colMainItemHeader + i.ToString();
                    cellMainItemHeaderValue = colMainItemHeaderValue + i.ToString();

                    sheet.Range[cellMainItemHeader].Value = "Escrow Agent:";
                    sheet.Range[cellMainItemHeaderValue].Value = escrowAgent;

                    i = i + 1;
                    cellMainItemHeader = colMainItemHeader + i.ToString();
                    cellMainItemHeaderValue = colMainItemHeaderValue + i.ToString();

                    sheet.Range[cellMainItemHeader].Value = "Subdivision?:";
                    sheet.Range[cellMainItemHeaderValue].Value = subDivision;

                    i = i + 1;
                    cellMainItemHeader = colMainItemHeader + i.ToString();
                    cellMainItemHeaderValue = colMainItemHeaderValue + i.ToString();

                    sheet.Range[cellMainItemHeader].Value = "Real Estate Agent:";
                    sheet.Range[cellMainItemHeaderValue].Value = realEstateAgent;



                    int propertyId = reader.IsDBNull(reader.GetOrdinal("site_details_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("site_details_id"));
                    var periodList = GetPeriodList(propertyId, "");
                    
                    foreach (var period in periodList)
                    {
                        cellPeriodNameHeader = colPeriodNameHeader + j.ToString();
                        sheet.Range[cellPeriodNameHeader].Value = period.PeriodMaster;

                        cellDurationHeader = colDurationHeader + j.ToString();
                        sheet.Range[cellDurationHeader].Value = period.Duration.ToString();

                        cellStartDate = colStartDate + j.ToString();
                        sheet.Range[cellStartDate].Value = period.StartDate.ToString("MM-dd-yyyy");

                        cellEndDate = colEndDate + j.ToString();
                        sheet.Range[cellEndDate].Value = period.EndDate.ToString("MM-dd-yyyy");

                        cellDaysToExpire = colDaysToExpire + j.ToString();
                        sheet.Range[cellDaysToExpire].Value = period.DaysToExpire.ToString();

                        cellNotes = colNotes + j.ToString();
                        sheet.Range[cellNotes].Value = period.PeriodNotes;

                        j = j + 1;
                    }

                    if (i < j)
                    {
                        i = j;
                        
                    }

                    i = i + 3;
                    j = i + 1;
                }

                con.Close();
            }

            wrkBook.SaveToFile(fullToFileName);


            byte[] fileBytes = GetFile(fullToFileName);
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fullToFileName);

        }


        public ActionResult ExportDueDiligenceClosedAcquisition()
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

            //string fileName = Path.GetFileNameWithoutExtension(@"\\OpsVsAdp\\Files\\Daily\\TempHours.xlsx");
            var filePath = Path.Combine(webHostEnvironment.WebRootPath + @"/templates", "Due_Diligence_Closed_Acquisitions_Template.xlsx");

            string fullFileName = Path.Combine(webHostEnvironment.WebRootPath + @"/templates", "Due_Diligence_Closed_Acquisitions_Template.xlsx");
            string fullToFileName = Path.Combine(webHostEnvironment.WebRootPath + @"/templates", "Due_Diligence_Closed_Acquisitions.xlsx");

            Workbook wrkBook = new Workbook();
            wrkBook.LoadFromFile(fullFileName);
            Worksheet sheet = wrkBook.Worksheets[0];

            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("GetSurplusAcquisitionList", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("acquisition_status", 1);

                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                int i = 5;
                int j = 6;
                string colMainItemHeader = "A";
                string colMainItemHeaderValue = "B";
                string colContactDate = "C";
                string colPeriodNameHeader = "D";
                string colDurationHeader = "E";
                string colStartDate = "F";
                string colEndDate = "G";
                string colDaysToExpire = "H";
                string colNotes = "I";

                while (reader.Read())
                {


                    string propertyHeader = reader.IsDBNull(reader.GetOrdinal("property_header")) ? "" : reader.GetString(reader.GetOrdinal("property_header"));
                    string purchasePrice = reader.IsDBNull(reader.GetOrdinal("purchase_price")) ? "" : reader.GetString(reader.GetOrdinal("purchase_price"));
                    string earnestMoney = reader.IsDBNull(reader.GetOrdinal("earnest_money")) ? "" : reader.GetString(reader.GetOrdinal("earnest_money"));
                    string exchage1031 = reader.IsDBNull(reader.GetOrdinal("exchange_1031")) ? "" : reader.GetString(reader.GetOrdinal("exchange_1031"));
                    string deadline1031 = reader.IsDBNull(reader.GetOrdinal("dead_line_1031")) ? "" : reader.GetString(reader.GetOrdinal("dead_line_1031"));
                    string sellers = reader.IsDBNull(reader.GetOrdinal("sellers")) ? "" : reader.GetString(reader.GetOrdinal("sellers"));
                    string escrowAgent = reader.IsDBNull(reader.GetOrdinal("escrow_agent")) ? "" : reader.GetString(reader.GetOrdinal("escrow_agent"));
                    string subDivision = reader.IsDBNull(reader.GetOrdinal("sub_division")) ? "" : reader.GetString(reader.GetOrdinal("sub_division"));
                    string realEstateAgent = reader.IsDBNull(reader.GetOrdinal("real_estate_agent")) ? "" : reader.GetString(reader.GetOrdinal("real_estate_agent"));
                    DateTime createdDate = reader.IsDBNull(reader.GetOrdinal("created_date")) ? DateTime.Now : reader.GetDateTime(reader.GetOrdinal("created_date"));
                    DateTime closedDate = reader.IsDBNull(reader.GetOrdinal("closed_date")) ? DateTime.Now : reader.GetDateTime(reader.GetOrdinal("closed_date"));
                    DateTime terminatedDate = reader.IsDBNull(reader.GetOrdinal("terminated_date")) ? DateTime.Now : reader.GetDateTime(reader.GetOrdinal("terminated_date"));


                    string cellMainItemHeader = colMainItemHeader + i.ToString();
                    string cellMainItemHeaderValue = colMainItemHeaderValue + i.ToString();
                    string cellContactDate = colContactDate + i.ToString();



                    sheet.Range[cellMainItemHeader].Value = propertyHeader;
                    sheet.Range[cellContactDate].Value = createdDate.ToString("MM-dd-yyyy");

                    string cellPeriodNameHeader = colPeriodNameHeader + i.ToString();
                    sheet.Range[cellPeriodNameHeader].Value = "Period";

                    string cellDurationHeader = colDurationHeader + i.ToString();
                    sheet.Range[cellDurationHeader].Value = "Duration";

                    string cellStartDate = colStartDate + i.ToString();
                    sheet.Range[cellStartDate].Value = "Start Date";

                    string cellEndDate = colEndDate + i.ToString();
                    sheet.Range[cellEndDate].Value = "End Date";

                    string cellDaysToExpire = colDaysToExpire + i.ToString();
                    sheet.Range[cellDaysToExpire].Value = "Days to Expire";

                    string cellNotes = colNotes + i.ToString();
                    sheet.Range[cellNotes].Value = "Notes";

                    sheet.Range[cellMainItemHeader + ":" + cellNotes].Style.Color = Color.LightBlue;
                    sheet.Range[cellMainItemHeader + ":" + cellNotes].Style.Font.IsBold = true;


                    i = i + 1;
                    cellMainItemHeader = colMainItemHeader + i.ToString();
                    cellMainItemHeaderValue = colMainItemHeaderValue + i.ToString();


                    sheet.Range[cellMainItemHeader].Value = "Purchase Price:";
                    sheet.Range[cellMainItemHeaderValue].Value = purchasePrice;

                    i = i + 1;
                    cellMainItemHeader = colMainItemHeader + i.ToString();
                    cellMainItemHeaderValue = colMainItemHeaderValue + i.ToString();

                    sheet.Range[cellMainItemHeader].Value = "Earnest Money:";
                    sheet.Range[cellMainItemHeaderValue].Value = earnestMoney;

                    i = i + 1;
                    cellMainItemHeader = colMainItemHeader + i.ToString();
                    cellMainItemHeaderValue = colMainItemHeaderValue + i.ToString();

                    sheet.Range[cellMainItemHeader].Value = "1031 Exchange:";
                    sheet.Range[cellMainItemHeaderValue].Value = exchage1031;

                    i = i + 1;
                    cellMainItemHeader = colMainItemHeader + i.ToString();
                    cellMainItemHeaderValue = colMainItemHeaderValue + i.ToString();

                    sheet.Range[cellMainItemHeader].Value = "1031 Deadline:";
                    sheet.Range[cellMainItemHeaderValue].Value = deadline1031;

                    i = i + 1;
                    cellMainItemHeader = colMainItemHeader + i.ToString();
                    cellMainItemHeaderValue = colMainItemHeaderValue + i.ToString();

                    sheet.Range[cellMainItemHeader].Value = "Sellers:";
                    sheet.Range[cellMainItemHeaderValue].Value = sellers;

                    i = i + 1;
                    cellMainItemHeader = colMainItemHeader + i.ToString();
                    cellMainItemHeaderValue = colMainItemHeaderValue + i.ToString();

                    sheet.Range[cellMainItemHeader].Value = "Escrow Agent:";
                    sheet.Range[cellMainItemHeaderValue].Value = escrowAgent;

                    i = i + 1;
                    cellMainItemHeader = colMainItemHeader + i.ToString();
                    cellMainItemHeaderValue = colMainItemHeaderValue + i.ToString();

                    sheet.Range[cellMainItemHeader].Value = "Closed:";
                    sheet.Range[cellMainItemHeaderValue].Value = closedDate.ToString("MM-dd-yyyy");

                    



                    int propertyId = reader.IsDBNull(reader.GetOrdinal("site_details_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("site_details_id"));
                    var periodList = GetPeriodList(propertyId, "");

                    foreach (var period in periodList)
                    {
                        cellPeriodNameHeader = colPeriodNameHeader + j.ToString();
                        sheet.Range[cellPeriodNameHeader].Value = period.PeriodMaster;

                        cellDurationHeader = colDurationHeader + j.ToString();
                        sheet.Range[cellDurationHeader].Value = period.Duration.ToString();

                        cellStartDate = colStartDate + j.ToString();
                        sheet.Range[cellStartDate].Value = period.StartDate.ToString("MM-dd-yyyy");

                        cellEndDate = colEndDate + j.ToString();
                        sheet.Range[cellEndDate].Value = period.EndDate.ToString("MM-dd-yyyy");

                        cellDaysToExpire = colDaysToExpire + j.ToString();
                        sheet.Range[cellDaysToExpire].Value = period.DaysToExpire.ToString();

                        cellNotes = colNotes + j.ToString();
                        sheet.Range[cellNotes].Value = period.PeriodNotes;

                        j = j + 1;
                    }

                    if (i < j)
                    {
                        i = j;

                    }

                    i = i + 3;
                    j = i + 1;
                }

                con.Close();
            }

            wrkBook.SaveToFile(fullToFileName);


            byte[] fileBytes = GetFile(fullToFileName);
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fullToFileName);

        }


        public ActionResult ExportDueDiligenceTerminatedAcquisition()
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

            //string fileName = Path.GetFileNameWithoutExtension(@"\\OpsVsAdp\\Files\\Daily\\TempHours.xlsx");
            var filePath = Path.Combine(webHostEnvironment.WebRootPath + @"/templates", "Due_Diligence_Terminated_Acquisition_Template.xlsx");

            string fullFileName = Path.Combine(webHostEnvironment.WebRootPath + @"/templates", "Due_Diligence_Terminated_Acquisition_Template.xlsx");
            string fullToFileName = Path.Combine(webHostEnvironment.WebRootPath + @"/templates", "Due_Diligence_Terminated_Acquisition.xlsx");

            Workbook wrkBook = new Workbook();
            wrkBook.LoadFromFile(fullFileName);
            Worksheet sheet = wrkBook.Worksheets[0];

            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("GetSurplusAcquisitionList", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("acquisition_status", 2);

                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                int i = 5;
                int j = 6;
                string colMainItemHeader = "A";
                string colMainItemHeaderValue = "B";
                string colContactDate = "C";
                string colPeriodNameHeader = "D";
                string colDurationHeader = "E";
                string colStartDate = "F";
                string colEndDate = "G";
                string colDaysToExpire = "H";
                string colNotes = "I";

                while (reader.Read())
                {


                    string propertyHeader = reader.IsDBNull(reader.GetOrdinal("property_header")) ? "" : reader.GetString(reader.GetOrdinal("property_header"));
                    string purchasePrice = reader.IsDBNull(reader.GetOrdinal("purchase_price")) ? "" : reader.GetString(reader.GetOrdinal("purchase_price"));
                    string earnestMoney = reader.IsDBNull(reader.GetOrdinal("earnest_money")) ? "" : reader.GetString(reader.GetOrdinal("earnest_money"));
                    string exchage1031 = reader.IsDBNull(reader.GetOrdinal("exchange_1031")) ? "" : reader.GetString(reader.GetOrdinal("exchange_1031"));
                    string deadline1031 = reader.IsDBNull(reader.GetOrdinal("dead_line_1031")) ? "" : reader.GetString(reader.GetOrdinal("dead_line_1031"));
                    string sellers = reader.IsDBNull(reader.GetOrdinal("sellers")) ? "" : reader.GetString(reader.GetOrdinal("sellers"));
                    string escrowAgent = reader.IsDBNull(reader.GetOrdinal("escrow_agent")) ? "" : reader.GetString(reader.GetOrdinal("escrow_agent"));
                    string subDivision = reader.IsDBNull(reader.GetOrdinal("sub_division")) ? "" : reader.GetString(reader.GetOrdinal("sub_division"));
                    string realEstateAgent = reader.IsDBNull(reader.GetOrdinal("real_estate_agent")) ? "" : reader.GetString(reader.GetOrdinal("real_estate_agent"));
                    DateTime createdDate = reader.IsDBNull(reader.GetOrdinal("created_date")) ? DateTime.Now : reader.GetDateTime(reader.GetOrdinal("created_date"));
                    DateTime closedDate = reader.IsDBNull(reader.GetOrdinal("closed_date")) ? DateTime.Now : reader.GetDateTime(reader.GetOrdinal("closed_date"));
                    DateTime terminatedDate = reader.IsDBNull(reader.GetOrdinal("terminated_date")) ? DateTime.Now : reader.GetDateTime(reader.GetOrdinal("terminated_date"));


                    string cellMainItemHeader = colMainItemHeader + i.ToString();
                    string cellMainItemHeaderValue = colMainItemHeaderValue + i.ToString();
                    string cellContactDate = colContactDate + i.ToString();



                    sheet.Range[cellMainItemHeader].Value = propertyHeader;
                    sheet.Range[cellContactDate].Value = createdDate.ToString("MM-dd-yyyy");

                    string cellPeriodNameHeader = colPeriodNameHeader + i.ToString();
                    sheet.Range[cellPeriodNameHeader].Value = "Period";

                    string cellDurationHeader = colDurationHeader + i.ToString();
                    sheet.Range[cellDurationHeader].Value = "Duration";

                    string cellStartDate = colStartDate + i.ToString();
                    sheet.Range[cellStartDate].Value = "Start Date";

                    string cellEndDate = colEndDate + i.ToString();
                    sheet.Range[cellEndDate].Value = "End Date";

                    string cellDaysToExpire = colDaysToExpire + i.ToString();
                    sheet.Range[cellDaysToExpire].Value = "Days to Expire";

                    string cellNotes = colNotes + i.ToString();
                    sheet.Range[cellNotes].Value = "Notes";

                    sheet.Range[cellMainItemHeader + ":" + cellNotes].Style.Color = Color.LightBlue;
                    sheet.Range[cellMainItemHeader + ":" + cellNotes].Style.Font.IsBold = true;


                    i = i + 1;
                    cellMainItemHeader = colMainItemHeader + i.ToString();
                    cellMainItemHeaderValue = colMainItemHeaderValue + i.ToString();


                    sheet.Range[cellMainItemHeader].Value = "Purchase Price:";
                    sheet.Range[cellMainItemHeaderValue].Value = purchasePrice;

                    i = i + 1;
                    cellMainItemHeader = colMainItemHeader + i.ToString();
                    cellMainItemHeaderValue = colMainItemHeaderValue + i.ToString();

                    sheet.Range[cellMainItemHeader].Value = "Earnest Money:";
                    sheet.Range[cellMainItemHeaderValue].Value = earnestMoney;

                    i = i + 1;
                    cellMainItemHeader = colMainItemHeader + i.ToString();
                    cellMainItemHeaderValue = colMainItemHeaderValue + i.ToString();

                    sheet.Range[cellMainItemHeader].Value = "1031 Exchange:";
                    sheet.Range[cellMainItemHeaderValue].Value = exchage1031;

                    i = i + 1;
                    cellMainItemHeader = colMainItemHeader + i.ToString();
                    cellMainItemHeaderValue = colMainItemHeaderValue + i.ToString();

                    sheet.Range[cellMainItemHeader].Value = "1031 Deadline:";
                    sheet.Range[cellMainItemHeaderValue].Value = deadline1031;

                    i = i + 1;
                    cellMainItemHeader = colMainItemHeader + i.ToString();
                    cellMainItemHeaderValue = colMainItemHeaderValue + i.ToString();

                    sheet.Range[cellMainItemHeader].Value = "Sellers:";
                    sheet.Range[cellMainItemHeaderValue].Value = sellers;

                    i = i + 1;
                    cellMainItemHeader = colMainItemHeader + i.ToString();
                    cellMainItemHeaderValue = colMainItemHeaderValue + i.ToString();

                    sheet.Range[cellMainItemHeader].Value = "Escrow Agent:";
                    sheet.Range[cellMainItemHeaderValue].Value = escrowAgent;

                    i = i + 1;
                    cellMainItemHeader = colMainItemHeader + i.ToString();
                    cellMainItemHeaderValue = colMainItemHeaderValue + i.ToString();

                    sheet.Range[cellMainItemHeader].Value = "Terminated:";
                    sheet.Range[cellMainItemHeaderValue].Value = terminatedDate.ToString("MM-dd-yyyy");





                    int propertyId = reader.IsDBNull(reader.GetOrdinal("site_details_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("site_details_id"));
                    var periodList = GetPeriodList(propertyId, "");

                    foreach (var period in periodList)
                    {
                        cellPeriodNameHeader = colPeriodNameHeader + j.ToString();
                        sheet.Range[cellPeriodNameHeader].Value = period.PeriodMaster;

                        cellDurationHeader = colDurationHeader + j.ToString();
                        sheet.Range[cellDurationHeader].Value = period.Duration.ToString();

                        cellStartDate = colStartDate + j.ToString();
                        sheet.Range[cellStartDate].Value = period.StartDate.ToString("MM-dd-yyyy");

                        cellEndDate = colEndDate + j.ToString();
                        sheet.Range[cellEndDate].Value = period.EndDate.ToString("MM-dd-yyyy");

                        cellDaysToExpire = colDaysToExpire + j.ToString();
                        sheet.Range[cellDaysToExpire].Value = period.DaysToExpire.ToString();

                        cellNotes = colNotes + j.ToString();
                        sheet.Range[cellNotes].Value = period.PeriodNotes;

                        j = j + 1;
                    }

                    if (i < j)
                    {
                        i = j;

                    }

                    i = i + 3;
                    j = i + 1;
                }

                con.Close();
            }

            wrkBook.SaveToFile(fullToFileName);


            byte[] fileBytes = GetFile(fullToFileName);
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fullToFileName);

        }


        public ActionResult ExportDueDiligenceDisposition()
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

            //string fileName = Path.GetFileNameWithoutExtension(@"\\OpsVsAdp\\Files\\Daily\\TempHours.xlsx");
            var filePath = Path.Combine(webHostEnvironment.WebRootPath + @"/templates", "Due_Diligence_Dispositions_Template.xlsx");

            string fullFileName = Path.Combine(webHostEnvironment.WebRootPath + @"/templates", "Due_Diligence_Dispositions_Template.xlsx");
            string fullToFileName = Path.Combine(webHostEnvironment.WebRootPath + @"/templates", "Due_Diligence_Dispositions.xlsx");

            Workbook wrkBook = new Workbook();
            wrkBook.LoadFromFile(fullFileName);
            Worksheet sheet = wrkBook.Worksheets[0];

            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("GetSurplusDispositionList", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("disposition_status", 0);

                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                int i = 5;
                int j = 6;
                string colMainItemHeader = "A";
                string colMainItemHeaderValue = "B";
                string colContactDate = "C";
                string colPeriodNameHeader = "D";
                string colDurationHeader = "E";
                string colStartDate = "F";
                string colEndDate = "G";
                string colDaysToExpire = "H";
                string colNotes = "I";

                while (reader.Read())
                {


                    string propertyHeader = reader.IsDBNull(reader.GetOrdinal("property_header")) ? "" : reader.GetString(reader.GetOrdinal("property_header"));
                    string salePrice = reader.IsDBNull(reader.GetOrdinal("sale_price")) ? "" : reader.GetString(reader.GetOrdinal("sale_price"));
                    string earnestMoney = reader.IsDBNull(reader.GetOrdinal("earnest_money")) ? "" : reader.GetString(reader.GetOrdinal("earnest_money"));
                    string buyers = reader.IsDBNull(reader.GetOrdinal("buyers")) ? "" : reader.GetString(reader.GetOrdinal("buyers"));
                    string escrowAgent = reader.IsDBNull(reader.GetOrdinal("escrow_agent")) ? "" : reader.GetString(reader.GetOrdinal("escrow_agent"));
                    string buyersAttorney = reader.IsDBNull(reader.GetOrdinal("buyers_attorney")) ? "" : reader.GetString(reader.GetOrdinal("buyers_attorney"));
                    string optionsToExtend = reader.IsDBNull(reader.GetOrdinal("options_to_extend")) ? "" : reader.GetString(reader.GetOrdinal("options_to_extend"));
                    string commissions = reader.IsDBNull(reader.GetOrdinal("commissions")) ? "" : reader.GetString(reader.GetOrdinal("commissions"));
                    DateTime createdDate = reader.IsDBNull(reader.GetOrdinal("created_date")) ? DateTime.Now : reader.GetDateTime(reader.GetOrdinal("created_date"));
                    DateTime closedDate = reader.IsDBNull(reader.GetOrdinal("closed_date")) ? DateTime.Now : reader.GetDateTime(reader.GetOrdinal("closed_date"));
                    DateTime terminatedDate = reader.IsDBNull(reader.GetOrdinal("terminated_date")) ? DateTime.Now : reader.GetDateTime(reader.GetOrdinal("terminated_date"));


                    string cellMainItemHeader = colMainItemHeader + i.ToString();
                    string cellMainItemHeaderValue = colMainItemHeaderValue + i.ToString();
                    string cellContactDate = colContactDate + i.ToString();



                    sheet.Range[cellMainItemHeader].Value = propertyHeader;
                    sheet.Range[cellContactDate].Value = createdDate.ToString("MM-dd-yyyy");

                    string cellPeriodNameHeader = colPeriodNameHeader + i.ToString();
                    sheet.Range[cellPeriodNameHeader].Value = "Period";

                    string cellDurationHeader = colDurationHeader + i.ToString();
                    sheet.Range[cellDurationHeader].Value = "Duration";

                    string cellStartDate = colStartDate + i.ToString();
                    sheet.Range[cellStartDate].Value = "Start Date";

                    string cellEndDate = colEndDate + i.ToString();
                    sheet.Range[cellEndDate].Value = "End Date";

                    string cellDaysToExpire = colDaysToExpire + i.ToString();
                    sheet.Range[cellDaysToExpire].Value = "Days to Expire";

                    string cellNotes = colNotes + i.ToString();
                    sheet.Range[cellNotes].Value = "Notes";

                    sheet.Range[cellMainItemHeader + ":" + cellNotes].Style.Color = Color.LightBlue;
                    sheet.Range[cellMainItemHeader + ":" + cellNotes].Style.Font.IsBold = true;


                    i = i + 1;
                    cellMainItemHeader = colMainItemHeader + i.ToString();
                    cellMainItemHeaderValue = colMainItemHeaderValue + i.ToString();


                    sheet.Range[cellMainItemHeader].Value = "Sale Price:";
                    sheet.Range[cellMainItemHeaderValue].Value = salePrice;

                    i = i + 1;
                    cellMainItemHeader = colMainItemHeader + i.ToString();
                    cellMainItemHeaderValue = colMainItemHeaderValue + i.ToString();

                    sheet.Range[cellMainItemHeader].Value = "Earnest Money:";
                    sheet.Range[cellMainItemHeaderValue].Value = earnestMoney;

                    i = i + 1;
                    cellMainItemHeader = colMainItemHeader + i.ToString();
                    cellMainItemHeaderValue = colMainItemHeaderValue + i.ToString();

                    sheet.Range[cellMainItemHeader].Value = "Buyers:";
                    sheet.Range[cellMainItemHeaderValue].Value = buyers;

                    i = i + 1;
                    cellMainItemHeader = colMainItemHeader + i.ToString();
                    cellMainItemHeaderValue = colMainItemHeaderValue + i.ToString();

                    sheet.Range[cellMainItemHeader].Value = "Escrow Agent:";
                    sheet.Range[cellMainItemHeaderValue].Value = escrowAgent;

                    i = i + 1;
                    cellMainItemHeader = colMainItemHeader + i.ToString();
                    cellMainItemHeaderValue = colMainItemHeaderValue + i.ToString();

                    sheet.Range[cellMainItemHeader].Value = "Buyers Attorney:";
                    sheet.Range[cellMainItemHeaderValue].Value = buyersAttorney;

                    i = i + 1;
                    cellMainItemHeader = colMainItemHeader + i.ToString();
                    cellMainItemHeaderValue = colMainItemHeaderValue + i.ToString();

                    sheet.Range[cellMainItemHeader].Value = "Options to Extend:";
                    sheet.Range[cellMainItemHeaderValue].Value = optionsToExtend;

                    i = i + 1;
                    cellMainItemHeader = colMainItemHeader + i.ToString();
                    cellMainItemHeaderValue = colMainItemHeaderValue + i.ToString();

                    sheet.Range[cellMainItemHeader].Value = "Commissions:";
                    sheet.Range[cellMainItemHeaderValue].Value = commissions;




                    int propertyId = reader.IsDBNull(reader.GetOrdinal("site_details_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("site_details_id"));
                    var periodList = GetPeriodList(propertyId, "");

                    foreach (var period in periodList)
                    {
                        cellPeriodNameHeader = colPeriodNameHeader + j.ToString();
                        sheet.Range[cellPeriodNameHeader].Value = period.PeriodMaster;

                        cellDurationHeader = colDurationHeader + j.ToString();
                        sheet.Range[cellDurationHeader].Value = period.Duration.ToString();

                        cellStartDate = colStartDate + j.ToString();
                        sheet.Range[cellStartDate].Value = period.StartDate.ToString("MM-dd-yyyy");

                        cellEndDate = colEndDate + j.ToString();
                        sheet.Range[cellEndDate].Value = period.EndDate.ToString("MM-dd-yyyy");

                        cellDaysToExpire = colDaysToExpire + j.ToString();
                        sheet.Range[cellDaysToExpire].Value = period.DaysToExpire.ToString();

                        cellNotes = colNotes + j.ToString();
                        sheet.Range[cellNotes].Value = period.PeriodNotes;

                        j = j + 1;
                    }

                    if (i < j)
                    {
                        i = j;

                    }

                    i = i + 3;
                    j = i + 1;
                }

                con.Close();
            }

            wrkBook.SaveToFile(fullToFileName);


            byte[] fileBytes = GetFile(fullToFileName);
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fullToFileName);

        }


        public ActionResult ExportDueDiligenceClosedDisposition()
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

            //string fileName = Path.GetFileNameWithoutExtension(@"\\OpsVsAdp\\Files\\Daily\\TempHours.xlsx");
            var filePath = Path.Combine(webHostEnvironment.WebRootPath + @"/templates", "Due_Diligence_Closed_Dispositions_Template.xlsx");

            string fullFileName = Path.Combine(webHostEnvironment.WebRootPath + @"/templates", "Due_Diligence_Closed_Dispositions_Template.xlsx");
            string fullToFileName = Path.Combine(webHostEnvironment.WebRootPath + @"/templates", "Due_Diligence_Closed_Dispositions.xlsx");

            Workbook wrkBook = new Workbook();
            wrkBook.LoadFromFile(fullFileName);
            Worksheet sheet = wrkBook.Worksheets[0];

            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("GetSurplusDispositionList", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("disposition_status", 1);

                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                int i = 5;
                int j = 6;
                string colMainItemHeader = "A";
                string colMainItemHeaderValue = "B";
                string colContactDate = "C";
                string colPeriodNameHeader = "D";
                string colDurationHeader = "E";
                string colStartDate = "F";
                string colEndDate = "G";
                string colDaysToExpire = "H";
                string colNotes = "I";

                while (reader.Read())
                {


                    string propertyHeader = reader.IsDBNull(reader.GetOrdinal("property_header")) ? "" : reader.GetString(reader.GetOrdinal("property_header"));
                    string salePrice = reader.IsDBNull(reader.GetOrdinal("sale_price")) ? "" : reader.GetString(reader.GetOrdinal("sale_price"));
                    string earnestMoney = reader.IsDBNull(reader.GetOrdinal("earnest_money")) ? "" : reader.GetString(reader.GetOrdinal("earnest_money"));
                    string buyers = reader.IsDBNull(reader.GetOrdinal("buyers")) ? "" : reader.GetString(reader.GetOrdinal("buyers"));
                    string escrowAgent = reader.IsDBNull(reader.GetOrdinal("escrow_agent")) ? "" : reader.GetString(reader.GetOrdinal("escrow_agent"));
                    string buyersAttorney = reader.IsDBNull(reader.GetOrdinal("buyers_attorney")) ? "" : reader.GetString(reader.GetOrdinal("buyers_attorney"));
                    string optionsToExtend = reader.IsDBNull(reader.GetOrdinal("options_to_extend")) ? "" : reader.GetString(reader.GetOrdinal("options_to_extend"));
                    string commissions = reader.IsDBNull(reader.GetOrdinal("commissions")) ? "" : reader.GetString(reader.GetOrdinal("commissions"));
                    DateTime createdDate = reader.IsDBNull(reader.GetOrdinal("created_date")) ? DateTime.Now : reader.GetDateTime(reader.GetOrdinal("created_date"));
                    DateTime closedDate = reader.IsDBNull(reader.GetOrdinal("closed_date")) ? DateTime.Now : reader.GetDateTime(reader.GetOrdinal("closed_date"));
                    DateTime terminatedDate = reader.IsDBNull(reader.GetOrdinal("terminated_date")) ? DateTime.Now : reader.GetDateTime(reader.GetOrdinal("terminated_date"));


                    string cellMainItemHeader = colMainItemHeader + i.ToString();
                    string cellMainItemHeaderValue = colMainItemHeaderValue + i.ToString();
                    string cellContactDate = colContactDate + i.ToString();



                    sheet.Range[cellMainItemHeader].Value = propertyHeader;
                    sheet.Range[cellContactDate].Value = createdDate.ToString("MM-dd-yyyy");

                    string cellPeriodNameHeader = colPeriodNameHeader + i.ToString();
                    sheet.Range[cellPeriodNameHeader].Value = "Period";

                    string cellDurationHeader = colDurationHeader + i.ToString();
                    sheet.Range[cellDurationHeader].Value = "Duration";

                    string cellStartDate = colStartDate + i.ToString();
                    sheet.Range[cellStartDate].Value = "Start Date";

                    string cellEndDate = colEndDate + i.ToString();
                    sheet.Range[cellEndDate].Value = "End Date";

                    string cellDaysToExpire = colDaysToExpire + i.ToString();
                    sheet.Range[cellDaysToExpire].Value = "Days to Expire";

                    string cellNotes = colNotes + i.ToString();
                    sheet.Range[cellNotes].Value = "Notes";

                    sheet.Range[cellMainItemHeader + ":" + cellNotes].Style.Color = Color.LightBlue;
                    sheet.Range[cellMainItemHeader + ":" + cellNotes].Style.Font.IsBold = true;


                    i = i + 1;
                    cellMainItemHeader = colMainItemHeader + i.ToString();
                    cellMainItemHeaderValue = colMainItemHeaderValue + i.ToString();


                    sheet.Range[cellMainItemHeader].Value = "Sale Price:";
                    sheet.Range[cellMainItemHeaderValue].Value = salePrice;

                    i = i + 1;
                    cellMainItemHeader = colMainItemHeader + i.ToString();
                    cellMainItemHeaderValue = colMainItemHeaderValue + i.ToString();

                    sheet.Range[cellMainItemHeader].Value = "Earnest Money:";
                    sheet.Range[cellMainItemHeaderValue].Value = earnestMoney;

                    i = i + 1;
                    cellMainItemHeader = colMainItemHeader + i.ToString();
                    cellMainItemHeaderValue = colMainItemHeaderValue + i.ToString();

                    sheet.Range[cellMainItemHeader].Value = "Buyers:";
                    sheet.Range[cellMainItemHeaderValue].Value = buyers;

                    i = i + 1;
                    cellMainItemHeader = colMainItemHeader + i.ToString();
                    cellMainItemHeaderValue = colMainItemHeaderValue + i.ToString();

                    sheet.Range[cellMainItemHeader].Value = "Escrow Agent:";
                    sheet.Range[cellMainItemHeaderValue].Value = escrowAgent;

                    i = i + 1;
                    cellMainItemHeader = colMainItemHeader + i.ToString();
                    cellMainItemHeaderValue = colMainItemHeaderValue + i.ToString();

                    sheet.Range[cellMainItemHeader].Value = "Buyers Attorney:";
                    sheet.Range[cellMainItemHeaderValue].Value = buyersAttorney;

                    i = i + 1;
                    cellMainItemHeader = colMainItemHeader + i.ToString();
                    cellMainItemHeaderValue = colMainItemHeaderValue + i.ToString();

                    sheet.Range[cellMainItemHeader].Value = "Options to Extend:";
                    sheet.Range[cellMainItemHeaderValue].Value = optionsToExtend;

                    i = i + 1;
                    cellMainItemHeader = colMainItemHeader + i.ToString();
                    cellMainItemHeaderValue = colMainItemHeaderValue + i.ToString();

                    sheet.Range[cellMainItemHeader].Value = "Closed:";
                    sheet.Range[cellMainItemHeaderValue].Value = closedDate.ToString("MM-dd-yyyy");




                    int propertyId = reader.IsDBNull(reader.GetOrdinal("site_details_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("site_details_id"));
                    var periodList = GetPeriodList(propertyId, "");

                    foreach (var period in periodList)
                    {
                        cellPeriodNameHeader = colPeriodNameHeader + j.ToString();
                        sheet.Range[cellPeriodNameHeader].Value = period.PeriodMaster;

                        cellDurationHeader = colDurationHeader + j.ToString();
                        sheet.Range[cellDurationHeader].Value = period.Duration.ToString();

                        cellStartDate = colStartDate + j.ToString();
                        sheet.Range[cellStartDate].Value = period.StartDate.ToString("MM-dd-yyyy");

                        cellEndDate = colEndDate + j.ToString();
                        sheet.Range[cellEndDate].Value = period.EndDate.ToString("MM-dd-yyyy");

                        cellDaysToExpire = colDaysToExpire + j.ToString();
                        sheet.Range[cellDaysToExpire].Value = period.DaysToExpire.ToString();

                        cellNotes = colNotes + j.ToString();
                        sheet.Range[cellNotes].Value = period.PeriodNotes;

                        j = j + 1;
                    }

                    if (i < j)
                    {
                        i = j;

                    }

                    i = i + 3;
                    j = i + 1;
                }

                con.Close();
            }

            wrkBook.SaveToFile(fullToFileName);


            byte[] fileBytes = GetFile(fullToFileName);
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fullToFileName);

        }

        public ActionResult ExportDueDiligenceTerminatedDisposition()
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

            //string fileName = Path.GetFileNameWithoutExtension(@"\\OpsVsAdp\\Files\\Daily\\TempHours.xlsx");
            var filePath = Path.Combine(webHostEnvironment.WebRootPath + @"/templates", "Due_Diligence_Terminated_Dispositions_Template.xlsx");

            string fullFileName = Path.Combine(webHostEnvironment.WebRootPath + @"/templates", "Due_Diligence_Terminated_Dispositions_Template.xlsx");
            string fullToFileName = Path.Combine(webHostEnvironment.WebRootPath + @"/templates", "Due_Diligence_Terminated_Dispositions.xlsx");

            Workbook wrkBook = new Workbook();
            wrkBook.LoadFromFile(fullFileName);
            Worksheet sheet = wrkBook.Worksheets[0];

            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("GetSurplusDispositionList", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("disposition_status", 1);

                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                int i = 5;
                int j = 6;
                string colMainItemHeader = "A";
                string colMainItemHeaderValue = "B";
                string colContactDate = "C";
                string colPeriodNameHeader = "D";
                string colDurationHeader = "E";
                string colStartDate = "F";
                string colEndDate = "G";
                string colDaysToExpire = "H";
                string colNotes = "I";

                while (reader.Read())
                {


                    string propertyHeader = reader.IsDBNull(reader.GetOrdinal("property_header")) ? "" : reader.GetString(reader.GetOrdinal("property_header"));
                    string salePrice = reader.IsDBNull(reader.GetOrdinal("sale_price")) ? "" : reader.GetString(reader.GetOrdinal("sale_price"));
                    string earnestMoney = reader.IsDBNull(reader.GetOrdinal("earnest_money")) ? "" : reader.GetString(reader.GetOrdinal("earnest_money"));
                    string buyers = reader.IsDBNull(reader.GetOrdinal("buyers")) ? "" : reader.GetString(reader.GetOrdinal("buyers"));
                    string escrowAgent = reader.IsDBNull(reader.GetOrdinal("escrow_agent")) ? "" : reader.GetString(reader.GetOrdinal("escrow_agent"));
                    string buyersAttorney = reader.IsDBNull(reader.GetOrdinal("buyers_attorney")) ? "" : reader.GetString(reader.GetOrdinal("buyers_attorney"));
                    string optionsToExtend = reader.IsDBNull(reader.GetOrdinal("options_to_extend")) ? "" : reader.GetString(reader.GetOrdinal("options_to_extend"));
                    string commissions = reader.IsDBNull(reader.GetOrdinal("commissions")) ? "" : reader.GetString(reader.GetOrdinal("commissions"));
                    DateTime createdDate = reader.IsDBNull(reader.GetOrdinal("created_date")) ? DateTime.Now : reader.GetDateTime(reader.GetOrdinal("created_date"));
                    DateTime closedDate = reader.IsDBNull(reader.GetOrdinal("closed_date")) ? DateTime.Now : reader.GetDateTime(reader.GetOrdinal("closed_date"));
                    DateTime terminatedDate = reader.IsDBNull(reader.GetOrdinal("terminated_date")) ? DateTime.Now : reader.GetDateTime(reader.GetOrdinal("terminated_date"));


                    string cellMainItemHeader = colMainItemHeader + i.ToString();
                    string cellMainItemHeaderValue = colMainItemHeaderValue + i.ToString();
                    string cellContactDate = colContactDate + i.ToString();



                    sheet.Range[cellMainItemHeader].Value = propertyHeader;
                    sheet.Range[cellContactDate].Value = createdDate.ToString("MM-dd-yyyy");

                    string cellPeriodNameHeader = colPeriodNameHeader + i.ToString();
                    sheet.Range[cellPeriodNameHeader].Value = "Period";

                    string cellDurationHeader = colDurationHeader + i.ToString();
                    sheet.Range[cellDurationHeader].Value = "Duration";

                    string cellStartDate = colStartDate + i.ToString();
                    sheet.Range[cellStartDate].Value = "Start Date";

                    string cellEndDate = colEndDate + i.ToString();
                    sheet.Range[cellEndDate].Value = "End Date";

                    string cellDaysToExpire = colDaysToExpire + i.ToString();
                    sheet.Range[cellDaysToExpire].Value = "Days to Expire";

                    string cellNotes = colNotes + i.ToString();
                    sheet.Range[cellNotes].Value = "Notes";

                    sheet.Range[cellMainItemHeader + ":" + cellNotes].Style.Color = Color.LightBlue;
                    sheet.Range[cellMainItemHeader + ":" + cellNotes].Style.Font.IsBold = true;


                    i = i + 1;
                    cellMainItemHeader = colMainItemHeader + i.ToString();
                    cellMainItemHeaderValue = colMainItemHeaderValue + i.ToString();


                    sheet.Range[cellMainItemHeader].Value = "Sale Price:";
                    sheet.Range[cellMainItemHeaderValue].Value = salePrice;

                    i = i + 1;
                    cellMainItemHeader = colMainItemHeader + i.ToString();
                    cellMainItemHeaderValue = colMainItemHeaderValue + i.ToString();

                    sheet.Range[cellMainItemHeader].Value = "Earnest Money:";
                    sheet.Range[cellMainItemHeaderValue].Value = earnestMoney;

                    i = i + 1;
                    cellMainItemHeader = colMainItemHeader + i.ToString();
                    cellMainItemHeaderValue = colMainItemHeaderValue + i.ToString();

                    sheet.Range[cellMainItemHeader].Value = "Buyers:";
                    sheet.Range[cellMainItemHeaderValue].Value = buyers;

                    i = i + 1;
                    cellMainItemHeader = colMainItemHeader + i.ToString();
                    cellMainItemHeaderValue = colMainItemHeaderValue + i.ToString();

                    sheet.Range[cellMainItemHeader].Value = "Escrow Agent:";
                    sheet.Range[cellMainItemHeaderValue].Value = escrowAgent;

                    i = i + 1;
                    cellMainItemHeader = colMainItemHeader + i.ToString();
                    cellMainItemHeaderValue = colMainItemHeaderValue + i.ToString();

                    sheet.Range[cellMainItemHeader].Value = "Buyers Attorney:";
                    sheet.Range[cellMainItemHeaderValue].Value = buyersAttorney;

                    i = i + 1;
                    cellMainItemHeader = colMainItemHeader + i.ToString();
                    cellMainItemHeaderValue = colMainItemHeaderValue + i.ToString();

                    sheet.Range[cellMainItemHeader].Value = "Options to Extend:";
                    sheet.Range[cellMainItemHeaderValue].Value = optionsToExtend;

                    i = i + 1;
                    cellMainItemHeader = colMainItemHeader + i.ToString();
                    cellMainItemHeaderValue = colMainItemHeaderValue + i.ToString();

                    sheet.Range[cellMainItemHeader].Value = "Terminated:";
                    sheet.Range[cellMainItemHeaderValue].Value = terminatedDate.ToString("MM-dd-yyyy");




                    int propertyId = reader.IsDBNull(reader.GetOrdinal("site_details_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("site_details_id"));
                    var periodList = GetPeriodList(propertyId, "");

                    foreach (var period in periodList)
                    {
                        cellPeriodNameHeader = colPeriodNameHeader + j.ToString();
                        sheet.Range[cellPeriodNameHeader].Value = period.PeriodMaster;

                        cellDurationHeader = colDurationHeader + j.ToString();
                        sheet.Range[cellDurationHeader].Value = period.Duration.ToString();

                        cellStartDate = colStartDate + j.ToString();
                        sheet.Range[cellStartDate].Value = period.StartDate.ToString("MM-dd-yyyy");

                        cellEndDate = colEndDate + j.ToString();
                        sheet.Range[cellEndDate].Value = period.EndDate.ToString("MM-dd-yyyy");

                        cellDaysToExpire = colDaysToExpire + j.ToString();
                        sheet.Range[cellDaysToExpire].Value = period.DaysToExpire.ToString();

                        cellNotes = colNotes + j.ToString();
                        sheet.Range[cellNotes].Value = period.PeriodNotes;

                        j = j + 1;
                    }

                    if (i < j)
                    {
                        i = j;

                    }

                    i = i + 3;
                    j = i + 1;
                }

                con.Close();
            }

            wrkBook.SaveToFile(fullToFileName);


            byte[] fileBytes = GetFile(fullToFileName);
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fullToFileName);

        }


        public ActionResult ExportDueDiligenceLease()
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

            //string fileName = Path.GetFileNameWithoutExtension(@"\\OpsVsAdp\\Files\\Daily\\TempHours.xlsx");
            var filePath = Path.Combine(webHostEnvironment.WebRootPath + @"/templates", "Due_Diligence_Lease_Template.xlsx");

            string fullFileName = Path.Combine(webHostEnvironment.WebRootPath + @"/templates", "Due_Diligence_Lease_Template.xlsx");
            string fullToFileName = Path.Combine(webHostEnvironment.WebRootPath + @"/templates", "Due_Diligence_Lease.xlsx");

            Workbook wrkBook = new Workbook();
            wrkBook.LoadFromFile(fullFileName);
            Worksheet sheet = wrkBook.Worksheets[0];

            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("GetSurplusLeaseList", con);
                cmd.CommandType = CommandType.StoredProcedure;

                //cmd.Parameters.AddWithValue("asset_status", 0);

                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                int i = 5;
                int j = 6;
                string colMainItemHeader = "A";
                string colMainItemHeaderValue = "B";
                string colContactDate = "C";
                string colPeriodNameHeader = "D";
                string colDurationHeader = "E";
                string colStartDate = "F";
                string colEndDate = "G";
                string colDaysToExpire = "H";
                string colNotes = "I";

                while (reader.Read())
                {


                    string propertyHeader = reader.IsDBNull(reader.GetOrdinal("property_header")) ? "" : reader.GetString(reader.GetOrdinal("property_header"));
                    string tenantName = reader.IsDBNull(reader.GetOrdinal("tenant_name")) ? "" : reader.GetString(reader.GetOrdinal("tenant_name"));

                    DateTime createdDate = reader.IsDBNull(reader.GetOrdinal("created_date")) ? DateTime.Now : reader.GetDateTime(reader.GetOrdinal("created_date"));

                    

                    string cellMainItemHeader = colMainItemHeader + i.ToString();
                    string cellMainItemHeaderValue = colMainItemHeaderValue + i.ToString();
                    string cellContactDate = colContactDate + i.ToString();



                    sheet.Range[cellMainItemHeader].Value = propertyHeader;
                    sheet.Range[cellContactDate].Value = createdDate.ToString("MM-dd-yyyy");

                    string cellPeriodNameHeader = colPeriodNameHeader + i.ToString();
                    sheet.Range[cellPeriodNameHeader].Value = "Period";

                    string cellDurationHeader = colDurationHeader + i.ToString();
                    sheet.Range[cellDurationHeader].Value = "Duration";

                    string cellStartDate = colStartDate + i.ToString();
                    sheet.Range[cellStartDate].Value = "Start Date";

                    string cellEndDate = colEndDate + i.ToString();
                    sheet.Range[cellEndDate].Value = "End Date";

                    string cellDaysToExpire = colDaysToExpire + i.ToString();
                    sheet.Range[cellDaysToExpire].Value = "Days to Expire";

                    string cellNotes = colNotes + i.ToString();
                    sheet.Range[cellNotes].Value = "Notes";

                    sheet.Range[cellMainItemHeader + ":" + cellNotes].Style.Color = Color.LightBlue;
                    sheet.Range[cellMainItemHeader + ":" + cellNotes].Style.Font.IsBold = true;


                    i = i + 1;
                    cellMainItemHeader = colMainItemHeader + i.ToString();
                    cellMainItemHeaderValue = colMainItemHeaderValue + i.ToString();


                    sheet.Range[cellMainItemHeader].Value = "Tenant:";
                    sheet.Range[cellMainItemHeaderValue].Value = tenantName;




                    int propertyId = reader.IsDBNull(reader.GetOrdinal("site_details_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("site_details_id"));
                    var periodList = GetPeriodList(propertyId, "");

                    foreach (var period in periodList)
                    {
                        cellPeriodNameHeader = colPeriodNameHeader + j.ToString();
                        sheet.Range[cellPeriodNameHeader].Value = period.PeriodMaster;

                        cellDurationHeader = colDurationHeader + j.ToString();
                        sheet.Range[cellDurationHeader].Value = period.Duration.ToString();

                        cellStartDate = colStartDate + j.ToString();
                        sheet.Range[cellStartDate].Value = period.StartDate.ToString("MM-dd-yyyy");

                        cellEndDate = colEndDate + j.ToString();
                        sheet.Range[cellEndDate].Value = period.EndDate.ToString("MM-dd-yyyy");

                        cellDaysToExpire = colDaysToExpire + j.ToString();
                        sheet.Range[cellDaysToExpire].Value = period.DaysToExpire.ToString();

                        cellNotes = colNotes + j.ToString();
                        sheet.Range[cellNotes].Value = period.PeriodNotes;

                        j = j + 1;
                    }

                    if (i < j)
                    {
                        i = j;

                    }

                    i = i + 3;
                    j = i + 1;
                }

                con.Close();
            }

            wrkBook.SaveToFile(fullToFileName);


            byte[] fileBytes = GetFile(fullToFileName);
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fullToFileName);

        }


        [HttpPost]
        public string GetPropertyIdByAssetId(string assetId)
        {
            string propertyId = "0";
            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("GetPropertyIdByAssetId", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("asset_id", assetId);
                cmd.Parameters.AddWithValue("propertyType", SamsPropertyType.Surplus);
                con.Open();

                SqlDataReader readerAssetType = cmd.ExecuteReader();
                while (readerAssetType.Read())
                {
                    int pId = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("propertyId")) ? 0 : readerAssetType.GetInt32(readerAssetType.GetOrdinal("propertyId"));
                    propertyId = pId.ToString();
                }
            }

            return propertyId;
        }



        public IActionResult GetUnderContractProperties()
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

            List<SiteDetails> surplusPropertiesList = new List<SiteDetails>();

            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {

                /*
                 * 1	Available
                 * 2	Under contract
                 * 3	Sold
                 */
                /*
               SqlCommand cmd = new SqlCommand("GetPropertyListByStatus", con);
               cmd.CommandType = CommandType.StoredProcedure;
               cmd.Parameters.AddWithValue("property_status_id", 2);
               */

                SqlCommand cmd = new SqlCommand("GetPropertyListByCategory", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("asset_status", 0);

                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var steDetails = new SiteDetails();
                    steDetails.SiteDetailsId = reader.IsDBNull(reader.GetOrdinal("site_details_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("site_details_id"));
                    steDetails.NamePrefix = reader.IsDBNull(reader.GetOrdinal("name_prefix")) ? "" : reader.GetString(reader.GetOrdinal("name_prefix"));
                    steDetails.FirstName = reader.IsDBNull(reader.GetOrdinal("first_name")) ? "" : reader.GetString(reader.GetOrdinal("first_name"));

                    steDetails.LastName = reader.IsDBNull(reader.GetOrdinal("last_name")) ? "" : reader.GetString(reader.GetOrdinal("last_name"));
                    steDetails.CompanyName = reader.IsDBNull(reader.GetOrdinal("company_name")) ? "" : reader.GetString(reader.GetOrdinal("company_name"));
                    steDetails.EmailAddress = reader.IsDBNull(reader.GetOrdinal("email_address")) ? "" : reader.GetString(reader.GetOrdinal("email_address"));
                    steDetails.Address = reader.IsDBNull(reader.GetOrdinal("address")) ? "" : reader.GetString(reader.GetOrdinal("address"));
                    steDetails.CityName = reader.IsDBNull(reader.GetOrdinal("city_name")) ? "" : reader.GetString(reader.GetOrdinal("city_name"));
                    steDetails.StateId = reader.IsDBNull(reader.GetOrdinal("state_id")) ? "" : reader.GetString(reader.GetOrdinal("state_id"));

                    steDetails.StateName = reader.IsDBNull(reader.GetOrdinal("state_name")) ? "" : reader.GetString(reader.GetOrdinal("state_name"));

                    steDetails.ZipCode = reader.IsDBNull(reader.GetOrdinal("zip_code")) ? "" : reader.GetString(reader.GetOrdinal("zip_code"));
                    steDetails.ContactNumber = reader.IsDBNull(reader.GetOrdinal("contact_number")) ? "" : reader.GetString(reader.GetOrdinal("contact_number"));
                    steDetails.SamsHoldingEmployee = reader.IsDBNull(reader.GetOrdinal("sams_holding_employee")) ? false : reader.GetBoolean(reader.GetOrdinal("sams_holding_employee"));
                    steDetails.MarketId = reader.IsDBNull(reader.GetOrdinal("market_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("market_id"));
                    steDetails.PropertyHeader = reader.IsDBNull(reader.GetOrdinal("property_header")) ? "" : reader.GetString(reader.GetOrdinal("property_header"));
                    steDetails.SiteAddress = reader.IsDBNull(reader.GetOrdinal("site_address")) ? "" : reader.GetString(reader.GetOrdinal("site_address"));
                    steDetails.SiteCity = reader.IsDBNull(reader.GetOrdinal("site_city")) ? "" : reader.GetString(reader.GetOrdinal("site_city"));
                    steDetails.SiteStateId = reader.IsDBNull(reader.GetOrdinal("site_state_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("site_state_id"));

                    steDetails.SiteStateName = reader.IsDBNull(reader.GetOrdinal("site_state_name")) ? "" : reader.GetString(reader.GetOrdinal("site_state_name"));

                    steDetails.SiteCounty = reader.IsDBNull(reader.GetOrdinal("site_county")) ? "" : reader.GetString(reader.GetOrdinal("site_county"));
                    steDetails.SiteCrossStreetName = reader.IsDBNull(reader.GetOrdinal("site_cross_street_name")) ? "" : reader.GetString(reader.GetOrdinal("site_cross_street_name"));
                    steDetails.IsPropertyAvailable = reader.IsDBNull(reader.GetOrdinal("is_property_available")) ? true : reader.GetBoolean(reader.GetOrdinal("is_property_available"));
                    steDetails.Zoning = reader.IsDBNull(reader.GetOrdinal("zoning")) ? "" : reader.GetString(reader.GetOrdinal("zoning"));
                    steDetails.LotSize = reader.IsDBNull(reader.GetOrdinal("lot_size")) ? "" : reader.GetString(reader.GetOrdinal("lot_size"));

                    steDetails.SalesPrice = reader.IsDBNull(reader.GetOrdinal("sales_price")) ? "" : reader.GetString(reader.GetOrdinal("sales_price"));
                    //steDetails.SalesPrice = Helper.FormatCurrency("$", steDetails.SalesPrice);

                    steDetails.Comments = reader.IsDBNull(reader.GetOrdinal("comments")) ? "" : reader.GetString(reader.GetOrdinal("comments"));

                    steDetails.CreatedDate = reader.IsDBNull(reader.GetOrdinal("created_date")) ? DateTime.Now : reader.GetDateTime(reader.GetOrdinal("created_date"));
                    steDetails.PropertyType = reader.IsDBNull(reader.GetOrdinal("property_type")) ? 0 : reader.GetInt32(reader.GetOrdinal("property_type"));

                    steDetails.ImageName = reader.IsDBNull(reader.GetOrdinal("image_name")) ? "" : reader.GetString(reader.GetOrdinal("image_name"));

                    steDetails.AssetTypeId = reader.IsDBNull(reader.GetOrdinal("asset_type_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("asset_type_id"));
                    steDetails.AssetTypeName = reader.IsDBNull(reader.GetOrdinal("asset_type_name")) ? "" : reader.GetString(reader.GetOrdinal("asset_type_name"));
                    steDetails.AssetStatus = reader.IsDBNull(reader.GetOrdinal("asset_status")) ? 0 : reader.GetInt32(reader.GetOrdinal("asset_status"));
                    steDetails.AssetId = reader.IsDBNull(reader.GetOrdinal("asset_id")) ? "" : reader.GetString(reader.GetOrdinal("asset_id"));

                    steDetails.SelectedPropertyStatusId = reader.IsDBNull(reader.GetOrdinal("property_status_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("property_status_id"));
                    steDetails.SelectedPropertyStatus = reader.IsDBNull(reader.GetOrdinal("property_status")) ? "" : reader.GetString(reader.GetOrdinal("property_status"));
                    steDetails.PropertyHeaderLine2 = reader.IsDBNull(reader.GetOrdinal("property_header_line_2")) ? "" : reader.GetString(reader.GetOrdinal("property_header_line_2"));


                    if (steDetails.SiteAddress.Length > 15)
                    {
                        steDetails.SiteAddressSmall = steDetails.SiteAddress.Substring(0, 15) + "..";
                    }
                    else
                    {
                        steDetails.SiteAddressSmall = steDetails.SiteAddress;
                    }

                    if (steDetails.AssetStatus == 0)
                    {
                        steDetails.AssetStatusName = "Available";
                    }
                    else
                    {
                        steDetails.AssetStatusName = "Sold";
                    }

                    steDetails.TransactionStatusName = "";

                    steDetails.DiligenceDispositions = GetDiligenceDispositions(steDetails.SiteDetailsId);

                    steDetails.SelectedDiligenceDispositions = new DiligenceDispositionsViewModel();

                    int saleLoi = 0, saleUnderContract = 0, saleTerminated = 0, saleClosed = 0;



                    steDetails.DiligenceAcquisitions = GetDiligenceAcquisition(steDetails.SiteDetailsId);
                    steDetails.DiligenceDispositions = GetDiligenceDispositions(steDetails.SiteDetailsId);
                    steDetails.DiligenceLease = GetDiligenceLease(steDetails.SiteDetailsId);
                    steDetails.DiligenceLeaseList = GetDiligenceLeaseList(steDetails.SiteDetailsId);
                    steDetails.DiligenceDispositions_SaleLeaseBack = GetDiligenceDispositions_SaleLeaseBack(steDetails.SiteDetailsId);

                    // steDetails.DiligenceDispositions = GetDiligenceDispositions(steDetails.SiteDetailsId);

                    steDetails.SelectedDiligenceDispositions = new DiligenceDispositionsViewModel();
                    DateTime? transactionClosedDate = default(DateTime?);

                    steDetails.DiligenceLeaseWithPurchase = new DiligenceLeaseWithPurchaseViewModel();
                    steDetails.SelectedDiligenceLease = new DiligenceLeaseViewModel();
                    steDetails.DiligenceLeaseWithPurchase = new DiligenceLeaseWithPurchaseViewModel();

                    bool canAddToList = false;
                    if (steDetails.AssetTypeId == (int)SamAssetType.Fee || steDetails.AssetTypeId == (int)SamAssetType.FeeSubjectToLease)
                    {
                        foreach (DiligenceDispositionsViewModel ddm in steDetails.DiligenceDispositions)
                        {

                            if (steDetails.MaxPriorityTransactionStatusId == 0)
                            {
                                steDetails.MaxPriorityTransactionStatusId = ddm.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = ddm.SelectedTransactionStatusName;
                                steDetails.SelectedDiligenceDispositions = ddm;
                                transactionClosedDate = ddm.ClosingDate;
                            }



                            if ((ddm.SelectedTransactionStatusId == (int)TransactionStatus.LOI_Received ||
                                ddm.SelectedTransactionStatusId == (int)TransactionStatus.Terminated_Acquisitions) &&
                                (ddm.SelectedTransactionStatusId != (int)SamsTransactionStatus.Under_LOI &&
                                steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Under_Contract &&
                                steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Closed_Dispositions
                                ))
                            {
                                steDetails.MaxPriorityTransactionStatusId = ddm.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = ddm.SelectedTransactionStatusName;
                                steDetails.StatusChangedDate = ddm.SelectedTransactionDate;
                                steDetails.SelectedDiligenceDispositions = ddm;
                                transactionClosedDate = ddm.ClosingDate;
                            }

                            if (ddm.SelectedTransactionStatusId == (int)SamsTransactionStatus.Under_LOI &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Under_Contract) &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Closed_Dispositions))
                            {
                                steDetails.MaxPriorityTransactionStatusId = ddm.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = ddm.SelectedTransactionStatusName;
                                steDetails.StatusChangedDate = ddm.SelectedTransactionDate;
                                steDetails.SelectedDiligenceDispositions = ddm;
                                transactionClosedDate = ddm.ClosingDate;
                            }

                            if (ddm.SelectedTransactionStatusId == (int)SamsTransactionStatus.Under_Contract &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Closed_Dispositions))
                            {
                                steDetails.MaxPriorityTransactionStatusId = ddm.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = ddm.SelectedTransactionStatusName;
                                steDetails.StatusChangedDate = ddm.SelectedTransactionDate;
                                steDetails.SelectedDiligenceDispositions = ddm;
                                transactionClosedDate = ddm.ClosingDate;
                            }

                            if (ddm.SelectedTransactionStatusId == (int)SamsTransactionStatus.Closed_Dispositions)
                            {
                                steDetails.MaxPriorityTransactionStatusId = ddm.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = ddm.SelectedTransactionStatusName;
                                steDetails.StatusChangedDate = ddm.SelectedTransactionDate;
                                steDetails.SelectedDiligenceDispositions = ddm;
                                transactionClosedDate = ddm.ClosingDate;
                            }

                            if (ddm.SelectedTransactionStatusId == (int)SamsTransactionStatus.Under_Contract)
                            {
                                canAddToList = true;
                            }

                        }


                        var dtClosedDate = "";
                        var daysToClose = 0;
                        if (transactionClosedDate != default(DateTime?))
                        {
                            dtClosedDate = transactionClosedDate.Value.ToString("MM/dd/yyyy");
                            daysToClose = (transactionClosedDate.Value - DateTime.Now).Days;
                            if (daysToClose < 0)
                            {
                                daysToClose = 0;
                            }
                        }

                        
                    }
                    else if (steDetails.AssetTypeId == (int)SamAssetType.Lease)
                    {
                        steDetails.DiligenceLeaseList = GetDiligenceLeaseList(steDetails.SiteDetailsId);
                        int leaseLoi = 0, leaseUnderContract = 0, leaseTerminated = 0, leaseClosed = 0;
                        foreach (DiligenceLeaseViewModel dl in steDetails.DiligenceLeaseList)
                        {
                            if (dl.SelectedTransactionStatusId == (int)TransactionStatus.Under_LOI)
                            {
                                leaseLoi = leaseLoi + 1;
                            }
                            else if (dl.SelectedTransactionStatusId == (int)TransactionStatus.Under_Contract)
                            {
                                leaseUnderContract = leaseUnderContract + 1;
                            }
                            else if (dl.SelectedTransactionStatusId == (int)TransactionStatus.Terminated_Acquisitions)
                            {
                                leaseTerminated = leaseTerminated + 1;
                            }
                            else if (dl.SelectedTransactionStatusId == (int)TransactionStatus.Closed_Acquisitions)
                            {
                                leaseClosed = leaseClosed + 1;
                            }

                            if (steDetails.MaxPriorityTransactionStatusId == 0)
                            {
                                steDetails.MaxPriorityTransactionStatusId = dl.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = dl.SelectedTransactionStatusName;
                                steDetails.SelectedDiligenceLease = dl;
                                transactionClosedDate = dl.ClosingDate;
                            }



                            if ((dl.SelectedTransactionStatusId == (int)TransactionStatus.LOI_Received ||
                                dl.SelectedTransactionStatusId == (int)TransactionStatus.Terminated_Acquisitions) &&
                                (dl.SelectedTransactionStatusId != (int)SamsTransactionStatus.Under_LOI &&
                                steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Under_Contract &&
                                steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Closed_Dispositions
                                ))
                            {
                                steDetails.MaxPriorityTransactionStatusId = dl.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = dl.SelectedTransactionStatusName;
                                steDetails.StatusChangedDate = dl.SelectedTransactionDate;
                                steDetails.SelectedDiligenceLease = dl;
                                transactionClosedDate = dl.ClosingDate;
                            }

                            if (dl.SelectedTransactionStatusId == (int)SamsTransactionStatus.Under_LOI &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Under_Contract) &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Closed_Dispositions))
                            {
                                steDetails.MaxPriorityTransactionStatusId = dl.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = dl.SelectedTransactionStatusName;
                                steDetails.StatusChangedDate = dl.SelectedTransactionDate;
                                steDetails.SelectedDiligenceLease = dl;
                                transactionClosedDate = dl.ClosingDate;
                            }

                            if (dl.SelectedTransactionStatusId == (int)SamsTransactionStatus.Under_Contract &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Closed_Dispositions))
                            {
                                steDetails.MaxPriorityTransactionStatusId = dl.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = dl.SelectedTransactionStatusName;
                                steDetails.StatusChangedDate = dl.SelectedTransactionDate;
                                steDetails.SelectedDiligenceLease = dl;
                                transactionClosedDate = dl.ClosingDate;
                            }

                            if (dl.SelectedTransactionStatusId == (int)SamsTransactionStatus.Closed_Dispositions)
                            {
                                steDetails.MaxPriorityTransactionStatusId = dl.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = dl.SelectedTransactionStatusName;
                                steDetails.StatusChangedDate = dl.SelectedTransactionDate;
                                steDetails.SelectedDiligenceLease = dl;
                                transactionClosedDate = dl.ClosingDate;
                            }


                            if (dl.SelectedTransactionStatusId == (int)SamsTransactionStatus.Under_Contract)
                            {
                                canAddToList = true;
                            }
                        }


                        if (steDetails.SelectedDiligenceLease != null)
                        {
                            var dtClosedDate = "";
                            var daysToClose = 0;
                            if (transactionClosedDate != default(DateTime?))
                            {
                                dtClosedDate = transactionClosedDate.Value.ToString("MM/dd/yyyy");
                                daysToClose = (transactionClosedDate.Value - DateTime.Now).Days;
                                if (daysToClose < 0)
                                {
                                    daysToClose = 0;
                                }
                            }
                        }

                    }
                    else if (steDetails.AssetTypeId == (int)SamAssetType.LeaseWithPurchaseOption)
                    {
                        steDetails.DiligenceLeaseWithPurchaseList = GetDiligenceLeaseWithPurchaseList(steDetails.SiteDetailsId);

                        foreach (DiligenceLeaseWithPurchaseViewModel dl in steDetails.DiligenceLeaseWithPurchaseList)
                        {

                            steDetails.DiligenceLeaseWithPurchase = dl;
                            if (steDetails.MaxPriorityTransactionStatusId == 0)
                            {
                                steDetails.MaxPriorityTransactionStatusId = dl.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = dl.SelectedTransactionStatusName;

                                steDetails.DiligenceLeaseWithPurchase = dl;
                                transactionClosedDate = dl.ClosingDate;
                            }



                            if ((dl.SelectedTransactionStatusId == (int)TransactionStatus.LOI_Received ||
                                dl.SelectedTransactionStatusId == (int)TransactionStatus.Terminated_Acquisitions) &&
                                (dl.SelectedTransactionStatusId != (int)SamsTransactionStatus.Under_LOI &&
                                steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Under_Contract &&
                                steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Closed_Dispositions
                                ))
                            {
                                steDetails.MaxPriorityTransactionStatusId = dl.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = dl.SelectedTransactionStatusName;
                                steDetails.DiligenceLeaseWithPurchase = dl;
                                transactionClosedDate = dl.ClosingDate;
                            }

                            if (dl.SelectedTransactionStatusId == (int)SamsTransactionStatus.Under_LOI &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Under_Contract) &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Closed_Dispositions))
                            {
                                steDetails.MaxPriorityTransactionStatusId = dl.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = dl.SelectedTransactionStatusName;
                                steDetails.DiligenceLeaseWithPurchase = dl;
                                transactionClosedDate = dl.ClosingDate;
                            }

                            if (dl.SelectedTransactionStatusId == (int)SamsTransactionStatus.Under_Contract &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Closed_Dispositions))
                            {
                                steDetails.MaxPriorityTransactionStatusId = dl.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = dl.SelectedTransactionStatusName;
                                steDetails.DiligenceLeaseWithPurchase = dl;
                                transactionClosedDate = dl.ClosingDate;
                            }

                            if (dl.SelectedTransactionStatusId == (int)SamsTransactionStatus.Closed_Dispositions)
                            {
                                steDetails.MaxPriorityTransactionStatusId = dl.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = dl.SelectedTransactionStatusName;
                                steDetails.DiligenceLeaseWithPurchase = dl;
                                transactionClosedDate = dl.ClosingDate;
                            }

                            if (dl.SelectedTransactionStatusId == (int)SamsTransactionStatus.Under_Contract)
                            {
                                canAddToList = true;
                            }
                        }



                        if (steDetails.DiligenceLeaseWithPurchase != null)
                        {

                            var dtClosedDate = "";
                            var daysToClose = 0;
                            if (transactionClosedDate != default(DateTime?))
                            {
                                dtClosedDate = transactionClosedDate.Value.ToString("MM/dd/yyyy");
                                daysToClose = (transactionClosedDate.Value - DateTime.Now).Days;
                                if (daysToClose < 0)
                                {
                                    daysToClose = 0;
                                }
                            }

                        }
                    }
                    else if (steDetails.AssetTypeId == (int)SamAssetType.SaleLeaseBack)
                    {
                        foreach (DiligenceDispositionsViewModel ddv in steDetails.DiligenceDispositions_SaleLeaseBack)
                        {

                            if (ddv.SelectedTransactionStatusId == (int)TransactionStatus.Closed_Acquisitions || ddv.SelectedTransactionStatusId == (int)TransactionStatus.Under_Contract || ddv.SelectedTransactionStatusId == (int)TransactionStatus.Under_LOI)
                            {
                                steDetails.CanAddTransactions = false;
                            }

                            if (ddv.SelectedTransactionStatusId == (int)TransactionStatus.Closed_Acquisitions)
                            {
                                steDetails.IsTransactionClosed = true;
                            }

                            if (steDetails.MaxPriorityTransactionStatusId == 0)
                            {
                                steDetails.MaxPriorityTransactionStatusId = ddv.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = ddv.SelectedTransactionStatusName;
                            }
                            steDetails.SelectedDiligenceDispositions = ddv;
                            transactionClosedDate = ddv.ClosingDate;


                            if ((ddv.SelectedTransactionStatusId == (int)TransactionStatus.LOI_Received ||
                                ddv.SelectedTransactionStatusId == (int)TransactionStatus.Terminated_Acquisitions) &&
                                (ddv.SelectedTransactionStatusId != (int)SamsTransactionStatus.Under_LOI &&
                                steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Under_Contract &&
                                steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Closed_Dispositions
                                ))
                            {
                                steDetails.MaxPriorityTransactionStatusId = ddv.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = ddv.SelectedTransactionStatusName;
                                steDetails.SelectedDiligenceDispositions = ddv;
                                transactionClosedDate = ddv.ClosingDate;
                            }

                            if (ddv.SelectedTransactionStatusId == (int)SamsTransactionStatus.Under_LOI &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Under_Contract) &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Closed_Dispositions))
                            {
                                steDetails.MaxPriorityTransactionStatusId = ddv.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = ddv.SelectedTransactionStatusName;
                                steDetails.SelectedDiligenceDispositions = ddv;
                                transactionClosedDate = ddv.ClosingDate;
                            }

                            if (ddv.SelectedTransactionStatusId == (int)SamsTransactionStatus.Under_Contract &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Closed_Dispositions))
                            {
                                steDetails.MaxPriorityTransactionStatusId = ddv.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = ddv.SelectedTransactionStatusName;
                                steDetails.SelectedDiligenceDispositions = ddv;
                                transactionClosedDate = ddv.ClosingDate;
                            }

                            if (ddv.SelectedTransactionStatusId == (int)SamsTransactionStatus.Closed_Dispositions)
                            {
                                steDetails.MaxPriorityTransactionStatusId = ddv.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = ddv.SelectedTransactionStatusName;
                                steDetails.SelectedDiligenceDispositions = ddv;
                                transactionClosedDate = ddv.ClosingDate;
                            }

                            if (ddv.SelectedTransactionStatusId == (int)SamsTransactionStatus.Under_Contract)
                            {
                                canAddToList = true;
                            }
                        }

                        var dtClosedDate = "";
                        var daysToClose = 0;
                        if (transactionClosedDate != default(DateTime?))
                        {
                            dtClosedDate = transactionClosedDate.Value.ToString("MM/dd/yyyy");
                            daysToClose = (transactionClosedDate.Value - DateTime.Now).Days;
                            if (daysToClose < 0)
                            {
                                daysToClose = 0;
                            }
                        }

                    }


                    steDetails.TodoList = GetTodoList(steDetails.SiteDetailsId);
                    if (steDetails.TodoList.Count > 0)
                    {
                        steDetails.LatestComment = steDetails.TodoList[0].TodoText;
                    }

                    if (canAddToList)
                    {
                        surplusPropertiesList.Add(steDetails);
                    }
                    
                }
                con.Close();
            }

            return View(surplusPropertiesList);
        }


        public IActionResult GetAvailableProperties()
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

            List<SiteDetails> surplusPropertiesList = new List<SiteDetails>();

            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {

                /*
                 * 1	Available
                 * 2	Under contract
                 * 3	Sold
                 */
                SqlCommand cmd = new SqlCommand("GetPropertyListByStatus", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("property_status_id", 1);
                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var steDetails = new SiteDetails();
                    steDetails.SiteDetailsId = reader.IsDBNull(reader.GetOrdinal("site_details_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("site_details_id"));
                    steDetails.NamePrefix = reader.IsDBNull(reader.GetOrdinal("name_prefix")) ? "" : reader.GetString(reader.GetOrdinal("name_prefix"));
                    steDetails.FirstName = reader.IsDBNull(reader.GetOrdinal("first_name")) ? "" : reader.GetString(reader.GetOrdinal("first_name"));

                    steDetails.LastName = reader.IsDBNull(reader.GetOrdinal("last_name")) ? "" : reader.GetString(reader.GetOrdinal("last_name"));
                    steDetails.CompanyName = reader.IsDBNull(reader.GetOrdinal("company_name")) ? "" : reader.GetString(reader.GetOrdinal("company_name"));
                    steDetails.EmailAddress = reader.IsDBNull(reader.GetOrdinal("email_address")) ? "" : reader.GetString(reader.GetOrdinal("email_address"));
                    steDetails.Address = reader.IsDBNull(reader.GetOrdinal("address")) ? "" : reader.GetString(reader.GetOrdinal("address"));
                    steDetails.CityName = reader.IsDBNull(reader.GetOrdinal("city_name")) ? "" : reader.GetString(reader.GetOrdinal("city_name"));
                    steDetails.StateId = reader.IsDBNull(reader.GetOrdinal("state_id")) ? "" : reader.GetString(reader.GetOrdinal("state_id"));

                    steDetails.StateName = reader.IsDBNull(reader.GetOrdinal("state_name")) ? "" : reader.GetString(reader.GetOrdinal("state_name"));

                    steDetails.ZipCode = reader.IsDBNull(reader.GetOrdinal("zip_code")) ? "" : reader.GetString(reader.GetOrdinal("zip_code"));
                    steDetails.ContactNumber = reader.IsDBNull(reader.GetOrdinal("contact_number")) ? "" : reader.GetString(reader.GetOrdinal("contact_number"));
                    steDetails.SamsHoldingEmployee = reader.IsDBNull(reader.GetOrdinal("sams_holding_employee")) ? false : reader.GetBoolean(reader.GetOrdinal("sams_holding_employee"));
                    steDetails.MarketId = reader.IsDBNull(reader.GetOrdinal("market_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("market_id"));
                    steDetails.PropertyHeader = reader.IsDBNull(reader.GetOrdinal("property_header")) ? "" : reader.GetString(reader.GetOrdinal("property_header"));
                    steDetails.SiteAddress = reader.IsDBNull(reader.GetOrdinal("site_address")) ? "" : reader.GetString(reader.GetOrdinal("site_address"));
                    steDetails.SiteCity = reader.IsDBNull(reader.GetOrdinal("site_city")) ? "" : reader.GetString(reader.GetOrdinal("site_city"));
                    steDetails.SiteStateId = reader.IsDBNull(reader.GetOrdinal("site_state_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("site_state_id"));

                    steDetails.SiteStateName = reader.IsDBNull(reader.GetOrdinal("site_state_name")) ? "" : reader.GetString(reader.GetOrdinal("site_state_name"));

                    steDetails.SiteCounty = reader.IsDBNull(reader.GetOrdinal("site_county")) ? "" : reader.GetString(reader.GetOrdinal("site_county"));
                    steDetails.SiteCrossStreetName = reader.IsDBNull(reader.GetOrdinal("site_cross_street_name")) ? "" : reader.GetString(reader.GetOrdinal("site_cross_street_name"));
                    steDetails.IsPropertyAvailable = reader.IsDBNull(reader.GetOrdinal("is_property_available")) ? true : reader.GetBoolean(reader.GetOrdinal("is_property_available"));
                    steDetails.Zoning = reader.IsDBNull(reader.GetOrdinal("zoning")) ? "" : reader.GetString(reader.GetOrdinal("zoning"));
                    steDetails.LotSize = reader.IsDBNull(reader.GetOrdinal("lot_size")) ? "" : reader.GetString(reader.GetOrdinal("lot_size"));

                    steDetails.SalesPrice = reader.IsDBNull(reader.GetOrdinal("sales_price")) ? "" : reader.GetString(reader.GetOrdinal("sales_price"));
                    //steDetails.SalesPrice = Helper.FormatCurrency("$", steDetails.SalesPrice);

                    steDetails.Comments = reader.IsDBNull(reader.GetOrdinal("comments")) ? "" : reader.GetString(reader.GetOrdinal("comments"));

                    steDetails.CreatedDate = reader.IsDBNull(reader.GetOrdinal("created_date")) ? DateTime.Now : reader.GetDateTime(reader.GetOrdinal("created_date"));
                    steDetails.PropertyType = reader.IsDBNull(reader.GetOrdinal("property_type")) ? 0 : reader.GetInt32(reader.GetOrdinal("property_type"));

                    steDetails.ImageName = reader.IsDBNull(reader.GetOrdinal("image_name")) ? "" : reader.GetString(reader.GetOrdinal("image_name"));

                    steDetails.AssetTypeId = reader.IsDBNull(reader.GetOrdinal("asset_type_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("asset_type_id"));
                    steDetails.AssetTypeName = reader.IsDBNull(reader.GetOrdinal("asset_type_name")) ? "" : reader.GetString(reader.GetOrdinal("asset_type_name"));
                    steDetails.AssetStatus = reader.IsDBNull(reader.GetOrdinal("asset_status")) ? 0 : reader.GetInt32(reader.GetOrdinal("asset_status"));
                    steDetails.AssetId = reader.IsDBNull(reader.GetOrdinal("asset_id")) ? "" : reader.GetString(reader.GetOrdinal("asset_id"));

                    steDetails.SelectedPropertyStatusId = reader.IsDBNull(reader.GetOrdinal("property_status_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("property_status_id"));
                    steDetails.SelectedPropertyStatus = reader.IsDBNull(reader.GetOrdinal("property_status")) ? "" : reader.GetString(reader.GetOrdinal("property_status"));
                    steDetails.PropertyHeaderLine2 = reader.IsDBNull(reader.GetOrdinal("property_header_line_2")) ? "" : reader.GetString(reader.GetOrdinal("property_header_line_2"));


                    if (steDetails.SiteAddress.Length > 15)
                    {
                        steDetails.SiteAddressSmall = steDetails.SiteAddress.Substring(0, 15) + "..";
                    }
                    else
                    {
                        steDetails.SiteAddressSmall = steDetails.SiteAddress;
                    }

                    if (steDetails.AssetStatus == 0)
                    {
                        steDetails.AssetStatusName = "Available";
                    }
                    else
                    {
                        steDetails.AssetStatusName = "Sold";
                    }

                    steDetails.TransactionStatusName = "";

                    steDetails.DiligenceDispositions = GetDiligenceDispositions(steDetails.SiteDetailsId);

                    steDetails.SelectedDiligenceDispositions = new DiligenceDispositionsViewModel();

                    int saleLoi = 0, saleUnderContract = 0, saleTerminated = 0, saleClosed = 0;



                    steDetails.DiligenceAcquisitions = GetDiligenceAcquisition(steDetails.SiteDetailsId);
                    steDetails.DiligenceDispositions = GetDiligenceDispositions(steDetails.SiteDetailsId);
                    steDetails.DiligenceLease = GetDiligenceLease(steDetails.SiteDetailsId);
                    steDetails.DiligenceLeaseList = GetDiligenceLeaseList(steDetails.SiteDetailsId);
                    steDetails.DiligenceDispositions_SaleLeaseBack = GetDiligenceDispositions_SaleLeaseBack(steDetails.SiteDetailsId);

                    // steDetails.DiligenceDispositions = GetDiligenceDispositions(steDetails.SiteDetailsId);

                    steDetails.SelectedDiligenceDispositions = new DiligenceDispositionsViewModel();
                    DateTime? transactionClosedDate = default(DateTime?);

                    steDetails.DiligenceLeaseWithPurchase = new DiligenceLeaseWithPurchaseViewModel();
                    steDetails.SelectedDiligenceLease = new DiligenceLeaseViewModel();
                    steDetails.DiligenceLeaseWithPurchase = new DiligenceLeaseWithPurchaseViewModel();


                    if (steDetails.AssetTypeId == (int)SamAssetType.Fee || steDetails.AssetTypeId == (int)SamAssetType.FeeSubjectToLease)
                    {
                        foreach (DiligenceDispositionsViewModel ddm in steDetails.DiligenceDispositions)
                        {

                            if (steDetails.MaxPriorityTransactionStatusId == 0)
                            {
                                steDetails.MaxPriorityTransactionStatusId = ddm.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = ddm.SelectedTransactionStatusName;
                                steDetails.SelectedDiligenceDispositions = ddm;
                                transactionClosedDate = ddm.ClosingDate;
                            }



                            if ((ddm.SelectedTransactionStatusId == (int)TransactionStatus.LOI_Received ||
                                ddm.SelectedTransactionStatusId == (int)TransactionStatus.Terminated_Acquisitions) &&
                                (ddm.SelectedTransactionStatusId != (int)SamsTransactionStatus.Under_LOI &&
                                steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Under_Contract &&
                                steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Closed_Dispositions
                                ))
                            {
                                steDetails.MaxPriorityTransactionStatusId = ddm.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = ddm.SelectedTransactionStatusName;
                                steDetails.StatusChangedDate = ddm.SelectedTransactionDate;
                                steDetails.SelectedDiligenceDispositions = ddm;
                                transactionClosedDate = ddm.ClosingDate;
                            }

                            if (ddm.SelectedTransactionStatusId == (int)SamsTransactionStatus.Under_LOI &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Under_Contract) &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Closed_Dispositions))
                            {
                                steDetails.MaxPriorityTransactionStatusId = ddm.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = ddm.SelectedTransactionStatusName;
                                steDetails.StatusChangedDate = ddm.SelectedTransactionDate;
                                steDetails.SelectedDiligenceDispositions = ddm;
                                transactionClosedDate = ddm.ClosingDate;
                            }

                            if (ddm.SelectedTransactionStatusId == (int)SamsTransactionStatus.Under_Contract &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Closed_Dispositions))
                            {
                                steDetails.MaxPriorityTransactionStatusId = ddm.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = ddm.SelectedTransactionStatusName;
                                steDetails.StatusChangedDate = ddm.SelectedTransactionDate;
                                steDetails.SelectedDiligenceDispositions = ddm;
                                transactionClosedDate = ddm.ClosingDate;
                            }

                            if (ddm.SelectedTransactionStatusId == (int)SamsTransactionStatus.Closed_Dispositions)
                            {
                                steDetails.MaxPriorityTransactionStatusId = ddm.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = ddm.SelectedTransactionStatusName;
                                steDetails.StatusChangedDate = ddm.SelectedTransactionDate;
                                steDetails.SelectedDiligenceDispositions = ddm;
                                transactionClosedDate = ddm.ClosingDate;
                            }

                        }


                        var dtClosedDate = "";
                        var daysToClose = 0;
                        if (transactionClosedDate != default(DateTime?))
                        {
                            dtClosedDate = transactionClosedDate.Value.ToString("MM/dd/yyyy");
                            daysToClose = (transactionClosedDate.Value - DateTime.Now).Days;
                            if (daysToClose < 0)
                            {
                                daysToClose = 0;
                            }
                        }
                    }
                    else if (steDetails.AssetTypeId == (int)SamAssetType.Lease)
                    {
                        steDetails.DiligenceLeaseList = GetDiligenceLeaseList(steDetails.SiteDetailsId);
                        int leaseLoi = 0, leaseUnderContract = 0, leaseTerminated = 0, leaseClosed = 0;
                        foreach (DiligenceLeaseViewModel dl in steDetails.DiligenceLeaseList)
                        {
                            if (dl.SelectedTransactionStatusId == (int)TransactionStatus.Under_LOI)
                            {
                                leaseLoi = leaseLoi + 1;
                            }
                            else if (dl.SelectedTransactionStatusId == (int)TransactionStatus.Under_Contract)
                            {
                                leaseUnderContract = leaseUnderContract + 1;
                            }
                            else if (dl.SelectedTransactionStatusId == (int)TransactionStatus.Terminated_Acquisitions)
                            {
                                leaseTerminated = leaseTerminated + 1;
                            }
                            else if (dl.SelectedTransactionStatusId == (int)TransactionStatus.Closed_Acquisitions)
                            {
                                leaseClosed = leaseClosed + 1;
                            }

                            if (steDetails.MaxPriorityTransactionStatusId == 0)
                            {
                                steDetails.MaxPriorityTransactionStatusId = dl.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = dl.SelectedTransactionStatusName;
                                steDetails.SelectedDiligenceLease = dl;
                                transactionClosedDate = dl.ClosingDate;
                            }



                            if ((dl.SelectedTransactionStatusId == (int)TransactionStatus.LOI_Received ||
                                dl.SelectedTransactionStatusId == (int)TransactionStatus.Terminated_Acquisitions) &&
                                (dl.SelectedTransactionStatusId != (int)SamsTransactionStatus.Under_LOI &&
                                steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Under_Contract &&
                                steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Closed_Dispositions
                                ))
                            {
                                steDetails.MaxPriorityTransactionStatusId = dl.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = dl.SelectedTransactionStatusName;
                                steDetails.StatusChangedDate = dl.SelectedTransactionDate;
                                steDetails.SelectedDiligenceLease = dl;
                                transactionClosedDate = dl.ClosingDate;
                            }

                            if (dl.SelectedTransactionStatusId == (int)SamsTransactionStatus.Under_LOI &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Under_Contract) &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Closed_Dispositions))
                            {
                                steDetails.MaxPriorityTransactionStatusId = dl.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = dl.SelectedTransactionStatusName;
                                steDetails.StatusChangedDate = dl.SelectedTransactionDate;
                                steDetails.SelectedDiligenceLease = dl;
                                transactionClosedDate = dl.ClosingDate;
                            }

                            if (dl.SelectedTransactionStatusId == (int)SamsTransactionStatus.Under_Contract &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Closed_Dispositions))
                            {
                                steDetails.MaxPriorityTransactionStatusId = dl.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = dl.SelectedTransactionStatusName;
                                steDetails.StatusChangedDate = dl.SelectedTransactionDate;
                                steDetails.SelectedDiligenceLease = dl;
                                transactionClosedDate = dl.ClosingDate;
                            }

                            if (dl.SelectedTransactionStatusId == (int)SamsTransactionStatus.Closed_Dispositions)
                            {
                                steDetails.MaxPriorityTransactionStatusId = dl.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = dl.SelectedTransactionStatusName;
                                steDetails.StatusChangedDate = dl.SelectedTransactionDate;
                                steDetails.SelectedDiligenceLease = dl;
                                transactionClosedDate = dl.ClosingDate;
                            }
                        }


                        if (steDetails.SelectedDiligenceLease != null)
                        {
                            var dtClosedDate = "";
                            var daysToClose = 0;
                            if (transactionClosedDate != default(DateTime?))
                            {
                                dtClosedDate = transactionClosedDate.Value.ToString("MM/dd/yyyy");
                                daysToClose = (transactionClosedDate.Value - DateTime.Now).Days;
                                if (daysToClose < 0)
                                {
                                    daysToClose = 0;
                                }
                            }
                        }

                    }
                    else if (steDetails.AssetTypeId == (int)SamAssetType.LeaseWithPurchaseOption)
                    {
                        steDetails.DiligenceLeaseWithPurchaseList = GetDiligenceLeaseWithPurchaseList(steDetails.SiteDetailsId);

                        foreach (DiligenceLeaseWithPurchaseViewModel dl in steDetails.DiligenceLeaseWithPurchaseList)
                        {

                            steDetails.DiligenceLeaseWithPurchase = dl;
                            if (steDetails.MaxPriorityTransactionStatusId == 0)
                            {
                                steDetails.MaxPriorityTransactionStatusId = dl.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = dl.SelectedTransactionStatusName;

                                steDetails.DiligenceLeaseWithPurchase = dl;
                                transactionClosedDate = dl.ClosingDate;
                            }



                            if ((dl.SelectedTransactionStatusId == (int)TransactionStatus.LOI_Received ||
                                dl.SelectedTransactionStatusId == (int)TransactionStatus.Terminated_Acquisitions) &&
                                (dl.SelectedTransactionStatusId != (int)SamsTransactionStatus.Under_LOI &&
                                steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Under_Contract &&
                                steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Closed_Dispositions
                                ))
                            {
                                steDetails.MaxPriorityTransactionStatusId = dl.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = dl.SelectedTransactionStatusName;
                                steDetails.DiligenceLeaseWithPurchase = dl;
                                transactionClosedDate = dl.ClosingDate;
                            }

                            if (dl.SelectedTransactionStatusId == (int)SamsTransactionStatus.Under_LOI &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Under_Contract) &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Closed_Dispositions))
                            {
                                steDetails.MaxPriorityTransactionStatusId = dl.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = dl.SelectedTransactionStatusName;
                                steDetails.DiligenceLeaseWithPurchase = dl;
                                transactionClosedDate = dl.ClosingDate;
                            }

                            if (dl.SelectedTransactionStatusId == (int)SamsTransactionStatus.Under_Contract &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Closed_Dispositions))
                            {
                                steDetails.MaxPriorityTransactionStatusId = dl.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = dl.SelectedTransactionStatusName;
                                steDetails.DiligenceLeaseWithPurchase = dl;
                                transactionClosedDate = dl.ClosingDate;
                            }

                            if (dl.SelectedTransactionStatusId == (int)SamsTransactionStatus.Closed_Dispositions)
                            {
                                steDetails.MaxPriorityTransactionStatusId = dl.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = dl.SelectedTransactionStatusName;
                                steDetails.DiligenceLeaseWithPurchase = dl;
                                transactionClosedDate = dl.ClosingDate;
                            }
                        }



                        if (steDetails.DiligenceLeaseWithPurchase != null)
                        {

                            var dtClosedDate = "";
                            var daysToClose = 0;
                            if (transactionClosedDate != default(DateTime?))
                            {
                                dtClosedDate = transactionClosedDate.Value.ToString("MM/dd/yyyy");
                                daysToClose = (transactionClosedDate.Value - DateTime.Now).Days;
                                if (daysToClose < 0)
                                {
                                    daysToClose = 0;
                                }
                            }

                        }
                    }
                    else if (steDetails.AssetTypeId == (int)SamAssetType.SaleLeaseBack)
                    {
                        foreach (DiligenceDispositionsViewModel ddv in steDetails.DiligenceDispositions_SaleLeaseBack)
                        {

                            if (ddv.SelectedTransactionStatusId == (int)TransactionStatus.Closed_Acquisitions || ddv.SelectedTransactionStatusId == (int)TransactionStatus.Under_Contract || ddv.SelectedTransactionStatusId == (int)TransactionStatus.Under_LOI)
                            {
                                steDetails.CanAddTransactions = false;
                            }

                            if (ddv.SelectedTransactionStatusId == (int)TransactionStatus.Closed_Acquisitions)
                            {
                                steDetails.IsTransactionClosed = true;
                            }

                            if (steDetails.MaxPriorityTransactionStatusId == 0)
                            {
                                steDetails.MaxPriorityTransactionStatusId = ddv.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = ddv.SelectedTransactionStatusName;
                            }
                            steDetails.SelectedDiligenceDispositions = ddv;
                            transactionClosedDate = ddv.ClosingDate;


                            if ((ddv.SelectedTransactionStatusId == (int)TransactionStatus.LOI_Received ||
                                ddv.SelectedTransactionStatusId == (int)TransactionStatus.Terminated_Acquisitions) &&
                                (ddv.SelectedTransactionStatusId != (int)SamsTransactionStatus.Under_LOI &&
                                steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Under_Contract &&
                                steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Closed_Dispositions
                                ))
                            {
                                steDetails.MaxPriorityTransactionStatusId = ddv.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = ddv.SelectedTransactionStatusName;
                                steDetails.SelectedDiligenceDispositions = ddv;
                                transactionClosedDate = ddv.ClosingDate;
                            }

                            if (ddv.SelectedTransactionStatusId == (int)SamsTransactionStatus.Under_LOI &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Under_Contract) &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Closed_Dispositions))
                            {
                                steDetails.MaxPriorityTransactionStatusId = ddv.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = ddv.SelectedTransactionStatusName;
                                steDetails.SelectedDiligenceDispositions = ddv;
                                transactionClosedDate = ddv.ClosingDate;
                            }

                            if (ddv.SelectedTransactionStatusId == (int)SamsTransactionStatus.Under_Contract &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Closed_Dispositions))
                            {
                                steDetails.MaxPriorityTransactionStatusId = ddv.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = ddv.SelectedTransactionStatusName;
                                steDetails.SelectedDiligenceDispositions = ddv;
                                transactionClosedDate = ddv.ClosingDate;
                            }

                            if (ddv.SelectedTransactionStatusId == (int)SamsTransactionStatus.Closed_Dispositions)
                            {
                                steDetails.MaxPriorityTransactionStatusId = ddv.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = ddv.SelectedTransactionStatusName;
                                steDetails.SelectedDiligenceDispositions = ddv;
                                transactionClosedDate = ddv.ClosingDate;
                            }


                        }

                        var dtClosedDate = "";
                        var daysToClose = 0;
                        if (transactionClosedDate != default(DateTime?))
                        {
                            dtClosedDate = transactionClosedDate.Value.ToString("MM/dd/yyyy");
                            daysToClose = (transactionClosedDate.Value - DateTime.Now).Days;
                            if (daysToClose < 0)
                            {
                                daysToClose = 0;
                            }
                        }

                    }


                    steDetails.TodoList = GetTodoList(steDetails.SiteDetailsId);
                    if (steDetails.TodoList.Count > 0)
                    {
                        steDetails.LatestComment = steDetails.TodoList[0].TodoText;
                    }


                    surplusPropertiesList.Add(steDetails);
                }
                con.Close();
            }

            return View(surplusPropertiesList);
        }

        public IActionResult GetSoldProperties()
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

            List<SiteDetails> surplusPropertiesList = new List<SiteDetails>();

            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {

                /*
                 * 1	Available
                 * 2	Under contract
                 * 3	Sold
                 */
                /*
               SqlCommand cmd = new SqlCommand("GetPropertyListByStatus", con);
               cmd.CommandType = CommandType.StoredProcedure;
               cmd.Parameters.AddWithValue("property_status_id", 3);
               */

                SqlCommand cmd = new SqlCommand("GetPropertyListByCategory", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("asset_status", 0);

                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var steDetails = new SiteDetails();
                    steDetails.SiteDetailsId = reader.IsDBNull(reader.GetOrdinal("site_details_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("site_details_id"));
                    steDetails.NamePrefix = reader.IsDBNull(reader.GetOrdinal("name_prefix")) ? "" : reader.GetString(reader.GetOrdinal("name_prefix"));
                    steDetails.FirstName = reader.IsDBNull(reader.GetOrdinal("first_name")) ? "" : reader.GetString(reader.GetOrdinal("first_name"));

                    steDetails.LastName = reader.IsDBNull(reader.GetOrdinal("last_name")) ? "" : reader.GetString(reader.GetOrdinal("last_name"));
                    steDetails.CompanyName = reader.IsDBNull(reader.GetOrdinal("company_name")) ? "" : reader.GetString(reader.GetOrdinal("company_name"));
                    steDetails.EmailAddress = reader.IsDBNull(reader.GetOrdinal("email_address")) ? "" : reader.GetString(reader.GetOrdinal("email_address"));
                    steDetails.Address = reader.IsDBNull(reader.GetOrdinal("address")) ? "" : reader.GetString(reader.GetOrdinal("address"));
                    steDetails.CityName = reader.IsDBNull(reader.GetOrdinal("city_name")) ? "" : reader.GetString(reader.GetOrdinal("city_name"));
                    steDetails.StateId = reader.IsDBNull(reader.GetOrdinal("state_id")) ? "" : reader.GetString(reader.GetOrdinal("state_id"));

                    steDetails.StateName = reader.IsDBNull(reader.GetOrdinal("state_name")) ? "" : reader.GetString(reader.GetOrdinal("state_name"));

                    steDetails.ZipCode = reader.IsDBNull(reader.GetOrdinal("zip_code")) ? "" : reader.GetString(reader.GetOrdinal("zip_code"));
                    steDetails.ContactNumber = reader.IsDBNull(reader.GetOrdinal("contact_number")) ? "" : reader.GetString(reader.GetOrdinal("contact_number"));
                    steDetails.SamsHoldingEmployee = reader.IsDBNull(reader.GetOrdinal("sams_holding_employee")) ? false : reader.GetBoolean(reader.GetOrdinal("sams_holding_employee"));
                    steDetails.MarketId = reader.IsDBNull(reader.GetOrdinal("market_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("market_id"));
                    steDetails.PropertyHeader = reader.IsDBNull(reader.GetOrdinal("property_header")) ? "" : reader.GetString(reader.GetOrdinal("property_header"));
                    steDetails.SiteAddress = reader.IsDBNull(reader.GetOrdinal("site_address")) ? "" : reader.GetString(reader.GetOrdinal("site_address"));
                    steDetails.SiteCity = reader.IsDBNull(reader.GetOrdinal("site_city")) ? "" : reader.GetString(reader.GetOrdinal("site_city"));
                    steDetails.SiteStateId = reader.IsDBNull(reader.GetOrdinal("site_state_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("site_state_id"));

                    steDetails.SiteStateName = reader.IsDBNull(reader.GetOrdinal("site_state_name")) ? "" : reader.GetString(reader.GetOrdinal("site_state_name"));

                    steDetails.SiteCounty = reader.IsDBNull(reader.GetOrdinal("site_county")) ? "" : reader.GetString(reader.GetOrdinal("site_county"));
                    steDetails.SiteCrossStreetName = reader.IsDBNull(reader.GetOrdinal("site_cross_street_name")) ? "" : reader.GetString(reader.GetOrdinal("site_cross_street_name"));
                    steDetails.IsPropertyAvailable = reader.IsDBNull(reader.GetOrdinal("is_property_available")) ? true : reader.GetBoolean(reader.GetOrdinal("is_property_available"));
                    steDetails.Zoning = reader.IsDBNull(reader.GetOrdinal("zoning")) ? "" : reader.GetString(reader.GetOrdinal("zoning"));
                    steDetails.LotSize = reader.IsDBNull(reader.GetOrdinal("lot_size")) ? "" : reader.GetString(reader.GetOrdinal("lot_size"));

                    steDetails.SalesPrice = reader.IsDBNull(reader.GetOrdinal("sales_price")) ? "" : reader.GetString(reader.GetOrdinal("sales_price"));
                    //steDetails.SalesPrice = Helper.FormatCurrency("$", steDetails.SalesPrice);

                    steDetails.Comments = reader.IsDBNull(reader.GetOrdinal("comments")) ? "" : reader.GetString(reader.GetOrdinal("comments"));

                    steDetails.CreatedDate = reader.IsDBNull(reader.GetOrdinal("created_date")) ? DateTime.Now : reader.GetDateTime(reader.GetOrdinal("created_date"));
                    steDetails.PropertyType = reader.IsDBNull(reader.GetOrdinal("property_type")) ? 0 : reader.GetInt32(reader.GetOrdinal("property_type"));

                    steDetails.ImageName = reader.IsDBNull(reader.GetOrdinal("image_name")) ? "" : reader.GetString(reader.GetOrdinal("image_name"));

                    steDetails.AssetTypeId = reader.IsDBNull(reader.GetOrdinal("asset_type_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("asset_type_id"));
                    steDetails.AssetTypeName = reader.IsDBNull(reader.GetOrdinal("asset_type_name")) ? "" : reader.GetString(reader.GetOrdinal("asset_type_name"));
                    steDetails.AssetStatus = reader.IsDBNull(reader.GetOrdinal("asset_status")) ? 0 : reader.GetInt32(reader.GetOrdinal("asset_status"));
                    steDetails.AssetId = reader.IsDBNull(reader.GetOrdinal("asset_id")) ? "" : reader.GetString(reader.GetOrdinal("asset_id"));

                    steDetails.SelectedPropertyStatusId = reader.IsDBNull(reader.GetOrdinal("property_status_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("property_status_id"));
                    steDetails.SelectedPropertyStatus = reader.IsDBNull(reader.GetOrdinal("property_status")) ? "" : reader.GetString(reader.GetOrdinal("property_status"));
                    steDetails.PropertyHeaderLine2 = reader.IsDBNull(reader.GetOrdinal("property_header_line_2")) ? "" : reader.GetString(reader.GetOrdinal("property_header_line_2"));


                    if (steDetails.SiteAddress.Length > 15)
                    {
                        steDetails.SiteAddressSmall = steDetails.SiteAddress.Substring(0, 15) + "..";
                    }
                    else
                    {
                        steDetails.SiteAddressSmall = steDetails.SiteAddress;
                    }

                    if (steDetails.AssetStatus == 0)
                    {
                        steDetails.AssetStatusName = "Available";
                    }
                    else
                    {
                        steDetails.AssetStatusName = "Sold";
                    }

                    steDetails.TransactionStatusName = "";

                    steDetails.DiligenceDispositions = GetDiligenceDispositions(steDetails.SiteDetailsId);

                    steDetails.SelectedDiligenceDispositions = new DiligenceDispositionsViewModel();

                    int saleLoi = 0, saleUnderContract = 0, saleTerminated = 0, saleClosed = 0;



                    steDetails.DiligenceAcquisitions = GetDiligenceAcquisition(steDetails.SiteDetailsId);
                    steDetails.DiligenceDispositions = GetDiligenceDispositions(steDetails.SiteDetailsId);
                    steDetails.DiligenceLease = GetDiligenceLease(steDetails.SiteDetailsId);
                    steDetails.DiligenceLeaseList = GetDiligenceLeaseList(steDetails.SiteDetailsId);
                    steDetails.DiligenceDispositions_SaleLeaseBack = GetDiligenceDispositions_SaleLeaseBack(steDetails.SiteDetailsId);

                    // steDetails.DiligenceDispositions = GetDiligenceDispositions(steDetails.SiteDetailsId);

                    steDetails.SelectedDiligenceDispositions = new DiligenceDispositionsViewModel();
                    DateTime? transactionClosedDate = default(DateTime?);

                    steDetails.DiligenceLeaseWithPurchase = new DiligenceLeaseWithPurchaseViewModel();
                    steDetails.SelectedDiligenceLease = new DiligenceLeaseViewModel();
                    steDetails.DiligenceLeaseWithPurchase = new DiligenceLeaseWithPurchaseViewModel();

                    bool canAddToList = false;
                    if (steDetails.AssetTypeId == (int)SamAssetType.Fee || steDetails.AssetTypeId == (int)SamAssetType.FeeSubjectToLease)
                    {
                        foreach (DiligenceDispositionsViewModel ddm in steDetails.DiligenceDispositions)
                        {

                            if (steDetails.MaxPriorityTransactionStatusId == 0)
                            {
                                steDetails.MaxPriorityTransactionStatusId = ddm.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = ddm.SelectedTransactionStatusName;
                                steDetails.SelectedDiligenceDispositions = ddm;
                                transactionClosedDate = ddm.ClosingDate;
                            }



                            if ((ddm.SelectedTransactionStatusId == (int)TransactionStatus.LOI_Received ||
                                ddm.SelectedTransactionStatusId == (int)TransactionStatus.Terminated_Acquisitions) &&
                                (ddm.SelectedTransactionStatusId != (int)SamsTransactionStatus.Under_LOI &&
                                steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Under_Contract &&
                                steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Closed_Dispositions
                                ))
                            {
                                steDetails.MaxPriorityTransactionStatusId = ddm.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = ddm.SelectedTransactionStatusName;
                                steDetails.StatusChangedDate = ddm.SelectedTransactionDate;
                                steDetails.SelectedDiligenceDispositions = ddm;
                                transactionClosedDate = ddm.ClosingDate;
                            }

                            if (ddm.SelectedTransactionStatusId == (int)SamsTransactionStatus.Under_LOI &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Under_Contract) &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Closed_Dispositions))
                            {
                                steDetails.MaxPriorityTransactionStatusId = ddm.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = ddm.SelectedTransactionStatusName;
                                steDetails.StatusChangedDate = ddm.SelectedTransactionDate;
                                steDetails.SelectedDiligenceDispositions = ddm;
                                transactionClosedDate = ddm.ClosingDate;
                            }

                            if (ddm.SelectedTransactionStatusId == (int)SamsTransactionStatus.Under_Contract &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Closed_Dispositions))
                            {
                                steDetails.MaxPriorityTransactionStatusId = ddm.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = ddm.SelectedTransactionStatusName;
                                steDetails.StatusChangedDate = ddm.SelectedTransactionDate;
                                steDetails.SelectedDiligenceDispositions = ddm;
                                transactionClosedDate = ddm.ClosingDate;
                            }

                            if (ddm.SelectedTransactionStatusId == (int)SamsTransactionStatus.Closed_Dispositions)
                            {
                                steDetails.MaxPriorityTransactionStatusId = ddm.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = ddm.SelectedTransactionStatusName;
                                steDetails.StatusChangedDate = ddm.SelectedTransactionDate;
                                steDetails.SelectedDiligenceDispositions = ddm;
                                transactionClosedDate = ddm.ClosingDate;
                            }

                            if (ddm.SelectedTransactionStatusId == (int)SamsTransactionStatus.Closed_Dispositions)
                            {
                                canAddToList = true;
                            }

                        }


                        var dtClosedDate = "";
                        var daysToClose = 0;
                        if (transactionClosedDate != default(DateTime?))
                        {
                            dtClosedDate = transactionClosedDate.Value.ToString("MM/dd/yyyy");
                            daysToClose = (transactionClosedDate.Value - DateTime.Now).Days;
                            if (daysToClose < 0)
                            {
                                daysToClose = 0;
                            }
                        }


                    }
                    else if (steDetails.AssetTypeId == (int)SamAssetType.Lease)
                    {
                        steDetails.DiligenceLeaseList = GetDiligenceLeaseList(steDetails.SiteDetailsId);
                        int leaseLoi = 0, leaseUnderContract = 0, leaseTerminated = 0, leaseClosed = 0;
                        foreach (DiligenceLeaseViewModel dl in steDetails.DiligenceLeaseList)
                        {
                            if (dl.SelectedTransactionStatusId == (int)TransactionStatus.Under_LOI)
                            {
                                leaseLoi = leaseLoi + 1;
                            }
                            else if (dl.SelectedTransactionStatusId == (int)TransactionStatus.Under_Contract)
                            {
                                leaseUnderContract = leaseUnderContract + 1;
                            }
                            else if (dl.SelectedTransactionStatusId == (int)TransactionStatus.Terminated_Acquisitions)
                            {
                                leaseTerminated = leaseTerminated + 1;
                            }
                            else if (dl.SelectedTransactionStatusId == (int)TransactionStatus.Closed_Acquisitions)
                            {
                                leaseClosed = leaseClosed + 1;
                            }

                            if (steDetails.MaxPriorityTransactionStatusId == 0)
                            {
                                steDetails.MaxPriorityTransactionStatusId = dl.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = dl.SelectedTransactionStatusName;
                                steDetails.SelectedDiligenceLease = dl;
                                transactionClosedDate = dl.ClosingDate;
                            }



                            if ((dl.SelectedTransactionStatusId == (int)TransactionStatus.LOI_Received ||
                                dl.SelectedTransactionStatusId == (int)TransactionStatus.Terminated_Acquisitions) &&
                                (dl.SelectedTransactionStatusId != (int)SamsTransactionStatus.Under_LOI &&
                                steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Under_Contract &&
                                steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Closed_Dispositions
                                ))
                            {
                                steDetails.MaxPriorityTransactionStatusId = dl.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = dl.SelectedTransactionStatusName;
                                steDetails.StatusChangedDate = dl.SelectedTransactionDate;
                                steDetails.SelectedDiligenceLease = dl;
                                transactionClosedDate = dl.ClosingDate;
                            }

                            if (dl.SelectedTransactionStatusId == (int)SamsTransactionStatus.Under_LOI &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Under_Contract) &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Closed_Dispositions))
                            {
                                steDetails.MaxPriorityTransactionStatusId = dl.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = dl.SelectedTransactionStatusName;
                                steDetails.StatusChangedDate = dl.SelectedTransactionDate;
                                steDetails.SelectedDiligenceLease = dl;
                                transactionClosedDate = dl.ClosingDate;
                            }

                            if (dl.SelectedTransactionStatusId == (int)SamsTransactionStatus.Under_Contract &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Closed_Dispositions))
                            {
                                steDetails.MaxPriorityTransactionStatusId = dl.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = dl.SelectedTransactionStatusName;
                                steDetails.StatusChangedDate = dl.SelectedTransactionDate;
                                steDetails.SelectedDiligenceLease = dl;
                                transactionClosedDate = dl.ClosingDate;
                            }

                            if (dl.SelectedTransactionStatusId == (int)SamsTransactionStatus.Closed_Dispositions)
                            {
                                steDetails.MaxPriorityTransactionStatusId = dl.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = dl.SelectedTransactionStatusName;
                                steDetails.StatusChangedDate = dl.SelectedTransactionDate;
                                steDetails.SelectedDiligenceLease = dl;
                                transactionClosedDate = dl.ClosingDate;
                            }


                            if (dl.SelectedTransactionStatusId == (int)SamsTransactionStatus.Closed_Dispositions)
                            {
                                canAddToList = true;
                            }
                        }


                        if (steDetails.SelectedDiligenceLease != null)
                        {
                            var dtClosedDate = "";
                            var daysToClose = 0;
                            if (transactionClosedDate != default(DateTime?))
                            {
                                dtClosedDate = transactionClosedDate.Value.ToString("MM/dd/yyyy");
                                daysToClose = (transactionClosedDate.Value - DateTime.Now).Days;
                                if (daysToClose < 0)
                                {
                                    daysToClose = 0;
                                }
                            }
                        }

                    }
                    else if (steDetails.AssetTypeId == (int)SamAssetType.LeaseWithPurchaseOption)
                    {
                        steDetails.DiligenceLeaseWithPurchaseList = GetDiligenceLeaseWithPurchaseList(steDetails.SiteDetailsId);

                        foreach (DiligenceLeaseWithPurchaseViewModel dl in steDetails.DiligenceLeaseWithPurchaseList)
                        {

                            steDetails.DiligenceLeaseWithPurchase = dl;
                            if (steDetails.MaxPriorityTransactionStatusId == 0)
                            {
                                steDetails.MaxPriorityTransactionStatusId = dl.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = dl.SelectedTransactionStatusName;

                                steDetails.DiligenceLeaseWithPurchase = dl;
                                transactionClosedDate = dl.ClosingDate;
                            }



                            if ((dl.SelectedTransactionStatusId == (int)TransactionStatus.LOI_Received ||
                                dl.SelectedTransactionStatusId == (int)TransactionStatus.Terminated_Acquisitions) &&
                                (dl.SelectedTransactionStatusId != (int)SamsTransactionStatus.Under_LOI &&
                                steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Under_Contract &&
                                steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Closed_Dispositions
                                ))
                            {
                                steDetails.MaxPriorityTransactionStatusId = dl.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = dl.SelectedTransactionStatusName;
                                steDetails.DiligenceLeaseWithPurchase = dl;
                                transactionClosedDate = dl.ClosingDate;
                            }

                            if (dl.SelectedTransactionStatusId == (int)SamsTransactionStatus.Under_LOI &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Under_Contract) &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Closed_Dispositions))
                            {
                                steDetails.MaxPriorityTransactionStatusId = dl.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = dl.SelectedTransactionStatusName;
                                steDetails.DiligenceLeaseWithPurchase = dl;
                                transactionClosedDate = dl.ClosingDate;
                            }

                            if (dl.SelectedTransactionStatusId == (int)SamsTransactionStatus.Under_Contract &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Closed_Dispositions))
                            {
                                steDetails.MaxPriorityTransactionStatusId = dl.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = dl.SelectedTransactionStatusName;
                                steDetails.DiligenceLeaseWithPurchase = dl;
                                transactionClosedDate = dl.ClosingDate;
                            }

                            if (dl.SelectedTransactionStatusId == (int)SamsTransactionStatus.Closed_Dispositions)
                            {
                                steDetails.MaxPriorityTransactionStatusId = dl.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = dl.SelectedTransactionStatusName;
                                steDetails.DiligenceLeaseWithPurchase = dl;
                                transactionClosedDate = dl.ClosingDate;
                            }

                            if (dl.SelectedTransactionStatusId == (int)SamsTransactionStatus.Closed_Dispositions)
                            {
                                canAddToList = true;
                            }
                        }



                        if (steDetails.DiligenceLeaseWithPurchase != null)
                        {

                            var dtClosedDate = "";
                            var daysToClose = 0;
                            if (transactionClosedDate != default(DateTime?))
                            {
                                dtClosedDate = transactionClosedDate.Value.ToString("MM/dd/yyyy");
                                daysToClose = (transactionClosedDate.Value - DateTime.Now).Days;
                                if (daysToClose < 0)
                                {
                                    daysToClose = 0;
                                }
                            }

                        }
                    }
                    else if (steDetails.AssetTypeId == (int)SamAssetType.SaleLeaseBack)
                    {
                        foreach (DiligenceDispositionsViewModel ddv in steDetails.DiligenceDispositions_SaleLeaseBack)
                        {

                            if (ddv.SelectedTransactionStatusId == (int)TransactionStatus.Closed_Acquisitions || ddv.SelectedTransactionStatusId == (int)TransactionStatus.Under_Contract || ddv.SelectedTransactionStatusId == (int)TransactionStatus.Under_LOI)
                            {
                                steDetails.CanAddTransactions = false;
                            }

                            if (ddv.SelectedTransactionStatusId == (int)TransactionStatus.Closed_Acquisitions)
                            {
                                steDetails.IsTransactionClosed = true;
                            }

                            if (steDetails.MaxPriorityTransactionStatusId == 0)
                            {
                                steDetails.MaxPriorityTransactionStatusId = ddv.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = ddv.SelectedTransactionStatusName;
                            }
                            steDetails.SelectedDiligenceDispositions = ddv;
                            transactionClosedDate = ddv.ClosingDate;


                            if ((ddv.SelectedTransactionStatusId == (int)TransactionStatus.LOI_Received ||
                                ddv.SelectedTransactionStatusId == (int)TransactionStatus.Terminated_Acquisitions) &&
                                (ddv.SelectedTransactionStatusId != (int)SamsTransactionStatus.Under_LOI &&
                                steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Under_Contract &&
                                steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Closed_Dispositions
                                ))
                            {
                                steDetails.MaxPriorityTransactionStatusId = ddv.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = ddv.SelectedTransactionStatusName;
                                steDetails.SelectedDiligenceDispositions = ddv;
                                transactionClosedDate = ddv.ClosingDate;
                            }

                            if (ddv.SelectedTransactionStatusId == (int)SamsTransactionStatus.Under_LOI &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Under_Contract) &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Closed_Dispositions))
                            {
                                steDetails.MaxPriorityTransactionStatusId = ddv.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = ddv.SelectedTransactionStatusName;
                                steDetails.SelectedDiligenceDispositions = ddv;
                                transactionClosedDate = ddv.ClosingDate;
                            }

                            if (ddv.SelectedTransactionStatusId == (int)SamsTransactionStatus.Under_Contract &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Closed_Dispositions))
                            {
                                steDetails.MaxPriorityTransactionStatusId = ddv.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = ddv.SelectedTransactionStatusName;
                                steDetails.SelectedDiligenceDispositions = ddv;
                                transactionClosedDate = ddv.ClosingDate;
                            }

                            if (ddv.SelectedTransactionStatusId == (int)SamsTransactionStatus.Closed_Dispositions)
                            {
                                steDetails.MaxPriorityTransactionStatusId = ddv.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = ddv.SelectedTransactionStatusName;
                                steDetails.SelectedDiligenceDispositions = ddv;
                                transactionClosedDate = ddv.ClosingDate;
                            }

                            if (ddv.SelectedTransactionStatusId == (int)SamsTransactionStatus.Closed_Dispositions)
                            {
                                canAddToList = true;
                            }
                        }

                        var dtClosedDate = "";
                        var daysToClose = 0;
                        if (transactionClosedDate != default(DateTime?))
                        {
                            dtClosedDate = transactionClosedDate.Value.ToString("MM/dd/yyyy");
                            daysToClose = (transactionClosedDate.Value - DateTime.Now).Days;
                            if (daysToClose < 0)
                            {
                                daysToClose = 0;
                            }
                        }

                    }


                    steDetails.TodoList = GetTodoList(steDetails.SiteDetailsId);
                    if (steDetails.TodoList.Count > 0)
                    {
                        steDetails.LatestComment = steDetails.TodoList[0].TodoText;
                    }

                    if (canAddToList)
                    {
                        surplusPropertiesList.Add(steDetails);
                    }

                }
                
                con.Close();
            }

            return View(surplusPropertiesList);
        }

        public RedirectToActionResult HideNotification(int periodId)
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
                SqlCommand cmd = new SqlCommand("HideNotification", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                cmd.Parameters.AddWithValue("period_id", periodId);

                cmd.ExecuteNonQuery();


                con.Close();
                return RedirectToAction("Dashboard");
            }
        }

        public RedirectToActionResult HidePropertyNotification(int propertyId)
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
                SqlCommand cmd = new SqlCommand("HideSurplusPropertyNotification", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                cmd.Parameters.AddWithValue("site_details_id", propertyId);

                cmd.ExecuteNonQuery();


                con.Close();
                return RedirectToAction("Dashboard");
            }
        }

        [HttpPost]
        public IActionResult SaveDateClosed(int PropertyId, DateTime DateClosed)
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
                SqlCommand cmd = new SqlCommand("UpdateCosedDate", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("property_id", PropertyId);
                cmd.Parameters.AddWithValue("closed_date", DateClosed);
                cmd.Parameters.AddWithValue("property_type", SamsPropertyType.Surplus);
                con.Open();

                cmd.ExecuteNonQuery();

                con.Close();

                return RedirectToAction("ViewSurplusProperty", new { propertyId = PropertyId });
            }
        }


        public IActionResult PublishProperty(int PropertyId, int canPublish)
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
                SqlCommand cmd = new SqlCommand("UpdatePublishStatus", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("property_id", PropertyId);
                cmd.Parameters.AddWithValue("property_type", SamsPropertyType.Surplus);

                cmd.Parameters.AddWithValue("can_publish", canPublish);


                con.Open();

                cmd.ExecuteNonQuery();

                con.Close();

                return RedirectToAction("ViewSurplusProperty", new { propertyId = PropertyId });
            }
        }



        public IActionResult GetTerminatedProperties()
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

            List<SiteDetails> surplusPropertiesList = new List<SiteDetails>();

            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {

                /*
                 * 1	Available
                 * 2	Under contract
                 * 3	Sold
                 * 4    Terminated
                 */
                SqlCommand cmd = new SqlCommand("GetPropertyListByStatus", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("property_status_id", 4);
                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var steDetails = new SiteDetails();
                    steDetails.SiteDetailsId = reader.IsDBNull(reader.GetOrdinal("site_details_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("site_details_id"));
                    steDetails.NamePrefix = reader.IsDBNull(reader.GetOrdinal("name_prefix")) ? "" : reader.GetString(reader.GetOrdinal("name_prefix"));
                    steDetails.FirstName = reader.IsDBNull(reader.GetOrdinal("first_name")) ? "" : reader.GetString(reader.GetOrdinal("first_name"));

                    steDetails.LastName = reader.IsDBNull(reader.GetOrdinal("last_name")) ? "" : reader.GetString(reader.GetOrdinal("last_name"));
                    steDetails.CompanyName = reader.IsDBNull(reader.GetOrdinal("company_name")) ? "" : reader.GetString(reader.GetOrdinal("company_name"));
                    steDetails.EmailAddress = reader.IsDBNull(reader.GetOrdinal("email_address")) ? "" : reader.GetString(reader.GetOrdinal("email_address"));
                    steDetails.Address = reader.IsDBNull(reader.GetOrdinal("address")) ? "" : reader.GetString(reader.GetOrdinal("address"));
                    steDetails.CityName = reader.IsDBNull(reader.GetOrdinal("city_name")) ? "" : reader.GetString(reader.GetOrdinal("city_name"));
                    steDetails.StateId = reader.IsDBNull(reader.GetOrdinal("state_id")) ? "" : reader.GetString(reader.GetOrdinal("state_id"));

                    steDetails.StateName = reader.IsDBNull(reader.GetOrdinal("state_name")) ? "" : reader.GetString(reader.GetOrdinal("state_name"));

                    steDetails.ZipCode = reader.IsDBNull(reader.GetOrdinal("zip_code")) ? "" : reader.GetString(reader.GetOrdinal("zip_code"));
                    steDetails.ContactNumber = reader.IsDBNull(reader.GetOrdinal("contact_number")) ? "" : reader.GetString(reader.GetOrdinal("contact_number"));
                    steDetails.SamsHoldingEmployee = reader.IsDBNull(reader.GetOrdinal("sams_holding_employee")) ? false : reader.GetBoolean(reader.GetOrdinal("sams_holding_employee"));
                    steDetails.MarketId = reader.IsDBNull(reader.GetOrdinal("market_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("market_id"));
                    steDetails.PropertyHeader = reader.IsDBNull(reader.GetOrdinal("property_header")) ? "" : reader.GetString(reader.GetOrdinal("property_header"));
                    steDetails.SiteAddress = reader.IsDBNull(reader.GetOrdinal("site_address")) ? "" : reader.GetString(reader.GetOrdinal("site_address"));
                    steDetails.SiteCity = reader.IsDBNull(reader.GetOrdinal("site_city")) ? "" : reader.GetString(reader.GetOrdinal("site_city"));
                    steDetails.SiteStateId = reader.IsDBNull(reader.GetOrdinal("site_state_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("site_state_id"));

                    steDetails.SiteStateName = reader.IsDBNull(reader.GetOrdinal("site_state_name")) ? "" : reader.GetString(reader.GetOrdinal("site_state_name"));

                    steDetails.SiteCounty = reader.IsDBNull(reader.GetOrdinal("site_county")) ? "" : reader.GetString(reader.GetOrdinal("site_county"));
                    steDetails.SiteCrossStreetName = reader.IsDBNull(reader.GetOrdinal("site_cross_street_name")) ? "" : reader.GetString(reader.GetOrdinal("site_cross_street_name"));
                    steDetails.IsPropertyAvailable = reader.IsDBNull(reader.GetOrdinal("is_property_available")) ? true : reader.GetBoolean(reader.GetOrdinal("is_property_available"));
                    steDetails.Zoning = reader.IsDBNull(reader.GetOrdinal("zoning")) ? "" : reader.GetString(reader.GetOrdinal("zoning"));
                    steDetails.LotSize = reader.IsDBNull(reader.GetOrdinal("lot_size")) ? "" : reader.GetString(reader.GetOrdinal("lot_size"));

                    steDetails.SalesPrice = reader.IsDBNull(reader.GetOrdinal("sales_price")) ? "" : reader.GetString(reader.GetOrdinal("sales_price"));
                    //steDetails.SalesPrice = Helper.FormatCurrency("$", steDetails.SalesPrice);

                    steDetails.Comments = reader.IsDBNull(reader.GetOrdinal("comments")) ? "" : reader.GetString(reader.GetOrdinal("comments"));

                    steDetails.CreatedDate = reader.IsDBNull(reader.GetOrdinal("created_date")) ? DateTime.Now : reader.GetDateTime(reader.GetOrdinal("created_date"));
                    steDetails.PropertyType = reader.IsDBNull(reader.GetOrdinal("property_type")) ? 0 : reader.GetInt32(reader.GetOrdinal("property_type"));

                    steDetails.ImageName = reader.IsDBNull(reader.GetOrdinal("image_name")) ? "" : reader.GetString(reader.GetOrdinal("image_name"));

                    steDetails.AssetTypeId = reader.IsDBNull(reader.GetOrdinal("asset_type_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("asset_type_id"));
                    steDetails.AssetTypeName = reader.IsDBNull(reader.GetOrdinal("asset_type_name")) ? "" : reader.GetString(reader.GetOrdinal("asset_type_name"));
                    steDetails.AssetStatus = reader.IsDBNull(reader.GetOrdinal("asset_status")) ? 0 : reader.GetInt32(reader.GetOrdinal("asset_status"));
                    steDetails.AssetId = reader.IsDBNull(reader.GetOrdinal("asset_id")) ? "" : reader.GetString(reader.GetOrdinal("asset_id"));

                    steDetails.SelectedPropertyStatusId = reader.IsDBNull(reader.GetOrdinal("property_status_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("property_status_id"));
                    steDetails.SelectedPropertyStatus = reader.IsDBNull(reader.GetOrdinal("property_status")) ? "" : reader.GetString(reader.GetOrdinal("property_status"));

                    if (steDetails.SiteAddress.Length > 15)
                    {
                        steDetails.SiteAddressSmall = steDetails.SiteAddress.Substring(0, 15) + "..";
                    }
                    else
                    {
                        steDetails.SiteAddressSmall = steDetails.SiteAddress;
                    }

                    if (steDetails.AssetStatus == 0)
                    {
                        steDetails.AssetStatusName = "Available";
                    }
                    else
                    {
                        steDetails.AssetStatusName = "Sold";
                    }
                    surplusPropertiesList.Add(steDetails);
                }
                con.Close();
            }

            return View(surplusPropertiesList);
        }

        public IActionResult GetDispositionCriticalItems(int diligenceDispositionsId, int propertyId)
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

            var periodList = new List<PeriodViewModel>();

            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("GetPeriodList", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("property_id", propertyId);
                cmd.Parameters.AddWithValue("property_type", (int)SamsPropertyType.Surplus);
                cmd.Parameters.AddWithValue("transaction_id", diligenceDispositionsId);
                cmd.Parameters.AddWithValue("period_type", "Disposition"); 
                con.Open();

                SqlDataReader readerAssetType = cmd.ExecuteReader();
                while (readerAssetType.Read())
                {
                    var periodView = new PeriodViewModel();

                    periodView.PeriodId = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("period_id")) ? 0 : readerAssetType.GetInt32(readerAssetType.GetOrdinal("period_id"));
                    periodView.PropertyId = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("property_id")) ? 0 : readerAssetType.GetInt32(readerAssetType.GetOrdinal("property_id"));
                    periodView.PropertyType = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("property_type")) ? 0 : readerAssetType.GetInt32(readerAssetType.GetOrdinal("property_type"));

                    periodView.PeriodMaster = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("period_master")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("period_master"));

                    periodView.StartDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("start_date")) ? DateTime.Now : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("start_date"));
                    periodView.EndDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("end_date")) ? DateTime.Now : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("end_date"));


                    periodView.PeriodNotes = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("period_notes")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("period_notes"));
                    periodView.PeriodType = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("period_type")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("period_type"));
                    periodList.Add(periodView);
                }

                con.Close();

            }

            ViewData["propertyId"] = propertyId;
            ViewData["transactionId"] = diligenceDispositionsId;
            return View(periodList);
        }


        public IActionResult GetLeaseCriticalItems(int diligenceLeaseId, int propertyId)
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

            var periodList = new List<PeriodViewModel>();

            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("GetPeriodList", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("property_id", propertyId);
                cmd.Parameters.AddWithValue("property_type", (int)SamsPropertyType.Surplus);
                cmd.Parameters.AddWithValue("transaction_id", diligenceLeaseId);
                cmd.Parameters.AddWithValue("period_type", "Lease");
                con.Open();

                SqlDataReader readerAssetType = cmd.ExecuteReader();
                while (readerAssetType.Read())
                {
                    var periodView = new PeriodViewModel();

                    periodView.PeriodId = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("period_id")) ? 0 : readerAssetType.GetInt32(readerAssetType.GetOrdinal("period_id"));
                    periodView.PropertyId = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("property_id")) ? 0 : readerAssetType.GetInt32(readerAssetType.GetOrdinal("property_id"));
                    periodView.PropertyType = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("property_type")) ? 0 : readerAssetType.GetInt32(readerAssetType.GetOrdinal("property_type"));

                    periodView.PeriodMaster = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("period_master")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("period_master"));

                    periodView.StartDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("start_date")) ? DateTime.Now : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("start_date"));
                    periodView.EndDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("end_date")) ? DateTime.Now : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("end_date"));


                    periodView.PeriodNotes = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("period_notes")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("period_notes"));
                    periodView.PeriodType = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("period_type")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("period_type"));
                    periodList.Add(periodView);
                }

                con.Close();

            }

            ViewData["propertyId"] = propertyId;
            ViewData["transactionId"] = diligenceLeaseId;
            return View(periodList);
        }


        List<TransactionStatusModel> GetTransactionStatusList()
        {
            var transactionStatusList = new List<TransactionStatusModel>();

            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("GetTransactionStatusList", con);
                cmd.CommandType = CommandType.StoredProcedure;

                con.Open();

                SqlDataReader readerAssetType = cmd.ExecuteReader();
                while (readerAssetType.Read())
                {
                    var transactionStatus = new TransactionStatusModel();

                    transactionStatus.TransactionStatusId = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("transaction_status_id")) ? 0 : readerAssetType.GetInt32(readerAssetType.GetOrdinal("transaction_status_id"));
                    transactionStatus.TransactionStatusName = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("transaction_status_name")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("transaction_status_name"));
                    transactionStatusList.Add(transactionStatus);
                }

                con.Close();

            }

            return transactionStatusList;
        }

        List<TransactionStatusModel> GetTransactionStatusList(int currentTransactionStatusId, int propertyTransactionStatusId)
        {
            var transactionStatusList = new List<TransactionStatusModel>();

            currentTransactionStatusId = propertyTransactionStatusId;

            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("GetTransactionStatusList", con);
                cmd.CommandType = CommandType.StoredProcedure;

                con.Open();

                SqlDataReader readerAssetType = cmd.ExecuteReader();
                while (readerAssetType.Read())
                {
                    var transactionStatus = new TransactionStatusModel();

                    transactionStatus.TransactionStatusId = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("transaction_status_id")) ? 0 : readerAssetType.GetInt32(readerAssetType.GetOrdinal("transaction_status_id"));
                    transactionStatus.TransactionStatusName = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("transaction_status_name")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("transaction_status_name"));

                    if(currentTransactionStatusId > 0)
                    {
                        if (currentTransactionStatusId == (int)SamsTransactionStatus.Under_LOI)
                        {
                            if (transactionStatus.TransactionStatusId == (int)SamsTransactionStatus.Under_Contract ||
                                transactionStatus.TransactionStatusId == (int)SamsTransactionStatus.Terminated_Dispositions ||
                                transactionStatus.TransactionStatusId == (int)SamsTransactionStatus.Closed_Dispositions ||
                                transactionStatus.TransactionStatusId == (int)SamsTransactionStatus.Under_LOI)
                            {
                                transactionStatusList.Add(transactionStatus);
                            }
                        }
                        else if (currentTransactionStatusId == (int)SamsTransactionStatus.Under_Contract)
                        {
                            if (transactionStatus.TransactionStatusId == (int)SamsTransactionStatus.Terminated_Dispositions ||
                                transactionStatus.TransactionStatusId == (int)SamsTransactionStatus.Closed_Dispositions ||
                                transactionStatus.TransactionStatusId == (int)SamsTransactionStatus.Under_Contract)
                            {
                                transactionStatusList.Add(transactionStatus);
                            }
                        }
                        else if (currentTransactionStatusId == (int)SamsTransactionStatus.Closed_Dispositions)
                        {
                            if (transactionStatus.TransactionStatusId == (int)SamsTransactionStatus.Closed_Dispositions ||
                                transactionStatus.TransactionStatusId == (int)SamsTransactionStatus.Terminated_Dispositions)
                            {
                                transactionStatusList.Add(transactionStatus);
                            }
                        }

                        else if (currentTransactionStatusId == (int)SamsTransactionStatus.LOI_Received ||
                            currentTransactionStatusId == (int)SamsTransactionStatus.Terminated_Dispositions)
                        {
                            transactionStatusList.Add(transactionStatus);
                        }
                    }
                    else
                    {
                        transactionStatusList.Add(transactionStatus);
                    }


                }

                con.Close();

            }

            return transactionStatusList;
        }


        public IActionResult ShowListByTransactionStatus(int transactionStatusId)
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


            List<SiteDetails> surplusPropertiesList = new List<SiteDetails>();

            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {

                /*
                 * 1	Available
                 * 2	Under contract
                 * 3	Sold
                 */
                SqlCommand cmd = new SqlCommand("GetPropertyListByCategory", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("asset_status", 0);

                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var steDetails = new SiteDetails();
                    steDetails.SiteDetailsId = reader.IsDBNull(reader.GetOrdinal("site_details_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("site_details_id"));
                    steDetails.NamePrefix = reader.IsDBNull(reader.GetOrdinal("name_prefix")) ? "" : reader.GetString(reader.GetOrdinal("name_prefix"));
                    steDetails.FirstName = reader.IsDBNull(reader.GetOrdinal("first_name")) ? "" : reader.GetString(reader.GetOrdinal("first_name"));

                    steDetails.LastName = reader.IsDBNull(reader.GetOrdinal("last_name")) ? "" : reader.GetString(reader.GetOrdinal("last_name"));
                    steDetails.CompanyName = reader.IsDBNull(reader.GetOrdinal("company_name")) ? "" : reader.GetString(reader.GetOrdinal("company_name"));
                    steDetails.EmailAddress = reader.IsDBNull(reader.GetOrdinal("email_address")) ? "" : reader.GetString(reader.GetOrdinal("email_address"));
                    steDetails.Address = reader.IsDBNull(reader.GetOrdinal("address")) ? "" : reader.GetString(reader.GetOrdinal("address"));
                    steDetails.CityName = reader.IsDBNull(reader.GetOrdinal("city_name")) ? "" : reader.GetString(reader.GetOrdinal("city_name"));
                    steDetails.StateId = reader.IsDBNull(reader.GetOrdinal("state_id")) ? "" : reader.GetString(reader.GetOrdinal("state_id"));

                    steDetails.StateName = reader.IsDBNull(reader.GetOrdinal("state_name")) ? "" : reader.GetString(reader.GetOrdinal("state_name"));

                    steDetails.ZipCode = reader.IsDBNull(reader.GetOrdinal("zip_code")) ? "" : reader.GetString(reader.GetOrdinal("zip_code"));
                    steDetails.ContactNumber = reader.IsDBNull(reader.GetOrdinal("contact_number")) ? "" : reader.GetString(reader.GetOrdinal("contact_number"));
                    steDetails.SamsHoldingEmployee = reader.IsDBNull(reader.GetOrdinal("sams_holding_employee")) ? false : reader.GetBoolean(reader.GetOrdinal("sams_holding_employee"));
                    steDetails.MarketId = reader.IsDBNull(reader.GetOrdinal("market_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("market_id"));
                    steDetails.PropertyHeader = reader.IsDBNull(reader.GetOrdinal("property_header")) ? "" : reader.GetString(reader.GetOrdinal("property_header"));
                    steDetails.SiteAddress = reader.IsDBNull(reader.GetOrdinal("site_address")) ? "" : reader.GetString(reader.GetOrdinal("site_address"));
                    steDetails.SiteCity = reader.IsDBNull(reader.GetOrdinal("site_city")) ? "" : reader.GetString(reader.GetOrdinal("site_city"));
                    steDetails.SiteStateId = reader.IsDBNull(reader.GetOrdinal("site_state_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("site_state_id"));

                    steDetails.SiteStateName = reader.IsDBNull(reader.GetOrdinal("site_state_name")) ? "" : reader.GetString(reader.GetOrdinal("site_state_name"));

                    steDetails.SiteCounty = reader.IsDBNull(reader.GetOrdinal("site_county")) ? "" : reader.GetString(reader.GetOrdinal("site_county"));
                    steDetails.SiteCrossStreetName = reader.IsDBNull(reader.GetOrdinal("site_cross_street_name")) ? "" : reader.GetString(reader.GetOrdinal("site_cross_street_name"));
                    steDetails.IsPropertyAvailable = reader.IsDBNull(reader.GetOrdinal("is_property_available")) ? true : reader.GetBoolean(reader.GetOrdinal("is_property_available"));
                    steDetails.Zoning = reader.IsDBNull(reader.GetOrdinal("zoning")) ? "" : reader.GetString(reader.GetOrdinal("zoning"));
                    steDetails.LotSize = reader.IsDBNull(reader.GetOrdinal("lot_size")) ? "" : reader.GetString(reader.GetOrdinal("lot_size"));

                    steDetails.SalesPrice = reader.IsDBNull(reader.GetOrdinal("sales_price")) ? "" : reader.GetString(reader.GetOrdinal("sales_price"));
                    //steDetails.SalesPrice = Helper.FormatCurrency("$", steDetails.SalesPrice);

                    steDetails.Comments = reader.IsDBNull(reader.GetOrdinal("comments")) ? "" : reader.GetString(reader.GetOrdinal("comments"));

                    steDetails.CreatedDate = reader.IsDBNull(reader.GetOrdinal("created_date")) ? DateTime.Now : reader.GetDateTime(reader.GetOrdinal("created_date"));
                    steDetails.PropertyType = reader.IsDBNull(reader.GetOrdinal("property_type")) ? 0 : reader.GetInt32(reader.GetOrdinal("property_type"));

                    steDetails.ImageName = reader.IsDBNull(reader.GetOrdinal("image_name")) ? "" : reader.GetString(reader.GetOrdinal("image_name"));

                    steDetails.AssetTypeId = reader.IsDBNull(reader.GetOrdinal("asset_type_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("asset_type_id"));
                    steDetails.AssetTypeName = reader.IsDBNull(reader.GetOrdinal("asset_type_name")) ? "" : reader.GetString(reader.GetOrdinal("asset_type_name"));
                    steDetails.AssetStatus = reader.IsDBNull(reader.GetOrdinal("asset_status")) ? 0 : reader.GetInt32(reader.GetOrdinal("asset_status"));
                    steDetails.AssetId = reader.IsDBNull(reader.GetOrdinal("asset_id")) ? "" : reader.GetString(reader.GetOrdinal("asset_id"));

                    steDetails.SelectedPropertyStatusId = reader.IsDBNull(reader.GetOrdinal("property_status_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("property_status_id"));
                    steDetails.SelectedPropertyStatus = reader.IsDBNull(reader.GetOrdinal("property_status")) ? "" : reader.GetString(reader.GetOrdinal("property_status"));
                    steDetails.PropertyHeaderLine2 = reader.IsDBNull(reader.GetOrdinal("property_header_line_2")) ? "" : reader.GetString(reader.GetOrdinal("property_header_line_2"));


                    if (steDetails.SiteAddress.Length > 15)
                    {
                        steDetails.SiteAddressSmall = steDetails.SiteAddress.Substring(0, 15) + "..";
                    }
                    else
                    {
                        steDetails.SiteAddressSmall = steDetails.SiteAddress;
                    }

                    if (steDetails.AssetStatus == 0)
                    {
                        steDetails.AssetStatusName = "Available";
                    }
                    else
                    {
                        steDetails.AssetStatusName = "Sold";
                    }

                    steDetails.TransactionStatusName = "";

                    steDetails.DiligenceDispositions = GetDiligenceDispositions(steDetails.SiteDetailsId);

                    steDetails.SelectedDiligenceDispositions = new DiligenceDispositionsViewModel();

                    int saleLoi = 0, saleUnderContract = 0, saleTerminated = 0, saleClosed = 0;



                    steDetails.DiligenceAcquisitions = GetDiligenceAcquisition(steDetails.SiteDetailsId);
                    steDetails.DiligenceDispositions = GetDiligenceDispositions(steDetails.SiteDetailsId);
                    steDetails.DiligenceLease = GetDiligenceLease(steDetails.SiteDetailsId);
                    steDetails.DiligenceLeaseList = GetDiligenceLeaseList(steDetails.SiteDetailsId);
                    steDetails.DiligenceDispositions_SaleLeaseBack = GetDiligenceDispositions_SaleLeaseBack(steDetails.SiteDetailsId);

                    // steDetails.DiligenceDispositions = GetDiligenceDispositions(steDetails.SiteDetailsId);

                    steDetails.SelectedDiligenceDispositions = new DiligenceDispositionsViewModel();
                    DateTime? transactionClosedDate = default(DateTime?);

                    steDetails.DiligenceLeaseWithPurchase = new DiligenceLeaseWithPurchaseViewModel();
                    steDetails.SelectedDiligenceLease = new DiligenceLeaseViewModel();
                    steDetails.DiligenceLeaseWithPurchase = new DiligenceLeaseWithPurchaseViewModel();


                    if (steDetails.AssetTypeId == (int)SamAssetType.Fee || steDetails.AssetTypeId == (int)SamAssetType.FeeSubjectToLease)
                    {
                        foreach (DiligenceDispositionsViewModel ddm in steDetails.DiligenceDispositions)
                        {

                            if (steDetails.MaxPriorityTransactionStatusId == 0)
                            {
                                steDetails.MaxPriorityTransactionStatusId = ddm.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = ddm.SelectedTransactionStatusName;
                                steDetails.SelectedDiligenceDispositions = ddm;
                                transactionClosedDate = ddm.ClosingDate;
                            }



                            if ((ddm.SelectedTransactionStatusId == (int)TransactionStatus.LOI_Received ||
                                ddm.SelectedTransactionStatusId == (int)TransactionStatus.Terminated_Acquisitions) &&
                                (ddm.SelectedTransactionStatusId != (int)SamsTransactionStatus.Under_LOI &&
                                steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Under_Contract &&
                                steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Closed_Dispositions
                                ))
                            {
                                steDetails.MaxPriorityTransactionStatusId = ddm.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = ddm.SelectedTransactionStatusName;
                                steDetails.StatusChangedDate = ddm.SelectedTransactionDate;
                                steDetails.SelectedDiligenceDispositions = ddm;
                                transactionClosedDate = ddm.ClosingDate;
                            }

                            if (ddm.SelectedTransactionStatusId == (int)SamsTransactionStatus.Under_LOI &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Under_Contract) &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Closed_Dispositions))
                            {
                                steDetails.MaxPriorityTransactionStatusId = ddm.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = ddm.SelectedTransactionStatusName;
                                steDetails.StatusChangedDate = ddm.SelectedTransactionDate;
                                steDetails.SelectedDiligenceDispositions = ddm;
                                transactionClosedDate = ddm.ClosingDate;
                            }

                            if (ddm.SelectedTransactionStatusId == (int)SamsTransactionStatus.Under_Contract &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Closed_Dispositions))
                            {
                                steDetails.MaxPriorityTransactionStatusId = ddm.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = ddm.SelectedTransactionStatusName;
                                steDetails.StatusChangedDate = ddm.SelectedTransactionDate;
                                steDetails.SelectedDiligenceDispositions = ddm;
                                transactionClosedDate = ddm.ClosingDate;
                            }

                            if (ddm.SelectedTransactionStatusId == (int)SamsTransactionStatus.Closed_Dispositions)
                            {
                                steDetails.MaxPriorityTransactionStatusId = ddm.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = ddm.SelectedTransactionStatusName;
                                steDetails.StatusChangedDate = ddm.SelectedTransactionDate;
                                steDetails.SelectedDiligenceDispositions = ddm;
                                transactionClosedDate = ddm.ClosingDate;
                            }

                        }


                        var dtClosedDate = "";
                        var daysToClose = 0;
                        if (transactionClosedDate != default(DateTime?))
                        {
                            dtClosedDate = transactionClosedDate.Value.ToString("MM/dd/yyyy");
                            daysToClose = (transactionClosedDate.Value - DateTime.Now).Days;
                            if (daysToClose < 0)
                            {
                                daysToClose = 0;
                            }
                        }
                    }
                    else if (steDetails.AssetTypeId == (int)SamAssetType.Lease)
                    {
                        steDetails.DiligenceLeaseList = GetDiligenceLeaseList(steDetails.SiteDetailsId);
                        int leaseLoi = 0, leaseUnderContract = 0, leaseTerminated = 0, leaseClosed = 0;
                        foreach (DiligenceLeaseViewModel dl in steDetails.DiligenceLeaseList)
                        {
                            if (dl.SelectedTransactionStatusId == (int)TransactionStatus.Under_LOI)
                            {
                                leaseLoi = leaseLoi + 1;
                            }
                            else if (dl.SelectedTransactionStatusId == (int)TransactionStatus.Under_Contract)
                            {
                                leaseUnderContract = leaseUnderContract + 1;
                            }
                            else if (dl.SelectedTransactionStatusId == (int)TransactionStatus.Terminated_Acquisitions)
                            {
                                leaseTerminated = leaseTerminated + 1;
                            }
                            else if (dl.SelectedTransactionStatusId == (int)TransactionStatus.Closed_Acquisitions)
                            {
                                leaseClosed = leaseClosed + 1;
                            }

                            if (steDetails.MaxPriorityTransactionStatusId == 0)
                            {
                                steDetails.MaxPriorityTransactionStatusId = dl.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = dl.SelectedTransactionStatusName;
                                steDetails.SelectedDiligenceLease = dl;
                                transactionClosedDate = dl.ClosingDate;
                            }



                            if ((dl.SelectedTransactionStatusId == (int)TransactionStatus.LOI_Received ||
                                dl.SelectedTransactionStatusId == (int)TransactionStatus.Terminated_Acquisitions) &&
                                (dl.SelectedTransactionStatusId != (int)SamsTransactionStatus.Under_LOI &&
                                steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Under_Contract &&
                                steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Closed_Dispositions
                                ))
                            {
                                steDetails.MaxPriorityTransactionStatusId = dl.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = dl.SelectedTransactionStatusName;
                                steDetails.StatusChangedDate = dl.SelectedTransactionDate;
                                steDetails.SelectedDiligenceLease = dl;
                                transactionClosedDate = dl.ClosingDate;
                            }

                            if (dl.SelectedTransactionStatusId == (int)SamsTransactionStatus.Under_LOI &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Under_Contract) &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Closed_Dispositions))
                            {
                                steDetails.MaxPriorityTransactionStatusId = dl.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = dl.SelectedTransactionStatusName;
                                steDetails.StatusChangedDate = dl.SelectedTransactionDate;
                                steDetails.SelectedDiligenceLease = dl;
                                transactionClosedDate = dl.ClosingDate;
                            }

                            if (dl.SelectedTransactionStatusId == (int)SamsTransactionStatus.Under_Contract &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Closed_Dispositions))
                            {
                                steDetails.MaxPriorityTransactionStatusId = dl.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = dl.SelectedTransactionStatusName;
                                steDetails.StatusChangedDate = dl.SelectedTransactionDate;
                                steDetails.SelectedDiligenceLease = dl;
                                transactionClosedDate = dl.ClosingDate;
                            }

                            if (dl.SelectedTransactionStatusId == (int)SamsTransactionStatus.Closed_Dispositions)
                            {
                                steDetails.MaxPriorityTransactionStatusId = dl.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = dl.SelectedTransactionStatusName;
                                steDetails.StatusChangedDate = dl.SelectedTransactionDate;
                                steDetails.SelectedDiligenceLease = dl;
                                transactionClosedDate = dl.ClosingDate;
                            }
                        }


                        if (steDetails.SelectedDiligenceLease != null)
                        {
                            var dtClosedDate = "";
                            var daysToClose = 0;
                            if (transactionClosedDate != default(DateTime?))
                            {
                                dtClosedDate = transactionClosedDate.Value.ToString("MM/dd/yyyy");
                                daysToClose = (transactionClosedDate.Value - DateTime.Now).Days;
                                if (daysToClose < 0)
                                {
                                    daysToClose = 0;
                                }
                            }
                        }

                    }
                    else if (steDetails.AssetTypeId == (int)SamAssetType.LeaseWithPurchaseOption)
                    {
                        steDetails.DiligenceLeaseWithPurchaseList = GetDiligenceLeaseWithPurchaseList(steDetails.SiteDetailsId);

                        foreach (DiligenceLeaseWithPurchaseViewModel dl in steDetails.DiligenceLeaseWithPurchaseList)
                        {

                            steDetails.DiligenceLeaseWithPurchase = dl;
                            if (steDetails.MaxPriorityTransactionStatusId == 0)
                            {
                                steDetails.MaxPriorityTransactionStatusId = dl.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = dl.SelectedTransactionStatusName;

                                steDetails.DiligenceLeaseWithPurchase = dl;
                                transactionClosedDate = dl.ClosingDate;
                            }



                            if ((dl.SelectedTransactionStatusId == (int)TransactionStatus.LOI_Received ||
                                dl.SelectedTransactionStatusId == (int)TransactionStatus.Terminated_Acquisitions) &&
                                (dl.SelectedTransactionStatusId != (int)SamsTransactionStatus.Under_LOI &&
                                steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Under_Contract &&
                                steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Closed_Dispositions
                                ))
                            {
                                steDetails.MaxPriorityTransactionStatusId = dl.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = dl.SelectedTransactionStatusName;
                                steDetails.DiligenceLeaseWithPurchase = dl;
                                transactionClosedDate = dl.ClosingDate;
                            }

                            if (dl.SelectedTransactionStatusId == (int)SamsTransactionStatus.Under_LOI &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Under_Contract) &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Closed_Dispositions))
                            {
                                steDetails.MaxPriorityTransactionStatusId = dl.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = dl.SelectedTransactionStatusName;
                                steDetails.DiligenceLeaseWithPurchase = dl;
                                transactionClosedDate = dl.ClosingDate;
                            }

                            if (dl.SelectedTransactionStatusId == (int)SamsTransactionStatus.Under_Contract &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Closed_Dispositions))
                            {
                                steDetails.MaxPriorityTransactionStatusId = dl.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = dl.SelectedTransactionStatusName;
                                steDetails.DiligenceLeaseWithPurchase = dl;
                                transactionClosedDate = dl.ClosingDate;
                            }

                            if (dl.SelectedTransactionStatusId == (int)SamsTransactionStatus.Closed_Dispositions)
                            {
                                steDetails.MaxPriorityTransactionStatusId = dl.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = dl.SelectedTransactionStatusName;
                                steDetails.DiligenceLeaseWithPurchase = dl;
                                transactionClosedDate = dl.ClosingDate;
                            }
                        }



                        if (steDetails.DiligenceLeaseWithPurchase != null)
                        {

                            var dtClosedDate = "";
                            var daysToClose = 0;
                            if (transactionClosedDate != default(DateTime?))
                            {
                                dtClosedDate = transactionClosedDate.Value.ToString("MM/dd/yyyy");
                                daysToClose = (transactionClosedDate.Value - DateTime.Now).Days;
                                if (daysToClose < 0)
                                {
                                    daysToClose = 0;
                                }
                            }

                        }
                    }
                    else if (steDetails.AssetTypeId == (int)SamAssetType.SaleLeaseBack)
                    {
                        foreach (DiligenceDispositionsViewModel ddv in steDetails.DiligenceDispositions_SaleLeaseBack)
                        {

                            if (ddv.SelectedTransactionStatusId == (int)TransactionStatus.Closed_Acquisitions || ddv.SelectedTransactionStatusId == (int)TransactionStatus.Under_Contract || ddv.SelectedTransactionStatusId == (int)TransactionStatus.Under_LOI)
                            {
                                steDetails.CanAddTransactions = false;
                            }

                            if (ddv.SelectedTransactionStatusId == (int)TransactionStatus.Closed_Acquisitions)
                            {
                                steDetails.IsTransactionClosed = true;
                            }

                            if (steDetails.MaxPriorityTransactionStatusId == 0)
                            {
                                steDetails.MaxPriorityTransactionStatusId = ddv.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = ddv.SelectedTransactionStatusName;
                            }
                            steDetails.SelectedDiligenceDispositions = ddv;
                            transactionClosedDate = ddv.ClosingDate;


                            if ((ddv.SelectedTransactionStatusId == (int)TransactionStatus.LOI_Received ||
                                ddv.SelectedTransactionStatusId == (int)TransactionStatus.Terminated_Acquisitions) &&
                                (ddv.SelectedTransactionStatusId != (int)SamsTransactionStatus.Under_LOI &&
                                steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Under_Contract &&
                                steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Closed_Dispositions
                                ))
                            {
                                steDetails.MaxPriorityTransactionStatusId = ddv.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = ddv.SelectedTransactionStatusName;
                                steDetails.SelectedDiligenceDispositions = ddv;
                                transactionClosedDate = ddv.ClosingDate;
                            }

                            if (ddv.SelectedTransactionStatusId == (int)SamsTransactionStatus.Under_LOI &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Under_Contract) &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Closed_Dispositions))
                            {
                                steDetails.MaxPriorityTransactionStatusId = ddv.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = ddv.SelectedTransactionStatusName;
                                steDetails.SelectedDiligenceDispositions = ddv;
                                transactionClosedDate = ddv.ClosingDate;
                            }

                            if (ddv.SelectedTransactionStatusId == (int)SamsTransactionStatus.Under_Contract &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Closed_Dispositions))
                            {
                                steDetails.MaxPriorityTransactionStatusId = ddv.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = ddv.SelectedTransactionStatusName;
                                steDetails.SelectedDiligenceDispositions = ddv;
                                transactionClosedDate = ddv.ClosingDate;
                            }

                            if (ddv.SelectedTransactionStatusId == (int)SamsTransactionStatus.Closed_Dispositions)
                            {
                                steDetails.MaxPriorityTransactionStatusId = ddv.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = ddv.SelectedTransactionStatusName;
                                steDetails.SelectedDiligenceDispositions = ddv;
                                transactionClosedDate = ddv.ClosingDate;
                            }


                        }

                        var dtClosedDate = "";
                        var daysToClose = 0;
                        if (transactionClosedDate != default(DateTime?))
                        {
                            dtClosedDate = transactionClosedDate.Value.ToString("MM/dd/yyyy");
                            daysToClose = (transactionClosedDate.Value - DateTime.Now).Days;
                            if (daysToClose < 0)
                            {
                                daysToClose = 0;
                            }
                        }

                    }


                    steDetails.TodoList = GetTodoList(steDetails.SiteDetailsId);
                    if (steDetails.TodoList.Count > 0)
                    {
                        steDetails.LatestComment = steDetails.TodoList[0].TodoText;
                    }

                    if (steDetails.MaxPriorityTransactionStatusId == transactionStatusId)
                    {
                        surplusPropertiesList.Add(steDetails);
                    }

                }
                con.Close();
            }


            return View(surplusPropertiesList);
        }

        int GetTotalCountByTransactionStatus(int transactionStatusId)
        {
            

            List<SiteDetails> surplusPropertiesList = new List<SiteDetails>();

            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {

                /*
                 * 1	Available
                 * 2	Under contract
                 * 3	Sold
                 */
                SqlCommand cmd = new SqlCommand("GetPropertyListByCategory", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("asset_status", 0);

                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var steDetails = new SiteDetails();
                    steDetails.SiteDetailsId = reader.IsDBNull(reader.GetOrdinal("site_details_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("site_details_id"));
                    steDetails.NamePrefix = reader.IsDBNull(reader.GetOrdinal("name_prefix")) ? "" : reader.GetString(reader.GetOrdinal("name_prefix"));
                    steDetails.FirstName = reader.IsDBNull(reader.GetOrdinal("first_name")) ? "" : reader.GetString(reader.GetOrdinal("first_name"));

                    steDetails.LastName = reader.IsDBNull(reader.GetOrdinal("last_name")) ? "" : reader.GetString(reader.GetOrdinal("last_name"));
                    steDetails.CompanyName = reader.IsDBNull(reader.GetOrdinal("company_name")) ? "" : reader.GetString(reader.GetOrdinal("company_name"));
                    steDetails.EmailAddress = reader.IsDBNull(reader.GetOrdinal("email_address")) ? "" : reader.GetString(reader.GetOrdinal("email_address"));
                    steDetails.Address = reader.IsDBNull(reader.GetOrdinal("address")) ? "" : reader.GetString(reader.GetOrdinal("address"));
                    steDetails.CityName = reader.IsDBNull(reader.GetOrdinal("city_name")) ? "" : reader.GetString(reader.GetOrdinal("city_name"));
                    steDetails.StateId = reader.IsDBNull(reader.GetOrdinal("state_id")) ? "" : reader.GetString(reader.GetOrdinal("state_id"));

                    steDetails.StateName = reader.IsDBNull(reader.GetOrdinal("state_name")) ? "" : reader.GetString(reader.GetOrdinal("state_name"));

                    steDetails.ZipCode = reader.IsDBNull(reader.GetOrdinal("zip_code")) ? "" : reader.GetString(reader.GetOrdinal("zip_code"));
                    steDetails.ContactNumber = reader.IsDBNull(reader.GetOrdinal("contact_number")) ? "" : reader.GetString(reader.GetOrdinal("contact_number"));
                    steDetails.SamsHoldingEmployee = reader.IsDBNull(reader.GetOrdinal("sams_holding_employee")) ? false : reader.GetBoolean(reader.GetOrdinal("sams_holding_employee"));
                    steDetails.MarketId = reader.IsDBNull(reader.GetOrdinal("market_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("market_id"));
                    steDetails.PropertyHeader = reader.IsDBNull(reader.GetOrdinal("property_header")) ? "" : reader.GetString(reader.GetOrdinal("property_header"));
                    steDetails.SiteAddress = reader.IsDBNull(reader.GetOrdinal("site_address")) ? "" : reader.GetString(reader.GetOrdinal("site_address"));
                    steDetails.SiteCity = reader.IsDBNull(reader.GetOrdinal("site_city")) ? "" : reader.GetString(reader.GetOrdinal("site_city"));
                    steDetails.SiteStateId = reader.IsDBNull(reader.GetOrdinal("site_state_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("site_state_id"));

                    steDetails.SiteStateName = reader.IsDBNull(reader.GetOrdinal("site_state_name")) ? "" : reader.GetString(reader.GetOrdinal("site_state_name"));

                    steDetails.SiteCounty = reader.IsDBNull(reader.GetOrdinal("site_county")) ? "" : reader.GetString(reader.GetOrdinal("site_county"));
                    steDetails.SiteCrossStreetName = reader.IsDBNull(reader.GetOrdinal("site_cross_street_name")) ? "" : reader.GetString(reader.GetOrdinal("site_cross_street_name"));
                    steDetails.IsPropertyAvailable = reader.IsDBNull(reader.GetOrdinal("is_property_available")) ? true : reader.GetBoolean(reader.GetOrdinal("is_property_available"));
                    steDetails.Zoning = reader.IsDBNull(reader.GetOrdinal("zoning")) ? "" : reader.GetString(reader.GetOrdinal("zoning"));
                    steDetails.LotSize = reader.IsDBNull(reader.GetOrdinal("lot_size")) ? "" : reader.GetString(reader.GetOrdinal("lot_size"));

                    steDetails.SalesPrice = reader.IsDBNull(reader.GetOrdinal("sales_price")) ? "" : reader.GetString(reader.GetOrdinal("sales_price"));
                    //steDetails.SalesPrice = Helper.FormatCurrency("$", steDetails.SalesPrice);

                    steDetails.Comments = reader.IsDBNull(reader.GetOrdinal("comments")) ? "" : reader.GetString(reader.GetOrdinal("comments"));

                    steDetails.CreatedDate = reader.IsDBNull(reader.GetOrdinal("created_date")) ? DateTime.Now : reader.GetDateTime(reader.GetOrdinal("created_date"));
                    steDetails.PropertyType = reader.IsDBNull(reader.GetOrdinal("property_type")) ? 0 : reader.GetInt32(reader.GetOrdinal("property_type"));

                    steDetails.ImageName = reader.IsDBNull(reader.GetOrdinal("image_name")) ? "" : reader.GetString(reader.GetOrdinal("image_name"));

                    steDetails.AssetTypeId = reader.IsDBNull(reader.GetOrdinal("asset_type_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("asset_type_id"));
                    steDetails.AssetTypeName = reader.IsDBNull(reader.GetOrdinal("asset_type_name")) ? "" : reader.GetString(reader.GetOrdinal("asset_type_name"));
                    steDetails.AssetStatus = reader.IsDBNull(reader.GetOrdinal("asset_status")) ? 0 : reader.GetInt32(reader.GetOrdinal("asset_status"));
                    steDetails.AssetId = reader.IsDBNull(reader.GetOrdinal("asset_id")) ? "" : reader.GetString(reader.GetOrdinal("asset_id"));

                    steDetails.SelectedPropertyStatusId = reader.IsDBNull(reader.GetOrdinal("property_status_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("property_status_id"));
                    steDetails.SelectedPropertyStatus = reader.IsDBNull(reader.GetOrdinal("property_status")) ? "" : reader.GetString(reader.GetOrdinal("property_status"));
                    steDetails.PropertyHeaderLine2 = reader.IsDBNull(reader.GetOrdinal("property_header_line_2")) ? "" : reader.GetString(reader.GetOrdinal("property_header_line_2"));


                    if (steDetails.SiteAddress.Length > 15)
                    {
                        steDetails.SiteAddressSmall = steDetails.SiteAddress.Substring(0, 15) + "..";
                    }
                    else
                    {
                        steDetails.SiteAddressSmall = steDetails.SiteAddress;
                    }

                    if (steDetails.AssetStatus == 0)
                    {
                        steDetails.AssetStatusName = "Available";
                    }
                    else
                    {
                        steDetails.AssetStatusName = "Sold";
                    }

                    steDetails.TransactionStatusName = "";

                    steDetails.DiligenceDispositions = GetDiligenceDispositions(steDetails.SiteDetailsId);

                    steDetails.SelectedDiligenceDispositions = new DiligenceDispositionsViewModel();

                    int saleLoi = 0, saleUnderContract = 0, saleTerminated = 0, saleClosed = 0;



                    steDetails.DiligenceAcquisitions = GetDiligenceAcquisition(steDetails.SiteDetailsId);
                    steDetails.DiligenceDispositions = GetDiligenceDispositions(steDetails.SiteDetailsId);
                    steDetails.DiligenceLease = GetDiligenceLease(steDetails.SiteDetailsId);
                    steDetails.DiligenceLeaseList = GetDiligenceLeaseList(steDetails.SiteDetailsId);
                    steDetails.DiligenceDispositions_SaleLeaseBack = GetDiligenceDispositions_SaleLeaseBack(steDetails.SiteDetailsId);

                    // steDetails.DiligenceDispositions = GetDiligenceDispositions(steDetails.SiteDetailsId);

                    steDetails.SelectedDiligenceDispositions = new DiligenceDispositionsViewModel();
                    DateTime? transactionClosedDate = default(DateTime?);

                    steDetails.DiligenceLeaseWithPurchase = new DiligenceLeaseWithPurchaseViewModel();
                    steDetails.SelectedDiligenceLease = new DiligenceLeaseViewModel();
                    steDetails.DiligenceLeaseWithPurchase = new DiligenceLeaseWithPurchaseViewModel();


                    if (steDetails.AssetTypeId == (int)SamAssetType.Fee || steDetails.AssetTypeId == (int)SamAssetType.FeeSubjectToLease)
                    {
                        foreach (DiligenceDispositionsViewModel ddm in steDetails.DiligenceDispositions)
                        {

                            if (steDetails.MaxPriorityTransactionStatusId == 0)
                            {
                                steDetails.MaxPriorityTransactionStatusId = ddm.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = ddm.SelectedTransactionStatusName;
                                steDetails.SelectedDiligenceDispositions = ddm;
                                transactionClosedDate = ddm.ClosingDate;
                            }



                            if ((ddm.SelectedTransactionStatusId == (int)TransactionStatus.LOI_Received ||
                                ddm.SelectedTransactionStatusId == (int)TransactionStatus.Terminated_Acquisitions) &&
                                (ddm.SelectedTransactionStatusId != (int)SamsTransactionStatus.Under_LOI &&
                                steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Under_Contract &&
                                steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Closed_Dispositions
                                ))
                            {
                                steDetails.MaxPriorityTransactionStatusId = ddm.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = ddm.SelectedTransactionStatusName;
                                steDetails.StatusChangedDate = ddm.SelectedTransactionDate;
                                steDetails.SelectedDiligenceDispositions = ddm;
                                transactionClosedDate = ddm.ClosingDate;
                            }

                            if (ddm.SelectedTransactionStatusId == (int)SamsTransactionStatus.Under_LOI &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Under_Contract) &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Closed_Dispositions))
                            {
                                steDetails.MaxPriorityTransactionStatusId = ddm.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = ddm.SelectedTransactionStatusName;
                                steDetails.StatusChangedDate = ddm.SelectedTransactionDate;
                                steDetails.SelectedDiligenceDispositions = ddm;
                                transactionClosedDate = ddm.ClosingDate;
                            }

                            if (ddm.SelectedTransactionStatusId == (int)SamsTransactionStatus.Under_Contract &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Closed_Dispositions))
                            {
                                steDetails.MaxPriorityTransactionStatusId = ddm.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = ddm.SelectedTransactionStatusName;
                                steDetails.StatusChangedDate = ddm.SelectedTransactionDate;
                                steDetails.SelectedDiligenceDispositions = ddm;
                                transactionClosedDate = ddm.ClosingDate;
                            }

                            if (ddm.SelectedTransactionStatusId == (int)SamsTransactionStatus.Closed_Dispositions)
                            {
                                steDetails.MaxPriorityTransactionStatusId = ddm.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = ddm.SelectedTransactionStatusName;
                                steDetails.StatusChangedDate = ddm.SelectedTransactionDate;
                                steDetails.SelectedDiligenceDispositions = ddm;
                                transactionClosedDate = ddm.ClosingDate;
                            }

                        }


                        var dtClosedDate = "";
                        var daysToClose = 0;
                        if (transactionClosedDate != default(DateTime?))
                        {
                            dtClosedDate = transactionClosedDate.Value.ToString("MM/dd/yyyy");
                            daysToClose = (transactionClosedDate.Value - DateTime.Now).Days;
                            if (daysToClose < 0)
                            {
                                daysToClose = 0;
                            }
                        }
                    }
                    else if (steDetails.AssetTypeId == (int)SamAssetType.Lease)
                    {
                        steDetails.DiligenceLeaseList = GetDiligenceLeaseList(steDetails.SiteDetailsId);
                        int leaseLoi = 0, leaseUnderContract = 0, leaseTerminated = 0, leaseClosed = 0;
                        foreach (DiligenceLeaseViewModel dl in steDetails.DiligenceLeaseList)
                        {
                            if (dl.SelectedTransactionStatusId == (int)TransactionStatus.Under_LOI)
                            {
                                leaseLoi = leaseLoi + 1;
                            }
                            else if (dl.SelectedTransactionStatusId == (int)TransactionStatus.Under_Contract)
                            {
                                leaseUnderContract = leaseUnderContract + 1;
                            }
                            else if (dl.SelectedTransactionStatusId == (int)TransactionStatus.Terminated_Acquisitions)
                            {
                                leaseTerminated = leaseTerminated + 1;
                            }
                            else if (dl.SelectedTransactionStatusId == (int)TransactionStatus.Closed_Acquisitions)
                            {
                                leaseClosed = leaseClosed + 1;
                            }

                            if (steDetails.MaxPriorityTransactionStatusId == 0)
                            {
                                steDetails.MaxPriorityTransactionStatusId = dl.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = dl.SelectedTransactionStatusName;
                                steDetails.SelectedDiligenceLease = dl;
                                transactionClosedDate = dl.ClosingDate;
                            }



                            if ((dl.SelectedTransactionStatusId == (int)TransactionStatus.LOI_Received ||
                                dl.SelectedTransactionStatusId == (int)TransactionStatus.Terminated_Acquisitions) &&
                                (dl.SelectedTransactionStatusId != (int)SamsTransactionStatus.Under_LOI &&
                                steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Under_Contract &&
                                steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Closed_Dispositions
                                ))
                            {
                                steDetails.MaxPriorityTransactionStatusId = dl.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = dl.SelectedTransactionStatusName;
                                steDetails.StatusChangedDate = dl.SelectedTransactionDate;
                                steDetails.SelectedDiligenceLease = dl;
                                transactionClosedDate = dl.ClosingDate;
                            }

                            if (dl.SelectedTransactionStatusId == (int)SamsTransactionStatus.Under_LOI &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Under_Contract) &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Closed_Dispositions))
                            {
                                steDetails.MaxPriorityTransactionStatusId = dl.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = dl.SelectedTransactionStatusName;
                                steDetails.StatusChangedDate = dl.SelectedTransactionDate;
                                steDetails.SelectedDiligenceLease = dl;
                                transactionClosedDate = dl.ClosingDate;
                            }

                            if (dl.SelectedTransactionStatusId == (int)SamsTransactionStatus.Under_Contract &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Closed_Dispositions))
                            {
                                steDetails.MaxPriorityTransactionStatusId = dl.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = dl.SelectedTransactionStatusName;
                                steDetails.StatusChangedDate = dl.SelectedTransactionDate;
                                steDetails.SelectedDiligenceLease = dl;
                                transactionClosedDate = dl.ClosingDate;
                            }

                            if (dl.SelectedTransactionStatusId == (int)SamsTransactionStatus.Closed_Dispositions)
                            {
                                steDetails.MaxPriorityTransactionStatusId = dl.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = dl.SelectedTransactionStatusName;
                                steDetails.StatusChangedDate = dl.SelectedTransactionDate;
                                steDetails.SelectedDiligenceLease = dl;
                                transactionClosedDate = dl.ClosingDate;
                            }
                        }


                        if (steDetails.SelectedDiligenceLease != null)
                        {
                            var dtClosedDate = "";
                            var daysToClose = 0;
                            if (transactionClosedDate != default(DateTime?))
                            {
                                dtClosedDate = transactionClosedDate.Value.ToString("MM/dd/yyyy");
                                daysToClose = (transactionClosedDate.Value - DateTime.Now).Days;
                                if (daysToClose < 0)
                                {
                                    daysToClose = 0;
                                }
                            }
                        }

                    }
                    else if (steDetails.AssetTypeId == (int)SamAssetType.LeaseWithPurchaseOption)
                    {
                        steDetails.DiligenceLeaseWithPurchaseList = GetDiligenceLeaseWithPurchaseList(steDetails.SiteDetailsId);

                        foreach (DiligenceLeaseWithPurchaseViewModel dl in steDetails.DiligenceLeaseWithPurchaseList)
                        {

                            steDetails.DiligenceLeaseWithPurchase = dl;
                            if (steDetails.MaxPriorityTransactionStatusId == 0)
                            {
                                steDetails.MaxPriorityTransactionStatusId = dl.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = dl.SelectedTransactionStatusName;

                                steDetails.DiligenceLeaseWithPurchase = dl;
                                transactionClosedDate = dl.ClosingDate;
                            }



                            if ((dl.SelectedTransactionStatusId == (int)TransactionStatus.LOI_Received ||
                                dl.SelectedTransactionStatusId == (int)TransactionStatus.Terminated_Acquisitions) &&
                                (dl.SelectedTransactionStatusId != (int)SamsTransactionStatus.Under_LOI &&
                                steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Under_Contract &&
                                steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Closed_Dispositions
                                ))
                            {
                                steDetails.MaxPriorityTransactionStatusId = dl.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = dl.SelectedTransactionStatusName;
                                steDetails.DiligenceLeaseWithPurchase = dl;
                                transactionClosedDate = dl.ClosingDate;
                            }

                            if (dl.SelectedTransactionStatusId == (int)SamsTransactionStatus.Under_LOI &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Under_Contract) &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Closed_Dispositions))
                            {
                                steDetails.MaxPriorityTransactionStatusId = dl.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = dl.SelectedTransactionStatusName;
                                steDetails.DiligenceLeaseWithPurchase = dl;
                                transactionClosedDate = dl.ClosingDate;
                            }

                            if (dl.SelectedTransactionStatusId == (int)SamsTransactionStatus.Under_Contract &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Closed_Dispositions))
                            {
                                steDetails.MaxPriorityTransactionStatusId = dl.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = dl.SelectedTransactionStatusName;
                                steDetails.DiligenceLeaseWithPurchase = dl;
                                transactionClosedDate = dl.ClosingDate;
                            }

                            if (dl.SelectedTransactionStatusId == (int)SamsTransactionStatus.Closed_Dispositions)
                            {
                                steDetails.MaxPriorityTransactionStatusId = dl.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = dl.SelectedTransactionStatusName;
                                steDetails.DiligenceLeaseWithPurchase = dl;
                                transactionClosedDate = dl.ClosingDate;
                            }
                        }



                        if (steDetails.DiligenceLeaseWithPurchase != null)
                        {

                            var dtClosedDate = "";
                            var daysToClose = 0;
                            if (transactionClosedDate != default(DateTime?))
                            {
                                dtClosedDate = transactionClosedDate.Value.ToString("MM/dd/yyyy");
                                daysToClose = (transactionClosedDate.Value - DateTime.Now).Days;
                                if (daysToClose < 0)
                                {
                                    daysToClose = 0;
                                }
                            }

                        }
                    }
                    else if (steDetails.AssetTypeId == (int)SamAssetType.SaleLeaseBack)
                    {
                        foreach (DiligenceDispositionsViewModel ddv in steDetails.DiligenceDispositions_SaleLeaseBack)
                        {

                            if (ddv.SelectedTransactionStatusId == (int)TransactionStatus.Closed_Acquisitions || ddv.SelectedTransactionStatusId == (int)TransactionStatus.Under_Contract || ddv.SelectedTransactionStatusId == (int)TransactionStatus.Under_LOI)
                            {
                                steDetails.CanAddTransactions = false;
                            }

                            if (ddv.SelectedTransactionStatusId == (int)TransactionStatus.Closed_Acquisitions)
                            {
                                steDetails.IsTransactionClosed = true;
                            }

                            if (steDetails.MaxPriorityTransactionStatusId == 0)
                            {
                                steDetails.MaxPriorityTransactionStatusId = ddv.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = ddv.SelectedTransactionStatusName;
                            }
                            steDetails.SelectedDiligenceDispositions = ddv;
                            transactionClosedDate = ddv.ClosingDate;


                            if ((ddv.SelectedTransactionStatusId == (int)TransactionStatus.LOI_Received ||
                                ddv.SelectedTransactionStatusId == (int)TransactionStatus.Terminated_Acquisitions) &&
                                (ddv.SelectedTransactionStatusId != (int)SamsTransactionStatus.Under_LOI &&
                                steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Under_Contract &&
                                steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Closed_Dispositions
                                ))
                            {
                                steDetails.MaxPriorityTransactionStatusId = ddv.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = ddv.SelectedTransactionStatusName;
                                steDetails.SelectedDiligenceDispositions = ddv;
                                transactionClosedDate = ddv.ClosingDate;
                            }

                            if (ddv.SelectedTransactionStatusId == (int)SamsTransactionStatus.Under_LOI &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Under_Contract) &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Closed_Dispositions))
                            {
                                steDetails.MaxPriorityTransactionStatusId = ddv.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = ddv.SelectedTransactionStatusName;
                                steDetails.SelectedDiligenceDispositions = ddv;
                                transactionClosedDate = ddv.ClosingDate;
                            }

                            if (ddv.SelectedTransactionStatusId == (int)SamsTransactionStatus.Under_Contract &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Closed_Dispositions))
                            {
                                steDetails.MaxPriorityTransactionStatusId = ddv.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = ddv.SelectedTransactionStatusName;
                                steDetails.SelectedDiligenceDispositions = ddv;
                                transactionClosedDate = ddv.ClosingDate;
                            }

                            if (ddv.SelectedTransactionStatusId == (int)SamsTransactionStatus.Closed_Dispositions)
                            {
                                steDetails.MaxPriorityTransactionStatusId = ddv.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = ddv.SelectedTransactionStatusName;
                                steDetails.SelectedDiligenceDispositions = ddv;
                                transactionClosedDate = ddv.ClosingDate;
                            }


                        }

                        var dtClosedDate = "";
                        var daysToClose = 0;
                        if (transactionClosedDate != default(DateTime?))
                        {
                            dtClosedDate = transactionClosedDate.Value.ToString("MM/dd/yyyy");
                            daysToClose = (transactionClosedDate.Value - DateTime.Now).Days;
                            if (daysToClose < 0)
                            {
                                daysToClose = 0;
                            }
                        }

                    }


                    steDetails.TodoList = GetTodoList(steDetails.SiteDetailsId);
                    if (steDetails.TodoList.Count > 0)
                    {
                        steDetails.LatestComment = steDetails.TodoList[0].TodoText;
                    }

                    if (steDetails.MaxPriorityTransactionStatusId == transactionStatusId)
                    {
                        surplusPropertiesList.Add(steDetails);
                    }
                    
                }
                con.Close();
            }


            return surplusPropertiesList.Count;
        }

        public IActionResult ResetClosedLeaseTransaction(int diligenceLeaseId, int propertyId)
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

            var transactionStatusList = new List<TransactionStatusModel>();

            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("ResetLeaseTransaction", con);
                cmd.Parameters.AddWithValue("diligence_lease_id", diligenceLeaseId);
                cmd.Parameters.AddWithValue("property_type", (int)SamsPropertyType.Surplus);
                cmd.Parameters.AddWithValue("property_id", propertyId);
                cmd.CommandType = CommandType.StoredProcedure;

                con.Open();

                cmd.ExecuteNonQuery();

                con.Close();

            }
            return RedirectToAction("ViewSurplusProperty", new { propertyId = propertyId });
        }


        public IActionResult ResetClosedSaleTransaction(int diligenceDispositionId, int propertyId)
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

            var transactionStatusList = new List<TransactionStatusModel>();

            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("ResetSaleTransaction", con);
                cmd.Parameters.AddWithValue("diligence_dispositions_id", diligenceDispositionId);
                cmd.Parameters.AddWithValue("property_type", (int)SamsPropertyType.Surplus);
                cmd.Parameters.AddWithValue("property_id", propertyId);
                cmd.CommandType = CommandType.StoredProcedure;

                con.Open();

                cmd.ExecuteNonQuery();
                

                con.Close();

            }
            return RedirectToAction("ViewSurplusProperty", new { propertyId = propertyId });
        }

        [HttpPost]
        public RedirectToActionResult SaveSaleTransactionFile(TransactionFilesViewModel uploadedFile)
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

            string actualFileName = uploadedFile.SelectedFile.FileName;
            var uniqueFileName = Helper.GetUniqueFileName(uploadedFile.SelectedFile.FileName);

            var filePath = Path.Combine(webHostEnvironment.WebRootPath + @"/transaction_files", uniqueFileName);

            using (var stream = System.IO.File.Create(filePath))
            {
                uploadedFile.SelectedFile.CopyTo(stream);
            }

            
            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("saveTransactionFiles", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                cmd.Parameters.AddWithValue("transaction_id", uploadedFile.TransactionId);
                cmd.Parameters.AddWithValue("transaction_type", TransactionType.Sale);
                cmd.Parameters.AddWithValue("file_header", uploadedFile.FileHeader);
                cmd.Parameters.AddWithValue("file_name", actualFileName);
                cmd.Parameters.AddWithValue("file_full_path", uniqueFileName);
                cmd.Parameters.AddWithValue("notes", uploadedFile.Notes);
                cmd.Parameters.AddWithValue("uploaded_by", loggedInUser.UserId);


                cmd.ExecuteNonQuery();


                con.Close();
            }



            return RedirectToAction("GetDiligenceDispositionById", new { diligenceDispositionId = uploadedFile.TransactionId, propertyId = uploadedFile.PropertyId });

        }

        public RedirectToActionResult DeleteSaleTransactionFile(int transactionFiled, int transactionId, int propertyId)
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
                SqlCommand cmd = new SqlCommand("deleteTransactionFiles", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                cmd.Parameters.AddWithValue("transaction_files_id", transactionFiled);
                cmd.ExecuteNonQuery();

                con.Close();
                return RedirectToAction("GetDiligenceDispositionById", new { diligenceDispositionId = transactionId, propertyId = propertyId });
            }
        }

        [HttpPost]
        public RedirectToActionResult SaveLeaseTransactionFile(TransactionFilesViewModel uploadedFile)
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

            string actualFileName = uploadedFile.SelectedFile.FileName;
            var uniqueFileName = Helper.GetUniqueFileName(uploadedFile.SelectedFile.FileName);

            var filePath = Path.Combine(webHostEnvironment.WebRootPath + @"/transaction_files", uniqueFileName);

            using (var stream = System.IO.File.Create(filePath))
            {
                uploadedFile.SelectedFile.CopyTo(stream);
            }


            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("saveTransactionFiles", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                cmd.Parameters.AddWithValue("transaction_id", uploadedFile.TransactionId);
                cmd.Parameters.AddWithValue("transaction_type", TransactionType.Lease);
                cmd.Parameters.AddWithValue("file_header", uploadedFile.FileHeader);
                cmd.Parameters.AddWithValue("file_name", actualFileName);
                cmd.Parameters.AddWithValue("file_full_path", uniqueFileName);
                cmd.Parameters.AddWithValue("notes", uploadedFile.Notes);
                cmd.Parameters.AddWithValue("uploaded_by", loggedInUser.UserId); 

                cmd.ExecuteNonQuery();


                con.Close();
            }



            return RedirectToAction("GetDiligenceLeaseById", new { diligenceLeaseId = uploadedFile.TransactionId, propertyId = uploadedFile.PropertyId });

        }

        public RedirectToActionResult DeleteLeaseTransactionFile(int transactionFiled, int transactionId, int propertyId)
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
                SqlCommand cmd = new SqlCommand("deleteTransactionFiles", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                cmd.Parameters.AddWithValue("transaction_files_id", transactionFiled);
                cmd.ExecuteNonQuery();

                con.Close();
                return RedirectToAction("GetDiligenceLeaseById", new { diligenceLeaseId = transactionId, propertyId = propertyId });
            }
        }

        public IActionResult GetDiligenceLeaseWithPurchaseById(int diligenceLeaseWithPurchaseId, int propertyId, int currentAssetStatusId, int assetTypeId)
        {
            var diligenceLeaseWithPurchase = new DiligenceLeaseWithPurchaseViewModel();
            diligenceLeaseWithPurchase.PropertyId = propertyId;
            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("GetDiligenceLeaseWithPurchaseById", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("property_id", propertyId);
                cmd.Parameters.AddWithValue("property_type", SamsPropertyType.Surplus);
                cmd.Parameters.AddWithValue("diligence_lease_with_purchase_id", diligenceLeaseWithPurchaseId);
                con.Open();

                bool haveRecords = false;

                SqlDataReader readerAssetType = cmd.ExecuteReader();
                while (readerAssetType.Read())
                {

                    haveRecords = true;
                    diligenceLeaseWithPurchase.PropertyId = propertyId;
                    diligenceLeaseWithPurchase.PropertyType = 1;

                    diligenceLeaseWithPurchase.DiligenceLeaseWithPurchaseId = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("diligence_lease_with_purchase_id")) ? 0 : readerAssetType.GetInt32(readerAssetType.GetOrdinal("diligence_lease_with_purchase_id"));
                    diligenceLeaseWithPurchase.Tenant = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("tenant_name")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("tenant_name"));


                    diligenceLeaseWithPurchase.Rent = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("rent")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("rent"));
                    diligenceLeaseWithPurchase.UnderContractDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("under_contract_date")) ? default(DateTime?) : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("under_contract_date"));
                    diligenceLeaseWithPurchase.DueDiligenceExpiryDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("due_diligence_expiry_date")) ? default(DateTime?) : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("due_diligence_expiry_date"));
                    diligenceLeaseWithPurchase.EarnestMoneyDeposit = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("earnest_money_deposit")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("earnest_money_deposit"));
                    diligenceLeaseWithPurchase.DDPExtension = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("ddp_extension")) ? default(DateTime?) : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("ddp_extension"));

                    diligenceLeaseWithPurchase.TenantAttorney = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("tenant_attorney")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("tenant_attorney"));
                    diligenceLeaseWithPurchase.TenantAgentCommission = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("tenant_agent_commission")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("tenant_agent_commission"));
                    diligenceLeaseWithPurchase.LandlordAgentCommission = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("land_lord_agent_commission")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("land_lord_agent_commission"));
                    diligenceLeaseWithPurchase.LeaseSecurityDeposit = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("lease_security_deposit")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("lease_security_deposit"));

                    diligenceLeaseWithPurchase.DispositionTerminatedStatus = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("disposition_terminated_status")) ? 0 : readerAssetType.GetInt32(readerAssetType.GetOrdinal("disposition_terminated_status"));
                    diligenceLeaseWithPurchase.DispositionTerminatedDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("disposition_terminated_date")) ? default(DateTime?) : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("disposition_terminated_date"));
                    diligenceLeaseWithPurchase.DispositionClosedStatus = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("disposition_closed_status")) ? 0 : readerAssetType.GetInt32(readerAssetType.GetOrdinal("disposition_closed_status"));
                    diligenceLeaseWithPurchase.DispositionClosedDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("disposition_closed_date")) ? default(DateTime?) : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("disposition_closed_date"));

                    diligenceLeaseWithPurchase.SelectedTransactionStatusId = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("selected_transaction_id")) ? 0 : readerAssetType.GetInt32(readerAssetType.GetOrdinal("selected_transaction_id"));
                    diligenceLeaseWithPurchase.SelectedTransactionDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("selected_transaction_date")) ? default(DateTime?) : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("selected_transaction_date"));

                    diligenceLeaseWithPurchase.CreatedDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("created_date")) ? DateTime.Now : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("created_date"));
                    diligenceLeaseWithPurchase.LeaseCommencementDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("lease_commencement_date")) ? default(DateTime?) : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("lease_commencement_date"));

                    diligenceLeaseWithPurchase.OptionPrice = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("option_price")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("option_price"));
                    diligenceLeaseWithPurchase.OptionPurchaseDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("option_purchase_date")) ? default(DateTime?) : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("option_purchase_date"));
                    diligenceLeaseWithPurchase.ClosingDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("closing_date")) ? default(DateTime?) : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("closing_date"));
                }

                if (!haveRecords)
                {
                    //diligenceLease.SelectedTransactionDate = DateTime.Now;
                }

                con.Close();

                diligenceLeaseWithPurchase.TransactionFileList = new List<TransactionFilesViewModel>();
                if (diligenceLeaseWithPurchaseId > 0)
                {
                    SqlCommand cmdGetTransactionFiles = new SqlCommand("getTransactionFiles", con);
                    cmdGetTransactionFiles.CommandType = CommandType.StoredProcedure;
                    cmdGetTransactionFiles.Parameters.AddWithValue("transaction_id", diligenceLeaseWithPurchaseId);
                    cmdGetTransactionFiles.Parameters.AddWithValue("transaction_type", TransactionType.LeaseWithPurchaseOption);
                    con.Open();

                    SqlDataReader readerGetTransactionFiles = cmdGetTransactionFiles.ExecuteReader();
                    while (readerGetTransactionFiles.Read())
                    {
                        TransactionFilesViewModel transactionFiles = new TransactionFilesViewModel();
                        transactionFiles.TransactionFilesId = readerGetTransactionFiles.IsDBNull(readerGetTransactionFiles.GetOrdinal("transaction_files_id")) ? 0 : readerGetTransactionFiles.GetInt32(readerGetTransactionFiles.GetOrdinal("transaction_files_id"));
                        transactionFiles.TransactionId = readerGetTransactionFiles.IsDBNull(readerGetTransactionFiles.GetOrdinal("transaction_id")) ? 0 : readerGetTransactionFiles.GetInt32(readerGetTransactionFiles.GetOrdinal("transaction_id"));
                        transactionFiles.FileHeader = readerGetTransactionFiles.IsDBNull(readerGetTransactionFiles.GetOrdinal("file_header")) ? "" : readerGetTransactionFiles.GetString(readerGetTransactionFiles.GetOrdinal("file_header"));
                        transactionFiles.FileName = readerGetTransactionFiles.IsDBNull(readerGetTransactionFiles.GetOrdinal("file_name")) ? "" : readerGetTransactionFiles.GetString(readerGetTransactionFiles.GetOrdinal("file_name"));

                        transactionFiles.FileFullName = readerGetTransactionFiles.IsDBNull(readerGetTransactionFiles.GetOrdinal("file_full_path")) ? "" : readerGetTransactionFiles.GetString(readerGetTransactionFiles.GetOrdinal("file_full_path"));

                        transactionFiles.Notes = readerGetTransactionFiles.IsDBNull(readerGetTransactionFiles.GetOrdinal("notes")) ? "" : readerGetTransactionFiles.GetString(readerGetTransactionFiles.GetOrdinal("notes"));
                        transactionFiles.UploadedDate = readerGetTransactionFiles.IsDBNull(readerGetTransactionFiles.GetOrdinal("UploadedDate")) ? DateTime.Now : readerGetTransactionFiles.GetDateTime(readerGetTransactionFiles.GetOrdinal("UploadedDate"));
                        transactionFiles.UploadedByName = readerGetTransactionFiles.IsDBNull(readerGetTransactionFiles.GetOrdinal("FullName")) ? "" : readerGetTransactionFiles.GetString(readerGetTransactionFiles.GetOrdinal("FullName"));

                        diligenceLeaseWithPurchase.TransactionFileList.Add(transactionFiles);
                    }

                    con.Close();

                }


                var periodList = new List<PeriodViewModel>();

                SqlCommand cmdPeriod = new SqlCommand("GetPeriodList", con);
                cmdPeriod.CommandType = CommandType.StoredProcedure;
                cmdPeriod.Parameters.AddWithValue("property_id", propertyId);
                cmdPeriod.Parameters.AddWithValue("property_type", (int)SamsPropertyType.Surplus);
                cmdPeriod.Parameters.AddWithValue("transaction_id", diligenceLeaseWithPurchaseId);
                cmdPeriod.Parameters.AddWithValue("period_type", "LeaseWithPurchase");
                con.Open();

                SqlDataReader readerPeriod = cmdPeriod.ExecuteReader();
                while (readerPeriod.Read())
                {
                    var periodView = new PeriodViewModel();

                    periodView.PeriodId = readerPeriod.IsDBNull(readerPeriod.GetOrdinal("period_id")) ? 0 : readerPeriod.GetInt32(readerPeriod.GetOrdinal("period_id"));
                    periodView.PropertyId = readerPeriod.IsDBNull(readerPeriod.GetOrdinal("property_id")) ? 0 : readerPeriod.GetInt32(readerPeriod.GetOrdinal("property_id"));
                    periodView.PropertyType = readerPeriod.IsDBNull(readerPeriod.GetOrdinal("property_type")) ? 0 : readerPeriod.GetInt32(readerPeriod.GetOrdinal("property_type"));

                    periodView.PeriodMaster = readerPeriod.IsDBNull(readerPeriod.GetOrdinal("period_master")) ? "" : readerPeriod.GetString(readerPeriod.GetOrdinal("period_master"));

                    periodView.StartDate = readerPeriod.IsDBNull(readerPeriod.GetOrdinal("start_date")) ? DateTime.Now : readerPeriod.GetDateTime(readerPeriod.GetOrdinal("start_date"));
                    periodView.EndDate = readerPeriod.IsDBNull(readerPeriod.GetOrdinal("end_date")) ? DateTime.Now : readerPeriod.GetDateTime(readerPeriod.GetOrdinal("end_date"));


                    periodView.PeriodNotes = readerPeriod.IsDBNull(readerPeriod.GetOrdinal("period_notes")) ? "" : readerPeriod.GetString(readerPeriod.GetOrdinal("period_notes"));
                    periodView.PeriodType = readerPeriod.IsDBNull(readerPeriod.GetOrdinal("period_type")) ? "" : readerPeriod.GetString(readerPeriod.GetOrdinal("period_type"));

                    
                    periodView.AlertDate = readerPeriod.IsDBNull(readerPeriod.GetOrdinal("alert_date")) ? default(DateTime?) : readerPeriod.GetDateTime(readerPeriod.GetOrdinal("alert_date"));
                    periodView.OtherEmailAddress = readerPeriod.IsDBNull(readerPeriod.GetOrdinal("other_email_address")) ? "" : readerPeriod.GetString(readerPeriod.GetOrdinal("other_email_address"));

                    periodList.Add(periodView);
                }

                con.Close();
                diligenceLeaseWithPurchase.DispositionCriticalItems = periodList;
            }

            diligenceLeaseWithPurchase.TransactionStatusList = GetTransactionStatusList(currentAssetStatusId, diligenceLeaseWithPurchase.SelectedTransactionStatusId);
            ViewData["propertyId"] = propertyId;
            ViewData["currentAssetStatusId"] = currentAssetStatusId;
            ViewData["assetTypeId"] = assetTypeId;

            diligenceLeaseWithPurchase.EmployeeList = SamsUsersController.GetUserList();
            return View(diligenceLeaseWithPurchase);
        }

        [HttpPost]
        public IActionResult SaveDiligenceLeaseWithPurchase(DiligenceLeaseWithPurchaseViewModel diligenceLeaseWithPurchase)
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
                SqlCommand cmd = new SqlCommand("SaveDiligenceLeaseWithPurchase", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("diligence_lease_with_purchase_id", diligenceLeaseWithPurchase.DiligenceLeaseWithPurchaseId);

                cmd.Parameters.AddWithValue("property_id", diligenceLeaseWithPurchase.PropertyId);
                cmd.Parameters.AddWithValue("property_type", SamsPropertyType.Surplus);
                cmd.Parameters.AddWithValue("tenant_name", diligenceLeaseWithPurchase.Tenant);
                cmd.Parameters.AddWithValue("rent", diligenceLeaseWithPurchase.Rent);

                cmd.Parameters.AddWithValue("under_contract_date", diligenceLeaseWithPurchase.UnderContractDate);
                cmd.Parameters.AddWithValue("due_diligence_expiry_date", diligenceLeaseWithPurchase.DueDiligenceExpiryDate);
                cmd.Parameters.AddWithValue("earnest_money_deposit", diligenceLeaseWithPurchase.EarnestMoneyDeposit);
                cmd.Parameters.AddWithValue("ddp_extension", diligenceLeaseWithPurchase.DDPExtension);

                cmd.Parameters.AddWithValue("tenant_attorney", diligenceLeaseWithPurchase.TenantAttorney);
                cmd.Parameters.AddWithValue("tenant_agent_commission", diligenceLeaseWithPurchase.TenantAgentCommission);
                cmd.Parameters.AddWithValue("land_lord_agent_commission", diligenceLeaseWithPurchase.LandlordAgentCommission);
                cmd.Parameters.AddWithValue("lease_security_deposit", diligenceLeaseWithPurchase.LeaseSecurityDeposit);

                cmd.Parameters.AddWithValue("disposition_terminated_status", diligenceLeaseWithPurchase.DispositionTerminatedStatus);
                cmd.Parameters.AddWithValue("disposition_terminated_date", diligenceLeaseWithPurchase.DispositionTerminatedDate);
                cmd.Parameters.AddWithValue("disposition_closed_status", diligenceLeaseWithPurchase.DispositionClosedStatus);
                cmd.Parameters.AddWithValue("disposition_closed_date", diligenceLeaseWithPurchase.DispositionClosedDate);

                cmd.Parameters.AddWithValue("selected_transaction_id", diligenceLeaseWithPurchase.SelectedTransactionStatusId);
                cmd.Parameters.AddWithValue("selected_transaction_date", diligenceLeaseWithPurchase.SelectedTransactionDate);
                cmd.Parameters.AddWithValue("lease_commencement_date", diligenceLeaseWithPurchase.LeaseCommencementDate);
                cmd.Parameters.AddWithValue("option_price", diligenceLeaseWithPurchase.OptionPrice);
                cmd.Parameters.AddWithValue("option_purchase_date", diligenceLeaseWithPurchase.OptionPurchaseDate);
                cmd.Parameters.AddWithValue("closing_date", diligenceLeaseWithPurchase.ClosingDate);

                con.Open();


                diligenceLeaseWithPurchase.DiligenceLeaseWithPurchaseId = int.Parse(cmd.ExecuteScalar().ToString());


                con.Close();


                PropertyHistoryModel propertyHistory = new PropertyHistoryModel();
                propertyHistory.PropertyId = diligenceLeaseWithPurchase.PropertyId;
                propertyHistory.StatusId = diligenceLeaseWithPurchase.SelectedTransactionStatusId;
                propertyHistory.Description = diligenceLeaseWithPurchase.TransactionDescription;
                propertyHistory.LoggedInId = loggedInUser.UserId;
                propertyHistory.TransactionId = diligenceLeaseWithPurchase.DiligenceLeaseWithPurchaseId;

                PropertyHistory.SavePropertyHistory(propertyHistory);

            }

            return RedirectToAction("ViewSurplusProperty", new { propertyId = diligenceLeaseWithPurchase.PropertyId });
        }

        List<DiligenceLeaseWithPurchaseViewModel> GetDiligenceLeaseWithPurchaseList(int propertyId)
        {
            var diligenceLeaseWithPurchaseList = new List<DiligenceLeaseWithPurchaseViewModel>();

            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("GetDiligenceLeaseWithPurchase", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("property_id", propertyId);
                cmd.Parameters.AddWithValue("property_type", SamsPropertyType.Surplus);
                con.Open();



                SqlDataReader readerAssetType = cmd.ExecuteReader();
                while (readerAssetType.Read())
                {
                    var diligenceLeaseWithPurchase = new DiligenceLeaseWithPurchaseViewModel();

                    diligenceLeaseWithPurchase.PropertyId = propertyId;
                    diligenceLeaseWithPurchase.PropertyType = 1;

                    diligenceLeaseWithPurchase.DiligenceLeaseWithPurchaseId = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("diligence_lease_with_purchase_id")) ? 0 : readerAssetType.GetInt32(readerAssetType.GetOrdinal("diligence_lease_with_purchase_id"));
                    diligenceLeaseWithPurchase.Tenant = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("tenant_name")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("tenant_name"));


                    diligenceLeaseWithPurchase.Rent = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("rent")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("rent"));
                    diligenceLeaseWithPurchase.UnderContractDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("under_contract_date")) ? default(DateTime?) : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("under_contract_date"));
                    diligenceLeaseWithPurchase.DueDiligenceExpiryDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("due_diligence_expiry_date")) ? default(DateTime?) : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("due_diligence_expiry_date"));
                    diligenceLeaseWithPurchase.EarnestMoneyDeposit = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("earnest_money_deposit")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("earnest_money_deposit"));
                    diligenceLeaseWithPurchase.DDPExtension = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("ddp_extension")) ? default(DateTime?) : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("ddp_extension"));

                    diligenceLeaseWithPurchase.TenantAttorney = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("tenant_attorney")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("tenant_attorney"));
                    diligenceLeaseWithPurchase.TenantAgentCommission = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("tenant_agent_commission")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("tenant_agent_commission"));
                    diligenceLeaseWithPurchase.LandlordAgentCommission = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("land_lord_agent_commission")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("land_lord_agent_commission"));
                    diligenceLeaseWithPurchase.LeaseSecurityDeposit = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("lease_security_deposit")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("lease_security_deposit"));


                    diligenceLeaseWithPurchase.CreatedDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("created_date")) ? DateTime.Now : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("created_date"));

                    diligenceLeaseWithPurchase.DispositionTerminatedStatus = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("disposition_terminated_status")) ? 0 : readerAssetType.GetInt32(readerAssetType.GetOrdinal("disposition_terminated_status"));
                    diligenceLeaseWithPurchase.DispositionTerminatedDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("disposition_terminated_date")) ? default(DateTime?) : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("disposition_terminated_date"));
                    diligenceLeaseWithPurchase.DispositionClosedStatus = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("disposition_closed_status")) ? 0 : readerAssetType.GetInt32(readerAssetType.GetOrdinal("disposition_closed_status"));
                    diligenceLeaseWithPurchase.DispositionClosedDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("disposition_closed_date")) ? default(DateTime?) : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("disposition_closed_date"));


                    diligenceLeaseWithPurchase.SelectedTransactionStatusId = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("selected_transaction_id")) ? 0 : readerAssetType.GetInt32(readerAssetType.GetOrdinal("selected_transaction_id"));
                    diligenceLeaseWithPurchase.SelectedTransactionStatusName = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("transaction_status_name")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("transaction_status_name"));
                    diligenceLeaseWithPurchase.SelectedTransactionDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("selected_transaction_date")) ? DateTime.Now : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("selected_transaction_date"));

                    diligenceLeaseWithPurchase.ClosingDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("closing_date")) ? default(DateTime?) : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("closing_date"));

                    diligenceLeaseWithPurchaseList.Add(diligenceLeaseWithPurchase);
                }

                con.Close();

            }

            ViewData["propertyId"] = propertyId;
            return diligenceLeaseWithPurchaseList;
        }

        public IActionResult ResetClosedLeaseWithPurchaseTransaction(int diligenceLeaseWithPurchaseId, int propertyId)
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

            var transactionStatusList = new List<TransactionStatusModel>();

            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("ResetLeaseWithPurchaseTransaction", con);
                cmd.Parameters.AddWithValue("diligence_lease_with_purchase_id", diligenceLeaseWithPurchaseId);
                cmd.Parameters.AddWithValue("property_type", (int)SamsPropertyType.Surplus);
                cmd.Parameters.AddWithValue("property_id", propertyId);
                cmd.CommandType = CommandType.StoredProcedure;

                con.Open();

                cmd.ExecuteNonQuery();

                con.Close();

            }
            return RedirectToAction("ViewSurplusProperty", new { propertyId = propertyId });
        }

        [HttpPost]
        public RedirectToActionResult SaveLeaseWithPurchaseTransactionFile(TransactionFilesViewModel uploadedFile)
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

            string actualFileName = uploadedFile.SelectedFile.FileName;
            var uniqueFileName = Helper.GetUniqueFileName(uploadedFile.SelectedFile.FileName);

            var filePath = Path.Combine(webHostEnvironment.WebRootPath + @"/transaction_files", uniqueFileName);

            using (var stream = System.IO.File.Create(filePath))
            {
                uploadedFile.SelectedFile.CopyTo(stream);
            }


            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("saveTransactionFiles", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                cmd.Parameters.AddWithValue("transaction_id", uploadedFile.TransactionId);
                cmd.Parameters.AddWithValue("transaction_type", TransactionType.LeaseWithPurchaseOption);
                cmd.Parameters.AddWithValue("file_header", uploadedFile.FileHeader);
                cmd.Parameters.AddWithValue("file_name", actualFileName);
                cmd.Parameters.AddWithValue("file_full_path", uniqueFileName);
                cmd.Parameters.AddWithValue("notes", uploadedFile.Notes);
                cmd.Parameters.AddWithValue("uploaded_by", loggedInUser.UserId);

                cmd.ExecuteNonQuery();


                con.Close();
            }



            return RedirectToAction("GetDiligenceLeaseWithPurchaseById", new { diligenceLeaseWithPurchaseId = uploadedFile.TransactionId, propertyId = uploadedFile.PropertyId });

        }

        public RedirectToActionResult DeleteLeaseWithPurchaseTransactionFile(int transactionFiled, int transactionId, int propertyId)
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
                SqlCommand cmd = new SqlCommand("deleteTransactionFiles", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                cmd.Parameters.AddWithValue("transaction_files_id", transactionFiled);
                cmd.ExecuteNonQuery();

                con.Close();
                return RedirectToAction("GetDiligenceLeaseWithPurchaseById", new { diligenceLeaseWithPurchaseId = transactionId, propertyId = propertyId });
            }
        }

        [HttpPost]
        public IActionResult SaveDiligenceNetlease(DiligenceNetleaseViewModel diligenceNetlease)
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
                SqlCommand cmd = new SqlCommand("SaveDiligenceDispositions", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("diligence_dispositions_id", diligenceNetlease.DiligenceDispositionsId);

                cmd.Parameters.AddWithValue("property_id", diligenceNetlease.PropertyId);
                cmd.Parameters.AddWithValue("property_type", SamsPropertyType.NetLease);
                cmd.Parameters.AddWithValue("sale_price", diligenceNetlease.SalePrice);
                cmd.Parameters.AddWithValue("earnest_money", diligenceNetlease.EarnestMoney);
                cmd.Parameters.AddWithValue("buyers", diligenceNetlease.Buyers);
                cmd.Parameters.AddWithValue("escrow_agent", diligenceNetlease.EscrowAgent);
                cmd.Parameters.AddWithValue("buyers_attorney", diligenceNetlease.BuyersAttorney);
                cmd.Parameters.AddWithValue("options_to_extend", diligenceNetlease.OptionsToExtend);
                cmd.Parameters.AddWithValue("commissions", diligenceNetlease.Commissions);

                cmd.Parameters.AddWithValue("under_contract_date", diligenceNetlease.UnderContractDate);

                cmd.Parameters.AddWithValue("due_diligence_expairy_date", diligenceNetlease.DueDiligenceExpairyDate);
                cmd.Parameters.AddWithValue("due_diligence_amount", diligenceNetlease.DueDiligenceAmount);
                cmd.Parameters.AddWithValue("emd", diligenceNetlease.EMD);

                cmd.Parameters.AddWithValue("ddp_extension", diligenceNetlease.DDPExtension);
                cmd.Parameters.AddWithValue("dueDiligenceApplicableStatus", diligenceNetlease.DDPExtensionOpted);

                cmd.Parameters.AddWithValue("sellersAttorney", diligenceNetlease.SellersAttorney);
                cmd.Parameters.AddWithValue("buyers_agent_commision", diligenceNetlease.BuyersAgentCommission);
                cmd.Parameters.AddWithValue("sellers_agent_commision", diligenceNetlease.SellersAgentCommission);

                cmd.Parameters.AddWithValue("disposition_terminated_status", diligenceNetlease.DispositionTerminatedStatus);
                cmd.Parameters.AddWithValue("disposition_terminated_date", diligenceNetlease.DispositionTerminatedDate);
                cmd.Parameters.AddWithValue("disposition_closed_status", diligenceNetlease.DispositionClosedStatus);
                cmd.Parameters.AddWithValue("disposition_closed_date", diligenceNetlease.DispositionClosedDate);

                cmd.Parameters.AddWithValue("selected_transaction_id", diligenceNetlease.SelectedTransactionStatusId);

                cmd.Parameters.AddWithValue("selected_transaction_date", diligenceNetlease.SelectedTransactionDate);
                cmd.Parameters.AddWithValue("permitting_period", diligenceNetlease.PermittingPeriod);

                cmd.Parameters.AddWithValue("tenant", diligenceNetlease.Tenant);
                cmd.Parameters.AddWithValue("tenant_rent", diligenceNetlease.TenantRent);

                con.Open();


                diligenceNetlease.DiligenceDispositionsId = int.Parse(cmd.ExecuteScalar().ToString());


                con.Close();



                PropertyHistoryModel propertyHistory = new PropertyHistoryModel();
                propertyHistory.PropertyId = diligenceNetlease.PropertyId;
                propertyHistory.StatusId = diligenceNetlease.SelectedTransactionStatusId;
                propertyHistory.Description = diligenceNetlease.TransactionDescription;
                propertyHistory.LoggedInId = loggedInUser.UserId;
                propertyHistory.TransactionId = diligenceNetlease.DiligenceDispositionsId;

                PropertyHistory.SavePropertyHistory(propertyHistory);


            }

            return RedirectToAction("ViewSurplusProperty", new { propertyId = diligenceNetlease.PropertyId });
        }

        List<DiligenceDispositionsViewModel> GetDiligenceDispositions_SaleLeaseBack(int propertyId)
        {
            var diligenceDispositions = new List<DiligenceDispositionsViewModel>();

            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("GetDiligenceDispositions_SaleLeaseBack", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("property_id", propertyId);
                cmd.Parameters.AddWithValue("property_type", SamsPropertyType.Surplus);
                con.Open();



                SqlDataReader readerAssetType = cmd.ExecuteReader();
                while (readerAssetType.Read())
                {
                    var ddpViewModel = new DiligenceDispositionsViewModel();

                    ddpViewModel.PropertyId = propertyId;
                    ddpViewModel.PropertyType = (int)SamsPropertyType.Surplus;
                    ddpViewModel.DiligenceDispositionsId = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("diligence_dispositions_id")) ? 0 : readerAssetType.GetInt32(readerAssetType.GetOrdinal("diligence_dispositions_id"));

                    ddpViewModel.SalePrice = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("sale_price")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("sale_price"));
                    ddpViewModel.EarnestMoney = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("earnest_money")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("earnest_money"));
                    //ddpViewModel.EarnestMoney = Helper.FormatCurrency("$", ddpViewModel.EarnestMoney);

                    ddpViewModel.Buyers = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("buyers")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("buyers"));
                    ddpViewModel.EscrowAgent = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("escrow_agent")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("escrow_agent"));

                    ddpViewModel.BuyersAttorney = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("buyers_attorney")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("buyers_attorney"));
                    ddpViewModel.OptionsToExtend = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("options_to_extend")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("options_to_extend"));
                    ddpViewModel.Commissions = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("commissions")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("commissions"));

                    ddpViewModel.CreatedDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("created_date")) ? DateTime.Now : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("created_date"));
                    ddpViewModel.DispositionStatus = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("disposition_status")) ? 0 : readerAssetType.GetInt32(readerAssetType.GetOrdinal("disposition_status"));

                    ddpViewModel.ClosedDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("closed_date")) ? DateTime.Now : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("closed_date"));
                    ddpViewModel.TerminatedDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("terminated_date")) ? DateTime.Now : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("terminated_date"));

                    ddpViewModel.UnderContractDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("under_contract_date")) ? default(DateTime?) : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("under_contract_date"));
                    ddpViewModel.DueDiligenceExpairyDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("due_diligence_expairy_date")) ? DateTime.Now : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("due_diligence_expairy_date"));

                    ddpViewModel.DueDiligenceAmount = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("due_diligence_amount")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("due_diligence_amount"));
                    ddpViewModel.EMD = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("emd")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("emd"));
                    ddpViewModel.DDPExtension = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("ddp_extension")) ? default(DateTime?) : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("ddp_extension"));
                    ddpViewModel.CreatedDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("created_date")) ? DateTime.Now : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("created_date"));


                    ddpViewModel.UnderContractDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("under_contract_date")) ? default(DateTime?) : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("under_contract_date"));
                    ddpViewModel.DueDiligenceExpairyDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("due_diligence_expairy_date")) ? default(DateTime?) : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("due_diligence_expairy_date"));

                    ddpViewModel.DueDiligenceAmount = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("due_diligence_amount")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("due_diligence_amount"));
                    ddpViewModel.EMD = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("emd")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("emd"));
                    //ddpViewModel.EMD = Helper.FormatCurrency("$", ddpViewModel.EMD);
                    ddpViewModel.DDPExtension = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("ddp_extension")) ? default(DateTime?) : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("ddp_extension"));
                    ddpViewModel.DDPExtensionOpted = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("dueDiligenceApplicableStatus")) ? 0 : readerAssetType.GetInt32(readerAssetType.GetOrdinal("dueDiligenceApplicableStatus"));

                    ddpViewModel.SellersAttorney = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("sellersAttorney")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("sellersAttorney"));
                    ddpViewModel.BuyersAgentCommission = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("buyers_agent_commision")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("buyers_agent_commision"));
                    ddpViewModel.SellersAgentCommission = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("sellers_agent_commision")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("sellers_agent_commision"));

                    ddpViewModel.DispositionTerminatedStatus = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("disposition_terminated_status")) ? 0 : readerAssetType.GetInt32(readerAssetType.GetOrdinal("disposition_terminated_status"));
                    ddpViewModel.DispositionTerminatedDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("disposition_terminated_date")) ? default(DateTime?) : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("disposition_terminated_date"));
                    ddpViewModel.DispositionClosedStatus = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("disposition_closed_status")) ? 0 : readerAssetType.GetInt32(readerAssetType.GetOrdinal("disposition_closed_status"));
                    ddpViewModel.DispositionClosedDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("disposition_closed_date")) ? default(DateTime?) : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("disposition_closed_date"));

                    ddpViewModel.SelectedTransactionStatusId = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("selected_transaction_id")) ? 0 : readerAssetType.GetInt32(readerAssetType.GetOrdinal("selected_transaction_id"));
                    ddpViewModel.SelectedTransactionStatusName = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("transaction_status_name")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("transaction_status_name"));
                    ddpViewModel.SelectedTransactionDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("selected_transaction_date")) ? DateTime.Now : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("selected_transaction_date"));

                    ddpViewModel.Rent_SaleLeaseBack = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("rent")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("rent"));
                    ddpViewModel.Term_SaleLeaseBack = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("term")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("term"));
                    ddpViewModel.LeaseType_SaleLeaseBack = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("lease_type")) ? 0 : readerAssetType.GetInt32(readerAssetType.GetOrdinal("lease_type"));
                    ddpViewModel.LeaseCommencementDate_SaleLeaseBack = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("lease_commencement_date")) ? default(DateTime?) : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("lease_commencement_date"));

                    ddpViewModel.ClosingDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("closing_date")) ? default(DateTime?) : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("closing_date"));

                    diligenceDispositions.Add(ddpViewModel);
                }

                con.Close();

            }

            return diligenceDispositions;
        }

        public IActionResult GetDiligenceSaleLeaseBackById(int saleLeaseBackId, int propertyId, int currentAssetStatusId)
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

            var ddpViewModel = new DiligenceDispositionsViewModel();
            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("GetSaleLeaseBackById", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("diligence_dispositions_id", saleLeaseBackId);
                con.Open();

                ddpViewModel.PropertyType = (int)SamsPropertyType.Surplus;
                bool haveRecords = false;

                SqlDataReader readerAssetType = cmd.ExecuteReader();
                while (readerAssetType.Read())
                {

                    haveRecords = true;
                    ddpViewModel.PropertyId = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("property_id")) ? 0 : readerAssetType.GetInt32(readerAssetType.GetOrdinal("property_id")); ;

                    ddpViewModel.DiligenceDispositionsId = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("diligence_dispositions_id")) ? 0 : readerAssetType.GetInt32(readerAssetType.GetOrdinal("diligence_dispositions_id"));

                    ddpViewModel.SalePrice = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("sale_price")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("sale_price"));
                    ddpViewModel.EarnestMoney = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("earnest_money")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("earnest_money"));
                    //ddpViewModel.EarnestMoney = Helper.FormatCurrency("$", ddpViewModel.EarnestMoney);

                    ddpViewModel.Buyers = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("buyers")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("buyers"));
                    ddpViewModel.EscrowAgent = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("escrow_agent")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("escrow_agent"));

                    ddpViewModel.BuyersAttorney = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("buyers_attorney")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("buyers_attorney"));
                    ddpViewModel.OptionsToExtend = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("options_to_extend")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("options_to_extend"));
                    ddpViewModel.Commissions = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("commissions")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("commissions"));

                    ddpViewModel.CreatedDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("created_date")) ? DateTime.Now : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("created_date"));
                    ddpViewModel.DispositionStatus = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("disposition_status")) ? 0 : readerAssetType.GetInt32(readerAssetType.GetOrdinal("disposition_status"));

                    ddpViewModel.ClosedDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("closed_date")) ? DateTime.Now : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("closed_date"));
                    ddpViewModel.TerminatedDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("terminated_date")) ? DateTime.Now : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("terminated_date"));

                    ddpViewModel.UnderContractDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("under_contract_date")) ? default(DateTime?) : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("under_contract_date"));
                    ddpViewModel.DueDiligenceExpairyDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("due_diligence_expairy_date")) ? DateTime.Now : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("due_diligence_expairy_date"));

                    ddpViewModel.DueDiligenceAmount = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("due_diligence_amount")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("due_diligence_amount"));
                    ddpViewModel.EMD = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("emd")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("emd"));
                    ddpViewModel.DDPExtension = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("ddp_extension")) ? default(DateTime?) : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("ddp_extension"));
                    ddpViewModel.CreatedDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("created_date")) ? DateTime.Now : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("created_date"));

                    ddpViewModel.UnderContractDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("under_contract_date")) ? default(DateTime?) : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("under_contract_date"));
                    ddpViewModel.DueDiligenceExpairyDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("due_diligence_expairy_date")) ? default(DateTime?) : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("due_diligence_expairy_date"));

                    ddpViewModel.DueDiligenceAmount = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("due_diligence_amount")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("due_diligence_amount"));
                    ddpViewModel.EMD = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("emd")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("emd"));
                    //ddpViewModel.EMD = Helper.FormatCurrency("$", ddpViewModel.EMD);
                    ddpViewModel.DDPExtension = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("ddp_extension")) ? default(DateTime?) : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("ddp_extension"));
                    ddpViewModel.DDPExtensionOpted = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("dueDiligenceApplicableStatus")) ? 0 : readerAssetType.GetInt32(readerAssetType.GetOrdinal("dueDiligenceApplicableStatus"));

                    ddpViewModel.SellersAttorney = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("sellersAttorney")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("sellersAttorney"));
                    ddpViewModel.BuyersAgentCommission = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("buyers_agent_commision")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("buyers_agent_commision"));
                    ddpViewModel.SellersAgentCommission = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("sellers_agent_commision")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("sellers_agent_commision"));

                    ddpViewModel.DispositionTerminatedStatus = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("disposition_terminated_status")) ? 0 : readerAssetType.GetInt32(readerAssetType.GetOrdinal("disposition_terminated_status"));
                    ddpViewModel.DispositionTerminatedDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("disposition_terminated_date")) ? default(DateTime?) : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("disposition_terminated_date"));
                    ddpViewModel.DispositionClosedStatus = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("disposition_closed_status")) ? 0 : readerAssetType.GetInt32(readerAssetType.GetOrdinal("disposition_closed_status"));
                    ddpViewModel.DispositionClosedDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("disposition_closed_date")) ? default(DateTime?) : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("disposition_closed_date"));

                    ddpViewModel.SelectedTransactionStatusId = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("selected_transaction_id")) ? 0 : readerAssetType.GetInt32(readerAssetType.GetOrdinal("selected_transaction_id"));
                    ddpViewModel.SelectedTransactionDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("selected_transaction_date")) ? default(DateTime?) : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("selected_transaction_date"));

                    ddpViewModel.PermittingPeriod = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("permitting_period")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("permitting_period"));

                    ddpViewModel.Rent_SaleLeaseBack = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("rent")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("rent"));
                    ddpViewModel.Term_SaleLeaseBack = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("term")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("term"));
                    ddpViewModel.LeaseType_SaleLeaseBack = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("lease_type")) ? 0 : readerAssetType.GetInt32(readerAssetType.GetOrdinal("lease_type"));
                    ddpViewModel.LeaseCommencementDate_SaleLeaseBack = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("lease_commencement_date")) ? default(DateTime?) : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("lease_commencement_date"));
                    ddpViewModel.ClosingDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("closing_date")) ? default(DateTime?) : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("closing_date"));
                }



                if (!haveRecords)
                {
                    //ddpViewModel.SelectedTransactionDate = DateTime.Now;
                }
                con.Close();

                ddpViewModel.TransactionFileList = new List<TransactionFilesViewModel>();
                if (saleLeaseBackId > 0)
                {
                    SqlCommand cmdGetTransactionFiles = new SqlCommand("getTransactionFiles", con);
                    cmdGetTransactionFiles.CommandType = CommandType.StoredProcedure;
                    cmdGetTransactionFiles.Parameters.AddWithValue("transaction_id", saleLeaseBackId);
                    cmdGetTransactionFiles.Parameters.AddWithValue("transaction_type", TransactionType.SaleLeaseBack);
                    con.Open();

                    SqlDataReader readerGetTransactionFiles = cmdGetTransactionFiles.ExecuteReader();
                    while (readerGetTransactionFiles.Read())
                    {
                        TransactionFilesViewModel transactionFiles = new TransactionFilesViewModel();
                        transactionFiles.TransactionFilesId = readerGetTransactionFiles.IsDBNull(readerGetTransactionFiles.GetOrdinal("transaction_files_id")) ? 0 : readerGetTransactionFiles.GetInt32(readerGetTransactionFiles.GetOrdinal("transaction_files_id"));
                        transactionFiles.TransactionId = readerGetTransactionFiles.IsDBNull(readerGetTransactionFiles.GetOrdinal("transaction_id")) ? 0 : readerGetTransactionFiles.GetInt32(readerGetTransactionFiles.GetOrdinal("transaction_id"));
                        transactionFiles.FileHeader = readerGetTransactionFiles.IsDBNull(readerGetTransactionFiles.GetOrdinal("file_header")) ? "" : readerGetTransactionFiles.GetString(readerGetTransactionFiles.GetOrdinal("file_header"));
                        transactionFiles.FileName = readerGetTransactionFiles.IsDBNull(readerGetTransactionFiles.GetOrdinal("file_name")) ? "" : readerGetTransactionFiles.GetString(readerGetTransactionFiles.GetOrdinal("file_name"));

                        transactionFiles.FileFullName = readerGetTransactionFiles.IsDBNull(readerGetTransactionFiles.GetOrdinal("file_full_path")) ? "" : readerGetTransactionFiles.GetString(readerGetTransactionFiles.GetOrdinal("file_full_path"));

                        transactionFiles.Notes = readerGetTransactionFiles.IsDBNull(readerGetTransactionFiles.GetOrdinal("notes")) ? "" : readerGetTransactionFiles.GetString(readerGetTransactionFiles.GetOrdinal("notes"));
                        transactionFiles.UploadedDate = readerGetTransactionFiles.IsDBNull(readerGetTransactionFiles.GetOrdinal("UploadedDate")) ? DateTime.Now : readerGetTransactionFiles.GetDateTime(readerGetTransactionFiles.GetOrdinal("UploadedDate"));
                        transactionFiles.UploadedByName = readerGetTransactionFiles.IsDBNull(readerGetTransactionFiles.GetOrdinal("FullName")) ? "" : readerGetTransactionFiles.GetString(readerGetTransactionFiles.GetOrdinal("FullName"));

                        ddpViewModel.TransactionFileList.Add(transactionFiles);
                    }

                    con.Close();
                }



                var periodList = new List<PeriodViewModel>();

                SqlCommand cmdPeriod = new SqlCommand("GetPeriodList", con);
                cmdPeriod.CommandType = CommandType.StoredProcedure;
                cmdPeriod.Parameters.AddWithValue("property_id", propertyId);
                cmdPeriod.Parameters.AddWithValue("property_type", (int)SamsPropertyType.Surplus);
                cmdPeriod.Parameters.AddWithValue("transaction_id", ddpViewModel.DiligenceDispositionsId);
                cmdPeriod.Parameters.AddWithValue("period_type", "PurchaseLeaseBack");
                con.Open();

                SqlDataReader readerPeriod = cmdPeriod.ExecuteReader();
                while (readerPeriod.Read())
                {
                    var periodView = new PeriodViewModel();

                    periodView.PeriodId = readerPeriod.IsDBNull(readerPeriod.GetOrdinal("period_id")) ? 0 : readerPeriod.GetInt32(readerPeriod.GetOrdinal("period_id"));
                    periodView.PropertyId = readerPeriod.IsDBNull(readerPeriod.GetOrdinal("property_id")) ? 0 : readerPeriod.GetInt32(readerPeriod.GetOrdinal("property_id"));
                    periodView.PropertyType = readerPeriod.IsDBNull(readerPeriod.GetOrdinal("property_type")) ? 0 : readerPeriod.GetInt32(readerPeriod.GetOrdinal("property_type"));

                    periodView.PeriodMaster = readerPeriod.IsDBNull(readerPeriod.GetOrdinal("period_master")) ? "" : readerPeriod.GetString(readerPeriod.GetOrdinal("period_master"));

                    periodView.StartDate = readerPeriod.IsDBNull(readerPeriod.GetOrdinal("start_date")) ? DateTime.Now : readerPeriod.GetDateTime(readerPeriod.GetOrdinal("start_date"));
                    periodView.EndDate = readerPeriod.IsDBNull(readerPeriod.GetOrdinal("end_date")) ? DateTime.Now : readerPeriod.GetDateTime(readerPeriod.GetOrdinal("end_date"));


                    periodView.PeriodNotes = readerPeriod.IsDBNull(readerPeriod.GetOrdinal("period_notes")) ? "" : readerPeriod.GetString(readerPeriod.GetOrdinal("period_notes"));
                    periodView.PeriodType = readerPeriod.IsDBNull(readerPeriod.GetOrdinal("period_type")) ? "" : readerPeriod.GetString(readerPeriod.GetOrdinal("period_type"));

                    periodView.AlertDate = readerPeriod.IsDBNull(readerPeriod.GetOrdinal("alert_date")) ? default(DateTime?) : readerPeriod.GetDateTime(readerPeriod.GetOrdinal("alert_date"));
                    periodView.OtherEmailAddress = readerPeriod.IsDBNull(readerPeriod.GetOrdinal("other_email_address")) ? "" : readerPeriod.GetString(readerPeriod.GetOrdinal("other_email_address"));

                    periodList.Add(periodView);
                }

                con.Close();
                ddpViewModel.DispositionCriticalItems = periodList;
            }

            ViewData["propertyId"] = propertyId;
            ViewData["currentAssetStatusId"] = currentAssetStatusId;


            ddpViewModel.TransactionStatusList = GetTransactionStatusList(currentAssetStatusId, ddpViewModel.SelectedTransactionStatusId);
            ddpViewModel.LeaseTypeList_SaleLeaseBack = GetLeaseTypeList();
            return View(ddpViewModel);
        }

        [HttpPost]
        public IActionResult SaveDiligenceDispositions_SaleLeaseBack(DiligenceDispositionsViewModel diligenceDispositions)
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
                SqlCommand cmd = new SqlCommand("SaveDiligenceDispositions_SaleLeaseBack", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("diligence_dispositions_id", diligenceDispositions.DiligenceDispositionsId);

                cmd.Parameters.AddWithValue("property_id", diligenceDispositions.PropertyId);
                cmd.Parameters.AddWithValue("property_type", SamsPropertyType.Surplus);
                cmd.Parameters.AddWithValue("sale_price", diligenceDispositions.SalePrice);
                cmd.Parameters.AddWithValue("earnest_money", diligenceDispositions.EarnestMoney);
                cmd.Parameters.AddWithValue("buyers", diligenceDispositions.Buyers);
                cmd.Parameters.AddWithValue("escrow_agent", diligenceDispositions.EscrowAgent);
                cmd.Parameters.AddWithValue("buyers_attorney", diligenceDispositions.BuyersAttorney);
                cmd.Parameters.AddWithValue("options_to_extend", diligenceDispositions.OptionsToExtend);
                cmd.Parameters.AddWithValue("commissions", diligenceDispositions.Commissions);

                cmd.Parameters.AddWithValue("under_contract_date", diligenceDispositions.UnderContractDate);

                cmd.Parameters.AddWithValue("due_diligence_expairy_date", diligenceDispositions.DueDiligenceExpairyDate);
                cmd.Parameters.AddWithValue("due_diligence_amount", diligenceDispositions.DueDiligenceAmount);
                cmd.Parameters.AddWithValue("emd", diligenceDispositions.EMD);

                cmd.Parameters.AddWithValue("ddp_extension", diligenceDispositions.DDPExtension);
                cmd.Parameters.AddWithValue("dueDiligenceApplicableStatus", diligenceDispositions.DDPExtensionOpted);

                cmd.Parameters.AddWithValue("sellersAttorney", diligenceDispositions.SellersAttorney);
                cmd.Parameters.AddWithValue("buyers_agent_commision", diligenceDispositions.BuyersAgentCommission);
                cmd.Parameters.AddWithValue("sellers_agent_commision", diligenceDispositions.SellersAgentCommission);

                cmd.Parameters.AddWithValue("disposition_terminated_status", diligenceDispositions.DispositionTerminatedStatus);
                cmd.Parameters.AddWithValue("disposition_terminated_date", diligenceDispositions.DispositionTerminatedDate);
                cmd.Parameters.AddWithValue("disposition_closed_status", diligenceDispositions.DispositionClosedStatus);
                cmd.Parameters.AddWithValue("disposition_closed_date", diligenceDispositions.DispositionClosedDate);

                cmd.Parameters.AddWithValue("selected_transaction_id", diligenceDispositions.SelectedTransactionStatusId);

                cmd.Parameters.AddWithValue("selected_transaction_date", diligenceDispositions.SelectedTransactionDate);
                cmd.Parameters.AddWithValue("permitting_period", diligenceDispositions.PermittingPeriod);
                cmd.Parameters.AddWithValue("rent", diligenceDispositions.Rent_SaleLeaseBack);
                cmd.Parameters.AddWithValue("term", diligenceDispositions.Term_SaleLeaseBack);
                cmd.Parameters.AddWithValue("lease_type", diligenceDispositions.LeaseType_SaleLeaseBack);
                cmd.Parameters.AddWithValue("lease_commencement_date", diligenceDispositions.LeaseCommencementDate_SaleLeaseBack);
                cmd.Parameters.AddWithValue("closing_date", diligenceDispositions.ClosingDate);

                con.Open();


                diligenceDispositions.DiligenceDispositionsId = int.Parse(cmd.ExecuteScalar().ToString());


                con.Close();



                PropertyHistoryModel propertyHistory = new PropertyHistoryModel();
                propertyHistory.PropertyId = diligenceDispositions.PropertyId;
                propertyHistory.StatusId = diligenceDispositions.SelectedTransactionStatusId;
                propertyHistory.Description = diligenceDispositions.TransactionDescription;
                propertyHistory.LoggedInId = loggedInUser.UserId;
                propertyHistory.TransactionId = diligenceDispositions.DiligenceDispositionsId;

                PropertyHistory.SavePropertyHistory(propertyHistory);


            }

            return RedirectToAction("ViewSurplusProperty", new { propertyId = diligenceDispositions.PropertyId });
        }

        [HttpPost]
        public RedirectToActionResult SaveSaleLeaseBackTransactionFile(TransactionFilesViewModel uploadedFile)
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

            string actualFileName = uploadedFile.SelectedFile.FileName;
            var uniqueFileName = Helper.GetUniqueFileName(uploadedFile.SelectedFile.FileName);

            var filePath = Path.Combine(webHostEnvironment.WebRootPath + @"/transaction_files", uniqueFileName);

            using (var stream = System.IO.File.Create(filePath))
            {
                uploadedFile.SelectedFile.CopyTo(stream);
            }


            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("saveTransactionFiles", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                cmd.Parameters.AddWithValue("transaction_id", uploadedFile.TransactionId);
                cmd.Parameters.AddWithValue("transaction_type", TransactionType.SaleLeaseBack);
                cmd.Parameters.AddWithValue("file_header", uploadedFile.FileHeader);
                cmd.Parameters.AddWithValue("file_name", actualFileName);
                cmd.Parameters.AddWithValue("file_full_path", uniqueFileName);
                cmd.Parameters.AddWithValue("notes", uploadedFile.Notes);
                cmd.Parameters.AddWithValue("uploaded_by", loggedInUser.UserId);


                cmd.ExecuteNonQuery();


                con.Close();
            }



            return RedirectToAction("GetDiligenceSaleLeaseBackById", new { saleLeaseBackId = uploadedFile.TransactionId, propertyId = uploadedFile.PropertyId });

        }

        public RedirectToActionResult DeleteSaleLeaseBackTransactionFile(int transactionFiled, int transactionId, int propertyId)
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
                SqlCommand cmd = new SqlCommand("deleteTransactionFiles", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                cmd.Parameters.AddWithValue("transaction_files_id", transactionFiled);
                cmd.ExecuteNonQuery();

                con.Close();
                return RedirectToAction("GetDiligenceSaleLeaseBackById", new { saleLeaseBackId = transactionId, propertyId = propertyId });
            }
        }

        public IActionResult ResetClosedSaleLeaseBackTransaction(int diligenceDispositionId, int propertyId)
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

            var transactionStatusList = new List<TransactionStatusModel>();

            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("ResetSaleLeaseBackTransaction", con);
                cmd.Parameters.AddWithValue("diligence_dispositions_id", diligenceDispositionId);
                cmd.Parameters.AddWithValue("property_type", (int)SamsPropertyType.Surplus);
                cmd.Parameters.AddWithValue("property_id", propertyId);
                cmd.CommandType = CommandType.StoredProcedure;

                con.Open();

                cmd.ExecuteNonQuery();


                con.Close();

            }
            return RedirectToAction("ViewSurplusProperty", new { propertyId = propertyId });
        }

        public RedirectToActionResult DeleteLeaseTransaction(int diligenceLeaseId, int propertyId)
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
                SqlCommand cmd = new SqlCommand("deleteTransactionFiles", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                cmd.Parameters.AddWithValue("diligenceLeaseId", diligenceLeaseId);
                cmd.ExecuteNonQuery();
                con.Close();
                
            }

            return RedirectToAction("ViewSurplusProperty", new { propertyId = propertyId });
        }

        public RedirectToActionResult DeleteLeaseWithPurchaseTransaction(int diligenceLeaseId, int propertyId)
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
                SqlCommand cmd = new SqlCommand("DeleteLeaseWithPurchaseTransaction", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                cmd.Parameters.AddWithValue("diligence_lease_with_purchase_id", diligenceLeaseId);
                cmd.ExecuteNonQuery();
                con.Close();

            }

            return RedirectToAction("ViewSurplusProperty", new { propertyId = propertyId });
        }

        public RedirectToActionResult DeleteSaleLeaseBackTransaction(int diligenceLeaseId, int propertyId)
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
                SqlCommand cmd = new SqlCommand("DeleteDiligenceSaleLeaseBack", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                cmd.Parameters.AddWithValue("diligence_dispositions_id", diligenceLeaseId);
                cmd.ExecuteNonQuery();
                con.Close();

            }

            return RedirectToAction("ViewSurplusProperty", new { propertyId = propertyId });
        }

        public IActionResult DeleteSaleTransaction(int diligenceDispositionId, int propertyId)
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

            var transactionStatusList = new List<TransactionStatusModel>();

            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("DeleteSaleTransaction", con);
                cmd.Parameters.AddWithValue("diligence_dispositions_id", diligenceDispositionId);
                cmd.CommandType = CommandType.StoredProcedure;

                con.Open();

                cmd.ExecuteNonQuery();


                con.Close();

            }
            return RedirectToAction("ViewSurplusProperty", new { propertyId = propertyId });
        }

        public IActionResult DeleteLeaseTransactionEntry(int diligenceLeaseId, int propertyId)
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

            var transactionStatusList = new List<TransactionStatusModel>();

            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("DeleteLeaseTransactionEntry", con);
                cmd.Parameters.AddWithValue("diligence_lease_id", diligenceLeaseId);
                cmd.CommandType = CommandType.StoredProcedure;

                con.Open();

                cmd.ExecuteNonQuery();

                con.Close();

            }
            return RedirectToAction("ViewSurplusProperty", new { propertyId = propertyId });
        }

        public IActionResult DeleteTransactionHistory(int historyId, int propertyId)
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

            PropertyHistory.DeletePropertyHistory(historyId);

            return RedirectToAction("ViewSurplusProperty", new { propertyId = propertyId });
        }

        public IActionResult DeleteTodo(int todoId, int propertyId)
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

            var periodList = new List<PeriodViewModel>();

            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("delete_todo_item", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("todo_id", todoId);

                con.Open();

                cmd.ExecuteReader();


                con.Close();

            }

            return RedirectToAction("ViewSurplusProperty", new { propertyId = propertyId });
        }

        public IActionResult Todo_MarkAsCompleted(int todoId, int propertyId)
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

            var periodList = new List<PeriodViewModel>();

            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("SaveTodo", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("todo_id", todoId);
                cmd.Parameters.AddWithValue("completed_status", 1);
                cmd.Parameters.AddWithValue("updated_by", loggedInUser.UserId);
                con.Open();

                cmd.ExecuteReader();


                con.Close();

            }

            return RedirectToAction("ViewSurplusProperty", new { propertyId = propertyId });
        }

        List<RegionViewModel> GetRegionList(int stateId)
        {
            List<RegionViewModel> regionList = new List<RegionViewModel>();

            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("GetRegionList", con);
                cmd.CommandType = CommandType.StoredProcedure;
                
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    RegionViewModel regionItem = new RegionViewModel();
                    regionItem.RegionId = reader.IsDBNull(reader.GetOrdinal("region_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("region_id"));
                    regionItem.RegionName = reader.IsDBNull(reader.GetOrdinal("region_name")) ? "" : reader.GetString(reader.GetOrdinal("region_name"));
                    regionItem.StateId = reader.IsDBNull(reader.GetOrdinal("state_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("state_id"));

                    if (regionItem.StateId == stateId)
                    {
                        regionList.Add(regionItem);
                    }
                    
                }
                con.Close();
            }

            return regionList;

        }

        [HttpPost]
        public IActionResult SavePeriodFromDashboard(PeriodViewModel period)
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
                SqlCommand cmd = new SqlCommand("UpdatePeriodFromDashboard", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("period_id", period.PeriodId);

                cmd.Parameters.AddWithValue("period_master", period.PeriodMaster);

                cmd.Parameters.AddWithValue("start_date", period.StartDate);

                DateTime endDate = period.StartDate.AddDays(period.AddedDuration);

                //cmd.Parameters.AddWithValue("end_date", period.EndDate);
                cmd.Parameters.AddWithValue("end_date", endDate);

                cmd.Parameters.AddWithValue("period_notes", period.PeriodNotes);


                cmd.Parameters.AddWithValue("alert_date", period.AlertDate);
                cmd.Parameters.AddWithValue("other_email_address", period.OtherEmailAddress);

                con.Open();


                period.PeriodId = int.Parse(cmd.ExecuteScalar().ToString());


                con.Close();

            }
            return RedirectToAction("Dashboard");

        }
    }


}