using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using AgroApp.Models;
using MySql.Data.MySqlClient;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace AgroApp.Controllers.Api
{
    [Route("api/[controller]")]
    public class CargoController : Controller
    {
        [HttpPost("add")]
        public async Task<bool> AddMachine([FromBody]Machine machine)
        {
            using (MySqlConnection conn = await DatabaseConnection.GetConnection())
            {
                string query = "SELECT * FROM Machine WHERE number=@0";
                using (MySqlDataReader reader = await MySqlHelper.ExecuteReaderAsync(conn, query,
                    new MySqlParameter("@0", machine.Number)))
                    if (reader.HasRows)
                        return false;

                query = "INSERT INTO Machine (name, number, tag, type, status) VALUES (@0, @1, @2, @3, @4)";
                using (MySqlDataReader reader = await MySqlHelper.ExecuteReaderAsync(conn, query,
                    new MySqlParameter("@0", machine.Name),
                    new MySqlParameter("@1", machine.Number),
                    new MySqlParameter("@2", machine.Tag),
                    new MySqlParameter("@3", machine.Type),
                    new MySqlParameter("@4", machine.Status)))
                    return reader.RecordsAffected == 1;
            }
        }
    }
}
