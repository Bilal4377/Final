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
    public class DmvsController : ControllerBase
    {
        private readonly DMVRecordsContext _context;
        private readonly JwtAuthenticationManager jwtAuthenticationManager;

        public DmvsController(DMVRecordsContext context, JwtAuthenticationManager jwtAuthenticationManager)
        {
            _context = context;
            this.jwtAuthenticationManager = jwtAuthenticationManager;
        }

        [Authorize]
        // GET: api/Dmvs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Dmv>>> GetDmvs()
        {
          if (_context.Dmvs == null)
          {
              return NotFound();
          }
            return await _context.Dmvs.ToListAsync();
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
        // GET: api/Dmvs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Dmv>> GetDmv(int id)
        {
          if (_context.Dmvs == null)
          {
              return NotFound();
          }
            var dmv = await _context.Dmvs.FindAsync(id);

            if (dmv == null)
            {
                return NotFound();
            }

            return dmv;
        }

        [Authorize]
        // PUT: api/Dmvs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDmv(int id, Dmv dmv)
        {
            if (id != dmv.DmvId)
            {
                return BadRequest();
            }

            _context.Entry(dmv).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DmvExists(id))
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
        // POST: api/Dmvs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Dmv>> PostDmv(Dmv dmv)
        {
          if (_context.Dmvs == null)
          {
              return Problem("Entity set 'DMVRecordsContext.Dmvs'  is null.");
          }
            _context.Dmvs.Add(dmv);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (DmvExists(dmv.DmvId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetDmv", new { id = dmv.DmvId }, dmv);
        }

        [Authorize]
        // DELETE: api/Dmvs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDmv(int id)
        {
            if (_context.Dmvs == null)
            {
                return NotFound();
            }
            var dmv = await _context.Dmvs.FindAsync(id);
            if (dmv == null)
            {
                return NotFound();
            }

            _context.Dmvs.Remove(dmv);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DmvExists(int id)
        {
            return (_context.Dmvs?.Any(e => e.DmvId == id)).GetValueOrDefault();
        }
    }

    public class User
    {
        public string username { get; set; }
        public string password { get; set; }
    }
}
