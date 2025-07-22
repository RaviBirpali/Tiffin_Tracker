using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tiffin_Tracker.Models
{
    public class Payment
    {
        [Key]
        public int PaymentId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public DateTime PaymentPeriod { get; set; }  // Use YYYY-MM-01 for the month

        [Required]
        public decimal AmountPaid { get; set; }

        public DateTime PaymentDate { get; set; } = DateTime.Now;

        [StringLength(50)]
        public string PaymentMethod { get; set; } = "Cash";

        [StringLength(200)]
        public string Remarks { get; set; }

        // Navigation
        [ForeignKey("UserId")]
        public User? User { get; set; }
    }
}
