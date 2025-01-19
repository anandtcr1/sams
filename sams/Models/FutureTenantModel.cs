using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sams.Models
{
    public class FutureTenantModel
    {
        public int FutureTenantId { get; set; }
        public int NetLeaseId { get; set; }
        public string Tenant { get; set; }
        public string Unit { get; set; }
        public string Term { get; set; }
        public string Rent { get; set; }
        public string CAM { get; set; }
        public DateTime? UnderContractDate { get; set; }
        public string DDP { get; set; }
        public string TenantUpfitConcession { get; set; }
        public int RentFreePeriod { get; set; }
        public DateTime? LeaseCommencementDate { get; set; }
        public DateTime? LeaseExpirationDate { get; set; }
        public string LeaseOptions { get; set; }
        public string RentEscalation { get; set; }
        public string TenantAttorney { get; set; }
        public string TenantAgentCommission { get; set; }
        public string LandlordAgentCommission { get; set; }
        public string LeaseSecurityDeposit { get; set; }
        public string FreeRentPeriodDescription { get; set; }
        public int TransactionStatusId { get; set; }
        public string TransactionStatusName { get; set; }
        public List<TransactionStatusModel> LeaseTransactionList { get; set; }
        public List<TransactionFilesViewModel> TransactionFileList { get; set; }
        public List<FutureTenantCriticalDateModel> TenantCriticalDates { get; set; }
        public DateTime? LeaseDate { get; set; }
        public int IsLeaseTransaction { get; set; }
    }
}
