using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tiffin_Tracker.Data;
using Tiffin_Tracker.Models;

namespace Tiffin_Tracker.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DailyPricingController : ControllerBase
    {
        private readonly TiffinTrackerContext _context;

        public DailyPricingController(TiffinTrackerContext context)
        {
            _context = context;
        }

        // GET: api/DailyPricing
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DailyPricing>>> GetDailyPricings()
        {
            return await _context.DailyPricings.ToListAsync();
        }

        // GET: api/DailyPricing/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DailyPricing>> GetDailyPricing(int id)
        {
            var dailyPricing = await _context.DailyPricings.FindAsync(id);

            if (dailyPricing == null)
            {
                return NotFound();
            }

            return dailyPricing;
        }

        // PUT: api/DailyPricing/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDailyPricing(int id, DailyPricing dailyPricing)
        {
            if (id != dailyPricing.PriceId)
            {
                return BadRequest();
            }

            _context.Entry(dailyPricing).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DailyPricingExists(id))
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

        // POST: api/DailyPricing
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<DailyPricing>> PostDailyPricing(DailyPricing dailyPricing)
        {
            _context.DailyPricings.Add(dailyPricing);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDailyPricing", new { id = dailyPricing.PriceId }, dailyPricing);
        }

        // DELETE: api/DailyPricing/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDailyPricing(int id)
        {
            var dailyPricing = await _context.DailyPricings.FindAsync(id);
            if (dailyPricing == null)
            {
                return NotFound();
            }

            _context.DailyPricings.Remove(dailyPricing);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DailyPricingExists(int id)
        {
            return _context.DailyPricings.Any(e => e.PriceId == id);
        }


        [HttpGet("current")]
        public IActionResult GetCurrentPricing()
        {
            var current = _context.DailyPricings
                .OrderByDescending(p => p.PriceDate)
                .FirstOrDefault();

            if (current == null)
                return BadRequest("No pricing record found.");

            return Ok(current);
        }

    }
}
