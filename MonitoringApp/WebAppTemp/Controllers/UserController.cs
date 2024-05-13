using Business.Exceptions;
using Business.Services;
using Domain;
using Microsoft.AspNetCore.Mvc;
using WebApp.Models;

namespace WebApp.Controllers;

public class UserController(UserService userService) : Controller {
    private readonly UserService _userService = userService;

    [HttpPost("api/user")]
    public async Task<ActionResult<User>> Login([FromBody] LoginModel loginModel) {
        try {
            User user = _userService.Authenticate(loginModel.Username, loginModel.Password);
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