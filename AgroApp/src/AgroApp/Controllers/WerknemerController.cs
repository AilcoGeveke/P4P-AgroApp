using AgroApp.Controllers.Api;
using AgroApp.Models;
using Microsoft.AspNet.Mvc;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Http;

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

        [HttpGet("statistieken")]
        public IActionResult Statistieken()
        {
            return View("../werknemer/Statistieken");
        }

        [HttpGet("getopdrachten")]
        public async Task<IEnumerable<Opdracht>> GetOpdrachten()
        {
            string query = "SELECT * FROM Opdracht";
            List<Opdracht> opdrachten = new List<Opdracht>();
            using (MySqlConnection conn = await DatabaseConnection.GetConnection())
            using (MySqlDataReader reader = await MySqlHelper.ExecuteReaderAsync(conn, query,
                new MySqlParameter("@0", 1)))
                while (await reader.ReadAsync())
                    opdrachten.Add(new Opdracht(
                        idOpdracht: reader["idOpdracht"] as int? ?? -1,
                        locatie: reader["locatie"] as string,
                        beschrijving: reader["beschrijving"] as string,
                        datum: reader["datum"] as DateTime? ?? DateTime.MinValue));
            return opdrachten;
        }

        [HttpGet("GetGebruikerOpdrachten")]
        public async Task<IEnumerable<OpdrachtWerknemer>> GetGebruikerOpdrachten()
        {
            string query = "SELECT Opdracht.idOpdracht, Opdracht.locatie, Opdracht.beschrijving, Opdracht.datum, Klant.naam, Klant.adres, OpdrachtWerknemer.idOpdrachtWerknemer" +
                " FROM Opdracht JOIN Klant ON Klant.idKlant = Opdracht.idKlant JOIN OpdrachtWerknemer ON Opdracht.idOpdracht = OpdrachtWerknemer.idOpdracht " +
                "WHERE OpdrachtWerknemer.idWerknemer = @0";
            List<OpdrachtWerknemer> OpdrachtWerknemer = new List<OpdrachtWerknemer>();
            using (MySqlConnection conn = await DatabaseConnection.GetConnection())
            using (MySqlDataReader reader = await MySqlHelper.ExecuteReaderAsync(conn, query,
                new MySqlParameter("@0", HttpContext.Request.Cookies["idUser"])))
                while (await reader.ReadAsync())
                    OpdrachtWerknemer.Add(new OpdrachtWerknemer()
                    {
                        Opdracht = new Opdracht(
                        idOpdracht: reader["idOpdracht"] as int? ?? -1,
                        selectedKlant: new Customer(Name: reader["naam"] as string, Adress: reader["adres"] as string),
                        locatie: reader["locatie"] as string,
                        beschrijving: reader["beschrijving"] as string,
                        datum: reader["datum"] as DateTime? ?? DateTime.MinValue),
                        idOpdrachtWerknemer = reader["idOpdrachtWerknemer"] as int? ?? -1
                    });
            return OpdrachtWerknemer;
        }

        [HttpGet("getOpdrachtWerknemer/{id}")]
        public async Task<OpdrachtWerknemer> GetOpdrachtWerknemer(int id)
        {
            string query = "SELECT Opdracht.idOpdracht, Opdracht.locatie, Opdracht.beschrijving, Opdracht.datum, Klant.idKlant, Klant.naam, Klant.adres, OpdrachtWerknemer.idWerknemer" +
                " FROM Opdracht LEFT JOIN Klant ON Klant.idKlant = Opdracht.idKlant LEFT JOIN OpdrachtWerknemer ON Opdracht.idOpdracht = OpdrachtWerknemer.idOpdracht " +
                "WHERE OpdrachtWerknemer.idOpdrachtWerknemer = @0";
            using (MySqlConnection conn = await DatabaseConnection.GetConnection())
            using (MySqlDataReader reader = await MySqlHelper.ExecuteReaderAsync(conn, query,
                new MySqlParameter("@0", id)))
                while (await reader.ReadAsync())
                    return new OpdrachtWerknemer()
                    {
                        Opdracht = new Opdracht(
                        idOpdracht: reader["idOpdracht"] as int? ?? -1,
                        selectedKlant: new Customer(IdCustomer: reader["idKlant"] as int? ?? -1, Name: reader["naam"] as string, Adress: reader["adres"] as string),
                        locatie: reader["locatie"] as string,
                        beschrijving: reader["beschrijving"] as string,
                        datum: reader["datum"] as DateTime? ?? DateTime.MinValue),
                        Werknemer = new User() { IdWerknemer = reader["idWerknemer"] as int? ?? -1 },
                        idOpdrachtWerknemer = id as int? ?? -1
                    };
            return null;
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
            OpdrachtWerknemer opdrachtWerknemer = await GetOpdrachtWerknemer(id);
            ViewData["id"] = id;
            ViewData["locatie"] = opdrachtWerknemer.Opdracht.locatie;
            ViewData["beschrijving"] = opdrachtWerknemer.Opdracht.beschrijving;
            ViewData["datum"] = opdrachtWerknemer.Opdracht?.datum.ToString() ?? "";
            return View("assignmentedit");
        }

        [HttpGet("werkboninvullen/{id}")]
        public async Task<IActionResult> WerkbonInvullen(int id)
        {
            OpdrachtWerknemer opdrachtWerknemer = await GetOpdrachtWerknemer(id);
            ViewData["id"] = id;
            ViewData["locatie"] = opdrachtWerknemer.Opdracht.locatie;
            ViewData["gebruiker"] = opdrachtWerknemer.Werknemer.Name;
            ViewData["klantNaam"] = opdrachtWerknemer.Opdracht.klant.Name;
            return View("../admin/werkbonadd");
        }

        [HttpGet("getWerkbonnen")]
        public async Task<IEnumerable<Werkbon>> GetWerkbonnen()
        {
            string query = "SELECT Werktijd.van, Werktijd.tot, Werktijd.urenTotaal, Werktijd.pauzeTotaal, Werktijd.datum, Werktijd.verbruikteMaterialen, "
                    + "Werktijd.Opmerking, Mankeuze.naam AS MankeuzeNaam, Werknemer.idWerknemer, "
                    + "Werknemer.naam AS WerknemerNaam, Opdracht.locatie, Klant.naam AS KlantNaam "
                    + "FROM Werktijd "
                    + "JOIN Mankeuze "
                    + "ON Werktijd.idMankeuze = Mankeuze.idMankeuze "
                    + "JOIN OpdrachtWerknemer "
                    + "ON Werktijd.idOpdrachtWerknemer = OpdrachtWerknemer.idOpdrachtWerknemer "
                    + "JOIN Werknemer "
                    + "ON Werknemer.idWerknemer = OpdrachtWerknemer.idWerknemer "
                    + "JOIN Opdracht "
                    + "ON OpdrachtWerknemer.idOpdracht = Opdracht.idOpdracht "
                    + "JOIN Klant "
                    + "ON Opdracht.idKlant = Klant.idKlant;";
            List<Werkbon> werkbon = new List<Werkbon>();
            using (MySqlConnection conn = await DatabaseConnection.GetConnection())
            using (MySqlDataReader reader = await MySqlHelper.ExecuteReaderAsync(conn, query,
                new MySqlParameter("@0", HttpContext.Request.Cookies["idUser"])))
                while (await reader.ReadAsync())
                    werkbon.Add(new Werkbon()
                    {
                        Gebruiker = new User() { IdWerknemer = reader["idWerknemer"] as int? ?? -1, Name = reader["WerknemerNaam"] as string ?? "" },
                        Datum = reader["datum"] as DateTime? ?? new DateTime(),
                        Klant = new Customer() {Name = reader["KlantNaam"] as string ?? "" },
                        Mankeuze = reader["MankeuzeNaam"] as string ?? "",
                        VanTijd = reader["van"] as int? ?? 0,
                        TotTijd = reader["tot"] as int? ?? 0,
                        TotaalTijd = reader["urenTotaal"] as int ? ?? 0,
                        PauzeTijd = reader["pauzeTotaal"] as int? ?? 0,
                        VerbruikteMaterialen = reader["verbruikteMaterialen"] as string ?? "",
                        Opmerking = reader["Opmerking"] as string ?? ""
                    });
            return werkbon;
        }

        [HttpGet("getStatistieken")]
        public async Task<IEnumerable<Werkbon>> GetStatistieken()
        {
            string query = "SELECT OpdrachtWerknemer.idOpdrachtWerknemer, Werktijd.van, Werktijd.tot, Werktijd.urenTotaal, Werktijd.datum "
                    + "FROM OpdrachtWerknemer "
                    + "JOIN Werktijd "
                    + "ON OpdrachtWerknemer.idOpdrachtWerknemer = Werktijd.idOpdrachtWerknemer "
                    + "WHERE OpdrachtWerknemer.idWerknemer = @0";
                    //+ "AND Werktijd.datum >= @1 AND Werktijd.datum <= @2";
            List<Werkbon> statistiek = new List<Werkbon>();
            using (MySqlConnection conn = await DatabaseConnection.GetConnection())
            using (MySqlDataReader reader = await MySqlHelper.ExecuteReaderAsync(conn, query,
                new MySqlParameter("@0", HttpContext.Request.Cookies["idUser"])))
                //new MySqlParameter("@1", datumVan),
                //new MySqlParameter("@2", datumTot)))
                while (await reader.ReadAsync())
                    statistiek.Add(new Werkbon()
                    {
                        Datum = reader["datum"] as DateTime? ?? new DateTime(),
                        VanTijd = reader["van"] as int? ?? 0,
                        TotTijd = reader["tot"] as int? ?? 0,
                        TotaalTijd = reader["urenTotaal"] as int? ?? 0,
                    });
            return statistiek;
        }
    }

}
