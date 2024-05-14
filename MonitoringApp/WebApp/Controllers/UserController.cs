using Business.Exceptions;
using Business.Services;
using Domain;
using Microsoft.AspNetCore.Mvc;
using WebApp.Model;

namespace WebApp.Controllers;

public class UserController(UserService userService) : Controller {
    private readonly UserService _userService = userService;

    [HttpPost("api/user/login")]
    public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest loginRequest) {
        try {
            User user = _userService.Authenticate(loginRequest.Username, loginRequest.Password);
            return Ok(user);
        }
        catch (AuthenticationException e) {
            return StatusCode(450, e.Message);
        }
        catch (Exception e) {
            return BadRequest();
        }
    }
}