using AgroApp.Controllers;
using AgroApp.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace AgroApp.Managers
{
    public static class MachineManager
    {
        public static async Task<Machine> GetMachine(int id)
        {
            if (id < 0)
                return null;

            using (MySqlConnection con = DatabaseConnection.GetConnection())
            {
                con.Open();
                string query = "SELECT idMachines, type, nummer, naam, kenteken, status FROM Machine WHERE idMachines=@0";
                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@0", id);
                    DbDataReader reader = await cmd.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        return new Machine(reader.GetInt32(0), reader.GetString(1), reader.GetInt32(2), reader.GetString(3), reader.GetString(4), reader.GetString(5));
                    }
                }
            }

            return null;
        }
    }
}
