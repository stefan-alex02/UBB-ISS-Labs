using WebApp.Model.DTO;

namespace WebApp.Notification;

public interface INotificationHub {
    public Task NotifyAttendance(AttendanceDto attendance);
    public Task NotifyLogout(AttendanceDto attendance);
    public Task NotifyTask(TaskDto task);
    public Task NotifyTaskUpdate(TaskDto task);
    public Task NotifyTaskDeletion(int taskId);
}