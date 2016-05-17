using AgroApp.Controllers;
using AgroApp.Models;
using Microsoft.AspNet.Authorization;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace AgroApp.Account
{
    public static class UserManager
    {
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
    }
}
