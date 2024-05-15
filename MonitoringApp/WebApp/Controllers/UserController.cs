using System.Security.Claims;
using Business.Exceptions;
using Business.Services;
using Domain;
using Domain.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using WebApp.Authentication;
using WebApp.Model;
using WebApp.Model.DTO;
using WebApp.Model.User;
using WebApp.Notification;

namespace WebApp.Controllers;

public class UserController(UserService userService, 
    AttendanceService attendanceService,
    JwtService jwtService, 
    IHubContext<NotificationHub, INotificationHub> hubContext) : Controller {
    [HttpPost("api/user/login")]
    public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest loginRequest) {
        try {
            User user = userService.Authenticate(loginRequest.Username, loginRequest.Password);
            var token = jwtService.GenerateToken(user.Id, user.Username, user.Name, user.UserRole);
            
            Console.WriteLine($"User {user.Username} logged in with ID {user.Id}");
            
            Response.Headers["Authorization"] = $"Bearer {token}";
            LoginResponse loginResponse = new LoginResponse(token);
            
            return Ok(loginResponse);
        }
        catch (AuthenticationException e) {
            return StatusCode(450, e.Message);
        }
        catch (Exception e) {
            return BadRequest();
        }
    }
    
    [HttpPost("api/user/logout")]
    [Authorize]
    public async Task<ActionResult<HttpResponse>> Logout() {
        if (HttpContext.User.Identity is not { IsAuthenticated: true }) {
            return Unauthorized();
        }
        
        // Get the user ID from the JWT token
        int userId = Convert.ToInt32(HttpContext.User.Claims.First(c => c.Type == "user_id").Value);
        
        // Mark the attendance as finished
        Attendance attendance = attendanceService.EndTodayAttendanceOf(userId, DateTime.Now);
        
        // Notify all clients
        Console.WriteLine("User logging out, sending notification...");
        await hubContext.Clients.All.NotifyLogout(AttendanceDto.FromAttendance(attendance));

        return Ok();
    }
    
    [HttpPost("api/user/register")]
    public async Task<ActionResult<HttpResponse>> Register([FromBody] RegisterRequest registerRequest) {
        try {
            userService.RegisterEmployee(registerRequest.Username, registerRequest.Password, registerRequest.Name);
            return Ok();
        }
        catch (RegisterException e) {
            return StatusCode(450, e.Message);
        }
        catch (Exception e) {
            return BadRequest();
        }
    }
}