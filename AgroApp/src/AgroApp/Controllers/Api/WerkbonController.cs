using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using MySql.Data.MySqlClient;
using System.Data.Common;
using AgroApp.Models;

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
