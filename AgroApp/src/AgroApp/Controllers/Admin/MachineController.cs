using AgroApp.Controllers.Api;
using AgroApp.Models;
using Microsoft.AspNet.Mvc;
using System.Threading.Tasks;

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

        [HttpGet("admin/machinebeheer/wijzigen/{id}")]
        public async Task<IActionResult> MachineWijzigen(int id)
        {
            Machine machine = await WerkbonController.GetMachine(id);
            ViewData["id"] = id;
            ViewData["naam"] = machine.Name;
            ViewData["machinenummer"] = machine.Number;
            ViewData["kenteken"] = machine.Tag;
            ViewData["type"] = machine.Type;
            return View("../admin/machinebeheer/machineedit");
        }

        [HttpGet("admin/machinebeheer/archief")]
        public IActionResult MachineTerugHalen()
        {
            return View("../admin/machinebeheer/machinearchief");
        }
    }
}
