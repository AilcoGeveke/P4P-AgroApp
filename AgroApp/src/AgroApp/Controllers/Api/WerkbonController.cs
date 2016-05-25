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

        //Machines
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
            string query = "SELECT naam, nummer, kenteken, idMachines FROM Machine WHERE isDeleted=@0";
            List<Machine> data = new List<Machine>();
            using (MySqlConnection conn = await DatabaseConnection.GetConnection())
            using (MySqlDataReader reader = await MySqlHelper.ExecuteReaderAsync(conn, query,
                new MySqlParameter("@0", false)))
                while (reader.Read())
                    data.Add(new Machine(idMachine: reader.GetInt32(3),kenteken: reader.GetString(2), nummer: reader.GetInt32(1), naam: reader.GetString(0)));
            return JsonConvert.SerializeObject(data);
        }

        [HttpGet("addmachine/{naam}/{nummer}/{kenteken}/{type}")]
        public async Task<bool> AddMachine(string naam, string type, int nummer = 0, string kenteken = "")
        {
            using (MySqlConnection conn = await DatabaseConnection.GetConnection())
            {
                string query = "SELECT * FROM Machine WHERE nummer=@0";
                using (MySqlDataReader reader = await MySqlHelper.ExecuteReaderAsync(conn, query,
                    new MySqlParameter("@0", nummer)))
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

        [HttpGet("editmachine/{id}/{naam}/{nummer}/{kenteken}/{type}")]
        public async Task<bool> EditMachine(int id, string naam, string type = "Kranen", int nummer = 0, string kenteken = "")
        {
            if (GetMachine(id) == null)
                return false;

            string query = "UPDATE Machine SET naam=@0, nummer=@1, kenteken=@2, type=@3 WHERE idMachines=@4";
            using (MySqlConnection conn = await DatabaseConnection.GetConnection())
            using (MySqlDataReader reader = await MySqlHelper.ExecuteReaderAsync(conn, query,
                new MySqlParameter("@0", naam),
                new MySqlParameter("@1", nummer),
                new MySqlParameter("@2", kenteken),
                new MySqlParameter("@3", type),
                new MySqlParameter("@4", id)))
                return reader.RecordsAffected == 1;
        }

        [HttpGet("deletemachine/{id}")]
        public async Task<bool> DeleteMachine(int id)
        {
            if (GetMachine(id) == null)
                return false;

            string query = "UPDATE Machine SET isDeleted=@0 WHERE idMachines=@1";
            using (MySqlConnection conn = await DatabaseConnection.GetConnection())
            using (MySqlDataReader reader = await MySqlHelper.ExecuteReaderAsync(conn, query,
                new MySqlParameter("@0", true),
                new MySqlParameter("@1", id)))
                return reader.RecordsAffected == 1;;
        }


        //Hulpstuk
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


        //Klanten
        public static async Task<Customer> GetKlant(int id)
        {
            if (id < 0)
                return null;

            string query = "SELECT idKlant, naam, adres FROM Klant WHERE idKlant=@0";
            using (MySqlConnection conn = await DatabaseConnection.GetConnection())
            using (MySqlDataReader reader = await MySqlHelper.ExecuteReaderAsync(conn, query,
                new MySqlParameter("@0", id)))
            {
                await reader.ReadAsync();
                return reader.HasRows ? new Customer(reader.GetInt32(0), reader.GetString(1), reader.GetString(2)) : null;
            }
        }

        [HttpGet("getklanten")]
        public async Task<string> GetKlanten()
        {
            string query = "SELECT * FROM Klant WHERE isDeleted=@0";
            List<Customer> data = new List<Customer>();
            using (MySqlConnection conn = await DatabaseConnection.GetConnection())
            using (MySqlDataReader reader = await MySqlHelper.ExecuteReaderAsync(conn, query,
                new MySqlParameter("@0", false)))
                while (reader.Read())
                    data.Add(new Customer(reader.GetInt32(0), reader.GetString(1), reader.GetString(2)));
            return JsonConvert.SerializeObject(data);

        }

        [HttpGet("getarchiefklanten")]
        public async Task<string> GetArchiefKlanten()
        {
            string query = "SELECT * FROM Klant WHERE isDeleted=@0";
            List<Customer> data = new List<Customer>();
            using (MySqlConnection conn = await DatabaseConnection.GetConnection())
            using (MySqlDataReader reader = await MySqlHelper.ExecuteReaderAsync(conn, query,
                new MySqlParameter("@0", true)))
                while (reader.Read())
                    data.Add(new Customer(reader.GetInt32(0), reader.GetString(1), reader.GetString(2)));
            return JsonConvert.SerializeObject(data);

        }

        [HttpGet("addklant/{naam}/{adres}")]
        public async Task<bool> AddKlant(string naam, string adres)
        {
            using (MySqlConnection conn = await DatabaseConnection.GetConnection())
            {
                string query = "SELECT * FROM Klant WHERE naam=@0 AND adres=@1";
                using (MySqlDataReader reader = await MySqlHelper.ExecuteReaderAsync(conn, query,
                    new MySqlParameter("@0", naam),
                    new MySqlParameter("@1", adres)))
                    if (reader.HasRows)
                        return false;

                query = "INSERT INTO Klant (naam, adres) VALUES (@0, @1)";
                using (MySqlDataReader reader = await MySqlHelper.ExecuteReaderAsync(conn, query,
                    new MySqlParameter("@0", naam),
                    new MySqlParameter("@1", adres)))
                    return reader.RecordsAffected == 1;
            }
        }

        [HttpGet("editklant/{id}/{naam}/{adres}")]
        public async Task<bool> EditKlant(int id, string naam, string adres)
        {
            if (GetKlant(id) == null)
                return false;

            string query = "UPDATE Klant SET naam=@0, adres=@1 WHERE idKlant=@2";
            using (MySqlConnection conn = await DatabaseConnection.GetConnection())
            using (MySqlDataReader reader = await MySqlHelper.ExecuteReaderAsync(conn, query,
                new MySqlParameter("@0", naam),
                new MySqlParameter("@1", adres),
                new MySqlParameter("@2", id)))
                return reader.RecordsAffected == 1;
        }

        [HttpGet("deleteklant/{id}")]
        public async Task<bool> DeleteKlant(int id)
        {
            bool isDeleted = true;
            if (GetKlant(id) == null)
                return false;

            string query = "UPDATE Klant SET isDeleted=@0 WHERE idKlant=@1";
            using (MySqlConnection conn = await DatabaseConnection.GetConnection())
            using (MySqlDataReader reader = await MySqlHelper.ExecuteReaderAsync(conn, query,
                new MySqlParameter("@0", isDeleted),
                new MySqlParameter("@1", id)))
                return reader.RecordsAffected == 1; ;
        }

        [HttpGet("klant/terughalen/{id}")]
        public async Task<bool> ReAddKlant(int id)
        {
            if (GetKlant(id) == null)
                return false;

            string query = "UPDATE Klant SET isDeleted=@0 WHERE idKlant=@1";
            using (MySqlConnection conn = await DatabaseConnection.GetConnection())
            using (MySqlDataReader reader = await MySqlHelper.ExecuteReaderAsync(conn, query,
                new MySqlParameter("@0", false),
                new MySqlParameter("@1", id)))
                return reader.RecordsAffected == 1;
        }

        //Opdracht
        [HttpGet("addopdracht")]
        public async Task<bool> AddOpdracht()
        {
            using (MySqlConnection conn = await DatabaseConnection.GetConnection())
            {
                string query = "SELECT * FROM Machine WHERE nummer=@0";
                using (MySqlDataReader reader = await MySqlHelper.ExecuteReaderAsync(conn, query))

                    if (reader.HasRows)
                        return false;

                query = "INSERT INTO Opdracht (locatie, beschrijving, idklant) VALUES (@0, @1, @2, @3, @4)";

                using (MySqlDataReader reader = await MySqlHelper.ExecuteReaderAsync(conn, query,
                    new MySqlParameter("@4", "")))
                    return reader.RecordsAffected == 1;
            }
            throw new NotImplementedException();
        }




        //[HttpGet("getcollegakeuze")]
        //public async Task<IEnumerable<string>> GetCollegaKeuze()
        //{
        //    using (MySqlConnection con = DatabaseConnection.GetConnection())
        //    {
        //        con.Open();
        //        string query = "SELECT naam FROM Werknemer";
        //        using (MySqlCommand cmd = new MySqlCommand(query, con))
        //        {
        //            List<string> data = new List<string>();
        //            DbDataReader reader = await cmd.ExecuteReaderAsync();
        //            while (reader.Read())
        //                data.Add(reader.GetString(0));
        //            return data;
        //        }
        //    }
        //}
    }
}
