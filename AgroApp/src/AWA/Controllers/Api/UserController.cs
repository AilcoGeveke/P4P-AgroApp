using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AWA.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace AWA.Controllers.Api
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private AgroContext _context;

        public UserController(AgroContext context)
        {
            _context = context;
        }

        #region Api Calls

        [HttpGet("login/{username}/{password}")]
        [AllowAnonymous]
        public async Task<bool> Login(string username, string password)
        {
            if (!IsValid(_context, username, password))
                return false;

            User user = GetUser(_context, username);
            IList<Claim> claimCollection = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Username),
                new Claim(ClaimTypes.Role, user.Role.ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString())
            };
            ClaimsPrincipal principal = new ClaimsPrincipal(new ClaimsIdentity(claimCollection, CookieAuthenticationDefaults.AuthenticationScheme));
            await HttpContext.Authentication.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
            return true;
        }
        
        public static async Task Logout(HttpContext context)
        {
            await context.Authentication.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        [HttpPost("register")]
        public object AddUser([FromBody] User user)
        {
            return AddUser(_context, user);
        }

        [HttpPost("edit")]
        public object EditUser([FromBody] User user)
        {
            return EditUser(_context, user);
        }

        [HttpGet("getall/{archived}")]
        public List<User> GetAllUsers(bool archived = false)
        {
            return GetUsers(_context, archived).ToList();
        }

        [HttpGet("archive/{userId}")]
        public object ArchiveUser(int userId)
        {
            GetUser(_context, userId).IsArchived = true;
            _context.SaveChanges();
            return true;
        }

        [HttpGet("restore/{userId}")]
        public object RestoreUser(int userId)
        {
            GetUser(_context, userId).IsArchived = false;
            _context.SaveChanges();
            return true;
        }

        #endregion

        #region Static Methods

        /// <param name="context"></param>
        /// <returns>Returns true if user is logged in</returns>
        public static bool IsLoggedIn(HttpContext context)
        {
            return context.User.Identity.IsAuthenticated;
        }

        /// <param name="context"></param>
        /// <param name="username"></param>
        /// <returns>Returns true if username is found</returns>
        public static bool Exist(AgroContext context, string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException();

            int count = context.Users.Count(x => x.Username == username);
            return count == 1;
        }

        /// <param name="context"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns>Returns true if the given username and password are matching and found</returns>
        public static bool IsValid(AgroContext context, string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                throw new ArgumentException();

            string hash = GetEncodedHash(password, "123");
            return context.Users.First(x => x.Username == username && x.PasswordEncrypted == hash) != null;
        }

        /// <param name="context"></param>
        /// <param name="username"></param>
        /// <returns>Returns User with given username</returns>
        public static User GetUser(AgroContext context, string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException();

            return context.Users.First(x => x.Username == username);
        }

        /// <param name="context"></param>
        /// <param name="userId"></param>
        /// <returns>Returns User with given UserId</returns>
        public static User GetUser(AgroContext context, int userId)
        {
            return context.Users.First(x => x.UserId == userId);
        }

        /// <param name="context"></param>
        /// <param name="archived"></param>
        /// <returns>Returns list with all users, if archived is true it returns all archived users</returns>
        public static IQueryable<User> GetUsers(AgroContext context, bool archived = false)
        {
            return context.Users.Where(x => x.IsArchived == archived);
        }

        /// <summary>
        /// Adds user to the database
        /// </summary>
        /// <param name="context"></param>
        /// <param name="user"></param>
        public static object AddUser(AgroContext context, User user)
        {
            user.Username = user.Username.ToLower();
            user.PasswordEncrypted = GetEncodedHash(user.Password, "123");

            if (GetUser(context, user.Username) != null)
                return "Gebruiker bestaat al!";

            context.Users.Add(user);
            try
            {
                context.SaveChanges();
                return true;
            }
            catch 
            {
                return "Er is iets misgegaan!";
            }
        }

        /// <summary>
        /// Edits the givin user with the data received
        /// </summary>
        /// <param name="context"></param>
        /// <param name="changedUser"></param>
        public static object EditUser(AgroContext context, User changedUser)
        {
            User user = GetUser(context, changedUser.UserId);

            user.Name = changedUser.Name;
            user.Role = changedUser.Role;
            user.Username = changedUser.Username.ToLower();
            
            if(changedUser.Password?.Length >= 3)
                user.PasswordEncrypted = GetEncodedHash(changedUser.Password, "123");
            
            try
            {
                context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return "Ër is iets misgegaan! " + ex.Message;
            }
        }

        private static string GetEncodedHash(string password, string salt)
        {
            MD5 md5 = MD5.Create();
            byte[] digest = md5.ComputeHash(Encoding.UTF8.GetBytes(password + salt));
            string base64digest = Convert.ToBase64String(digest, 0, digest.Length);
            return base64digest.Substring(0, base64digest.Length - 2);
        }

        #endregion
    }
}
