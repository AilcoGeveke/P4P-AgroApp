using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using MySql.Data.MySqlClient;
using System.Data.Common;
using AgroApp.Models;
using AgroApp.Managers;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace AgroApp.Controllers.Api
{
    [Route("api/[controller]")]
    public class WerkbonController : Controller
    {
        // GET: api/values
        [HttpGet("getmankeuze")]
        public async Task<IEnumerable<string>> GetManKeuze()
        {
            using (MySqlConnection con = DatabaseConnection.GetConnection())
            {
                con.Open();
                string query = "SELECT naam FROM Mankeuze";
                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {
                    List<string> data = new List<string>();
                    DbDataReader reader = await cmd.ExecuteReaderAsync();
                    while (reader.Read())
                        data.Add(reader.GetString(0));
                    return data;
                }
            }
        }

        [HttpGet("getmachine")]
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

        [HttpGet("getmachines")]
        public async Task<string> GetMachines()
        {
            using (MySqlConnection con = DatabaseConnection.GetConnection())
            {
                con.Open();
                string query = "SELECT naam, nummer, idMachines FROM Machine";
                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {
                    List<Machine> data = new List<Machine>();
                    DbDataReader reader = await cmd.ExecuteReaderAsync();
                    while (reader.Read())
                        data.Add(new Machine(idMachine: reader.GetInt32(2), nummer: reader.GetInt32(1), naam: reader.GetString(0)));
                    return Newtonsoft.Json.JsonConvert.SerializeObject(data);
                }
            }
        }

        [HttpGet("addmachine/{naam}/{nummer}/{kenteken}/{type}")]
        public async Task<bool> AddMachine(string naam, string type, int nummer = 0, string kenteken = "")
        {
            using (MySqlConnection con = DatabaseConnection.GetConnection())
            {
                con.Open();
                string query = "SELECT * FROM Machine WHERE nummer=@0";
                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@0", nummer);
                    DbDataReader reader = await cmd.ExecuteReaderAsync();
                    reader.Read();
                    if (reader.HasRows)
                        return false;
                    reader.Close();
                }
                
                query = "INSERT INTO Machine (naam, nummer, kenteken, type, status) "
                                + "VALUES (@0, @1, @2, @3, @4)";
                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@0", naam);
                    cmd.Parameters.AddWithValue("@1", nummer);
                    cmd.Parameters.AddWithValue("@2", kenteken);
                    cmd.Parameters.AddWithValue("@3", type);
                    cmd.Parameters.AddWithValue("@4", "");
                    DbDataReader reader = await cmd.ExecuteReaderAsync();
                    reader.Read();
                    return true;
                }
            }
        }

        [HttpGet("updatemachine")]
        public async Task<string> UpdateMachine(int id)
        {
            if (GetMachine(id) == null)
                return "error";

            using (MySqlConnection con = DatabaseConnection.GetConnection())
            {
                con.Open();
                string query = "UPDATE Machine SET naam=@0, nummer=@1, kenteken=@2, type=@3) "
                                + "WHERE idMachines=@4";
                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {
                    List<Machine> data = new List<Machine>();
                    DbDataReader reader = await cmd.ExecuteReaderAsync();
                    while (reader.Read())
                        data.Add(new Machine(idMachine: reader.GetInt32(2), nummer: reader.GetInt32(1), naam: reader.GetString(0)));
                    return Newtonsoft.Json.JsonConvert.SerializeObject(data);
                }
            }
        }
        
        [HttpGet("getcollegakeuze")]
        public async Task<IEnumerable<string>> GetCollegaKeuze()
        {
            using (MySqlConnection con = DatabaseConnection.GetConnection())
            {
                con.Open();
                string query = "SELECT naam FROM Werknemer";
                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {
                    List<string> data = new List<string>();
                    DbDataReader reader = await cmd.ExecuteReaderAsync();
                    while (reader.Read())
                        data.Add(reader.GetString(0));
                    return data;
                }
            }
        }
    }


}
