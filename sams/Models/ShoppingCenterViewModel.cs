using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sams.Models
{
    public class ShoppingCenterViewModel
    {
        public int ShoppingCenterId { get; set; }
        public string ShoppingCenterName { get; set; }
        public int StateId { get; set; }
        public string StateName { get; set; }
        public string CityName { get; set; }
        public string Zipcode { get; set; }
        /// <summary>
        /// Lease/Sale
        /// </summary>
        public int PropertyStatusId { get; set; }
        public string PropertyStatusName { get; set; }

        public string RentAmount { get; set; }

        /// <summary>
        /// Retail/ Office
        /// </summary>
        public int PropertyTypeId { get; set; }
        public string PropertyTypeName { get; set; }

        /// <summary>
        /// 1 space, 3 spaces etc..
        /// </summary>
        public string Spaces { get; set; }

        public string SpacesAvailable { get; set; }
        public string BuildingSize { get; set; }

        public string ShopDescription { get; set; }
        public DateTime CreatedDate { get; set; }
        public int IsDeleted { get; set; }

        public List<StateDetails> StateList { get; set; }

        public List<ImageViewModel> ImageList { get; set; }
        public List<AssetTypeViewModel> AssetTypeList { get; set; }
        public int AssetStatus { get; set; }
        public string AssetStatusName { get; set; }
        public List<TodoViewModel> TodoList { get; set; }

        
    }
}
