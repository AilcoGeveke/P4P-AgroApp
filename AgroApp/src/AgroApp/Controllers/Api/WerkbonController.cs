using AgroApp.Models;
using Microsoft.AspNet.Mvc;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
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
                    data.Add(new Machine(idMachine: reader.GetInt32(3), kenteken: reader.GetString(2), nummer: reader.GetInt32(1), naam: reader.GetString(0)));
            return JsonConvert.SerializeObject(data);
        }

        [HttpGet("getarchiefmachine")]
        public async Task<string> GetArchiefMachines()
        {
            string query = "SELECT naam, nummer, kenteken, idMachines FROM Machine WHERE isDeleted=@0";
            List<Machine> data = new List<Machine>();
            using (MySqlConnection conn = await DatabaseConnection.GetConnection())
            using (MySqlDataReader reader = await MySqlHelper.ExecuteReaderAsync(conn, query,
                new MySqlParameter("@0", true)))
                while (reader.Read())
                    data.Add(new Machine(idMachine: reader.GetInt32(3), kenteken: reader.GetString(2), nummer: reader.GetInt32(1), naam: reader.GetString(0)));
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
                return reader.RecordsAffected == 1; ;
        }

        [HttpGet("machine/terughalen/{id}")]
        public async Task<bool> ReAddMachine(int id)
        {
            if (GetMachine(id) == null)
                return false;

            string query = "UPDATE Machine SET isDeleted=@0 WHERE idMachines=@1";
            using (MySqlConnection conn = await DatabaseConnection.GetConnection())
            using (MySqlDataReader reader = await MySqlHelper.ExecuteReaderAsync(conn, query,
                new MySqlParameter("@0", false),
                new MySqlParameter("@1", id)))
                return reader.RecordsAffected == 1; ;
        }

        //Hulpstuk
        public static async Task<Hulpstuk> GetHulpstuk(int id)
        {
            if (id < 0)
                return null;

            string query = "SELECT idHulpstuk, naam, nummer FROM Hulpstuk WHERE idHulpstuk=@0";
            using (MySqlConnection conn = await DatabaseConnection.GetConnection())
            using (MySqlDataReader reader = await MySqlHelper.ExecuteReaderAsync(conn, query,
                new MySqlParameter("@0", id)))
            {
                await reader.ReadAsync();
                return reader.HasRows ? new Hulpstuk(idHulpstuk: reader.GetInt32(0), naam: reader.GetString(1), nummer: reader.GetInt32(2)) : null;
            }
        }

        [HttpGet("gethulpstukken")]
        public async Task<string> GetHulpstukken()
        {
            string query = "SELECT idHulpstuk, nummer, naam, isDeleted FROM Hulpstuk WHERE isDeleted = @0";
            List<Hulpstuk> data = new List<Hulpstuk>();
            using (MySqlConnection conn = await DatabaseConnection.GetConnection())
            using (MySqlDataReader reader = await MySqlHelper.ExecuteReaderAsync(conn, query,
                new MySqlParameter("@0", false)))
                while (reader.Read())
                    data.Add(new Hulpstuk(idHulpstuk: reader.GetInt32(0), nummer: reader.GetInt32(1), naam: reader.GetString(2)));
            return JsonConvert.SerializeObject(data);
        }

        [HttpGet("getarchiefhulpstukken")]
        public async Task<string> GetArchiefHulpstukken()
        {
            string query = "SELECT idHulpstuk, nummer, naam, isDeleted FROM Hulpstuk WHERE isDeleted = @0";
            List<Hulpstuk> data = new List<Hulpstuk>();
            using (MySqlConnection conn = await DatabaseConnection.GetConnection())
            using (MySqlDataReader reader = await MySqlHelper.ExecuteReaderAsync(conn, query,
                new MySqlParameter("@0", true)))
                while (reader.Read())
                    data.Add(new Hulpstuk(idHulpstuk: reader.GetInt32(0), nummer: reader.GetInt32(1), naam: reader.GetString(2)));
            return JsonConvert.SerializeObject(data);
        }

        [HttpGet("addhulpstuk/{naam}/{nummer}")]
        public async Task<bool> AddHulpstuk(string naam, int nummer = 0)
        {
            using (MySqlConnection conn = await DatabaseConnection.GetConnection())
            {
                string query = "SELECT * FROM Hulpstuk WHERE naam=@0 AND nummer=@1";
                using (MySqlDataReader reader = await MySqlHelper.ExecuteReaderAsync(conn, query,
                    new MySqlParameter("@0", naam),
                    new MySqlParameter("@1", nummer)))
                    if (reader.HasRows)
                        return false;

                query = "INSERT INTO Hulpstuk (naam, nummer) VALUES (@0, @1)";
                using (MySqlDataReader reader = await MySqlHelper.ExecuteReaderAsync(conn, query,
                    new MySqlParameter("@0", naam),
                    new MySqlParameter("@1", nummer)))
                    return reader.RecordsAffected == 1;
            }
        }

        [HttpGet("edithulpstuk/{id}/{naam}/{nummer}")]
        public async Task<bool> EditHulpstuk(int id, string naam, string nummer)
        {
            if (GetHulpstuk(id) == null)
                return false;

            string query = "UPDATE Hulpstuk SET naam=@0, nummer=@1 WHERE idHulpstuk=@2";
            using (MySqlConnection conn = await DatabaseConnection.GetConnection())
            using (MySqlDataReader reader = await MySqlHelper.ExecuteReaderAsync(conn, query,
                new MySqlParameter("@0", naam),
                new MySqlParameter("@1", nummer),
                new MySqlParameter("@2", id)))
                return reader.RecordsAffected == 1;
        }

        [HttpGet("deletehulpstuk/{id}")]
        public async Task<bool> DeleteHulpstuk(int id)
        {
            if (GetHulpstuk(id) == null)
                return false;

            string query = "UPDATE Hulpstuk SET isDeleted=@0 WHERE idHulpstuk=@1";
            using (MySqlConnection conn = await DatabaseConnection.GetConnection())
            using (MySqlDataReader reader = await MySqlHelper.ExecuteReaderAsync(conn, query,
                new MySqlParameter("@0", true),
                new MySqlParameter("@1", id)))
                return reader.RecordsAffected == 1; ;
        }

        [HttpGet("hulpstuk/terughalen/{id}")]
        public async Task<bool> ReAddHulpstuk(int id)
        {
            if (GetHulpstuk(id) == null)
                return false;

            string query = "UPDATE Hulpstuk SET isDeleted=@0 WHERE idHulpstuk=@1";
            using (MySqlConnection conn = await DatabaseConnection.GetConnection())
            using (MySqlDataReader reader = await MySqlHelper.ExecuteReaderAsync(conn, query,
                new MySqlParameter("@0", false),
                new MySqlParameter("@1", id)))
                return reader.RecordsAffected == 1; ;
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

        // GET: /admin/werkboncheck
        [HttpGet("admin/werkbon/controleren")]
        public IActionResult Opdracht()
        {
            return View("../admin/werkbonCheck");
        }

        [HttpGet("getdata")]
        public async Task<string> GetData()
        {
            string query = "SELECT Opdracht.locatie, Klant.naam FROM Opdracht JOIN OpdrachtWerknemer ON Opdracht.idOpdracht = OpdrachtWerknemer.idOpdracht JOIN Klant ON Opdracht.idKlant = Klant.idKlant";
            List<Customer> data = new List<Customer>();
            using (MySqlConnection conn = await DatabaseConnection.GetConnection())
            using (MySqlDataReader reader = await MySqlHelper.ExecuteReaderAsync(conn, query,
                new MySqlParameter("@0", false)))
                while (reader.Read())
                    data.Add(new Customer(reader.GetInt32(0), reader.GetString(1), reader.GetString(2)));
            return JsonConvert.SerializeObject(data);

        }

        // Werkbonnen
        [HttpPost("toevoegen")]
        public async Task<bool> Toevoegen([FromBody] Werkbon werkbon)
        {
            using (MySqlConnection conn = await DatabaseConnection.GetConnection())
            {
                int werktijdId;
                string query = "INSERT INTO Werktijd "
                             + "SET Werktijd.van = @0, Werktijd.tot = @1, "
                             + "Werktijd.urenTotaal = @2, Werktijd.pauzeTotaal = @3, Werktijd.datum = @4, "
                             + "Werktijd.verbruikteMaterialen = @5, Werktijd.Opmerking = @6, "
                             + "Werktijd.manKeuze = @7, " 
                             + "Werktijd.idOpdrachtWerknemer = @8;"
                             + "SELECT LAST_INSERT_ID();";
                using (MySqlDataReader reader = await MySqlHelper.ExecuteReaderAsync(conn, query,
                    new MySqlParameter("@0", werkbon.VanTijd),
                    new MySqlParameter("@1", werkbon.TotTijd),
                    new MySqlParameter("@2", werkbon.TotaalTijd),
                    new MySqlParameter("@3", werkbon.PauzeTijd),
                    new MySqlParameter("@4", werkbon.Datum),
                    new MySqlParameter("@5", werkbon.VerbruikteMaterialen),
                    new MySqlParameter("@6", werkbon.Opmerking),
                    new MySqlParameter("@7", werkbon.Mankeuze),
                    new MySqlParameter("@8", werkbon.IdOpdrachtWerknemer)))
                {
                    await reader.ReadAsync();
                    werktijdId = reader.GetInt32(0);
                }

                query = "INSERT INTO WerktijdMachines "
                        + "SET WerktijdMachines.idWerktijd = @last_id_Werktijd, "
                        + "WerktijdMachines.idMachines = @0; ";
                foreach (Machine machine in werkbon.Machines ?? Enumerable.Empty<Machine>())
                    await MySqlHelper.ExecuteNonQueryAsync(conn, query,
                        new MySqlParameter("@0", machine.IdMachine));

                query = "INSERT INTO WerktijdHulpstuk "
                    + "SET WerktijdHulpstuk.idWerktijd = @last_id_Werktijd, "
                    + "WerktijdHulpstuk.idHulpstuk = @0";
                foreach (Hulpstuk hulpstuk in werkbon.Hulpstukken ?? Enumerable.Empty<Hulpstuk>())
                    await MySqlHelper.ExecuteNonQueryAsync(conn, query,
                        new MySqlParameter("@0", hulpstuk.IdHulpstuk));

                query = "INSERT INTO Gewicht "
                    + "SET Gewicht.type = @0, Gewicht.volGewicht = @1, Gewicht.nettoGewicht = @2, "
                    + "Gewicht.richting = @3, Gewicht.idWerktijd = @last_id_Werktijd";
                foreach (Gewicht gewicht in werkbon.Gewichten ?? Enumerable.Empty<Gewicht>())
                    await MySqlHelper.ExecuteNonQueryAsync(conn, query,
                        new MySqlParameter("@0", gewicht.Type),
                        new MySqlParameter("@1", gewicht.VolGewicht),
                        new MySqlParameter("@2", gewicht.NettoGewicht),
                        new MySqlParameter("@3", gewicht.Richting));

                return true;
            }
        }

        [HttpGet("deleteall")]
        public async Task<bool> DeleteAllData()
        {
            string query = "SET FOREIGN_KEY_CHECKS = 0; "
                    + "TRUNCATE TABLE Gewicht; "
                    + "TRUNCATE TABLE Rijplaten ; "
                    + "TRUNCATE TABLE OpdrachtWerknemer; "
                    + "TRUNCATE TABLE Opdracht; "
                    + "TRUNCATE TABLE WerktijdHulpstuk; "
                    + "TRUNCATE TABLE WerktijdMachines; "
                    + "TRUNCATE TABLE Werktijd; "
                    + "SET FOREIGN_KEY_CHECKS = 1;";
            using (MySqlConnection conn = await DatabaseConnection.GetConnection())
            using (MySqlDataReader reader = await MySqlHelper.ExecuteReaderAsync(conn, query))
                return reader.RecordsAffected >= 1; ;
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
