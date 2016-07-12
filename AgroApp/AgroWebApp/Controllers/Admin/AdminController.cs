using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace AgroApp.Controllers.Admin
{
    public class AdminController : Controller
    {
        /// <summary>
        /// Returns main page of the admins
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("admin/werkbon")]
        public IActionResult WorkOrderView()
        {
            return View("timesheet");
        }

        [HttpGet("admin/gebruikers/overzicht")]
        public IActionResult OverviewUsers()
        {
            return View("manage-user/overview-user");
        }

        [HttpGet("admin/gebruikers/toevoegen")]
        public IActionResult AddUser()
        {
            return View("manage-user/add-user");
        }

        [HttpGet("admin/gebruikers/wijzigen")]
        public IActionResult EditUser()
        {
            return View("manage-user/edit-user");
        }

        [HttpGet("admin/gebruikers/archief")]
        public IActionResult ArchivedUsers()
        {
            return View("manage-user/archive-user");
        }

    }
}
