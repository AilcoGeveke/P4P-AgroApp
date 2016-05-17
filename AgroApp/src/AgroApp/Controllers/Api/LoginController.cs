using AgroApp.Account;
using AgroApp.Models;
using Microsoft.AspNet.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AgroApp.Controllers.Api
{
    [Route("api/[controller]")]
    public class LoginController : Controller
    {
        // GET: api/values
        [HttpGet("{username}/{password}")]
        public async Task<string> Login(string username, string password)
        {
            if (await UserManager.IsValid(username, password))
            {
                string name = (await UserManager.GetUser(username))?.Name ?? "";
                List<Claim> claimCollection = new List<Claim> {
                    new Claim(ClaimTypes.Name, name),
                    new Claim(ClaimTypes.Email, username),
                    new Claim(ClaimTypes.Role, "Admin") };

                await HttpContext.Authentication.SignInAsync("AgroAppCookie", new ClaimsPrincipal(new ClaimsIdentity(claimCollection)));
                return "true"; // auth succeed 
            }
            return "false";
        }

        // GET: api/values
        [HttpGet("logout")]
        public async Task<string> Logout(string username, string password)
        {
            await HttpContext.Authentication.SignOutAsync("AgroAppCookie");
            return "succes";
        }
    }
}
