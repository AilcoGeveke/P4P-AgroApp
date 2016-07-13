using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using MySql.Data.MySqlClient;
using AgroApp.Models;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace AgroApp.Controllers.Api
{
    //public class AssignmentController : Controller
    //{
    //    // GET: /<controller>/
    //    public static async Task<List<Assignment>> GetAssignment(int id)
    //    {
    //        if (id < 0)
    //            return null;

    //        string query = "SELECT idMachine, type, number, name, tag, status FROM Machine WHERE idMachine=@0";
    //        using (MySqlConnection conn = await DatabaseConnection.GetConnection())
    //        using (MySqlDataReader reader = await MySqlHelper.ExecuteReaderAsync(conn, query,
    //            new MySqlParameter("@0", id)))
    //        {
    //            await reader.ReadAsync();
    //            return reader.HasRows ? new Assignment(reader["idAssignment"] as int -1, reader["location"] as string, reader["description"] as string, reader["date"] as DateTime? ?? null) :null;
    //        }
    //    }
    //}
}
