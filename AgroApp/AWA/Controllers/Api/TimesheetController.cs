﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AWA.Models;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace AWA.Controllers.Api
{
    [Route("api/[controller]")]
    public class TimesheetController : Controller
    {
        private AgroContext _context;

        public TimesheetController(AgroContext context)
        {
            _context = context;
        }

        [HttpGet("getall/{idEmployeeAssignment}")]
        public List<Timesheet> GetAllTimeSheets(int idEmployeeAssignment)
        {
            return _context.Timesheets.Where(x => x.EmployeeAssignmentId == idEmployeeAssignment).ToList();
        }

        [HttpPost("add")]
        public bool AddTimeSheet([FromBody]Timesheet timesheet)
        {
            _context.Timesheets.Add(timesheet);
            _context.SaveChanges();
            return true;

            //using (MySqlConnection conn = await DatabaseConnection.GetConnection())
            //{
            //    int idTimesheet = -1;
            //    string query = "INSERT INTO Timesheet (timesheet.idEmployeeAssignment, timesheet.workType, timesheet.startTime, timesheet.endTime, timesheet.totalTime, timesheet.description) "
            //                + "VALUES (@0, @1, @2, @3, @4, @5); SELECT LAST_INSERT_ID()";
            //    using (MySqlDataReader reader = await MySqlHelper.ExecuteReaderAsync(conn, query,
            //        new MySqlParameter("@0", timesheet.IdEmployeeAssignment),
            //        new MySqlParameter("@1", timesheet.WorkType),
            //        new MySqlParameter("@2", timesheet.StartTime),
            //        new MySqlParameter("@3", timesheet.EndTime),
            //        new MySqlParameter("@4", timesheet.TotalTime),
            //        new MySqlParameter("@5", timesheet.Description)))
            //    {
            //        await reader.ReadAsync();
            //        idTimesheet = reader.GetInt32(0);
            //    }

            //    try
            //    {
            //        query = "INSERT INTO TimeSheetMachine (idTimesheet, idMachine) VALUES (@0, @1)";
            //        foreach (Machine machine in timesheet.Machines)
            //            await MySqlHelper.ExecuteNonQueryAsync(conn, query,
            //                new MySqlParameter("@0", idTimesheet),
            //                new MySqlParameter("@1", machine.IdMachine));

            //        query = "INSERT INTO TimeSheetAttachment (idTimesheet, idAttachment) VALUES (@0, @1)";
            //        foreach (Attachment attachment in timesheet.Attachments)
            //            await MySqlHelper.ExecuteNonQueryAsync(conn, query,
            //                new MySqlParameter("@0", idTimesheet),
            //                new MySqlParameter("@1", attachment.IdAttachment));

            //    }
            //    catch (Exception ex)
            //    {
            //        return false;
            //    }
            //    return true;
            //}
        }
    }
}