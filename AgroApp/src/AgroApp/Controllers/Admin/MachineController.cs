using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;

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
    }
}
