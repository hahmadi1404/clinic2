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
    public class ServiceController : ControllerBase
    {
        private readonly DBContext _context;

        
        private int getClinicId()
        {
            return Convert.ToInt32(User.Claims.First(a => a.Type.ToLower().Contains("role")).Value);
        }
        public ServiceController(DBContext context)
        {
            _context = context;
        }

        // GET: api/Service
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Service>>> GetServices()
        {
            int cId = getClinicId();
          if (_context.Services == null)
          {
              return NotFound();
          }
            return await _context.Services.Where(a=>a.ClinikId==cId).ToListAsync();
        }

        // GET: api/Service/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Service>> GetService(int id)
        {
          if (_context.Services == null)
          {
              return NotFound();
          }
            var service = await _context.Services.FindAsync(id);

            if (service == null)
            {
                return NotFound();
            }

            return service;
        }

        // PUT: api/Service/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutService(int id, Service service)
        {
          
            if (id != service.ServiceId)
            {
                return BadRequest();
            }

            _context.Entry(service).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ServiceExists(id))
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

        // POST: api/Service
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Service>> PostService(Service service)
        {
            
            service.ClinikId=getClinicId();
          if (_context.Services == null)
          {
              return Problem("Entity set 'DBContext.Services'  is null.");
          }
            _context.Services.Add(service);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetService", new { id = service.ServiceId }, service);
        }

        // DELETE: api/Service/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteService(int id)
        {
            if (_context.Services == null)
            {
                return NotFound();
            }
            var service = await _context.Services.FindAsync(id);
            if (service == null)
            {
                return NotFound();
            }

            _context.Services.Remove(service);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ServiceExists(int id)
        {
            return (_context.Services?.Any(e => e.ServiceId == id)).GetValueOrDefault();
        }
    }
}
