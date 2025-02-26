using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WSC.Data;
using WSC.Models;

namespace WSC.Services
{
    public class CollectionService : ICollectionService
    {
        private readonly AppDbContext _context;
        private readonly SeriesMapper _seriesMapper;

        public CollectionService(AppDbContext context, SeriesMapper seriesMapper)
        {
            _context = context;
            _seriesMapper = seriesMapper;
        }

        public async Task<bool> AddToCollectionAsync(int cardId, string userId = null, string guestId = null, int quantity = 1)
        {
            // Check if either userId or guestId is provided
            if (string.IsNullOrEmpty(userId) && string.IsNullOrEmpty(guestId))
            {
                throw new ArgumentException("Either userId or guestId must be provided");
            }

            // Check if the card exists
            var cardExists = await _context.Cards.AnyAsync(c => c.CardId == cardId);
            if (!cardExists)
            {
                return false;
            }

            // Check if the card is already in the collection
            var existingItem = await _context.CollectionItems
                .FirstOrDefaultAsync(ci =>
                    ci.CardId == cardId &&
                    (ci.UserId == userId || ci.GuestId == guestId));

            if (existingItem != null)
            {
                // Update quantity
                existingItem.Quantity += quantity;
                _context.CollectionItems.Update(existingItem);
            }
            else
            {
                // Add new collection item
                var newItem = new CollectionItem
                {
                    CardId = cardId,
                    UserId = userId,
                    GuestId = guestId,
                    Quantity = quantity,
                    AddedDate = DateTime.UtcNow
                };
                _context.CollectionItems.Add(newItem);
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveFromCollectionAsync(int cardId, string userId = null, string guestId = null)
        {
            // Check if either userId or guestId is provided
            if (string.IsNullOrEmpty(userId) && string.IsNullOrEmpty(guestId))
            {
                throw new ArgumentException("Either userId or guestId must be provided");
            }

            var item = await _context.CollectionItems
                .FirstOrDefaultAsync(ci =>
                    ci.CardId == cardId &&
                    (ci.UserId == userId || ci.GuestId == guestId));

            if (item == null)
            {
                return false;
            }

            _context.CollectionItems.Remove(item);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateQuantityAsync(int cardId, int quantity, string userId = null, string guestId = null)
        {
            // Check if either userId or guestId is provided
            if (string.IsNullOrEmpty(userId) && string.IsNullOrEmpty(guestId))
            {
                throw new ArgumentException("Either userId or guestId must be provided");
            }

            var item = await _context.CollectionItems
                .FirstOrDefaultAsync(ci =>
                    ci.CardId == cardId &&
                    (ci.UserId == userId || ci.GuestId == guestId));

            if (item == null)
            {
                return false;
            }

            if (quantity <= 0)
            {
                // Remove if quantity is 0 or negative
                _context.CollectionItems.Remove(item);
            }
            else
            {
                // Update quantity
                item.Quantity = quantity;
                _context.CollectionItems.Update(item);
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<(List<Card> Cards, int TotalCount)> GetCollectionAsync(string userId = null, string guestId = null, int page = 1, int pageSize = 20)
        {
            // Check if either userId or guestId is provided
            if (string.IsNullOrEmpty(userId) && string.IsNullOrEmpty(guestId))
            {
                throw new ArgumentException("Either userId or guestId must be provided");
            }

            // Get all card IDs in the collection
            var query = _context.CollectionItems
                .AsNoTracking()
                .Where(ci => ci.UserId == userId || ci.GuestId == guestId);

            var totalCount = await query.CountAsync();
            
            var collectionItems = await query
                .OrderByDescending(ci => ci.AddedDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // Get all cards in one query
            var cardIds = collectionItems.Select(ci => ci.CardId).ToList();
            var cards = await _context.Cards
                .AsNoTracking()
                .Where(c => cardIds.Contains(c.CardId))
                .ToListAsync();

            // Map series names and organize by the original order
            var orderedCards = new List<Card>();
            foreach (var item in collectionItems)
            {
                var card = cards.FirstOrDefault(c => c.CardId == item.CardId);
                if (card != null)
                {
                    var seriesCode = card.GetSeriesCode();
                    card.Series = _seriesMapper.GetSeriesName(seriesCode);
                    orderedCards.Add(card);
                }
            }

            return (orderedCards, totalCount);
        }

        public async Task<bool> IsInCollectionAsync(int cardId, string userId = null, string guestId = null)
        {
            // Check if either userId or guestId is provided
            if (string.IsNullOrEmpty(userId) && string.IsNullOrEmpty(guestId))
            {
                throw new ArgumentException("Either userId or guestId must be provided");
            }

            return await _context.CollectionItems
                .AnyAsync(ci =>
                    ci.CardId == cardId &&
                    (ci.UserId == userId || ci.GuestId == guestId));
        }

        public async Task<int> GetQuantityAsync(int cardId, string userId = null, string guestId = null)
        {
            // Check if either userId or guestId is provided
            if (string.IsNullOrEmpty(userId) && string.IsNullOrEmpty(guestId))
            {
                throw new ArgumentException("Either userId or guestId must be provided");
            }

            var item = await _context.CollectionItems
                .AsNoTracking()
                .FirstOrDefaultAsync(ci =>
                    ci.CardId == cardId &&
                    (ci.UserId == userId || ci.GuestId == guestId));

            return item?.Quantity ?? 0;
        }

        public async Task<(List<Card> Cards, int TotalCount)> FilterCollectionAsync(CardFilterOptions options, string userId = null, string guestId = null)
        {
            // Check if either userId or guestId is provided
            if (string.IsNullOrEmpty(userId) && string.IsNullOrEmpty(guestId))
            {
                throw new ArgumentException("Either userId or guestId must be provided");
            }

            // Override CollectionOnly flag
            options.CollectionOnly = true;
            options.UserId = userId;
            options.GuestId = guestId;

            // Use CardService's FilterCardsAsync method
            var cardService = new CardService(_context, _seriesMapper);
            return await cardService.FilterCardsAsync(options);
        }

        public async Task<CollectionStatistics> GetStatisticsAsync(string userId = null, string guestId = null)
        {
            // Check if either userId or guestId is provided
            if (string.IsNullOrEmpty(userId) && string.IsNullOrEmpty(guestId))
            {
                throw new ArgumentException("Either userId or guestId must be provided");
            }

            // Get all collection items
            var collectionItems = await _context.CollectionItems
                .AsNoTracking()
                .Where(ci => ci.UserId == userId || ci.GuestId == guestId)
                .ToListAsync();

            // Calculate total card quantity
            var totalQuantity = collectionItems.Sum(ci => ci.Quantity);

            // Get all unique card IDs
            var cardIds = collectionItems.Select(ci => ci.CardId).Distinct().ToList();

            // Get all cards
            var cards = await _context.Cards
                .AsNoTracking()
                .Where(c => cardIds.Contains(c.CardId))
                .ToListAsync();

            // Calculate statistics
            var cardsByType = new Dictionary<string, int>();
            var cardsBySeries = new Dictionary<string, int>();
            var cardsByRarity = new Dictionary<string, int>();

            foreach (var card in cards)
            {
                // Card type statistics
                if (!string.IsNullOrEmpty(card.CardType))
                {
                    if (cardsByType.ContainsKey(card.CardType))
                        cardsByType[card.CardType]++;
                    else
                        cardsByType[card.CardType] = 1;
                }

                // Series statistics
                var seriesCode = card.GetSeriesCode();
                var seriesName = _seriesMapper.GetSeriesName(seriesCode);
                if (!string.IsNullOrEmpty(seriesName))
                {
                    if (cardsBySeries.ContainsKey(seriesName))
                        cardsBySeries[seriesName]++;
                    else
                        cardsBySeries[seriesName] = 1;
                }

                // Rarity statistics
                if (!string.IsNullOrEmpty(card.Rarity))
                {
                    if (cardsByRarity.ContainsKey(card.Rarity))
                        cardsByRarity[card.Rarity]++;
                    else
                        cardsByRarity[card.Rarity] = 1;
                }
            }

            return new CollectionStatistics
            {
                TotalUniqueCards = cards.Count,
                TotalCardQuantity = totalQuantity,
                CardsByType = cardsByType,
                CardsBySeries = cardsBySeries,
                CardsByRarity = cardsByRarity
            };
        }
    }
}
