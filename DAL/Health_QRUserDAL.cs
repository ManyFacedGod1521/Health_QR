using Health_QR.Areas.Identity.Data;
using Health_QR.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Health_QR.DAL
{
    public class Health_QRUserDAL
    {

        private string _connectionString = "Data Source=LAPTOP-G3KL2AFQ;Initial Catalog=health_QR;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        public string GetUserByUserName(string searchParameter)
        {
            string commandText = $"SELECT [Id], [UserName] FROM [dbo].[AspNetUsers] WHERE UserName LIKE '%{searchParameter}%'";
            SqlConnection connection = new SqlConnection(_connectionString);
            SqlCommand command = new SqlCommand(commandText, connection as SqlConnection);

            Health_QRUser CurrentUser = new Health_QRUser();

            using (var adapter = new SqlDataAdapter(command))
            {
                var resultTable = new DataTable();
                adapter.Fill(resultTable);

                foreach (var row in resultTable.AsEnumerable())
                {
                    Health_QRUser user = new Health_QRUser()
                    {
                        Id = row["ID"].ToString(),
                        UserName = row["UserName"].ToString(),

                    };
                    CurrentUser = user;
                }
            }
            return CurrentUser.Id;
        }
    }
}
