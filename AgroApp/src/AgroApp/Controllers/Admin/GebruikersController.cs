using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using AgroApp.Models;
using AgroApp.Managers;
using AgroApp.Controllers.Api;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace AgroApp.Controllers
{
    public class GebruikersController : Controller
    {
        // GET: /<controller>/
        [HttpGet("admin/gebruikers")]
        public IActionResult Index()
        {
            return View("../admin/gebruikerbeheer/gebruikerbeheer");
        }

        [HttpGet("admin/gebruikers/wijzigen/{id}")]
        public async Task<IActionResult> GebruikerWijzigen(int id)
        {
            User user = await UserController.GetUser(id);
            ViewData["naam"] = user.Name;
            ViewData["email"] = user.Email;
            return View("../admin/gebruikerbeheer/gebruikeredit");
        }

        [HttpGet("admin/gebruikers/toevoegen")]
        public IActionResult GebruikerToevoegen()
        {
            return View("../admin/gebruikerbeheer/gebruikeradd");
        }
    }
}
