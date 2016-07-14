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
    [Route("api/[controller]")]
    public class AssignmentController : Controller
    {

        [HttpPost("add")]
        public async Task<string> AddAssignment([FromBody]Assignment am)
        {
            using (MySqlConnection conn = await DatabaseConnection.GetConnection())
            {
                DateTime date = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(am.Date).AddDays(1).ToLocalTime();
                int idAssignment = -1;
                string query = "INSERT INTO Assignment (Location, Description, IdCustomer, Date) VALUES (@0, @1, @2, @3); SELECT LAST_INSERT_ID()";
                using (MySqlDataReader reader = await MySqlHelper.ExecuteReaderAsync(conn, query,
                    new MySqlParameter("@0", am.Location),
                    new MySqlParameter("@1", am.Description),
                    new MySqlParameter("@2", am.Customer.IdCustomer),
                    new MySqlParameter("@3", date)))
                {
                    await reader.ReadAsync();
                    idAssignment = reader.GetInt32(0);
                }

                query = "INSERT INTO EmployeeAssignment (IdEmployee, idAssignment) VALUES (@0, @1)";
                foreach (User user in am.Employees)
                    await MySqlHelper.ExecuteNonQueryAsync(conn, query,
                        new MySqlParameter("@1", idAssignment),
                        new MySqlParameter("@0", user.IdEmployee));

                return "true";
            }
        }

        [HttpGet("getall/{archive}/{datelong}")]
        public async Task<IEnumerable<Assignment>> GetOpdrachten(bool archived, string datelong)
        {
            DateTime date = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(long.Parse(datelong)).ToLocalTime();
            string query = "SELECT Assignment.*, COUNT(EmployeeAssignment.idEmployeeAssignment) as count FROM Assignment LEFT JOIN EmployeeAssignment ON Assignment.idAssignment = EmployeeAssignment.idAssignment WHERE Assignment.Date >= DATE(@1) AND Assignment.Date < DATE(@2) GROUP BY Assignment.idAssignment";
            List<Assignment> assignments = new List<Assignment>();
            using (MySqlConnection conn = await DatabaseConnection.GetConnection())
            using (MySqlDataReader reader = await MySqlHelper.ExecuteReaderAsync(conn, query,
                new MySqlParameter("@0", 1),
                new MySqlParameter("@1", date.ToString("yyyy-MM-dd")), 
                new MySqlParameter("@2", date.AddDays(1).ToString("yyyy-MM-dd"))))
                while (await reader.ReadAsync())
                    assignments.Add(new Assignment(
                        idAssignment: reader["idAssignment"] as int? ?? -1,
                        location: reader["location"] as string,
                        description: reader["description"] as string,
                        date: reader["date"] as long? ?? 0)
                    {
                        Customer = await CustomerController.GetCustomer(reader["idCustomer"] as int? ?? -1),
                        EmployeeCount = (int)(reader["count"] as long? ?? (long)0)
                    });
            return assignments;
        }

        // GET: /<controller>/
        [HttpGet("getassignments/{datelong}")]
        public async Task<IEnumerable<Assignment>> GetAssignments(string datelong)
        {
            DateTime date = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(long.Parse(datelong)).ToLocalTime();
            string query = "SELECT assignment.* customer.name FROM assignment JOIN customer ON assignment.idCustomer = customer.idCustomer" +
            "JOIN employeeassignment ON employeeassignment.idAssignment = assignment.idAssignment WHERE Assignment.Date >= DATE(@0) AND Assignment.Date < DATE(@1) AND employeeassignment.idEmployee = @2; ";
            List<Assignment> assignments = new List<Assignment>();
            using (MySqlConnection conn = await DatabaseConnection.GetConnection())
            using (MySqlDataReader reader = await MySqlHelper.ExecuteReaderAsync(conn, query,
                new MySqlParameter("@0", date.ToString("yyyy-MM-dd")),
                new MySqlParameter("@1", date.AddDays(1).ToString("yyyy-MM-dd")),
                new MySqlParameter("@2", ViewData["idEmployee"])))
                while (await reader.ReadAsync())
                    assignments.Add(new Assignment(
                        idAssignment: reader["idAssignment"] as int? ?? -1,
                        location: reader["location"] as string,
                        description: reader["description"] as string,
                        date: reader["date"] as long? ?? 0)
                    {
                        Customer = await CustomerController.GetCustomer(reader["idCustomer"] as int? ?? -1)
                    });
            return assignments;
        }
    }

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
