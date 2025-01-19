using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using sams.Models;

namespace sams.Controllers
{
    public class PropertyTypeController : Controller
    {
        public IActionResult Index()
        {
            List<PropertyTypeViewModel> propertyTypeList = new List<PropertyTypeViewModel>();

            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmdAssetType = new SqlCommand("GetPropertyTypeList", con);
                cmdAssetType.CommandType = CommandType.StoredProcedure;
                con.Open();

                SqlDataReader readerAssetType = cmdAssetType.ExecuteReader();
                while (readerAssetType.Read())
                {
                    var propertyType = new PropertyTypeViewModel();
                    propertyType.PropertyTypeId = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("property_type_id")) ? 0 : readerAssetType.GetInt32(readerAssetType.GetOrdinal("property_type_id"));
                    propertyType.PropertyTypeName = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("property_type_name")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("property_type_name"));

                    propertyTypeList.Add(propertyType);
                }
                con.Close();
            }
            return View(propertyTypeList);

        }

        public IActionResult AddPropertyType(int propertyTypeId)
        {
            string CS = DBConnection.ConnectionString;
            var propertyType = new PropertyTypeViewModel();

            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmdAssetType = new SqlCommand("GetPropertyTypeById", con);
                cmdAssetType.CommandType = CommandType.StoredProcedure;
                cmdAssetType.Parameters.AddWithValue("property_type_id", propertyTypeId);
                con.Open();

                SqlDataReader readerAssetType = cmdAssetType.ExecuteReader();
                while (readerAssetType.Read())
                {

                    propertyType.PropertyTypeId = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("property_type_id")) ? 0 : readerAssetType.GetInt32(readerAssetType.GetOrdinal("property_type_id"));
                    propertyType.PropertyTypeName = readerAssetType.IsDBNull(readerAssetType.GetOrdinal("property_type_name")) ? "" : readerAssetType.GetString(readerAssetType.GetOrdinal("property_type_name"));


                }
                con.Close();
            }

            return View(propertyType);
        }

        public IActionResult SavePropertyType(PropertyTypeViewModel propertyType)
        {
            string CS = DBConnection.ConnectionString;

            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmdAssetType = new SqlCommand("SavePropertyType", con);
                cmdAssetType.CommandType = CommandType.StoredProcedure;
                cmdAssetType.Parameters.AddWithValue("property_type_id", propertyType.PropertyTypeId);
                cmdAssetType.Parameters.AddWithValue("property_type_name", propertyType.PropertyTypeName);
                con.Open();

                cmdAssetType.ExecuteNonQuery();
                con.Close();
            }

            return RedirectToAction("Index");
        }

        public IActionResult DeletePropertyType(int propertyTypeId)
        {
            string CS = DBConnection.ConnectionString;

            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmdAssetType = new SqlCommand("DeletePropertyType", con);
                cmdAssetType.CommandType = CommandType.StoredProcedure;
                cmdAssetType.Parameters.AddWithValue("property_type_id", propertyTypeId);
                con.Open();

                cmdAssetType.ExecuteNonQuery();
                con.Close();
            }

            return RedirectToAction("Index");
        }
    }
}