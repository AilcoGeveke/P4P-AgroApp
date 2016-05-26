﻿using AgroApp.Controllers.Api;
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

        [HttpGet("assignmentedit/{id}")]
        public IActionResult OpdrachtEdit()
        {
            return View("../werknemer/assignmentedit");
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
                        idKlant: reader["idKlant"] as int? ?? -1, 
                        datum: reader["datum"] as DateTime? ?? DateTime.MinValue));

            System.Diagnostics.Debug.WriteLine("-------------------------------------------");
            System.Diagnostics.Debug.WriteLine("Debug value of opdrachten:");
            System.Diagnostics.Debug.WriteLine(opdrachten);
            System.Diagnostics.Debug.WriteLine("-------------------------------------------");
            return opdrachten;
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
                return reader.HasRows ? new Opdracht(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetInt32(3), reader.GetDateTime(4)) : null;
            }
        }

        [HttpGet("werknemer/assignmentedit/{id}")]
        public async Task<IActionResult> GebruikerWijzigen(int id)
        {
            Opdracht opdracht = await GetOpdracht(id);
            ViewData["id"] = id;
            ViewData["idOpdracht"] = opdracht.idOpdracht;
            ViewData["locatie"] = opdracht.locatie;
            ViewData["beschrijving"] = opdracht.beschrijving;
            ViewData["datum"] = opdracht.datum;
            return View("..werknemer/assignmentedit/");
        }

    }

}
