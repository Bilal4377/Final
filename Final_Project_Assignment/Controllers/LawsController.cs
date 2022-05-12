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
    public class LawsController : ControllerBase
    {
        private readonly DMVRecordsContext _context;
        private readonly JwtAuthenticationManager jwtAuthenticationManager;

        public LawsController(DMVRecordsContext context, JwtAuthenticationManager jwtAuthenticationManager)
        {
            _context = context;
            this.jwtAuthenticationManager = jwtAuthenticationManager;
        }

        [Authorize]
        // GET: api/Laws
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Law>>> GetLaws()
        {
          if (_context.Laws == null)
          {
              return NotFound();
          }
            return await _context.Laws.ToListAsync();
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
        // GET: api/Laws/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Law>> GetLaw(int id)
        {
          if (_context.Laws == null)
          {
              return NotFound();
          }
            var law = await _context.Laws.FindAsync(id);

            if (law == null)
            {
                return NotFound();
            }

            return law;
        }

        [Authorize]
        // PUT: api/Laws/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLaw(int id, Law law)
        {
            if (id != law.LawId)
            {
                return BadRequest();
            }

            _context.Entry(law).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LawExists(id))
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
        // POST: api/Laws
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Law>> PostLaw(Law law)
        {
          if (_context.Laws == null)
          {
              return Problem("Entity set 'DMVRecordsContext.Laws'  is null.");
          }
            _context.Laws.Add(law);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (LawExists(law.LawId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetLaw", new { id = law.LawId }, law);
        }

        [Authorize]
        // DELETE: api/Laws/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLaw(int id)
        {
            if (_context.Laws == null)
            {
                return NotFound();
            }
            var law = await _context.Laws.FindAsync(id);
            if (law == null)
            {
                return NotFound();
            }

            _context.Laws.Remove(law);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LawExists(int id)
        {
            return (_context.Laws?.Any(e => e.LawId == id)).GetValueOrDefault();
        }
    }
}
