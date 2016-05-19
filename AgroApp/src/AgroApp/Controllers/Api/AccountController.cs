using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc;
using Newtonsoft.Json;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace AgroApp.Controllers.Api
{
    [Route("api/[controller]")]
    //[Authorize]
    public class AccountController : Controller
    {
        // GET: api/account
        [Authorize]
        [HttpGet("me")]
        public string GetMe()
        {
            return HttpContext.User.Identity.Name;
        }

        // GET: api/account/getfulllist
        [HttpGet("getfulllist")]
        public async Task<string> GetAllUsers()
        {
            return JsonConvert.SerializeObject(await UserController.GetAllUsers());
        }
    }
}
