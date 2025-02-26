using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WSC.Data;
using WSC.Models;

namespace WSC.Services
{
    public class CardService : ICardService
    {
        private readonly AppDbContext _context;
        private readonly SeriesMapper _seriesMapper;

        public CardService(AppDbContext context, SeriesMapper seriesMapper)
        {
            _context = context;
            _seriesMapper = seriesMapper;
        }

        public async Task<(List<Card> Cards, int TotalCount)> GetCardsAsync(int page = 1, int pageSize = 20)
        {
            var query = _context.Cards.AsNoTracking();
            
            var totalCount = await query.CountAsync();
            var cards = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
                
            // Map series names based on card numbers
            foreach (var card in cards)
            {
                var seriesCode = card.GetSeriesCode();
                card.Series = _seriesMapper.GetSeriesName(seriesCode);
            }
            
            return (cards, totalCount);
        }

        public async Task<Card> GetCardByIdAsync(int cardId)
        {
            var card = await _context.Cards
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.CardId == cardId);
                
            if (card != null)
            {
                var seriesCode = card.GetSeriesCode();
                card.Series = _seriesMapper.GetSeriesName(seriesCode);
            }
            
            return card;
        }

        public async Task<(List<Card> Cards, int TotalCount)> SearchCardsAsync(string searchText, int page = 1, int pageSize = 20)
        {
            if (string.IsNullOrWhiteSpace(searchText))
                return await GetCardsAsync(page, pageSize);
                
            var query = _context.Cards
                .AsNoTracking()
                .Where(c => 
                    c.Name.Contains(searchText) ||
                    c.CardNumber.Contains(searchText) ||
                    c.Expansion.Contains(searchText) ||
                    c.Effect.Contains(searchText) ||
                    c.Traits.Contains(searchText) ||
                    c.Flavor.Contains(searchText) ||
                    c.CardId.ToString() == searchText
                );
                
            var totalCount = await query.CountAsync();
            var cards = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
                
            // Map series names
            foreach (var card in cards)
            {
                var seriesCode = card.GetSeriesCode();
                card.Series = _seriesMapper.GetSeriesName(seriesCode);
            }
            
            return (cards, totalCount);
        }

        public async Task<(List<Card> Cards, int TotalCount)> FilterCardsAsync(CardFilterOptions options)
        {
            var query = _context.Cards.AsNoTracking();
            
            // Apply filters based on the options
            if (!string.IsNullOrEmpty(options.SearchText))
            {
                query = query.Where(c => 
                    c.Name.Contains(options.SearchText) ||
                    c.CardNumber.Contains(options.SearchText) ||
                    c.Expansion.Contains(options.SearchText) ||
                    c.Effect.Contains(options.SearchText) ||
                    c.Traits.Contains(options.SearchText) ||
                    c.Flavor.Contains(options.SearchText) ||
                    c.CardId.ToString() == options.SearchText
                );
            }
            
            if (options.CardId.HasValue)
            {
                query = query.Where(c => c.CardId == options.CardId.Value);
            }
            
            if (!string.IsNullOrEmpty(options.Series))
            {
                query = query.Where(c => c.CardNumber.StartsWith(options.Series + "/"));
            }
            
            if (!string.IsNullOrEmpty(options.CardType))
            {
                query = query.Where(c => c.CardType == options.CardType);
            }
            
            if (!string.IsNullOrEmpty(options.Rarity))
            {
                query = query.Where(c => c.Rarity == options.Rarity);
            }
            
            if (options.Level.HasValue)
            {
                query = query.Where(c => c.Level == options.Level.Value);
            }
            
            if (!string.IsNullOrEmpty(options.Soul))
            {
                query = query.Where(c => c.Soul == options.Soul);
            }
            
            if (options.Power.HasValue)
            {
                query = query.Where(c => c.Power == options.Power.Value);
            }
            
            if (!string.IsNullOrEmpty(options.Expansion))
            {
                query = query.Where(c => c.Expansion == options.Expansion);
            }
            
            if (!string.IsNullOrEmpty(options.Trigger))
            {
                query = query.Where(c => c.Trigger == options.Trigger);
            }
            
            if (!string.IsNullOrEmpty(options.Color))
            {
                query = query.Where(c => c.Color == options.Color);
            }
            
            // If filtering collection
            if (options.CollectionOnly)
            {
                var collectionQuery = _context.CollectionItems.AsQueryable();
                
                if (!string.IsNullOrEmpty(options.UserId))
                {
                    collectionQuery = collectionQuery.Where(ci => ci.UserId == options.UserId);
                }
                else if (!string.IsNullOrEmpty(options.GuestId))
                {
                    collectionQuery = collectionQuery.Where(ci => ci.GuestId == options.GuestId);
                }
                else
                {
                    // No user or guest specified - return empty
                    return (new List<Card>(), 0);
                }
                
                var cardIds = await collectionQuery.Select(ci => ci.CardId).ToListAsync();
                query = query.Where(c => cardIds.Contains(c.CardId));
            }
            
            var totalCount = await query.CountAsync();
            var cards = await query
                .Skip((options.Page - 1) * options.PageSize)
                .Take(options.PageSize)
                .ToListAsync();
                
            // Map series names
            foreach (var card in cards)
            {
                var seriesCode = card.GetSeriesCode();
                card.Series = _seriesMapper.GetSeriesName(seriesCode);
            }
            
            return (cards, totalCount);
        }

        public async Task<List<string>> GetExpansionsAsync()
        {
            return await _context.Cards
                .AsNoTracking()
                .Select(c => c.Expansion)
                .Distinct()
                .OrderBy(e => e)
                .ToListAsync();
        }

        public async Task<List<string>> GetCardTypesAsync()
        {
            return await _context.Cards
                .AsNoTracking()
                .Select(c => c.CardType)
                .Distinct()
                .OrderBy(t => t)
                .ToListAsync();
        }

        public async Task<List<string>> GetRaritiesAsync()
        {
            return await _context.Cards
                .AsNoTracking()
                .Select(c => c.Rarity)
                .Distinct()
                .OrderBy(r => r)
                .ToListAsync();
        }

        public async Task<List<int>> GetLevelsAsync()
        {
            return await _context.Cards
                .AsNoTracking()
                .Select(c => c.Level)
                .Distinct()
                .OrderBy(l => l)
                .ToListAsync();
        }

        public async Task<List<string>> GetSoulValuesAsync()
        {
            return await _context.Cards
                .AsNoTracking()
                .Select(c => c.Soul)
                .Distinct()
                .OrderBy(s => s)
                .ToListAsync();
        }

        public async Task<List<int>> GetPowerValuesAsync()
        {
            return await _context.Cards
                .AsNoTracking()
                .Select(c => c.Power)
                .Distinct()
                .OrderBy(p => p)
                .ToListAsync();
        }

        public async Task<List<string>> GetTriggersAsync()
        {
            return await _context.Cards
                .AsNoTracking()
                .Select(c => c.Trigger)
                .Distinct()
                .OrderBy(t => t)
                .ToListAsync();
        }

        public async Task<List<string>> GetColorsAsync()
        {
            return await _context.Cards
                .AsNoTracking()
                .Select(c => c.Color)
                .Distinct()
                .OrderBy(c => c)
                .ToListAsync();
        }

        public async Task<(List<Card> Cards, int TotalCount)> GetCardsBySeriesAsync(string seriesCode, int page = 1, int pageSize = 20)
        {
            if (string.IsNullOrEmpty(seriesCode))
                return await GetCardsAsync(page, pageSize);
                
            var query = _context.Cards
                .AsNoTracking()
                .Where(c => c.CardNumber.StartsWith(seriesCode + "/"));
                
            var totalCount = await query.CountAsync();
            var cards = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
                
            // Map series names
            foreach (var card in cards)
            {
                card.Series = _seriesMapper.GetSeriesName(seriesCode);
            }
            
            return (cards, totalCount);
        }
    }
}
