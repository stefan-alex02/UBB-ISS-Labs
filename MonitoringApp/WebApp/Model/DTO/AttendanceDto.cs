namespace WebApp.Model.DTO;

public class AttendanceDto(int attendanceId, string username, string name, TimeOnly? startTime) {
    public int AttendanceId { get; set; } = attendanceId;
    public string Username { get; set; } = username;
    public string Name { get; set; } = name;
    public TimeOnly? StartTime { get; set; } = startTime;
    
    public static AttendanceDto FromAttendance(Domain.Attendance attendance) {
        return new AttendanceDto(attendance.Id, 
            attendance.MarkedBy.Username, 
            attendance.MarkedBy.Name, 
            attendance.Start);
    }
}