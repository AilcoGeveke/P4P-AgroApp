using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Authorization;
using AgroApp.Managers;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace AgroApp.Controllers.Api
{
    [Route("api/[controller]")]
    //[Authorize]
    public class AccountController : Controller
    {
        // GET: api/account
        [HttpGet("me")]
        public string GetMe()
        {
            return HttpContext.User.Identity.Name;
        }

        // GET: api/account/getfulllist
        [HttpGet("getfulllist")]
        public async Task<string> GetAllUsers()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(await UserManager.GetAllUsers());
        }
    }
}
