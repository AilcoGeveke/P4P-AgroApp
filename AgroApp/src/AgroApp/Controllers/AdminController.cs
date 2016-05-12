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
            return View();
        }

        // GET: /admin/planning
        public IActionResult Planning()
        {
            return View();
        }
    }
}
