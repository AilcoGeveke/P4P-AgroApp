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
            if (UserController.IsLoggedIn)
            {
                User user = await UserController.GetUser(HttpContext);
                if (user.Rol == Models.User.UserRol.Admin)
                    return View("");
                else
                    return View("");
            }
            return View();
        }
    }
}
