using sams.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace sams.Controllers
{
    public class PropertyHistory
    {
        public static void SavePropertyHistory(PropertyHistoryModel propertyHistory)
        {
            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("SavePropertyHistory", con);
                cmd.CommandType = CommandType.StoredProcedure;
                
                cmd.Parameters.AddWithValue("property_id", propertyHistory.PropertyId);
                cmd.Parameters.AddWithValue("status_id", propertyHistory.StatusId);
                cmd.Parameters.AddWithValue("description", propertyHistory.Description);
                cmd.Parameters.AddWithValue("changed_by", propertyHistory.LoggedInId);
                cmd.Parameters.AddWithValue("transaction_id", propertyHistory.TransactionId);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        public static List<PropertyHistoryModel> GetPropertyHistoryList(int propertyId)
        {
            var propertyHistoryList = new List<PropertyHistoryModel>();

            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("GetPropertyHistory", con);
                cmd.Parameters.AddWithValue("property_id", propertyId);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var propertyHistory = new PropertyHistoryModel();

                    propertyHistory.PropertyHistoryId = reader.IsDBNull(reader.GetOrdinal("property_history_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("property_history_id"));
                    propertyHistory.PropertyId = reader.IsDBNull(reader.GetOrdinal("property_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("property_id"));
                    propertyHistory.StatusName = reader.IsDBNull(reader.GetOrdinal("transaction_status_name")) ? "" : reader.GetString(reader.GetOrdinal("transaction_status_name"));
                    propertyHistory.Description = reader.IsDBNull(reader.GetOrdinal("description")) ? "" : reader.GetString(reader.GetOrdinal("description"));
                    propertyHistory.LoggedInUserName = reader.IsDBNull(reader.GetOrdinal("user_name")) ? "" : reader.GetString(reader.GetOrdinal("user_name"));
                    propertyHistory.CreatedDate = reader.IsDBNull(reader.GetOrdinal("created_date")) ? DateTime.Now : reader.GetDateTime(reader.GetOrdinal("created_date"));
                    propertyHistory.TransactionId = reader.IsDBNull(reader.GetOrdinal("transaction_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("transaction_id"));

                    propertyHistoryList.Add(propertyHistory);
                }
            }
            return propertyHistoryList;
        }

        public static void DeletePropertyHistory(int historyId)
        {
            string CS = DBConnection.ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("DeletePropertyHistory", con);
                cmd.Parameters.AddWithValue("property_history_id", historyId);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }
    }
}
