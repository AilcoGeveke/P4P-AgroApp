using AgroApp.Controllers.Api;
using AgroApp.Models;
using Microsoft.AspNet.Mvc;
using System.Threading.Tasks;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace AgroApp.Controllers.Admin
{
    public class HulpstukController : Controller
    {
        // GET: /<controller>/
        [HttpGet("admin/hulpstukbeheer")]
        public IActionResult Index()
        {
            return View("../admin/hulpstukbeheer/hulpstukbeheer");
        }

        [HttpGet("admin/hulpstukbeheer/toevoegen")]
        public IActionResult HulpstukToevoegen()
        {
            return View("../admin/hulpstukbeheer/hulpstukadd");
        }

        [HttpGet("admin/hulpstukbeheer/wijzigen/{id}")]
        public async Task<IActionResult> HulpstukWijzigen(int id)
        {
            Hulpstuk hulpstuk = await WerkbonController.GetHulpstuk(id);
            ViewData["id"] = id;
            ViewData["naam"] = hulpstuk.Naam;
            ViewData["hulpstuknummer"] = hulpstuk.Nummer;
            return View("../admin/hulpstukbeheer/hulpstukedit");
        }

        [HttpGet("admin/hulpstukbeheer/archief")]
        public IActionResult HulpstukTerugHalen()
        {
            return View("../admin/hulpstukbeheer/hulpstukarchief");
        }
    }
}
