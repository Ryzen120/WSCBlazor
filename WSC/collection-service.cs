using System.Collections.Generic;
using System.Threading.Tasks;
using WSCollector.Blazor.Models;

namespace WSCollector.Blazor.Services
{
    public interface ICollectionService
    {
        /// <summary>
        /// Adds a card to a user's collection
        /// </summary>
        Task<bool> AddToCollectionAsync(int cardId, string userId = null, string guestId = null, int quantity = 1);
        
        /// <summary>
        /// Removes a card from a user's collection
        /// </summary>
        Task<bool> RemoveFromCollectionAsync(int cardId, string userId = null, string guestId = null);
        
        /// <summary>
        /// Updates the quantity of a card in a user's collection
        /// </summary>
        Task<bool> UpdateQuantityAsync(int cardId, int quantity, string userId = null, string guestId = null);
        
        /// <summary>
        /// Gets all cards in a user's collection
        /// </summary>
        Task<(List<Card> Cards, int TotalCount)> GetCollectionAsync(string userId = null, string guestId = null, int page = 1, int pageSize = 20);
        
        /// <summary>
        /// Checks if a card is in a user's collection
        /// </summary>
        Task<bool> IsInCollectionAsync(int cardId, string userId = null, string guestId = null);
        
        /// <summary>
        /// Gets the quantity of a card in a user's collection
        /// </summary>
        Task<int> GetQuantityAsync(int cardId, string userId = null, string guestId = null);
        
        /// <summary>
        /// Gets filtered cards from a user's collection
        /// </summary>
        Task<(List<Card> Cards, int TotalCount)> FilterCollectionAsync(CardFilterOptions options, string userId = null, string guestId = null);
        
        /// <summary>
        /// Gets collection statistics
        /// </summary>
        Task<CollectionStatistics> GetStatisticsAsync(string userId = null, string guestId = null);
    }
    
    public class CollectionStatistics
    {
        public int TotalUniqueCards { get; set; }
        public int TotalCardQuantity { get; set; }
        public Dictionary<string, int> CardsByType { get; set; } = new Dictionary<string, int>();
        public Dictionary<string, int> CardsBySeries { get; set; } = new Dictionary<string, int>();
        public Dictionary<string, int> CardsByRarity { get; set; } = new Dictionary<string, int>();
    }
}