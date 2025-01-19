using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sams.Models
{
    public class TransactionFilesViewModel
    {
        public int TransactionFilesId { get; set; }
        public int TransactionId { get; set; }
        public int PropertyId { get; set; }
        public string FileHeader { get; set; }
        public string FileName { get; set; }
        public string FileFullName { get; set; }
        public IFormFile SelectedFile { get; set; }
        public string Notes { get; set; }
        public DateTime UploadedDate { get; set; }
        public int UploadedById { get; set; }
        public string UploadedByName { get; set; }
    }
}
