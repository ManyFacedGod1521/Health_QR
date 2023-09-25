using Health_QR.Models;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Numerics;

namespace Health_QR.DAL
{
    public class RolesDAL
    {
        private string _connectionString = "Data Source=LAPTOP-G3KL2AFQ;Initial Catalog=health_QR;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        public string GetRoleByName(string searchParameter)
        {
            string commandText = $"SELECT [Id], [Name] FROM [dbo].[AspNetRoles] WHERE Name LIKE '%{searchParameter}%'";
            SqlConnection connection = new SqlConnection(_connectionString);
            SqlCommand command = new SqlCommand(commandText, connection as SqlConnection);

            Role CurrentRole = new Role();

            using (var adapter = new SqlDataAdapter(command))
            {
                var resultTable = new DataTable();
                adapter.Fill(resultTable);

                foreach (var row in resultTable.AsEnumerable())
                {
                    Role role = new Role()
                    {
                        Id = row["ID"].ToString(),
                        Name = row["Name"].ToString(),

                    };
                    CurrentRole = role;
                }
            }
            return CurrentRole.Id;
        }

        public bool AddUserRole(string UserId, string RoleId)
        {
            string commandText =
                $"INSERT INTO [dbo].[AspNetUserRoles] ([UserId],[RoleId])" +
                $"VALUES (@UserId, @RoleId)";

            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter("UserId", UserId),
                new SqlParameter("RoleId", RoleId),
            };

            int result = runQuery(commandText, parameters);

            return result == 1
                ? true : false;
        }
        private int runQuery(string commandText, List<SqlParameter> parameters)
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            SqlCommand command = new SqlCommand(commandText, connection as SqlConnection);

            parameters.ForEach(parameter => command.Parameters.Add(parameter));

            connection.Open();
            int result = command.ExecuteNonQuery();
            connection.Close();

            return result;

        }
    }
}
