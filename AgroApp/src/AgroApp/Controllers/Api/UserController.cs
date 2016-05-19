using AgroApp.Managers;
using AgroApp.Models;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
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
            if (await IsValid(username, password))
            {
                string name = (await GetUser(username))?.Name ?? "error with loading name";
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
        [Authorize]
        public async Task<string> Logout(string username, string password)
        {
            await HttpContext.Authentication.SignOutAsync("AgroAppCookie");
            return "succes";
        }

        public static async Task<bool> IsValid(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                return false;

            using (MySqlConnection con = DatabaseConnection.GetConnection())
            {
                con.Open();
                string query = "SELECT COUNT(*) FROM werknemer WHERE gebruikersnaam=@0 AND wachtwoord=@1;";
                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@0", username);
                    cmd.Parameters.AddWithValue("@1", password);

                    DbDataReader reader = await cmd.ExecuteReaderAsync();
                    reader.Read();
                    return reader.GetInt32(0) == 1;
                }
            }
        }

        public static async Task<User> GetUser(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return null;

            using (MySqlConnection con = DatabaseConnection.GetConnection())
            {
                con.Open();
                string query = "SELECT idWerknemer, naam, gebruikersnaam, rol FROM werknemer WHERE gebruikersnaam=@0";
                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@0", email);
                    DbDataReader reader = await cmd.ExecuteReaderAsync();
                    while (reader.Read())
                        return new User(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(3));
                }
            }

            return null;
        }

        public static async Task<User> GetUser(int id)
        {
            if (id < 0)
                return null;

            using (MySqlConnection con = DatabaseConnection.GetConnection())
            {
                con.Open();
                string query = "SELECT idWerknemer, naam, gebruikersnaam, rol FROM werknemer WHERE idWerknemer=@0";
                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@0", id);
                    DbDataReader reader = await cmd.ExecuteReaderAsync();
                    while (reader.Read())
                        return new User(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(3));
                }
            }

            return null;
        }

        [HttpGet("register/{username}/{password}/{fullname}/{rol}")]
        public async Task<string> AddUser(string username, string password, string fullname, string rol)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(rol))
                return "Een van de opgegeven velden is leeg";

            password = GetEncodedHash(password, "123");
            if (await IsValid(username, password))//GAAT NIET WERKEN, ALS NAAM OF WACHTWOORD MAAR IETS ANDERS IS RETURNED IE FALSE
                return "Gebruiker bestaat al";
            
            using (MySqlConnection con = DatabaseConnection.GetConnection())
            {
                con.Open();
                string query = "INSERT INTO werknemer (`naam`, `gebruikersnaam`, `wachtwoord`, `rol`) VALUES (@0, @1, @2, @3);";
                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@0", fullname);
                    cmd.Parameters.AddWithValue("@1", username);
                    cmd.Parameters.AddWithValue("@2", password);
                    cmd.Parameters.AddWithValue("@3", rol);

                    try { await cmd.ExecuteNonQueryAsync(); return "true"; }
                    catch { return "Er is iets misgegaan!"; }
                }
            }
        }

        //[Authorize("Admin")]
        public static async Task<IEnumerable<User>> GetAllUsers()
        {
            using (MySqlConnection con = DatabaseConnection.GetConnection())
            {
                con.Open();
                string query = "SELECT idWerknemer, naam, gebruikersnaam, rol FROM werknemer";
                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {
                    var reader = await cmd.ExecuteReaderAsync();

                    List<User> users = new List<User>();
                    while (reader.Read())
                        users.Add(new User(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(3)));
                    return users;
                }
            }
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
