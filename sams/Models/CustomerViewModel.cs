using DocuSign.eSign.Model;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace sams.Models
{

    public class CustomerViewModel
    {

        public int CustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string FullName {
            get {
                return this.FirstName + " " + this.LastName;
            }
        }

        public string EmailAddress { get; set; }
        public string ContactNumber { get; set; }
        public string CellNumber { get; set; }
        public string Company { get; set; }
        public string GivenTitle { get; set; }

        public string Zipcode { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public int StateId { get; set; }
        public string StateName { get; set; }
        public string SignedNDAFileName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string RetypePassword { get; set; }
        
        public DateTime CreatedDate { get; set; }
        public DateTime LastLoginDate { get; set; }
        public int LoginPropertyId { get; set; }

        public List<StateDetails> StateList { get; set; }
        public string CustomerSignature { get; set; }
        public string DirectorSignature { get; set; }

        [JsonIgnore]
        public IFormFile UploadedNDAFile { set; get; }

        public string SignedStatus { get; set; }
        public string ClientIpAddress { get; set; }

        public string EmailBody { get; set; }

        public string  RealEstateDirectorName { get; set; }
        public string SignatureId { get; set; }
        public string ResetPasswordId { get; set; }
        public List<PageHitViewModel> PageHitList { get; set; }

    }
}
