using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using MySql.Data.MySqlClient;
using AgroApp.Models;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace AgroApp.Controllers.Api
{
    [Route("api/[controller]")]
    public class MachineController : Controller
    {
        [HttpPost("add")]
        public async Task<bool> AddMachine([FromBody]Machine machine)
        {
            using (MySqlConnection conn = await DatabaseConnection.GetConnection())
            {
                string query = "SELECT * FROM Machine WHERE number=@0";
                using (MySqlDataReader reader = await MySqlHelper.ExecuteReaderAsync(conn, query,
                    new MySqlParameter("@0", machine.Number)))
                    if (reader.HasRows)
                        return false;

                query = "INSERT INTO Machine (name, number, tag, type, status) VALUES (@0, @1, @2, @3, @4)";
                using (MySqlDataReader reader = await MySqlHelper.ExecuteReaderAsync(conn, query,
                    new MySqlParameter("@0", machine.Name),
                    new MySqlParameter("@1", machine.Number),
                    new MySqlParameter("@2", machine.Tag),
                    new MySqlParameter("@3", machine.Type),
                    new MySqlParameter("@4", machine.Status)))
                    return reader.RecordsAffected == 1;
            }
        }

        [HttpGet("getall")]
        public async Task<List<Machine>> GetMachines()
        {
            string query = "SELECT name, number, tag, idMachine, status FROM Machine WHERE isDeleted=@0";
            List<Machine> data = new List<Machine>();
            using (MySqlConnection conn = await DatabaseConnection.GetConnection())
            using (MySqlDataReader reader = await MySqlHelper.ExecuteReaderAsync(conn, query,
                new MySqlParameter("@0", false)))
                while (reader.Read())
                    data.Add(new Machine(idMachine: reader.GetInt32(3), tag: reader["tag"] as string, number: reader.GetInt32(1), name: reader.GetString(0), status: reader["status"] as string));
            return data;
        }

        public static async Task<Machine> GetMachine(int id)
        {
            if (id < 0)
                return null;

            string query = "SELECT idMachine, type, number, name, tag, status FROM Machine WHERE idMachine=@0";
            using (MySqlConnection conn = await DatabaseConnection.GetConnection())
            using (MySqlDataReader reader = await MySqlHelper.ExecuteReaderAsync(conn, query,
                new MySqlParameter("@0", id)))
            {
                await reader.ReadAsync();
                return reader.HasRows ? new Machine(reader["idMachine"] as int? ?? -1, reader["type"] as string, reader["number"] as int? ?? -1, reader["name"] as string, reader["tag"] as string, reader["status"] as string) : null;
            }
        }
    }
}
