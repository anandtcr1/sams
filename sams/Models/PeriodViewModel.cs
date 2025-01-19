using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sams.Models
{
    public class PeriodViewModel
    {
        public int PeriodId { get; set; }
        public int PropertyId { get; set; }
        public int PropertyType { get; set; }
        public string PeriodMaster { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string PeriodNotes { get; set; }
        public int Duration
        {
            get
            {
                return (EndDate.Date - StartDate.Date).Days;
            }
            
        }
        public int AddedDuration { get; set; }
        public int DaysToExpire
        {
            get
            {
                return (EndDate.Date - DateTime.Now.Date).Days;
            }
        }

        public string AssetId { get; set; }
        public string PeriodType { get; set; }
        public int TransactionId { get; set; }
        public int CurrentAssetStatusId { get; set; }
        public DateTime? AlertDate { get; set; }
        public string EmployeeEmailAddress { get; set; }
        public string OtherEmailAddress { get; set; }
    }
}
