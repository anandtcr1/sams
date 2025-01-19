using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sams.Models
{
    public class SamsSettings
    {
        public int SettingsId { get; set; }
        public string SmtpServer { get; set; }
        public string SmtpPortNumber { get; set; }
        public string SmtpEmailAddress { get; set; }
        public string SmtpPassword { get; set; }
        public string EmailHeader { get; set; }
        public string EmailBody { get; set; }
        public string RealEstateDirectorName { get; set; }
        public string DirectorEmailAddress { get; set; }
        public string DirectorPhoneNumber { get; set; }
        public int ShowShoppingCenterMenu { get; set; }
    }
}
