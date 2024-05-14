using Business.Services;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using WebApp.Notification;

namespace WebApp.Controllers;

public class AttendancesController(AttendanceService attendanceService, IHubContext<NotificationHub> hubContext, UserService userService) : Controller {
    private readonly IHubContext<NotificationHub> _hubContext = hubContext;
    private readonly UserService _userService = userService;

    
    [HttpGet("api/attendance")]
    public IActionResult GetAttendancesOfToday() {
      var attendances = attendanceService.GetAttendancesOfToday();
      
      var attendancesDtos = attendances.Select(a => new {
        Id = a.Id,
        Start = a.Start,
        MarkedBy = a.MarkedBy.Name,
        MarkedById = a.MarkedById,
      }).ToArray();
      
      return Ok(attendancesDtos);
    }
    
    [HttpPost("api/attendance")]
    public IActionResult MarkAddendance([FromBody] AttendanceModel attendance) {
        try {
            attendanceService.RecordAttendance(new Attendance {
                Day = DateOnly.FromDateTime(DateTime.Today),
                Start = TimeOnly.Parse(attendance.Time),
                End = null,
                MarkedById = attendance.MarkedById,
            });

            // var managers = userService.GetAllManagers();
            // foreach (var manager in managers) {
            //     _hubContext.Clients.User(manager.Id.ToString()).SendAsync("ReceiveNotification", "New attendance", "A new attendance has been recorded");
            // }
            _hubContext.Clients.All.SendAsync("ReceiveNotification",
                "New attendance",
                "A new attendance has been recorded");

            return Ok();
        }
        catch (FormatException e) {
            return StatusCode(450,"Invalid time format");
        }
        catch (Exception e) {
            return BadRequest(e.Message);
        }
    }
}