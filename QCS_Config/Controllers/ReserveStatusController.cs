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
    public class ReserveStatusController : ControllerBase
    {
        private readonly DBContext _context;

        public ReserveStatusController(DBContext context)
        {
            _context = context;
        }
        private int getClinicId()
        {
            return Convert.ToInt32(User.Claims.First(a => a.Type.ToLower().Contains("role")).Value);
        }
        // GET: api/ReserveStatus
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReserveStatus>>> GetReserveStatuses()
        {
            int cId = getClinicId();
          if (_context.ReserveStatuses == null)
          {
              return NotFound();
          }
            return await _context.ReserveStatuses.Where(a=>a.ClinicId==cId).ToListAsync();
        }
        //
        // // GET: api/ReserveStatus/5
        // [HttpGet("{id}")]
        // public async Task<ActionResult<ReserveStatus>> GetReserveStatus(int id)
        // {
        //   if (_context.ReserveStatuses == null)
        //   {
        //       return NotFound();
        //   }
        //     var reserveStatus = await _context.ReserveStatuses.FindAsync(id);
        //
        //     if (reserveStatus == null)
        //     {
        //         return NotFound();
        //     }
        //
        //     return reserveStatus;
        // }
        //
        // // PUT: api/ReserveStatus/5
        // // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        // [HttpPut("{id}")]
        // public async Task<IActionResult> PutReserveStatus(int id, ReserveStatus reserveStatus)
        // {
        //     if (id != reserveStatus.StatusId)
        //     {
        //         return BadRequest();
        //     }
        //
        //     _context.Entry(reserveStatus).State = EntityState.Modified;
        //
        //     try
        //     {
        //         await _context.SaveChangesAsync();
        //     }
        //     catch (DbUpdateConcurrencyException)
        //     {
        //         if (!ReserveStatusExists(id))
        //         {
        //             return NotFound();
        //         }
        //         else
        //         {
        //             throw;
        //         }
        //     }
        //
        //     return NoContent();
        // }
        //
        // // POST: api/ReserveStatus
        // // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        // [HttpPost]
        // public async Task<ActionResult<ReserveStatus>> PostReserveStatus(ReserveStatus reserveStatus)
        // {
        //   if (_context.ReserveStatuses == null)
        //   {
        //       return Problem("Entity set 'DBContext.ReserveStatuses'  is null.");
        //   }
        //     _context.ReserveStatuses.Add(reserveStatus);
        //     await _context.SaveChangesAsync();
        //
        //     return CreatedAtAction("GetReserveStatus", new { id = reserveStatus.StatusId }, reserveStatus);
        // }
        //
        // // DELETE: api/ReserveStatus/5
        // [HttpDelete("{id}")]
        // public async Task<IActionResult> DeleteReserveStatus(int id)
        // {
        //     if (_context.ReserveStatuses == null)
        //     {
        //         return NotFound();
        //     }
        //     var reserveStatus = await _context.ReserveStatuses.FindAsync(id);
        //     if (reserveStatus == null)
        //     {
        //         return NotFound();
        //     }
        //
        //     _context.ReserveStatuses.Remove(reserveStatus);
        //     await _context.SaveChangesAsync();
        //
        //     return NoContent();
        // }

        private bool ReserveStatusExists(int id)
        {
            return (_context.ReserveStatuses?.Any(e => e.StatusId == id)).GetValueOrDefault();
        }
    }
}
