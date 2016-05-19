using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using MySql.Data.MySqlClient;
using System.Data.Common;
using AgroApp.Models;
using AgroApp.Managers;
using AgroApp.Controllers.Api;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace AgroApp.Controllers.Admin
{
    public class MachineController : Controller
    {
        // GET: /<controller>/
        [HttpGet("admin/machinebeheer")]
        public IActionResult Index()
        {
            return View("../admin/machinebeheer/machinebeheer");
        }

        [HttpGet("admin/machinebeheer/toevoegen")]
        public IActionResult MachineToevoegen()
        {
            return View("../admin/machinebeheer/machineadd");
        }

        [HttpGet("admin/machinebeheer/wijzigen/{id}")]
        public async Task<IActionResult> GebruikerWijzigen(int id)
        {
            Machine machine = await WerkbonController.GetMachine(id);
            ViewData["id"] = id;
            ViewData["naam"] = machine.Naam;
            ViewData["machinenummer"] = machine.Nummer;
            ViewData["kenteken"] = machine.Kenteken;
            ViewData["type"] = machine.Type;
            return View("../admin/machinebeheer/machineedit");
        }
    }
}
