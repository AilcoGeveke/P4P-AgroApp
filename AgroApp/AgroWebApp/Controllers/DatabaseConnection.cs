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
            string connectionString = "Database=agrowebappdatabase;Data Source=eu-cdbr-azure-west-d.cloudapp.net;User Id=b8bbed4ed57e75;Password=e02aa9bb;pooling=true;maxpoolsize=2";
            MySqlConnection conn = new MySqlConnection(connectionString);
            await conn.OpenAsync();
            return conn;
        }
    }
}
