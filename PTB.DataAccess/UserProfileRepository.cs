using PTB.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTB.DataAccess
{
    public class UserProfileRepository
    {
        protected string PTBConnectionString { get; set; }

        public UserProfileRepository()
        {
            PTBConnectionString = ConfigurationManager.ConnectionStrings["PTBConnectionString"].ConnectionString;
        }        
        public UserDetailModel GetUser(int userId)
        {
            using (var conn = new SqlConnection(PTBConnectionString))
            {
                conn.Open();

                using (var cmd = new SqlCommand("GetUser", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = userId;

                    var myReader = cmd.ExecuteReader();

                    UserDetailModel user = null;
                    try
                    {
                        if (myReader.Read())
                        {
                            user = new UserDetailModel(myReader);
                        }

                    }
                    catch (Exception ex)
                    {
                        // LOG ERROR
                        throw ex;
                    }
                    return user;
                }
            }
        }

        public UserDetailModel GetUser(string userName)
        {
            var userId = GetUserIdByUserName(userName);
            if (userId != -1)
                return GetUser(userId);

            return null;
        }

        public int GetUserIdByUserName(string userName)
        {
            var userId = 0;
            using (var conn = new SqlConnection(PTBConnectionString))
            {
                conn.Open();

                using (var cmd = new SqlCommand("GetUserIdByUserName", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@UserName", SqlDbType.VarChar).Value = userName;

                    try
                    {
                        userId = (int)cmd.ExecuteScalar();
                    }
                    catch (Exception ex)
                    {
                        // LOG ERROR
                        userId = -1;
                    }
                }
            }
            return userId;
        }
    }
}
