using Microsoft.AspNetCore.SignalR;

namespace WebApp.Notification;

public class NotificationHub : Hub {
    public async Task NotifyAttendance(string user, string message) {
        await Clients.All.SendAsync("ReceiveNotification", user, message);
    }
    
    public async Task NotifyLogout(string user) {
        await Clients.All.SendAsync("ReceiveNotification", user, "logged out", "");
    }
}