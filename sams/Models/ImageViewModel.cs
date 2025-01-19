using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sams.Models
{
    public class ImageViewModel
    {
        public int ImageId { get; set; }
        public int PropertyId { get; set; }
        public string ImageName { get; set; }

        public IFormFile UploadedImage { set; get; }
        public int PropertyType { get; set; }
    }
}
