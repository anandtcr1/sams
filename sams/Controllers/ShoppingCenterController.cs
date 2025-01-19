using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using sams.Common;
using sams.Models;

namespace sams.Controllers
{
    public class ShoppingCenterController : Controller
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        //private readonly ApplicationDbContext dbContext;

        public ShoppingCenterController(IWebHostEnvironment hostEnvironment)
        {
            webHostEnvironment = hostEnvironment;
        }
        public IActionResult Index()
        {
            List<ShoppingCenterViewModel> shoppingCenterList = new List<ShoppingCenterViewModel>();

            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("GetShoppingCenterList", con);
                cmd.Parameters.AddWithValue("asset_status", 0);

                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                shoppingCenterList = GetShoppingCenter(reader);
                con.Close();
            }

            return View(shoppingCenterList);
        }

        public IActionResult GetSoldoutCenters()
        {
            List<ShoppingCenterViewModel> shoppingCenterList = new List<ShoppingCenterViewModel>();

            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("GetShoppingCenterList", con);
                cmd.Parameters.AddWithValue("asset_status", 1);

                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                shoppingCenterList = GetShoppingCenter(reader);
                con.Close();
            }

            return View(shoppingCenterList);
        }


        public IActionResult EditShoppingCenter(int centerId)
        {
            var shoppingCenter = new ShoppingCenterViewModel();

            List<ShoppingCenterViewModel> shoppingCenterList = new List<ShoppingCenterViewModel>();
            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("GetShoppingCenterById", con);
                cmd.Parameters.AddWithValue("shopping_center_id", centerId);

                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                shoppingCenterList = GetShoppingCenter(reader);
                con.Close();
            }
            if (shoppingCenterList.Count > 0)
            {
                shoppingCenter = shoppingCenterList[0];
            }
            else
            {
                shoppingCenter = new ShoppingCenterViewModel();
            }

            shoppingCenter.StateList = GetStateList();
            shoppingCenter.AssetTypeList = GetAssetType();


            return View(shoppingCenter);
        }

        [HttpPost]
        public IActionResult SaveShoppingCenter(ShoppingCenterViewModel shoppingCenterModel)
        {
            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("SaveShoppingCenter", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("shopping_center_id", shoppingCenterModel.ShoppingCenterId);
                cmd.Parameters.AddWithValue("shopping_center_name", shoppingCenterModel.ShoppingCenterName);
                cmd.Parameters.AddWithValue("state_id", shoppingCenterModel.StateId);
                cmd.Parameters.AddWithValue("city_name", shoppingCenterModel.CityName);
                cmd.Parameters.AddWithValue("zip_code", shoppingCenterModel.Zipcode);
                cmd.Parameters.AddWithValue("property_status_id", shoppingCenterModel.PropertyStatusId);
                cmd.Parameters.AddWithValue("rent_amount", shoppingCenterModel.RentAmount);
                cmd.Parameters.AddWithValue("property_type_id", shoppingCenterModel.PropertyTypeId);
                cmd.Parameters.AddWithValue("spaces", shoppingCenterModel.Spaces);
                cmd.Parameters.AddWithValue("spaces_available", shoppingCenterModel.SpacesAvailable);
                cmd.Parameters.AddWithValue("building_size", shoppingCenterModel.BuildingSize);

                cmd.Parameters.AddWithValue("asset_status", shoppingCenterModel.AssetStatus);
                cmd.Parameters.AddWithValue("shop_description", shoppingCenterModel.ShopDescription);
                cmd.Parameters.AddWithValue("is_deleted", shoppingCenterModel.IsDeleted);

                con.Open();

                shoppingCenterModel.ShoppingCenterId = int.Parse(cmd.ExecuteScalar().ToString());

                return RedirectToAction("ViewShoppingCenter", new { centerId = shoppingCenterModel.ShoppingCenterId });
            }
        }

        public IActionResult ViewShoppingCenter(int centerId)
        {
            var shoppingCenter = new ShoppingCenterViewModel();

            List<ShoppingCenterViewModel> shoppingCenterList = new List<ShoppingCenterViewModel>();
            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("GetShoppingCenterById", con);
                cmd.Parameters.AddWithValue("shopping_center_id", centerId);

                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                shoppingCenterList = GetShoppingCenter(reader);
                con.Close();
            }
            if (shoppingCenterList.Count > 0)
            {
                shoppingCenter = shoppingCenterList[0];
            }
            else
            {
                shoppingCenter = new ShoppingCenterViewModel();
            }

            shoppingCenter.StateList = GetStateList();
            shoppingCenter.AssetTypeList = GetAssetType();
            shoppingCenter.ImageList = GetImages(centerId);

            return View(shoppingCenter);
        }

        List<ImageViewModel> GetImages(int centerId)
        {
            List<ImageViewModel> propertyImageList = new List<ImageViewModel>();

            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmdImageList = new SqlCommand("GetPropertyImageList", con);

                cmdImageList.Parameters.AddWithValue("property_id", centerId);
                cmdImageList.Parameters.AddWithValue("property_type", SamsPropertyType.ShoppingCenter);

                cmdImageList.CommandType = CommandType.StoredProcedure;
                con.Open();

                SqlDataReader readerMarket = cmdImageList.ExecuteReader();
                
                while (readerMarket.Read())
                {
                    var imageItem = new ImageViewModel();
                    imageItem.ImageId = readerMarket.IsDBNull(readerMarket.GetOrdinal("image_id")) ? 0 : readerMarket.GetInt32(readerMarket.GetOrdinal("image_id"));
                    imageItem.PropertyId = centerId;



                    imageItem.ImageName = readerMarket.IsDBNull(readerMarket.GetOrdinal("image_name")) ? "" : readerMarket.GetString(readerMarket.GetOrdinal("image_name"));
                    string pic = @"../../UploadedImage/" + imageItem.ImageName;
                    imageItem.ImageName = pic;
                    propertyImageList.Add(imageItem);
                }
                con.Close();
            }

            return propertyImageList;
        }

        List<AssetTypeViewModel> GetAssetType()
        {
            List<AssetTypeViewModel> assetTypeList = new List<AssetTypeViewModel>();
            string CS = DBConnection.ConnectionString;

            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmdAssetType = new SqlCommand("GetAssetType", con);
                cmdAssetType.CommandType = CommandType.StoredProcedure;
                con.Open();

                SqlDataReader readerAssetType = cmdAssetType.ExecuteReader();
                while (readerAssetType.Read())
                {
                    var assetType = new AssetTypeViewModel();
                    assetType.AssetTypeId = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("asset_type_id")) ? 0 : readerAssetType.GetInt32(readerAssetType.GetOrdinal("asset_type_id"));
                    assetType.AssetTypeName = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("asset_type_name")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("asset_type_name"));

                    assetTypeList.Add(assetType);
                }
                con.Close();
            }
            return assetTypeList;
        }

        List<StateDetails> GetStateList()
        {
            string CS = DBConnection.ConnectionString;
            List<StateDetails> stateList = new List<StateDetails>();
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("GetStateList", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var stateDetails = new StateDetails();
                    stateDetails.StateId = reader.IsDBNull(reader.GetOrdinal("state_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("state_id"));
                    stateDetails.StateCode = reader.IsDBNull(reader.GetOrdinal("state_code")) ? "" : reader.GetString(reader.GetOrdinal("state_code"));
                    stateDetails.StateName = reader.IsDBNull(reader.GetOrdinal("state_name")) ? "" : reader.GetString(reader.GetOrdinal("state_name"));
                    stateList.Add(stateDetails);
                }
                con.Close();

            }
            return stateList;
        }
        List<ShoppingCenterViewModel> GetShoppingCenter(SqlDataReader reader)
        {
            List<ShoppingCenterViewModel> shoppingCenterList = new List<ShoppingCenterViewModel>();
            

            while (reader.Read())
            {
                var shoppingCenterModel = new ShoppingCenterViewModel();

                shoppingCenterModel.ShoppingCenterId = reader.IsDBNull(reader.GetOrdinal("shopping_center_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("shopping_center_id"));

                shoppingCenterModel.ShoppingCenterName = reader.IsDBNull(reader.GetOrdinal("shopping_center_name")) ? "" : reader.GetString(reader.GetOrdinal("shopping_center_name"));
                shoppingCenterModel.StateId = reader.IsDBNull(reader.GetOrdinal("state_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("state_id"));
                shoppingCenterModel.StateName = reader.IsDBNull(reader.GetOrdinal("state_name")) ? "" : reader.GetString(reader.GetOrdinal("state_name"));

                shoppingCenterModel.CityName = reader.IsDBNull(reader.GetOrdinal("city_name")) ? "" : reader.GetString(reader.GetOrdinal("city_name"));
                shoppingCenterModel.Zipcode = reader.IsDBNull(reader.GetOrdinal("zip_code")) ? "" : reader.GetString(reader.GetOrdinal("zip_code"));
                
                shoppingCenterModel.PropertyStatusId = reader.IsDBNull(reader.GetOrdinal("property_status_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("property_status_id"));
                shoppingCenterModel.PropertyStatusName = reader.IsDBNull(reader.GetOrdinal("asset_type_name")) ? "" : reader.GetString(reader.GetOrdinal("asset_type_name"));

                shoppingCenterModel.RentAmount = reader.IsDBNull(reader.GetOrdinal("rent_amount")) ? "" : reader.GetString(reader.GetOrdinal("rent_amount"));
                shoppingCenterModel.PropertyTypeId = reader.IsDBNull(reader.GetOrdinal("property_type_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("property_type_id"));
                shoppingCenterModel.Spaces = reader.IsDBNull(reader.GetOrdinal("spaces")) ? "" : reader.GetString(reader.GetOrdinal("spaces"));
                shoppingCenterModel.SpacesAvailable = reader.IsDBNull(reader.GetOrdinal("spaces_available")) ? "" : reader.GetString(reader.GetOrdinal("spaces_available"));
                shoppingCenterModel.BuildingSize = reader.IsDBNull(reader.GetOrdinal("building_size")) ? "" : reader.GetString(reader.GetOrdinal("building_size"));
                shoppingCenterModel.AssetStatus = reader.IsDBNull(reader.GetOrdinal("asset_status")) ? 0 : reader.GetInt32(reader.GetOrdinal("asset_status"));
                
                shoppingCenterModel.ShopDescription = reader.IsDBNull(reader.GetOrdinal("shop_description")) ? "" : reader.GetString(reader.GetOrdinal("shop_description")); 

                shoppingCenterModel.CreatedDate = reader.IsDBNull(reader.GetOrdinal("created_date")) ? DateTime.Now : reader.GetDateTime(reader.GetOrdinal("created_date"));
                
                shoppingCenterList.Add(shoppingCenterModel);
            }

            return shoppingCenterList;
        }

        [HttpPost]
        public RedirectToActionResult UploadImage(ImageViewModel uploadedImge)
        {

            var uniqueFileName = Helper.GetUniqueFileName(uploadedImge.UploadedImage.FileName);

            var filePath = Path.Combine(webHostEnvironment.WebRootPath + @"/UploadedImage", uniqueFileName);

            using (var stream = System.IO.File.Create(filePath))
            {
                uploadedImge.UploadedImage.CopyTo(stream);
            }

            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("SavePropertyImage", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                cmd.Parameters.AddWithValue("property_id", uploadedImge.PropertyId);
                cmd.Parameters.AddWithValue("image_name", uniqueFileName);
                cmd.Parameters.AddWithValue("property_type", uploadedImge.PropertyType);

                cmd.ExecuteNonQuery();


                con.Close();
            }

            return RedirectToAction("ViewShoppingCenter", new { centerId = uploadedImge.PropertyId });
            

        }

        public RedirectToActionResult DeleteImage(int imageId, int propertyId)
        {


            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("DeleteUploadedImage", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                cmd.Parameters.AddWithValue("image_id", imageId);

                cmd.ExecuteNonQuery();


                con.Close();
                
                return RedirectToAction("ViewShoppingCenter", new { centerId = propertyId });
            }




        }
    }
}