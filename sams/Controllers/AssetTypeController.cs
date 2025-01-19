using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using sams.Models;

namespace sams.Controllers
{
    public class AssetTypeController : Controller
    {
        public IActionResult Index()
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
            return View(assetTypeList);
        }

        public IActionResult AddAssetType(int AssetTypeId)
        {
            var assetType = new AssetTypeViewModel();
            string CS = DBConnection.ConnectionString;

            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmdAssetType = new SqlCommand("GetAssetTypeById", con);
                cmdAssetType.CommandType = CommandType.StoredProcedure;
                cmdAssetType.Parameters.AddWithValue("asset_type_id", AssetTypeId);
                con.Open();

                SqlDataReader readerAssetType = cmdAssetType.ExecuteReader();
                while (readerAssetType.Read())
                {

                    assetType.AssetTypeId = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("asset_type_id")) ? 0 : readerAssetType.GetInt32(readerAssetType.GetOrdinal("asset_type_id"));
                    assetType.AssetTypeName = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("asset_type_name")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("asset_type_name"));


                }
                con.Close();
            }

            return View(assetType);
        }

        public IActionResult SaveAssetType(AssetTypeViewModel assetType)
        {
            string CS = DBConnection.ConnectionString;

            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmdAssetType = new SqlCommand("SaveAssetType", con);
                cmdAssetType.CommandType = CommandType.StoredProcedure;
                cmdAssetType.Parameters.AddWithValue("asset_type_id", assetType.AssetTypeId);
                cmdAssetType.Parameters.AddWithValue("asset_type_name", assetType.AssetTypeName);
                con.Open();

                cmdAssetType.ExecuteNonQuery();
                con.Close();

            }
            return RedirectToAction("Index");
        }

        public IActionResult DeleteAssetType(int AssetTypeId)
        {
            string CS = DBConnection.ConnectionString;

            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmdAssetType = new SqlCommand("DeleteAssetType", con);
                cmdAssetType.CommandType = CommandType.StoredProcedure;
                cmdAssetType.Parameters.AddWithValue("asset_type_id", AssetTypeId);
                con.Open();

                cmdAssetType.ExecuteNonQuery();
                con.Close();

            }
            return RedirectToAction("Index");
        }
    }
}