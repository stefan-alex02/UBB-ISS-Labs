using WebApp.Model.DTO;

namespace WebApp.Notification;

public interface INotificationHub {
    public Task NotifyAttendance(AttendanceDto attendance);
    public Task NotifyLogout(AttendanceDto attendance);
}