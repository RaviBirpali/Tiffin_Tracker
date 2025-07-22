using Microsoft.EntityFrameworkCore;
using Tiffin_Tracker.Models;

namespace Tiffin_Tracker.Data
{
    public class TiffinTrackerContext : DbContext
    {
        public TiffinTrackerContext(DbContextOptions<TiffinTrackerContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<TiffinEntry> TiffinEntries { get; set; }
        public DbSet<DailyPricing> DailyPricings { get; set; }
        public DbSet<Payment> Payments { get; set; }
    }
}
