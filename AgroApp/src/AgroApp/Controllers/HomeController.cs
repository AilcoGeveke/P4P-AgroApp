using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using AgroApp.Controllers.Api;

namespace AgroApp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("logout")]
        public async Task<IActionResult> Logout()
        {
            await UserController.Logout(HttpContext);
            return RedirectToAction("Index");
        }
    }
}
