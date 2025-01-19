
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sams.Models
{
    public class DiligenceLeaseViewModel
    {
        public int DiligenceLeaseId { get; set; }
        public int PropertyId { get; set; }
        public int PropertyType { get; set; }
        public string Rent { get; set; }
        public string SellingPrice { get; set; }
        public string ListingPrice { get; set; }
        public DateTime? UnderContractDate { get; set; }
        public DateTime? DueDiligenceExpiryDate { get; set; } 
        public string EarnestMoneyDeposit { get; set; }
        public DateTime? DDPExtension { get; set; }

        public string Tenant { get; set; }
        public string TenantAttorney { get; set; }
        public string TenantAgentCommission { get; set; }
        public string LandlordAgentCommission { get; set; }
        public string LeaseSecurityDeposit { get; set; }
        public DateTime CreatedDate { get; set; }

        public int DispositionTerminatedStatus { get; set; }
        public DateTime? DispositionTerminatedDate { get; set; }

        public int DispositionClosedStatus { get; set; }
        public DateTime? DispositionClosedDate { get; set; }

        public int SelectedTransactionStatusId { get; set; }
        public string SelectedTransactionStatusName { get; set; }
        public DateTime? SelectedTransactionDate { get; set; }
        public List<TransactionStatusModel> TransactionStatusList { get; set; }
        public string TransactionDescription { get; set; }
        public List<TransactionFilesViewModel> TransactionFileList { get; set; }
        public List<PeriodViewModel> DispositionCriticalItems { get; set; }
        public DateTime? LeaseCommencementDate { get; set; }
        public DateTime? ClosingDate { get; set; }
    }
}
