using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using sams.Common;
using sams.Models;

namespace sams.Controllers
{
    public class SamsRoleController : Controller
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        public SamsRoleController(IWebHostEnvironment hostEnvironment)
        {
            webHostEnvironment = hostEnvironment;
        }

        public IActionResult Index()
        {
            UserViewModel loggedInUser = HttpContext.Session.GetObjectFromJson<UserViewModel>("LoggedInAdmin");
            if (loggedInUser == null)
            {
                return RedirectToAction("DoLogin", "Admin");
            }
            if (loggedInUser != null && loggedInUser.UserId <= 0)
            {
                return RedirectToAction("DoLogin", "Admin");
            }

            var roleList = new List<RoleViewModel>();

            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("GetRoles", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var nRole = new RoleViewModel();

                    nRole.RoleId = reader.IsDBNull(reader.GetOrdinal("role_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("role_id"));
                    nRole.RoleName = reader.IsDBNull(reader.GetOrdinal("role_name")) ? "" : reader.GetString(reader.GetOrdinal("role_name"));
                    nRole.CanPublishListings = reader.IsDBNull(reader.GetOrdinal("can_publish_listing")) ? false : reader.GetBoolean(reader.GetOrdinal("can_publish_listing"));
                    roleList.Add(nRole);
                }
            }
            return View(roleList);
        }

        public IActionResult AddRole(int roleId)
        {
            UserViewModel loggedInUser = HttpContext.Session.GetObjectFromJson<UserViewModel>("LoggedInAdmin");
            if (loggedInUser == null)
            {
                return RedirectToAction("DoLogin", "Admin");
            }
            if (loggedInUser != null && loggedInUser.UserId <= 0)
            {
                return RedirectToAction("DoLogin", "Admin");
            }

            string CS = DBConnection.ConnectionString;
            var nRole = new RoleViewModel();

            var rolePermissionList = new List<RolePermissionViewModel>();

            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("GetRoleById", con);
                cmd.Parameters.AddWithValue("role_id", roleId);

                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    nRole.RoleId = reader.IsDBNull(reader.GetOrdinal("role_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("role_id"));
                    nRole.RoleName = reader.IsDBNull(reader.GetOrdinal("role_name")) ? "" : reader.GetString(reader.GetOrdinal("role_name"));
                    nRole.CanPublishListings = reader.IsDBNull(reader.GetOrdinal("can_publish_listing")) ? false : reader.GetBoolean(reader.GetOrdinal("can_publish_listing"));
                }
            }


            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("GetRolePermission", con);
                cmd.Parameters.AddWithValue("role_id", roleId);

                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var rPermission = new RolePermissionViewModel();

                    rPermission.RolePermissionId = reader.IsDBNull(reader.GetOrdinal("role_permission_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("role_permission_id"));
                    rPermission.ModuleId = reader.IsDBNull(reader.GetOrdinal("module_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("module_id"));
                    rPermission.RoleId = reader.IsDBNull(reader.GetOrdinal("role_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("role_id"));

                    rPermission.ModuleName = reader.IsDBNull(reader.GetOrdinal("module_name")) ? "" : reader.GetString(reader.GetOrdinal("module_name"));
                    
                    rPermission.CanRead = reader.IsDBNull(reader.GetOrdinal("can_read")) ? false : reader.GetBoolean(reader.GetOrdinal("can_read"));
                    rPermission.CanEdit = reader.IsDBNull(reader.GetOrdinal("can_edit")) ? false : reader.GetBoolean(reader.GetOrdinal("can_edit"));
                    rPermission.CanCreate = reader.IsDBNull(reader.GetOrdinal("can_create")) ? false : reader.GetBoolean(reader.GetOrdinal("can_create"));
                    rPermission.CanDelete = reader.IsDBNull(reader.GetOrdinal("can_delete")) ? false : reader.GetBoolean(reader.GetOrdinal("can_delete"));

                    rolePermissionList.Add(rPermission);

                }
            }

            ModuleRolePermissionViewModel modulePermission = new ModuleRolePermissionViewModel();
            modulePermission.SamsRole = nRole;
            modulePermission.RolePermissionList = rolePermissionList;

            return View(modulePermission);
        }

        [HttpPost]
        public IActionResult SaveRole(ModuleRolePermissionViewModel permissionList)
        {
            UserViewModel loggedInUser = HttpContext.Session.GetObjectFromJson<UserViewModel>("LoggedInAdmin");
            if (loggedInUser == null)
            {
                return RedirectToAction("DoLogin", "Admin");
            }
            if (loggedInUser != null && loggedInUser.UserId <= 0)
            {
                return RedirectToAction("DoLogin", "Admin");
            }

            if (permissionList.SamsRole.RoleName == "Admin")
            {
                return RedirectToAction("Index");
            }

            string CS = DBConnection.ConnectionString;
            var nRole = new RoleViewModel();

            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("SaveRole", con);
                cmd.Parameters.AddWithValue("role_id", permissionList.SamsRole.RoleId);
                cmd.Parameters.AddWithValue("role_name", permissionList.SamsRole.RoleName);
                cmd.Parameters.AddWithValue("can_publish_listing", permissionList.SamsRole.CanPublishListings);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                permissionList.SamsRole.RoleId = int.Parse(cmd.ExecuteScalar().ToString());
                con.Close();

                foreach (RolePermissionViewModel rp in permissionList.RolePermissionList)
                {
                    SqlCommand cmdRolePermission = new SqlCommand("SaveRolePermission", con);
                    cmdRolePermission.Parameters.AddWithValue("role_id", permissionList.SamsRole.RoleId);
                    cmdRolePermission.Parameters.AddWithValue("module_id", rp.ModuleId);
                    
                    cmdRolePermission.Parameters.AddWithValue("can_read", rp.CanRead);
                    cmdRolePermission.Parameters.AddWithValue("can_edit", rp.CanEdit);
                    cmdRolePermission.Parameters.AddWithValue("can_create", rp.CanCreate);
                    cmdRolePermission.Parameters.AddWithValue("can_delete", rp.CanDelete);

                    cmdRolePermission.CommandType = CommandType.StoredProcedure;
                    con.Open();

                    cmdRolePermission.ExecuteNonQuery();
                    con.Close();
                }

                /*
                foreach(RolePermissionViewModel rp in permissionList)
                {
                    bool s1 = rp.CanRead;
                    bool s2 = rp.CanCreate;
                }

                
                SqlCommand cmd = new SqlCommand("SaveRole", con);
                cmd.Parameters.AddWithValue("role_id", roleModel.SamsRole.RoleId);
                cmd.Parameters.AddWithValue("role_name", roleModel.SamsRole.RoleName);

                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                roleModel.SamsRole.RoleId = int.Parse(cmd.ExecuteScalar().ToString());
                con.Close();
                */
            }
            //return RedirectToAction("AddRole", new { roleId = roleModel.RoleId });
            return RedirectToAction("Index");
        }

        public IActionResult DeleteRole(int roleId)
        {
            string CS = DBConnection.ConnectionString;
            var nRole = new RoleViewModel();
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("DeleteRole", con);
                cmd.Parameters.AddWithValue("role_id", roleId);

                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                cmd.ExecuteNonQuery();
                
            }

            return RedirectToAction("Index");
        }
    }
}