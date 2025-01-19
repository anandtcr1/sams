using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sams.Models
{
    public class DashBoardSummaryViewModel
    {
        public int TotalSurplusProperties { get; set; }
        public int TotalNetleaseProperties { get; set; }
        public int TotalCstores { get; set; }
        public int TotalFromCustomers { get; set; }

        public UserViewModel LoggedInUser { get; set; }
        public List<AssetMonthlySalesViewModel> SurplusInStock { get; set; }
        public List<AssetMonthlySalesViewModel> SurplusSold { get; set; }
        public string MonthNames { get; set; }
        public string InStockSurplusData { get; set; }
        public string SoldSurplusData { get; set; }




        public List<AssetMonthlySalesViewModel> NetLeaseInStock { get; set; }
        public List<AssetMonthlySalesViewModel> NetLeaseSold { get; set; }
        public string InStockNetLeaseData { get; set; }
        public string SoldNetLeaseData { get; set; }



        public List<AssetMonthlySalesViewModel> CStoresInStock { get; set; }
        public List<AssetMonthlySalesViewModel> CStoresSold { get; set; }
        public string InStockCStoresData { get; set; }
        public string SoldCStoresData { get; set; }

        public List<PropertyLocationViewModel> PropertyLocationList { get; set; }
    }
}
