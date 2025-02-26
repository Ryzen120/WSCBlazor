namespace WSC.Models
{
    public class CardFilterOptions
    {
        // General search text that could match any field
        public string SearchText { get; set; }
        
        // Filter by specific card ID
        public int? CardId { get; set; }
        
        // Filter by series (e.g., "SAO", "FZ")
        public string Series { get; set; }
        
        // Filter by card type (Character, Climax, Event, etc)
        public string CardType { get; set; }
        
        // Filter by rarity (SP, RR, R, C, etc)
        public string Rarity { get; set; }
        
        // Filter by card level (0, 1, 2, 3)
        public int? Level { get; set; }
        
        // Filter by soul count
        public string Soul { get; set; }
        
        // Filter by card power
        public int? Power { get; set; }
        
        // Filter by expansion name
        public string Expansion { get; set; }
        
        // Filter by trigger type
        public string Trigger { get; set; }
        
        // Filter by card color
        public string Color { get; set; }
        
        // Apply to collection only
        public bool CollectionOnly { get; set; }
        
        // User ID for collection filtering
        public string UserId { get; set; }
        
        // Guest ID for collection filtering
        public string GuestId { get; set; }
        
        // Pagination properties
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}
