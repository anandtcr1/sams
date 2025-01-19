using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using sams.Common;
using sams.Models;
using Spire.Xls;

namespace sams.Controllers
{
    public class NetLeasePropertiesController : Controller
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        //private readonly ApplicationDbContext dbContext;

        public NetLeasePropertiesController(IWebHostEnvironment hostEnvironment)
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

            List<NetleasePropertiesViewModel> netLeasePropertiesList = new List<NetleasePropertiesViewModel>();

            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("GetNetleasePropertyList", con);
                cmd.Parameters.AddWithValue("asset_status", 0);

                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var steDetails = new NetleasePropertiesViewModel();
                    steDetails.NetleasePropertyId = reader.IsDBNull(reader.GetOrdinal("net_lease_property_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("net_lease_property_id"));
                    steDetails.AssetId = reader.IsDBNull(reader.GetOrdinal("asset_id")) ? "" : reader.GetString(reader.GetOrdinal("asset_id"));
                    steDetails.AssetName = reader.IsDBNull(reader.GetOrdinal("asset_name")) ? "" : reader.GetString(reader.GetOrdinal("asset_name"));
                    steDetails.StateId = reader.IsDBNull(reader.GetOrdinal("state_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("state_id"));
                    steDetails.City = reader.IsDBNull(reader.GetOrdinal("city")) ? "" : reader.GetString(reader.GetOrdinal("city"));

                    steDetails.PropertyPrice = reader.IsDBNull(reader.GetOrdinal("property_price")) ? "" : reader.GetString(reader.GetOrdinal("property_price"));
                    steDetails.CapRate = reader.IsDBNull(reader.GetOrdinal("cap_rate")) ? 0 : reader.GetDouble(reader.GetOrdinal("cap_rate"));

                    steDetails.Term = reader.IsDBNull(reader.GetOrdinal("term")) ? "" : reader.GetString(reader.GetOrdinal("term"));

                    steDetails.PdfFileName = reader.IsDBNull(reader.GetOrdinal("detail_pdf")) ? "" : reader.GetString(reader.GetOrdinal("detail_pdf"));
                    steDetails.StateName = reader.IsDBNull(reader.GetOrdinal("state_name")) ? "" : reader.GetString(reader.GetOrdinal("state_name"));
                    
                    steDetails.CreatedDate = reader.IsDBNull(reader.GetOrdinal("created_date")) ? DateTime.Now : reader.GetDateTime(reader.GetOrdinal("created_date"));
                    steDetails.AssetTypeId = reader.IsDBNull(reader.GetOrdinal("asset_type_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("asset_type_id"));
                    steDetails.AssetTypeName = reader.IsDBNull(reader.GetOrdinal("asset_type_name")) ? "" : reader.GetString(reader.GetOrdinal("asset_type_name"));
                    steDetails.IsShoppingCenter = reader.IsDBNull(reader.GetOrdinal("is_shopping_center")) ? false : reader.GetBoolean(reader.GetOrdinal("is_shopping_center"));
                    if (steDetails.IsShoppingCenter)
                    {
                        steDetails.ShoppingCenterOrNetlease = "Shopping Center";
                    }
                    else
                    {
                        steDetails.ShoppingCenterOrNetlease = "Net Lease";
                    }
                    steDetails.Address = reader.IsDBNull(reader.GetOrdinal("property_address")) ? "" : reader.GetString(reader.GetOrdinal("property_address"));
                    steDetails.ZipCode = reader.IsDBNull(reader.GetOrdinal("property_zipcode")) ? "" : reader.GetString(reader.GetOrdinal("property_zipcode"));

                    steDetails.SelectedPropertyStatusId = reader.IsDBNull(reader.GetOrdinal("property_status_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("property_status_id"));
                    steDetails.SelectedPropertyStatus = reader.IsDBNull(reader.GetOrdinal("property_status")) ? "" : reader.GetString(reader.GetOrdinal("property_status"));

                    steDetails.ShowInListing = reader.IsDBNull(reader.GetOrdinal("can_publish")) ? false : reader.GetBoolean(reader.GetOrdinal("can_publish"));

                    if (steDetails.Address.Length > 15)
                    {
                        steDetails.AddressShort = steDetails.Address.Substring(0, 15) + "..";
                    }
                    else
                    {
                        steDetails.AddressShort = steDetails.Address;
                    }


                    steDetails.TransactionStatusName = "";

                    steDetails.DiligenceDispositionList = GetDiligenceDispositions(steDetails.NetleasePropertyId);

                    steDetails.DiligenceAcquisitions = GetDiligenceAcquisition(steDetails.NetleasePropertyId);
                    steDetails.DiligenceDispositionList = GetDiligenceDispositions(steDetails.NetleasePropertyId);
                    steDetails.DiligenceLeaseList = GetDiligenceLease(steDetails.NetleasePropertyId);

                    steDetails.DispositionPeriodList = GetPeriodList(steDetails.NetleasePropertyId, "Disposition");
                    steDetails.LeasePeriodList = GetPeriodList(steDetails.NetleasePropertyId, "Lease");

                    steDetails.LeaseTypeList = GetLeaseTypeList();
                    steDetails.FutureTenantList = GetFutureTenantList(steDetails.NetleasePropertyId);

                    steDetails.DiligenceDispositions = new DiligenceDispositionsViewModel();
                    steDetails.SelectedDiligenceDisposition = new DiligenceDispositionsViewModel();
                    
                    DateTime? transactionClosedDate = default(DateTime?);

                    steDetails.DiligenceLease = new DiligenceLeaseViewModel();
                    steDetails.SelectedDiligenceDisposition = new DiligenceDispositionsViewModel();
                    steDetails.DiligenceLeaseWithPurchase = new DiligenceLeaseWithPurchaseViewModel();
                    steDetails.SelectedDiligenceNetlease = new DiligenceNetleaseViewModel();

                    int saleLoi = 0, saleUnderContract = 0, saleTerminated = 0, saleClosed = 0;

                    if (steDetails.AssetTypeId == (int)SamAssetType.Fee || steDetails.AssetTypeId == (int)SamAssetType.FeeSubjectToLease)
                    {
                        steDetails.SelectedDiligenceDisposition = new DiligenceDispositionsViewModel();
                        foreach (DiligenceDispositionsViewModel ddm in steDetails.DiligenceDispositionList)
                        {
                            if (ddm.SelectedTransactionStatusId == (int)TransactionStatus.Closed_Acquisitions || ddm.SelectedTransactionStatusId == (int)TransactionStatus.Under_Contract || ddm.SelectedTransactionStatusId == (int)TransactionStatus.Under_LOI)
                            {
                                steDetails.CanAddTransactions = false;
                            }

                            if (steDetails.MaxPriorityTransactionStatusId == 0)
                            {
                                steDetails.MaxPriorityTransactionStatusId = ddm.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = ddm.SelectedTransactionStatusName;

                                steDetails.SelectedDiligenceDisposition = ddm;
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

                                steDetails.SelectedDiligenceDisposition = ddm;
                                transactionClosedDate = ddm.ClosingDate;
                            }

                            if (ddm.SelectedTransactionStatusId == (int)SamsTransactionStatus.Under_LOI &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Under_Contract) &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Closed_Dispositions))
                            {
                                steDetails.MaxPriorityTransactionStatusId = ddm.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = ddm.SelectedTransactionStatusName;
                                steDetails.StatusChangedDate = ddm.SelectedTransactionDate;

                                steDetails.SelectedDiligenceDisposition = ddm;
                                transactionClosedDate = ddm.ClosingDate;
                            }

                            if (ddm.SelectedTransactionStatusId == (int)SamsTransactionStatus.Under_Contract &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Closed_Dispositions))
                            {
                                steDetails.MaxPriorityTransactionStatusId = ddm.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = ddm.SelectedTransactionStatusName;
                                steDetails.StatusChangedDate = ddm.SelectedTransactionDate;

                                steDetails.SelectedDiligenceDisposition = ddm;
                                transactionClosedDate = ddm.ClosingDate;
                            }

                            if (ddm.SelectedTransactionStatusId == (int)SamsTransactionStatus.Closed_Dispositions)
                            {
                                steDetails.MaxPriorityTransactionStatusId = ddm.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = ddm.SelectedTransactionStatusName;
                                steDetails.StatusChangedDate = ddm.SelectedTransactionDate;
                                steDetails.SelectedDiligenceDisposition = ddm;
                                transactionClosedDate = ddm.ClosingDate;
                            }
                        }

                    }
                    else if (steDetails.AssetTypeId == (int)SamAssetType.Lease)
                    {
                        
                        foreach (DiligenceLeaseViewModel dl in steDetails.DiligenceLeaseList)
                        {

                            if (steDetails.MaxPriorityTransactionStatusId == 0)
                            {
                                steDetails.MaxPriorityTransactionStatusId = dl.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = dl.SelectedTransactionStatusName;

                                steDetails.DiligenceLease = dl;
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

                                steDetails.DiligenceLease = dl;
                                transactionClosedDate = dl.ClosingDate;
                            }

                            if (dl.SelectedTransactionStatusId == (int)SamsTransactionStatus.Under_LOI &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Under_Contract) &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Closed_Dispositions))
                            {
                                steDetails.MaxPriorityTransactionStatusId = dl.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = dl.SelectedTransactionStatusName;
                                steDetails.StatusChangedDate = dl.SelectedTransactionDate;

                                steDetails.DiligenceLease = dl;
                                transactionClosedDate = dl.ClosingDate;
                            }

                            if (dl.SelectedTransactionStatusId == (int)SamsTransactionStatus.Under_Contract &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Closed_Dispositions))
                            {
                                steDetails.MaxPriorityTransactionStatusId = dl.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = dl.SelectedTransactionStatusName;
                                steDetails.StatusChangedDate = dl.SelectedTransactionDate;

                                steDetails.DiligenceLease = dl;
                                transactionClosedDate = dl.ClosingDate;
                            }

                            if (dl.SelectedTransactionStatusId == (int)SamsTransactionStatus.Closed_Dispositions)
                            {
                                steDetails.MaxPriorityTransactionStatusId = dl.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = dl.SelectedTransactionStatusName;
                                steDetails.StatusChangedDate = dl.SelectedTransactionDate;
                                transactionClosedDate = dl.ClosingDate;
                                steDetails.DiligenceLease = dl;
                            }
                        }


                    }
                    else if (steDetails.AssetTypeId == (int)SamAssetType.NetLease)
                    {
                        steDetails.DiligenceNetleaseList = GetDiligenceNetleaseList(steDetails.NetleasePropertyId);
                        steDetails.SelectedDiligenceNetlease = new DiligenceNetleaseViewModel();


                        foreach (DiligenceNetleaseViewModel dl in steDetails.DiligenceNetleaseList)
                        {
                            if (dl.SelectedTransactionStatusId == (int)TransactionStatus.Closed_Acquisitions || dl.SelectedTransactionStatusId == (int)TransactionStatus.Under_Contract || dl.SelectedTransactionStatusId == (int)TransactionStatus.Under_LOI)
                            {
                                steDetails.CanAddTransactions = false;
                            }



                            if (steDetails.MaxPriorityTransactionStatusId == 0)
                            {
                                steDetails.MaxPriorityTransactionStatusId = dl.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = dl.SelectedTransactionStatusName;

                                steDetails.SelectedDiligenceNetlease = dl;
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

                                steDetails.SelectedDiligenceNetlease = dl;
                                transactionClosedDate = dl.ClosingDate;
                            }

                            if (dl.SelectedTransactionStatusId == (int)SamsTransactionStatus.Under_LOI &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Under_Contract) &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Closed_Dispositions))
                            {
                                steDetails.MaxPriorityTransactionStatusId = dl.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = dl.SelectedTransactionStatusName;

                                steDetails.SelectedDiligenceNetlease = dl;
                                transactionClosedDate = dl.ClosingDate;
                            }

                            if (dl.SelectedTransactionStatusId == (int)SamsTransactionStatus.Under_Contract &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Closed_Dispositions))
                            {
                                steDetails.MaxPriorityTransactionStatusId = dl.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = dl.SelectedTransactionStatusName;

                                steDetails.SelectedDiligenceNetlease = dl;
                                transactionClosedDate = dl.ClosingDate;
                            }

                            if (dl.SelectedTransactionStatusId == (int)SamsTransactionStatus.Closed_Dispositions)
                            {
                                steDetails.MaxPriorityTransactionStatusId = dl.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = dl.SelectedTransactionStatusName;
                                transactionClosedDate = dl.ClosingDate;
                                steDetails.SelectedDiligenceNetlease = dl;
                            }
                        }

                        

                    }
                    else if (steDetails.AssetTypeId == (int)SamAssetType.LeaseWithPurchaseOption)
                    {
                        steDetails.DiligenceLeaseWithPurchaseList = GetDiligenceLeaseWithPurchaseList(steDetails.NetleasePropertyId);
                        steDetails.DiligenceLeaseWithPurchase = new DiligenceLeaseWithPurchaseViewModel();

                        int leaseLoi = 0, leaseUnderContract = 0, leaseTerminated = 0, leaseClosed = 0;
                        foreach (DiligenceLeaseWithPurchaseViewModel dl in steDetails.DiligenceLeaseWithPurchaseList)
                        {

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
                                transactionClosedDate = dl.ClosingDate;
                                steDetails.DiligenceLeaseWithPurchase = dl;
                            }
                        }

                        
                    }
                    else if (steDetails.AssetTypeId == (int)SamAssetType.SaleLeaseBack)
                    {
                        steDetails.DiligenceDispositions_SaleLeaseBack = GetDiligenceDispositions_SaleLeaseBack(steDetails.NetleasePropertyId);



                        foreach (DiligenceDispositionsViewModel ddm in steDetails.DiligenceDispositions_SaleLeaseBack)
                        {
                            if (ddm.SelectedTransactionStatusId == (int)TransactionStatus.Closed_Acquisitions || ddm.SelectedTransactionStatusId == (int)TransactionStatus.Under_Contract || ddm.SelectedTransactionStatusId == (int)TransactionStatus.Under_LOI)
                            {
                                steDetails.CanAddTransactions = false;
                            }

                            steDetails.SelectedDiligenceDisposition = ddm;
                            transactionClosedDate = ddm.ClosingDate;

                            if (steDetails.MaxPriorityTransactionStatusId == 0)
                            {
                                steDetails.MaxPriorityTransactionStatusId = ddm.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = ddm.SelectedTransactionStatusName;


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

                                steDetails.SelectedDiligenceDisposition = ddm;
                                transactionClosedDate = ddm.ClosingDate;
                            }

                            if (ddm.SelectedTransactionStatusId == (int)SamsTransactionStatus.Under_LOI &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Under_Contract) &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Closed_Dispositions))
                            {
                                steDetails.MaxPriorityTransactionStatusId = ddm.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = ddm.SelectedTransactionStatusName;
                                steDetails.StatusChangedDate = ddm.SelectedTransactionDate;

                                steDetails.SelectedDiligenceDisposition = ddm;
                                transactionClosedDate = ddm.ClosingDate;
                            }

                            if (ddm.SelectedTransactionStatusId == (int)SamsTransactionStatus.Under_Contract &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Closed_Dispositions))
                            {
                                steDetails.MaxPriorityTransactionStatusId = ddm.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = ddm.SelectedTransactionStatusName;
                                steDetails.StatusChangedDate = ddm.SelectedTransactionDate;

                                steDetails.SelectedDiligenceDisposition = ddm;
                                transactionClosedDate = ddm.ClosingDate;
                            }

                            if (ddm.SelectedTransactionStatusId == (int)SamsTransactionStatus.Closed_Dispositions)
                            {
                                steDetails.MaxPriorityTransactionStatusId = ddm.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = ddm.SelectedTransactionStatusName;
                                steDetails.StatusChangedDate = ddm.SelectedTransactionDate;
                                transactionClosedDate = ddm.ClosingDate;
                                steDetails.SelectedDiligenceDisposition = ddm;
                            }
                        }

                        

                    }

                    steDetails.TodoList = GetTodoList(steDetails.NetleasePropertyId);
                    StringBuilder todoText = new StringBuilder();
                    if (steDetails.TodoList.Count > 0)
                    {
                        foreach(TodoViewModel td in steDetails.TodoList)
                        {
                            todoText.Append(td.TodoText + "\r\n");
                        }
                    }
                    steDetails.LatestComment = todoText.ToString();
                    netLeasePropertiesList.Add(steDetails);
                }
                con.Close();
            }

            return View(netLeasePropertiesList);
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

            List<NetleasePropertiesViewModel> netLeasePropertiesList = new List<NetleasePropertiesViewModel>();

            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                /*
                 * 1	Available
                 * 2	Under contract
                 * 3	Sold
                 */
                 /*
                SqlCommand cmd = new SqlCommand("GetNetleasePropertyListByStatus", con);
                cmd.Parameters.AddWithValue("property_status_id", 1);
                */

                SqlCommand cmd = new SqlCommand("GetNetleasePropertyList", con);
                cmd.Parameters.AddWithValue("asset_status", 0);

                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var steDetails = new NetleasePropertiesViewModel();
                    steDetails.NetleasePropertyId = reader.IsDBNull(reader.GetOrdinal("net_lease_property_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("net_lease_property_id"));
                    steDetails.AssetId = reader.IsDBNull(reader.GetOrdinal("asset_id")) ? "" : reader.GetString(reader.GetOrdinal("asset_id"));
                    steDetails.AssetName = reader.IsDBNull(reader.GetOrdinal("asset_name")) ? "" : reader.GetString(reader.GetOrdinal("asset_name"));
                    steDetails.StateId = reader.IsDBNull(reader.GetOrdinal("state_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("state_id"));
                    steDetails.City = reader.IsDBNull(reader.GetOrdinal("city")) ? "" : reader.GetString(reader.GetOrdinal("city"));

                    steDetails.PropertyPrice = reader.IsDBNull(reader.GetOrdinal("property_price")) ? "" : reader.GetString(reader.GetOrdinal("property_price"));
                    steDetails.CapRate = reader.IsDBNull(reader.GetOrdinal("cap_rate")) ? 0 : reader.GetDouble(reader.GetOrdinal("cap_rate"));

                    steDetails.Term = reader.IsDBNull(reader.GetOrdinal("term")) ? "" : reader.GetString(reader.GetOrdinal("term"));

                    steDetails.PdfFileName = reader.IsDBNull(reader.GetOrdinal("detail_pdf")) ? "" : reader.GetString(reader.GetOrdinal("detail_pdf"));
                    steDetails.StateName = reader.IsDBNull(reader.GetOrdinal("state_name")) ? "" : reader.GetString(reader.GetOrdinal("state_name"));

                    steDetails.CreatedDate = reader.IsDBNull(reader.GetOrdinal("created_date")) ? DateTime.Now : reader.GetDateTime(reader.GetOrdinal("created_date"));
                    steDetails.AssetTypeId = reader.IsDBNull(reader.GetOrdinal("asset_type_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("asset_type_id"));
                    steDetails.AssetTypeName = reader.IsDBNull(reader.GetOrdinal("asset_type_name")) ? "" : reader.GetString(reader.GetOrdinal("asset_type_name"));
                    steDetails.IsShoppingCenter = reader.IsDBNull(reader.GetOrdinal("is_shopping_center")) ? false : reader.GetBoolean(reader.GetOrdinal("is_shopping_center"));
                    if (steDetails.IsShoppingCenter)
                    {
                        steDetails.ShoppingCenterOrNetlease = "Shopping Center";
                    }
                    else
                    {
                        steDetails.ShoppingCenterOrNetlease = "Net Lease";
                    }
                    steDetails.Address = reader.IsDBNull(reader.GetOrdinal("property_address")) ? "" : reader.GetString(reader.GetOrdinal("property_address"));
                    steDetails.ZipCode = reader.IsDBNull(reader.GetOrdinal("property_zipcode")) ? "" : reader.GetString(reader.GetOrdinal("property_zipcode"));

                    steDetails.SelectedPropertyStatusId = reader.IsDBNull(reader.GetOrdinal("property_status_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("property_status_id"));
                    steDetails.SelectedPropertyStatus = reader.IsDBNull(reader.GetOrdinal("property_status")) ? "" : reader.GetString(reader.GetOrdinal("property_status"));

                    if (steDetails.Address.Length > 15)
                    {
                        steDetails.AddressShort = steDetails.Address.Substring(0, 15) + "..";
                    }
                    else
                    {
                        steDetails.AddressShort = steDetails.Address;
                    }


                    steDetails.TransactionStatusName = "";

                    steDetails.DiligenceDispositionList = GetDiligenceDispositions(steDetails.NetleasePropertyId);

                    steDetails.DiligenceAcquisitions = GetDiligenceAcquisition(steDetails.NetleasePropertyId);
                    steDetails.DiligenceDispositionList = GetDiligenceDispositions(steDetails.NetleasePropertyId);
                    steDetails.DiligenceLeaseList = GetDiligenceLease(steDetails.NetleasePropertyId);

                    steDetails.DispositionPeriodList = GetPeriodList(steDetails.NetleasePropertyId, "Disposition");
                    steDetails.LeasePeriodList = GetPeriodList(steDetails.NetleasePropertyId, "Lease");

                    steDetails.LeaseTypeList = GetLeaseTypeList();
                    steDetails.FutureTenantList = GetFutureTenantList(steDetails.NetleasePropertyId);

                    steDetails.DiligenceDispositions = new DiligenceDispositionsViewModel();
                    steDetails.SelectedDiligenceDisposition = new DiligenceDispositionsViewModel();

                    DateTime? transactionClosedDate = default(DateTime?);

                    steDetails.DiligenceLease = new DiligenceLeaseViewModel();
                    steDetails.SelectedDiligenceDisposition = new DiligenceDispositionsViewModel();
                    steDetails.DiligenceLeaseWithPurchase = new DiligenceLeaseWithPurchaseViewModel();
                    steDetails.SelectedDiligenceNetlease = new DiligenceNetleaseViewModel();

                    int saleLoi = 0, saleUnderContract = 0, saleTerminated = 0, saleClosed = 0;


                    if (steDetails.AssetTypeId == (int)SamAssetType.Fee || steDetails.AssetTypeId == (int)SamAssetType.FeeSubjectToLease)
                    {
                        steDetails.SelectedDiligenceDisposition = new DiligenceDispositionsViewModel();
                        foreach (DiligenceDispositionsViewModel ddm in steDetails.DiligenceDispositionList)
                        {
                            if (ddm.SelectedTransactionStatusId == (int)TransactionStatus.Closed_Acquisitions || ddm.SelectedTransactionStatusId == (int)TransactionStatus.Under_Contract || ddm.SelectedTransactionStatusId == (int)TransactionStatus.Under_LOI)
                            {
                                steDetails.CanAddTransactions = false;
                            }

                            if (steDetails.MaxPriorityTransactionStatusId == 0)
                            {
                                steDetails.MaxPriorityTransactionStatusId = ddm.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = ddm.SelectedTransactionStatusName;

                                steDetails.SelectedDiligenceDisposition = ddm;
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

                                steDetails.SelectedDiligenceDisposition = ddm;
                                transactionClosedDate = ddm.ClosingDate;

                            }

                            if (ddm.SelectedTransactionStatusId == (int)SamsTransactionStatus.Under_LOI &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Under_Contract) &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Closed_Dispositions))
                            {
                                steDetails.MaxPriorityTransactionStatusId = ddm.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = ddm.SelectedTransactionStatusName;
                                steDetails.StatusChangedDate = ddm.SelectedTransactionDate;

                                steDetails.SelectedDiligenceDisposition = ddm;
                                transactionClosedDate = ddm.ClosingDate;
                            }

                            if (ddm.SelectedTransactionStatusId == (int)SamsTransactionStatus.Under_Contract &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Closed_Dispositions))
                            {
                                steDetails.MaxPriorityTransactionStatusId = ddm.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = ddm.SelectedTransactionStatusName;
                                steDetails.StatusChangedDate = ddm.SelectedTransactionDate;

                                steDetails.SelectedDiligenceDisposition = ddm;
                                transactionClosedDate = ddm.ClosingDate;
                            }

                            if (ddm.SelectedTransactionStatusId == (int)SamsTransactionStatus.Closed_Dispositions)
                            {
                                steDetails.MaxPriorityTransactionStatusId = ddm.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = ddm.SelectedTransactionStatusName;
                                steDetails.StatusChangedDate = ddm.SelectedTransactionDate;
                                steDetails.SelectedDiligenceDisposition = ddm;
                                transactionClosedDate = ddm.ClosingDate;
                            }

                            
                        }

                        
                    }
                    else if (steDetails.AssetTypeId == (int)SamAssetType.Lease)
                    {

                        foreach (DiligenceLeaseViewModel dl in steDetails.DiligenceLeaseList)
                        {

                            if (steDetails.MaxPriorityTransactionStatusId == 0)
                            {
                                steDetails.MaxPriorityTransactionStatusId = dl.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = dl.SelectedTransactionStatusName;

                                steDetails.DiligenceLease = dl;
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

                                steDetails.DiligenceLease = dl;
                                transactionClosedDate = dl.ClosingDate;
                            }

                            if (dl.SelectedTransactionStatusId == (int)SamsTransactionStatus.Under_LOI &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Under_Contract) &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Closed_Dispositions))
                            {
                                steDetails.MaxPriorityTransactionStatusId = dl.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = dl.SelectedTransactionStatusName;
                                steDetails.StatusChangedDate = dl.SelectedTransactionDate;

                                steDetails.DiligenceLease = dl;
                                transactionClosedDate = dl.ClosingDate;
                            }

                            if (dl.SelectedTransactionStatusId == (int)SamsTransactionStatus.Under_Contract &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Closed_Dispositions))
                            {
                                steDetails.MaxPriorityTransactionStatusId = dl.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = dl.SelectedTransactionStatusName;
                                steDetails.StatusChangedDate = dl.SelectedTransactionDate;

                                steDetails.DiligenceLease = dl;
                                transactionClosedDate = dl.ClosingDate;
                            }

                            if (dl.SelectedTransactionStatusId == (int)SamsTransactionStatus.Closed_Dispositions)
                            {
                                steDetails.MaxPriorityTransactionStatusId = dl.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = dl.SelectedTransactionStatusName;
                                steDetails.StatusChangedDate = dl.SelectedTransactionDate;
                                transactionClosedDate = dl.ClosingDate;
                                steDetails.DiligenceLease = dl;
                            }
                        }


                    }
                    else if (steDetails.AssetTypeId == (int)SamAssetType.NetLease)
                    {
                        steDetails.DiligenceNetleaseList = GetDiligenceNetleaseList(steDetails.NetleasePropertyId);
                        steDetails.SelectedDiligenceNetlease = new DiligenceNetleaseViewModel();


                        foreach (DiligenceNetleaseViewModel dl in steDetails.DiligenceNetleaseList)
                        {
                            if (dl.SelectedTransactionStatusId == (int)TransactionStatus.Closed_Acquisitions || dl.SelectedTransactionStatusId == (int)TransactionStatus.Under_Contract || dl.SelectedTransactionStatusId == (int)TransactionStatus.Under_LOI)
                            {
                                steDetails.CanAddTransactions = false;
                            }



                            if (steDetails.MaxPriorityTransactionStatusId == 0)
                            {
                                steDetails.MaxPriorityTransactionStatusId = dl.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = dl.SelectedTransactionStatusName;

                                steDetails.SelectedDiligenceNetlease = dl;
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

                                steDetails.SelectedDiligenceNetlease = dl;
                                transactionClosedDate = dl.ClosingDate;
                            }

                            if (dl.SelectedTransactionStatusId == (int)SamsTransactionStatus.Under_LOI &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Under_Contract) &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Closed_Dispositions))
                            {
                                steDetails.MaxPriorityTransactionStatusId = dl.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = dl.SelectedTransactionStatusName;

                                steDetails.SelectedDiligenceNetlease = dl;
                                transactionClosedDate = dl.ClosingDate;
                            }

                            if (dl.SelectedTransactionStatusId == (int)SamsTransactionStatus.Under_Contract &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Closed_Dispositions))
                            {
                                steDetails.MaxPriorityTransactionStatusId = dl.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = dl.SelectedTransactionStatusName;

                                steDetails.SelectedDiligenceNetlease = dl;
                                transactionClosedDate = dl.ClosingDate;
                            }

                            if (dl.SelectedTransactionStatusId == (int)SamsTransactionStatus.Closed_Dispositions)
                            {
                                steDetails.MaxPriorityTransactionStatusId = dl.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = dl.SelectedTransactionStatusName;
                                transactionClosedDate = dl.ClosingDate;
                                steDetails.SelectedDiligenceNetlease = dl;
                            }
                        }



                    }
                    else if (steDetails.AssetTypeId == (int)SamAssetType.LeaseWithPurchaseOption)
                    {
                        steDetails.DiligenceLeaseWithPurchaseList = GetDiligenceLeaseWithPurchaseList(steDetails.NetleasePropertyId);
                        steDetails.DiligenceLeaseWithPurchase = new DiligenceLeaseWithPurchaseViewModel();

                        int leaseLoi = 0, leaseUnderContract = 0, leaseTerminated = 0, leaseClosed = 0;
                        foreach (DiligenceLeaseWithPurchaseViewModel dl in steDetails.DiligenceLeaseWithPurchaseList)
                        {

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
                                transactionClosedDate = dl.ClosingDate;
                                steDetails.DiligenceLeaseWithPurchase = dl;
                            }
                        }


                    }
                    else if (steDetails.AssetTypeId == (int)SamAssetType.SaleLeaseBack)
                    {
                        steDetails.DiligenceDispositions_SaleLeaseBack = GetDiligenceDispositions_SaleLeaseBack(steDetails.NetleasePropertyId);



                        foreach (DiligenceDispositionsViewModel ddm in steDetails.DiligenceDispositions_SaleLeaseBack)
                        {
                            if (ddm.SelectedTransactionStatusId == (int)TransactionStatus.Closed_Acquisitions || ddm.SelectedTransactionStatusId == (int)TransactionStatus.Under_Contract || ddm.SelectedTransactionStatusId == (int)TransactionStatus.Under_LOI)
                            {
                                steDetails.CanAddTransactions = false;
                            }

                            steDetails.SelectedDiligenceDisposition = ddm;
                            transactionClosedDate = ddm.ClosingDate;

                            if (steDetails.MaxPriorityTransactionStatusId == 0)
                            {
                                steDetails.MaxPriorityTransactionStatusId = ddm.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = ddm.SelectedTransactionStatusName;


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

                                steDetails.SelectedDiligenceDisposition = ddm;
                                transactionClosedDate = ddm.ClosingDate;
                            }

                            if (ddm.SelectedTransactionStatusId == (int)SamsTransactionStatus.Under_LOI &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Under_Contract) &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Closed_Dispositions))
                            {
                                steDetails.MaxPriorityTransactionStatusId = ddm.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = ddm.SelectedTransactionStatusName;
                                steDetails.StatusChangedDate = ddm.SelectedTransactionDate;

                                steDetails.SelectedDiligenceDisposition = ddm;
                                transactionClosedDate = ddm.ClosingDate;
                            }

                            if (ddm.SelectedTransactionStatusId == (int)SamsTransactionStatus.Under_Contract &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Closed_Dispositions))
                            {
                                steDetails.MaxPriorityTransactionStatusId = ddm.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = ddm.SelectedTransactionStatusName;
                                steDetails.StatusChangedDate = ddm.SelectedTransactionDate;

                                steDetails.SelectedDiligenceDisposition = ddm;
                                transactionClosedDate = ddm.ClosingDate;
                            }

                            if (ddm.SelectedTransactionStatusId == (int)SamsTransactionStatus.Closed_Dispositions)
                            {
                                steDetails.MaxPriorityTransactionStatusId = ddm.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = ddm.SelectedTransactionStatusName;
                                steDetails.StatusChangedDate = ddm.SelectedTransactionDate;
                                transactionClosedDate = ddm.ClosingDate;
                                steDetails.SelectedDiligenceDisposition = ddm;
                            }
                        }



                    }

                    steDetails.TodoList = GetTodoList(steDetails.NetleasePropertyId);
                    StringBuilder todoText = new StringBuilder();
                    if (steDetails.TodoList.Count > 0)
                    {
                        foreach (TodoViewModel td in steDetails.TodoList)
                        {
                            todoText.Append(td.TodoText + "\r\n");
                        }
                    }
                    steDetails.LatestComment = todoText.ToString();

                    if (steDetails.MaxPriorityTransactionStatusId == (int)SamsTransactionStatus.LOI_Received || steDetails.MaxPriorityTransactionStatusId == 0)
                    {
                        netLeasePropertiesList.Add(steDetails);
                    }
                    
                }
                con.Close();
            }

            return View(netLeasePropertiesList);
        }


        public IActionResult ViewUnderContractProperty()
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

            List<NetleasePropertiesViewModel> netLeasePropertiesList = new List<NetleasePropertiesViewModel>();

            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                /*
                 * 1	Available
                 * 2	Under contract
                 * 3	Sold
                 */
                /*
               SqlCommand cmd = new SqlCommand("GetNetleasePropertyListByStatus", con);
               cmd.Parameters.AddWithValue("property_status_id", 2);
               */

                SqlCommand cmd = new SqlCommand("GetNetleasePropertyList", con);
                cmd.Parameters.AddWithValue("asset_status", 0);

                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var steDetails = new NetleasePropertiesViewModel();
                    steDetails.NetleasePropertyId = reader.IsDBNull(reader.GetOrdinal("net_lease_property_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("net_lease_property_id"));
                    steDetails.AssetId = reader.IsDBNull(reader.GetOrdinal("asset_id")) ? "" : reader.GetString(reader.GetOrdinal("asset_id"));
                    steDetails.AssetName = reader.IsDBNull(reader.GetOrdinal("asset_name")) ? "" : reader.GetString(reader.GetOrdinal("asset_name"));
                    steDetails.StateId = reader.IsDBNull(reader.GetOrdinal("state_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("state_id"));
                    steDetails.City = reader.IsDBNull(reader.GetOrdinal("city")) ? "" : reader.GetString(reader.GetOrdinal("city"));

                    steDetails.PropertyPrice = reader.IsDBNull(reader.GetOrdinal("property_price")) ? "" : reader.GetString(reader.GetOrdinal("property_price"));
                    steDetails.CapRate = reader.IsDBNull(reader.GetOrdinal("cap_rate")) ? 0 : reader.GetDouble(reader.GetOrdinal("cap_rate"));

                    steDetails.Term = reader.IsDBNull(reader.GetOrdinal("term")) ? "" : reader.GetString(reader.GetOrdinal("term"));

                    steDetails.PdfFileName = reader.IsDBNull(reader.GetOrdinal("detail_pdf")) ? "" : reader.GetString(reader.GetOrdinal("detail_pdf"));
                    steDetails.StateName = reader.IsDBNull(reader.GetOrdinal("state_name")) ? "" : reader.GetString(reader.GetOrdinal("state_name"));

                    steDetails.CreatedDate = reader.IsDBNull(reader.GetOrdinal("created_date")) ? DateTime.Now : reader.GetDateTime(reader.GetOrdinal("created_date"));
                    steDetails.AssetTypeId = reader.IsDBNull(reader.GetOrdinal("asset_type_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("asset_type_id"));
                    steDetails.AssetTypeName = reader.IsDBNull(reader.GetOrdinal("asset_type_name")) ? "" : reader.GetString(reader.GetOrdinal("asset_type_name"));
                    steDetails.IsShoppingCenter = reader.IsDBNull(reader.GetOrdinal("is_shopping_center")) ? false : reader.GetBoolean(reader.GetOrdinal("is_shopping_center"));
                    if (steDetails.IsShoppingCenter)
                    {
                        steDetails.ShoppingCenterOrNetlease = "Shopping Center";
                    }
                    else
                    {
                        steDetails.ShoppingCenterOrNetlease = "Net Lease";
                    }
                    steDetails.Address = reader.IsDBNull(reader.GetOrdinal("property_address")) ? "" : reader.GetString(reader.GetOrdinal("property_address"));
                    steDetails.ZipCode = reader.IsDBNull(reader.GetOrdinal("property_zipcode")) ? "" : reader.GetString(reader.GetOrdinal("property_zipcode"));

                    steDetails.SelectedPropertyStatusId = reader.IsDBNull(reader.GetOrdinal("property_status_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("property_status_id"));
                    steDetails.SelectedPropertyStatus = reader.IsDBNull(reader.GetOrdinal("property_status")) ? "" : reader.GetString(reader.GetOrdinal("property_status"));

                    if (steDetails.Address.Length > 15)
                    {
                        steDetails.AddressShort = steDetails.Address.Substring(0, 15) + "..";
                    }
                    else
                    {
                        steDetails.AddressShort = steDetails.Address;
                    }


                    steDetails.TransactionStatusName = "";

                    steDetails.DiligenceDispositionList = GetDiligenceDispositions(steDetails.NetleasePropertyId);

                    steDetails.DiligenceAcquisitions = GetDiligenceAcquisition(steDetails.NetleasePropertyId);
                    steDetails.DiligenceDispositionList = GetDiligenceDispositions(steDetails.NetleasePropertyId);
                    steDetails.DiligenceLeaseList = GetDiligenceLease(steDetails.NetleasePropertyId);

                    steDetails.DispositionPeriodList = GetPeriodList(steDetails.NetleasePropertyId, "Disposition");
                    steDetails.LeasePeriodList = GetPeriodList(steDetails.NetleasePropertyId, "Lease");

                    steDetails.LeaseTypeList = GetLeaseTypeList();
                    steDetails.FutureTenantList = GetFutureTenantList(steDetails.NetleasePropertyId);

                    steDetails.DiligenceDispositions = new DiligenceDispositionsViewModel();
                    steDetails.SelectedDiligenceDisposition = new DiligenceDispositionsViewModel();

                    DateTime? transactionClosedDate = default(DateTime?);

                    steDetails.DiligenceLease = new DiligenceLeaseViewModel();
                    steDetails.SelectedDiligenceDisposition = new DiligenceDispositionsViewModel();
                    steDetails.DiligenceLeaseWithPurchase = new DiligenceLeaseWithPurchaseViewModel();
                    steDetails.SelectedDiligenceNetlease = new DiligenceNetleaseViewModel();

                    int saleLoi = 0, saleUnderContract = 0, saleTerminated = 0, saleClosed = 0;


                    if (steDetails.AssetTypeId == (int)SamAssetType.Fee || steDetails.AssetTypeId == (int)SamAssetType.FeeSubjectToLease)
                    {
                        steDetails.SelectedDiligenceDisposition = new DiligenceDispositionsViewModel();
                        foreach (DiligenceDispositionsViewModel ddm in steDetails.DiligenceDispositionList)
                        {
                            if (ddm.SelectedTransactionStatusId == (int)TransactionStatus.Closed_Acquisitions || ddm.SelectedTransactionStatusId == (int)TransactionStatus.Under_Contract || ddm.SelectedTransactionStatusId == (int)TransactionStatus.Under_LOI)
                            {
                                steDetails.CanAddTransactions = false;
                            }

                            if (steDetails.MaxPriorityTransactionStatusId == 0)
                            {
                                steDetails.MaxPriorityTransactionStatusId = ddm.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = ddm.SelectedTransactionStatusName;

                                steDetails.SelectedDiligenceDisposition = ddm;
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

                                steDetails.SelectedDiligenceDisposition = ddm;
                                transactionClosedDate = ddm.ClosingDate;

                            }

                            if (ddm.SelectedTransactionStatusId == (int)SamsTransactionStatus.Under_LOI &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Under_Contract) &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Closed_Dispositions))
                            {
                                steDetails.MaxPriorityTransactionStatusId = ddm.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = ddm.SelectedTransactionStatusName;
                                steDetails.StatusChangedDate = ddm.SelectedTransactionDate;

                                steDetails.SelectedDiligenceDisposition = ddm;
                                transactionClosedDate = ddm.ClosingDate;
                            }

                            if (ddm.SelectedTransactionStatusId == (int)SamsTransactionStatus.Under_Contract &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Closed_Dispositions))
                            {
                                steDetails.MaxPriorityTransactionStatusId = ddm.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = ddm.SelectedTransactionStatusName;
                                steDetails.StatusChangedDate = ddm.SelectedTransactionDate;

                                steDetails.SelectedDiligenceDisposition = ddm;
                                transactionClosedDate = ddm.ClosingDate;
                            }

                            if (ddm.SelectedTransactionStatusId == (int)SamsTransactionStatus.Closed_Dispositions)
                            {
                                steDetails.MaxPriorityTransactionStatusId = ddm.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = ddm.SelectedTransactionStatusName;
                                steDetails.StatusChangedDate = ddm.SelectedTransactionDate;
                                steDetails.SelectedDiligenceDisposition = ddm;
                                transactionClosedDate = ddm.ClosingDate;
                            }


                        }


                    }
                    else if (steDetails.AssetTypeId == (int)SamAssetType.Lease)
                    {

                        foreach (DiligenceLeaseViewModel dl in steDetails.DiligenceLeaseList)
                        {

                            if (steDetails.MaxPriorityTransactionStatusId == 0)
                            {
                                steDetails.MaxPriorityTransactionStatusId = dl.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = dl.SelectedTransactionStatusName;

                                steDetails.DiligenceLease = dl;
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

                                steDetails.DiligenceLease = dl;
                                transactionClosedDate = dl.ClosingDate;
                            }

                            if (dl.SelectedTransactionStatusId == (int)SamsTransactionStatus.Under_LOI &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Under_Contract) &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Closed_Dispositions))
                            {
                                steDetails.MaxPriorityTransactionStatusId = dl.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = dl.SelectedTransactionStatusName;
                                steDetails.StatusChangedDate = dl.SelectedTransactionDate;

                                steDetails.DiligenceLease = dl;
                                transactionClosedDate = dl.ClosingDate;
                            }

                            if (dl.SelectedTransactionStatusId == (int)SamsTransactionStatus.Under_Contract &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Closed_Dispositions))
                            {
                                steDetails.MaxPriorityTransactionStatusId = dl.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = dl.SelectedTransactionStatusName;
                                steDetails.StatusChangedDate = dl.SelectedTransactionDate;

                                steDetails.DiligenceLease = dl;
                                transactionClosedDate = dl.ClosingDate;
                            }

                            if (dl.SelectedTransactionStatusId == (int)SamsTransactionStatus.Closed_Dispositions)
                            {
                                steDetails.MaxPriorityTransactionStatusId = dl.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = dl.SelectedTransactionStatusName;
                                steDetails.StatusChangedDate = dl.SelectedTransactionDate;
                                transactionClosedDate = dl.ClosingDate;
                                steDetails.DiligenceLease = dl;
                            }
                        }


                    }
                    else if (steDetails.AssetTypeId == (int)SamAssetType.NetLease)
                    {
                        steDetails.DiligenceNetleaseList = GetDiligenceNetleaseList(steDetails.NetleasePropertyId);
                        steDetails.SelectedDiligenceNetlease = new DiligenceNetleaseViewModel();


                        foreach (DiligenceNetleaseViewModel dl in steDetails.DiligenceNetleaseList)
                        {
                            if (dl.SelectedTransactionStatusId == (int)TransactionStatus.Closed_Acquisitions || dl.SelectedTransactionStatusId == (int)TransactionStatus.Under_Contract || dl.SelectedTransactionStatusId == (int)TransactionStatus.Under_LOI)
                            {
                                steDetails.CanAddTransactions = false;
                            }



                            if (steDetails.MaxPriorityTransactionStatusId == 0)
                            {
                                steDetails.MaxPriorityTransactionStatusId = dl.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = dl.SelectedTransactionStatusName;

                                steDetails.SelectedDiligenceNetlease = dl;
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

                                steDetails.SelectedDiligenceNetlease = dl;
                                transactionClosedDate = dl.ClosingDate;
                            }

                            if (dl.SelectedTransactionStatusId == (int)SamsTransactionStatus.Under_LOI &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Under_Contract) &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Closed_Dispositions))
                            {
                                steDetails.MaxPriorityTransactionStatusId = dl.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = dl.SelectedTransactionStatusName;

                                steDetails.SelectedDiligenceNetlease = dl;
                                transactionClosedDate = dl.ClosingDate;
                            }

                            if (dl.SelectedTransactionStatusId == (int)SamsTransactionStatus.Under_Contract &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Closed_Dispositions))
                            {
                                steDetails.MaxPriorityTransactionStatusId = dl.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = dl.SelectedTransactionStatusName;

                                steDetails.SelectedDiligenceNetlease = dl;
                                transactionClosedDate = dl.ClosingDate;
                            }

                            if (dl.SelectedTransactionStatusId == (int)SamsTransactionStatus.Closed_Dispositions)
                            {
                                steDetails.MaxPriorityTransactionStatusId = dl.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = dl.SelectedTransactionStatusName;
                                transactionClosedDate = dl.ClosingDate;
                                steDetails.SelectedDiligenceNetlease = dl;
                            }
                        }



                    }
                    else if (steDetails.AssetTypeId == (int)SamAssetType.LeaseWithPurchaseOption)
                    {
                        steDetails.DiligenceLeaseWithPurchaseList = GetDiligenceLeaseWithPurchaseList(steDetails.NetleasePropertyId);
                        steDetails.DiligenceLeaseWithPurchase = new DiligenceLeaseWithPurchaseViewModel();

                        int leaseLoi = 0, leaseUnderContract = 0, leaseTerminated = 0, leaseClosed = 0;
                        foreach (DiligenceLeaseWithPurchaseViewModel dl in steDetails.DiligenceLeaseWithPurchaseList)
                        {

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
                                transactionClosedDate = dl.ClosingDate;
                                steDetails.DiligenceLeaseWithPurchase = dl;
                            }
                        }


                    }
                    else if (steDetails.AssetTypeId == (int)SamAssetType.SaleLeaseBack)
                    {
                        steDetails.DiligenceDispositions_SaleLeaseBack = GetDiligenceDispositions_SaleLeaseBack(steDetails.NetleasePropertyId);



                        foreach (DiligenceDispositionsViewModel ddm in steDetails.DiligenceDispositions_SaleLeaseBack)
                        {
                            if (ddm.SelectedTransactionStatusId == (int)TransactionStatus.Closed_Acquisitions || ddm.SelectedTransactionStatusId == (int)TransactionStatus.Under_Contract || ddm.SelectedTransactionStatusId == (int)TransactionStatus.Under_LOI)
                            {
                                steDetails.CanAddTransactions = false;
                            }

                            steDetails.SelectedDiligenceDisposition = ddm;
                            transactionClosedDate = ddm.ClosingDate;

                            if (steDetails.MaxPriorityTransactionStatusId == 0)
                            {
                                steDetails.MaxPriorityTransactionStatusId = ddm.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = ddm.SelectedTransactionStatusName;


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

                                steDetails.SelectedDiligenceDisposition = ddm;
                                transactionClosedDate = ddm.ClosingDate;
                            }

                            if (ddm.SelectedTransactionStatusId == (int)SamsTransactionStatus.Under_LOI &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Under_Contract) &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Closed_Dispositions))
                            {
                                steDetails.MaxPriorityTransactionStatusId = ddm.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = ddm.SelectedTransactionStatusName;
                                steDetails.StatusChangedDate = ddm.SelectedTransactionDate;

                                steDetails.SelectedDiligenceDisposition = ddm;
                                transactionClosedDate = ddm.ClosingDate;
                            }

                            if (ddm.SelectedTransactionStatusId == (int)SamsTransactionStatus.Under_Contract &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Closed_Dispositions))
                            {
                                steDetails.MaxPriorityTransactionStatusId = ddm.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = ddm.SelectedTransactionStatusName;
                                steDetails.StatusChangedDate = ddm.SelectedTransactionDate;

                                steDetails.SelectedDiligenceDisposition = ddm;
                                transactionClosedDate = ddm.ClosingDate;
                            }

                            if (ddm.SelectedTransactionStatusId == (int)SamsTransactionStatus.Closed_Dispositions)
                            {
                                steDetails.MaxPriorityTransactionStatusId = ddm.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = ddm.SelectedTransactionStatusName;
                                steDetails.StatusChangedDate = ddm.SelectedTransactionDate;
                                transactionClosedDate = ddm.ClosingDate;
                                steDetails.SelectedDiligenceDisposition = ddm;
                            }
                        }



                    }

                    steDetails.TodoList = GetTodoList(steDetails.NetleasePropertyId);
                    StringBuilder todoText = new StringBuilder();
                    if (steDetails.TodoList.Count > 0)
                    {
                        foreach (TodoViewModel td in steDetails.TodoList)
                        {
                            todoText.Append(td.TodoText + "\r\n");
                        }
                    }
                    steDetails.LatestComment = todoText.ToString();

                    if (steDetails.MaxPriorityTransactionStatusId == (int)SamsTransactionStatus.Under_Contract)
                    {
                        netLeasePropertiesList.Add(steDetails);
                    }

                }
                con.Close();
            }

            return View(netLeasePropertiesList);
        }

        public IActionResult ViewSoldOutProperty()
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

            List<NetleasePropertiesViewModel> netLeasePropertiesList = new List<NetleasePropertiesViewModel>();

            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                /*
                 * 1	Available
                 * 2	Under contract
                 * 3	Sold
                 */
                /*
               SqlCommand cmd = new SqlCommand("GetNetleasePropertyListByStatus", con);
               cmd.Parameters.AddWithValue("property_status_id", 3);
               */
                SqlCommand cmd = new SqlCommand("GetNetleasePropertyList", con);
                cmd.Parameters.AddWithValue("asset_status", 0);

                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var steDetails = new NetleasePropertiesViewModel();
                    steDetails.NetleasePropertyId = reader.IsDBNull(reader.GetOrdinal("net_lease_property_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("net_lease_property_id"));
                    steDetails.AssetId = reader.IsDBNull(reader.GetOrdinal("asset_id")) ? "" : reader.GetString(reader.GetOrdinal("asset_id"));
                    steDetails.AssetName = reader.IsDBNull(reader.GetOrdinal("asset_name")) ? "" : reader.GetString(reader.GetOrdinal("asset_name"));
                    steDetails.StateId = reader.IsDBNull(reader.GetOrdinal("state_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("state_id"));
                    steDetails.City = reader.IsDBNull(reader.GetOrdinal("city")) ? "" : reader.GetString(reader.GetOrdinal("city"));

                    steDetails.PropertyPrice = reader.IsDBNull(reader.GetOrdinal("property_price")) ? "" : reader.GetString(reader.GetOrdinal("property_price"));
                    steDetails.CapRate = reader.IsDBNull(reader.GetOrdinal("cap_rate")) ? 0 : reader.GetDouble(reader.GetOrdinal("cap_rate"));

                    steDetails.Term = reader.IsDBNull(reader.GetOrdinal("term")) ? "" : reader.GetString(reader.GetOrdinal("term"));

                    steDetails.PdfFileName = reader.IsDBNull(reader.GetOrdinal("detail_pdf")) ? "" : reader.GetString(reader.GetOrdinal("detail_pdf"));
                    steDetails.StateName = reader.IsDBNull(reader.GetOrdinal("state_name")) ? "" : reader.GetString(reader.GetOrdinal("state_name"));

                    steDetails.CreatedDate = reader.IsDBNull(reader.GetOrdinal("created_date")) ? DateTime.Now : reader.GetDateTime(reader.GetOrdinal("created_date"));
                    steDetails.AssetTypeId = reader.IsDBNull(reader.GetOrdinal("asset_type_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("asset_type_id"));
                    steDetails.AssetTypeName = reader.IsDBNull(reader.GetOrdinal("asset_type_name")) ? "" : reader.GetString(reader.GetOrdinal("asset_type_name"));
                    steDetails.IsShoppingCenter = reader.IsDBNull(reader.GetOrdinal("is_shopping_center")) ? false : reader.GetBoolean(reader.GetOrdinal("is_shopping_center"));
                    if (steDetails.IsShoppingCenter)
                    {
                        steDetails.ShoppingCenterOrNetlease = "Shopping Center";
                    }
                    else
                    {
                        steDetails.ShoppingCenterOrNetlease = "Net Lease";
                    }
                    steDetails.Address = reader.IsDBNull(reader.GetOrdinal("property_address")) ? "" : reader.GetString(reader.GetOrdinal("property_address"));
                    steDetails.ZipCode = reader.IsDBNull(reader.GetOrdinal("property_zipcode")) ? "" : reader.GetString(reader.GetOrdinal("property_zipcode"));

                    steDetails.SelectedPropertyStatusId = reader.IsDBNull(reader.GetOrdinal("property_status_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("property_status_id"));
                    steDetails.SelectedPropertyStatus = reader.IsDBNull(reader.GetOrdinal("property_status")) ? "" : reader.GetString(reader.GetOrdinal("property_status"));

                    if (steDetails.Address.Length > 15)
                    {
                        steDetails.AddressShort = steDetails.Address.Substring(0, 15) + "..";
                    }
                    else
                    {
                        steDetails.AddressShort = steDetails.Address;
                    }


                    steDetails.TransactionStatusName = "";

                    steDetails.DiligenceDispositionList = GetDiligenceDispositions(steDetails.NetleasePropertyId);

                    steDetails.DiligenceAcquisitions = GetDiligenceAcquisition(steDetails.NetleasePropertyId);
                    steDetails.DiligenceDispositionList = GetDiligenceDispositions(steDetails.NetleasePropertyId);
                    steDetails.DiligenceLeaseList = GetDiligenceLease(steDetails.NetleasePropertyId);

                    steDetails.DispositionPeriodList = GetPeriodList(steDetails.NetleasePropertyId, "Disposition");
                    steDetails.LeasePeriodList = GetPeriodList(steDetails.NetleasePropertyId, "Lease");

                    steDetails.LeaseTypeList = GetLeaseTypeList();
                    steDetails.FutureTenantList = GetFutureTenantList(steDetails.NetleasePropertyId);

                    steDetails.DiligenceDispositions = new DiligenceDispositionsViewModel();
                    steDetails.SelectedDiligenceDisposition = new DiligenceDispositionsViewModel();

                    DateTime? transactionClosedDate = default(DateTime?);

                    steDetails.DiligenceLease = new DiligenceLeaseViewModel();
                    steDetails.SelectedDiligenceDisposition = new DiligenceDispositionsViewModel();
                    steDetails.DiligenceLeaseWithPurchase = new DiligenceLeaseWithPurchaseViewModel();
                    steDetails.SelectedDiligenceNetlease = new DiligenceNetleaseViewModel();

                    int saleLoi = 0, saleUnderContract = 0, saleTerminated = 0, saleClosed = 0;


                    if (steDetails.AssetTypeId == (int)SamAssetType.Fee || steDetails.AssetTypeId == (int)SamAssetType.FeeSubjectToLease)
                    {
                        steDetails.SelectedDiligenceDisposition = new DiligenceDispositionsViewModel();
                        foreach (DiligenceDispositionsViewModel ddm in steDetails.DiligenceDispositionList)
                        {
                            if (ddm.SelectedTransactionStatusId == (int)TransactionStatus.Closed_Acquisitions || ddm.SelectedTransactionStatusId == (int)TransactionStatus.Under_Contract || ddm.SelectedTransactionStatusId == (int)TransactionStatus.Under_LOI)
                            {
                                steDetails.CanAddTransactions = false;
                            }

                            if (steDetails.MaxPriorityTransactionStatusId == 0)
                            {
                                steDetails.MaxPriorityTransactionStatusId = ddm.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = ddm.SelectedTransactionStatusName;

                                steDetails.SelectedDiligenceDisposition = ddm;
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

                                steDetails.SelectedDiligenceDisposition = ddm;
                                transactionClosedDate = ddm.ClosingDate;

                            }

                            if (ddm.SelectedTransactionStatusId == (int)SamsTransactionStatus.Under_LOI &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Under_Contract) &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Closed_Dispositions))
                            {
                                steDetails.MaxPriorityTransactionStatusId = ddm.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = ddm.SelectedTransactionStatusName;
                                steDetails.StatusChangedDate = ddm.SelectedTransactionDate;

                                steDetails.SelectedDiligenceDisposition = ddm;
                                transactionClosedDate = ddm.ClosingDate;
                            }

                            if (ddm.SelectedTransactionStatusId == (int)SamsTransactionStatus.Under_Contract &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Closed_Dispositions))
                            {
                                steDetails.MaxPriorityTransactionStatusId = ddm.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = ddm.SelectedTransactionStatusName;
                                steDetails.StatusChangedDate = ddm.SelectedTransactionDate;

                                steDetails.SelectedDiligenceDisposition = ddm;
                                transactionClosedDate = ddm.ClosingDate;
                            }

                            if (ddm.SelectedTransactionStatusId == (int)SamsTransactionStatus.Closed_Dispositions)
                            {
                                steDetails.MaxPriorityTransactionStatusId = ddm.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = ddm.SelectedTransactionStatusName;
                                steDetails.StatusChangedDate = ddm.SelectedTransactionDate;
                                steDetails.SelectedDiligenceDisposition = ddm;
                                transactionClosedDate = ddm.ClosingDate;
                            }


                        }


                    }
                    else if (steDetails.AssetTypeId == (int)SamAssetType.Lease)
                    {

                        foreach (DiligenceLeaseViewModel dl in steDetails.DiligenceLeaseList)
                        {

                            if (steDetails.MaxPriorityTransactionStatusId == 0)
                            {
                                steDetails.MaxPriorityTransactionStatusId = dl.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = dl.SelectedTransactionStatusName;

                                steDetails.DiligenceLease = dl;
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

                                steDetails.DiligenceLease = dl;
                                transactionClosedDate = dl.ClosingDate;
                            }

                            if (dl.SelectedTransactionStatusId == (int)SamsTransactionStatus.Under_LOI &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Under_Contract) &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Closed_Dispositions))
                            {
                                steDetails.MaxPriorityTransactionStatusId = dl.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = dl.SelectedTransactionStatusName;
                                steDetails.StatusChangedDate = dl.SelectedTransactionDate;

                                steDetails.DiligenceLease = dl;
                                transactionClosedDate = dl.ClosingDate;
                            }

                            if (dl.SelectedTransactionStatusId == (int)SamsTransactionStatus.Under_Contract &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Closed_Dispositions))
                            {
                                steDetails.MaxPriorityTransactionStatusId = dl.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = dl.SelectedTransactionStatusName;
                                steDetails.StatusChangedDate = dl.SelectedTransactionDate;

                                steDetails.DiligenceLease = dl;
                                transactionClosedDate = dl.ClosingDate;
                            }

                            if (dl.SelectedTransactionStatusId == (int)SamsTransactionStatus.Closed_Dispositions)
                            {
                                steDetails.MaxPriorityTransactionStatusId = dl.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = dl.SelectedTransactionStatusName;
                                steDetails.StatusChangedDate = dl.SelectedTransactionDate;
                                transactionClosedDate = dl.ClosingDate;
                                steDetails.DiligenceLease = dl;
                            }
                        }


                    }
                    else if (steDetails.AssetTypeId == (int)SamAssetType.NetLease)
                    {
                        steDetails.DiligenceNetleaseList = GetDiligenceNetleaseList(steDetails.NetleasePropertyId);
                        steDetails.SelectedDiligenceNetlease = new DiligenceNetleaseViewModel();


                        foreach (DiligenceNetleaseViewModel dl in steDetails.DiligenceNetleaseList)
                        {
                            if (dl.SelectedTransactionStatusId == (int)TransactionStatus.Closed_Acquisitions || dl.SelectedTransactionStatusId == (int)TransactionStatus.Under_Contract || dl.SelectedTransactionStatusId == (int)TransactionStatus.Under_LOI)
                            {
                                steDetails.CanAddTransactions = false;
                            }



                            if (steDetails.MaxPriorityTransactionStatusId == 0)
                            {
                                steDetails.MaxPriorityTransactionStatusId = dl.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = dl.SelectedTransactionStatusName;

                                steDetails.SelectedDiligenceNetlease = dl;
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

                                steDetails.SelectedDiligenceNetlease = dl;
                                transactionClosedDate = dl.ClosingDate;
                            }

                            if (dl.SelectedTransactionStatusId == (int)SamsTransactionStatus.Under_LOI &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Under_Contract) &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Closed_Dispositions))
                            {
                                steDetails.MaxPriorityTransactionStatusId = dl.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = dl.SelectedTransactionStatusName;

                                steDetails.SelectedDiligenceNetlease = dl;
                                transactionClosedDate = dl.ClosingDate;
                            }

                            if (dl.SelectedTransactionStatusId == (int)SamsTransactionStatus.Under_Contract &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Closed_Dispositions))
                            {
                                steDetails.MaxPriorityTransactionStatusId = dl.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = dl.SelectedTransactionStatusName;

                                steDetails.SelectedDiligenceNetlease = dl;
                                transactionClosedDate = dl.ClosingDate;
                            }

                            if (dl.SelectedTransactionStatusId == (int)SamsTransactionStatus.Closed_Dispositions)
                            {
                                steDetails.MaxPriorityTransactionStatusId = dl.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = dl.SelectedTransactionStatusName;
                                transactionClosedDate = dl.ClosingDate;
                                steDetails.SelectedDiligenceNetlease = dl;
                            }
                        }



                    }
                    else if (steDetails.AssetTypeId == (int)SamAssetType.LeaseWithPurchaseOption)
                    {
                        steDetails.DiligenceLeaseWithPurchaseList = GetDiligenceLeaseWithPurchaseList(steDetails.NetleasePropertyId);
                        steDetails.DiligenceLeaseWithPurchase = new DiligenceLeaseWithPurchaseViewModel();

                        int leaseLoi = 0, leaseUnderContract = 0, leaseTerminated = 0, leaseClosed = 0;
                        foreach (DiligenceLeaseWithPurchaseViewModel dl in steDetails.DiligenceLeaseWithPurchaseList)
                        {

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
                                transactionClosedDate = dl.ClosingDate;
                                steDetails.DiligenceLeaseWithPurchase = dl;
                            }
                        }


                    }
                    else if (steDetails.AssetTypeId == (int)SamAssetType.SaleLeaseBack)
                    {
                        steDetails.DiligenceDispositions_SaleLeaseBack = GetDiligenceDispositions_SaleLeaseBack(steDetails.NetleasePropertyId);



                        foreach (DiligenceDispositionsViewModel ddm in steDetails.DiligenceDispositions_SaleLeaseBack)
                        {
                            if (ddm.SelectedTransactionStatusId == (int)TransactionStatus.Closed_Acquisitions || ddm.SelectedTransactionStatusId == (int)TransactionStatus.Under_Contract || ddm.SelectedTransactionStatusId == (int)TransactionStatus.Under_LOI)
                            {
                                steDetails.CanAddTransactions = false;
                            }

                            steDetails.SelectedDiligenceDisposition = ddm;
                            transactionClosedDate = ddm.ClosingDate;

                            if (steDetails.MaxPriorityTransactionStatusId == 0)
                            {
                                steDetails.MaxPriorityTransactionStatusId = ddm.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = ddm.SelectedTransactionStatusName;


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

                                steDetails.SelectedDiligenceDisposition = ddm;
                                transactionClosedDate = ddm.ClosingDate;
                            }

                            if (ddm.SelectedTransactionStatusId == (int)SamsTransactionStatus.Under_LOI &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Under_Contract) &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Closed_Dispositions))
                            {
                                steDetails.MaxPriorityTransactionStatusId = ddm.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = ddm.SelectedTransactionStatusName;
                                steDetails.StatusChangedDate = ddm.SelectedTransactionDate;

                                steDetails.SelectedDiligenceDisposition = ddm;
                                transactionClosedDate = ddm.ClosingDate;
                            }

                            if (ddm.SelectedTransactionStatusId == (int)SamsTransactionStatus.Under_Contract &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Closed_Dispositions))
                            {
                                steDetails.MaxPriorityTransactionStatusId = ddm.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = ddm.SelectedTransactionStatusName;
                                steDetails.StatusChangedDate = ddm.SelectedTransactionDate;

                                steDetails.SelectedDiligenceDisposition = ddm;
                                transactionClosedDate = ddm.ClosingDate;
                            }

                            if (ddm.SelectedTransactionStatusId == (int)SamsTransactionStatus.Closed_Dispositions)
                            {
                                steDetails.MaxPriorityTransactionStatusId = ddm.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = ddm.SelectedTransactionStatusName;
                                steDetails.StatusChangedDate = ddm.SelectedTransactionDate;
                                transactionClosedDate = ddm.ClosingDate;
                                steDetails.SelectedDiligenceDisposition = ddm;
                            }
                        }



                    }

                    steDetails.TodoList = GetTodoList(steDetails.NetleasePropertyId);
                    StringBuilder todoText = new StringBuilder();
                    if (steDetails.TodoList.Count > 0)
                    {
                        foreach (TodoViewModel td in steDetails.TodoList)
                        {
                            todoText.Append(td.TodoText + "\r\n");
                        }
                    }
                    steDetails.LatestComment = todoText.ToString();

                    if (steDetails.MaxPriorityTransactionStatusId == (int)SamsTransactionStatus.Closed_Dispositions)
                    {
                        netLeasePropertiesList.Add(steDetails);
                    }

                }
                con.Close();
            }

            return View(netLeasePropertiesList);
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

            List<NetleasePropertiesViewModel> netLeasePropertiesList = new List<NetleasePropertiesViewModel>();

            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("GetNetleaseDispositionPropertyList", con);
                cmd.Parameters.AddWithValue("asset_status", 0);
                cmd.Parameters.AddWithValue("disposition_status", dispositionStatus); 

                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var steDetails = new NetleasePropertiesViewModel();
                    steDetails.NetleasePropertyId = reader.IsDBNull(reader.GetOrdinal("net_lease_property_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("net_lease_property_id"));
                    steDetails.AssetId = reader.IsDBNull(reader.GetOrdinal("asset_id")) ? "" : reader.GetString(reader.GetOrdinal("asset_id"));
                    steDetails.AssetName = reader.IsDBNull(reader.GetOrdinal("asset_name")) ? "" : reader.GetString(reader.GetOrdinal("asset_name"));
                    steDetails.StateId = reader.IsDBNull(reader.GetOrdinal("state_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("state_id"));
                    steDetails.City = reader.IsDBNull(reader.GetOrdinal("city")) ? "" : reader.GetString(reader.GetOrdinal("city"));

                    steDetails.PropertyPrice = reader.IsDBNull(reader.GetOrdinal("property_price")) ? "" : reader.GetString(reader.GetOrdinal("property_price"));
                    steDetails.CapRate = reader.IsDBNull(reader.GetOrdinal("cap_rate")) ? 0 : reader.GetDouble(reader.GetOrdinal("cap_rate"));

                    steDetails.Term = reader.IsDBNull(reader.GetOrdinal("term")) ? "" : reader.GetString(reader.GetOrdinal("term"));

                    steDetails.PdfFileName = reader.IsDBNull(reader.GetOrdinal("detail_pdf")) ? "" : reader.GetString(reader.GetOrdinal("detail_pdf"));
                    steDetails.StateName = reader.IsDBNull(reader.GetOrdinal("state_name")) ? "" : reader.GetString(reader.GetOrdinal("state_name"));

                    steDetails.CreatedDate = reader.IsDBNull(reader.GetOrdinal("created_date")) ? DateTime.Now : reader.GetDateTime(reader.GetOrdinal("created_date"));
                    steDetails.AssetTypeId = reader.IsDBNull(reader.GetOrdinal("asset_type_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("asset_type_id"));
                    steDetails.AssetTypeName = reader.IsDBNull(reader.GetOrdinal("asset_type_name")) ? "" : reader.GetString(reader.GetOrdinal("asset_type_name"));
                    steDetails.IsShoppingCenter = reader.IsDBNull(reader.GetOrdinal("is_shopping_center")) ? false : reader.GetBoolean(reader.GetOrdinal("is_shopping_center"));
                    if (steDetails.IsShoppingCenter)
                    {
                        steDetails.ShoppingCenterOrNetlease = "Shopping Center";
                    }
                    else
                    {
                        steDetails.ShoppingCenterOrNetlease = "Net Lease";
                    }

                    steDetails.Address = reader.IsDBNull(reader.GetOrdinal("property_address")) ? "" : reader.GetString(reader.GetOrdinal("property_address"));
                    steDetails.ZipCode = reader.IsDBNull(reader.GetOrdinal("property_zipcode")) ? "" : reader.GetString(reader.GetOrdinal("property_zipcode"));

                    if (steDetails.Address.Length > 15)
                    {
                        steDetails.AddressShort = steDetails.Address.Substring(0, 15) + "..";
                    }
                    else
                    {
                        steDetails.AddressShort = steDetails.Address;
                    }

                    netLeasePropertiesList.Add(steDetails);
                }
                con.Close();
            }

            return View(netLeasePropertiesList);
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

            List<NetleasePropertiesViewModel> netLeasePropertiesList = new List<NetleasePropertiesViewModel>();

            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("GetNetleaseAcquisitionPropertyList", con);
                cmd.Parameters.AddWithValue("asset_status", 0);
                cmd.Parameters.AddWithValue("acquisition_status", acquisitionStatus);

                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var steDetails = new NetleasePropertiesViewModel();
                    steDetails.NetleasePropertyId = reader.IsDBNull(reader.GetOrdinal("net_lease_property_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("net_lease_property_id"));
                    steDetails.AssetId = reader.IsDBNull(reader.GetOrdinal("asset_id")) ? "" : reader.GetString(reader.GetOrdinal("asset_id"));
                    steDetails.AssetName = reader.IsDBNull(reader.GetOrdinal("asset_name")) ? "" : reader.GetString(reader.GetOrdinal("asset_name"));
                    steDetails.StateId = reader.IsDBNull(reader.GetOrdinal("state_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("state_id"));
                    steDetails.City = reader.IsDBNull(reader.GetOrdinal("city")) ? "" : reader.GetString(reader.GetOrdinal("city"));

                    steDetails.PropertyPrice = reader.IsDBNull(reader.GetOrdinal("property_price")) ? "" : reader.GetString(reader.GetOrdinal("property_price"));
                    steDetails.CapRate = reader.IsDBNull(reader.GetOrdinal("cap_rate")) ? 0 : reader.GetDouble(reader.GetOrdinal("cap_rate"));

                    steDetails.Term = reader.IsDBNull(reader.GetOrdinal("term")) ? "" : reader.GetString(reader.GetOrdinal("term"));

                    steDetails.PdfFileName = reader.IsDBNull(reader.GetOrdinal("detail_pdf")) ? "" : reader.GetString(reader.GetOrdinal("detail_pdf"));
                    steDetails.StateName = reader.IsDBNull(reader.GetOrdinal("state_name")) ? "" : reader.GetString(reader.GetOrdinal("state_name"));

                    steDetails.CreatedDate = reader.IsDBNull(reader.GetOrdinal("created_date")) ? DateTime.Now : reader.GetDateTime(reader.GetOrdinal("created_date"));
                    steDetails.AssetTypeId = reader.IsDBNull(reader.GetOrdinal("asset_type_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("asset_type_id"));
                    steDetails.AssetTypeName = reader.IsDBNull(reader.GetOrdinal("asset_type_name")) ? "" : reader.GetString(reader.GetOrdinal("asset_type_name"));
                    steDetails.IsShoppingCenter = reader.IsDBNull(reader.GetOrdinal("is_shopping_center")) ? false : reader.GetBoolean(reader.GetOrdinal("is_shopping_center"));
                    if (steDetails.IsShoppingCenter)
                    {
                        steDetails.ShoppingCenterOrNetlease = "Shopping Center";
                    }
                    else
                    {
                        steDetails.ShoppingCenterOrNetlease = "Net Lease";
                    }

                    steDetails.Address = reader.IsDBNull(reader.GetOrdinal("property_address")) ? "" : reader.GetString(reader.GetOrdinal("property_address"));
                    steDetails.ZipCode = reader.IsDBNull(reader.GetOrdinal("property_zipcode")) ? "" : reader.GetString(reader.GetOrdinal("property_zipcode"));

                    if (steDetails.Address.Length > 15)
                    {
                        steDetails.AddressShort = steDetails.Address.Substring(0, 15) + "..";
                    }
                    else
                    {
                        steDetails.AddressShort = steDetails.Address;
                    }

                    netLeasePropertiesList.Add(steDetails);
                }
                con.Close();
            }

            return View(netLeasePropertiesList);
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

            List<NetleasePropertiesViewModel> netLeasePropertiesList = new List<NetleasePropertiesViewModel>();

            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("GetNetleasePropertyList", con);
                cmd.Parameters.AddWithValue("asset_status", 1);

                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var steDetails = new NetleasePropertiesViewModel();
                    steDetails.NetleasePropertyId = reader.IsDBNull(reader.GetOrdinal("net_lease_property_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("net_lease_property_id"));
                    steDetails.AssetId = reader.IsDBNull(reader.GetOrdinal("asset_id")) ? "" : reader.GetString(reader.GetOrdinal("asset_id"));
                    steDetails.AssetName = reader.IsDBNull(reader.GetOrdinal("asset_name")) ? "" : reader.GetString(reader.GetOrdinal("asset_name"));
                    steDetails.StateId = reader.IsDBNull(reader.GetOrdinal("state_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("state_id"));
                    steDetails.City = reader.IsDBNull(reader.GetOrdinal("city")) ? "" : reader.GetString(reader.GetOrdinal("city"));

                    steDetails.PropertyPrice = reader.IsDBNull(reader.GetOrdinal("property_price")) ? "" : reader.GetString(reader.GetOrdinal("property_price"));
                    steDetails.CapRate = reader.IsDBNull(reader.GetOrdinal("cap_rate")) ? 0 : reader.GetDouble(reader.GetOrdinal("cap_rate"));

                    steDetails.Term = reader.IsDBNull(reader.GetOrdinal("term")) ? "" : reader.GetString(reader.GetOrdinal("term"));

                    steDetails.PdfFileName = reader.IsDBNull(reader.GetOrdinal("detail_pdf")) ? "" : reader.GetString(reader.GetOrdinal("detail_pdf"));
                    steDetails.StateName = reader.IsDBNull(reader.GetOrdinal("state_name")) ? "" : reader.GetString(reader.GetOrdinal("state_name"));

                    steDetails.CreatedDate = reader.IsDBNull(reader.GetOrdinal("created_date")) ? DateTime.Now : reader.GetDateTime(reader.GetOrdinal("created_date"));
                    steDetails.AssetTypeId = reader.IsDBNull(reader.GetOrdinal("asset_type_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("asset_type_id"));
                    steDetails.AssetTypeName = reader.IsDBNull(reader.GetOrdinal("asset_type_name")) ? "" : reader.GetString(reader.GetOrdinal("asset_type_name"));
                    steDetails.IsShoppingCenter = reader.IsDBNull(reader.GetOrdinal("is_shopping_center")) ? false : reader.GetBoolean(reader.GetOrdinal("is_shopping_center"));
                    if (steDetails.IsShoppingCenter)
                    {
                        steDetails.ShoppingCenterOrNetlease = "Shopping Center";
                    }
                    else
                    {
                        steDetails.ShoppingCenterOrNetlease = "Net Lease";
                    }

                    steDetails.Address = reader.IsDBNull(reader.GetOrdinal("property_address")) ? "" : reader.GetString(reader.GetOrdinal("property_address"));
                    steDetails.ZipCode = reader.IsDBNull(reader.GetOrdinal("property_zipcode")) ? "" : reader.GetString(reader.GetOrdinal("property_zipcode"));

                    if (steDetails.Address.Length > 15)
                    {
                        steDetails.AddressShort = steDetails.Address.Substring(0, 15) + "..";
                    }
                    else
                    {
                        steDetails.AddressShort = steDetails.Address;
                    }

                    netLeasePropertiesList.Add(steDetails);
                }
                con.Close();
            }

            return View(netLeasePropertiesList);
        }
        

        public IActionResult AddNetLeaseProperties()
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

            var steDetails = new NetleasePropertiesViewModel();

            List<StateDetails> stateList = new List<StateDetails>();
            

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

                
            }

            steDetails.StateList = stateList;
            steDetails.AssetTypeList = GetAssetTypeList(2);

            return View(steDetails);
        }

        [HttpPost]
        public IActionResult SaveNetLeaseProperties(NetleasePropertiesViewModel netleaseProperties)
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

            int siteId = netleaseProperties.NetleasePropertyId;
            string CS = DBConnection.ConnectionString;

            if(netleaseProperties.UploadedPdf != null)
            {
                var uniqueFileName = Helper.GetUniqueFileName(netleaseProperties.UploadedPdf.FileName);
                var filePath = Path.Combine(webHostEnvironment.WebRootPath + @"/UploadedPdf", uniqueFileName);

                using (var stream = System.IO.File.Create(filePath))
                {
                    netleaseProperties.UploadedPdf.CopyTo(stream);
                }
                netleaseProperties.PdfFileName = uniqueFileName;
            }
            
            
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("SaveNetleaseProperty", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                cmd.Parameters.AddWithValue("net_lease_property_id", netleaseProperties.NetleasePropertyId); 
                cmd.Parameters.AddWithValue("asset_id", netleaseProperties.AssetId);
                cmd.Parameters.AddWithValue("asset_name", netleaseProperties.AssetName);
                cmd.Parameters.AddWithValue("netlease_asset_name", netleaseProperties.NetleaseAssetName);
                cmd.Parameters.AddWithValue("state_id", netleaseProperties.StateId);
                cmd.Parameters.AddWithValue("city", netleaseProperties.City);

                cmd.Parameters.AddWithValue("cap_rate", netleaseProperties.CapRate);
                cmd.Parameters.AddWithValue("property_price", netleaseProperties.PropertyPrice);
                cmd.Parameters.AddWithValue("term", netleaseProperties.Term);
                cmd.Parameters.AddWithValue("detail_pdf", netleaseProperties.PdfFileName);
                
                cmd.Parameters.AddWithValue("asset_status", netleaseProperties.AssetStatus);
                cmd.Parameters.AddWithValue("is_shopping_center", netleaseProperties.IsShoppingCenter);
                if (netleaseProperties.IsShoppingCenter)
                {
                    cmd.Parameters.AddWithValue("asset_type_id", netleaseProperties.AssetTypeId_ShoppingCenter);
                }
                else
                {
                    cmd.Parameters.AddWithValue("asset_type_id", netleaseProperties.AssetTypeId);
                }
                cmd.Parameters.AddWithValue("property_address", netleaseProperties.Address);
                cmd.Parameters.AddWithValue("property_zipcode", netleaseProperties.ZipCode);

                cmd.Parameters.AddWithValue("property_latitude", netleaseProperties.Latitude);
                cmd.Parameters.AddWithValue("property_longitude", netleaseProperties.Longitude);
                cmd.Parameters.AddWithValue("property_status_id", netleaseProperties.SelectedPropertyStatusId);

                cmd.Parameters.AddWithValue("check_if_property_listed", netleaseProperties.CheckIfPropertyListed);
                cmd.Parameters.AddWithValue("listing_agent_name", netleaseProperties.ListingAgentName);

                if(netleaseProperties.ListingExpiry.Year > 1)
                {
                    cmd.Parameters.AddWithValue("listing_expiry", netleaseProperties.ListingExpiry);
                }
                

                cmd.Parameters.AddWithValue("listing_price", netleaseProperties.ListingPrice);
                cmd.Parameters.AddWithValue("asking_rent", netleaseProperties.AskingRent);
                cmd.Parameters.AddWithValue("lease_type", netleaseProperties.LeaseType);
                cmd.Parameters.AddWithValue("details", netleaseProperties.Details);
                cmd.Parameters.AddWithValue("status_changed_date", netleaseProperties.StatusChangedDate);

                cmd.Parameters.AddWithValue("term_remaining", netleaseProperties.TermRemaining);
                cmd.Parameters.AddWithValue("rental_income", netleaseProperties.RentalIncome);
                cmd.Parameters.AddWithValue("lease_type_net_lease", netleaseProperties.LeaseTypeLeaseAndFee);

                cmd.Parameters.AddWithValue("term_option_purchase", netleaseProperties.TermOptionPurchase);
                cmd.Parameters.AddWithValue("asking_rent_option_purchase", netleaseProperties.AskingRentOptionPurchase);
                cmd.Parameters.AddWithValue("lease_type_purchase", netleaseProperties.LeaseTypePurchase);
                cmd.Parameters.AddWithValue("option_purchase_price", netleaseProperties.OptionPurchasePrice);
                cmd.Parameters.AddWithValue("potential_use", netleaseProperties.PotentialUse);
                cmd.Parameters.AddWithValue("region_id", netleaseProperties.RegionId);
                cmd.Parameters.AddWithValue("property_header_line_2", netleaseProperties.PropertyHeaderLine2);

                netleaseProperties.NetleasePropertyId = int.Parse(cmd.ExecuteScalar().ToString());

                con.Close();
            }

            return RedirectToAction("ViewNetLeaseProperties", new { propertyId = netleaseProperties.NetleasePropertyId });
        }

        public IActionResult EditNetLeaseProperty(int propertyId)
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

            var steDetails = new NetleasePropertiesViewModel();

            List<StateDetails> stateList = new List<StateDetails>();
            List<PropertyStatusModel> propertyStatusList = new List<PropertyStatusModel>();
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
                SqlCommand cmd = new SqlCommand("GetNetleasePropertyById", con);

                cmd.Parameters.AddWithValue("net_lease_property_id", propertyId);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    steDetails.NetleasePropertyId = reader.IsDBNull(reader.GetOrdinal("net_lease_property_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("net_lease_property_id"));
                    steDetails.AssetId = reader.IsDBNull(reader.GetOrdinal("asset_id")) ? "" : reader.GetString(reader.GetOrdinal("asset_id"));
                    steDetails.AssetName = reader.IsDBNull(reader.GetOrdinal("asset_name")) ? "" : reader.GetString(reader.GetOrdinal("asset_name"));
                    steDetails.NetleaseAssetName = reader.IsDBNull(reader.GetOrdinal("netlease_asset_name")) ? "" : reader.GetString(reader.GetOrdinal("netlease_asset_name"));
                    steDetails.StateId = reader.IsDBNull(reader.GetOrdinal("state_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("state_id"));
                    steDetails.City = reader.IsDBNull(reader.GetOrdinal("city")) ? "" : reader.GetString(reader.GetOrdinal("city"));
                    steDetails.PropertyPrice = reader.IsDBNull(reader.GetOrdinal("property_price")) ? "" : reader.GetString(reader.GetOrdinal("property_price"));
                    steDetails.CapRate = reader.IsDBNull(reader.GetOrdinal("cap_rate")) ? 0 : reader.GetDouble(reader.GetOrdinal("cap_rate"));

                    steDetails.Term = reader.IsDBNull(reader.GetOrdinal("term")) ? "" : reader.GetString(reader.GetOrdinal("term"));

                    steDetails.PdfFileName = reader.IsDBNull(reader.GetOrdinal("detail_pdf")) ? "" : reader.GetString(reader.GetOrdinal("detail_pdf"));
                    steDetails.StateName = reader.IsDBNull(reader.GetOrdinal("state_name")) ? "" : reader.GetString(reader.GetOrdinal("state_name"));

                    steDetails.CreatedDate = reader.IsDBNull(reader.GetOrdinal("created_date")) ? DateTime.Now : reader.GetDateTime(reader.GetOrdinal("created_date"));
                    steDetails.AssetTypeId = reader.IsDBNull(reader.GetOrdinal("asset_type_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("asset_type_id"));
                    steDetails.AssetTypeName = reader.IsDBNull(reader.GetOrdinal("asset_type_name")) ? "" : reader.GetString(reader.GetOrdinal("asset_type_name"));

                    steDetails.IsShoppingCenter = reader.IsDBNull(reader.GetOrdinal("is_shopping_center")) ? false : reader.GetBoolean(reader.GetOrdinal("is_shopping_center"));
                    if (steDetails.IsShoppingCenter)
                    {
                        steDetails.ShoppingCenterOrNetlease = "Shopping Center";
                    }
                    else
                    {
                        steDetails.ShoppingCenterOrNetlease = "Net Lease";
                    }

                    steDetails.Address = reader.IsDBNull(reader.GetOrdinal("property_address")) ? "" : reader.GetString(reader.GetOrdinal("property_address"));
                    steDetails.ZipCode = reader.IsDBNull(reader.GetOrdinal("property_zipcode")) ? "" : reader.GetString(reader.GetOrdinal("property_zipcode"));

                    steDetails.Latitude = reader.IsDBNull(reader.GetOrdinal("property_latitude")) ? "" : reader.GetString(reader.GetOrdinal("property_latitude"));
                    steDetails.Longitude = reader.IsDBNull(reader.GetOrdinal("property_longitude")) ? "" : reader.GetString(reader.GetOrdinal("property_longitude"));

                    steDetails.SelectedPropertyStatusId = reader.IsDBNull(reader.GetOrdinal("property_status_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("property_status_id"));
                    steDetails.SelectedPropertyStatus = reader.IsDBNull(reader.GetOrdinal("property_status")) ? "" : reader.GetString(reader.GetOrdinal("property_status"));

                    steDetails.CheckIfPropertyListed = reader.IsDBNull(reader.GetOrdinal("check_if_property_listed")) ? 0 : reader.GetInt32(reader.GetOrdinal("check_if_property_listed"));
                    steDetails.ListingAgentName = reader.IsDBNull(reader.GetOrdinal("listing_agent_name")) ? "" : reader.GetString(reader.GetOrdinal("listing_agent_name"));
                    steDetails.ListingExpiry = reader.IsDBNull(reader.GetOrdinal("listing_expiry")) ? DateTime.Now : reader.GetDateTime(reader.GetOrdinal("listing_expiry"));

                    steDetails.ListingPrice = reader.IsDBNull(reader.GetOrdinal("listing_price")) ? "" : reader.GetString(reader.GetOrdinal("listing_price"));
                    steDetails.AskingRent = reader.IsDBNull(reader.GetOrdinal("asking_rent")) ? "" : reader.GetString(reader.GetOrdinal("asking_rent"));
                    steDetails.LeaseType = reader.IsDBNull(reader.GetOrdinal("lease_type")) ? 0 : reader.GetInt32(reader.GetOrdinal("lease_type"));
                    steDetails.Details = reader.IsDBNull(reader.GetOrdinal("details")) ? "" : reader.GetString(reader.GetOrdinal("details"));

                    steDetails.SavedShoppingMartPlanFileName = reader.IsDBNull(reader.GetOrdinal("shopping_mart_plan_file_name")) ? "" : "OtherFiles/" + reader.GetString(reader.GetOrdinal("shopping_mart_plan_file_name"));

                    steDetails.StatusChangedDate = reader.IsDBNull(reader.GetOrdinal("status_changed_date")) ? DateTime.Now : reader.GetDateTime(reader.GetOrdinal("status_changed_date"));
                    steDetails.IsClosed = reader.IsDBNull(reader.GetOrdinal("is_closed")) ? 0 : reader.GetInt32(reader.GetOrdinal("is_closed"));

                    steDetails.TermRemaining = reader.IsDBNull(reader.GetOrdinal("term_remaining")) ? "" : reader.GetString(reader.GetOrdinal("term_remaining"));
                    steDetails.RentalIncome = reader.IsDBNull(reader.GetOrdinal("rental_income")) ? "" : reader.GetString(reader.GetOrdinal("rental_income"));
                    steDetails.LeaseTypeLeaseAndFee = reader.IsDBNull(reader.GetOrdinal("lease_type_net_lease")) ? 0 : reader.GetInt32(reader.GetOrdinal("lease_type_net_lease"));

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
                steDetails.propertyStatusList = propertyStatusList;
                /*
                if (steDetails.IsShoppingCenter)
                {
                    steDetails.AssetTypeListShoppingCenter = GetAssetTypeList(0);
                }
                else
                {
                    steDetails.AssetTypeList = GetAssetTypeList(2);
                }
                */
                steDetails.AssetTypeListShoppingCenter = GetAssetTypeList(4);
                steDetails.AssetTypeList = GetAssetTypeList(5);

                steDetails.LeaseTypeList = leaseTypeList;

                steDetails.ShoppingCenterClientList = GetShoppingCenterClientList(steDetails.NetleasePropertyId);
                
                steDetails.RegionList = GetRegionList(steDetails.NetleasePropertyId);

                return View(steDetails);
            }
        }

        List<ShoppingCenterClients> GetShoppingCenterClientList(int cStoreId)
        {
            

            List<ShoppingCenterClients> shoppingCenterClientList = new List<ShoppingCenterClients>();
            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmdShoppingCenterClient = new SqlCommand("GetShoppingCenterClientList", con);
                cmdShoppingCenterClient.Parameters.AddWithValue("c_store_id", cStoreId);

                cmdShoppingCenterClient.CommandType = CommandType.StoredProcedure;
                con.Open();

                SqlDataReader readerShoppingCenterClient = cmdShoppingCenterClient.ExecuteReader();
                while (readerShoppingCenterClient.Read())
                {
                    var shoppingCenterClient = new ShoppingCenterClients();
                    shoppingCenterClient.ShoppingCenterClientId = readerShoppingCenterClient.IsDBNull(readerShoppingCenterClient.GetOrdinal("shopping_center_client_id")) ? 0 : readerShoppingCenterClient.GetInt32(readerShoppingCenterClient.GetOrdinal("shopping_center_client_id"));
                    shoppingCenterClient.CStoreId = readerShoppingCenterClient.IsDBNull(readerShoppingCenterClient.GetOrdinal("c_store_id")) ? 0 : readerShoppingCenterClient.GetInt32(readerShoppingCenterClient.GetOrdinal("c_store_id"));
                    shoppingCenterClient.TenantName = readerShoppingCenterClient.IsDBNull(readerShoppingCenterClient.GetOrdinal("tenant_name")) ? "" : readerShoppingCenterClient.GetString(readerShoppingCenterClient.GetOrdinal("tenant_name"));

                    shoppingCenterClient.UnitSelected = readerShoppingCenterClient.IsDBNull(readerShoppingCenterClient.GetOrdinal("unit_selected")) ? "" : readerShoppingCenterClient.GetString(readerShoppingCenterClient.GetOrdinal("unit_selected"));
                    shoppingCenterClient.AnnualRent = readerShoppingCenterClient.IsDBNull(readerShoppingCenterClient.GetOrdinal("annual_rent")) ? "" : readerShoppingCenterClient.GetString(readerShoppingCenterClient.GetOrdinal("annual_rent"));
                    shoppingCenterClient.MonthlyRent = readerShoppingCenterClient.IsDBNull(readerShoppingCenterClient.GetOrdinal("monthly_rent")) ? "" : readerShoppingCenterClient.GetString(readerShoppingCenterClient.GetOrdinal("monthly_rent"));
                    shoppingCenterClient.CamMonthly = readerShoppingCenterClient.IsDBNull(readerShoppingCenterClient.GetOrdinal("cam_monthly")) ? "" : readerShoppingCenterClient.GetString(readerShoppingCenterClient.GetOrdinal("cam_monthly"));
                    shoppingCenterClient.CamYearly = readerShoppingCenterClient.IsDBNull(readerShoppingCenterClient.GetOrdinal("cam_yearly")) ? "" : readerShoppingCenterClient.GetString(readerShoppingCenterClient.GetOrdinal("cam_yearly"));
                    shoppingCenterClient.SetOrAdjustAutomatically = readerShoppingCenterClient.IsDBNull(readerShoppingCenterClient.GetOrdinal("set_or_adjust_automatically")) ? "" : readerShoppingCenterClient.GetString(readerShoppingCenterClient.GetOrdinal("set_or_adjust_automatically"));
                    shoppingCenterClient.RentAndCamMonthly = readerShoppingCenterClient.IsDBNull(readerShoppingCenterClient.GetOrdinal("rent_and_cam_monthly")) ? "" : readerShoppingCenterClient.GetString(readerShoppingCenterClient.GetOrdinal("rent_and_cam_monthly"));
                    shoppingCenterClient.RentAndCamYearly = readerShoppingCenterClient.IsDBNull(readerShoppingCenterClient.GetOrdinal("rent_and_cam_yearly")) ? "" : readerShoppingCenterClient.GetString(readerShoppingCenterClient.GetOrdinal("rent_and_cam_yearly"));
                    shoppingCenterClient.PiecePerSquareFoot = readerShoppingCenterClient.IsDBNull(readerShoppingCenterClient.GetOrdinal("piece_per_square_foot")) ? "" : readerShoppingCenterClient.GetString(readerShoppingCenterClient.GetOrdinal("piece_per_square_foot"));
                    shoppingCenterClient.LeaseExpaires = readerShoppingCenterClient.IsDBNull(readerShoppingCenterClient.GetOrdinal("lease_expires")) ? "" : readerShoppingCenterClient.GetString(readerShoppingCenterClient.GetOrdinal("lease_expires"));
                    
                    shoppingCenterClient.DateRentChanges = readerShoppingCenterClient.IsDBNull(readerShoppingCenterClient.GetOrdinal("date_rent_changed")) ? default(DateTime?) : readerShoppingCenterClient.GetDateTime(readerShoppingCenterClient.GetOrdinal("date_rent_changed"));

                    shoppingCenterClient.AnnualRentChangeTo = readerShoppingCenterClient.IsDBNull(readerShoppingCenterClient.GetOrdinal("annual_rent_changed_to")) ? "" : readerShoppingCenterClient.GetString(readerShoppingCenterClient.GetOrdinal("annual_rent_changed_to"));

                    shoppingCenterClient.RentPerMonthChangeTo = readerShoppingCenterClient.IsDBNull(readerShoppingCenterClient.GetOrdinal("rent_per_month_changed_to")) ? "" : readerShoppingCenterClient.GetString(readerShoppingCenterClient.GetOrdinal("rent_per_month_changed_to"));
                    shoppingCenterClient.RentAndCamChangeTo = readerShoppingCenterClient.IsDBNull(readerShoppingCenterClient.GetOrdinal("rent_and_cam_changed_to")) ? "" : readerShoppingCenterClient.GetString(readerShoppingCenterClient.GetOrdinal("rent_and_cam_changed_to"));
                    shoppingCenterClient.PiecePerSquareFootChangeTo = readerShoppingCenterClient.IsDBNull(readerShoppingCenterClient.GetOrdinal("piece_per_square_foot_changed_to")) ? "" : readerShoppingCenterClient.GetString(readerShoppingCenterClient.GetOrdinal("piece_per_square_foot_changed_to"));
                    shoppingCenterClient.SubspaceSquareFootage = readerShoppingCenterClient.IsDBNull(readerShoppingCenterClient.GetOrdinal("subspace_square_footage")) ? "" : readerShoppingCenterClient.GetString(readerShoppingCenterClient.GetOrdinal("subspace_square_footage"));
                    shoppingCenterClient.Notes = readerShoppingCenterClient.IsDBNull(readerShoppingCenterClient.GetOrdinal("notes")) ? "" : readerShoppingCenterClient.GetString(readerShoppingCenterClient.GetOrdinal("notes"));

                    shoppingCenterClientList.Add(shoppingCenterClient);
                }
                con.Close();
            }

            return shoppingCenterClientList;
        }



        public IActionResult ViewNetLeaseProperties(int propertyId)
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

            var steDetails = new NetleasePropertiesViewModel();

            List<StateDetails> stateList = new List<StateDetails>();
            List<AdditionalFilesViewModel> additionalFiles = new List<AdditionalFilesViewModel>();

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



                SqlCommand cmdComplianceList = new SqlCommand("GetNetLeaseFiles", con);

                cmdComplianceList.Parameters.AddWithValue("property_id", propertyId);
                cmdComplianceList.CommandType = CommandType.StoredProcedure;
                con.Open();

                SqlDataReader readerComplianceList = cmdComplianceList.ExecuteReader();
                
                while (readerComplianceList.Read())
                {
                    var c_storeFile = new AdditionalFilesViewModel();
                    c_storeFile.FileId = readerComplianceList.IsDBNull(readerComplianceList.GetOrdinal("file_id")) ? 0 : readerComplianceList.GetInt32(readerComplianceList.GetOrdinal("file_id"));
                    c_storeFile.PropertyId = propertyId;
                    c_storeFile.FileType = readerComplianceList.IsDBNull(readerComplianceList.GetOrdinal("file_type")) ? "" : readerComplianceList.GetString(readerComplianceList.GetOrdinal("file_type"));


                    c_storeFile.FileName = readerComplianceList.IsDBNull(readerComplianceList.GetOrdinal("file_name")) ? "" : readerComplianceList.GetString(readerComplianceList.GetOrdinal("file_name"));

                    c_storeFile.FileNameWithoutPath = c_storeFile.FileName.Length < 35 ? c_storeFile.FileName : c_storeFile.FileName.Substring(0, 34) + "...";

                    string pic = @"../../property_files/" + c_storeFile.FileName;
                    c_storeFile.FileName = pic;
                    additionalFiles.Add(c_storeFile);
                }
                con.Close();
            }

            List<AdditionalFilesViewModel> confidentialFiles = new List<AdditionalFilesViewModel>();
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("GetNetleasePropertyById", con);

                cmd.Parameters.AddWithValue("net_lease_property_id", propertyId);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    steDetails.NetleasePropertyId = reader.IsDBNull(reader.GetOrdinal("net_lease_property_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("net_lease_property_id"));
                    steDetails.AssetId = reader.IsDBNull(reader.GetOrdinal("asset_id")) ? "" : reader.GetString(reader.GetOrdinal("asset_id"));
                    steDetails.AssetName = reader.IsDBNull(reader.GetOrdinal("asset_name")) ? "" : reader.GetString(reader.GetOrdinal("asset_name"));
                    steDetails.NetleaseAssetName = reader.IsDBNull(reader.GetOrdinal("netlease_asset_name")) ? "" : reader.GetString(reader.GetOrdinal("netlease_asset_name"));
                    steDetails.StateId = reader.IsDBNull(reader.GetOrdinal("state_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("state_id"));
                    steDetails.City = reader.IsDBNull(reader.GetOrdinal("city")) ? "" : reader.GetString(reader.GetOrdinal("city"));

                    steDetails.PropertyPrice = reader.IsDBNull(reader.GetOrdinal("property_price")) ? "" : reader.GetString(reader.GetOrdinal("property_price"));
                    steDetails.CapRate = reader.IsDBNull(reader.GetOrdinal("cap_rate")) ? 0 : reader.GetDouble(reader.GetOrdinal("cap_rate"));

                    steDetails.Term = reader.IsDBNull(reader.GetOrdinal("term")) ? "" : reader.GetString(reader.GetOrdinal("term"));

                    steDetails.PdfFileName = reader.IsDBNull(reader.GetOrdinal("detail_pdf")) ? "" : reader.GetString(reader.GetOrdinal("detail_pdf"));
                    steDetails.StateName = reader.IsDBNull(reader.GetOrdinal("state_name")) ? "" : reader.GetString(reader.GetOrdinal("state_name"));

                    steDetails.CreatedDate = reader.IsDBNull(reader.GetOrdinal("created_date")) ? DateTime.Now : reader.GetDateTime(reader.GetOrdinal("created_date"));

                    steDetails.AssetTypeId = reader.IsDBNull(reader.GetOrdinal("asset_type_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("asset_type_id"));
                    steDetails.AssetTypeName = reader.IsDBNull(reader.GetOrdinal("asset_type_name")) ? "" : reader.GetString(reader.GetOrdinal("asset_type_name"));

                    steDetails.IsShoppingCenter = reader.IsDBNull(reader.GetOrdinal("is_shopping_center")) ? false : reader.GetBoolean(reader.GetOrdinal("is_shopping_center"));
                    if (steDetails.IsShoppingCenter)
                    {
                        steDetails.ShoppingCenterOrNetlease = "Shopping Center";
                    }
                    else
                    {
                        steDetails.ShoppingCenterOrNetlease = "Net Lease";
                    }

                    steDetails.Address = reader.IsDBNull(reader.GetOrdinal("property_address")) ? "" : reader.GetString(reader.GetOrdinal("property_address"));
                    steDetails.ZipCode = reader.IsDBNull(reader.GetOrdinal("property_zipcode")) ? "" : reader.GetString(reader.GetOrdinal("property_zipcode"));

                    if (steDetails.PdfFileName.Trim().Length > 0)
                    {
                        string fileName = @"../../UploadedPdf/" + steDetails.PdfFileName;
                        steDetails.PdfFileName = fileName;
                    }
                    

                    steDetails.DiligenceType = reader.IsDBNull(reader.GetOrdinal("diligence_type")) ? 0 : reader.GetInt32(reader.GetOrdinal("diligence_type"));

                    steDetails.Latitude = reader.IsDBNull(reader.GetOrdinal("property_latitude")) ? "" : reader.GetString(reader.GetOrdinal("property_latitude"));
                    steDetails.Longitude = reader.IsDBNull(reader.GetOrdinal("property_longitude")) ? "" : reader.GetString(reader.GetOrdinal("property_longitude"));

                    steDetails.SelectedPropertyStatusId = reader.IsDBNull(reader.GetOrdinal("property_status_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("property_status_id"));
                    steDetails.SelectedPropertyStatus = reader.IsDBNull(reader.GetOrdinal("property_status")) ? "" : reader.GetString(reader.GetOrdinal("property_status"));

                    steDetails.CheckIfPropertyListed = reader.IsDBNull(reader.GetOrdinal("check_if_property_listed")) ? 0 : reader.GetInt32(reader.GetOrdinal("check_if_property_listed"));
                    steDetails.ListingAgentName = reader.IsDBNull(reader.GetOrdinal("listing_agent_name")) ? "" : reader.GetString(reader.GetOrdinal("listing_agent_name"));
                    steDetails.ListingExpiry = reader.IsDBNull(reader.GetOrdinal("listing_expiry")) ? DateTime.Now : reader.GetDateTime(reader.GetOrdinal("listing_expiry"));

                    steDetails.ListingPrice = reader.IsDBNull(reader.GetOrdinal("listing_price")) ? "" : reader.GetString(reader.GetOrdinal("listing_price"));
                    steDetails.AskingRent = reader.IsDBNull(reader.GetOrdinal("asking_rent")) ? "" : reader.GetString(reader.GetOrdinal("asking_rent"));
                    steDetails.LeaseType = reader.IsDBNull(reader.GetOrdinal("lease_type")) ? 0 : reader.GetInt32(reader.GetOrdinal("lease_type"));
                    
                    steDetails.Details = reader.IsDBNull(reader.GetOrdinal("details")) ? "" : reader.GetString(reader.GetOrdinal("details"));

                    steDetails.ShoppingMartPlanFileName = reader.IsDBNull(reader.GetOrdinal("shopping_mart_plan_file_name")) ? "" : reader.GetString(reader.GetOrdinal("shopping_mart_plan_file_name"));
                    steDetails.SavedShoppingMartPlanFileName = reader.IsDBNull(reader.GetOrdinal("shopping_mart_plan_file_name")) ? "" : "OtherFiles/" + reader.GetString(reader.GetOrdinal("shopping_mart_plan_file_name"));

                    steDetails.StatusChangedDate = reader.IsDBNull(reader.GetOrdinal("status_changed_date")) ? DateTime.Now : reader.GetDateTime(reader.GetOrdinal("status_changed_date"));
                    steDetails.IsClosed = reader.IsDBNull(reader.GetOrdinal("is_closed")) ? 0 : reader.GetInt32(reader.GetOrdinal("is_closed"));
                    steDetails.ShowInListing = reader.IsDBNull(reader.GetOrdinal("can_publish")) ? false : reader.GetBoolean(reader.GetOrdinal("can_publish"));

                    steDetails.TermOptionPurchase = reader.IsDBNull(reader.GetOrdinal("term_option_purchase")) ? "" : reader.GetString(reader.GetOrdinal("term_option_purchase"));
                    steDetails.AskingRentOptionPurchase = reader.IsDBNull(reader.GetOrdinal("asking_rent_option_purchase")) ? "" : reader.GetString(reader.GetOrdinal("asking_rent_option_purchase"));
                    steDetails.LeaseTypePurchase = reader.IsDBNull(reader.GetOrdinal("lease_type_purchase")) ? 0 : reader.GetInt32(reader.GetOrdinal("lease_type_purchase"));
                    steDetails.OptionPurchasePrice = reader.IsDBNull(reader.GetOrdinal("option_purchase_price")) ? "" : reader.GetString(reader.GetOrdinal("option_purchase_price"));

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


                SqlCommand cmdImageList = new SqlCommand("GetPropertyImageList", con);

                cmdImageList.Parameters.AddWithValue("property_id", propertyId);
                cmdImageList.Parameters.AddWithValue("property_type", SamsPropertyType.NetLease);

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

                SqlCommand cmdComplianceList = new SqlCommand("GetNetleaseComplianceFiles", con);

                cmdComplianceList.Parameters.AddWithValue("property_id", propertyId);
                cmdComplianceList.CommandType = CommandType.StoredProcedure;
                con.Open();

                SqlDataReader readerComplianceList = cmdComplianceList.ExecuteReader();
                
                while (readerComplianceList.Read())
                {
                    var c_storeFile = new AdditionalFilesViewModel();
                    c_storeFile.FileId = readerComplianceList.IsDBNull(readerComplianceList.GetOrdinal("file_id")) ? 0 : readerComplianceList.GetInt32(readerComplianceList.GetOrdinal("file_id"));
                    c_storeFile.PropertyId = propertyId;
                    c_storeFile.FileType = readerComplianceList.IsDBNull(readerComplianceList.GetOrdinal("file_type")) ? "" : readerComplianceList.GetString(readerComplianceList.GetOrdinal("file_type"));


                    c_storeFile.FileName = readerComplianceList.IsDBNull(readerComplianceList.GetOrdinal("file_name")) ? "" : readerComplianceList.GetString(readerComplianceList.GetOrdinal("file_name"));
                    c_storeFile.FileNameWithoutPath = c_storeFile.FileName;
                    string pic = @"../../property_files/" + c_storeFile.FileName;
                    c_storeFile.FileName = pic;
                    confidentialFiles.Add(c_storeFile);
                }
                con.Close();

                steDetails.NDAComplaintsFilesList = confidentialFiles;
                steDetails.StateList = stateList;
                steDetails.ImageList = propertyImageList;
                steDetails.AdditionalFilesList = additionalFiles;
                steDetails.TodoList = GetTodoList(steDetails.NetleasePropertyId);

                steDetails.DiligenceAcquisitions = GetDiligenceAcquisition(propertyId);
                steDetails.DiligenceDispositionList = GetDiligenceDispositions(propertyId);
                steDetails.DiligenceLeaseList = GetDiligenceLease(propertyId);

                steDetails.DispositionPeriodList = GetPeriodList(propertyId, "Disposition");
                steDetails.LeasePeriodList = GetPeriodList(propertyId, "Lease");
                steDetails.LeaseTypeList = GetLeaseTypeList();
                steDetails.FutureTenantList = GetFutureTenantList(steDetails.NetleasePropertyId);

                steDetails.CanAddTransactions = true;

                if (steDetails.AssetTypeId == (int)SamAssetType.Fee || steDetails.AssetTypeId == (int)SamAssetType.FeeSubjectToLease)
                {
                    foreach (DiligenceDispositionsViewModel ddm in steDetails.DiligenceDispositionList)
                    {
                        if (ddm.SelectedTransactionStatusId == (int)TransactionStatus.Closed_Acquisitions || ddm.SelectedTransactionStatusId == (int)TransactionStatus.Under_Contract || ddm.SelectedTransactionStatusId == (int)TransactionStatus.Under_LOI)
                        {
                            steDetails.CanAddTransactions = false;
                        }

                        if (steDetails.MaxPriorityTransactionStatusId == 0)
                        {
                            steDetails.MaxPriorityTransactionStatusId = ddm.SelectedTransactionStatusId;
                            steDetails.MaxPriorityTransactionStatusName = ddm.SelectedTransactionStatusName;

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
                        }

                        if (ddm.SelectedTransactionStatusId == (int)SamsTransactionStatus.Under_LOI &&
                            (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Under_Contract) &&
                            (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Closed_Dispositions))
                        {
                            steDetails.MaxPriorityTransactionStatusId = ddm.SelectedTransactionStatusId;
                            steDetails.MaxPriorityTransactionStatusName = ddm.SelectedTransactionStatusName;
                            steDetails.StatusChangedDate = ddm.SelectedTransactionDate;
                        }

                        if (ddm.SelectedTransactionStatusId == (int)SamsTransactionStatus.Under_Contract &&
                            (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Closed_Dispositions))
                        {
                            steDetails.MaxPriorityTransactionStatusId = ddm.SelectedTransactionStatusId;
                            steDetails.MaxPriorityTransactionStatusName = ddm.SelectedTransactionStatusName;
                            steDetails.StatusChangedDate = ddm.SelectedTransactionDate;
                        }

                        if (ddm.SelectedTransactionStatusId == (int)SamsTransactionStatus.Closed_Dispositions)
                        {
                            steDetails.MaxPriorityTransactionStatusId = ddm.SelectedTransactionStatusId;
                            steDetails.MaxPriorityTransactionStatusName = ddm.SelectedTransactionStatusName;
                            steDetails.StatusChangedDate = ddm.SelectedTransactionDate;
                        }
                    }
                }
                else if(steDetails.AssetTypeId==(int)SamAssetType.Lease)
                {
                    

                    int leaseLoi = 0, leaseUnderContract = 0, leaseTerminated = 0, leaseClosed = 0;
                    foreach (DiligenceLeaseViewModel dl in steDetails.DiligenceLeaseList)
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
                else if (steDetails.AssetTypeId == (int)SamAssetType.NetLease)
                {
                    steDetails.DiligenceNetleaseList = GetDiligenceNetleaseList(steDetails.NetleasePropertyId);
                    int leaseLoi = 0, leaseUnderContract = 0, leaseTerminated = 0, leaseClosed = 0;
                    foreach (DiligenceNetleaseViewModel dl in steDetails.DiligenceNetleaseList)
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
                else if (steDetails.AssetTypeId == (int)SamAssetType.LeaseWithPurchaseOption)
                {
                    steDetails.DiligenceLeaseWithPurchaseList = GetDiligenceLeaseWithPurchaseList(steDetails.NetleasePropertyId);
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

            }
            steDetails.ShoppingCenterClientList = GetShoppingCenterClientList(steDetails.NetleasePropertyId);
            steDetails.PropertyHistoryList = PropertyHistory.GetPropertyHistoryList(steDetails.NetleasePropertyId);
            steDetails.DiligenceDispositions_SaleLeaseBack = GetDiligenceDispositions_SaleLeaseBack(propertyId);

            

            return View(steDetails);
        }

        public IActionResult GetShoppingCenterClientItem(int shoppingCenterClientId, int cStoreId)
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
            var shoppingCenterClient = new ShoppingCenterClients();

            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmdShoppingCenterClient = new SqlCommand("GetShoppingCenterClientItem", con);
                cmdShoppingCenterClient.Parameters.AddWithValue("shopping_center_client_id", shoppingCenterClientId);

                cmdShoppingCenterClient.CommandType = CommandType.StoredProcedure;
                con.Open();

                SqlDataReader readerShoppingCenterClient = cmdShoppingCenterClient.ExecuteReader();
                while (readerShoppingCenterClient.Read())
                {

                    shoppingCenterClient.ShoppingCenterClientId = readerShoppingCenterClient.IsDBNull(readerShoppingCenterClient.GetOrdinal("shopping_center_client_id")) ? 0 : readerShoppingCenterClient.GetInt32(readerShoppingCenterClient.GetOrdinal("shopping_center_client_id"));
                    shoppingCenterClient.CStoreId = readerShoppingCenterClient.IsDBNull(readerShoppingCenterClient.GetOrdinal("c_store_id")) ? 0 : readerShoppingCenterClient.GetInt32(readerShoppingCenterClient.GetOrdinal("c_store_id"));
                    shoppingCenterClient.TenantName = readerShoppingCenterClient.IsDBNull(readerShoppingCenterClient.GetOrdinal("tenant_name")) ? "" : readerShoppingCenterClient.GetString(readerShoppingCenterClient.GetOrdinal("tenant_name"));

                    shoppingCenterClient.UnitSelected = readerShoppingCenterClient.IsDBNull(readerShoppingCenterClient.GetOrdinal("unit_selected")) ? "" : readerShoppingCenterClient.GetString(readerShoppingCenterClient.GetOrdinal("unit_selected"));
                    shoppingCenterClient.AnnualRent = readerShoppingCenterClient.IsDBNull(readerShoppingCenterClient.GetOrdinal("annual_rent")) ? "" : readerShoppingCenterClient.GetString(readerShoppingCenterClient.GetOrdinal("annual_rent"));
                    shoppingCenterClient.MonthlyRent = readerShoppingCenterClient.IsDBNull(readerShoppingCenterClient.GetOrdinal("monthly_rent")) ? "" : readerShoppingCenterClient.GetString(readerShoppingCenterClient.GetOrdinal("monthly_rent"));
                    shoppingCenterClient.CamMonthly = readerShoppingCenterClient.IsDBNull(readerShoppingCenterClient.GetOrdinal("cam_monthly")) ? "" : readerShoppingCenterClient.GetString(readerShoppingCenterClient.GetOrdinal("cam_monthly"));
                    shoppingCenterClient.CamYearly = readerShoppingCenterClient.IsDBNull(readerShoppingCenterClient.GetOrdinal("cam_yearly")) ? "" : readerShoppingCenterClient.GetString(readerShoppingCenterClient.GetOrdinal("cam_yearly"));
                    shoppingCenterClient.SetOrAdjustAutomatically = readerShoppingCenterClient.IsDBNull(readerShoppingCenterClient.GetOrdinal("set_or_adjust_automatically")) ? "" : readerShoppingCenterClient.GetString(readerShoppingCenterClient.GetOrdinal("set_or_adjust_automatically"));
                    shoppingCenterClient.RentAndCamMonthly = readerShoppingCenterClient.IsDBNull(readerShoppingCenterClient.GetOrdinal("rent_and_cam_monthly")) ? "" : readerShoppingCenterClient.GetString(readerShoppingCenterClient.GetOrdinal("rent_and_cam_monthly"));
                    shoppingCenterClient.RentAndCamYearly = readerShoppingCenterClient.IsDBNull(readerShoppingCenterClient.GetOrdinal("rent_and_cam_yearly")) ? "" : readerShoppingCenterClient.GetString(readerShoppingCenterClient.GetOrdinal("rent_and_cam_yearly"));
                    shoppingCenterClient.PiecePerSquareFoot = readerShoppingCenterClient.IsDBNull(readerShoppingCenterClient.GetOrdinal("piece_per_square_foot")) ? "" : readerShoppingCenterClient.GetString(readerShoppingCenterClient.GetOrdinal("piece_per_square_foot"));
                    shoppingCenterClient.LeaseExpaires = readerShoppingCenterClient.IsDBNull(readerShoppingCenterClient.GetOrdinal("lease_expires")) ? "" : readerShoppingCenterClient.GetString(readerShoppingCenterClient.GetOrdinal("lease_expires"));

                    shoppingCenterClient.DateRentChanges = readerShoppingCenterClient.IsDBNull(readerShoppingCenterClient.GetOrdinal("date_rent_changed")) ? default(DateTime?) : readerShoppingCenterClient.GetDateTime(readerShoppingCenterClient.GetOrdinal("date_rent_changed"));
                    //shoppingCenterClient.DateRentChanges = readerShoppingCenterClient.IsDBNull(readerShoppingCenterClient.GetOrdinal("date_rent_changed")) ? "" : readerShoppingCenterClient.GetString(readerShoppingCenterClient.GetOrdinal("date_rent_changed"));

                    shoppingCenterClient.AnnualRentChangeTo = readerShoppingCenterClient.IsDBNull(readerShoppingCenterClient.GetOrdinal("annual_rent_changed_to")) ? "" : readerShoppingCenterClient.GetString(readerShoppingCenterClient.GetOrdinal("annual_rent_changed_to"));

                    shoppingCenterClient.RentPerMonthChangeTo = readerShoppingCenterClient.IsDBNull(readerShoppingCenterClient.GetOrdinal("rent_per_month_changed_to")) ? "" : readerShoppingCenterClient.GetString(readerShoppingCenterClient.GetOrdinal("rent_per_month_changed_to"));
                    shoppingCenterClient.RentAndCamChangeTo = readerShoppingCenterClient.IsDBNull(readerShoppingCenterClient.GetOrdinal("rent_and_cam_changed_to")) ? "" : readerShoppingCenterClient.GetString(readerShoppingCenterClient.GetOrdinal("rent_and_cam_changed_to"));
                    shoppingCenterClient.PiecePerSquareFootChangeTo = readerShoppingCenterClient.IsDBNull(readerShoppingCenterClient.GetOrdinal("piece_per_square_foot_changed_to")) ? "" : readerShoppingCenterClient.GetString(readerShoppingCenterClient.GetOrdinal("piece_per_square_foot_changed_to"));
                    shoppingCenterClient.SubspaceSquareFootage = readerShoppingCenterClient.IsDBNull(readerShoppingCenterClient.GetOrdinal("subspace_square_footage")) ? "" : readerShoppingCenterClient.GetString(readerShoppingCenterClient.GetOrdinal("subspace_square_footage"));
                    shoppingCenterClient.Notes = readerShoppingCenterClient.IsDBNull(readerShoppingCenterClient.GetOrdinal("notes")) ? "" : readerShoppingCenterClient.GetString(readerShoppingCenterClient.GetOrdinal("notes"));
                    shoppingCenterClient.CoiExpire = readerShoppingCenterClient.IsDBNull(readerShoppingCenterClient.GetOrdinal("coi_expire")) ? default(DateTime?) : readerShoppingCenterClient.GetDateTime(readerShoppingCenterClient.GetOrdinal("coi_expire"));

                }
                con.Close();
            }
            shoppingCenterClient.CStoreId = cStoreId;
            return View(shoppingCenterClient);
        }


        [HttpPost]
        public IActionResult SaveShoppingCenterTenant(ShoppingCenterClients shoppingCenterClient)
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
                SqlCommand cmd = new SqlCommand("SaveShoppingCenterTenant", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                cmd.Parameters.AddWithValue("shopping_center_client_id", shoppingCenterClient.ShoppingCenterClientId);
                cmd.Parameters.AddWithValue("c_store_id", shoppingCenterClient.CStoreId);

                cmd.Parameters.AddWithValue("tenant_name", shoppingCenterClient.TenantName);
                cmd.Parameters.AddWithValue("unit_selected", shoppingCenterClient.UnitSelected);
                cmd.Parameters.AddWithValue("annual_rent", shoppingCenterClient.AnnualRent);
                cmd.Parameters.AddWithValue("monthly_rent", shoppingCenterClient.MonthlyRent);
                cmd.Parameters.AddWithValue("cam_monthly", shoppingCenterClient.CamMonthly);
                cmd.Parameters.AddWithValue("cam_yearly", shoppingCenterClient.CamYearly);
                cmd.Parameters.AddWithValue("set_or_adjust_automatically", shoppingCenterClient.SetOrAdjustAutomatically);
                cmd.Parameters.AddWithValue("rent_and_cam_monthly", shoppingCenterClient.RentAndCamMonthly);

                cmd.Parameters.AddWithValue("rent_and_cam_yearly", shoppingCenterClient.RentAndCamYearly);
                cmd.Parameters.AddWithValue("piece_per_square_foot", shoppingCenterClient.PiecePerSquareFoot);
                cmd.Parameters.AddWithValue("lease_expires", shoppingCenterClient.LeaseExpaires);
                cmd.Parameters.AddWithValue("date_rent_changed", shoppingCenterClient.DateRentChanges);
                cmd.Parameters.AddWithValue("annual_rent_changed_to", shoppingCenterClient.AnnualRentChangeTo);
                cmd.Parameters.AddWithValue("rent_per_month_changed_to", shoppingCenterClient.RentPerMonthChangeTo);
                cmd.Parameters.AddWithValue("rent_and_cam_changed_to", shoppingCenterClient.RentAndCamChangeTo);
                cmd.Parameters.AddWithValue("piece_per_square_foot_changed_to", shoppingCenterClient.PiecePerSquareFootChangeTo);

                cmd.Parameters.AddWithValue("subspace_square_footage", shoppingCenterClient.SubspaceSquareFootage);
                cmd.Parameters.AddWithValue("notes", shoppingCenterClient.Notes);
                cmd.Parameters.AddWithValue("coi_expire", shoppingCenterClient.CoiExpire); 

                cmd.ExecuteNonQuery();

                con.Close();
            }
            // return RedirectToAction("ViewCStore", new { propertyId = shoppingCenterClient.CStoreId });
            return RedirectToAction("ViewNetLeaseProperties", new { propertyId = shoppingCenterClient.CStoreId });
        }

        public IActionResult DeleteShoppingCenterClientItem(int shoppingCenterClientId, int cStoreId)
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
            var shoppingCenterClient = new ShoppingCenterClients();

            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmdShoppingCenterClient = new SqlCommand("DeleteShoppingCenterClient", con);
                cmdShoppingCenterClient.Parameters.AddWithValue("shopping_center_client_id", shoppingCenterClientId);

                cmdShoppingCenterClient.CommandType = CommandType.StoredProcedure;
                con.Open();

                cmdShoppingCenterClient.ExecuteNonQuery();

                con.Close();
            }
            shoppingCenterClient.CStoreId = cStoreId;
            //return RedirectToAction("ViewCStore", new { propertyId = cStoreId });
            return RedirectToAction("ViewNetLeaseProperties", new { propertyId = cStoreId });
        }


        public IActionResult ExporTenantListTotExcel(int cStoreId)
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
            //string fileName = Path.GetFileNameWithoutExtension(@"\\OpsVsAdp\\Files\\Daily\\TempHours.xlsx");
            var filePath = Path.Combine(webHostEnvironment.WebRootPath + @"/templates", "Tenant_List_Template.xlsx");

            string fullFileName = Path.Combine(webHostEnvironment.WebRootPath + @"/templates", "Tenant_List_Template.xlsx");
            string fullToFileName = "Tenant_List" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xlsx";

            Workbook wrkBook = new Workbook();
            wrkBook.LoadFromFile(fullFileName);
            Worksheet sheet = wrkBook.Worksheets[0];

            string propertyHeader = "";

            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("GetCStoreById", con);

                cmd.Parameters.AddWithValue("c_store_id", cStoreId);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    propertyHeader = reader.IsDBNull(reader.GetOrdinal("property_header")) ? "" : reader.GetString(reader.GetOrdinal("property_header"));
                    // sheet.Range["A1"].Value = propertyHeader;
                }
                con.Close();
            }

            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmdShoppingCenterClient = new SqlCommand("GetShoppingCenterClientList", con);
                cmdShoppingCenterClient.Parameters.AddWithValue("c_store_id", cStoreId);

                cmdShoppingCenterClient.CommandType = CommandType.StoredProcedure;
                con.Open();

                int i = 4;
                string colTenantName = "A", colUnit = "B", colAnnualRent = "C", colRentMonthly = "D", colCamMonthly = "E", colCamYearly = "F";
                string colSetAdjusted = "G", colRentAndCamMonthly = "H", colRentAndCamYearly = "I", colPricePerSquareFoot = "J", colLeaseExpires = "K";
                string colDateRentChanges = "L", colAnnualRentChangedTo = "M", colREntMonthWillChangeTo = "N", colRentAndCamChangeTo = "O";
                string colPricePerSquareFootChangeTo = "P", colSubspaceSquareFootage = "Q", colCoi = "R", colNotes = "S";
                

                SqlDataReader readerShoppingCenterClient = cmdShoppingCenterClient.ExecuteReader();
                while (readerShoppingCenterClient.Read())
                {
                    var shoppingCenterClient = new ShoppingCenterClients();

                    shoppingCenterClient.ShoppingCenterClientId = readerShoppingCenterClient.IsDBNull(readerShoppingCenterClient.GetOrdinal("shopping_center_client_id")) ? 0 : readerShoppingCenterClient.GetInt32(readerShoppingCenterClient.GetOrdinal("shopping_center_client_id"));
                    shoppingCenterClient.CStoreId = readerShoppingCenterClient.IsDBNull(readerShoppingCenterClient.GetOrdinal("c_store_id")) ? 0 : readerShoppingCenterClient.GetInt32(readerShoppingCenterClient.GetOrdinal("c_store_id"));
                    shoppingCenterClient.TenantName = readerShoppingCenterClient.IsDBNull(readerShoppingCenterClient.GetOrdinal("tenant_name")) ? "" : readerShoppingCenterClient.GetString(readerShoppingCenterClient.GetOrdinal("tenant_name"));

                    shoppingCenterClient.UnitSelected = readerShoppingCenterClient.IsDBNull(readerShoppingCenterClient.GetOrdinal("unit_selected")) ? "" : readerShoppingCenterClient.GetString(readerShoppingCenterClient.GetOrdinal("unit_selected"));
                    shoppingCenterClient.AnnualRent = readerShoppingCenterClient.IsDBNull(readerShoppingCenterClient.GetOrdinal("annual_rent")) ? "" : readerShoppingCenterClient.GetString(readerShoppingCenterClient.GetOrdinal("annual_rent"));
                    shoppingCenterClient.MonthlyRent = readerShoppingCenterClient.IsDBNull(readerShoppingCenterClient.GetOrdinal("monthly_rent")) ? "" : readerShoppingCenterClient.GetString(readerShoppingCenterClient.GetOrdinal("monthly_rent"));
                    shoppingCenterClient.CamMonthly = readerShoppingCenterClient.IsDBNull(readerShoppingCenterClient.GetOrdinal("cam_monthly")) ? "" : readerShoppingCenterClient.GetString(readerShoppingCenterClient.GetOrdinal("cam_monthly"));
                    shoppingCenterClient.CamYearly = readerShoppingCenterClient.IsDBNull(readerShoppingCenterClient.GetOrdinal("cam_yearly")) ? "" : readerShoppingCenterClient.GetString(readerShoppingCenterClient.GetOrdinal("cam_yearly"));
                    shoppingCenterClient.SetOrAdjustAutomatically = readerShoppingCenterClient.IsDBNull(readerShoppingCenterClient.GetOrdinal("set_or_adjust_automatically")) ? "" : readerShoppingCenterClient.GetString(readerShoppingCenterClient.GetOrdinal("set_or_adjust_automatically"));
                    shoppingCenterClient.RentAndCamMonthly = readerShoppingCenterClient.IsDBNull(readerShoppingCenterClient.GetOrdinal("rent_and_cam_monthly")) ? "" : readerShoppingCenterClient.GetString(readerShoppingCenterClient.GetOrdinal("rent_and_cam_monthly"));
                    shoppingCenterClient.RentAndCamYearly = readerShoppingCenterClient.IsDBNull(readerShoppingCenterClient.GetOrdinal("rent_and_cam_yearly")) ? "" : readerShoppingCenterClient.GetString(readerShoppingCenterClient.GetOrdinal("rent_and_cam_yearly"));
                    shoppingCenterClient.PiecePerSquareFoot = readerShoppingCenterClient.IsDBNull(readerShoppingCenterClient.GetOrdinal("piece_per_square_foot")) ? "" : readerShoppingCenterClient.GetString(readerShoppingCenterClient.GetOrdinal("piece_per_square_foot"));
                    shoppingCenterClient.LeaseExpaires = readerShoppingCenterClient.IsDBNull(readerShoppingCenterClient.GetOrdinal("lease_expires")) ? "" : readerShoppingCenterClient.GetString(readerShoppingCenterClient.GetOrdinal("lease_expires"));

                    shoppingCenterClient.DateRentChanges = readerShoppingCenterClient.IsDBNull(readerShoppingCenterClient.GetOrdinal("date_rent_changed")) ? default(DateTime?) : readerShoppingCenterClient.GetDateTime(readerShoppingCenterClient.GetOrdinal("date_rent_changed"));
                    //shoppingCenterClient.DateRentChanges = readerShoppingCenterClient.IsDBNull(readerShoppingCenterClient.GetOrdinal("date_rent_changed")) ? "" : readerShoppingCenterClient.GetString(readerShoppingCenterClient.GetOrdinal("date_rent_changed"));

                    shoppingCenterClient.AnnualRentChangeTo = readerShoppingCenterClient.IsDBNull(readerShoppingCenterClient.GetOrdinal("annual_rent_changed_to")) ? "" : readerShoppingCenterClient.GetString(readerShoppingCenterClient.GetOrdinal("annual_rent_changed_to"));

                    shoppingCenterClient.RentPerMonthChangeTo = readerShoppingCenterClient.IsDBNull(readerShoppingCenterClient.GetOrdinal("rent_per_month_changed_to")) ? "" : readerShoppingCenterClient.GetString(readerShoppingCenterClient.GetOrdinal("rent_per_month_changed_to"));
                    shoppingCenterClient.RentAndCamChangeTo = readerShoppingCenterClient.IsDBNull(readerShoppingCenterClient.GetOrdinal("rent_and_cam_changed_to")) ? "" : readerShoppingCenterClient.GetString(readerShoppingCenterClient.GetOrdinal("rent_and_cam_changed_to"));
                    shoppingCenterClient.PiecePerSquareFootChangeTo = readerShoppingCenterClient.IsDBNull(readerShoppingCenterClient.GetOrdinal("piece_per_square_foot_changed_to")) ? "" : readerShoppingCenterClient.GetString(readerShoppingCenterClient.GetOrdinal("piece_per_square_foot_changed_to"));
                    shoppingCenterClient.SubspaceSquareFootage = readerShoppingCenterClient.IsDBNull(readerShoppingCenterClient.GetOrdinal("subspace_square_footage")) ? "" : readerShoppingCenterClient.GetString(readerShoppingCenterClient.GetOrdinal("subspace_square_footage"));
                    shoppingCenterClient.CoiExpire = readerShoppingCenterClient.IsDBNull(readerShoppingCenterClient.GetOrdinal("coi_expire")) ? default(DateTime?) : readerShoppingCenterClient.GetDateTime(readerShoppingCenterClient.GetOrdinal("coi_expire"));
                    shoppingCenterClient.Notes = readerShoppingCenterClient.IsDBNull(readerShoppingCenterClient.GetOrdinal("notes")) ? "" : readerShoppingCenterClient.GetString(readerShoppingCenterClient.GetOrdinal("notes"));


                    string cellTenantName = colTenantName + i.ToString();
                    string cellUnit = colUnit + i.ToString();
                    string cellAnnualRent = colAnnualRent + i.ToString();
                    string cellRentMonthly = colRentMonthly + i.ToString();
                    string cellCamMonthly = colCamMonthly + i.ToString();
                    string cellCamYearly = colCamYearly + i.ToString();
                    string cellSetAdjusted = colSetAdjusted + i.ToString();
                    string cellRentAndCamMonthly = colRentAndCamMonthly + i.ToString();
                    string cellRentAndCamYearly = colRentAndCamYearly + i.ToString();
                    string cellPricePerSquareFoot = colPricePerSquareFoot + i.ToString();
                    string cellLeaseExpires = colLeaseExpires + i.ToString();
                    string cellDateRentChanges = colDateRentChanges + i.ToString();
                    string cellAnnualRentChangedTo = colAnnualRentChangedTo + i.ToString();
                    string cellRentMonthWillChangeTo = colREntMonthWillChangeTo + i.ToString();
                    string cellRentAndCamChangeTo = colRentAndCamChangeTo + i.ToString();
                    string cellPricePerSquareFootChangeTo = colPricePerSquareFootChangeTo + i.ToString();
                    string cellSubspaceSquareFootage = colSubspaceSquareFootage + i.ToString();
                    string cellCoi = colCoi + i.ToString();
                    string cellNotes = colNotes + i.ToString();

                    sheet.Range[cellTenantName].Value = shoppingCenterClient.TenantName;
                    sheet.Range[cellUnit].Value = shoppingCenterClient.UnitSelected;

                    sheet.Range[cellAnnualRent].Value = shoppingCenterClient.AnnualRent;
                    sheet.Range[cellRentMonthly].Value = shoppingCenterClient.MonthlyRent;
                    sheet.Range[cellCamMonthly].Value = shoppingCenterClient.CamMonthly;
                    sheet.Range[cellCamYearly].Value = shoppingCenterClient.CamYearly;
                    sheet.Range[cellSetAdjusted].Value = shoppingCenterClient.SetOrAdjustAutomatically;
                    sheet.Range[cellRentAndCamMonthly].Value = shoppingCenterClient.RentAndCamMonthly;
                    sheet.Range[cellRentAndCamYearly].Value = shoppingCenterClient.RentAndCamYearly;
                    sheet.Range[cellPricePerSquareFoot].Value = shoppingCenterClient.PiecePerSquareFoot;
                    sheet.Range[cellLeaseExpires].Value = shoppingCenterClient.LeaseExpaires;
                    sheet.Range[cellDateRentChanges].Value = shoppingCenterClient.DateRentChanges.ToString();
                    sheet.Range[cellAnnualRentChangedTo].Value = shoppingCenterClient.AnnualRentChangeTo;
                    sheet.Range[cellRentMonthWillChangeTo].Value = shoppingCenterClient.RentPerMonthChangeTo;

                    sheet.Range[cellRentAndCamChangeTo].Value = shoppingCenterClient.RentAndCamChangeTo;
                    sheet.Range[cellPricePerSquareFootChangeTo].Value = shoppingCenterClient.PiecePerSquareFootChangeTo;
                    sheet.Range[cellSubspaceSquareFootage].Value = shoppingCenterClient.SubspaceSquareFootage;
                    if(shoppingCenterClient.CoiExpire != default(DateTime?))
                    {
                        sheet.Range[cellCoi].Value = shoppingCenterClient.CoiExpire.Value.ToString("MM-dd-yyyy");
                    }
                    
                    sheet.Range[cellNotes].Value = shoppingCenterClient.Notes;


                    i = i + 1;
                }
                con.Close();
            }


            wrkBook.SaveToFile(fullToFileName);

            byte[] fileBytes = GetFile(fullToFileName);
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fullToFileName);
        }



        List<DiligenceLeaseViewModel> GetDiligenceLease(int propertyId)
        {

            var diligenceLeaseList = new List<DiligenceLeaseViewModel>();

            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("GetDiligenceLease_ForLeaseTransaction", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("property_id", propertyId);
                cmd.Parameters.AddWithValue("property_type", SamsPropertyType.NetLease);
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

                    diligenceLease.ListingPrice = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("listing_price")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("listing_price"));

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

                    diligenceLease.CreatedDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("created_date")) ? DateTime.Now : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("created_date"));

                    diligenceLease.SelectedTransactionStatusId = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("selected_transaction_id")) ? 0 : readerAssetType.GetInt32(readerAssetType.GetOrdinal("selected_transaction_id"));
                    diligenceLease.SelectedTransactionStatusName = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("transaction_status_name")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("transaction_status_name"));
                    diligenceLease.SelectedTransactionDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("selected_transaction_date")) ? DateTime.Now : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("selected_transaction_date"));

                    diligenceLease.ClosingDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("closing_date")) ? default(DateTime?) : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("closing_date"));

                    diligenceLeaseList.Add(diligenceLease);
                }

                con.Close();

            }

            return diligenceLeaseList;
        }

        /*
        [HttpPost]
        public IActionResult SaveDiligenceLease(DiligenceLeaseViewModel diligenceLease)
        {
            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("SaveDiligenceLease", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("diligence_lease_id", diligenceLease.DiligenceLeaseId);

                cmd.Parameters.AddWithValue("property_id", diligenceLease.PropertyId);
                cmd.Parameters.AddWithValue("property_type", SamsPropertyType.NetLease);
                cmd.Parameters.AddWithValue("tenant_name", diligenceLease.Tenant);

                cmd.Parameters.AddWithValue("rent", diligenceLease.Rent);
                cmd.Parameters.AddWithValue("listing_price", diligenceLease.ListingPrice);

                cmd.Parameters.AddWithValue("under_contract_date", diligenceLease.UnderContractDate);
                cmd.Parameters.AddWithValue("due_diligence_expiry_date", diligenceLease.DueDiligenceExpiryDate);
                cmd.Parameters.AddWithValue("earnest_money_deposit", diligenceLease.EarnestMoneyDeposit);
                cmd.Parameters.AddWithValue("ddp_extension", diligenceLease.DDPExtension);

                cmd.Parameters.AddWithValue("tenant_attorney", diligenceLease.TenantAttorney);
                cmd.Parameters.AddWithValue("tenant_agent_commission", diligenceLease.TenantAgentCommission);
                cmd.Parameters.AddWithValue("land_lord_agent_commission", diligenceLease.LandlordAgentCommission);
                cmd.Parameters.AddWithValue("lease_security_deposit", diligenceLease.LeaseSecurityDeposit);

                con.Open();


                diligenceLease.DiligenceLeaseId = int.Parse(cmd.ExecuteScalar().ToString());


                con.Close();

            }

            return RedirectToAction("ViewNetLeaseProperties", new { propertyId = diligenceLease.PropertyId });
        }
        */


        List<PeriodViewModel> GetPeriodList(int propertyId, string periodType)
        {
            var periodList = new List<PeriodViewModel>();

            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("GetPeriodList", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("property_id", propertyId);
                cmd.Parameters.AddWithValue("property_type", SamsPropertyType.NetLease);
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
                cmd.Parameters.AddWithValue("property_type", SamsPropertyType.NetLease);
                cmd.Parameters.AddWithValue("period_master", period.PeriodMaster);

                cmd.Parameters.AddWithValue("start_date", period.StartDate);
                DateTime endDate = period.StartDate.AddDays(period.AddedDuration);

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

            //return RedirectToAction("ViewNetLeaseProperties", new { propertyId = period.PropertyId });
            if (period.PeriodType == "Disposition")
            {
                //return RedirectToAction("GetDispositionCriticalItems", new { diligenceDispositionsId = period.TransactionId, propertyId = period.PropertyId });
                return RedirectToAction("GetDiligenceDispositionById", new { diligenceDispositionId = period.TransactionId, propertyId = period.PropertyId, currentAssetStatusId = period.CurrentAssetStatusId });
            }
            else if(period.PeriodType=="Lease")
            {
                //return RedirectToAction("GetLeaseCriticalItems", new { diligenceLeaseId = period.TransactionId, propertyId = period.PropertyId });
                return RedirectToAction("GetDiligenceLeaseById", new { diligenceLeaseId = period.TransactionId, propertyId = period.PropertyId, currentAssetStatusId = period.CurrentAssetStatusId });
            }
            else if (period.PeriodType == "Netlease")
            {
                //return RedirectToAction("GetLeaseCriticalItems", new { diligenceLeaseId = period.TransactionId, propertyId = period.PropertyId });
                return RedirectToAction("GetDiligenceNetleaseById", new { diligenceDispositionId = period.TransactionId, propertyId = period.PropertyId, currentAssetStatusId = period.CurrentAssetStatusId });
            }
            else
            {
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

            return RedirectToAction("ViewNetLeaseProperties", new { propertyId = propertyId });
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
                cmd.Parameters.AddWithValue("property_type", SamsPropertyType.NetLease);
                con.Open();

                diligenceAcquisition.PropertyId = propertyId;
                diligenceAcquisition.PropertyType = 1;

                SqlDataReader readerAssetType = cmd.ExecuteReader();
                while (readerAssetType.Read())
                {
                    diligenceAcquisition.DiligenceAcquisitionId = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("diligence_acquisition_id")) ? 0 : readerAssetType.GetInt32(readerAssetType.GetOrdinal("diligence_acquisition_id"));


                    diligenceAcquisition.PurchasePrice = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("purchase_price")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("purchase_price"));
                    diligenceAcquisition.EarnestMoney = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("earnest_money")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("earnest_money"));

                    diligenceAcquisition.Exchage1031 = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("exchange_1031")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("exchange_1031"));
                    diligenceAcquisition.Deadline1031 = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("dead_line_1031")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("dead_line_1031"));

                    diligenceAcquisition.Sellers = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("sellers")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("sellers"));
                    diligenceAcquisition.EscrowAgent = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("escrow_agent")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("escrow_agent"));
                    diligenceAcquisition.SubDivision = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("sub_division")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("sub_division")); diligenceAcquisition.Deadline1031 = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("dead_line_1031")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("dead_line_1031"));
                    diligenceAcquisition.RealEstateAgent = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("real_estate_agent")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("real_estate_agent"));

                    diligenceAcquisition.CreatedDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("created_date")) ? DateTime.Now : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("created_date"));
                    diligenceAcquisition.AcquisitionStatus = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("acquisition_status")) ? 0 : readerAssetType.GetInt32(readerAssetType.GetOrdinal("acquisition_status"));

                    diligenceAcquisition.ClosingDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("closing_date")) ? default(DateTime?) : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("closing_date"));

                }

                con.Close();

            }

            return diligenceAcquisition;
        }


        List<DiligenceDispositionsViewModel> GetDiligenceDispositions(int propertyId)
        {
            var diligenceDispositionsList = new List<DiligenceDispositionsViewModel>();

            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("GetDiligenceDispositions", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("property_id", propertyId);
                cmd.Parameters.AddWithValue("property_type", SamsPropertyType.NetLease);
                con.Open();

                

                SqlDataReader readerAssetType = cmd.ExecuteReader();
                while (readerAssetType.Read())
                {
                    var diligenceDispositions = new DiligenceDispositionsViewModel();

                    diligenceDispositions.PropertyId = propertyId;
                    diligenceDispositions.PropertyType = 1;

                    diligenceDispositions.DiligenceDispositionsId = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("diligence_dispositions_id")) ? 0 : readerAssetType.GetInt32(readerAssetType.GetOrdinal("diligence_dispositions_id"));

                    diligenceDispositions.SalePrice = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("sale_price")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("sale_price"));
                    diligenceDispositions.EarnestMoney = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("earnest_money")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("earnest_money"));

                    diligenceDispositions.Buyers = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("buyers")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("buyers"));
                    diligenceDispositions.EscrowAgent = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("escrow_agent")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("escrow_agent"));

                    diligenceDispositions.BuyersAttorney = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("buyers_attorney")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("buyers_attorney"));
                    diligenceDispositions.OptionsToExtend = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("options_to_extend")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("options_to_extend"));
                    diligenceDispositions.Commissions = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("commissions")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("commissions"));

                    diligenceDispositions.CreatedDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("created_date")) ? DateTime.Now : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("created_date"));
                    diligenceDispositions.DispositionStatus = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("disposition_status")) ? 0 : readerAssetType.GetInt32(readerAssetType.GetOrdinal("disposition_status"));

                    diligenceDispositions.ClosedDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("closed_date")) ? DateTime.Now : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("closed_date"));
                    diligenceDispositions.TerminatedDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("terminated_date")) ? DateTime.Now : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("terminated_date"));

                    diligenceDispositions.UnderContractDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("under_contract_date")) ? default(DateTime?) : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("under_contract_date"));
                    diligenceDispositions.DueDiligenceExpairyDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("due_diligence_expairy_date")) ? default(DateTime?) : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("due_diligence_expairy_date"));

                    diligenceDispositions.DueDiligenceAmount = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("due_diligence_amount")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("due_diligence_amount"));
                    diligenceDispositions.EMD = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("emd")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("emd"));
                    diligenceDispositions.DDPExtension = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("ddp_extension")) ? default(DateTime?) : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("ddp_extension")); 
                    diligenceDispositions.CreatedDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("created_date")) ? DateTime.Now : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("created_date"));

                    //diligenceDispositions.DDPExtension = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("ddp_extension")) ? DateTime.Now : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("ddp_extension"));
                    diligenceDispositions.DDPExtensionOpted = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("dueDiligenceApplicableStatus")) ? 0 : readerAssetType.GetInt32(readerAssetType.GetOrdinal("dueDiligenceApplicableStatus"));

                    diligenceDispositions.SellersAttorney = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("sellersAttorney")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("sellersAttorney"));
                    diligenceDispositions.BuyersAgentCommission = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("buyers_agent_commision")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("buyers_agent_commision"));
                    diligenceDispositions.SellersAgentCommission = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("sellers_agent_commision")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("sellers_agent_commision"));

                    diligenceDispositions.DispositionTerminatedStatus = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("disposition_terminated_status")) ? 0 : readerAssetType.GetInt32(readerAssetType.GetOrdinal("disposition_terminated_status"));
                    diligenceDispositions.DispositionTerminatedDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("disposition_terminated_date")) ? default(DateTime?) : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("disposition_terminated_date"));
                    diligenceDispositions.DispositionClosedStatus = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("disposition_closed_status")) ? 0 : readerAssetType.GetInt32(readerAssetType.GetOrdinal("disposition_closed_status"));
                    diligenceDispositions.DispositionClosedDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("disposition_closed_date")) ? default(DateTime?) : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("disposition_closed_date"));

                    diligenceDispositions.SelectedTransactionStatusId = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("selected_transaction_id")) ? 0 : readerAssetType.GetInt32(readerAssetType.GetOrdinal("selected_transaction_id"));
                    diligenceDispositions.SelectedTransactionStatusName = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("transaction_status_name")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("transaction_status_name"));
                    diligenceDispositions.SelectedTransactionDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("selected_transaction_date")) ? DateTime.Now : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("selected_transaction_date"));

                    diligenceDispositions.ClosingDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("closing_date")) ? default(DateTime?) : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("closing_date"));

                    diligenceDispositionsList.Add(diligenceDispositions);
                }

                con.Close();

            }

            return diligenceDispositionsList;
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
                cmd.Parameters.AddWithValue("property_type", SamsPropertyType.NetLease);
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

            return RedirectToAction("ViewNetLeaseProperties", new { propertyId = diligenceAcquisition.PropertyId });
        }


        [HttpPost]
        public IActionResult SaveDiligenceDispositions(DiligenceNetleaseViewModel diligenceDispositions)
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
                cmd.Parameters.AddWithValue("property_type", SamsPropertyType.NetLease);
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
                cmd.Parameters.AddWithValue("tenant", diligenceDispositions.Tenant);
                cmd.Parameters.AddWithValue("tenant_rent", diligenceDispositions.TenantRent);
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

            return RedirectToAction("ViewNetLeaseProperties", new { propertyId = diligenceDispositions.PropertyId });
        }

        //
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

            return RedirectToAction("ViewNetLeaseProperties", new { propertyId = propertyId });
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

            return RedirectToAction("ViewNetLeaseProperties", new { propertyId = propertyId });
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

            return RedirectToAction("ViewNetLeaseProperties", new { propertyId = propertyId });
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

            return RedirectToAction("ViewNetLeaseProperties", new { propertyId = propertyId });
        }


        [HttpPost]
        public RedirectToActionResult UploadImage(ImageViewModel uploadedImge)
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


            return RedirectToAction("ViewNetLeaseProperties", new { propertyId = uploadedImge.PropertyId });

        }

        [HttpPost]
        public RedirectToActionResult UploadShoppingMartPlanFileName(ImageViewModel uploadedImge)
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

            var filePath = Path.Combine(webHostEnvironment.WebRootPath + @"/OtherFiles", uniqueFileName);

            using (var stream = System.IO.File.Create(filePath))
            {
                uploadedImge.UploadedImage.CopyTo(stream);
            }

            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("SaveNetleaseShoppingMartPlanFile", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                cmd.Parameters.AddWithValue("property_id", uploadedImge.PropertyId);
                cmd.Parameters.AddWithValue("shopping_mart_plan_file_name", uniqueFileName);

                cmd.ExecuteNonQuery();


                con.Close();
            }


            return RedirectToAction("ViewNetLeaseProperties", new { propertyId = uploadedImge.PropertyId });

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
                return RedirectToAction("ViewNetLeaseProperties", new { propertyId = propertyId });
            }


            

        }

        List<AssetTypeViewModel> GetAssetTypeList(int intAssetType)
        {
            string CS = DBConnection.ConnectionString;
            List<AssetTypeViewModel> assetTypeList = new List<AssetTypeViewModel>();
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmdAssetType = new SqlCommand("GetAssetType", con);

                cmdAssetType.Parameters.AddWithValue("property_type", intAssetType);

                cmdAssetType.CommandType = CommandType.StoredProcedure;
                con.Open();

                SqlDataReader readerAssetType = cmdAssetType.ExecuteReader();
                while (readerAssetType.Read())
                {
                    var assetType = new AssetTypeViewModel();
                    assetType.AssetTypeId = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("asset_type_id")) ? 0 : readerAssetType.GetInt32(readerAssetType.GetOrdinal("asset_type_id"));
                    assetType.AssetTypeName = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("asset_type_name")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("asset_type_name"));

                    assetTypeList.Add(assetType);
                }
                con.Close();
            }
            return assetTypeList;
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
                SqlCommand cmd = new SqlCommand("SaveNetLeaseFiles", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                cmd.Parameters.AddWithValue("file_id", uploadedFile.FileId);
                cmd.Parameters.AddWithValue("property_id", uploadedFile.PropertyId);
                cmd.Parameters.AddWithValue("file_type", uploadedFile.FileType);
                cmd.Parameters.AddWithValue("file_name", uniqueFileName);


                cmd.ExecuteNonQuery();


                con.Close();
            }


            return RedirectToAction("ViewNetLeaseProperties", new { propertyId = uploadedFile.PropertyId });

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
                SqlCommand cmd = new SqlCommand("DeleteNetLeaseFile", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                cmd.Parameters.AddWithValue("file_id", fileId);

                cmd.ExecuteNonQuery();


                con.Close();
                return RedirectToAction("ViewNetLeaseProperties", new { propertyId = propertyId });
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
                SqlCommand cmd = new SqlCommand("DeleteNetleaseProperty", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                cmd.Parameters.AddWithValue("net_lease_property_id", propertyId);

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
                SqlCommand cmd = new SqlCommand("SellNetLeaseProperty", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                cmd.Parameters.AddWithValue("net_lease_property_id", propertyId);

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
                cmd.Parameters.AddWithValue("property_type", SamsPropertyType.NetLease);
                cmd.Parameters.AddWithValue("created_by", loggedInUser.UserId);

                cmd.ExecuteNonQuery();


                con.Close();
                return RedirectToAction("ViewNetLeaseProperties", new { propertyId = todoModel.PropertyId });
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
                cmd.Parameters.AddWithValue("property_type", SamsPropertyType.NetLease);
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


        //Shopping Center List
        public IActionResult GetShoppingCenterList()
        {

            ShoppingCenterDashboardModel dashboardModel = new ShoppingCenterDashboardModel();
            UserViewModel loggedInUser = HttpContext.Session.GetObjectFromJson<UserViewModel>("LoggedInAdmin");
            if (loggedInUser == null)
            {
                return RedirectToAction("DoLogin", "Admin");
            }
            if (loggedInUser != null && loggedInUser.UserId <= 0)
            {
                return RedirectToAction("DoLogin", "Admin");
            }



            List<NetleasePropertiesViewModel> netLeasePropertiesList = new List<NetleasePropertiesViewModel>();

            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("GetNetleaseShoppingCenterList", con);
                cmd.Parameters.AddWithValue("asset_status", 0);

                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var steDetails = new NetleasePropertiesViewModel();
                    steDetails.NetleasePropertyId = reader.IsDBNull(reader.GetOrdinal("net_lease_property_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("net_lease_property_id"));
                    steDetails.AssetId = reader.IsDBNull(reader.GetOrdinal("asset_id")) ? "" : reader.GetString(reader.GetOrdinal("asset_id"));
                    steDetails.AssetName = reader.IsDBNull(reader.GetOrdinal("asset_name")) ? "" : reader.GetString(reader.GetOrdinal("asset_name"));
                    steDetails.StateId = reader.IsDBNull(reader.GetOrdinal("state_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("state_id"));
                    steDetails.City = reader.IsDBNull(reader.GetOrdinal("city")) ? "" : reader.GetString(reader.GetOrdinal("city"));

                    steDetails.PropertyPrice = reader.IsDBNull(reader.GetOrdinal("property_price")) ? "" : reader.GetString(reader.GetOrdinal("property_price"));
                    steDetails.CapRate = reader.IsDBNull(reader.GetOrdinal("cap_rate")) ? 0 : reader.GetDouble(reader.GetOrdinal("cap_rate"));

                    steDetails.Term = reader.IsDBNull(reader.GetOrdinal("term")) ? "" : reader.GetString(reader.GetOrdinal("term"));

                    steDetails.PdfFileName = reader.IsDBNull(reader.GetOrdinal("detail_pdf")) ? "" : reader.GetString(reader.GetOrdinal("detail_pdf"));
                    steDetails.StateName = reader.IsDBNull(reader.GetOrdinal("state_name")) ? "" : reader.GetString(reader.GetOrdinal("state_name"));

                    steDetails.CreatedDate = reader.IsDBNull(reader.GetOrdinal("created_date")) ? DateTime.Now : reader.GetDateTime(reader.GetOrdinal("created_date"));
                    steDetails.AssetTypeId = reader.IsDBNull(reader.GetOrdinal("asset_type_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("asset_type_id"));
                    steDetails.AssetTypeName = reader.IsDBNull(reader.GetOrdinal("asset_type_name")) ? "" : reader.GetString(reader.GetOrdinal("asset_type_name"));
                    steDetails.IsShoppingCenter = reader.IsDBNull(reader.GetOrdinal("is_shopping_center")) ? false : reader.GetBoolean(reader.GetOrdinal("is_shopping_center"));
                    if (steDetails.IsShoppingCenter)
                    {
                        steDetails.ShoppingCenterOrNetlease = "Shopping Center";
                    }
                    else
                    {
                        steDetails.ShoppingCenterOrNetlease = "Net Lease";
                    }

                    steDetails.Address = reader.IsDBNull(reader.GetOrdinal("property_address")) ? "" : reader.GetString(reader.GetOrdinal("property_address"));
                    steDetails.ZipCode = reader.IsDBNull(reader.GetOrdinal("property_zipcode")) ? "" : reader.GetString(reader.GetOrdinal("property_zipcode"));

                    steDetails.SelectedPropertyStatusId = reader.IsDBNull(reader.GetOrdinal("property_status_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("property_status_id"));
                    steDetails.SelectedPropertyStatus = reader.IsDBNull(reader.GetOrdinal("property_status")) ? "" : reader.GetString(reader.GetOrdinal("property_status"));

                    if (steDetails.Address.Length > 15)
                    {
                        steDetails.AddressShort = steDetails.Address.Substring(0, 15) + "..";
                    }
                    else
                    {
                        steDetails.AddressShort = steDetails.Address;
                    }

                    netLeasePropertiesList.Add(steDetails);
                }
                con.Close();

                SqlCommand cmdSurplus = new SqlCommand("ShoppingCenterNotificationList", con);
                cmdSurplus.CommandType = CommandType.StoredProcedure;
                con.Open();
                SqlDataReader readerAssetType = cmdSurplus.ExecuteReader();
                dashboardModel.ShoppingCenterNotificationList = SamsNotificationController.CreateNotificationList(readerAssetType);
                con.Close();

                SqlCommand cmdScNotifications = new SqlCommand("GetShoppingCenterClientNotifications", con);
                cmdScNotifications.CommandType = CommandType.StoredProcedure;

                con.Open();

                dashboardModel.TenantCriticalItemList = new List<TenantCriticalDateModel>();
                SqlDataReader readerScNotifications = cmdScNotifications.ExecuteReader();
                while (readerScNotifications.Read())
                {
                    var tenantCriticalDate = new TenantCriticalDateModel();

                    tenantCriticalDate.ShoppingCenterClientId = readerScNotifications.IsDBNull(readerScNotifications.GetOrdinal("shopping_center_client_id")) ? 0 : readerScNotifications.GetInt32(readerScNotifications.GetOrdinal("shopping_center_client_id"));
                    tenantCriticalDate.ShoppingCenterId = readerScNotifications.IsDBNull(readerScNotifications.GetOrdinal("c_store_id")) ? 0 : readerScNotifications.GetInt32(readerScNotifications.GetOrdinal("c_store_id"));
                    tenantCriticalDate.UnitSelected = readerScNotifications.IsDBNull(readerScNotifications.GetOrdinal("unit_selected")) ? "" : readerScNotifications.GetString(readerScNotifications.GetOrdinal("unit_selected"));
                    tenantCriticalDate.TenantName = readerScNotifications.IsDBNull(readerScNotifications.GetOrdinal("tenant_name")) ? "" : readerScNotifications.GetString(readerScNotifications.GetOrdinal("tenant_name"));
                    tenantCriticalDate.DateRentChanged = readerScNotifications.IsDBNull(readerScNotifications.GetOrdinal("date_rent_changed")) ? default(DateTime?) : readerScNotifications.GetDateTime(readerScNotifications.GetOrdinal("date_rent_changed"));
                    tenantCriticalDate.CoiExpire = readerScNotifications.IsDBNull(readerScNotifications.GetOrdinal("coi_expire")) ? default(DateTime?) : readerScNotifications.GetDateTime(readerScNotifications.GetOrdinal("coi_expire"));

                    dashboardModel.TenantCriticalItemList.Add(tenantCriticalDate);
                }
                con.Close();
            }

            dashboardModel.NetLeasePropertiesList = netLeasePropertiesList;
            

            return View(dashboardModel);
        }

        public IActionResult GetSoldoutShoppingCenters()
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

            List<NetleasePropertiesViewModel> netLeasePropertiesList = new List<NetleasePropertiesViewModel>();

            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("GetNetleaseShoppingCenterList", con);
                cmd.Parameters.AddWithValue("asset_status", 1);

                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var steDetails = new NetleasePropertiesViewModel();
                    steDetails.NetleasePropertyId = reader.IsDBNull(reader.GetOrdinal("net_lease_property_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("net_lease_property_id"));
                    steDetails.AssetId = reader.IsDBNull(reader.GetOrdinal("asset_id")) ? "" : reader.GetString(reader.GetOrdinal("asset_id"));
                    steDetails.AssetName = reader.IsDBNull(reader.GetOrdinal("asset_name")) ? "" : reader.GetString(reader.GetOrdinal("asset_name"));
                    steDetails.StateId = reader.IsDBNull(reader.GetOrdinal("state_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("state_id"));
                    steDetails.City = reader.IsDBNull(reader.GetOrdinal("city")) ? "" : reader.GetString(reader.GetOrdinal("city"));

                    steDetails.PropertyPrice = reader.IsDBNull(reader.GetOrdinal("property_price")) ? "" : reader.GetString(reader.GetOrdinal("property_price"));
                    steDetails.CapRate = reader.IsDBNull(reader.GetOrdinal("cap_rate")) ? 0 : reader.GetDouble(reader.GetOrdinal("cap_rate"));

                    steDetails.Term = reader.IsDBNull(reader.GetOrdinal("term")) ? "" : reader.GetString(reader.GetOrdinal("term"));

                    steDetails.PdfFileName = reader.IsDBNull(reader.GetOrdinal("detail_pdf")) ? "" : reader.GetString(reader.GetOrdinal("detail_pdf"));
                    steDetails.StateName = reader.IsDBNull(reader.GetOrdinal("state_name")) ? "" : reader.GetString(reader.GetOrdinal("state_name"));

                    steDetails.CreatedDate = reader.IsDBNull(reader.GetOrdinal("created_date")) ? DateTime.Now : reader.GetDateTime(reader.GetOrdinal("created_date"));
                    steDetails.AssetTypeId = reader.IsDBNull(reader.GetOrdinal("asset_type_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("asset_type_id"));
                    steDetails.AssetTypeName = reader.IsDBNull(reader.GetOrdinal("asset_type_name")) ? "" : reader.GetString(reader.GetOrdinal("asset_type_name"));
                    steDetails.IsShoppingCenter = reader.IsDBNull(reader.GetOrdinal("is_shopping_center")) ? false : reader.GetBoolean(reader.GetOrdinal("is_shopping_center"));
                    if (steDetails.IsShoppingCenter)
                    {
                        steDetails.ShoppingCenterOrNetlease = "Shopping Center";
                    }
                    else
                    {
                        steDetails.ShoppingCenterOrNetlease = "Net Lease";
                    }

                    steDetails.Address = reader.IsDBNull(reader.GetOrdinal("property_address")) ? "" : reader.GetString(reader.GetOrdinal("property_address"));
                    steDetails.ZipCode = reader.IsDBNull(reader.GetOrdinal("property_zipcode")) ? "" : reader.GetString(reader.GetOrdinal("property_zipcode"));

                    if (steDetails.Address.Length > 15)
                    {
                        steDetails.AddressShort = steDetails.Address.Substring(0, 15) + "..";
                    }
                    else
                    {
                        steDetails.AddressShort = steDetails.Address;
                    }

                    netLeasePropertiesList.Add(steDetails);
                }
                con.Close();
            }

            return View(netLeasePropertiesList);
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
                cmd.Parameters.AddWithValue("propertyType", SamsPropertyType.NetLease);
                con.Open();

                cmd.ExecuteNonQuery();

                con.Close();

                return RedirectToAction("ViewNetLeaseProperties", new { propertyId = PropertyId });
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
            var filePath = Path.Combine(webHostEnvironment.WebRootPath + @"/templates", "net_lease_properties_template.xlsx");

            string fullFileName = Path.Combine(webHostEnvironment.WebRootPath + @"/templates", "net_lease_properties_template.xlsx");
            string fullToFileName = "net_lease_properties" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xlsx";

            Workbook wrkBook = new Workbook();
            wrkBook.LoadFromFile(fullFileName);
            Worksheet sheet = wrkBook.Worksheets[0];


            string CS = DBConnection.ConnectionString;

            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("GetNetleasePropertyList", con);
                cmd.Parameters.AddWithValue("asset_status", 0);

                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                int i = 5;

                string colAssetId = "A", colAssetName = "B", colAddress = "C", colCity = "D", colState = "E", colZipcode = "F";
                string colPrice = "G", colCapRate = "H", colStatus = "I", colBuyer = "J", colSellingPrice = "K";
                string colTenant = "L", colRent = "M", colUnderControlDate = "N", colDdp = "O", colClosingDate = "P";
                string colDaysToClose = "Q";

                while (reader.Read())
                {
                    string cellAssetId = colAssetId + i.ToString();
                    string cellAssetName = colAssetName + i.ToString();
                    string cellAddress = colAddress + i.ToString();
                    string cellCity = colCity + i.ToString();

                    string cellState = colState + i.ToString();
                    string cellZipcode = colZipcode + i.ToString();
                    string cellPrice = colPrice + i.ToString();
                    string cellCapRate = colCapRate + i.ToString();
                    string cellStatus = colStatus + i.ToString();
                    string cellBuyer = colBuyer + i.ToString();

                    string cellSellingPrice= colSellingPrice + i.ToString();
                    string cellTenant = colTenant + i.ToString();
                    string cellRent = colRent + i.ToString();
                    string cellUnderContractDate = colUnderControlDate + i.ToString();

                    string cellDdp = colDdp + i.ToString();
                    string cellClosingDate = colClosingDate + i.ToString();

                    string cellDaysToClose = colDaysToClose + i.ToString();

                    var dtClosedDate = "";
                    int? daysToClose = null;

                    var steDetails = new NetleasePropertiesViewModel();


                    steDetails.NetleasePropertyId = reader.IsDBNull(reader.GetOrdinal("net_lease_property_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("net_lease_property_id"));
                    steDetails.AssetId = reader.IsDBNull(reader.GetOrdinal("asset_id")) ? "" : reader.GetString(reader.GetOrdinal("asset_id"));
                    steDetails.AssetName = reader.IsDBNull(reader.GetOrdinal("asset_name")) ? "" : reader.GetString(reader.GetOrdinal("asset_name"));
                    steDetails.NetleaseAssetName = reader.IsDBNull(reader.GetOrdinal("netlease_asset_name")) ? "" : reader.GetString(reader.GetOrdinal("netlease_asset_name"));
                    steDetails.StateId = reader.IsDBNull(reader.GetOrdinal("state_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("state_id"));
                    steDetails.City = reader.IsDBNull(reader.GetOrdinal("city")) ? "" : reader.GetString(reader.GetOrdinal("city"));

                    steDetails.PropertyPrice = reader.IsDBNull(reader.GetOrdinal("property_price")) ? "" : reader.GetString(reader.GetOrdinal("property_price"));
                    steDetails.CapRate = reader.IsDBNull(reader.GetOrdinal("cap_rate")) ? 0 : reader.GetDouble(reader.GetOrdinal("cap_rate"));

                    steDetails.Term = reader.IsDBNull(reader.GetOrdinal("term")) ? "" : reader.GetString(reader.GetOrdinal("term"));

                    steDetails.PdfFileName = reader.IsDBNull(reader.GetOrdinal("detail_pdf")) ? "" : reader.GetString(reader.GetOrdinal("detail_pdf"));
                    steDetails.StateName = reader.IsDBNull(reader.GetOrdinal("state_name")) ? "" : reader.GetString(reader.GetOrdinal("state_name"));

                    steDetails.CreatedDate = reader.IsDBNull(reader.GetOrdinal("created_date")) ? DateTime.Now : reader.GetDateTime(reader.GetOrdinal("created_date"));
                    steDetails.AssetTypeId = reader.IsDBNull(reader.GetOrdinal("asset_type_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("asset_type_id"));
                    steDetails.AssetTypeName = reader.IsDBNull(reader.GetOrdinal("asset_type_name")) ? "" : reader.GetString(reader.GetOrdinal("asset_type_name"));
                    steDetails.IsShoppingCenter = reader.IsDBNull(reader.GetOrdinal("is_shopping_center")) ? false : reader.GetBoolean(reader.GetOrdinal("is_shopping_center"));
                    if (steDetails.IsShoppingCenter)
                    {
                        steDetails.ShoppingCenterOrNetlease = "Shopping Center";
                    }
                    else
                    {
                        steDetails.ShoppingCenterOrNetlease = "Net Lease";
                    }
                    steDetails.Address = reader.IsDBNull(reader.GetOrdinal("property_address")) ? "" : reader.GetString(reader.GetOrdinal("property_address"));
                    steDetails.ZipCode = reader.IsDBNull(reader.GetOrdinal("property_zipcode")) ? "" : reader.GetString(reader.GetOrdinal("property_zipcode"));

                    steDetails.SelectedPropertyStatusId = reader.IsDBNull(reader.GetOrdinal("property_status_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("property_status_id"));
                    steDetails.SelectedPropertyStatus = reader.IsDBNull(reader.GetOrdinal("property_status")) ? "" : reader.GetString(reader.GetOrdinal("property_status"));

                    steDetails.ListingPrice = reader.IsDBNull(reader.GetOrdinal("listing_price")) ? "" : reader.GetString(reader.GetOrdinal("listing_price"));

                    if (steDetails.Address.Length > 15)
                    {
                        steDetails.AddressShort = steDetails.Address.Substring(0, 15) + "..";
                    }
                    else
                    {
                        steDetails.AddressShort = steDetails.Address;
                    }


                    steDetails.TransactionStatusName = "";

                    steDetails.DiligenceAcquisitions = GetDiligenceAcquisition(steDetails.NetleasePropertyId);
                    steDetails.DiligenceDispositionList = GetDiligenceDispositions(steDetails.NetleasePropertyId);
                    steDetails.DiligenceLeaseList = GetDiligenceLease(steDetails.NetleasePropertyId);

                    steDetails.DispositionPeriodList = GetPeriodList(steDetails.NetleasePropertyId, "Disposition");
                    steDetails.LeasePeriodList = GetPeriodList(steDetails.NetleasePropertyId, "Lease");
                    steDetails.LeaseTypeList = GetLeaseTypeList();
                    steDetails.FutureTenantList = GetFutureTenantList(steDetails.NetleasePropertyId);

                    DateTime? transactionClosedDate = default(DateTime?);

                    steDetails.DiligenceDispositions = new DiligenceDispositionsViewModel();

                    int saleLoi = 0, saleUnderContract = 0, saleTerminated = 0, saleClosed = 0;
                    
                    string strDate = "";

                    if (steDetails.AssetTypeId == (int)SamAssetType.Fee || steDetails.AssetTypeId == (int)SamAssetType.FeeSubjectToLease)
                    {
                        steDetails.SelectedDiligenceDisposition = new DiligenceDispositionsViewModel();
                        foreach (DiligenceDispositionsViewModel ddm in steDetails.DiligenceDispositionList)
                        {
                            if (ddm.SelectedTransactionStatusId == (int)TransactionStatus.Closed_Acquisitions || ddm.SelectedTransactionStatusId == (int)TransactionStatus.Under_Contract || ddm.SelectedTransactionStatusId == (int)TransactionStatus.Under_LOI)
                            {
                                steDetails.CanAddTransactions = false;
                            }

                            if (steDetails.MaxPriorityTransactionStatusId == 0)
                            {
                                steDetails.MaxPriorityTransactionStatusId = ddm.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = ddm.SelectedTransactionStatusName;

                                steDetails.SelectedDiligenceDisposition = ddm;
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

                                steDetails.SelectedDiligenceDisposition = ddm;
                                transactionClosedDate = ddm.ClosingDate;
                            }

                            if (ddm.SelectedTransactionStatusId == (int)SamsTransactionStatus.Under_LOI &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Under_Contract) &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Closed_Dispositions))
                            {
                                steDetails.MaxPriorityTransactionStatusId = ddm.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = ddm.SelectedTransactionStatusName;
                                steDetails.StatusChangedDate = ddm.SelectedTransactionDate;

                                steDetails.SelectedDiligenceDisposition = ddm;
                                transactionClosedDate = ddm.ClosingDate;
                            }

                            if (ddm.SelectedTransactionStatusId == (int)SamsTransactionStatus.Under_Contract &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Closed_Dispositions))
                            {
                                steDetails.MaxPriorityTransactionStatusId = ddm.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = ddm.SelectedTransactionStatusName;
                                steDetails.StatusChangedDate = ddm.SelectedTransactionDate;

                                steDetails.SelectedDiligenceDisposition = ddm;
                                transactionClosedDate = ddm.ClosingDate;
                            }

                            if (ddm.SelectedTransactionStatusId == (int)SamsTransactionStatus.Closed_Dispositions)
                            {
                                steDetails.MaxPriorityTransactionStatusId = ddm.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = ddm.SelectedTransactionStatusName;
                                steDetails.StatusChangedDate = ddm.SelectedTransactionDate;
                                steDetails.SelectedDiligenceDisposition = ddm;
                                transactionClosedDate = ddm.ClosingDate;
                            }
                        }

                        sheet.Range[cellBuyer].Value = steDetails.SelectedDiligenceDisposition.Buyers;
                        sheet.Range[cellSellingPrice].Value = steDetails.SelectedDiligenceDisposition.SalePrice;

                        strDate = steDetails.SelectedDiligenceDisposition.UnderContractDate == default(DateTime?) ? "" : steDetails.SelectedDiligenceDisposition.UnderContractDate.Value.ToString("MM/dd/yyyy");
                        if (strDate.Trim().Length > 3)
                        {
                            sheet.Range[cellUnderContractDate].Value = strDate;
                        }

                        strDate = steDetails.SelectedDiligenceDisposition.DueDiligenceExpairyDate == default(DateTime?) ? "" : steDetails.SelectedDiligenceDisposition.DueDiligenceExpairyDate.Value.ToString("MM/dd/yyyy");
                        if (strDate.Trim().Length > 3)
                        {
                            sheet.Range[cellDdp].Value = strDate;
                        }

                        
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
                            sheet.Range[cellClosingDate].Value = dtClosedDate;
                        }

                        if (daysToClose != null)
                        {
                            sheet.Range[cellDaysToClose].Value = daysToClose.ToString();
                        }

                    }
                    else if (steDetails.AssetTypeId == (int)SamAssetType.Lease)
                    {

                        steDetails.DiligenceLease = new DiligenceLeaseViewModel();
                        foreach (DiligenceLeaseViewModel dl in steDetails.DiligenceLeaseList)
                        {
                            

                            if (steDetails.MaxPriorityTransactionStatusId == 0)
                            {
                                steDetails.MaxPriorityTransactionStatusId = dl.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = dl.SelectedTransactionStatusName;

                                steDetails.DiligenceLease = dl;
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

                                steDetails.DiligenceLease = dl;
                                transactionClosedDate = dl.ClosingDate;
                            }

                            if (dl.SelectedTransactionStatusId == (int)SamsTransactionStatus.Under_LOI &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Under_Contract) &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Closed_Dispositions))
                            {
                                steDetails.MaxPriorityTransactionStatusId = dl.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = dl.SelectedTransactionStatusName;
                                steDetails.StatusChangedDate = dl.SelectedTransactionDate;

                                steDetails.DiligenceLease = dl;
                                transactionClosedDate = dl.ClosingDate;
                            }

                            if (dl.SelectedTransactionStatusId == (int)SamsTransactionStatus.Under_Contract &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Closed_Dispositions))
                            {
                                steDetails.MaxPriorityTransactionStatusId = dl.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = dl.SelectedTransactionStatusName;
                                steDetails.StatusChangedDate = dl.SelectedTransactionDate;

                                steDetails.DiligenceLease = dl;
                                transactionClosedDate = dl.ClosingDate;
                            }

                            if (dl.SelectedTransactionStatusId == (int)SamsTransactionStatus.Closed_Dispositions)
                            {
                                steDetails.MaxPriorityTransactionStatusId = dl.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = dl.SelectedTransactionStatusName;
                                steDetails.StatusChangedDate = dl.SelectedTransactionDate;
                                transactionClosedDate = dl.ClosingDate;
                                steDetails.DiligenceLease = dl;
                            }
                        }

                        sheet.Range[cellTenant].Value = steDetails.DiligenceLease.Tenant;
                        sheet.Range[cellRent].Value = steDetails.DiligenceLease.Rent;
                        sheet.Range[cellUnderContractDate].Value = steDetails.DiligenceLease.UnderContractDate == default(DateTime?) ? "" : steDetails.DiligenceLease.UnderContractDate.Value.ToString("MM/dd/yyyy");

                        sheet.Range[cellDdp].Value = steDetails.DiligenceLease.DueDiligenceExpiryDate == default(DateTime?) ? "" : steDetails.DiligenceLease.DueDiligenceExpiryDate.Value.ToString("MM/dd/yyyy");

                        
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
                            sheet.Range[cellClosingDate].Value = dtClosedDate;
                        }
                        if (daysToClose != null)
                        {
                            sheet.Range[cellDaysToClose].Value = daysToClose.ToString();
                        }

                    }
                    else if (steDetails.AssetTypeId == (int)SamAssetType.NetLease)
                    {
                        steDetails.DiligenceNetleaseList = GetDiligenceNetleaseList(steDetails.NetleasePropertyId);
                        steDetails.SelectedDiligenceNetlease = new DiligenceNetleaseViewModel();

                        
                        foreach (DiligenceNetleaseViewModel dl in steDetails.DiligenceNetleaseList)
                        {
                            if (dl.SelectedTransactionStatusId == (int)TransactionStatus.Closed_Acquisitions || dl.SelectedTransactionStatusId == (int)TransactionStatus.Under_Contract || dl.SelectedTransactionStatusId == (int)TransactionStatus.Under_LOI)
                            {
                                steDetails.CanAddTransactions = false;
                            }

                            

                            if (steDetails.MaxPriorityTransactionStatusId == 0)
                            {
                                steDetails.MaxPriorityTransactionStatusId = dl.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = dl.SelectedTransactionStatusName;

                                steDetails.SelectedDiligenceNetlease = dl;
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

                                steDetails.SelectedDiligenceNetlease = dl;
                                transactionClosedDate = dl.ClosingDate;
                            }

                            if (dl.SelectedTransactionStatusId == (int)SamsTransactionStatus.Under_LOI &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Under_Contract) &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Closed_Dispositions))
                            {
                                steDetails.MaxPriorityTransactionStatusId = dl.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = dl.SelectedTransactionStatusName;

                                steDetails.SelectedDiligenceNetlease = dl;
                                transactionClosedDate = dl.ClosingDate;
                            }

                            if (dl.SelectedTransactionStatusId == (int)SamsTransactionStatus.Under_Contract &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Closed_Dispositions))
                            {
                                steDetails.MaxPriorityTransactionStatusId = dl.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = dl.SelectedTransactionStatusName;

                                steDetails.SelectedDiligenceNetlease = dl;
                                transactionClosedDate = dl.ClosingDate;
                            }

                            if (dl.SelectedTransactionStatusId == (int)SamsTransactionStatus.Closed_Dispositions)
                            {
                                steDetails.MaxPriorityTransactionStatusId = dl.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = dl.SelectedTransactionStatusName;
                                transactionClosedDate = dl.ClosingDate;
                                steDetails.SelectedDiligenceNetlease = dl;
                            }
                        }

                        sheet.Range[cellBuyer].Value = steDetails.SelectedDiligenceNetlease.Buyers;
                        sheet.Range[cellSellingPrice].Value = steDetails.SelectedDiligenceNetlease.SalePrice;

                        sheet.Range[cellTenant].Value = steDetails.SelectedDiligenceNetlease.Tenant;
                        sheet.Range[cellRent].Value = steDetails.SelectedDiligenceNetlease.TenantRent;
                        sheet.Range[cellUnderContractDate].Value = steDetails.SelectedDiligenceNetlease.UnderContractDate == default(DateTime?) ? "" : steDetails.SelectedDiligenceNetlease.UnderContractDate.Value.ToString("MM/dd/yyyy");

                        //sheet.Range[cellDdp].Value = steDetails.SelectedDiligenceNetlease.DueDiligenceExpiryDate == default(DateTime?) ? "" : steDetails.SelectedDiligenceNetlease.DueDiligenceExpiryDate.Value.ToString("MM/dd/yyyy");


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
                            sheet.Range[cellClosingDate].Value = dtClosedDate;
                        }
                        if (daysToClose != null)
                        {
                            sheet.Range[cellDaysToClose].Value = daysToClose.ToString();
                        }

                    }
                    else if (steDetails.AssetTypeId == (int)SamAssetType.LeaseWithPurchaseOption)
                    {
                        steDetails.DiligenceLeaseWithPurchaseList = GetDiligenceLeaseWithPurchaseList(steDetails.NetleasePropertyId);
                        steDetails.DiligenceLeaseWithPurchase = new DiligenceLeaseWithPurchaseViewModel();

                        int leaseLoi = 0, leaseUnderContract = 0, leaseTerminated = 0, leaseClosed = 0;
                        foreach (DiligenceLeaseWithPurchaseViewModel dl in steDetails.DiligenceLeaseWithPurchaseList)
                        {
                            
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
                                transactionClosedDate = dl.ClosingDate;
                                steDetails.DiligenceLeaseWithPurchase = dl;
                            }
                        }

                        sheet.Range[cellTenant].Value = steDetails.DiligenceLeaseWithPurchase.Tenant;
                        sheet.Range[cellRent].Value = steDetails.DiligenceLeaseWithPurchase.Rent;
                        sheet.Range[cellUnderContractDate].Value = steDetails.DiligenceLeaseWithPurchase.UnderContractDate == default(DateTime?) ? "" : steDetails.DiligenceLeaseWithPurchase.UnderContractDate.Value.ToString("MM/dd/yyyy");

                        sheet.Range[cellDdp].Value = steDetails.DiligenceLeaseWithPurchase.DueDiligenceExpiryDate == default(DateTime?) ? "" : steDetails.DiligenceLeaseWithPurchase.DueDiligenceExpiryDate.Value.ToString("MM/dd/yyyy");

                        
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
                            sheet.Range[cellClosingDate].Value = dtClosedDate;
                        }
                        if (daysToClose != null)
                        {
                            sheet.Range[cellDaysToClose].Value = daysToClose.ToString();
                        }
                    }
                    else if (steDetails.AssetTypeId == (int)SamAssetType.SaleLeaseBack)
                    {
                        steDetails.DiligenceDispositions_SaleLeaseBack = GetDiligenceDispositions_SaleLeaseBack(steDetails.NetleasePropertyId);



                        foreach (DiligenceDispositionsViewModel ddm in steDetails.DiligenceDispositions_SaleLeaseBack)
                        {
                            if (ddm.SelectedTransactionStatusId == (int)TransactionStatus.Closed_Acquisitions || ddm.SelectedTransactionStatusId == (int)TransactionStatus.Under_Contract || ddm.SelectedTransactionStatusId == (int)TransactionStatus.Under_LOI)
                            {
                                steDetails.CanAddTransactions = false;
                            }

                            steDetails.SelectedDiligenceDisposition = ddm;
                            transactionClosedDate = ddm.ClosingDate;

                            if (steDetails.MaxPriorityTransactionStatusId == 0)
                            {
                                steDetails.MaxPriorityTransactionStatusId = ddm.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = ddm.SelectedTransactionStatusName;
                                

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

                                steDetails.SelectedDiligenceDisposition = ddm;
                                transactionClosedDate = ddm.ClosingDate;
                            }

                            if (ddm.SelectedTransactionStatusId == (int)SamsTransactionStatus.Under_LOI &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Under_Contract) &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Closed_Dispositions))
                            {
                                steDetails.MaxPriorityTransactionStatusId = ddm.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = ddm.SelectedTransactionStatusName;
                                steDetails.StatusChangedDate = ddm.SelectedTransactionDate;

                                steDetails.SelectedDiligenceDisposition = ddm;
                                transactionClosedDate = ddm.ClosingDate;
                            }

                            if (ddm.SelectedTransactionStatusId == (int)SamsTransactionStatus.Under_Contract &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Closed_Dispositions))
                            {
                                steDetails.MaxPriorityTransactionStatusId = ddm.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = ddm.SelectedTransactionStatusName;
                                steDetails.StatusChangedDate = ddm.SelectedTransactionDate;

                                steDetails.SelectedDiligenceDisposition = ddm;
                                transactionClosedDate = ddm.ClosingDate;
                            }

                            if (ddm.SelectedTransactionStatusId == (int)SamsTransactionStatus.Closed_Dispositions)
                            {
                                steDetails.MaxPriorityTransactionStatusId = ddm.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = ddm.SelectedTransactionStatusName;
                                steDetails.StatusChangedDate = ddm.SelectedTransactionDate;
                                transactionClosedDate = ddm.ClosingDate;
                                steDetails.SelectedDiligenceDisposition = ddm;
                            }
                        }

                        if (steDetails.SelectedDiligenceDisposition != null)
                        {
                            sheet.Range[cellBuyer].Value = steDetails.SelectedDiligenceDisposition.Buyers;
                            sheet.Range[cellSellingPrice].Value = steDetails.SelectedDiligenceDisposition.SalePrice;
                            sheet.Range[cellUnderContractDate].Value = steDetails.SelectedDiligenceDisposition.UnderContractDate == default(DateTime?) ? "" : steDetails.SelectedDiligenceDisposition.UnderContractDate.Value.ToString("MM/dd/yyyy");

                            sheet.Range[cellDdp].Value = steDetails.SelectedDiligenceDisposition.DueDiligenceExpairyDate == default(DateTime?) ? "" : steDetails.SelectedDiligenceDisposition.DueDiligenceExpairyDate.Value.ToString("MM/dd/yyyy");


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
                                sheet.Range[cellClosingDate].Value = dtClosedDate;
                            }
                            if (daysToClose != null)
                            {
                                sheet.Range[cellDaysToClose].Value = daysToClose.ToString();
                            }
                        }
                        
                    }



                        sheet.Range[cellAssetId].Value = steDetails.AssetId;

                    sheet.Range[cellAssetName].Value = steDetails.NetleaseAssetName;

                    List<string> addressStings = steDetails.Address.Split(',').ToList<string>();
                    if (addressStings.Count > 0)
                    {
                        sheet.Range[cellAddress].Value = addressStings[0];
                    }

                    sheet.Range[cellCity].Value = steDetails.City;
                    sheet.Range[cellState].Value = steDetails.StateName;
                    sheet.Range[cellZipcode].Value = steDetails.ZipCode;

                    sheet.Range[cellPrice].Value = steDetails.ListingPrice;
                    sheet.Range[cellCapRate].Value = steDetails.CapRate.ToString();
                    sheet.Range[cellStatus].Value = steDetails.SelectedPropertyStatus;

                    sheet.Range[cellUnderContractDate].NumberFormat = "mm-dd-yyyy;@";
                    sheet.Range[cellDdp].NumberFormat = "mm-dd-yyyy;@";
                    sheet.Range[cellClosingDate].NumberFormat = "mm-dd-yyyy;@";


                    i = i + 1;
                }
                con.Close();
                sheet.Range["A5:R" + i.ToString()].BorderInside(LineStyleType.Thin, Color.Black);
                sheet.Range["A5:R" + i.ToString()].BorderAround(LineStyleType.Thin, Color.Black);
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
                SqlCommand cmd = new SqlCommand("GetNetleaseDashboard", con);
                cmd.CommandType = CommandType.StoredProcedure;

                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    string dataType = reader.IsDBNull(reader.GetOrdinal("data_type")) ? "" : reader.GetString(reader.GetOrdinal("data_type"));
                    int totalRecords = reader.IsDBNull(reader.GetOrdinal("totalRecores")) ? 0 : reader.GetInt32(reader.GetOrdinal("totalRecores"));

                    switch (dataType)
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

                SqlCommand cmdPeriod = new SqlCommand("GetNetLeasePropertyPeriod", con);
                cmdPeriod.CommandType = CommandType.StoredProcedure;

                con.Open();

                surplusPropertiesDashboard.LatestPeriodList = new List<PeriodViewModel>();
                SqlDataReader readerPeriod = cmdPeriod.ExecuteReader();
                while (readerPeriod.Read())
                {
                    var period = new PeriodViewModel();

                    period.PeriodId = readerPeriod.IsDBNull(readerPeriod.GetOrdinal("period_id")) ? 0 : readerPeriod.GetInt32(readerPeriod.GetOrdinal("period_id"));
                    period.PropertyId = readerPeriod.IsDBNull(readerPeriod.GetOrdinal("property_id")) ? 0 : readerPeriod.GetInt32(readerPeriod.GetOrdinal("property_id"));
                    period.PeriodMaster = readerPeriod.IsDBNull(readerPeriod.GetOrdinal("period_master")) ? "" : readerPeriod.GetString(readerPeriod.GetOrdinal("period_master"));
                    period.StartDate = readerPeriod.IsDBNull(readerPeriod.GetOrdinal("start_date")) ? DateTime.Now : readerPeriod.GetDateTime(readerPeriod.GetOrdinal("start_date"));
                    period.EndDate = readerPeriod.IsDBNull(readerPeriod.GetOrdinal("end_date")) ? DateTime.Now : readerPeriod.GetDateTime(readerPeriod.GetOrdinal("end_date"));
                    period.PeriodNotes = readerPeriod.IsDBNull(readerPeriod.GetOrdinal("period_master")) ? "" : readerPeriod.GetString(readerPeriod.GetOrdinal("period_master"));
                    period.AssetId = readerPeriod.IsDBNull(readerPeriod.GetOrdinal("asset_id")) ? "" : readerPeriod.GetString(readerPeriod.GetOrdinal("asset_id"));

                    period.AlertDate = readerPeriod.IsDBNull(readerPeriod.GetOrdinal("alert_date")) ? default(DateTime?) : readerPeriod.GetDateTime(readerPeriod.GetOrdinal("alert_date"));
                    period.OtherEmailAddress = readerPeriod.IsDBNull(readerPeriod.GetOrdinal("other_email_address")) ? "" : readerPeriod.GetString(readerPeriod.GetOrdinal("other_email_address"));

                    surplusPropertiesDashboard.LatestPeriodList.Add(period);
                }
                con.Close();



                

                surplusPropertiesDashboard.SearchedNetleaseList = new List<NetleasePropertiesViewModel>();

                if (s != null && s != "all")
                {
                    using (SqlConnection con1 = new SqlConnection(CS))
                    {
                        List<NetleasePropertiesViewModel> netLeasePropertiesList = new List<NetleasePropertiesViewModel>();
                        SqlCommand cmd1 = new SqlCommand("SearchNetleasePropertyList", con1);
                        cmd1.CommandType = CommandType.StoredProcedure;
                        cmd1.Parameters.AddWithValue("searchString", s);
                        con1.Open();

                        SqlDataReader reader1 = cmd1.ExecuteReader();
                        while (reader1.Read())
                        {
                            var steDetails = new NetleasePropertiesViewModel();
                            steDetails.NetleasePropertyId = reader1.IsDBNull(reader1.GetOrdinal("net_lease_property_id")) ? 0 : reader1.GetInt32(reader1.GetOrdinal("net_lease_property_id"));
                            steDetails.AssetId = reader1.IsDBNull(reader1.GetOrdinal("asset_id")) ? "" : reader1.GetString(reader1.GetOrdinal("asset_id"));
                            steDetails.AssetName = reader1.IsDBNull(reader1.GetOrdinal("asset_name")) ? "" : reader1.GetString(reader1.GetOrdinal("asset_name"));
                            steDetails.StateId = reader1.IsDBNull(reader1.GetOrdinal("state_id")) ? 0 : reader1.GetInt32(reader1.GetOrdinal("state_id"));
                            steDetails.City = reader1.IsDBNull(reader1.GetOrdinal("city")) ? "" : reader1.GetString(reader1.GetOrdinal("city"));

                            steDetails.PropertyPrice = reader1.IsDBNull(reader1.GetOrdinal("property_price")) ? "" : reader1.GetString(reader1.GetOrdinal("property_price"));
                            steDetails.CapRate = reader1.IsDBNull(reader1.GetOrdinal("cap_rate")) ? 0 : reader1.GetDouble(reader1.GetOrdinal("cap_rate"));

                            steDetails.Term = reader1.IsDBNull(reader1.GetOrdinal("term")) ? "" : reader1.GetString(reader1.GetOrdinal("term"));

                            steDetails.PdfFileName = reader1.IsDBNull(reader1.GetOrdinal("detail_pdf")) ? "" : reader1.GetString(reader1.GetOrdinal("detail_pdf"));
                            steDetails.StateName = reader1.IsDBNull(reader1.GetOrdinal("state_name")) ? "" : reader1.GetString(reader1.GetOrdinal("state_name"));

                            steDetails.CreatedDate = reader1.IsDBNull(reader1.GetOrdinal("created_date")) ? DateTime.Now : reader1.GetDateTime(reader1.GetOrdinal("created_date"));
                            steDetails.AssetTypeId = reader1.IsDBNull(reader1.GetOrdinal("asset_type_id")) ? 0 : reader1.GetInt32(reader1.GetOrdinal("asset_type_id"));
                            steDetails.AssetTypeName = reader1.IsDBNull(reader1.GetOrdinal("asset_type_name")) ? "" : reader1.GetString(reader1.GetOrdinal("asset_type_name"));
                            steDetails.IsShoppingCenter = reader1.IsDBNull(reader1.GetOrdinal("is_shopping_center")) ? false : reader1.GetBoolean(reader1.GetOrdinal("is_shopping_center"));

                            steDetails.Address = reader1.IsDBNull(reader1.GetOrdinal("property_address")) ? "" : reader1.GetString(reader1.GetOrdinal("property_address"));
                            steDetails.ZipCode = reader1.IsDBNull(reader1.GetOrdinal("property_zipcode")) ? "" : reader1.GetString(reader1.GetOrdinal("property_zipcode"));

                            steDetails.SelectedPropertyStatusId = reader1.IsDBNull(reader1.GetOrdinal("property_status_id")) ? 0 : reader1.GetInt32(reader1.GetOrdinal("property_status_id"));
                            steDetails.SelectedPropertyStatus = reader1.IsDBNull(reader1.GetOrdinal("property_status")) ? "" : reader1.GetString(reader1.GetOrdinal("property_status"));

                            if (steDetails.Address.Length > 15)
                            {
                                steDetails.AddressShort = steDetails.Address.Substring(0, 15) + "..";
                            }
                            else
                            {
                                steDetails.AddressShort = steDetails.Address;
                            }

                            netLeasePropertiesList.Add(steDetails);
                        }
                        con1.Close();
                        surplusPropertiesDashboard.SearchedNetleaseList = netLeasePropertiesList;
                    }
                }
                con.Close();



                surplusPropertiesDashboard.TotalLoi = GetTotalCountByTransactionStatus((int)SamsTransactionStatus.Under_LOI);
                surplusPropertiesDashboard.TotalUnderContract = GetTotalCountByTransactionStatus((int)SamsTransactionStatus.Under_Contract);
                surplusPropertiesDashboard.TotalClosed = GetTotalCountByTransactionStatus((int)SamsTransactionStatus.Closed_Dispositions);
                surplusPropertiesDashboard.TotalTerminated = GetTotalCountByTransactionStatus((int)SamsTransactionStatus.Terminated_Dispositions);



                SqlCommand cmdSurplus = new SqlCommand("NetLeaseNotificationList", con);
                cmdSurplus.CommandType = CommandType.StoredProcedure;
                con.Open();
                SqlDataReader readerAssetType = cmdSurplus.ExecuteReader();
                surplusPropertiesDashboard.SurplusNotificationList = SamsNotificationController.CreateNotificationList(readerAssetType);
                con.Close();

                SqlCommand cmdSurplusListing = new SqlCommand("NetLeaseListingExpiry", con);
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
                SqlCommand cmd = new SqlCommand("GetNetLeaseAcquisitionList", con);
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



                    int propertyId = reader.IsDBNull(reader.GetOrdinal("net_lease_property_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("net_lease_property_id"));
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
                SqlCommand cmd = new SqlCommand("GetNetLeaseDispositionList", con);
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




                    int propertyId = reader.IsDBNull(reader.GetOrdinal("net_lease_property_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("net_lease_property_id"));
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
                SqlCommand cmd = new SqlCommand("GetNetLeasePropertyPeriod", con);
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




                    int propertyId = reader.IsDBNull(reader.GetOrdinal("net_lease_property_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("net_lease_property_id"));
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
                SqlCommand cmd = new SqlCommand("GetNetLeaseAcquisitionList", con);
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





                    int propertyId = reader.IsDBNull(reader.GetOrdinal("net_lease_property_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("net_lease_property_id"));
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
                SqlCommand cmd = new SqlCommand("GetNetLeaseAcquisitionList", con);
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





                    int propertyId = reader.IsDBNull(reader.GetOrdinal("net_lease_property_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("net_lease_property_id"));
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
                SqlCommand cmd = new SqlCommand("GetNetLeaseDispositionList", con);
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




                    int propertyId = reader.IsDBNull(reader.GetOrdinal("net_lease_property_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("net_lease_property_id"));
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
                SqlCommand cmd = new SqlCommand("GetNetLeaseDispositionList", con);
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




                    int propertyId = reader.IsDBNull(reader.GetOrdinal("net_lease_property_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("net_lease_property_id"));
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
                cmd.Parameters.AddWithValue("propertyType", SamsPropertyType.NetLease);
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
        public IActionResult SaveFutureTenant(FutureTenantModel futureTenant)
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
                SqlCommand cmd = new SqlCommand("SaveFutureTenant", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                cmd.Parameters.AddWithValue("future_tenent_id", futureTenant.FutureTenantId);
                cmd.Parameters.AddWithValue("netlease_id", futureTenant.NetLeaseId);

                cmd.Parameters.AddWithValue("tenant_name", futureTenant.Tenant);
                cmd.Parameters.AddWithValue("tenant_unit", futureTenant.Unit);
                cmd.Parameters.AddWithValue("term", futureTenant.Term);
                cmd.Parameters.AddWithValue("rent", futureTenant.Rent);
                cmd.Parameters.AddWithValue("cam", futureTenant.CAM);
                cmd.Parameters.AddWithValue("under_contract_date", futureTenant.UnderContractDate);
                cmd.Parameters.AddWithValue("ddp", futureTenant.DDP);
                cmd.Parameters.AddWithValue("tenant_upfit_concession", futureTenant.TenantUpfitConcession);

                cmd.Parameters.AddWithValue("rent_free_period", futureTenant.RentFreePeriod);
                cmd.Parameters.AddWithValue("lease_commencement_date", futureTenant.LeaseCommencementDate);
                cmd.Parameters.AddWithValue("lease_expiration_date", futureTenant.LeaseExpirationDate);
                cmd.Parameters.AddWithValue("lease_options", futureTenant.LeaseOptions);
                cmd.Parameters.AddWithValue("rent_escalation", futureTenant.RentEscalation);
                cmd.Parameters.AddWithValue("tenant_attorney", futureTenant.TenantAttorney);
                cmd.Parameters.AddWithValue("tenant_agent_commission", futureTenant.TenantAgentCommission);
                cmd.Parameters.AddWithValue("landlord_agent_commission", futureTenant.LandlordAgentCommission);

                cmd.Parameters.AddWithValue("lease_security_deposit", futureTenant.LeaseSecurityDeposit);
                cmd.Parameters.AddWithValue("free_rent_description", futureTenant.FreeRentPeriodDescription);
                cmd.Parameters.AddWithValue("selected_transaction_status_id", futureTenant.TransactionStatusId);
                cmd.Parameters.AddWithValue("lease_date", futureTenant.LeaseDate);

                cmd.ExecuteNonQuery();

                con.Close();
            }
            // return RedirectToAction("ViewCStore", new { propertyId = shoppingCenterClient.CStoreId });

            return RedirectToAction("ViewNetLeaseProperties", new { propertyId = futureTenant.NetLeaseId });
        }

        public IActionResult GetFutureTenantById(int futureTenantId, int netLeaseId)
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

            var futureTenant = new FutureTenantModel();
            futureTenant.NetLeaseId = netLeaseId;
            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("GetFutureTenantById", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("future_tenent_id", futureTenantId);

                con.Open();

                SqlDataReader readerAssetType = cmd.ExecuteReader();
                while (readerAssetType.Read())
                {
                    futureTenant.FutureTenantId = futureTenantId;
                    
                    futureTenant.Tenant = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("tenant_name")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("tenant_name"));

                    futureTenant.Unit = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("tenant_unit")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("tenant_unit"));
                    futureTenant.Term = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("term")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("term"));
                    futureTenant.Rent = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("rent")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("rent"));
                    futureTenant.CAM = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("cam")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("cam"));
                    futureTenant.UnderContractDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("under_contract_date")) ? default(DateTime?) : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("under_contract_date"));

                    futureTenant.DDP = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("ddp")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("ddp"));
                    futureTenant.TenantUpfitConcession = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("tenant_upfit_concession")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("tenant_upfit_concession"));
                    futureTenant.RentFreePeriod = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("rent_free_period")) ? 0 : readerAssetType.GetInt32(readerAssetType.GetOrdinal("rent_free_period"));

                    futureTenant.LeaseCommencementDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("lease_commencement_date")) ? default(DateTime?) : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("lease_commencement_date"));
                    futureTenant.LeaseExpirationDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("lease_expiration_date")) ? default(DateTime?) : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("lease_expiration_date"));
                    futureTenant.LeaseOptions = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("lease_options")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("lease_options"));
                    

                    futureTenant.RentEscalation = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("rent_escalation")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("rent_escalation"));
                    futureTenant.TenantAttorney = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("tenant_attorney")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("tenant_attorney"));
                    futureTenant.TenantAgentCommission = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("tenant_agent_commission")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("tenant_agent_commission"));
                    futureTenant.LandlordAgentCommission = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("landlord_agent_commission")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("landlord_agent_commission"));
                    futureTenant.LeaseSecurityDeposit = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("lease_security_deposit")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("lease_security_deposit"));
                    futureTenant.FreeRentPeriodDescription = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("free_rent_description")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("free_rent_description"));
                    futureTenant.LeaseDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("lease_date")) ? default(DateTime?) : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("lease_date"));

                }

                con.Close();

            }

            return View(futureTenant);
        }

        List<FutureTenantModel> GetFutureTenantList(int netLeaseId)
        {
            var futureTenantList = new List<FutureTenantModel>();

            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("GetFutureTenantList", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("netlease_id", netLeaseId);

                con.Open();

                SqlDataReader readerAssetType = cmd.ExecuteReader();
                while (readerAssetType.Read())
                {
                    var futureTenant = new FutureTenantModel();
                    futureTenant.FutureTenantId = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("future_tenent_id")) ? 0 : readerAssetType.GetInt32(readerAssetType.GetOrdinal("future_tenent_id"));
                    futureTenant.NetLeaseId = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("netlease_id")) ? 0 : readerAssetType.GetInt32(readerAssetType.GetOrdinal("netlease_id"));
                    futureTenant.Tenant = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("tenant_name")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("tenant_name"));

                    futureTenant.Unit = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("tenant_unit")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("tenant_unit"));
                    futureTenant.Term = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("term")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("term"));
                    futureTenant.Rent = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("rent")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("rent"));
                    futureTenant.CAM = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("cam")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("cam"));
                    futureTenant.UnderContractDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("under_contract_date")) ? default(DateTime?) : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("under_contract_date"));

                    futureTenant.DDP = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("ddp")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("ddp"));
                    futureTenant.TenantUpfitConcession = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("tenant_upfit_concession")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("tenant_upfit_concession"));
                    futureTenant.RentFreePeriod = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("rent_free_period")) ? 0 : readerAssetType.GetInt32(readerAssetType.GetOrdinal("rent_free_period"));

                    futureTenant.LeaseCommencementDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("lease_commencement_date")) ? default(DateTime?) : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("lease_commencement_date"));
                    futureTenant.LeaseExpirationDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("lease_expiration_date")) ? default(DateTime?) : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("lease_expiration_date"));
                    futureTenant.LeaseOptions = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("lease_options")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("lease_options"));


                    futureTenant.RentEscalation = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("rent_escalation")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("rent_escalation"));
                    futureTenant.TenantAttorney = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("tenant_attorney")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("tenant_attorney"));
                    futureTenant.TenantAgentCommission = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("tenant_agent_commission")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("tenant_agent_commission"));
                    futureTenant.LandlordAgentCommission = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("landlord_agent_commission")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("landlord_agent_commission"));
                    futureTenant.LeaseSecurityDeposit = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("lease_security_deposit")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("lease_security_deposit"));

                    futureTenantList.Add(futureTenant);
                }

                con.Close();

            }

            return futureTenantList;
        }

        public IActionResult DeleteFutureTenant(int futureTenantId, int netLeaseId)
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

            var futureTenant = new FutureTenantModel();
            futureTenant.NetLeaseId = netLeaseId;
            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("DeleteFutureTenant", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("future_tenent_id", futureTenantId);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

            }

            return RedirectToAction("ViewNetLeaseProperties", new { propertyId = futureTenant.NetLeaseId });
        }
        
        public IActionResult GetFutureTenantCriticalDateList(int futureTenantId, int netleaseId)
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

            var futureTenantCriticalDateList = new List<FutureTenantCriticalDateModel>();

            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("GetAllFutureTenantCriticalDates", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("future_tenant_id", futureTenantId);
                con.Open();

                SqlDataReader readerAssetType = cmd.ExecuteReader();
                while (readerAssetType.Read())
                {
                    var futureTenantCriticalDate = new FutureTenantCriticalDateModel();

                    futureTenantCriticalDate.CriticalDateId = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("critical_date_id")) ? 0 : readerAssetType.GetInt32(readerAssetType.GetOrdinal("critical_date_id"));
                    futureTenantCriticalDate.FutureTenantId = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("future_tenant_id")) ? 0 : readerAssetType.GetInt32(readerAssetType.GetOrdinal("future_tenant_id"));

                    futureTenantCriticalDate.CriticalDateMaster = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("critical_date_master")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("critical_date_master"));

                    futureTenantCriticalDate.StartDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("start_date")) ? DateTime.Now : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("start_date"));
                    futureTenantCriticalDate.EndDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("end_date")) ? DateTime.Now : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("end_date"));
                    futureTenantCriticalDate.CriticalDateNotes = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("critical_date_notes")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("critical_date_notes"));

                    futureTenantCriticalDateList.Add(futureTenantCriticalDate);
                }

                con.Close();

            }
            ViewBag.NetleaseId = netleaseId;
            ViewBag.FutureTenantId = futureTenantId;
            return View(futureTenantCriticalDateList);
        }


        public IActionResult SaveFutureTenantCriticalDate(FutureTenantCriticalDateModel futureTenantCriticalDate)
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

            var futureTenantCriticalDateList = new List<FutureTenantCriticalDateModel>();

            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("SaveFutureTenantCriticalDate", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("critical_date_id", futureTenantCriticalDate.CriticalDateId);
                cmd.Parameters.AddWithValue("critical_date_master", futureTenantCriticalDate.CriticalDateMaster);
                cmd.Parameters.AddWithValue("start_date", futureTenantCriticalDate.StartDate);

                DateTime endDate = futureTenantCriticalDate.StartDate.AddDays(futureTenantCriticalDate.AddedDuration);

                cmd.Parameters.AddWithValue("end_date", endDate);
                cmd.Parameters.AddWithValue("critical_date_notes", futureTenantCriticalDate.CriticalDateNotes);
                cmd.Parameters.AddWithValue("future_tenant_id", futureTenantCriticalDate.FutureTenantId);
                con.Open();

                cmd.ExecuteNonQuery();
                con.Close();
            }
            ViewBag.NetleaseId = futureTenantCriticalDate.NetleasePropertyId;
            ViewBag.FutureTenantId = futureTenantCriticalDate.FutureTenantId;
            if (futureTenantCriticalDate.IsFromNetLease == 1)
            {
                return RedirectToAction("GetFutureTenantCriticalDateList", new { futureTenantId = futureTenantCriticalDate.FutureTenantId, netleaseId = futureTenantCriticalDate.NetleasePropertyId });
            }
            else
            {
                return RedirectToAction("GetFutureTenantByIdOnTransaction", new { futureTenantId = futureTenantCriticalDate.FutureTenantId, netLeaseId = futureTenantCriticalDate.NetleasePropertyId });
            }
        }


        public IActionResult DeleteFutureTenantCriticalDate(int criticalDateId, int futureTenantId, int netleaseId)
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
                SqlCommand cmd = new SqlCommand("DeleteFutureTenantCriticalDates", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("critical_date_id", criticalDateId);
                con.Open();

                cmd.ExecuteNonQuery();
                con.Close();
            }
            ViewBag.NetleaseId = netleaseId;
            ViewBag.FutureTenantId = futureTenantId;

            return RedirectToAction("GetFutureTenantByIdOnTransaction", new { futureTenantId = futureTenantId, netLeaseId = netleaseId });
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
                SqlCommand cmd = new SqlCommand("HideNetLeasePropertyNotification", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                cmd.Parameters.AddWithValue("net_lease_property_id", propertyId);

                cmd.ExecuteNonQuery();


                con.Close();
                return RedirectToAction("Dashboard");
            }
        }

        public RedirectToActionResult HideTenantNotification(int shoppingCenterClientId)
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
                SqlCommand cmd = new SqlCommand("HideTenantNotification", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                cmd.Parameters.AddWithValue("shopping_center_client_id", shoppingCenterClientId);

                cmd.ExecuteNonQuery();


                con.Close();
                return RedirectToAction("GetShoppingCenterList");
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
                cmd.Parameters.AddWithValue("property_type", SamsPropertyType.NetLease);
                con.Open();

                cmd.ExecuteNonQuery();

                con.Close();

                return RedirectToAction("ViewNetLeaseProperties", new { propertyId = PropertyId });

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
                cmd.Parameters.AddWithValue("property_type", SamsPropertyType.NetLease);
                cmd.Parameters.AddWithValue("can_publish", canPublish);
                con.Open();

                cmd.ExecuteNonQuery();

                con.Close();

                return RedirectToAction("ViewNetLeaseProperties", new { propertyId = PropertyId });
            }
        }

        /*
        public IActionResult GetDiligenceDispositionById(int diligenceDispositionId, int propertyId)
        {
            var ddpViewModel = new DiligenceDispositionsViewModel();
            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("GetDiligenceDispositionById", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("diligence_dispositions_id", diligenceDispositionId);
                con.Open();


                bool haveRecords = false;
                SqlDataReader readerAssetType = cmd.ExecuteReader();
                while (readerAssetType.Read())
                {

                    haveRecords = true;
                    ddpViewModel.PropertyId = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("property_id")) ? 0 : readerAssetType.GetInt32(readerAssetType.GetOrdinal("property_id")); ;
                    
                    ddpViewModel.DiligenceDispositionsId = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("diligence_dispositions_id")) ? 0 : readerAssetType.GetInt32(readerAssetType.GetOrdinal("diligence_dispositions_id"));

                    ddpViewModel.SalePrice = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("sale_price")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("sale_price"));
                    ddpViewModel.EarnestMoney = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("earnest_money")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("earnest_money"));

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
                    ddpViewModel.SelectedTransactionDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("selected_transaction_date")) ? DateTime.Now : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("selected_transaction_date"));
                }

                ddpViewModel.PropertyType = (int)SamsPropertyType.NetLease;
                if (!haveRecords)
                {
                    ddpViewModel.SelectedTransactionDate = DateTime.Now;
                }
                con.Close();

            }

            ViewData["propertyId"] = propertyId;
            ddpViewModel.TransactionStatusList = GetTransactionStatusList();
            return View(ddpViewModel);
        }
        */


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


                bool haveRecords = false;
                SqlDataReader readerAssetType = cmd.ExecuteReader();
                while (readerAssetType.Read())
                {

                    haveRecords = true;
                    ddpViewModel.PropertyId = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("property_id")) ? 0 : readerAssetType.GetInt32(readerAssetType.GetOrdinal("property_id")); ;

                    ddpViewModel.DiligenceDispositionsId = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("diligence_dispositions_id")) ? 0 : readerAssetType.GetInt32(readerAssetType.GetOrdinal("diligence_dispositions_id"));

                    ddpViewModel.SalePrice = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("sale_price")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("sale_price"));
                    ddpViewModel.EarnestMoney = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("earnest_money")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("earnest_money"));

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
                    ddpViewModel.SelectedTransactionDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("selected_transaction_date")) ? default(DateTime?) : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("selected_transaction_date"));
                    ddpViewModel.ClosingDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("closing_date")) ? default(DateTime?) : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("closing_date"));
                }

                ddpViewModel.PropertyType = (int)SamsPropertyType.NetLease;
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

                    var periodList = new List<PeriodViewModel>();

                    SqlCommand cmdPeriod = new SqlCommand("GetPeriodList", con);
                    cmdPeriod.CommandType = CommandType.StoredProcedure;
                    cmdPeriod.Parameters.AddWithValue("property_id", propertyId);
                    cmdPeriod.Parameters.AddWithValue("property_type", (int)SamsPropertyType.NetLease);
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
            }

            ViewData["propertyId"] = propertyId;
            ViewData["currentAssetStatusId"] = currentAssetStatusId;
            ddpViewModel.TransactionStatusList = GetTransactionStatusList(currentAssetStatusId, ddpViewModel.SelectedTransactionStatusId);
            return View(ddpViewModel);
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
                cmd.Parameters.AddWithValue("property_type", (int)SamsPropertyType.NetLease);
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

                    if (currentTransactionStatusId > 0)
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


            List<NetleasePropertiesViewModel> netLeasePropertiesList = new List<NetleasePropertiesViewModel>();

            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("GetNetleasePropertyList", con);
                cmd.Parameters.AddWithValue("asset_status", 0);

                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var steDetails = new NetleasePropertiesViewModel();
                    steDetails.NetleasePropertyId = reader.IsDBNull(reader.GetOrdinal("net_lease_property_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("net_lease_property_id"));
                    steDetails.AssetId = reader.IsDBNull(reader.GetOrdinal("asset_id")) ? "" : reader.GetString(reader.GetOrdinal("asset_id"));
                    steDetails.AssetName = reader.IsDBNull(reader.GetOrdinal("asset_name")) ? "" : reader.GetString(reader.GetOrdinal("asset_name"));
                    steDetails.StateId = reader.IsDBNull(reader.GetOrdinal("state_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("state_id"));
                    steDetails.City = reader.IsDBNull(reader.GetOrdinal("city")) ? "" : reader.GetString(reader.GetOrdinal("city"));

                    steDetails.PropertyPrice = reader.IsDBNull(reader.GetOrdinal("property_price")) ? "" : reader.GetString(reader.GetOrdinal("property_price"));
                    steDetails.CapRate = reader.IsDBNull(reader.GetOrdinal("cap_rate")) ? 0 : reader.GetDouble(reader.GetOrdinal("cap_rate"));

                    steDetails.Term = reader.IsDBNull(reader.GetOrdinal("term")) ? "" : reader.GetString(reader.GetOrdinal("term"));

                    steDetails.PdfFileName = reader.IsDBNull(reader.GetOrdinal("detail_pdf")) ? "" : reader.GetString(reader.GetOrdinal("detail_pdf"));
                    steDetails.StateName = reader.IsDBNull(reader.GetOrdinal("state_name")) ? "" : reader.GetString(reader.GetOrdinal("state_name"));

                    steDetails.CreatedDate = reader.IsDBNull(reader.GetOrdinal("created_date")) ? DateTime.Now : reader.GetDateTime(reader.GetOrdinal("created_date"));
                    steDetails.AssetTypeId = reader.IsDBNull(reader.GetOrdinal("asset_type_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("asset_type_id"));
                    steDetails.AssetTypeName = reader.IsDBNull(reader.GetOrdinal("asset_type_name")) ? "" : reader.GetString(reader.GetOrdinal("asset_type_name"));
                    steDetails.IsShoppingCenter = reader.IsDBNull(reader.GetOrdinal("is_shopping_center")) ? false : reader.GetBoolean(reader.GetOrdinal("is_shopping_center"));
                    if (steDetails.IsShoppingCenter)
                    {
                        steDetails.ShoppingCenterOrNetlease = "Shopping Center";
                    }
                    else
                    {
                        steDetails.ShoppingCenterOrNetlease = "Net Lease";
                    }
                    steDetails.Address = reader.IsDBNull(reader.GetOrdinal("property_address")) ? "" : reader.GetString(reader.GetOrdinal("property_address"));
                    steDetails.ZipCode = reader.IsDBNull(reader.GetOrdinal("property_zipcode")) ? "" : reader.GetString(reader.GetOrdinal("property_zipcode"));

                    steDetails.SelectedPropertyStatusId = reader.IsDBNull(reader.GetOrdinal("property_status_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("property_status_id"));
                    steDetails.SelectedPropertyStatus = reader.IsDBNull(reader.GetOrdinal("property_status")) ? "" : reader.GetString(reader.GetOrdinal("property_status"));

                    if (steDetails.Address.Length > 15)
                    {
                        steDetails.AddressShort = steDetails.Address.Substring(0, 15) + "..";
                    }
                    else
                    {
                        steDetails.AddressShort = steDetails.Address;
                    }


                    steDetails.TransactionStatusName = "";

                    steDetails.DiligenceDispositionList = GetDiligenceDispositions(steDetails.NetleasePropertyId);

                    steDetails.DiligenceAcquisitions = GetDiligenceAcquisition(steDetails.NetleasePropertyId);
                    steDetails.DiligenceDispositionList = GetDiligenceDispositions(steDetails.NetleasePropertyId);
                    steDetails.DiligenceLeaseList = GetDiligenceLease(steDetails.NetleasePropertyId);

                    steDetails.DispositionPeriodList = GetPeriodList(steDetails.NetleasePropertyId, "Disposition");
                    steDetails.LeasePeriodList = GetPeriodList(steDetails.NetleasePropertyId, "Lease");

                    steDetails.LeaseTypeList = GetLeaseTypeList();
                    steDetails.FutureTenantList = GetFutureTenantList(steDetails.NetleasePropertyId);

                    steDetails.DiligenceDispositions = new DiligenceDispositionsViewModel();
                    steDetails.SelectedDiligenceDisposition = new DiligenceDispositionsViewModel();

                    DateTime? transactionClosedDate = default(DateTime?);

                    steDetails.DiligenceLease = new DiligenceLeaseViewModel();
                    steDetails.SelectedDiligenceDisposition = new DiligenceDispositionsViewModel();
                    steDetails.DiligenceLeaseWithPurchase = new DiligenceLeaseWithPurchaseViewModel();
                    steDetails.SelectedDiligenceNetlease = new DiligenceNetleaseViewModel();

                    int saleLoi = 0, saleUnderContract = 0, saleTerminated = 0, saleClosed = 0;

                    if (steDetails.AssetTypeId == (int)SamAssetType.Fee || steDetails.AssetTypeId == (int)SamAssetType.FeeSubjectToLease)
                    {
                        steDetails.SelectedDiligenceDisposition = new DiligenceDispositionsViewModel();
                        foreach (DiligenceDispositionsViewModel ddm in steDetails.DiligenceDispositionList)
                        {
                            if (ddm.SelectedTransactionStatusId == (int)TransactionStatus.Closed_Acquisitions || ddm.SelectedTransactionStatusId == (int)TransactionStatus.Under_Contract || ddm.SelectedTransactionStatusId == (int)TransactionStatus.Under_LOI)
                            {
                                steDetails.CanAddTransactions = false;
                            }

                            if (steDetails.MaxPriorityTransactionStatusId == 0)
                            {
                                steDetails.MaxPriorityTransactionStatusId = ddm.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = ddm.SelectedTransactionStatusName;

                                steDetails.SelectedDiligenceDisposition = ddm;
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

                                steDetails.SelectedDiligenceDisposition = ddm;
                                transactionClosedDate = ddm.ClosingDate;
                            }

                            if (ddm.SelectedTransactionStatusId == (int)SamsTransactionStatus.Under_LOI &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Under_Contract) &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Closed_Dispositions))
                            {
                                steDetails.MaxPriorityTransactionStatusId = ddm.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = ddm.SelectedTransactionStatusName;
                                steDetails.StatusChangedDate = ddm.SelectedTransactionDate;

                                steDetails.SelectedDiligenceDisposition = ddm;
                                transactionClosedDate = ddm.ClosingDate;
                            }

                            if (ddm.SelectedTransactionStatusId == (int)SamsTransactionStatus.Under_Contract &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Closed_Dispositions))
                            {
                                steDetails.MaxPriorityTransactionStatusId = ddm.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = ddm.SelectedTransactionStatusName;
                                steDetails.StatusChangedDate = ddm.SelectedTransactionDate;

                                steDetails.SelectedDiligenceDisposition = ddm;
                                transactionClosedDate = ddm.ClosingDate;
                            }

                            if (ddm.SelectedTransactionStatusId == (int)SamsTransactionStatus.Closed_Dispositions)
                            {
                                steDetails.MaxPriorityTransactionStatusId = ddm.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = ddm.SelectedTransactionStatusName;
                                steDetails.StatusChangedDate = ddm.SelectedTransactionDate;
                                steDetails.SelectedDiligenceDisposition = ddm;
                                transactionClosedDate = ddm.ClosingDate;
                            }
                        }

                    }
                    else if (steDetails.AssetTypeId == (int)SamAssetType.Lease)
                    {

                        foreach (DiligenceLeaseViewModel dl in steDetails.DiligenceLeaseList)
                        {

                            if (steDetails.MaxPriorityTransactionStatusId == 0)
                            {
                                steDetails.MaxPriorityTransactionStatusId = dl.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = dl.SelectedTransactionStatusName;

                                steDetails.DiligenceLease = dl;
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

                                steDetails.DiligenceLease = dl;
                                transactionClosedDate = dl.ClosingDate;
                            }

                            if (dl.SelectedTransactionStatusId == (int)SamsTransactionStatus.Under_LOI &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Under_Contract) &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Closed_Dispositions))
                            {
                                steDetails.MaxPriorityTransactionStatusId = dl.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = dl.SelectedTransactionStatusName;
                                steDetails.StatusChangedDate = dl.SelectedTransactionDate;

                                steDetails.DiligenceLease = dl;
                                transactionClosedDate = dl.ClosingDate;
                            }

                            if (dl.SelectedTransactionStatusId == (int)SamsTransactionStatus.Under_Contract &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Closed_Dispositions))
                            {
                                steDetails.MaxPriorityTransactionStatusId = dl.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = dl.SelectedTransactionStatusName;
                                steDetails.StatusChangedDate = dl.SelectedTransactionDate;

                                steDetails.DiligenceLease = dl;
                                transactionClosedDate = dl.ClosingDate;
                            }

                            if (dl.SelectedTransactionStatusId == (int)SamsTransactionStatus.Closed_Dispositions)
                            {
                                steDetails.MaxPriorityTransactionStatusId = dl.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = dl.SelectedTransactionStatusName;
                                steDetails.StatusChangedDate = dl.SelectedTransactionDate;
                                transactionClosedDate = dl.ClosingDate;
                                steDetails.DiligenceLease = dl;
                            }
                        }


                    }
                    else if (steDetails.AssetTypeId == (int)SamAssetType.NetLease)
                    {
                        steDetails.DiligenceNetleaseList = GetDiligenceNetleaseList(steDetails.NetleasePropertyId);
                        steDetails.SelectedDiligenceNetlease = new DiligenceNetleaseViewModel();


                        foreach (DiligenceNetleaseViewModel dl in steDetails.DiligenceNetleaseList)
                        {
                            if (dl.SelectedTransactionStatusId == (int)TransactionStatus.Closed_Acquisitions || dl.SelectedTransactionStatusId == (int)TransactionStatus.Under_Contract || dl.SelectedTransactionStatusId == (int)TransactionStatus.Under_LOI)
                            {
                                steDetails.CanAddTransactions = false;
                            }



                            if (steDetails.MaxPriorityTransactionStatusId == 0)
                            {
                                steDetails.MaxPriorityTransactionStatusId = dl.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = dl.SelectedTransactionStatusName;

                                steDetails.SelectedDiligenceNetlease = dl;
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

                                steDetails.SelectedDiligenceNetlease = dl;
                                transactionClosedDate = dl.ClosingDate;
                            }

                            if (dl.SelectedTransactionStatusId == (int)SamsTransactionStatus.Under_LOI &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Under_Contract) &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Closed_Dispositions))
                            {
                                steDetails.MaxPriorityTransactionStatusId = dl.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = dl.SelectedTransactionStatusName;

                                steDetails.SelectedDiligenceNetlease = dl;
                                transactionClosedDate = dl.ClosingDate;
                            }

                            if (dl.SelectedTransactionStatusId == (int)SamsTransactionStatus.Under_Contract &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Closed_Dispositions))
                            {
                                steDetails.MaxPriorityTransactionStatusId = dl.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = dl.SelectedTransactionStatusName;

                                steDetails.SelectedDiligenceNetlease = dl;
                                transactionClosedDate = dl.ClosingDate;
                            }

                            if (dl.SelectedTransactionStatusId == (int)SamsTransactionStatus.Closed_Dispositions)
                            {
                                steDetails.MaxPriorityTransactionStatusId = dl.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = dl.SelectedTransactionStatusName;
                                transactionClosedDate = dl.ClosingDate;
                                steDetails.SelectedDiligenceNetlease = dl;
                            }
                        }



                    }
                    else if (steDetails.AssetTypeId == (int)SamAssetType.LeaseWithPurchaseOption)
                    {
                        steDetails.DiligenceLeaseWithPurchaseList = GetDiligenceLeaseWithPurchaseList(steDetails.NetleasePropertyId);
                        steDetails.DiligenceLeaseWithPurchase = new DiligenceLeaseWithPurchaseViewModel();

                        int leaseLoi = 0, leaseUnderContract = 0, leaseTerminated = 0, leaseClosed = 0;
                        foreach (DiligenceLeaseWithPurchaseViewModel dl in steDetails.DiligenceLeaseWithPurchaseList)
                        {

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
                                transactionClosedDate = dl.ClosingDate;
                                steDetails.DiligenceLeaseWithPurchase = dl;
                            }
                        }


                    }
                    else if (steDetails.AssetTypeId == (int)SamAssetType.SaleLeaseBack)
                    {
                        steDetails.DiligenceDispositions_SaleLeaseBack = GetDiligenceDispositions_SaleLeaseBack(steDetails.NetleasePropertyId);



                        foreach (DiligenceDispositionsViewModel ddm in steDetails.DiligenceDispositions_SaleLeaseBack)
                        {
                            if (ddm.SelectedTransactionStatusId == (int)TransactionStatus.Closed_Acquisitions || ddm.SelectedTransactionStatusId == (int)TransactionStatus.Under_Contract || ddm.SelectedTransactionStatusId == (int)TransactionStatus.Under_LOI)
                            {
                                steDetails.CanAddTransactions = false;
                            }

                            steDetails.SelectedDiligenceDisposition = ddm;
                            transactionClosedDate = ddm.ClosingDate;

                            if (steDetails.MaxPriorityTransactionStatusId == 0)
                            {
                                steDetails.MaxPriorityTransactionStatusId = ddm.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = ddm.SelectedTransactionStatusName;


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

                                steDetails.SelectedDiligenceDisposition = ddm;
                                transactionClosedDate = ddm.ClosingDate;
                            }

                            if (ddm.SelectedTransactionStatusId == (int)SamsTransactionStatus.Under_LOI &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Under_Contract) &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Closed_Dispositions))
                            {
                                steDetails.MaxPriorityTransactionStatusId = ddm.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = ddm.SelectedTransactionStatusName;
                                steDetails.StatusChangedDate = ddm.SelectedTransactionDate;

                                steDetails.SelectedDiligenceDisposition = ddm;
                                transactionClosedDate = ddm.ClosingDate;
                            }

                            if (ddm.SelectedTransactionStatusId == (int)SamsTransactionStatus.Under_Contract &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Closed_Dispositions))
                            {
                                steDetails.MaxPriorityTransactionStatusId = ddm.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = ddm.SelectedTransactionStatusName;
                                steDetails.StatusChangedDate = ddm.SelectedTransactionDate;

                                steDetails.SelectedDiligenceDisposition = ddm;
                                transactionClosedDate = ddm.ClosingDate;
                            }

                            if (ddm.SelectedTransactionStatusId == (int)SamsTransactionStatus.Closed_Dispositions)
                            {
                                steDetails.MaxPriorityTransactionStatusId = ddm.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = ddm.SelectedTransactionStatusName;
                                steDetails.StatusChangedDate = ddm.SelectedTransactionDate;
                                transactionClosedDate = ddm.ClosingDate;
                                steDetails.SelectedDiligenceDisposition = ddm;
                            }
                        }



                    }

                    steDetails.TodoList = GetTodoList(steDetails.NetleasePropertyId);
                    StringBuilder todoText = new StringBuilder();
                    if (steDetails.TodoList.Count > 0)
                    {
                        foreach (TodoViewModel td in steDetails.TodoList)
                        {
                            todoText.Append(td.TodoText + "\r\n");
                        }
                    }
                    steDetails.LatestComment = todoText.ToString();

                    if (steDetails.MaxPriorityTransactionStatusId == transactionStatusId)
                    {
                        netLeasePropertiesList.Add(steDetails);
                    }

                }
                con.Close();
            }

            return View(netLeasePropertiesList);
        }


        int GetTotalCountByTransactionStatus(int transactionStatusId)
        {


            List<NetleasePropertiesViewModel> netLeasePropertiesList = new List<NetleasePropertiesViewModel>();

            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("GetNetleasePropertyList", con);
                cmd.Parameters.AddWithValue("asset_status", 0);

                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var steDetails = new NetleasePropertiesViewModel();
                    steDetails.NetleasePropertyId = reader.IsDBNull(reader.GetOrdinal("net_lease_property_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("net_lease_property_id"));
                    steDetails.AssetId = reader.IsDBNull(reader.GetOrdinal("asset_id")) ? "" : reader.GetString(reader.GetOrdinal("asset_id"));
                    steDetails.AssetName = reader.IsDBNull(reader.GetOrdinal("asset_name")) ? "" : reader.GetString(reader.GetOrdinal("asset_name"));
                    steDetails.StateId = reader.IsDBNull(reader.GetOrdinal("state_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("state_id"));
                    steDetails.City = reader.IsDBNull(reader.GetOrdinal("city")) ? "" : reader.GetString(reader.GetOrdinal("city"));

                    steDetails.PropertyPrice = reader.IsDBNull(reader.GetOrdinal("property_price")) ? "" : reader.GetString(reader.GetOrdinal("property_price"));
                    steDetails.CapRate = reader.IsDBNull(reader.GetOrdinal("cap_rate")) ? 0 : reader.GetDouble(reader.GetOrdinal("cap_rate"));

                    steDetails.Term = reader.IsDBNull(reader.GetOrdinal("term")) ? "" : reader.GetString(reader.GetOrdinal("term"));

                    steDetails.PdfFileName = reader.IsDBNull(reader.GetOrdinal("detail_pdf")) ? "" : reader.GetString(reader.GetOrdinal("detail_pdf"));
                    steDetails.StateName = reader.IsDBNull(reader.GetOrdinal("state_name")) ? "" : reader.GetString(reader.GetOrdinal("state_name"));

                    steDetails.CreatedDate = reader.IsDBNull(reader.GetOrdinal("created_date")) ? DateTime.Now : reader.GetDateTime(reader.GetOrdinal("created_date"));
                    steDetails.AssetTypeId = reader.IsDBNull(reader.GetOrdinal("asset_type_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("asset_type_id"));
                    steDetails.AssetTypeName = reader.IsDBNull(reader.GetOrdinal("asset_type_name")) ? "" : reader.GetString(reader.GetOrdinal("asset_type_name"));
                    steDetails.IsShoppingCenter = reader.IsDBNull(reader.GetOrdinal("is_shopping_center")) ? false : reader.GetBoolean(reader.GetOrdinal("is_shopping_center"));
                    if (steDetails.IsShoppingCenter)
                    {
                        steDetails.ShoppingCenterOrNetlease = "Shopping Center";
                    }
                    else
                    {
                        steDetails.ShoppingCenterOrNetlease = "Net Lease";
                    }
                    steDetails.Address = reader.IsDBNull(reader.GetOrdinal("property_address")) ? "" : reader.GetString(reader.GetOrdinal("property_address"));
                    steDetails.ZipCode = reader.IsDBNull(reader.GetOrdinal("property_zipcode")) ? "" : reader.GetString(reader.GetOrdinal("property_zipcode"));

                    steDetails.SelectedPropertyStatusId = reader.IsDBNull(reader.GetOrdinal("property_status_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("property_status_id"));
                    steDetails.SelectedPropertyStatus = reader.IsDBNull(reader.GetOrdinal("property_status")) ? "" : reader.GetString(reader.GetOrdinal("property_status"));

                    if (steDetails.Address.Length > 15)
                    {
                        steDetails.AddressShort = steDetails.Address.Substring(0, 15) + "..";
                    }
                    else
                    {
                        steDetails.AddressShort = steDetails.Address;
                    }


                    steDetails.TransactionStatusName = "";

                    steDetails.DiligenceDispositionList = GetDiligenceDispositions(steDetails.NetleasePropertyId);

                    steDetails.DiligenceAcquisitions = GetDiligenceAcquisition(steDetails.NetleasePropertyId);
                    steDetails.DiligenceDispositionList = GetDiligenceDispositions(steDetails.NetleasePropertyId);
                    steDetails.DiligenceLeaseList = GetDiligenceLease(steDetails.NetleasePropertyId);

                    steDetails.DispositionPeriodList = GetPeriodList(steDetails.NetleasePropertyId, "Disposition");
                    steDetails.LeasePeriodList = GetPeriodList(steDetails.NetleasePropertyId, "Lease");

                    steDetails.LeaseTypeList = GetLeaseTypeList();
                    steDetails.FutureTenantList = GetFutureTenantList(steDetails.NetleasePropertyId);

                    steDetails.DiligenceDispositions = new DiligenceDispositionsViewModel();
                    steDetails.SelectedDiligenceDisposition = new DiligenceDispositionsViewModel();

                    DateTime? transactionClosedDate = default(DateTime?);

                    steDetails.DiligenceLease = new DiligenceLeaseViewModel();
                    steDetails.SelectedDiligenceDisposition = new DiligenceDispositionsViewModel();
                    steDetails.DiligenceLeaseWithPurchase = new DiligenceLeaseWithPurchaseViewModel();
                    steDetails.SelectedDiligenceNetlease = new DiligenceNetleaseViewModel();

                    int saleLoi = 0, saleUnderContract = 0, saleTerminated = 0, saleClosed = 0;

                    if (steDetails.AssetTypeId == (int)SamAssetType.Fee || steDetails.AssetTypeId == (int)SamAssetType.FeeSubjectToLease)
                    {
                        steDetails.SelectedDiligenceDisposition = new DiligenceDispositionsViewModel();
                        foreach (DiligenceDispositionsViewModel ddm in steDetails.DiligenceDispositionList)
                        {
                            if (ddm.SelectedTransactionStatusId == (int)TransactionStatus.Closed_Acquisitions || ddm.SelectedTransactionStatusId == (int)TransactionStatus.Under_Contract || ddm.SelectedTransactionStatusId == (int)TransactionStatus.Under_LOI)
                            {
                                steDetails.CanAddTransactions = false;
                            }

                            if (steDetails.MaxPriorityTransactionStatusId == 0)
                            {
                                steDetails.MaxPriorityTransactionStatusId = ddm.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = ddm.SelectedTransactionStatusName;

                                steDetails.SelectedDiligenceDisposition = ddm;
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

                                steDetails.SelectedDiligenceDisposition = ddm;
                                transactionClosedDate = ddm.ClosingDate;
                            }

                            if (ddm.SelectedTransactionStatusId == (int)SamsTransactionStatus.Under_LOI &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Under_Contract) &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Closed_Dispositions))
                            {
                                steDetails.MaxPriorityTransactionStatusId = ddm.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = ddm.SelectedTransactionStatusName;
                                steDetails.StatusChangedDate = ddm.SelectedTransactionDate;

                                steDetails.SelectedDiligenceDisposition = ddm;
                                transactionClosedDate = ddm.ClosingDate;
                            }

                            if (ddm.SelectedTransactionStatusId == (int)SamsTransactionStatus.Under_Contract &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Closed_Dispositions))
                            {
                                steDetails.MaxPriorityTransactionStatusId = ddm.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = ddm.SelectedTransactionStatusName;
                                steDetails.StatusChangedDate = ddm.SelectedTransactionDate;

                                steDetails.SelectedDiligenceDisposition = ddm;
                                transactionClosedDate = ddm.ClosingDate;
                            }

                            if (ddm.SelectedTransactionStatusId == (int)SamsTransactionStatus.Closed_Dispositions)
                            {
                                steDetails.MaxPriorityTransactionStatusId = ddm.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = ddm.SelectedTransactionStatusName;
                                steDetails.StatusChangedDate = ddm.SelectedTransactionDate;
                                steDetails.SelectedDiligenceDisposition = ddm;
                                transactionClosedDate = ddm.ClosingDate;
                            }
                        }

                    }
                    else if (steDetails.AssetTypeId == (int)SamAssetType.Lease)
                    {

                        foreach (DiligenceLeaseViewModel dl in steDetails.DiligenceLeaseList)
                        {

                            if (steDetails.MaxPriorityTransactionStatusId == 0)
                            {
                                steDetails.MaxPriorityTransactionStatusId = dl.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = dl.SelectedTransactionStatusName;

                                steDetails.DiligenceLease = dl;
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

                                steDetails.DiligenceLease = dl;
                                transactionClosedDate = dl.ClosingDate;
                            }

                            if (dl.SelectedTransactionStatusId == (int)SamsTransactionStatus.Under_LOI &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Under_Contract) &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Closed_Dispositions))
                            {
                                steDetails.MaxPriorityTransactionStatusId = dl.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = dl.SelectedTransactionStatusName;
                                steDetails.StatusChangedDate = dl.SelectedTransactionDate;

                                steDetails.DiligenceLease = dl;
                                transactionClosedDate = dl.ClosingDate;
                            }

                            if (dl.SelectedTransactionStatusId == (int)SamsTransactionStatus.Under_Contract &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Closed_Dispositions))
                            {
                                steDetails.MaxPriorityTransactionStatusId = dl.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = dl.SelectedTransactionStatusName;
                                steDetails.StatusChangedDate = dl.SelectedTransactionDate;

                                steDetails.DiligenceLease = dl;
                                transactionClosedDate = dl.ClosingDate;
                            }

                            if (dl.SelectedTransactionStatusId == (int)SamsTransactionStatus.Closed_Dispositions)
                            {
                                steDetails.MaxPriorityTransactionStatusId = dl.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = dl.SelectedTransactionStatusName;
                                steDetails.StatusChangedDate = dl.SelectedTransactionDate;
                                transactionClosedDate = dl.ClosingDate;
                                steDetails.DiligenceLease = dl;
                            }
                        }


                    }
                    else if (steDetails.AssetTypeId == (int)SamAssetType.NetLease)
                    {
                        steDetails.DiligenceNetleaseList = GetDiligenceNetleaseList(steDetails.NetleasePropertyId);
                        steDetails.SelectedDiligenceNetlease = new DiligenceNetleaseViewModel();


                        foreach (DiligenceNetleaseViewModel dl in steDetails.DiligenceNetleaseList)
                        {
                            if (dl.SelectedTransactionStatusId == (int)TransactionStatus.Closed_Acquisitions || dl.SelectedTransactionStatusId == (int)TransactionStatus.Under_Contract || dl.SelectedTransactionStatusId == (int)TransactionStatus.Under_LOI)
                            {
                                steDetails.CanAddTransactions = false;
                            }



                            if (steDetails.MaxPriorityTransactionStatusId == 0)
                            {
                                steDetails.MaxPriorityTransactionStatusId = dl.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = dl.SelectedTransactionStatusName;

                                steDetails.SelectedDiligenceNetlease = dl;
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

                                steDetails.SelectedDiligenceNetlease = dl;
                                transactionClosedDate = dl.ClosingDate;
                            }

                            if (dl.SelectedTransactionStatusId == (int)SamsTransactionStatus.Under_LOI &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Under_Contract) &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Closed_Dispositions))
                            {
                                steDetails.MaxPriorityTransactionStatusId = dl.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = dl.SelectedTransactionStatusName;

                                steDetails.SelectedDiligenceNetlease = dl;
                                transactionClosedDate = dl.ClosingDate;
                            }

                            if (dl.SelectedTransactionStatusId == (int)SamsTransactionStatus.Under_Contract &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Closed_Dispositions))
                            {
                                steDetails.MaxPriorityTransactionStatusId = dl.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = dl.SelectedTransactionStatusName;

                                steDetails.SelectedDiligenceNetlease = dl;
                                transactionClosedDate = dl.ClosingDate;
                            }

                            if (dl.SelectedTransactionStatusId == (int)SamsTransactionStatus.Closed_Dispositions)
                            {
                                steDetails.MaxPriorityTransactionStatusId = dl.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = dl.SelectedTransactionStatusName;
                                transactionClosedDate = dl.ClosingDate;
                                steDetails.SelectedDiligenceNetlease = dl;
                            }
                        }



                    }
                    else if (steDetails.AssetTypeId == (int)SamAssetType.LeaseWithPurchaseOption)
                    {
                        steDetails.DiligenceLeaseWithPurchaseList = GetDiligenceLeaseWithPurchaseList(steDetails.NetleasePropertyId);
                        steDetails.DiligenceLeaseWithPurchase = new DiligenceLeaseWithPurchaseViewModel();

                        int leaseLoi = 0, leaseUnderContract = 0, leaseTerminated = 0, leaseClosed = 0;
                        foreach (DiligenceLeaseWithPurchaseViewModel dl in steDetails.DiligenceLeaseWithPurchaseList)
                        {

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
                                transactionClosedDate = dl.ClosingDate;
                                steDetails.DiligenceLeaseWithPurchase = dl;
                            }
                        }


                    }
                    else if (steDetails.AssetTypeId == (int)SamAssetType.SaleLeaseBack)
                    {
                        steDetails.DiligenceDispositions_SaleLeaseBack = GetDiligenceDispositions_SaleLeaseBack(steDetails.NetleasePropertyId);



                        foreach (DiligenceDispositionsViewModel ddm in steDetails.DiligenceDispositions_SaleLeaseBack)
                        {
                            if (ddm.SelectedTransactionStatusId == (int)TransactionStatus.Closed_Acquisitions || ddm.SelectedTransactionStatusId == (int)TransactionStatus.Under_Contract || ddm.SelectedTransactionStatusId == (int)TransactionStatus.Under_LOI)
                            {
                                steDetails.CanAddTransactions = false;
                            }

                            steDetails.SelectedDiligenceDisposition = ddm;
                            transactionClosedDate = ddm.ClosingDate;

                            if (steDetails.MaxPriorityTransactionStatusId == 0)
                            {
                                steDetails.MaxPriorityTransactionStatusId = ddm.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = ddm.SelectedTransactionStatusName;


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

                                steDetails.SelectedDiligenceDisposition = ddm;
                                transactionClosedDate = ddm.ClosingDate;
                            }

                            if (ddm.SelectedTransactionStatusId == (int)SamsTransactionStatus.Under_LOI &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Under_Contract) &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Closed_Dispositions))
                            {
                                steDetails.MaxPriorityTransactionStatusId = ddm.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = ddm.SelectedTransactionStatusName;
                                steDetails.StatusChangedDate = ddm.SelectedTransactionDate;

                                steDetails.SelectedDiligenceDisposition = ddm;
                                transactionClosedDate = ddm.ClosingDate;
                            }

                            if (ddm.SelectedTransactionStatusId == (int)SamsTransactionStatus.Under_Contract &&
                                (steDetails.MaxPriorityTransactionStatusId != (int)SamsTransactionStatus.Closed_Dispositions))
                            {
                                steDetails.MaxPriorityTransactionStatusId = ddm.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = ddm.SelectedTransactionStatusName;
                                steDetails.StatusChangedDate = ddm.SelectedTransactionDate;

                                steDetails.SelectedDiligenceDisposition = ddm;
                                transactionClosedDate = ddm.ClosingDate;
                            }

                            if (ddm.SelectedTransactionStatusId == (int)SamsTransactionStatus.Closed_Dispositions)
                            {
                                steDetails.MaxPriorityTransactionStatusId = ddm.SelectedTransactionStatusId;
                                steDetails.MaxPriorityTransactionStatusName = ddm.SelectedTransactionStatusName;
                                steDetails.StatusChangedDate = ddm.SelectedTransactionDate;
                                transactionClosedDate = ddm.ClosingDate;
                                steDetails.SelectedDiligenceDisposition = ddm;
                            }
                        }



                    }

                    steDetails.TodoList = GetTodoList(steDetails.NetleasePropertyId);
                    StringBuilder todoText = new StringBuilder();
                    if (steDetails.TodoList.Count > 0)
                    {
                        foreach (TodoViewModel td in steDetails.TodoList)
                        {
                            todoText.Append(td.TodoText + "\r\n");
                        }
                    }
                    steDetails.LatestComment = todoText.ToString();

                    if (steDetails.MaxPriorityTransactionStatusId == transactionStatusId)
                    {
                        netLeasePropertiesList.Add(steDetails);
                    }
                    
                }
                con.Close();
            }

            return netLeasePropertiesList.Count;
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
                cmd.Parameters.AddWithValue("property_type", (int)SamsPropertyType.NetLease);
                cmd.Parameters.AddWithValue("property_id", propertyId);
                cmd.CommandType = CommandType.StoredProcedure;

                con.Open();

                cmd.ExecuteNonQuery();

                con.Close();

            }
            return RedirectToAction("ViewNetLeaseProperties", new { propertyId = propertyId });
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
                cmd.Parameters.AddWithValue("property_type", (int)SamsPropertyType.NetLease);
                cmd.Parameters.AddWithValue("property_id", propertyId);
                cmd.CommandType = CommandType.StoredProcedure;

                con.Open();

                cmd.ExecuteNonQuery();


                con.Close();

            }
            return RedirectToAction("ViewNetLeaseProperties", new { propertyId = propertyId });
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
            return RedirectToAction("ViewNetLeaseProperties", new { propertyId = propertyId });
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

        public IActionResult GetDiligenceLeaseById(int diligenceLeaseId, int propertyId, int currentAssetStatusId, int assetTypeId)
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

            var diligenceLease = new DiligenceLeaseViewModel();
            diligenceLease.PropertyId = propertyId;
            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("GetDiligenceLeaseById", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("property_id", propertyId);
                cmd.Parameters.AddWithValue("property_type", SamsPropertyType.NetLease);
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

                    diligenceLease.CreatedDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("created_date")) ? DateTime.Now : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("created_date"));

                    diligenceLease.SelectedTransactionStatusId = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("selected_transaction_id")) ? 0 : readerAssetType.GetInt32(readerAssetType.GetOrdinal("selected_transaction_id"));
                    diligenceLease.SelectedTransactionDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("selected_transaction_date")) ? default(DateTime?) : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("selected_transaction_date"));
                    diligenceLease.LeaseCommencementDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("lease_commencement_date")) ? default(DateTime?) : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("lease_commencement_date"));

                    diligenceLease.ClosingDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("closing_date")) ? default(DateTime?) : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("closing_date"));
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

                    var periodList = new List<PeriodViewModel>();

                    SqlCommand cmdPeriod = new SqlCommand("GetPeriodList", con);
                    cmdPeriod.CommandType = CommandType.StoredProcedure;
                    cmdPeriod.Parameters.AddWithValue("property_id", propertyId);
                    cmdPeriod.Parameters.AddWithValue("property_type", (int)SamsPropertyType.NetLease);
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


                if (!haveRecords)
                {
                    //diligenceLease.SelectedTransactionDate = DateTime.Now;
                }
            }
            ViewData["propertyId"] = propertyId;
            ViewData["currentAssetStatusId"] = currentAssetStatusId;
            ViewData["assetTypeId"] = assetTypeId;
            diligenceLease.TransactionStatusList = GetTransactionStatusLeaseList();// GetTransactionStatusList(currentAssetStatusId, diligenceLease.SelectedTransactionStatusId);
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
                cmd.Parameters.AddWithValue("property_type", SamsPropertyType.NetLease);
                cmd.Parameters.AddWithValue("tenant_name", diligenceLease.Tenant);

                cmd.Parameters.AddWithValue("rent", diligenceLease.Rent);
                cmd.Parameters.AddWithValue("listing_price", diligenceLease.ListingPrice);
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

            return RedirectToAction("ViewNetLeaseProperties", new { propertyId = diligenceLease.PropertyId });
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



            return RedirectToAction("GetFutureTenantByIdOnTransaction", new { futureTenantId = uploadedFile.TransactionId, netLeaseId = uploadedFile.PropertyId });

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
                return RedirectToAction("GetFutureTenantByIdOnTransaction", new { futureTenantId = transactionId, netLeaseId = propertyId });
            }
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
                cmd.Parameters.AddWithValue("property_type", (int)SamsPropertyType.NetLease);
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





        /*
         *
         * Lease with Purchase option transaction
         *
         */
        List<DiligenceLeaseWithPurchaseViewModel> GetDiligenceLeaseWithPurchaseList(int propertyId)
        {
            var diligenceLeaseWithPurchaseList = new List<DiligenceLeaseWithPurchaseViewModel>();

            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("GetDiligenceLeaseWithPurchase", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("property_id", propertyId);
                cmd.Parameters.AddWithValue("property_type", SamsPropertyType.NetLease);
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
                cmd.Parameters.AddWithValue("property_type", SamsPropertyType.NetLease);
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
                cmdPeriod.Parameters.AddWithValue("property_type", (int)SamsPropertyType.NetLease);
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
                cmd.Parameters.AddWithValue("property_type", SamsPropertyType.NetLease);
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

            return RedirectToAction("ViewNetLeaseProperties", new { propertyId = diligenceLeaseWithPurchase.PropertyId });
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
                cmd.Parameters.AddWithValue("property_type", (int)SamsPropertyType.NetLease);
                cmd.Parameters.AddWithValue("property_id", propertyId);
                cmd.CommandType = CommandType.StoredProcedure;

                con.Open();

                cmd.ExecuteNonQuery();

                con.Close();

            }
            return RedirectToAction("ViewNetLeaseProperties", new { propertyId = propertyId });
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




        /**
         * Netlease transaction
         * */
        List<DiligenceNetleaseViewModel> GetDiligenceNetleaseList(int propertyId)
        {
            var diligenceDispositions = new List<DiligenceNetleaseViewModel>();

            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("GetDiligenceDispositions", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("property_id", propertyId);
                cmd.Parameters.AddWithValue("property_type", SamsPropertyType.NetLease);
                con.Open();



                SqlDataReader readerAssetType = cmd.ExecuteReader();
                while (readerAssetType.Read())
                {
                    var ddpViewModel = new DiligenceNetleaseViewModel();

                    ddpViewModel.PropertyId = propertyId;
                    ddpViewModel.PropertyType = (int)SamsPropertyType.NetLease;
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

                    ddpViewModel.Tenant = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("tenant")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("tenant"));
                    ddpViewModel.TenantRent = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("tenant_rent")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("tenant_rent"));

                    ddpViewModel.ClosingDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("closing_date")) ? default(DateTime?) : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("closing_date"));

                    diligenceDispositions.Add(ddpViewModel);
                }

                con.Close();

            }

            

            ViewData["propertyId"] = propertyId;
            return diligenceDispositions;
        }


        public IActionResult GetDiligenceNetleaseById(int diligenceDispositionId, int propertyId, int currentAssetStatusId)
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

            var ddpViewModel = new DiligenceNetleaseViewModel();
            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("GetDiligenceDispositionById", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("diligence_dispositions_id", diligenceDispositionId);
                con.Open();


                bool haveRecords = false;
                SqlDataReader readerAssetType = cmd.ExecuteReader();
                while (readerAssetType.Read())
                {

                    haveRecords = true;
                    ddpViewModel.PropertyId = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("property_id")) ? 0 : readerAssetType.GetInt32(readerAssetType.GetOrdinal("property_id")); ;

                    ddpViewModel.DiligenceDispositionsId = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("diligence_dispositions_id")) ? 0 : readerAssetType.GetInt32(readerAssetType.GetOrdinal("diligence_dispositions_id"));

                    ddpViewModel.SalePrice = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("sale_price")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("sale_price"));
                    ddpViewModel.EarnestMoney = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("earnest_money")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("earnest_money"));

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
                    ddpViewModel.SelectedTransactionDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("selected_transaction_date")) ? default(DateTime?) : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("selected_transaction_date"));

                    ddpViewModel.Tenant = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("tenant")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("tenant"));
                    ddpViewModel.TenantRent = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("tenant_rent")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("tenant_rent"));

                    ddpViewModel.ClosingDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("closing_date")) ? default(DateTime?) : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("closing_date"));
                }

                ddpViewModel.PropertyType = (int)SamsPropertyType.NetLease;
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
                    cmdGetTransactionFiles.Parameters.AddWithValue("transaction_type", TransactionType.Netlease);
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

                    var periodList = new List<PeriodViewModel>();

                    SqlCommand cmdPeriod = new SqlCommand("GetPeriodList", con);
                    cmdPeriod.CommandType = CommandType.StoredProcedure;
                    cmdPeriod.Parameters.AddWithValue("property_id", propertyId);
                    cmdPeriod.Parameters.AddWithValue("property_type", (int)SamsPropertyType.NetLease);
                    cmdPeriod.Parameters.AddWithValue("transaction_id", ddpViewModel.DiligenceDispositionsId);
                    cmdPeriod.Parameters.AddWithValue("period_type", "Netlease");
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
            }

            ViewData["propertyId"] = propertyId;
            ViewData["currentAssetStatusId"] = currentAssetStatusId;
            ddpViewModel.TransactionStatusList = GetTransactionStatusList(currentAssetStatusId, ddpViewModel.SelectedTransactionStatusId);
            return View(ddpViewModel);
        }

        public RedirectToActionResult DeleteNetleaseTransactionFile(int transactionFiled, int transactionId, int propertyId)
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
                return RedirectToAction("GetDiligenceNetleaseById", new { diligenceDispositionId = transactionId, propertyId = propertyId });
            }
        }

        [HttpPost]
        public RedirectToActionResult SaveNetleaseTransactionFile(TransactionFilesViewModel uploadedFile)
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
                cmd.Parameters.AddWithValue("transaction_type", TransactionType.Netlease);
                cmd.Parameters.AddWithValue("file_header", uploadedFile.FileHeader);
                cmd.Parameters.AddWithValue("file_name", actualFileName);
                cmd.Parameters.AddWithValue("file_full_path", uniqueFileName);
                cmd.Parameters.AddWithValue("notes", uploadedFile.Notes);
                cmd.Parameters.AddWithValue("uploaded_by", loggedInUser.UserId);

                cmd.ExecuteNonQuery();


                con.Close();
            }



            return RedirectToAction("GetDiligenceNetleaseById", new { diligenceDispositionId = uploadedFile.TransactionId, propertyId = uploadedFile.PropertyId });

        }


        public IActionResult GetFutureTenantByIdOnTransaction(int futureTenantId, int netLeaseId)
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

            var futureTenant = new FutureTenantModel();
            futureTenant.NetLeaseId = netLeaseId;
            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("GetFutureTenantById", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("future_tenent_id", futureTenantId);

                con.Open();

                SqlDataReader readerAssetType = cmd.ExecuteReader();
                while (readerAssetType.Read())
                {
                    futureTenant.FutureTenantId = futureTenantId;

                    futureTenant.Tenant = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("tenant_name")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("tenant_name"));

                    futureTenant.Unit = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("tenant_unit")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("tenant_unit"));
                    futureTenant.Term = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("term")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("term"));
                    futureTenant.Rent = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("rent")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("rent"));
                    futureTenant.CAM = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("cam")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("cam"));
                    futureTenant.UnderContractDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("under_contract_date")) ? default(DateTime?) : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("under_contract_date"));

                    futureTenant.DDP = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("ddp")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("ddp"));
                    futureTenant.TenantUpfitConcession = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("tenant_upfit_concession")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("tenant_upfit_concession"));
                    futureTenant.RentFreePeriod = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("rent_free_period")) ? 0 : readerAssetType.GetInt32(readerAssetType.GetOrdinal("rent_free_period"));

                    futureTenant.LeaseCommencementDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("lease_commencement_date")) ? default(DateTime?) : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("lease_commencement_date"));
                    futureTenant.LeaseExpirationDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("lease_expiration_date")) ? default(DateTime?) : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("lease_expiration_date"));
                    futureTenant.LeaseOptions = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("lease_options")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("lease_options"));


                    futureTenant.RentEscalation = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("rent_escalation")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("rent_escalation"));
                    futureTenant.TenantAttorney = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("tenant_attorney")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("tenant_attorney"));
                    futureTenant.TenantAgentCommission = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("tenant_agent_commission")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("tenant_agent_commission"));
                    futureTenant.LandlordAgentCommission = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("landlord_agent_commission")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("landlord_agent_commission"));
                    futureTenant.LeaseSecurityDeposit = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("lease_security_deposit")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("lease_security_deposit"));
                    futureTenant.FreeRentPeriodDescription = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("free_rent_description")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("free_rent_description"));

                    futureTenant.TransactionStatusId = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("selected_transaction_status_id")) ? 0 : readerAssetType.GetInt32(readerAssetType.GetOrdinal("selected_transaction_status_id"));
                    futureTenant.TransactionStatusName = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("transaction_status_name")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("transaction_status_name"));
                    futureTenant.LeaseDate = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("lease_date")) ? default(DateTime?) : readerAssetType.GetDateTime(readerAssetType.GetOrdinal("lease_date"));
                }

                con.Close();
                futureTenant.LeaseTransactionList = GetTransactionStatusLeaseList();

                futureTenant.TransactionFileList = new List<TransactionFilesViewModel>();
                futureTenant.TenantCriticalDates = new List<FutureTenantCriticalDateModel>();
                if (futureTenantId > 0)
                {
                    SqlCommand cmdGetTransactionFiles = new SqlCommand("getTransactionFiles", con);
                    cmdGetTransactionFiles.CommandType = CommandType.StoredProcedure;
                    cmdGetTransactionFiles.Parameters.AddWithValue("transaction_id", futureTenantId);
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

                        futureTenant.TransactionFileList.Add(transactionFiles);
                    }

                    con.Close();

                    SqlCommand cmdTenantCriticalDate = new SqlCommand("GetAllFutureTenantCriticalDates", con);
                    cmdTenantCriticalDate.CommandType = CommandType.StoredProcedure;
                    cmdTenantCriticalDate.Parameters.AddWithValue("future_tenant_id", futureTenantId);
                    con.Open();

                    SqlDataReader readerTenantCriticalDate = cmdTenantCriticalDate.ExecuteReader();
                    while (readerTenantCriticalDate.Read())
                    {
                        var futureTenantCriticalDate = new FutureTenantCriticalDateModel();

                        futureTenantCriticalDate.CriticalDateId = readerTenantCriticalDate.IsDBNull(readerTenantCriticalDate.GetOrdinal("critical_date_id")) ? 0 : readerTenantCriticalDate.GetInt32(readerTenantCriticalDate.GetOrdinal("critical_date_id"));
                        futureTenantCriticalDate.FutureTenantId = readerTenantCriticalDate.IsDBNull(readerTenantCriticalDate.GetOrdinal("future_tenant_id")) ? 0 : readerTenantCriticalDate.GetInt32(readerTenantCriticalDate.GetOrdinal("future_tenant_id"));

                        futureTenantCriticalDate.CriticalDateMaster = readerTenantCriticalDate.IsDBNull(readerTenantCriticalDate.GetOrdinal("critical_date_master")) ? "" : readerTenantCriticalDate.GetString(readerTenantCriticalDate.GetOrdinal("critical_date_master"));

                        futureTenantCriticalDate.StartDate = readerTenantCriticalDate.IsDBNull(readerTenantCriticalDate.GetOrdinal("start_date")) ? DateTime.Now : readerTenantCriticalDate.GetDateTime(readerTenantCriticalDate.GetOrdinal("start_date"));
                        futureTenantCriticalDate.EndDate = readerTenantCriticalDate.IsDBNull(readerTenantCriticalDate.GetOrdinal("end_date")) ? DateTime.Now : readerTenantCriticalDate.GetDateTime(readerTenantCriticalDate.GetOrdinal("end_date"));
                        futureTenantCriticalDate.CriticalDateNotes = readerTenantCriticalDate.IsDBNull(readerTenantCriticalDate.GetOrdinal("critical_date_notes")) ? "" : readerTenantCriticalDate.GetString(readerTenantCriticalDate.GetOrdinal("critical_date_notes"));

                        futureTenant.TenantCriticalDates.Add(futureTenantCriticalDate);
                    }

                    con.Close();
                }
            }

            return View(futureTenant);
        }

        List<TransactionStatusModel> GetTransactionStatusLeaseList()
        {
            var transactionStatusList = new List<TransactionStatusModel>();

            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("GetTransactionStatusLease", con);
                cmd.CommandType = CommandType.StoredProcedure;

                con.Open();

                SqlDataReader readerAssetType = cmd.ExecuteReader();
                while (readerAssetType.Read())
                {
                    var transactionStatusItem = new TransactionStatusModel();

                    transactionStatusItem.TransactionStatusId = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("transaction_status_id")) ? 0 : readerAssetType.GetInt32(readerAssetType.GetOrdinal("transaction_status_id"));
                    transactionStatusItem.TransactionStatusName = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("transaction_status_name")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("transaction_status_name"));
                    transactionStatusList.Add(transactionStatusItem);
                }

                con.Close();

            }

            return transactionStatusList;
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
                cmd.Parameters.AddWithValue("property_type", SamsPropertyType.NetLease);
                con.Open();



                SqlDataReader readerAssetType = cmd.ExecuteReader();
                while (readerAssetType.Read())
                {
                    var ddpViewModel = new DiligenceDispositionsViewModel();

                    ddpViewModel.PropertyId = propertyId;
                    ddpViewModel.PropertyType = (int)SamsPropertyType.NetLease;
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

                ddpViewModel.PropertyType = (int)SamsPropertyType.NetLease;
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
                cmdPeriod.Parameters.AddWithValue("property_type", (int)SamsPropertyType.NetLease);
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
                cmd.Parameters.AddWithValue("property_type", SamsPropertyType.NetLease);
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

            return RedirectToAction("ViewNetLeaseProperties", new { propertyId = diligenceDispositions.PropertyId });
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
                cmd.Parameters.AddWithValue("property_type", (int)SamsPropertyType.NetLease);
                cmd.Parameters.AddWithValue("property_id", propertyId);
                cmd.CommandType = CommandType.StoredProcedure;

                con.Open();

                cmd.ExecuteNonQuery();


                con.Close();

            }
            return RedirectToAction("ViewNetLeaseProperties", new { propertyId = propertyId });
        }


        public IActionResult DeleteSaleLeaseBackTransaction(int diligenceDispositionId, int propertyId)
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
                SqlCommand cmd = new SqlCommand("DeleteSaleLeaseBackTransaction", con);
                cmd.Parameters.AddWithValue("diligence_dispositions_id", diligenceDispositionId);
                cmd.CommandType = CommandType.StoredProcedure;

                con.Open();

                cmd.ExecuteNonQuery();


                con.Close();

            }
            return RedirectToAction("ViewNetLeaseProperties", new { propertyId = propertyId });
        }

        public IActionResult DeleteLeaseTransaction(int diligenceLeaseId, int propertyId)
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
            return RedirectToAction("ViewNetLeaseProperties", new { propertyId = propertyId });
        }


        public IActionResult DeleteLeaseWithPurchaseTransaction(int diligenceLeaseWithPurchaseId, int propertyId)
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
                SqlCommand cmd = new SqlCommand("DeleteLeaseWithPurchaseTransaction", con);
                cmd.Parameters.AddWithValue("diligence_lease_with_purchase_id", diligenceLeaseWithPurchaseId);
                cmd.CommandType = CommandType.StoredProcedure;

                con.Open();

                cmd.ExecuteNonQuery();

                con.Close();

            }
            return RedirectToAction("ViewNetLeaseProperties", new { propertyId = propertyId });
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

            return RedirectToAction("ViewNetLeaseProperties", new { propertyId = propertyId });
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

            return RedirectToAction("ViewNetLeaseProperties", new { propertyId = propertyId });
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

            return RedirectToAction("ViewNetLeaseProperties", new { propertyId = propertyId });
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
        public RedirectToActionResult SaveConfidentialFile(AdditionalFilesViewModel uploadedFile)
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
                SqlCommand cmd = new SqlCommand("SaveNetLeaseConfidentialFiles", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                cmd.Parameters.AddWithValue("file_id", uploadedFile.FileId);
                cmd.Parameters.AddWithValue("property_id", uploadedFile.PropertyId);
                cmd.Parameters.AddWithValue("file_type", uploadedFile.FileType);
                cmd.Parameters.AddWithValue("file_name", uniqueFileName);


                cmd.ExecuteNonQuery();


                con.Close();
            }


            return RedirectToAction("ViewNetLeaseProperties", new { propertyId = uploadedFile.PropertyId });

        }

        public RedirectToActionResult DeleteConfidentialFile(int fileId, int propertyId)
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
                SqlCommand cmd = new SqlCommand("DeleteConfidentialFiles", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                cmd.Parameters.AddWithValue("file_id", fileId);

                cmd.ExecuteNonQuery();


                con.Close();
                return RedirectToAction("ViewNetLeaseProperties", new { propertyId = propertyId });
            }

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