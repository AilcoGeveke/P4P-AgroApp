using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AWA.Models;

namespace AWA.Controllers.Api
{
    [Produces("application/json")]
    [Route("api/Timesheets")]
    public class TimesheetsController : Controller
    {
        private readonly AgroContext _context;

        public TimesheetsController(AgroContext context)
        {
            _context = context;
        }

        // GET: api/Timesheets
        [HttpGet]
        public IEnumerable<Timesheet> GetTimesheets()
        {
            return _context.Timesheets;
        }

        // GET: api/Timesheets/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTimesheet([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Timesheet timesheet = await _context.Timesheets.SingleOrDefaultAsync(m => m.TimesheetId == id);

            if (timesheet == null)
            {
                return NotFound();
            }

            return Ok(timesheet);
        }

        // PUT: api/Timesheets/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTimesheet([FromRoute] int id, [FromBody] Timesheet timesheet)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            Timesheet original = await _context.Timesheets.SingleOrDefaultAsync(m => m.TimesheetId == id);
            original.IsVerified = timesheet.IsVerified;

            try
            {
                original.Records?.Clear();
                await _context.SaveChangesAsync();
                original.Records = timesheet.Records;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TimesheetExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Timesheets
        [HttpPost]
        public async Task<IActionResult> PostTimesheet([FromBody] Timesheet timesheet)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Timesheets.Add(timesheet);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (TimesheetExists(timesheet.TimesheetId))
                {
                    return new StatusCodeResult(StatusCodes.Status409Conflict);
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetTimesheet", new { id = timesheet.TimesheetId }, timesheet);
        }

        // DELETE: api/Timesheets/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTimesheet([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Timesheet timesheet = await _context.Timesheets.SingleOrDefaultAsync(m => m.TimesheetId == id);
            if (timesheet == null)
            {
                return NotFound();
            }

            _context.Timesheets.Remove(timesheet);
            await _context.SaveChangesAsync();

            return Ok(timesheet);
        }

        private bool TimesheetExists(int id)
        {
            return _context.Timesheets.Any(e => e.TimesheetId == id);
        }
    }
}