using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AWA.Controllers.Api;
using AWA.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AWA.Controllers
{
    public class HomeController : Controller
    {
        private AgroContext _context;

        public HomeController(AgroContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            if (!UserController.IsLoggedIn(HttpContext))
                return View();

            try
            {
                User user = UserController.GetUser(_context, HttpContext);
                return RedirectToAction("Index", user.Role == Models.User.UserRole.Admin ? "admin" : "employee");
            }
            catch (Exception)
            {
                return await Logout();
            }
        }

        [HttpGet("logout"), Authorize]
        public async Task<IActionResult> Logout()
        {
            await UserController.Logout(HttpContext);
            return RedirectToAction("Index");
        }
    }
}
