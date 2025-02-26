using System.Collections.Generic;
using System.Threading.Tasks;
using WSC.Models;

namespace WSC.Services
{
    public interface ICardService
    {
        /// <summary>
        /// Gets a paginated list of cards
        /// </summary>
        Task<(List<Card> Cards, int TotalCount)> GetCardsAsync(int page = 1, int pageSize = 20);
        
        /// <summary>
        /// Gets a card by its ID
        /// </summary>
        Task<Card> GetCardByIdAsync(int cardId);
        
        /// <summary>
        /// Searches for cards based on search text across multiple fields
        /// </summary>
        Task<(List<Card> Cards, int TotalCount)> SearchCardsAsync(string searchText, int page = 1, int pageSize = 20);
        
        /// <summary>
        /// Filters cards based on multiple criteria
        /// </summary>
        Task<(List<Card> Cards, int TotalCount)> FilterCardsAsync(CardFilterOptions options);
        
        /// <summary>
        /// Gets a list of all available expansions
        /// </summary>
        Task<List<string>> GetExpansionsAsync();
        
        /// <summary>
        /// Gets a list of all available card types
        /// </summary>
        Task<List<string>> GetCardTypesAsync();
        
        /// <summary>
        /// Gets a list of all available rarities
        /// </summary>
        Task<List<string>> GetRaritiesAsync();
        
        /// <summary>
        /// Gets a list of all available levels
        /// </summary>
        Task<List<int>> GetLevelsAsync();
        
        /// <summary>
        /// Gets a list of all available soul values
        /// </summary>
        Task<List<string>> GetSoulValuesAsync();
        
        /// <summary>
        /// Gets a list of all available power values
        /// </summary>
        Task<List<int>> GetPowerValuesAsync();
        
        /// <summary>
        /// Gets a list of all available triggers
        /// </summary>
        Task<List<string>> GetTriggersAsync();
        
        /// <summary>
        /// Gets a list of all available colors
        /// </summary>
        Task<List<string>> GetColorsAsync();
        
        /// <summary>
        /// Gets cards by series code
        /// </summary>
        Task<(List<Card> Cards, int TotalCount)> GetCardsBySeriesAsync(string seriesCode, int page = 1, int pageSize = 20);
    }
}
