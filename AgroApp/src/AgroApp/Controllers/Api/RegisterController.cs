using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using MySql.Data.MySqlClient;
using System.Data.Common;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace AgroApp.Controllers.Api
{
    [Route("api/[controller]")]
    public class RegisterController : Controller
    {
        // GET api/register/email/pass
        [HttpGet("{id}/{pass}")]
        public async Task<string> Get(string id, string pass)
        {
            if (string.IsNullOrWhiteSpace(id) || string.IsNullOrWhiteSpace(pass))
                return "Ingevoerde gegevens zijn incompleet";

            using (MySqlConnection con = DatabaseConnection.GetConnection())
            {
                con.Open();
                string query = "COUNT(*) FROM werknemer WHERE gebruikersnaam=@0";
                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@0", id);
                    cmd.Parameters.AddWithValue("@1", pass);

                    DbDataReader reader = await cmd.ExecuteReaderAsync();
                    if (reader.GetInt16(0) != 0)
                        return "Gebruiker bestaat al";
                }

                query = "";
                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@0", id);
                    cmd.Parameters.AddWithValue("@1", pass);

                    DbDataReader reader = await cmd.ExecuteReaderAsync();
                    if (reader.GetInt16(0) != 0)
                        return "Gebruiker bestaat al";
                }
            }

            return "Er is iets misgegaan";
        }
    }
}
