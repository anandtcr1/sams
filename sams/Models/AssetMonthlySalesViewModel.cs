using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sams.Models
{
    public class AssetMonthlySalesViewModel
    {
        public int MonthId { get; set; }
        public string MonthName { get; set; }
        public int SelectedYear { get; set; }
        public int TotalRecords { get; set; }
    }
}
