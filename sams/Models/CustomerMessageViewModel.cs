using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sams.Models
{
    public class CustomerMessageViewModel
    {
        public int CustomerMessageId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string EmailSubject { get; set; }
        public string CustomerMessage { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CaptchaEntered { get; set; }
    }
}
