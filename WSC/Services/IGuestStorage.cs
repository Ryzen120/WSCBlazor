using System.Threading.Tasks;

namespace WSC.Services
{
    public interface IGuestStorage
    {
        Task<string> GetGuestIdAsync();
        Task SaveGuestIdAsync(string guestId);
    }
}