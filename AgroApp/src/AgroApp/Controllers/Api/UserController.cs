using AgroApp.Models;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Http;
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
                    new Claim(ClaimTypes.Role, "Admin")};

            HttpContext.Response.Cookies.Append("idUser", user.IdWerknemer.ToString());
            await HttpContext.Authentication.SignInAsync("AgroAppCookie", new ClaimsPrincipal(new ClaimsIdentity(claimCollection)));
            return user?.Rol == Models.User.UserRol.Admin ? "admin/main" : "werknemer/menu"; // auth succeed 

        }

        // GET: api/values
        [HttpGet("logout")]
        [Authorize]
        public async Task<string> Logout(string username, string password)
        {
            await HttpContext.Authentication.SignOutAsync("AgroAppCookie");
            return "succes";
        }

        public static async Task<bool> IsValid(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                throw new ArgumentException();

            string query = "SELECT COUNT(*) FROM werknemer WHERE gebruikersnaam=@0 AND wachtwoord=@1 AND isDeleted=@2;";
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

            string query = "SELECT idWerknemer, naam, gebruikersnaam, rol, isDeleted FROM werknemer WHERE gebruikersnaam=@0";
            using (MySqlConnection conn = await DatabaseConnection.GetConnection())
            using (MySqlDataReader reader = await MySqlHelper.ExecuteReaderAsync(conn, query,
                new MySqlParameter("@0", email)))
            {
                await reader.ReadAsync();
                return reader.HasRows ? new User(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetBoolean(4)) : null;
            }
        }

        public static async Task<User> GetUser(int id)
        {
            if (id < 0)
                return null;

            string query = "SELECT idWerknemer, naam, gebruikersnaam, rol, isDeleted FROM werknemer WHERE idWerknemer=@0";
            using (MySqlConnection conn = await DatabaseConnection.GetConnection())
            using (MySqlDataReader reader = await MySqlHelper.ExecuteReaderAsync(conn, query,
                new MySqlParameter("@0", id)))
            {
                await reader.ReadAsync();
                return reader.HasRows ? new User(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetBoolean(4)) : null;
            }
        }

        [HttpGet("register/{username}/{password}/{fullname}/{rol}")]
        public async Task<string> AddUser(string username, string password, string fullname, string rol)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(rol))
                return "Een van de opgegeven velden is leeg";

            password = GetEncodedHash(password, "123");
            if (await IsValid(username, password))//GAAT NIET WERKEN, ALS NAAM OF WACHTWOORD MAAR IETS ANDERS IS RETURNED IE FALSE
                return "Gebruiker bestaat al";

            string query = "INSERT INTO werknemer (`naam`, `gebruikersnaam`, `wachtwoord`, `rol`) VALUES (@0, @1, @2, @3);";
            try
            {
                using (MySqlConnection conn = await DatabaseConnection.GetConnection())
                {
                    await MySqlHelper.ExecuteNonQueryAsync(conn, query,
                      new MySqlParameter("@0", fullname),
                      new MySqlParameter("@1", username),
                      new MySqlParameter("@2", password),
                      new MySqlParameter("@3", rol));
                    return "true";
                }
            }
            catch { return "Er is iets misgegaan!"; }
        }

        [HttpGet("wijzigen/{id}/{naam}/{gebruikersnaam}/{rol}")]
        public async Task<bool> EditUser(int id, string naam, string gebruikersnaam, string rol)
        {
            if (GetUser(id) == null)
                return false;

            string query = "UPDATE Werknemer SET naam=@0, gebruikersnaam=@1, rol=@2 WHERE idWerknemer=@3";
            using (MySqlConnection conn = await DatabaseConnection.GetConnection())
            using (MySqlDataReader reader = await MySqlHelper.ExecuteReaderAsync(conn, query,
                new MySqlParameter("@0", naam),
                new MySqlParameter("@1", gebruikersnaam),
                new MySqlParameter("@2", rol),
                new MySqlParameter("@3", id)))
                return reader.RecordsAffected == 1;
        }

        [HttpGet("archiveren/{id}")]
        public async Task<bool> ArchiveUser(int id)
        {
            if (GetUser(id) == null)
                return false;

            string query = "UPDATE Werknemer SET isDeleted=@0 WHERE idWerknemer=@1";
            using (MySqlConnection conn = await DatabaseConnection.GetConnection())
            using (MySqlDataReader reader = await MySqlHelper.ExecuteReaderAsync(conn, query,
                new MySqlParameter("@0", true),
                new MySqlParameter("@1", id)))
                return reader.RecordsAffected == 1; ;
        }

        //[Authorize("Admin")]
        public static async Task<IEnumerable<User>> GetAllUsers()
        {
            string query = "SELECT idWerknemer, naam, gebruikersnaam, rol, isDeleted FROM Werknemer WHERE isDeleted=@0";
            List<User> users = new List<User>();
            using (MySqlConnection conn = await DatabaseConnection.GetConnection())
            using (MySqlDataReader reader = await MySqlHelper.ExecuteReaderAsync(conn, query,
                new MySqlParameter("@0", false)))
                while (await reader.ReadAsync())
                    users.Add(new User(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(3)));
            return users;
        }

        public static async Task<IEnumerable<User>> GetArchivedUsers()
        {
            string query = "SELECT idWerknemer, naam, gebruikersnaam, rol, isDeleted FROM Werknemer WHERE isDeleted=@0";
            List<User> users = new List<User>();
            using (MySqlConnection conn = await DatabaseConnection.GetConnection())
            using (MySqlDataReader reader = await MySqlHelper.ExecuteReaderAsync(conn, query,
                new MySqlParameter("@0", true)))
                while (await reader.ReadAsync())
                    users.Add(new User(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetBoolean(4)));
            return users;
        }

        [HttpGet("gebruikerbeheer/terughalen/{id}")]
        public async Task<bool> ReAddKlant(int id)
        {
            if (GetUser(id) == null)
                return false;

            string query = "UPDATE Werknemer SET isDeleted=@0 WHERE idWerknemer=@1";
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
