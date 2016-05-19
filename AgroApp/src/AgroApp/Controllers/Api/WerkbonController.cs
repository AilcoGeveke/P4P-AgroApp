using AgroApp.Models;
using Microsoft.AspNet.Mvc;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
            string query = "SELECT naam FROM Mankeuze";
            List<string> data = new List<string>();
            using (MySqlConnection conn = await DatabaseConnection.GetConnection())
            using (MySqlDataReader reader = await MySqlHelper.ExecuteReaderAsync(conn, query))
                while (await reader.ReadAsync())
                    data.Add(reader.GetString(0));
            return data;
        }

        [HttpGet("getmachine")]
        private async Task<Machine> _GetMachine(int id)
        {
            return await GetMachine(id);
        }

        public static async Task<Machine> GetMachine(int id)
        {
            if (id < 0)
                return null;

            string query = "SELECT idMachines, type, nummer, naam, kenteken, status FROM Machine WHERE idMachines=@0";
            using (MySqlConnection conn = await DatabaseConnection.GetConnection())
            using (MySqlDataReader reader = await MySqlHelper.ExecuteReaderAsync(conn, query,
                new MySqlParameter("@0", id)))
            {
                await reader.ReadAsync();
                return reader.HasRows ? new Machine(reader.GetInt32(0), reader.GetString(1), reader.GetInt32(2), reader.GetString(3), reader.GetString(4), reader.GetString(5)) : null;
            }
        }

        [HttpGet("getmachines")]
        public async Task<string> GetMachines()
        {
            string query = "SELECT naam, nummer, idMachines FROM Machine";
            List<Machine> data = new List<Machine>();
            using (MySqlConnection conn = await DatabaseConnection.GetConnection())
            using (MySqlDataReader reader = await MySqlHelper.ExecuteReaderAsync(conn, query))
                while (reader.Read())
                    data.Add(new Machine(idMachine: reader.GetInt32(2), nummer: reader.GetInt32(1), naam: reader.GetString(0)));
            return JsonConvert.SerializeObject(data);
        }

        [HttpGet("addmachine/{naam}/{nummer}/{kenteken}/{type}")]
        public async Task<bool> AddMachine(string naam, string type, int nummer = 0, string kenteken = "")
        {
            using (MySqlConnection conn = await DatabaseConnection.GetConnection())
            {
                string query = "SELECT * FROM Machine WHERE nummer=@0";
                using (MySqlDataReader reader = await MySqlHelper.ExecuteReaderAsync(conn, query))
                    if (reader.HasRows)
                        return false;

                query = "INSERT INTO Machine (naam, nummer, kenteken, type, status) VALUES (@0, @1, @2, @3, @4)";
                using (MySqlDataReader reader = await MySqlHelper.ExecuteReaderAsync(conn, query,
                    new MySqlParameter("@0", naam),
                    new MySqlParameter("@1", nummer),
                    new MySqlParameter("@2", kenteken),
                    new MySqlParameter("@3", type),
                    new MySqlParameter("@4", "")))
                    return reader.RecordsAffected == 1;
            }
        }

        [HttpGet("updatemachine")]//WERKT NIET AILCO, WAAR ZIJN DE PARAMETERS
        public async Task<bool> UpdateMachine(int id)
        {
            if (GetMachine(id) == null)
                throw new ArgumentException();

            string query = "UPDATE Machine SET naam=@0, nummer=@1, kenteken=@2, type=@3) WHERE idMachines=@4";
            using (MySqlConnection conn = await DatabaseConnection.GetConnection())
            using (MySqlDataReader reader = await MySqlHelper.ExecuteReaderAsync(conn, query))
                return reader.RecordsAffected > 0;
        }

        [HttpGet("gethulpstukken")]
        public async Task<IEnumerable<string>> GetHulpstukken()
        {
            string query = "SELECT idHulpstuk, nummer, naam FROM Hulpstuk";
            List<string> data = new List<string>();
            using (MySqlConnection conn = await DatabaseConnection.GetConnection())
            using (MySqlDataReader reader = await MySqlHelper.ExecuteReaderAsync(conn, query))
                while (reader.Read())
                    data.Add(reader.GetString(2));
            return data;
        }
    }
}
