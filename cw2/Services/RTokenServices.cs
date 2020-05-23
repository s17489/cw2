using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using cw2.Models;

namespace cw2.Services
{
    public class RTokenServices : IRTokenServices
    {
        private string SqlConn = "Data Source=db-mssql;Initial Catalog=s17489;Integrated Security=True";

        public bool CheckToken(Guid token)
        {
            using (SqlConnection connection = new SqlConnection(SqlConn))
            using (SqlCommand command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "SELECT * from RToken WHERE Token = @token"; ;
                command.Parameters.AddWithValue("@token", token);
                var reader = command.ExecuteReader();
                if (!reader.Read())
                {
                    return false;
                }

                return true;
            }
        }

        public void SetToken(Guid token)
        {
            using (SqlConnection connection = new SqlConnection(SqlConn))
            using (SqlCommand command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "INSERT INTO RToken VALUES(@token)";
                command.Parameters.AddWithValue("@token", token);
                command.ExecuteNonQuery();
            }
        }
    }
}
