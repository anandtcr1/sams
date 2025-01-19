using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sams.Models
{
    public class ShoppingMartPlanViewModel
    {
        public int CStoreId { get; set; }
        public IFormFile UploadedFile { set; get; }
    }
}
