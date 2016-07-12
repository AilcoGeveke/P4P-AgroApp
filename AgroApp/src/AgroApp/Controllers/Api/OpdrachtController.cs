using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using MySql.Data.MySqlClient;
using AgroApp.Models;
using Newtonsoft.Json;
using AgroApp.Controllers.Admin;

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
                    new MySqlParameter("@0", opdracht.location),
                    new MySqlParameter("@1", opdracht.description),
                    new MySqlParameter("@2", opdracht.customer.IdCustomer),
                    new MySqlParameter("@3", opdracht.date)))
                {
                    await reader.ReadAsync();
                    opdrachtId = reader.GetInt32(0);
                }

                query = "INSERT INTO OpdrachtWerknemer (idWerknemer, idOpdracht) VALUES (@0, @1)";
                foreach (User user in opdracht.users)
                    await MySqlHelper.ExecuteNonQueryAsync(conn, query,
                        new MySqlParameter("@1", opdrachtId),
                        new MySqlParameter("@0", user.IdWerknemer));

                return true;
            }
        }

        [HttpGet("alle/{archive}")]
        public async Task<IEnumerable<Opdracht>> GetOpdrachten(bool archived)
        {
            string query = "SELECT Opdracht.*, COUNT(OpdrachtWerknemer.idOpdrachtWerknemer) as count FROM Opdracht LEFT JOIN OpdrachtWerknemer ON Opdracht.idOpdracht = OpdrachtWerknemer.idOpdracht GROUP BY Opdracht.idOpdracht";
            List<Opdracht> opdrachten = new List<Opdracht>();
            using (MySqlConnection conn = await DatabaseConnection.GetConnection())
            using (MySqlDataReader reader = await MySqlHelper.ExecuteReaderAsync(conn, query,
                new MySqlParameter("@0", 1)))
                while (await reader.ReadAsync())
                    opdrachten.Add(new Opdracht(
                        idOpdracht: reader["idOpdracht"] as int? ?? -1,
                        locatie: reader["locatie"] as string,
                        beschrijving: reader["beschrijving"] as string,
                        datum: reader["datum"] as DateTime? ?? null)
                    {
                        klant = await WerkbonController.GetKlant(reader["idKlant"] as int? ?? -1),
                        gebruikerCount = (int)(reader["count"] as long? ?? (long)0) 
                    });
            return opdrachten;
        }
    }
}
