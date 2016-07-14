using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using AgroApp.Controllers.Api;
using AgroApp.Models;
using Newtonsoft.Json;

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

        [HttpGet("admin/urenoverzicht")]
        public IActionResult WorkOrderView()
        {
            return View("timesheet");
        }

        //User
        [HttpGet("admin/gebruikers/overzicht")]
        public IActionResult OverviewUsers()
        {
            return View("management/overviewUser");
        }

        [HttpGet("admin/gebruikers/toevoegen")]
        public IActionResult AddUser()
        {
            return View("management/addUser");
        }

        [HttpGet("admin/gebruikers/wijzigen/{id}")]
        public async Task<IActionResult> EditUser(int id)
        {
            User user = await UserController.GetUser(id);
            ViewData["userData"] = JsonConvert.SerializeObject(user);

            return View("management/editUser");
        }

        [HttpGet("admin/gebruikers/archief")]
        public IActionResult ArchivedUsers()
        {
            return View("management/archiveUser");
        }

        //Customer
        [HttpGet("admin/klanten/overzicht")]
        public IActionResult OverviewCustomers()
        {
            return View("management/overviewcustomer");
        }

        [HttpGet("admin/klanten/toevoegen")]
        public IActionResult AddCustomer()
        {
            return View("management/addcustomer");
        }

        [HttpGet("admin/klanten/archief")]
        public IActionResult ArchiveCustomer()
        {
            return View("management/archivecustomer");
        }

        [HttpGet("admin/klanten/wijzigen/{id}")]
        public async Task<IActionResult> EditCustomer(int id)
        {

            Customer customer = await CustomerController.GetCustomer(id);
            ViewData["userData"] = JsonConvert.SerializeObject(customer);

            return View("management/editcustomer");
        }

        // Machine
        [HttpGet("admin/machines/overzicht")]
        public IActionResult OverviewMachines()
        {
            return View("management/overviewMachine");
        }

        [HttpGet("admin/machines/toevoegen")]
        public IActionResult AddMachine()
        {
            return View("management/addMachine");
        }

        [HttpGet("admin/werkbon/{idEmployeeAssignment}")]
        public IActionResult AddTimesheet()
        {
            return View("timesheet");
        }


        [HttpGet("admin/machines/wijzigen/{id}")]
        public async Task<IActionResult> EditMachine(int id)
        {
            Machine machine = await MachineController.GetMachine(id);
            ViewData["machineData"] = JsonConvert.SerializeObject(machine); ;

            return View("management/editUser");
        }

        // Assignment
        [HttpGet("admin/opdrachten")]
        public IActionResult OverviewAssignments()
        {
            ViewData["userAssignment"] = false;
            return View("management/OverviewAssignment");
        }

        [HttpGet("admin/opdrachten/eigen")]
        public async Task<IActionResult> OverviewUserAssignments()
        {
            ViewData["userAssignment"] = true;
            ViewData["user"] = JsonConvert.SerializeObject(await UserController.GetUser(HttpContext));
            return View("management/OverviewAssignment");
        }

        // Cargo
        [HttpGet("admin/vrachten/overzicht")]
        public IActionResult OverviewCargo()
        {
            return View("management/overviewcargo");
        }

        [HttpGet("admin/vrachten/toevoegen")]
        public IActionResult AddCargo()
        {
            return View("management/addcargo");
        }
    }
}
