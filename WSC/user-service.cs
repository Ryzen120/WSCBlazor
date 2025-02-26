using System.Threading.Tasks;
using WSCollector.Blazor.Models;

namespace WSCollector.Blazor.Services
{
    public interface IUserService
    {
        /// <summary>
        /// Registers a new user
        /// </summary>
        Task<(bool Success, string Message)> RegisterAsync(string username, string email, string password);
        
        /// <summary>
        /// Logs in a user
        /// </summary>
        Task<(bool Success, string Message, User User)> LoginAsync(string username, string password);
        
        /// <summary>
        /// Gets a user by their ID
        /// </summary>
        Task<User> GetUserByIdAsync(string userId);
        
        /// <summary>
        /// Updates a user's profile
        /// </summary>
        Task<bool> UpdateUserAsync(User user);
        
        /// <summary>
        /// Changes a user's password
        /// </summary>
        Task<bool> ChangePasswordAsync(string userId, string currentPassword, string newPassword);
        
        /// <summary>
        /// Generates a guest ID for anonymous collections
        /// </summary>
        string GenerateGuestId();
        
        /// <summary>
        /// Merges a guest collection into a user's collection
        /// </summary>
        Task<bool> MergeGuestCollectionAsync(string guestId, string userId);
    }
}