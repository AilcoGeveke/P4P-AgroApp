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
    [Route("api/Machines")]
    public class MachinesController : Controller
    {
        private readonly AgroContext _context;

        public MachinesController(AgroContext context)
        {
            _context = context;
        }

        // GET: api/Machines
        [HttpGet]
        public IEnumerable<Machine> GetMachines()
        {
            return _context.Machines;
        }

        // GET: api/Machines/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMachine([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Machine machine = await _context.Machines.SingleOrDefaultAsync(m => m.MachineId == id);

            if (machine == null)
            {
                return NotFound();
            }

            return Ok(machine);
        }

        // PUT: api/Machines/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMachine([FromRoute] int id, [FromBody] Machine machine)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != machine.MachineId)
            {
                return BadRequest();
            }

            _context.Entry(machine).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MachineExists(id))
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

        // POST: api/Machines
        [HttpPost]
        public async Task<IActionResult> PostMachine([FromBody] Machine machine)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Machines.Add(machine);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (MachineExists(machine.MachineId))
                {
                    return new StatusCodeResult(StatusCodes.Status409Conflict);
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetMachine", new { id = machine.MachineId }, machine);
        }

        // DELETE: api/Machines/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMachine([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Machine machine = await _context.Machines.SingleOrDefaultAsync(m => m.MachineId == id);
            if (machine == null)
            {
                return NotFound();
            }

            _context.Machines.Remove(machine);
            await _context.SaveChangesAsync();

            return Ok(machine);
        }

        private bool MachineExists(int id)
        {
            return _context.Machines.Any(e => e.MachineId == id);
        }
    }
}