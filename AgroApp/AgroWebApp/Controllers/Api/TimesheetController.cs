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
        [HttpGet("getall/{idEmployeeAssignment}")]
        public async Task<IEnumerable<Timesheet>> GetAllTimeSheets(int idEmployeeAssignment)
        {
            using (MySqlConnection conn = await DatabaseConnection.GetConnection())
            {
                List<Timesheet> timesheets = new List<Timesheet>();
                string query = "SELECT timesheet.* FROM timesheet WHERE idEmployeeAssignment = @0;";
                using (MySqlDataReader reader = await MySqlHelper.ExecuteReaderAsync(conn, query,
                   new MySqlParameter("@0", idEmployeeAssignment)))
                    while (await reader.ReadAsync())
                        timesheets.Add(new Timesheet(
                            idTimesheet: reader["idTimesheet"] as int? ?? -1,
                            idEmployeeAssignment: idEmployeeAssignment,
                            workType: reader["workType"] as string,
                            startTime: reader["startTime"] as long? ?? -1,
                             endTime: reader["endTime"] as long? ?? -1,
                              totalTime: reader["totalTime"] as long? ?? -1,
                            description: reader["description"] as string
                            ));
                return timesheets;
            }
        }

        [HttpPost("add")]
        public async Task<bool> AddTimeSheet([FromBody]Timesheet timesheet)
        {
            using (MySqlConnection conn = await DatabaseConnection.GetConnection())
            {
                int idTimesheet = -1;
                string query = "INSERT INTO Timesheet (timesheet.idEmployeeAssignment, timesheet.workType, timesheet.startTime, timesheet.endTime, timesheet.totalTime, timesheet.description) "
                            + "VALUES (@0, @1, @2, @3, @4, @5); SELECT LAST_INSERT_ID()";
                using (MySqlDataReader reader = await MySqlHelper.ExecuteReaderAsync(conn, query,
                    new MySqlParameter("@0", timesheet.IdEmployeeAssignment),
                    new MySqlParameter("@1", timesheet.WorkType),
                    new MySqlParameter("@2", timesheet.StartTime),
                    new MySqlParameter("@3", timesheet.EndTime),
                    new MySqlParameter("@4", timesheet.TotalTime),
                    new MySqlParameter("@5", timesheet.Description)))
                {
                    await reader.ReadAsync();
                    idTimesheet = reader.GetInt32(0);
                }

                try
                {
                    query = "INSERT INTO TimeSheetMachine (idTimesheet, idMachine) VALUES (@0, @1)";
                    foreach (Machine machine in timesheet.Machines)
                        await MySqlHelper.ExecuteNonQueryAsync(conn, query,
                            new MySqlParameter("@0", idTimesheet),
                            new MySqlParameter("@1", machine.IdMachine));

                    query = "INSERT INTO TimeSheetAttachment (idTimesheet, idAttachment) VALUES (@0, @1)";
                    foreach (Attachment attachment in timesheet.Attachments)
                        await MySqlHelper.ExecuteNonQueryAsync(conn, query,
                            new MySqlParameter("@0", idTimesheet),
                            new MySqlParameter("@1", attachment.IdAttachment));

                }
                catch (Exception ex)
                {
                    return false;
                }
                return true;
            }
        }
    }
}
