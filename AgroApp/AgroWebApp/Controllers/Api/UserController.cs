using AgroApp.Models;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Http.Authentication;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Mvc;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AgroApp.Controllers.Api
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        public static bool IsLoggedIn(HttpContext context)
        {
            return context.User.Identity.IsAuthenticated;
        }

        // GET: api/values
        [HttpGet("login/{username}/{password}")]
        [AllowAnonymous]
        public async Task<string> Login(string username, string password)
        {
            password = GetEncodedHash(password, "123");

            if (!await IsValid(username, password))
                return "false";

            User user = await GetUser(username);
            string name = user?.Name ?? "error with loading name";
            List<Claim> claimCollection = new List<Claim> {
                    new Claim(ClaimTypes.Name, name),
                    new Claim(ClaimTypes.Email, username),
                    new Claim(ClaimTypes.Role, "Admin") };


            
            HttpContext.Response.Cookies.Append("idUser", user.IdEmployee.ToString());
            await HttpContext.Authentication.SignInAsync("AgroAppCookie", new ClaimsPrincipal(new ClaimsIdentity(claimCollection, IdentityCookieOptions.ApplicationCookieAuthenticationType)));
            return "true";
            //return user?.Role == Models.User.UserRole.Admin ? "admin" : "werknemer"; // auth succeed 
            

        }

        public static async Task Logout(HttpContext context)
        {
            await context.Authentication.SignOutAsync("AgroAppCookie");
        }

        public static async Task<bool> IsValid(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                throw new ArgumentException();

            string query = "SELECT COUNT(*) FROM employee WHERE username=@0 AND password=@1 AND isDeleted=@2;";
            using (MySqlConnection conn = await DatabaseConnection.GetConnection())
            using (MySqlDataReader reader = await MySqlHelper.ExecuteReaderAsync(conn, query,
                new MySqlParameter("@0", username),
                new MySqlParameter("@1", password),
                new MySqlParameter("@2", false)))
            {
                await reader.ReadAsync();
                return reader.GetInt32(0) == 1;
            }
        }

        public static async Task<User> GetUser(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException();

            string query = "SELECT idEmployee, name, username, role, isDeleted FROM employee WHERE username=@0";
            using (MySqlConnection conn = await DatabaseConnection.GetConnection())
            using (MySqlDataReader reader = await MySqlHelper.ExecuteReaderAsync(conn, query,
                new MySqlParameter("@0", email)))
            {
                await reader.ReadAsync();
                return reader.HasRows ? new User((reader["idEmployee"] as int?) ?? -1,
                    reader["name"] as string,
                    reader["username"] as string,
                    reader["role"] as string) : null;
            }
        }

        public static async Task<User> GetUser(HttpContext context)
        {
            return IsLoggedIn(context) ? await GetUser(int.Parse(context.Request.Cookies["idUser"])) : null;
        }

        public static async Task<User> GetUser(int id)
        {
            if (id < 0)
                return null;

            string query = "SELECT idEmployee, name, username, role, isDeleted FROM employee WHERE idEmployee=@0";
            using (MySqlConnection conn = await DatabaseConnection.GetConnection())
            using (MySqlDataReader reader = await MySqlHelper.ExecuteReaderAsync(conn, query,
                new MySqlParameter("@0", id)))
            {
                await reader.ReadAsync();
                return reader.HasRows ? new User(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(3)) : null;
            }
        }

        [HttpPost("register")]
        public async Task<string> AddUser([FromBody]User user)
        {
            if (string.IsNullOrWhiteSpace(user.Name) || string.IsNullOrWhiteSpace(user.Password))
                return "Een van de opgegeven velden is leeg";

            user.Password = GetEncodedHash(user.Password, "123");
            if (await IsValid(user.Username, user.Password))//GAAT NIET WERKEN, ALS NAAM OF WACHTWOORD MAAR IETS ANDERS IS RETURNED IE FALSE
                return "Gebruiker bestaat al";

            string query = "INSERT INTO employee (`name`, `username`, `password`, `role`) VALUES (@0, @1, @2, @3);";
            try
            {
                using (MySqlConnection conn = await DatabaseConnection.GetConnection())
                {
                    await MySqlHelper.ExecuteNonQueryAsync(conn, query,
                      new MySqlParameter("@0", user.Name),
                      new MySqlParameter("@1", user.Username),
                      new MySqlParameter("@2", user.Password),
                      new MySqlParameter("@3", user.Role));
                    return "succes";
                }
            }
            catch { return "Er is iets misgegaan, neem contact op met een ontwikkelaar!"; }
        }

        [HttpPost("wijzigen")]
        public async Task<bool> EditUser([FromBody]User user)
        {
            if (GetUser(user.IdEmployee) == null)
                return false;

            string query = "UPDATE employee SET Name=@0, Username=@1, Role=@2 WHERE IdEmployee=@3";
            using (MySqlConnection conn = await DatabaseConnection.GetConnection())
            using (MySqlDataReader reader = await MySqlHelper.ExecuteReaderAsync(conn, query,
                new MySqlParameter("@0", user.Name),
                new MySqlParameter("@1", user.Username),
                new MySqlParameter("@2", user.Role),
                new MySqlParameter("@3", user.IdEmployee)))
                return reader.RecordsAffected == 1;
        }

        [HttpGet("archive/{id}")]
        public async Task<bool> ArchiveUser(int id)
        {
            if (GetUser(id) == null)
                return false;

            string query = "UPDATE employee SET isDeleted=@0 WHERE idemployee=@1";
            using (MySqlConnection conn = await DatabaseConnection.GetConnection())
            using (MySqlDataReader reader = await MySqlHelper.ExecuteReaderAsync(conn, query,
                new MySqlParameter("@0", true),
                new MySqlParameter("@1", id)))
                return reader.RecordsAffected == 1;
        }

        //[Authorize("Admin")]
        [HttpGet("getall")]
        public async Task<IEnumerable<User>> GetAllUsers()
        {
            string query = "SELECT idEmployee, name, username, role, isDeleted FROM Employee WHERE isDeleted=@0";
            List<User> users = new List<User>();
            using (MySqlConnection conn = await DatabaseConnection.GetConnection())
            using (MySqlDataReader reader = await MySqlHelper.ExecuteReaderAsync(conn, query,
                new MySqlParameter("@0", false)))
                while (await reader.ReadAsync())
                    users.Add(new User(reader["idEmployee"] as int? ?? -1, reader["name"] as string, reader["username"] as string, reader["role"] as string));
            return users;
        }

        [HttpGet("getallarchived")]
        public async Task<IEnumerable<User>> GetArchivedUsers()
        {
            string query = "SELECT idemployee, name, username, role, isDeleted FROM employee WHERE isDeleted=@0";
            List<User> users = new List<User>();
            using (MySqlConnection conn = await DatabaseConnection.GetConnection())
            using (MySqlDataReader reader = await MySqlHelper.ExecuteReaderAsync(conn, query,
                new MySqlParameter("@0", true)))
                while (await reader.ReadAsync())
                    users.Add(new User(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(3)));
            return users;
        }

        [HttpGet("restore/{id}")]
        public async Task<bool> RestoreUser(int id)
        {
            if (GetUser(id) == null)
                return false;

            string query = "UPDATE employee SET isDeleted=@0 WHERE idemployee=@1";
            using (MySqlConnection conn = await DatabaseConnection.GetConnection())
            using (MySqlDataReader reader = await MySqlHelper.ExecuteReaderAsync(conn, query,
                new MySqlParameter("@0", false),
                new MySqlParameter("@1", id)))
                return reader.RecordsAffected == 1;
        }

        static string GetEncodedHash(string password, string salt)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] digest = md5.ComputeHash(Encoding.UTF8.GetBytes(password + salt));
            string base64digest = Convert.ToBase64String(digest, 0, digest.Length);
            return base64digest.Substring(0, base64digest.Length - 2);
        }
    }
}
