using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using MySql.Data.MySqlClient;
using AgroApp.Models;
using Newtonsoft.Json;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace AgroApp.Controllers.Api
{
    [Route("api/[controller]")]
    public class OpdrachtController : Controller
    {
        // POST api/values
        [HttpPost("toevoegen")]
        public async Task<bool> Toevoegen([FromBody]Opdracht opdracht)
        {
            using (MySqlConnection conn = await DatabaseConnection.GetConnection())
            {
                int opdrachtId = -1;
                string query = "INSERT INTO Opdracht (locatie, beschrijving, idKlant, datum) VALUES (@0, @1, @2, @3); SELECT LAST_INSERT_ID()";
                using (MySqlDataReader reader = await MySqlHelper.ExecuteReaderAsync(conn, query,
                    new MySqlParameter("@0", opdracht.locatie),
                    new MySqlParameter("@1", opdracht.beschrijving),
                    new MySqlParameter("@2", opdracht.selectedKlant.IdCustomer),
                    new MySqlParameter("@3", opdracht.datum)))
                {
                    await reader.ReadAsync();
                    opdrachtId = reader.GetInt32(0);
                }

                query = "INSERT INTO OpdrachtWerknemer (idWerknemer, idOpdracht) VALUES (@0, @1)";
                foreach (User user in opdracht.selectedGebruikers)
                    await MySqlHelper.ExecuteNonQueryAsync(conn, query,
                        new MySqlParameter("@1", opdrachtId),
                        new MySqlParameter("@0", user.IdWerknemer));

                return true;
            }
        }

        public static async Task<IEnumerable<Opdracht>> GetAllOpdrachten()
        {
            string query = "SELECT locatie, beschrijving, datum FROM Opdracht";
            List<Opdracht> opdrachten = new List<Opdracht>();
            using (MySqlConnection conn = await DatabaseConnection.GetConnection())
            using (MySqlDataReader reader = await MySqlHelper.ExecuteReaderAsync(conn, query,
                new MySqlParameter("@0", false)))
                while (await reader.ReadAsync())
                {
                    opdrachten.Add(new Opdracht(locatie: reader.GetString(0), beschrijving: reader.GetString(1), datum: reader.GetDateTime(2)));
                    opdrachten.Add(new Customer(locatie: reader.GetString(0), beschrijving: reader.GetString(1), datum: reader.GetDateTime(2)));
                }
                    
            return opdrachten;
        }
    }
}
