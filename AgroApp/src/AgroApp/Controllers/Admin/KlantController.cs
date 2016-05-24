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

        [HttpGet("admin/klantbeheer/verwijderd")]
        public IActionResult KlantTerugHalen()
        {
            return View("../admin/klantbeheer/klantverwijderd");
        }

        [HttpGet("admin/klantbeheer/wijzigen/{id}")]
        public async Task<IActionResult> GebruikerWijzigen(int id)
        {
            Klant klant = await WerkbonController.GetKlant(id);
            ViewData["idKlant"] = id;
            ViewData["naamKlant"] = klant.Naam;
            ViewData["adresKlant"] = klant.Adres;
            return View("../admin/klantbeheer/klantedit");
        }
    }
}

