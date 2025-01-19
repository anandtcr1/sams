using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sams.Models
{
    public class AdditionalFilesViewModel
    {
        public int FileId { get; set; }
        public int PropertyId { get; set; }
        public string FileType { get; set; }
        public string FileName { get; set; }
        public string FileNameWithoutPath { get; set; }

        public IFormFile SelectedFile { get; set; }
    }
}
