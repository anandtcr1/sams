using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sams.Models
{
    public class DiligenceAcquisitionViewModel
    {
        public int DiligenceAcquisitionId { get; set; }
        public int PropertyId { get; set; }
        public int PropertyType { get; set; }
        public string PurchasePrice { get; set; }
        public string EarnestMoney { get; set; }
        public string Exchage1031 { get; set; }
        public string Deadline1031 { get; set; }
        public string Sellers { get; set; }
        public string EscrowAgent { get; set; }
        public string SubDivision { get; set; }
        public string RealEstateAgent { get; set; }
        public DateTime CreatedDate { get; set; }
        public int AcquisitionStatus { get; set; }

        public DateTime ClosedDate { get; set; }
        public DateTime TerminatedDate { get; set; }

        public DateTime? UnderContractDate { get; set; }
        public DateTime? DueDiligenceExpairyDate { get; set; }
        public DateTime? DDPExtension { get; set; }
        public int DDPExtensionOpted { get; set; }
        public string AdditionalEarnestMoneyDeposit { get; set; }
        public string PermittingPeriod { get; set; }
        public string BuyingEntity { get; set; }
        public string BuyersAgent { get; set; }
        public string SellersAgent { get; set; }
        public string BuyersAttorney { get; set; }
        public string SellersAttorney { get; set; }
        public string BuyersAgentCommission { get; set; }
        public string SellersAgentCommission { get; set; }
        public DateTime? ClosingDate { get; set; }
        public List<TransactionStatusModel> TransactionStatusList { get; set; }

    }
}
