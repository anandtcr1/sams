using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sams.Models
{
    public class CStoreViewModel
    {
        public int CStoreId { get; set; }

        public string PropertyHeader { get; set; }
        public string Address { get; set; }
        public string AddressShort { get; set; }
        
        public int StateId { get; set; }
        public string StateName { get; set; }
        public string City { get; set; }
        public string Zipcode { get; set; }
        public string County { get; set; }

        public string AssetId { get; set; }
        public int PropertyTypeId { get; set; }
        public string PropertyTypeName { get; set; }

        
        public string Description { get; set; }
        public string AskingPrice { get; set; }
        public string AskingPriceString { get; set; }
        public string Rent { get; set; }
        public int AssetTypeId { get; set; }
        public string AssetTypeName { get; set; }
        public string LandSize { get; set; }
        public string BuildingArea { get; set; }
        public string PropertyTaxes { get; set; }
        public string YearBuilt { get; set; }
        public string KnownEnvironmentalConditions { get; set; }
        public string EMVCompliance { get; set; }

        public string HoursOfOperation { get; set; }

        public IFormFile EnvironentNDAPdf { get; set; }
        public string EnvironentNDAPdfFileName { get; set; }
        public DateTime CreatedDate { get; set; }

        public List<StateDetails> StateList { get; set; }
        public List<PropertyTypeViewModel> PropertyTypeList { get; set; }
        public List<AssetTypeViewModel> AssetTypeList { get; set; }

        public List<ImageViewModel> ImageList { get; set; }
        public List<AdditionalFilesViewModel> NDAComplaintsFilesList { get; set; }
        public List<AdditionalFilesViewModel> GeneralFilesList { get; set; }

        public CustomerViewModel LoggedInUser { get; set; }

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

        public List<LeaseTypeModel> LeaseTypeList { get; set; }

        public string Latitude { get; set; }
        public string Longitude { get; set; }

        //public ShoppingMartPlanViewModel ShoppingMartPlan { get; set; }

        public IFormFile ShoppingMartPlanFile { get; set; }
        public string ShoppingMartPlanFileName { get; set; }
        public List<ShoppingCenterClients> ShoppingCenterClientList
        {
            get; set;
        }

        public int SelectedPropertyStatusId { get; set; }
        public string SelectedPropertyStatus { get; set; }

        public List<PropertyStatusModel> propertyStatusList { get; set; }

        public int CheckIfPropertyListed { get; set; }
        public string ListingAgentName { get; set; }
        public DateTime ListingExpiry { get; set; }
        public string ListingPrice { get; set; }

        public string Term { get; set; }
        public string AskingRent { get; set; }
        public int LeaseType { get; set; }

        public int CheckIfOilSupplyContractApplicable { get; set; }
        public string TermOfSupplyContract { get; set; }

        public string Details { get; set; }
        public string TermRemaining { get; set; }
        public string RentalIncome { get; set; }
        public int LeaseTypeLeaseAndFee { get; set; }
        public int CheckIfOilSupplyContractApplicableLeaseAndFee { get; set; }
        public string TermOfSupplyContractLeaseAndFee { get; set; }
        public int LeaseAndFee { get; set; }
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
        public SamsSettings MySettings { get; set; }
        public List<DiligenceLeaseWithPurchaseViewModel> DiligenceLeaseWithPurchaseList { get; set; }

        public string TermOptionPurchase { get; set; }
        public string AskingRentOptionPurchase { get; set; }
        public int LeaseTypePurchase { get; set; }
        public string OptionPurchasePrice { get; set; }
        public List<DiligenceDispositionsViewModel> DiligenceDispositions_SaleLeaseBack { get; set; }

        public DiligenceLeaseWithPurchaseViewModel DiligenceLeaseWithPurchase { get; set; }
        public string PotentialUse { get; set; }

        public int RegionId { get; set; }
        public List<RegionViewModel> RegionList { get; set; }
        public string RegionName { get; set; }
        public string PropertyHeaderLine2 { get; set; }
    }
}
