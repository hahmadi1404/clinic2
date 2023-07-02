using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QCS_Config.Models;

namespace QCS_Config.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GraphyController : ControllerBase
    {
        private readonly DBContext _context;

        public GraphyController(DBContext context)
        {
            _context = context;
        }
        private int getClinicId()
        {
            return Convert.ToInt32(User.Claims.First(a => a.Type.ToLower().Contains("role")).Value);
        }
        // GET: api/Graphy
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Graphy>>> GetGraphies()
        {
            int cId = getClinicId();
          if (_context.Graphies == null)
          {
              return NotFound();
          }
            return await _context.Graphies.Where(a=>a.ClinicId==cId).ToListAsync();
        }

        // GET: api/Graphy/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Graphy>> GetGraphy(int id)
        {
          if (_context.Graphies == null)
          {
              return NotFound();
          }
            var graphy = await _context.Graphies.FindAsync(id);

            if (graphy == null)
            {
                return NotFound();
            }

            return graphy;
        }

        // PUT: api/Graphy/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGraphy(int id, Graphy graphy)
        {
            if (id != graphy.GraphyId)
            {
                return BadRequest();
            }

            _context.Entry(graphy).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GraphyExists(id))
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

        // POST: api/Graphy
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Graphy>> PostGraphy(Graphy graphy)
        {
            graphy.ClinicId=getClinicId();
          if (_context.Graphies == null)
          {
              return Problem("Entity set 'DBContext.Graphies'  is null.");
          }
            _context.Graphies.Add(graphy);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (GraphyExists(graphy.GraphyId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetGraphy", new { id = graphy.GraphyId }, graphy);
        }

        // DELETE: api/Graphy/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGraphy(int id)
        {
            if (_context.Graphies == null)
            {
                return NotFound();
            }
            var graphy = await _context.Graphies.FindAsync(id);
            if (graphy == null)
            {
                return NotFound();
            }

            _context.Graphies.Remove(graphy);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool GraphyExists(int id)
        {
            return (_context.Graphies?.Any(e => e.GraphyId == id)).GetValueOrDefault();
        }
    }
}
