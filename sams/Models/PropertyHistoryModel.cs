using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sams.Models
{
    public class PropertyHistoryModel
    {
        public int PropertyHistoryId { get; set; }
        public int PropertyId { get; set; }
        public int StatusId { get; set; }
        public string StatusName { get; set; }
        public string Description { get; set; }

        public int LoggedInId { get; set; }
        public string LoggedInUserName { get; set; }
        public DateTime CreatedDate { get; set; }
        public int TransactionId { get; set; }

    }
}
