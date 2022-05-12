using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Final_Project_Assignment.Data;
using Final_Project_Assignment.Models;
using Microsoft.AspNetCore.Authorization;

namespace Final_Project_Assignment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DriverInfractionsController : ControllerBase
    {
        private readonly DMVRecordsContext _context;
        private readonly JwtAuthenticationManager jwtAuthenticationManager;

        public DriverInfractionsController(DMVRecordsContext context, JwtAuthenticationManager jwtAuthenticationManager)
        {
            _context = context;
            this.jwtAuthenticationManager = jwtAuthenticationManager;
        }

        [Authorize]
        // GET: api/DriverInfractions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DriverInfraction>>> GetDriverInfractions()
        {
          if (_context.DriverInfractions == null)
          {
              return NotFound();
          }
            return await _context.DriverInfractions.ToListAsync();
        }

        [AllowAnonymous]
        [HttpPost("Authorize")]
        public IActionResult AuthUser([FromBody] User usr)
        {
            var token = jwtAuthenticationManager.Authenticate(usr.username, usr.password);
            if (token == null)
            {
                return Unauthorized();
            }
            return Ok(token);
        }

        [Authorize]
        // GET: api/DriverInfractions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DriverInfraction>> GetDriverInfraction(int id)
        {
          if (_context.DriverInfractions == null)
          {
              return NotFound();
          }
            var driverInfraction = await _context.DriverInfractions.FindAsync(id);

            if (driverInfraction == null)
            {
                return NotFound();
            }

            return driverInfraction;
        }

        [Authorize]
        // PUT: api/DriverInfractions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDriverInfraction(int id, DriverInfraction driverInfraction)
        {
            if (id != driverInfraction.DriverId)
            {
                return BadRequest();
            }

            _context.Entry(driverInfraction).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DriverInfractionExists(id))
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

        [Authorize]
        // POST: api/DriverInfractions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<DriverInfraction>> PostDriverInfraction(DriverInfraction driverInfraction)
        {
          if (_context.DriverInfractions == null)
          {
              return Problem("Entity set 'DMVRecordsContext.DriverInfractions'  is null.");
          }
            _context.DriverInfractions.Add(driverInfraction);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (DriverInfractionExists(driverInfraction.DriverId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetDriverInfraction", new { id = driverInfraction.DriverId }, driverInfraction);
        }

        [Authorize]
        // DELETE: api/DriverInfractions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDriverInfraction(int id)
        {
            if (_context.DriverInfractions == null)
            {
                return NotFound();
            }
            var driverInfraction = await _context.DriverInfractions.FindAsync(id);
            if (driverInfraction == null)
            {
                return NotFound();
            }

            _context.DriverInfractions.Remove(driverInfraction);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DriverInfractionExists(int id)
        {
            return (_context.DriverInfractions?.Any(e => e.DriverId == id)).GetValueOrDefault();
        }
    }
}
