using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sams.Models
{
    public class NetleasePropertiesViewModel
    {
        public int NetleasePropertyId { get; set; }
        public string AssetId { get; set; }
        public string AssetName { get; set; }
        public string Address { get; set; }
        public string AddressShort { get; set; }
        public string City { get; set; }
        public int StateId { get; set; }
        
        public string StateName { get; set; }

        public string ZipCode { get; set; }

        public string PropertyPrice { get; set; }
        public double CapRate { get; set; }
        public string Term { get; set; }
        public string PdfFileName { get; set; }
        public IFormFile UploadedPdf { set; get; }
        public string SelectedPdfFileName { get; set; }
        public int AssetTypeId { get; set; }
        public int AssetTypeId_ShoppingCenter { get; set; }
        public string AssetTypeName { get; set; }

        public DateTime CreatedDate { get; set; }

        public bool IsShoppingCenter { get; set; }
        public string ShoppingCenterOrNetlease { get; set; }

        public List<StateDetails> StateList { get; set; }

        public List<ImageViewModel> ImageList { get; set; }
        public List<AssetTypeViewModel> AssetTypeList { get; set; }
        public List<AssetTypeViewModel> AssetTypeListShoppingCenter { get; set; }

        public List<AdditionalFilesViewModel> AdditionalFilesList { get; set; }

        public int AssetStatus { get; set; }
        public string AssetStatusName { get; set; }
        public List<TodoViewModel> TodoList { get; set; }
        public string LatestComment { get; set; }
        public int DiligenceType { get; set; }

        public DiligenceAcquisitionViewModel DiligenceAcquisitions { get; set; }
        public DiligenceDispositionsViewModel DiligenceDispositions { get; set; }
        public List<DiligenceDispositionsViewModel> DiligenceDispositionList { get; set; }
        public DiligenceLeaseViewModel DiligenceLease { get; set; }
        public List<DiligenceLeaseViewModel> DiligenceLeaseList { get; set; }
        public List<PeriodViewModel> DispositionPeriodList { get; set; }
        public List<PeriodViewModel> LeasePeriodList { get; set; }
        public List<FutureTenantModel> FutureTenantList { get; set; }


        public string Latitude { get; set; }
        public string Longitude { get; set; }


        public IFormFile ShoppingMartPlanFile { get; set; }
        public string ShoppingMartPlanFileName { get; set; }
        public string SavedShoppingMartPlanFileName { get; set; }
        public List<ShoppingCenterClients> ShoppingCenterClientList
        {
            get; set;
        }

        public int SelectedPropertyStatusId { get; set; }
        public string SelectedPropertyStatus { get; set; }

        public List<PropertyStatusModel> propertyStatusList { get; set; }
        public string FileType { get; set; }

        public List<LeaseTypeModel> LeaseTypeList { get; set; }

        public int CheckIfPropertyListed { get; set; }
        public string ListingAgentName { get; set; }
        public DateTime ListingExpiry { get; set; }
        public string ListingPrice { get; set; }
        public string AskingRent { get; set; }
        public int LeaseType { get; set; }
        public string LeaseTypeName { get; set; }
        public int LeaseTypeLeaseAndFee { get; set; }
        public string LeaseTypeLeaseAndFeeName { get; set; }


        public string Details { get; set; }
        public DateTime? StatusChangedDate { get; set; }
        public int IsClosed { get; set; }

        public bool ShowInListing { get; set; }
        public string TransactionStatusName { get; set; }
        public string SaleTransactions { get; set; }
        public string LeaseTransactions { get; set; }
        public bool CanAddTransactions { get; set; }

        public int MaxPriorityTransactionStatusId { get; set; }
        public string MaxPriorityTransactionStatusName { get; set; }
        public List<PropertyHistoryModel> PropertyHistoryList { get; set; }

        public string TermRemaining { get; set; }
        public string RentalIncome { get; set; }
        public SamsSettings MySettings { get; set; }
        public List<DiligenceLeaseWithPurchaseViewModel> DiligenceLeaseWithPurchaseList { get; set; }
        public DiligenceLeaseWithPurchaseViewModel DiligenceLeaseWithPurchase { get; set; }
        public List<DiligenceNetleaseViewModel> DiligenceNetleaseList { get; set; }
        public string TermOptionPurchase { get; set; }
        public string AskingRentOptionPurchase { get; set; }
        public int LeaseTypePurchase { get; set; }
        public string OptionPurchasePrice { get; set; }
        public List<DiligenceDispositionsViewModel> DiligenceDispositions_SaleLeaseBack { get; set; }
        public DiligenceDispositionsViewModel SelectedDiligenceDisposition { get; set; }
        public DiligenceNetleaseViewModel SelectedDiligenceNetlease { get; set; }
        public string NetleaseAssetName { get; set; }
        public string PotentialUse { get; set; }

        public int RegionId { get; set; }
        public List<RegionViewModel> RegionList { get; set; }
        public string RegionName { get; set; }
        public List<AdditionalFilesViewModel> NDAComplaintsFilesList { get; set; }
        public CustomerViewModel LoggedInUser { get; set; }
        public string PropertyHeaderLine2 { get; set; }
    }
}
