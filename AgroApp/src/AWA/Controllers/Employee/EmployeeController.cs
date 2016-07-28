using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace AgroApp.Controllers.Employee
{
    public class EmployeeController : Controller
    {
        /// <summary>
        /// Returns main page of the Employee
        /// </summary>
        /// <returns></returns>
        [HttpGet("werknemer")]
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet("werknemer/opdrachten")]
        public IActionResult AssignmentView()
        {
            //ViewData["userAssignment"] = true;
            //ViewData["user"] = JsonConvert.SerializeObject(await UserController.GetUser(HttpContext));
            return View("~/views/Admin/Management/OverviewAssignment");
        }

        [HttpGet("werkemer/werkbon/{idAssignment}")]
        public IActionResult AddTimesheet(int id)
        {
            ViewData["idAssignment"] = id;

            return View("timesheet");
        }
    }
}
