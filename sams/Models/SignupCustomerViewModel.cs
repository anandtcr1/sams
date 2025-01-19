using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sams.Models
{
    public class SignupCustomerViewModel
    {
        public int SignupCustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string ContactNumber { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool Subscribe { get; set; }
        public string LastFourDigitNumber { get; set; }
    }
}
