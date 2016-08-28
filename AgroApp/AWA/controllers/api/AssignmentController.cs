using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using AWA.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace AWA.Controllers.Api
{
    [Route("api/[controller]")]
    public class AssignmentController : Controller
    {
        private AgroContext _context;

        public AssignmentController(AgroContext context)
        {
            _context = context;
        }

        #region Api Methods

        [HttpPost("add")]
        public object AddAssignment([FromBody]Assignment am)
        {
            return AddAssignment(_context, am);
        }

        [HttpGet("get/{assignmentId}")]
        public object GetAllAssignment(int assignmentId)
        {
            return _context.Assignments.Where(x => x.AssignmentId == assignmentId);
        }

        [HttpGet("getallunverified/{dateToday}")]
        public object GetAllUnverfiedAssignments(string dateToday)
        {
            long date = long.Parse(dateToday);

            var list = (from x in _context.Assignments
                         join ea in _context.EmployeeAssignments
                         on x.AssignmentId equals ea.AssignmentId
                         where x.Date <= date && ea.IsVerified == false
                         select new
                         {
                             x.Customer,
                             x.Date,
                             x.Description,
                             x.AssignmentId,
                             x.Location,

                             EmployeeAssignments = x.EmployeeAssignments.Select(s => new
                             {
                                 s.IsVerified,
                                 s.UserId,
                                 User = new { s.User.Name }
                             })
                         }).Distinct();

            return list.ToList();
        }

        [HttpGet("getall/{datelong}")]
        [HttpGet("getall/{datelong}/{fillEmployees}")]
        public object GetAllAssignments(string datelong, bool fillEmployees = false)
        {
            return GetAllAssignments(long.Parse(datelong), long.Parse(datelong) + 86400000, fillEmployees);
        }

        [HttpGet("getallperiod/{start}/{end}")]
        [HttpGet("getallperiod/{start}/{end}/{fillEmployees}")]
        public object GetAllAssignments(string start, string end, bool fillEmployees = false)
        {
            return GetAllAssignments(long.Parse(start), long.Parse(end), fillEmployees);
        }

        public object GetAllAssignments(long startDate, long endDate, bool fillEmployees)
        {
            var assignments = _context.Assignments.Where(x => x.Date >= startDate && x.Date < endDate)
                .Include(x => x.Customer)
                .Include(x => x.EmployeeAssignments)
                .ThenInclude(x => x.User);

            var list = assignments.Select(x => new
            {
                x.Location,
                x.Customer,
                x.Date,
                x.Description,
                x.AssignmentId,

                EmployeeAssignments = x.EmployeeAssignments.Select(s => new
                {
                    s.IsVerified,
                    s.UserId,
                    User = new { s.User.Name, s.User.Username }
                })
            }).ToList();

            return list;
        }

        [HttpGet("getall/userspecific/{datelong}")]
        public object GetAssignmentsUserSpecific(string datelong)
        {
            long startDate = long.Parse(datelong);
            long endDate = startDate + 86400000;
            User user = UserController.GetUser(_context, HttpContext);

            var list = from x in _context.Assignments
                       join ea in _context.EmployeeAssignments
                       on x.AssignmentId equals ea.AssignmentId
                       where x.Date >= startDate && x.Date < endDate && ea.UserId == user.UserId
                       select new
                       {
                           x.Customer,
                           x.Date,
                           x.Description,
                           x.AssignmentId,

                           EmployeeAssignments = x.EmployeeAssignments.Select(s => new
                           {
                               s.IsVerified,
                               s.UserId
                           })
                       };

            return list.ToList();
        }

        #endregion

        #region Static Methods

        public static object AddAssignment(AgroContext context, Assignment am)
        {
            if (am == null)
                throw new ArgumentException("Assignment is null");

            //am.Customer = CustomerController.GetCustomer(context, am.CustomerId);
            am.EmployeeAssignments = new List<EmployeeAssignment>();
            if (am.Employees != null)
                foreach (User user in am.Employees)
                    am.EmployeeAssignments.Add(new EmployeeAssignment() { UserId = user.UserId });

            context.Assignments.Add(am);
            context.SaveChanges();

            return true;
        }

        public static Assignment GetAssignment(AgroContext context, int assignmentId)
        {
            return context.Assignments.FirstOrDefault(x => x.AssignmentId == assignmentId);
        }

        public static object GetEmployeeAssignment(AgroContext context, HttpContext httpContext, int assignmentId)
        {
            return GetEmployeeAssignment(context, UserController.GetLoggedInUserId(httpContext), assignmentId);
        }

        public static object GetEmployeeAssignment(AgroContext context, int employeeId, int assignmentId)
        {
            return context.EmployeeAssignments.Where(x => x.AssignmentId == assignmentId && x.UserId == employeeId)
                .Include(x => x.Records)
                .Include(x => x.Assignment)
                .ThenInclude(x => x.Customer)
                .Select(x => new
                {
                    x.EmployeeAssignmentId,
                    x.Records,
                    User = new { x.User.Name },

                    Assignment = new
                    {
                        x.Assignment.Customer,
                        x.Assignment.Date,
                        x.Assignment.Location,
                        x.Assignment.Description,
                        x.Assignment.Employees
                    }
                }).First();
        }

        #endregion
    }
}
