using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace sams.Models
{
    public class SiteDetails
    {
        public int SiteDetailsId { get; set; }
        public string NamePrefix { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string FullName
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }

        public string CompanyName { get; set; } 
        public string EmailAddress { get; set; }
        public string Address { get; set; }
        public string CityName { get; set; }
        public string StateId { get; set; }
        public string StateName { get; set; }
        public string ZipCode { get; set; }
        public string ContactNumber { get; set; }
        public bool SamsHoldingEmployee { get; set; }

        public int MarketId { get; set; }

        public string PropertyHeader { get; set; }

        public string AssetId { get; set; }

        public string MarketName { get; set; }
        public string SiteAddress { get; set; }
        public string SiteAddressSmall { get; set; }
        public string SiteCity { get; set; }
        public int SiteStateId { get; set; }
        public string SiteStateName { get; set; }
        public string SiteCounty { get; set; }
        public string SiteCrossStreetName { get; set; }
        public bool IsPropertyAvailable { get; set; }
        public string Zoning { get; set; }
        public string LotSize { get; set; }
        public string SalesPrice { get; set; }
        public string Comments { get; set; }
        public string EnteredCaptcha { get; set; }


        public List<StateDetails> StateList { get; set; }
        public List<StateDetails> AllStateList { get; set; }
        public List<MarketDetails> MarketList { get; set; }

        public Bitmap CaptchaImage { get; set; }
        public string CaptchaEntered { get; set; }

        public DateTime CreatedDate { get; set; }
        public int PropertyType { get; set; }
        public string ImageName { get; set; }

        public List<ImageViewModel> PropertyImageList { get; set; }

        public int AssetTypeId { get; set; }
        public string AssetTypeName { get; set; }
        public List<AdditionalFilesViewModel> AdditionalFiles { get; set; }

        public List<AssetTypeViewModel> AssetTypeList { get; set; }

        public IFormFile SelectedImage { get; set; }
        public IFormFile SelectedPdf { get; set; }

        public string SelectedImageName { get; set; }
        public string SelectedPdfName { get; set; }

        /// <summary>
        /// Initially, it started with true or false. But, later added 3 types
        /// 0 - New Record
        /// 1 - Closed Record
        /// 2 - In-Progress Recrod
        /// </summary>
        public int IsDeleted { get; set; }

        public int AssetStatus { get; set; }
        public string AssetStatusName { get; set; }

        public int DiligenceType { get; set; }

        public List<TodoViewModel> TodoList { get; set; }
        public string LatestComment { get; set; }
        public DiligenceAcquisitionViewModel DiligenceAcquisitions { get; set; }
        public List<DiligenceDispositionsViewModel> DiligenceDispositions { get; set; }
        public DiligenceDispositionsViewModel SelectedDiligenceDispositions { get; set; }
        public DiligenceLeaseViewModel DiligenceLease { get; set; }
        public List<DiligenceLeaseViewModel> DiligenceLeaseList { get; set; }
        public DiligenceLeaseViewModel SelectedDiligenceLease { get; set; }
        public List<PeriodViewModel> AcquisitionPeriodList { get; set; }
        public List<PeriodViewModel> DispositionPeriodList { get; set; }
        public List<PeriodViewModel> LeasePeriodList { get; set; }
        public List<PeriodViewModel> PurchaseLeaseBackPeriodList { get; set; }
        public List<PeriodViewModel> LeaseWithPurchasePeriodList { get; set; }
        public List<PropertyStatusModel> propertyStatusList { get; set; }
        public List<LeaseTypeModel> LeaseTypeList { get; set; }
        public int SelectedPropertyStatusId { get; set; }
        public string SelectedPropertyStatus { get; set; }

        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string PotentialUse { get; set; }
        public int CheckIfClientRepresentedByABroker { get; set; }
        public string BrokerOrFirmName { get; set; }
        public string BrokerContactNumber { get; set; }
        public string BrokerEmailAddress { get; set; }

        public int CheckIfPropertyListed { get; set; }
        public string ListingAgentName { get; set; }
        public DateTime? ListingExpiry { get; set; }
        public string ListingPrice { get; set; }

        public string Term { get; set; }
        public string AskingRent { get; set; }
        public string OptionPurchase { get; set; }
        public string AskingPrice { get; set; }
        
        public int LeaseType { get; set; }
        public DateTime? StatusChangedDate { get; set; }
        public int IsClosed { get; set; }
        public List<NewPropertyStatusModel> NewPropertyStatusList { get; set; }
        public int NewPropertyStatusId { get; set; }
        public string NewPropertyStatusName { get; set; }
        public bool ShowInListing { get; set; }
        public int TransactionStatusId { get; set; }
        public string TransactionStatusName { get; set; }
        public string SaleTransactions { get; set; }
        public string LeaseTransactions { get; set; }
        public bool CanAddTransactions { get; set; }
        public bool IsTransactionClosed { get; set; }
        public int MaxPriorityTransactionStatusId { get; set; }
        public string MaxPriorityTransactionStatusName { get; set; }

        public List<PropertyHistoryModel> PropertyHistoryList { get; set; }
        public SamsSettings MySettings { get; set; }
        public List<DiligenceLeaseWithPurchaseViewModel> DiligenceLeaseWithPurchaseList { get; set; }
        public DiligenceLeaseWithPurchaseViewModel DiligenceLeaseWithPurchase { get; set; }
        public List<PeriodViewModel> LeasePurchasePeriodList { get; set; }
        public string TermOptionPurchase { get; set; }
        public string AskingRentOptionPurchase { get; set; }
        public int LeaseTypePurchase { get; set; }
        public string OptionPurchasePrice { get; set; }
        public string PotentialUseOptionPurchase { get; set; }
        public string CommentsOptionPurchase { get; set; }
        public List<DiligenceDispositionsViewModel> DiligenceDispositions_SaleLeaseBack { get; set; }
        public DiligenceDispositionsViewModel DiligenceDispositions_PurchaseLeaseBack { get; set; }
        public string PurchasePrice { get; set; }
        public string PurchaseLeasebackTerm { get; set; }
        public string PurchaseLeasebackRent { get; set; }
        public int PurchaseLeasebackLeaseTypeId { get; set; }
        public string PurchaseLeasebackPotentialUse { get; set; }
        public string PurchaseLeasebackComments { get; set; }
        public long RowNumber { get; set; }

        public string FeePotentialUse { get; set; }
        public string FeeComments { get; set; }

        public int RegionId { get; set; }
        public List<RegionViewModel> RegionList { get; set; }
        public string RegionName { get; set; }
        public string PropertyHeaderLine2 { get; set; }
        public string LastFourDigitNumber { get; set; }
        public string ContactNumber1 { get; set; }
        public string ContactNumber2 { get; set; }
        public string ContactNumber3 { get; set; }
    }
}
