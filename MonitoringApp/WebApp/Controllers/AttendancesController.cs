using Business.Exceptions;
using Business.Services;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using WebApp.Model.Attendance;
using WebApp.Model.DTO;
using WebApp.Notification;

namespace WebApp.Controllers;

public class AttendancesController(AttendanceService attendanceService, 
    IHubContext<NotificationHub, INotificationHub> hubContext, UserService userService) : Controller {
    [HttpGet("api/attendances")]
    [Authorize]
    public IActionResult GetUnfinishedAttendances() {
        if (HttpContext.User.Identity is not { IsAuthenticated: true }) {
            return Unauthorized();
        }
        
        IEnumerable<AttendanceDto> attendances = attendanceService
          .GetUnfinishedAttendances()
          .Select(AttendanceDto.FromAttendance); 
        
        return Ok(attendances);
    }
    
    [HttpPost("api/attendances")]
    [Authorize]
    public async Task<IActionResult> MarkAttendance([FromBody] MarkAttendanceRequest request) {
        if (HttpContext.User.Identity is not { IsAuthenticated: true }) {
            return Unauthorized();
        }
        
        try {
            // Get the user ID from the JWT token
            int userId = Convert.ToInt32(HttpContext.User.Claims.First(c => c.Type == "user_id").Value);
            
            // Mark the attendance
            Attendance attendance = attendanceService
                .RecordAttendance(userId, request.StartTime);

            // Notify all clients
            await hubContext.Clients.All.NotifyAttendance(AttendanceDto.FromAttendance(attendance));

            return Ok();
        }
        catch (FormatException e) {
            return StatusCode(450,"Invalid time format");
        }
        catch(NotFoundException e) {
            return StatusCode(404, e.Message);
        }
        catch(UserException e) {
            return StatusCode(450, e.Message);
        }
        catch (Exception e) {
            return BadRequest(e.Message);
        }
    }
}