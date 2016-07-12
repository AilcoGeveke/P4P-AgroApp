using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AgroApp.Controllers
{
    public static class DatabaseConnection
    {
        public static async Task<MySqlConnection> GetConnection()
        {
            string connectionString = "Database=agrowebdatabase;Data Source=eu-cdbr-azure-west-d.cloudapp.net;User Id=b899ef5ed45ce9;Password=8d16616f;pooling=false";
            MySqlConnection conn = new MySqlConnection(connectionString);
            await conn.OpenAsync();
            return conn;
        }
    }
}
