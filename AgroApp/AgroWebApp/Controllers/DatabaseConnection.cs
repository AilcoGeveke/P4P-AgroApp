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
            string connectionString = "Database=acsm_c2056a9f0688b3b;Data Source=eu-cdbr-azure-west-d.cloudapp.net;User Id=bbe0ad93c4b224;Password=3b9d7513;pooling=false";
            MySqlConnection conn = new MySqlConnection(connectionString);
            await conn.OpenAsync();
            return conn;
        }
    }
}
