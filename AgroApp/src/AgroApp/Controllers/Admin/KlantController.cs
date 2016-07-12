using AgroApp.Controllers.Api;
using AgroApp.Models;
using Microsoft.AspNet.Mvc;
using System.Threading.Tasks;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace AgroApp.Controllers.Admin
{
    public class KlantController : Controller
    {
        // GET: /<controller>/
        [HttpGet("admin/klantbeheer")]
        public IActionResult Index()
        {
            return View("../admin/klantbeheer/klantbeheer");
        }

        [HttpGet("admin/klantbeheer/toevoegen")]
        public IActionResult KlantToevoegen()
        {
            return View("../admin/klantbeheer/klantadd");
        }

        [HttpGet("admin/klantbeheer/archief")]
        public IActionResult KlantTerugHalen()
        {
            return View("../admin/klantbeheer/klantarchief");
        }

        [HttpGet("admin/klantbeheer/wijzigen/{id}")]
        public async Task<IActionResult> GebruikerWijzigen(int id)
        {
            Customer klant = await WerkbonController.GetKlant(id);
            ViewData["idKlant"] = id;
            ViewData["naamKlant"] = klant.Name;
            ViewData["adresKlant"] = klant.Address;
            return View("../admin/klantbeheer/klantedit");
        }
    }
}

