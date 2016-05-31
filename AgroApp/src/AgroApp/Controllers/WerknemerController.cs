using AgroApp.Controllers.Api;
using AgroApp.Models;
using Microsoft.AspNet.Mvc;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace AgroApp.Controllers
{
    [Route("[controller]")]
    public class WerknemerController : Controller
    {
        // GET: /user/usermain
        [HttpGet("menu")]
        public IActionResult Index()
        {
            ViewData["volledigenaam"] = HttpContext.User.Identity.Name;
            return View("../werknemer/main");
        }

        [HttpGet("werkbontoevoegen")]
        public IActionResult werkbontoevoegen()
        {
            return View("../admin/werkbonadd");
        }

        [HttpGet("gebruikerbeheer")]
        public IActionResult Accountbeheren()
        {
            return View("../werknemer/Manage");
        }

        [HttpGet("opdrachten")]
        public IActionResult Opdrachten()
        {
            return View("../werknemer/Assignment");
        }


        [HttpGet("getopdracht")]
        private async Task<Opdracht> _GetOpdracht(int id)
        {
            return await _GetOpdracht(id);
        }

        public static async Task<Opdracht> GetOpdracht(int id)
        {
            if (id < 0)
                return null;

            string query = "SELECT idOpdracht, locatie, beschrijving,  datum FROM Opdracht WHERE idOpdracht=@0";
            using (MySqlConnection conn = await DatabaseConnection.GetConnection())
            using (MySqlDataReader reader = await MySqlHelper.ExecuteReaderAsync(conn, query,
                new MySqlParameter("@0", id)))
            {
                await reader.ReadAsync();
                return new Opdracht(reader["idOpdracht"] as int? ?? 0, reader["locatie"] as string ?? "", reader["beschrijving"] as string ?? "", reader["datum"] as DateTime?);
            }
        }

        [HttpGet("assignmentedit/{id}")]
        public async Task<IActionResult> GebruikerWijzigen(int id)
        {
            Opdracht opdracht = await GetOpdracht(id);
            ViewData["id"] = id;
            ViewData["locatie"] = opdracht.locatie;
            ViewData["beschrijving"] = opdracht.beschrijving;
            ViewData["datum"] = opdracht?.datum.ToString() ?? "";
            return View("assignmentedit");
        }

    }

}
