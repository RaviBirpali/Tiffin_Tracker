using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tiffin_Tracker.Models
{
    public class TiffinEntry
    {
        [Key]
        public int EntryId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public DateTime EntryDate { get; set; }

        [Required]
        public int TiffinCount { get; set; } = 1;

        [StringLength(200)]
        public string Notes { get; set; }

        public bool IsHoliday { get; set; } = false;
        public bool IsSkipped { get; set; } = false;

        // Navigation
        [ForeignKey("UserId")]
        public User? User { get; set; }
    }
}