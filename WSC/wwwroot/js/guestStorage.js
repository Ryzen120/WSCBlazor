// Guest ID management for WSC application
window.guestStorage = {
    getGuestId: function () {
        const key = 'wsc_guest_id';
        let guestId = localStorage.getItem(key);
        if (!guestId) {
            guestId = 'guest-' + new Date().getTime() + '-' + Math.random().toString(36).substring(2, 15);
            localStorage.setItem(key, guestId);
        }
        return guestId;
    },

    setGuestId: function (guestId) {
        localStorage.setItem('wsc_guest_id', guestId);
    }
};