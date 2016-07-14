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
    public class TimesheetController : Controller
    {
        //[HttpPost("getall")]
        //public async Task<bool> GetAllTimeSheets([FromBody]Timesheet timesheet)
        //{
        //    using (MySqlConnection conn = await DatabaseConnection.GetConnection())
        //    {
        //        int idTimeSheetPart = -1;
        //        string query = "SELECT TimesheetPart.startTime, TimesheetPart.endTime, TimesheetPart.totalTime, "
        //    + "TimesheetPart.description, Customer.name, Assignment.location, Employee.name "
        //    + "FROM TimesheetPart "
        //    + "JOIN EmployeeAssignment "
        //    + "ON TimesheetPart.idEmployeeAssignment = EmployeeAssignment.idEmployeeAssignment "
        //    + "JOIN Employee "
        //    + "ON Employee.idEmployee = EmployeeAssignment.idEmployee "
        //    + "JOIN Assignment "
        //    + "ON Assignment.idAssignment = EmployeeAssignment.idAssignment "
        //    + "JOIN Customer "
        //    + "ON Customer.idCustomer = Assignment.idCustomer "
        //    + "JOIN CoWorker "
        //    + "ON CoWorker.idTimesheetPart = TimesheetPart.idTimesheetPart "
        //    + "WHERE assignment.date = @0 AND Employee.idEmployee = @1; "
        //    + "WHERE assignment.date = @0 AND Employee.idEmployee = @1; SELECT LAST_INSERT_ID();";
        //        using (MySqlDataReader reader = await MySqlHelper.ExecuteReaderAsync(conn, query,
        //           new MySqlParameter("@0", opdracht.Location),
        //           new MySqlParameter("@1", opdracht.Description),
        //           new MySqlParameter("@2", opdracht.Customer.IdCustomer),
        //           new MySqlParameter("@3", opdracht.Date)))
        //        {
        //            await reader.ReadAsync();
        //            idTimeSheetPart = reader.GetInt32(0);
        //        }

        //        query = "SELECT Machine.name, Machine.number FROM Machine "
        //                    + "JOIN workordermachine "
        //                    + "ON workordermachine.idMachine = workordermachine.idMachine "
        //                    + "JOIN timesheetpart "
        //                    + "ON workordermachine.idTimesheetPart = timesheetpart.idTimesheetPart "
        //                    + "WHERE timesheetpart.idTimesheetPart = @3;";
        //        foreach (User user in opdracht.Users)
        //            await MySqlHelper.ExecuteNonQueryAsync(conn, query,
        //                new MySqlParameter("@1", opdrachtId),
        //                new MySqlParameter("@0", user.IdWerknemer));

        //        return true;
        //    }
        //}

        [HttpPost("add")]
        public async Task<bool> AddTimeSheet([FromBody]Timesheet timesheet)
        {
            using (MySqlConnection conn = await DatabaseConnection.GetConnection())
            {
                int idTimesheet = -1;
                string query = "INSERT INTO Timesheet (timesheet.workType, timesheet.startTime, timesheet.endTime, timesheet.totalTime, timesheet.description) "
                            + "VALUES (@0, @1, @2, @3, @4)” SELECT LAST_INSERT_ID()";
                using (MySqlDataReader reader = await MySqlHelper.ExecuteReaderAsync(conn, query,
                    new MySqlParameter("@0", timesheet.WorkType),
                    new MySqlParameter("@1", timesheet.StartTime),
                    new MySqlParameter("@2", timesheet.EndTime),
                    new MySqlParameter("@3", timesheet.TotalTime),
                    new MySqlParameter("@4", timesheet.Description)))
                {
                    await reader.ReadAsync();
                    idTimesheet = reader.GetInt32(0);
                }

                query = "INSERT INTO TimeSheetMachine (idTimesheet, idMachine) VALUES (@0, @1)";
                foreach (Machine machine in timesheet.Machines)
                    await MySqlHelper.ExecuteNonQueryAsync(conn, query,
                        new MySqlParameter("@1", idTimesheet),
                        new MySqlParameter("@0", machine.IdMachine));

                query = "INSERT INTO TimeSheetAttachment (idTimesheet, idAttachment) VALUES (@0, @1)";
                foreach (Attachment attachment in timesheet.Attachments)
                    await MySqlHelper.ExecuteNonQueryAsync(conn, query,
                        new MySqlParameter("@1", idTimesheet),
                        new MySqlParameter("@0", attachment.IdAttachment));

                return true;

            }
        }
    }
}
