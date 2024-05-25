using Business.Exceptions;
using Business.Services;
using Domain;
using Domain.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using WebApp.Authentication;
using WebApp.Model.DTO;
using WebApp.Model.User;
using WebApp.Notification;

namespace WebApp.Controllers;

public class UserController(UserService userService, AttendanceService attendanceService, JwtService jwtService, 
    IHubContext<NotificationHub, INotificationHub> hubContext) : Controller {
    [HttpPost("api/users/login")]
    public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest loginRequest) {
        try {
            User user = userService.Authenticate(loginRequest.Username, loginRequest.Password);
            var token = jwtService.GenerateToken(user.Id, user.Username, user.Name, user.UserRole);
            
            Console.WriteLine($"User {user.Username} logged in with ID {user.Id}");
            
            // Finished any old unfinished attendances if the user is an employee
            if (user.UserRole == UserRole.Employee) {
                try {
                    Attendance _ = attendanceService
                        .EndAttendanceOf(user.Id, TimeOnly.FromDateTime(DateTime.Now));

                    Console.WriteLine($"Ended unfinished attendance for user {user.Username}");
                }
                catch (NotFoundException) {
                    Console.WriteLine($"No unfinished attendance found for user {user.Username}");
                }
            }
            
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
    
    [HttpPost("api/users/logout")]
    [Authorize]
    public async Task<ActionResult<HttpResponse>> Logout() {
        if (HttpContext.User.Identity is not { IsAuthenticated: true }) {
            return Unauthorized();
        }
        // Check if the user is an employee
        UserRole userRole = (UserRole) Enum.Parse(typeof(UserRole), HttpContext.User.Claims
            .First(c => c.Type == "user_role").Value);
        
        if (userRole == UserRole.Employee) {
            try {
                // Get the user ID from the JWT token
                int userId = Convert.ToInt32(HttpContext.User.Claims.First(c => c.Type == "user_id").Value);
                
                // Mark the attendance as finished
                Attendance attendance = attendanceService
                    .EndAttendanceOf(userId, TimeOnly.FromDateTime(DateTime.Now));
                
                // Notify all clients
                Console.WriteLine("User logging out, sending notification...");
                await hubContext.Clients.All.NotifyLogout(AttendanceDto.FromAttendance(attendance));
            }
            catch (NotFoundException) {
                Console.WriteLine("No unfinished attendance found for user");
            }
        }
        
        return Ok();
    }
    
    [HttpPost("api/users/register")]
    public async Task<ActionResult<HttpResponse>> Register([FromBody] RegisterRequest registerRequest) {
        try {
            userService.RegisterEmployee(registerRequest.Username, registerRequest.Password, registerRequest.Name);
            return Ok();
        }
        catch (RegisterException e) {
            return StatusCode(450, e.Message);
        }
        catch (Exception e) {
            return BadRequest(e.Message);
        }
    }
}