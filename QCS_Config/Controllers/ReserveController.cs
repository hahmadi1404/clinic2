using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Globals;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PersianDate.Standard;
using QCS_Config.Models;

namespace QCS_Config.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReserveController : ControllerBase
    {
        private readonly DBContext _context;

        public ReserveController(DBContext context)
        {
            _context = context;
        }
        private int getClinicId()
        {
            return Convert.ToInt32(User.Claims.First(a => a.Type.ToLower().Contains("role")).Value);
        }

        // GET: api/Reserve
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Reserve>>> GetReserves()
        {
            int cId = getClinicId();
          if (_context.Reserves == null)
          {
              return NotFound();
          }
            return await _context.Reserves.Where(a=>a.ClinicId==cId).ToListAsync();
        }
     
        // GET: api/Reserve/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Reserve>> GetReserve(int id)
        {
          if (_context.Reserves == null)
          {
              return NotFound();
          }
            var reserve = await _context.Reserves.FindAsync(id);

            if (reserve == null)
            {
                return NotFound();
            }

            return reserve;
        }

        // PUT: api/Reserve/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutReserve(int id, Reserve reserve)
        {
            if (id != reserve.ReserveId)
            {
                return BadRequest();
            }

            _context.Entry(reserve).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReserveExists(id))
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

        // POST: api/Reserve
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Reserve>> PostReserve(Reserve reserve)
        {
            try
            {

           
            reserve.ClinicId=getClinicId();
            reserve.ReserveDate = reserve.ReserveDatePersian.ToEn();
            reserve.CreateDate = DateTime.Now;
            reserve.CreateDatePersian = DateTime.Now.ToFa();
            var receptionCount = GlobalDB.ReceptionCount(reserve.ShiftId??-1, reserve.InsuranceId??-1);
            var date = DateTime.Now;
            if (GlobalDB.ReserveCount(date, reserve.ShiftId ?? -1, reserve.InsuranceId ?? -1) == receptionCount)
            {
                return ValidationProblem("در روز و شیفت انتخابی امکان ثبت نداریم ، ظرفیت پر شده");
            }

                if (_context.Reserves == null)
          {
              return Problem("Entity set 'DBContext.Reserves'  is null.");
          }
            _context.Reserves.Add(reserve);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ReserveExists(reserve.ReserveId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }
            }
            catch (Exception)
            {

                return Forbid("در روز و شیفت انتخابی امکان ثبت نداریم ، ظرفیت پر شده");
            }

            return CreatedAtAction("GetReserve", new { id = reserve.ReserveId }, reserve);
        }

        // DELETE: api/Reserve/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReserve(int id)
        {
            if (_context.Reserves == null)
            {
                return NotFound();
            }
            var reserve = await _context.Reserves.FindAsync(id);
            if (reserve == null)
            {
                return NotFound();
            }

            _context.Reserves.Remove(reserve);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ReserveExists(int id)
        {
            return (_context.Reserves?.Any(e => e.ReserveId == id)).GetValueOrDefault();
        }
    }
}
