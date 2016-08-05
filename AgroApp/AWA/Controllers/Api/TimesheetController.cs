using System;
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
    }
}
