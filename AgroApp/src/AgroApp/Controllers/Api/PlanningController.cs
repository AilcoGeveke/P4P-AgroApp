using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using AgroApp.Models;
using MySql.Data.MySqlClient;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace AgroApp.Controllers.Api
{
    [Route("api/[controller]")]
    public class PlanningController : Controller
    {
        // GET: api/values
        [HttpGet("GetGebruikersWerktijden/{time}")]
        public async Task<List<GebruikerTijd>> GetGebruikersWerktijden(DateTime time)
        {
            List<GebruikerTijd> tijden = new List<GebruikerTijd>();
            List<User> users = new List<User>(await UserController.GetAllUsers());
            using (MySqlConnection conn = await DatabaseConnection.GetConnection())
            {
                foreach (User user in users)
                {
                    GebruikerTijd gt = new GebruikerTijd(user.IdWerknemer, user.Name);
                    tijden.Add(gt);

                    string query = "SELECT werktijd.van, werktijd.tot FROM Werktijd JOIN OpdrachtWerknemer ON opdrachtWerknemer.idOpdrachtWerknemer=@0 WHERE werktijd.datum >= @1 AND werktijd.datum < @2";
                    using (MySqlDataReader reader = await MySqlHelper.ExecuteReaderAsync(conn, query,
                        new MySqlParameter("@0", user.IdWerknemer),
                        new MySqlParameter("@1", time),
                        new MySqlParameter("@2", time.AddDays(1))))
                    {
                        while (await reader.ReadAsync())
                        {
                            gt.vanTijd = reader["van"] as DateTime? ?? DateTime.Now;
                            gt.totTijd = reader["tot"] as DateTime? ?? DateTime.Now;
                        }
                    }
                }
            }

            return tijden;
        }


    }

    public class GebruikerTijd
    {
        public DateTime vanTijd;
        public DateTime totTijd;
        public string naam;
        public int idUser;

        public GebruikerTijd(int idUser, string naam)
        {
            this.idUser = idUser;
            this.naam = naam;
        }
    }
}
