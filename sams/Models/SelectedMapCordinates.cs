using Microsoft.CodeAnalysis.Operations;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace sams.Models
{
    public class SelectedMapCordinates
    {
        public int CoordinateId { get; set; }
        public int HeaderId { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string MarkerColor { get; set; }
        public string MarkerHeader { get; set; }
        public string MarkerAddress { get; set; }
        public string MarkerType { get; set; }

        public string AddedAddress { get; set; }
        public string LandSize { get; set; }
        public string AskingPrice { get; set; }
        public string Zoning { get; set; }
        public string SamsMarkerHeader { get; set; }
    }
}
