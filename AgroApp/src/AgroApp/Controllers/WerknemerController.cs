using AgroApp.Controllers.Api;
using AgroApp.Models;
using Microsoft.AspNet.Mvc;
using MySql.Data.MySqlClient;
using System.Threading.Tasks;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace AgroApp.Controllers
{
    public class WerknemerController : Controller
    {
        // GET: /user/usermain
        [HttpGet("werknemer/menu")]
        public IActionResult Index()
        {
            ViewData["volledigenaam"] = HttpContext.User.Identity.Name;
            return View("../werknemer/main");
        }

        [HttpGet("werknemer/werkbontoevoegen")]
        public IActionResult werkbontoevoegen()
        {
            return View("../admin/werkbonadd");
        }

        [HttpGet("werknemer/gebruikerbeheer")]
        public IActionResult Accountbeheren()
        {
            return View("../werknemer/Manage");
        }

        [HttpGet("werknemer/opdrachten")]
        public IActionResult Opdrachten()
        {
            return View("../werknemer/Assignment");
        }



    }

}
