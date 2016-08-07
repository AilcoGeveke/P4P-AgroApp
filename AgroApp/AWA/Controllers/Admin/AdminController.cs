using System.Threading.Tasks;
using AWA.Controllers.Api;
using AWA.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace AWA.Controllers.Admin
{
    public class AdminController : Controller
    {
        private AgroContext _context;

        public AdminController(AgroContext context)
        {
            _context = context;
        }
        /// <summary>
        /// Returns main page of the admins
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("admin/urenoverzicht/{IdAssignment}")]
        public IActionResult TimesheetView(int idAssignment)
        {
            ViewData["EnableControls"] = true;
            ViewData["EmployeeAssignment"] = JsonConvert.SerializeObject(AssignmentController.GetEmployeeAssignment(_context, HttpContext, idAssignment), new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });
            return View("timesheet");
        }

        [HttpGet("admin/urenoverzicht/{IdAssignment}/{IdEmployee}")]
        public IActionResult TimesheetView(int idAssignment, int idEmployee)
        {
            ViewData["EnableControls"] = false;
            ViewData["EmployeeAssignment"] = JsonConvert.SerializeObject(AssignmentController.GetEmployeeAssignment(_context, idEmployee, idAssignment), new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });
            return View("timesheet");
        }

        [HttpGet("~/admin/verwijderen")]
        public IActionResult DeleteDatabase()
        {
            return View("management/truncatealldata");
        }
        //Attachment
        [HttpGet("admin/hulpstuk/overzicht")]
        public IActionResult OverviewAttachment()
        {
            return View("management/overviewattachment");
        }

        [HttpGet("admin/hulpstuk/toevoegen")]
        public IActionResult AddAttachment()
        {
            return View("management/addattachment");
        }

        [HttpGet("admin/hulpstuk/archief")]
        public IActionResult ArchiveAttachment()
        {
            return View("management/archiveattachment");
        }

        [HttpGet("admin/hulpstuk/wijzigen/{id}")]
        public IActionResult EditAttachment(int id)
        {
            //Attachment attachment = await AttachmentController.GetAttachment(id);
            //ViewData["userData"] = JsonConvert.SerializeObject(attachment);

            return View("management/editattachment");
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
        public IActionResult EditCustomer(int id)
        {
            //Customer customer = await CustomerController.GetCustomer(id);
            //ViewData["userData"] = JsonConvert.SerializeObject(customer);

            return View("management/editcustomer");
        }

        // Machine
        [HttpGet("admin/machines/overzicht")]
        public IActionResult OverviewMachines()
        {
            return View("management/overviewMachine");
        }

        [HttpGet("admin/machines/archief")]
        public IActionResult ArchiveMachines()
        {
            return View("management/archivemachine");
        }

        [HttpGet("admin/machines/toevoegen")]
        public IActionResult AddMachine()
        {
            return View("management/addMachine");
        }

        [HttpGet("admin/machines/wijzigen/{id}")]
        public IActionResult EditMachine(int id)
        {
            //Machine machine = await MachineController.GetMachine(id);
            //ViewData["machineData"] = JsonConvert.SerializeObject(machine); ;

            return View("management/editmachine");
        }

        // Assignment
        [HttpGet("admin/opdrachten")]
        public IActionResult OverviewAssignments()
        {
            ViewData["userAssignment"] = false;
            return View("management/OverviewAssignment");
        }

        [HttpGet("admin/opdrachten/eigen")]
        public IActionResult OverviewUserAssignments()
        {
            ViewData["userAssignment"] = true;
            ViewData["user"] = UserController.GetUser(_context, HttpContext);
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
