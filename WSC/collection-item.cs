using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WSC.Models
{
    public class CollectionItem
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public int CardId { get; set; }
        
        // Foreign key to the Card entity
        [ForeignKey("CardId")]
        public WSC.Models.Card Card { get; set; }
        
        // User ID, could be null for guest collections
        public string UserId { get; set; }
        
        // Guest identifier for anonymous collections
        public string GuestId { get; set; }
        
        // Quantity of this card owned
        public int Quantity { get; set; } = 1;
        
        // When the card was added to the collection
        public DateTime AddedDate { get; set; } = DateTime.UtcNow;
        
        // Optional notes about this specific card
        public string Notes { get; set; }
    }
}
