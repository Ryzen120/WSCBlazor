using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WSC.Models
{
    public class User
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        
        [Required, MaxLength(50)]
        public string Username { get; set; }
        
        [Required, EmailAddress]
        public string Email { get; set; }
        
        [Required]
        public string PasswordHash { get; set; }
        
        // When the user account was created
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        // Last login time
        public DateTime? LastLogin { get; set; }
        
        // Navigation property for user's collection
        public virtual ICollection<CollectionItem> Collection { get; set; }
    }
}
