using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace AgroApp.Controllers
{
    public class AdminController : Controller
    {
        // GET: /admin/main
        public IActionResult Main()
        {
            ViewData["volledigenaam"] = HttpContext.User.Identity.Name;
            return View();
        }

        // GET: /admin/planning
        public IActionResult Planning()
        {
            return View();
        }

        // GET: /admin/machine
        public IActionResult Machine()
        {
            return View();
        }

        // GET: /admin/opdracht
        [HttpGet("admin/opdracht/toevoegen")]
        public IActionResult Opdracht()
        {
            return View("../admin/opdrachtadd");
        }

        // GET: /admin/werkboncheck
        public IActionResult WerkbonCheck()
        {
            return View();
        }

        // GET: /admin/werkbonadd
        public IActionResult Werkbonadd()
        {
            return View();
        }

        [HttpGet("admin/werkbonoverzicht")]
        public IActionResult WerkbonOverzicht()
        {
            return View("../admin/werkbonoverzicht");
        }

    }
}
