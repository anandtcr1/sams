using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sams.Models
{
    public class TodoViewModel
    {
        public int TodoId { get; set; }
        public int PropertyId { get; set; }
        public string TodoText { get; set; }
        
        public int PropertyType { get; set; }
        public DateTime CreatedDate { get; set; }

        public int CompletedStatus { get; set; }
        public int CreatedById { get; set; }
        public string CreatedUserName { get; set; }
        public int UpdatedById { get; set; }
        public string UpdatedUserName { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
