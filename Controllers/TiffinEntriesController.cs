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
    [Route("api/[controller]")]
    [ApiController]
    public class TiffinEntriesController : ControllerBase
    {
        private readonly TiffinTrackerContext _context;

        public TiffinEntriesController(TiffinTrackerContext context)
        {
            _context = context;
        }

        // GET: api/TiffinEntries
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TiffinEntry>>> GetTiffinEntries()
        {
            return await _context.TiffinEntries.ToListAsync();
        }

        // GET: api/TiffinEntries/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TiffinEntry>> GetTiffinEntry(int id)
        {
            var tiffinEntry = await _context.TiffinEntries.FindAsync(id);

            if (tiffinEntry == null)
            {
                return NotFound();
            }

            return tiffinEntry;
        }

        // PUT: api/TiffinEntries/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTiffinEntry(int id, TiffinEntry tiffinEntry)
        {
            if (id != tiffinEntry.EntryId)
            {
                return BadRequest();
            }

            _context.Entry(tiffinEntry).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TiffinEntryExists(id))
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

        // POST: api/TiffinEntries
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TiffinEntry>> PostTiffinEntry(TiffinEntry tiffinEntry)
        {
            _context.TiffinEntries.Add(tiffinEntry);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTiffinEntry", new { id = tiffinEntry.EntryId }, tiffinEntry);
        }

        // DELETE: api/TiffinEntries/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTiffinEntry(int id)
        {
            var tiffinEntry = await _context.TiffinEntries.FindAsync(id);
            if (tiffinEntry == null)
            {
                return NotFound();
            }

            _context.TiffinEntries.Remove(tiffinEntry);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TiffinEntryExists(int id)
        {
            return _context.TiffinEntries.Any(e => e.EntryId == id);
        }


        [HttpGet("Calendar")]
        public async Task<ActionResult<IEnumerable<object>>> GetMonthlyCalendar(int userId, string month)
        {
            // Parse the month parameter
            if (!DateTime.TryParse($"{month}-01", out var firstDay))
                return BadRequest("Invalid month format. Use YYYY-MM");

            var lastDay = firstDay.AddMonths(1).AddDays(-1);

            // Fetch all entries for the month and user
            var entries = await _context.TiffinEntries
                .Where(te => te.UserId == userId &&
                             te.EntryDate >= firstDay &&
                             te.EntryDate <= lastDay)
                .OrderBy(te => te.EntryDate)
                .ToListAsync();

            // Create a daily calendar for the whole month (fill missing days)
            var daysInMonth = Enumerable.Range(0, (lastDay - firstDay).Days + 1)
                .Select(offset =>
                {
                    var date = firstDay.AddDays(offset);
                    var entry = entries.FirstOrDefault(e => e.EntryDate.Date == date.Date);
                    return new
                    {
                        Date = date.ToString("yyyy-MM-dd"),
                        TiffinCount = entry?.TiffinCount ?? 0,
                        IsHoliday = entry?.IsHoliday ?? false,
                        IsSkipped = entry?.IsSkipped ?? false,
                        Notes = entry?.Notes ?? ""
                    };
                });

            return Ok(daysInMonth);
        }


        [HttpPatch("MarkDay")]
        public async Task<IActionResult> MarkDay([FromBody] MarkDayDto dto)
        {
            var entry = await _context.TiffinEntries
                .FirstOrDefaultAsync(e => e.UserId == dto.UserId && e.EntryDate.Date == dto.Date.Date);

            if (entry == null)
            {
                entry = new TiffinEntry
                {
                    UserId = dto.UserId,
                    EntryDate = dto.Date,
                    IsSkipped = dto.IsSkipped,
                    IsHoliday = dto.IsHoliday,
                    Notes = dto.Notes,
                    TiffinCount = dto.TiffinCount ?? 0
                };
                _context.TiffinEntries.Add(entry);
            }
            else
            {
                entry.IsSkipped = dto.IsSkipped;
                entry.IsHoliday = dto.IsHoliday;
                entry.Notes = dto.Notes;
                if (dto.TiffinCount.HasValue)
                    entry.TiffinCount = dto.TiffinCount.Value;
            }

            await _context.SaveChangesAsync();
            return Ok(entry);
        }

        public class MarkDayDto
        {
            public int UserId { get; set; }
            public DateTime Date { get; set; }
            public bool IsSkipped { get; set; }
            public bool IsHoliday { get; set; }
            public string Notes { get; set; }
            public int? TiffinCount { get; set; }
        }


    }
}
