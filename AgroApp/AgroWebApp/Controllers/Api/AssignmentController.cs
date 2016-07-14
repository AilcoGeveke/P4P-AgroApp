using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using MySql.Data.MySqlClient;
using AgroApp.Models;
using Microsoft.AspNet.Http;

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
                DateTime date = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(am.Date).ToLocalTime();
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
        
        public static async Task<int> GetEmployeeAssignment(HttpContext context, int idAssignment)
        {
            if (idAssignment < 0)
                return -1;

            User user = await UserController.GetUser(context);
            string query = "SELECT idEmployeeAssignment FROM EmployeeAssignment WHERE idAssignment = @0 AND idEmployee = @1";

            using (MySqlConnection conn = await DatabaseConnection.GetConnection())
            using (MySqlDataReader reader = await MySqlHelper.ExecuteReaderAsync(conn, query,
                new MySqlParameter("@0", idAssignment),
                new MySqlParameter("@1", user.IdEmployee)))
            {
                int idEmployeeAssignment = reader.GetInt32(0);
                return idEmployeeAssignment;
            }
        }

        //[HttpGet("getall/{idAssignment}")]
        //public async Task<IEnumerable<Assignment>> GetAssignment(int idAssignment)
        //{
        //    string query = "SELECT Assignment.*, COUNT(EmployeeAssignment.idEmployeeAssignment) as count FROM Assignment LEFT JOIN EmployeeAssignment ON Assignment.idAssignment = EmployeeAssignment.idAssignment WHERE Assignment.Date >= DATE(@1) AND Assignment.Date < DATE(@2) GROUP BY Assignment.idAssignment";
        //    List<Assignment> assignments = new List<Assignment>();
        //    using (MySqlConnection conn = await DatabaseConnection.GetConnection())
        //    {
        //        using (MySqlDataReader reader = await MySqlHelper.ExecuteReaderAsync(conn, query,
        //            new MySqlParameter("@0", 1),
        //            new MySqlParameter("@1", startDate.ToString("yyyy-MM-dd")),
        //            new MySqlParameter("@2", endDate.ToString("yyyy-MM-dd"))))
        //            while (await reader.ReadAsync())
        //                assignments.Add(new Assignment(
        //                    idAssignment: reader["idAssignment"] as int? ?? -1,
        //                    location: reader["location"] as string,
        //                    description: reader["description"] as string,
        //                    date: reader["date"] as long? ?? 0)
        //                {
        //                    Customer = await CustomerController.GetCustomer(reader["idCustomer"] as int? ?? -1),
        //                    EmployeeCount = (int)(reader["count"] as long? ?? (long)0)
        //                });
        //        return assignments;
        //    }
        //}

        [HttpGet("getall/{datelong}")]
        [HttpGet("getall/{datelong}/{fillEmployees}")]
        public async Task<IEnumerable<Assignment>> GetAllAssignments(string datelong, bool fillEmployees = false)
        {
            DateTime date = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(long.Parse(datelong)).ToLocalTime();
            return await GetAllAssignments(date, date.AddDays(1), fillEmployees);
        }

        [HttpGet("getallperiod/{start}/{end}")]
        [HttpGet("getallperiod/{start}/{end}/{fillEmployees}")]
        public async Task<IEnumerable<Assignment>> GetAllAssignments(string start, string end, bool fillEmployees = false)
        {
            DateTime startDate = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(long.Parse(start)).ToLocalTime();
            DateTime endDate = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(long.Parse(end)).ToLocalTime();
            return await GetAllAssignments(startDate, endDate, fillEmployees);
        }

        public async Task<IEnumerable<Assignment>> GetAllAssignments(DateTime startDate, DateTime endDate, bool fillEmployees)
        {
            string query = "SELECT Assignment.*, COUNT(EmployeeAssignment.idEmployeeAssignment) as count FROM Assignment LEFT JOIN EmployeeAssignment ON Assignment.idAssignment = EmployeeAssignment.idAssignment WHERE Assignment.Date >= DATE(@1) AND Assignment.Date < DATE(@2) GROUP BY Assignment.idAssignment";
            List<Assignment> assignments = new List<Assignment>();
            using (MySqlConnection conn = await DatabaseConnection.GetConnection())
            {
                using (MySqlDataReader reader = await MySqlHelper.ExecuteReaderAsync(conn, query,
                    new MySqlParameter("@0", 1),
                    new MySqlParameter("@1", startDate.ToString("yyyy-MM-dd")),
                    new MySqlParameter("@2", endDate.ToString("yyyy-MM-dd"))))
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

                if (fillEmployees)
                    foreach (Assignment a in assignments)
                        using (MySqlDataReader reader = await MySqlHelper.ExecuteReaderAsync(conn,
                            "SELECT EmployeeAssignment.idEmployeeAssignment FROM EmployeeAssignment LEFT JOIN Assignment on EmployeeAssignment.IdAssignment = @0",
                            new MySqlParameter("@0", a.IdAssignment)))
                            while (await reader.ReadAsync())
                                a.Employees.Add(new Models.User() { IdEmployee = reader["idEmployeeAssignment"] as int? ?? -1 });

            }

            if (fillEmployees)
                foreach (Assignment ass in assignments)
                    for (int i = 0; i < ass.EmployeeCount; i++)
                        ass.Employees[i] = await UserController.GetUser(ass.Employees[i].IdEmployee);

            return assignments;
        }


        // GET: /<controller>/
        public async Task<IEnumerable<Assignment>> GetAssignmentsUserSpecific(string datelong)
        {
            User user = await UserController.GetUser(HttpContext);
            DateTime date = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(long.Parse(datelong)).ToLocalTime();
            string query = "SELECT assignment.*, customer.name FROM assignment LEFT JOIN customer ON assignment.idCustomer = customer.idCustomer" +
            " LEFT JOIN employeeassignment ON employeeassignment.idAssignment = assignment.idAssignment WHERE Assignment.Date >= DATE(@0) AND Assignment.Date < DATE(@1) AND employeeassignment.idEmployee = @2; ";
            List<Assignment> assignments = new List<Assignment>();
            using (MySqlConnection conn = await DatabaseConnection.GetConnection())
            using (MySqlDataReader reader = await MySqlHelper.ExecuteReaderAsync(conn, query,
                new MySqlParameter("@0", date.ToString("yyyy-MM-dd")),
                new MySqlParameter("@1", date.AddDays(1).ToString("yyyy-MM-dd")),
                new MySqlParameter("@2", user.IdEmployee)))
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

        [HttpGet("getall/{datelong}/{userSpecific}")]
        public async Task<IEnumerable<Assignment>> GetAssignments(string datelong, bool userSpecific)
        {
            if (userSpecific)
                return await GetAllAssignments(datelong, false);
            else
                return await GetAssignmentsUserSpecific(datelong);
        }

        [HttpGet("deleteall")]
        public async Task<bool> DeleteAllData()
        {
            string query = "SET FOREIGN_KEY_CHECKS = 0; "
                    + "TRUNCATE TABLE Cargo; "
                    + "TRUNCATE TABLE RoadPlate ; "
                    + "TRUNCATE TABLE EmployeeAssignment; "
                    + "TRUNCATE TABLE Assignment; "
                    + "TRUNCATE TABLE TimesheetAttachment; "
                    + "TRUNCATE TABLE Timesheet;"
                    + "TRUNCATE TABLE TimesheetWorkType;"
                    + "SET FOREIGN_KEY_CHECKS = 1;";
            using (MySqlConnection conn = await DatabaseConnection.GetConnection())
            using (MySqlDataReader reader = await MySqlHelper.ExecuteReaderAsync(conn, query))
                return reader.RecordsAffected >= 1; ;
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
