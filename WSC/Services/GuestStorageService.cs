using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace WSC.Services
{
    public class GuestStorageService : IGuestStorage
    {
        private readonly IJSRuntime _jsRuntime;

        public GuestStorageService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async Task<string> GetGuestIdAsync()
        {
            try
            {
                // Use the JavaScript method we created
                return await _jsRuntime.InvokeAsync<string>("guestStorage.getGuestId");
            }
            catch (Exception)
            {
                // Fallback if JS interop fails
                return "guest-" + Guid.NewGuid().ToString();
            }
        }

        public async Task SaveGuestIdAsync(string guestId)
        {
            try
            {
                await _jsRuntime.InvokeVoidAsync("guestStorage.setGuestId", guestId);
            }
            catch (Exception)
            {
                // Silently fail if JS interop doesn't work
            }
        }
    }
}