using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Tiffin_Tracker.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; }

        [Required, StringLength(100)]
        public string Username { get; set; }

        [Required, StringLength(256)]
        public string PasswordHash { get; set; }

        [Required, StringLength(20)]
        public string Role { get; set; }  // "Admin" or "User"

        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation
        public ICollection<TiffinEntry> TiffinEntries { get; set; }
        public ICollection<Payment> Payments { get; set; }
    }
}