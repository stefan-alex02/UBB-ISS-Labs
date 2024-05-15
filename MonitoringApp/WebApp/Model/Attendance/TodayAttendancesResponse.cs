using WebApp.Model.DTO;

namespace WebApp.Model.Attendance;

public class TodayAttendancesResponse(AttendanceDto[] attendances) {
    public AttendanceDto[] Attendances { get; set; } = attendances;
}