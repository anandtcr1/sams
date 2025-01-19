using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using sams.Models;

namespace sams.Controllers
{
    public class RegionController : Controller
    {
        public IActionResult Index()
        {
            List<RegionViewModel> regionList = new List<RegionViewModel>();

            string CS = DBConnection.ConnectionString;

            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("GetRegionList", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var regionModel = new RegionViewModel();
                    regionModel.RegionId = reader.IsDBNull(reader.GetOrdinal("region_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("region_id"));
                    regionModel.StateId = reader.IsDBNull(reader.GetOrdinal("state_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("state_id"));
                    regionModel.StateName = reader.IsDBNull(reader.GetOrdinal("state_name")) ? "" : reader.GetString(reader.GetOrdinal("state_name"));
                    regionModel.RegionName = reader.IsDBNull(reader.GetOrdinal("region_name")) ? "" : reader.GetString(reader.GetOrdinal("region_name"));
                    
                    regionList.Add(regionModel);
                }
                con.Close();
            }

            return View(regionList);
        }

        public IActionResult ManageRegion(int regionId)
        {
            string CS = DBConnection.ConnectionString;
            var regionModel = new RegionViewModel();

            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("GetRegionById", con);
                cmd.Parameters.AddWithValue("region_id", regionId);

                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    
                    regionModel.RegionId = reader.IsDBNull(reader.GetOrdinal("region_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("region_id"));
                    regionModel.StateId = reader.IsDBNull(reader.GetOrdinal("state_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("state_id"));
                    regionModel.StateName = reader.IsDBNull(reader.GetOrdinal("state_name")) ? "" : reader.GetString(reader.GetOrdinal("state_name"));
                    regionModel.RegionName = reader.IsDBNull(reader.GetOrdinal("region_name")) ? "" : reader.GetString(reader.GetOrdinal("region_name"));

                }
                regionModel.StateList = GetStateList();
                con.Close();
            }

            return View(regionModel);
        }

        [HttpPost]
        public RedirectToActionResult SaveRegion(RegionViewModel regionModel)
        {
            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("SaveRegion", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("region_id", regionModel.RegionId);
                cmd.Parameters.AddWithValue("state_id", regionModel.StateId);
                cmd.Parameters.AddWithValue("region_name", regionModel.RegionName);
                con.Open();

                regionModel.RegionId = int.Parse(cmd.ExecuteScalar().ToString());


            }
            //return RedirectToAction("ManageRegion", new { regionId = regionModel.RegionId });
            return RedirectToAction("Index");
        }

        public RedirectToActionResult DeleteRegion(int regionId)
        {
            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("DeleteRegion", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("region_id", regionId);
                con.Open();

                cmd.ExecuteNonQuery();
                con.Close();
            }
            return RedirectToAction("Index");
        }

        static List<StateDetails> GetStateList()
        {
            List<StateDetails> stateList = new List<StateDetails>();

            string CS = DBConnection.ConnectionString;

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

        public IActionResult GetRegionByStateId(int stateId)
        {
            List<RegionViewModel> regionList = new List<RegionViewModel>();

            string CS = DBConnection.ConnectionString;

            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("GetRegionList", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    
                    var regionModel = new RegionViewModel();

                    regionModel.RegionId = reader.IsDBNull(reader.GetOrdinal("region_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("region_id"));
                    regionModel.StateId = reader.IsDBNull(reader.GetOrdinal("state_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("state_id"));
                    regionModel.StateName = reader.IsDBNull(reader.GetOrdinal("state_name")) ? "" : reader.GetString(reader.GetOrdinal("state_name"));
                    regionModel.RegionName = reader.IsDBNull(reader.GetOrdinal("region_name")) ? "" : reader.GetString(reader.GetOrdinal("region_name"));

                    if(regionModel.StateId == stateId)
                    {
                        regionList.Add(regionModel);
                    }
                    
                }
                con.Close();
            }

            var SubCategory_List = regionList.Where(s => s.StateId == stateId).Select(c => new { Id = c.RegionId, Name = c.RegionName }).ToList();
            return Json(SubCategory_List);

            //return Json(regionList);
        }
    }
}