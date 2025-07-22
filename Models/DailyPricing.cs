using System;
using System.ComponentModel.DataAnnotations;

namespace Tiffin_Tracker.Models
{
    public class DailyPricing
    {
        [Key]
        public int PriceId { get; set; }

        [Required]
        public DateTime PriceDate { get; set; }

        [Required]
        public decimal PricePerTiffin { get; set; }
    }
}
