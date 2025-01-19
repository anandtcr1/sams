using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sams.Models
{
    public class DiligenceNetleaseViewModel
    {
        public int DiligenceDispositionsId { get; set; }
        public int PropertyId { get; set; }
        public int PropertyType { get; set; }
        public string SalePrice { get; set; }
        public string EarnestMoney { get; set; }
        public string Buyers { get; set; }
        public string EscrowAgent { get; set; }
        public string BuyersAttorney { get; set; }
        public string OptionsToExtend { get; set; }
        public string Commissions { get; set; }
        public DateTime CreatedDate { get; set; }
        public int DispositionStatus { get; set; }

        public DateTime ClosedDate { get; set; }
        public DateTime TerminatedDate { get; set; }



        public DateTime? UnderContractDate { get; set; }
        public DateTime? DueDiligenceExpairyDate { get; set; }
        public string DueDiligenceAmount { get; set; }
        public string EMD { get; set; }
        public DateTime? DDPExtension { get; set; }
        public int DDPExtensionOpted { get; set; }

        public string SellersAttorney { get; set; }
        public string BuyersAgentCommission { get; set; }
        public string SellersAgentCommission { get; set; }


        public int DispositionTerminatedStatus { get; set; }
        public DateTime? DispositionTerminatedDate { get; set; }

        public int DispositionClosedStatus { get; set; }
        public DateTime? DispositionClosedDate { get; set; }
        public int SelectedTransactionStatusId { get; set; }
        public string SelectedTransactionStatusName { get; set; }
        public DateTime? SelectedTransactionDate { get; set; }
        public List<TransactionStatusModel> TransactionStatusList { get; set; }
        public string PermittingPeriod { get; set; }
        public string TransactionDescription { get; set; }

        public List<TransactionFilesViewModel> TransactionFileList { get; set; }
        public List<PeriodViewModel> DispositionCriticalItems { get; set; }
        public string Tenant { get; set; }
        public string TenantRent { get; set; }
        public DateTime? ClosingDate { get; set; }
    }
}
