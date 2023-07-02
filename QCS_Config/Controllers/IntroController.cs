using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QCS_Config.Models;

namespace QCS_Config.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class IntroController : ControllerBase
    {
        private readonly DBContext _context;

        public IntroController(DBContext context)
        {
            _context = context;
        }

        // GET: api/Intro
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Intro>>> GetIntros()
        {
          if (_context.Intros == null)
          {
              return NotFound();
          }
            return await _context.Intros.ToListAsync();
        }

        // GET: api/Intro/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Intro>> GetIntro(int id)
        {
          if (_context.Intros == null)
          {
              return NotFound();
          }

          var intro = _context.Intros.FirstOrDefault(a => a.ClinikId == id);

            if (intro == null)
            {
                return NotFound();
            }

            return intro;
        }

        // PUT: api/Intro/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutIntro(int id, Intro intro)
        {
            intro.ClinikId = id;
            // if (id != intro.ClinikId)
            // {
            //     return BadRequest();
            // }

            _context.Entry(intro).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!IntroExists(id))
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

        // POST: api/Intro
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Intro>> PostIntro(Intro intro)
        {
          if (_context.Intros == null)
          {
              return Problem("Entity set 'DBContext.Intros'  is null.");
          }
            _context.Intros.Add(intro);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (IntroExists(intro.ClinikId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetIntro", new { id = intro.ClinikId }, intro);
        }

        // DELETE: api/Intro/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteIntro(int id)
        {
            if (_context.Intros == null)
            {
                return NotFound();
            }
            var intro = await _context.Intros.FindAsync(id);
            if (intro == null)
            {
                return NotFound();
            }

            _context.Intros.Remove(intro);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool IntroExists(int id)
        {
            return (_context.Intros?.Any(e => e.ClinikId == id)).GetValueOrDefault();
        }
    }
}
