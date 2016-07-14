using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using AgroApp.Controllers.Api;
using AgroApp.Models;

namespace AgroApp.Controllers
{
    public class HomeController : Controller
    {
        public async Task<IActionResult> Index()
        {
            if (UserController.IsLoggedIn(HttpContext))
            {
                User user = await UserController.GetUser(HttpContext);
                if (user.Role == Models.User.UserRole.Admin)
                    return RedirectToAction("Index", "Admin");
                else
                    return RedirectToAction("Index", "User");
            }
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
