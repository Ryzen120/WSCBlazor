using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using WSC.Data;
using WSC.Models;

namespace WSC.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly ICollectionService _collectionService;

        public UserService(
            AppDbContext context,
            IPasswordHasher<User> passwordHasher,
            ICollectionService collectionService)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _collectionService = collectionService;
        }

        public async Task<(bool Success, string Message)> RegisterAsync(string username, string email, string password)
        {
            // Check if username is already taken
            if (await _context.Users.AnyAsync(u => u.Username == username))
            {
                return (false, "Username is already taken");
            }

            // Check if email is already taken
            if (await _context.Users.AnyAsync(u => u.Email == email))
            {
                return (false, "Email is already in use");
            }

            // Create new user
            var user = new User
            {
                Username = username,
                Email = email,
                CreatedAt = DateTime.UtcNow
            };

            // Hash password
            user.PasswordHash = _passwordHasher.HashPassword(user, password);

            // Save user
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return (true, "Registration successful");
        }

        public async Task<(bool Success, string Message, User User)> LoginAsync(string username, string password)
        {
            // Find user by username
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == username);

            if (user == null)
            {
                return (false, "Invalid username or password", null);
            }

            // Verify password
            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
            if (result != PasswordVerificationResult.Success)
            {
                return (false, "Invalid username or password", null);
            }

            // Update last login
            user.LastLogin = DateTime.UtcNow;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return (true, "Login successful", user);
        }

        public async Task<User> GetUserByIdAsync(string userId)
        {
            return await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<bool> UpdateUserAsync(User user)
        {
            // Check if user exists
            var existingUser = await _context.Users.FindAsync(user.Id);
            if (existingUser == null)
            {
                return false;
            }

            // Update only allowed fields (not password)
            existingUser.Username = user.Username;
            existingUser.Email = user.Email;

            _context.Users.Update(existingUser);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ChangePasswordAsync(string userId, string currentPassword, string newPassword)
        {
            // Find user
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                return false;
            }

            // Verify current password
            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, currentPassword);
            if (result != PasswordVerificationResult.Success)
            {
                return false;
            }

            // Update password
            user.PasswordHash = _passwordHasher.HashPassword(user, newPassword);
            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return true;
        }

        public string GenerateGuestId()
        {
            // Generate a unique guest ID
            return $"guest-{Guid.NewGuid()}";
        }

        public async Task<bool> MergeGuestCollectionAsync(string guestId, string userId)
        {
            // Get all guest collection items
            var guestItems = await _context.CollectionItems
                .Where(ci => ci.GuestId == guestId)
                .ToListAsync();

            // No items to merge
            if (!guestItems.Any())
            {
                return true;
            }

            // Get user's collection items
            var userItems = await _context.CollectionItems
                .Where(ci => ci.UserId == userId)
                .ToDictionaryAsync(ci => ci.CardId);

            // Start a transaction
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                foreach (var item in guestItems)
                {
                    if (userItems.TryGetValue(item.CardId, out var userItem))
                    {
                        // Card already in user's collection, update quantity
                        userItem.Quantity += item.Quantity;
                        _context.CollectionItems.Update(userItem);
                    }
                    else
                    {
                        // Add to user's collection
                        var newItem = new CollectionItem
                        {
                            CardId = item.CardId,
                            UserId = userId,
                            Quantity = item.Quantity,
                            AddedDate = DateTime.UtcNow,
                            Notes = item.Notes
                        };
                        _context.CollectionItems.Add(newItem);
                    }

                    // Remove from guest collection
                    _context.CollectionItems.Remove(item);
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                return false;
            }
        }
    }
}
