using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sams.Models
{
    public class ShoppingCenterClients
    {
        public int ShoppingCenterClientId { get; set; }
        public int CStoreId { get; set; }
        public string TenantName { get; set; }
        public string UnitSelected { get; set; }
        
        public string AnnualRent { get; set; }
        
        public string MonthlyRent { get; set; }
        public string CamMonthly { get; set; }
        public string CamYearly { get; set; }
        public string SetOrAdjustAutomatically { get; set; }
        public string RentAndCamMonthly { get; set; }
        public string RentAndCamYearly { get; set; }
        public string PiecePerSquareFoot { get; set; }
        public string LeaseExpaires { get; set; }
        public DateTime? DateRentChanges { get; set; }
        public string AnnualRentChangeTo { get; set; }
        public string RentPerMonthChangeTo { get; set; }
        public string RentAndCamChangeTo { get; set; }
        public string PiecePerSquareFootChangeTo { get; set; }
        public string SubspaceSquareFootage { get; set; }
        public string Notes { get; set; }
        public DateTime? CoiExpire { get; set; }
    }
}
