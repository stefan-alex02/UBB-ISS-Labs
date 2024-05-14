using Business.Exceptions;
using Business.Services;
using Domain.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.Authentication;
using WebApp.Model;

namespace WebApp.Controllers;

public class UserController(UserService userService, JwtService jwtService) : Controller {
    [HttpPost("api/user/login")]
    public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest loginRequest) {
        try {
            User user = userService.Authenticate(loginRequest.Username, loginRequest.Password);
            var token = jwtService.GenerateToken(user.Id, user.Username, user.Name, user.UserRole);
            
            Console.WriteLine($"User {user.Username} logged in with ID {user.Id}");
            
            Response.Headers["Authorization"] = $"Bearer {token}";
            LoginResponse loginResponse = new LoginResponse {
                Token = token,
            };
            
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
        
        Console.WriteLine("User logging out");

        return Ok();
    }
    
    [HttpPost("api/user/register")]
    public async Task<ActionResult<HttpResponse>> Register([FromBody] RegisterRequest registerRequest) {
        try {
            userService.RegisterEmployee(registerRequest.Username, registerRequest.Password, registerRequest.Name);
            return Ok();
        }
        catch (AuthenticationException e) {
            return StatusCode(450, e.Message);
        }
        catch (Exception e) {
            return BadRequest();
        }
    }
}