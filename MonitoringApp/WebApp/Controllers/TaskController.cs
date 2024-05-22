using Business.Exceptions;
using Business.Services;
using Domain.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using WebApp.Model.DTO;
using WebApp.Model.Task;
using WebApp.Notification;

namespace WebApp.Controllers;

public class TaskController(TaskService taskService, 
    IHubContext<NotificationHub, INotificationHub> hubContext) : Controller {
    
    [HttpPost("api/tasks")]
    [Authorize]
    public async Task<IActionResult> AddTask([FromBody] AddTaskRequest request) {
        if (HttpContext.User.Identity is not { IsAuthenticated: true }) {
            return Unauthorized();
        }

        try {
            UserRole role = (UserRole)Enum.Parse(typeof(UserRole), HttpContext.User.Claims
                .First(c => c.Type == "user_role").Value);

            if (role != UserRole.Manager) {
                return StatusCode(403, "Only managers can add tasks");
            }

            Domain.Task createdTask = taskService.AssignTask(request.CreatedByUsername, request.AssignedToUsername,
                request.Description, request.AssignedDate, request.AssignedTime);

            await hubContext.Clients.All.NotifyTask(TaskDto.FromTask(createdTask));

            return Ok();
        }
        catch (NotFoundException e) {
            return NotFound(e.Message);
        }
        catch (UnauthorizedException e) {
            return StatusCode(403, e.Message);
        }
        catch (Exception e) {
            return StatusCode(500, e.Message);
        }
    }
    
    [HttpGet("api/tasks/{employeeId}")]
    [Authorize]
    public IActionResult GetTasksForEmployee(int employeeId) {
        if (HttpContext.User.Identity is not { IsAuthenticated: true }) {
            return Unauthorized();
        }

        try {
            UserRole role = (UserRole)Enum.Parse(typeof(UserRole), HttpContext.User.Claims
                .First(c => c.Type == "user_role").Value);

            if (role != UserRole.Manager && role != UserRole.Employee) {
                return StatusCode(403, "Only managers and employees can view tasks");
            }

            IEnumerable<Domain.Task> tasks = taskService.GetOngoingTasksFor(employeeId);

            return Ok(tasks.Select(TaskDto.FromTask));
        }
        catch (NotFoundException e) {
            return NotFound(e.Message);
        }
        catch (Exception e) {
            return StatusCode(500, e.Message);
        }
    }
    
    [HttpPut("api/tasks/{taskId}")]
    [Authorize]
    public async Task<IActionResult> UpdateTask(int taskId, [FromBody] UpdateTaskRequest request) {
        if (HttpContext.User.Identity is not { IsAuthenticated: true }) {
            return Unauthorized();
        }

        try {
            UserRole role = (UserRole)Enum.Parse(typeof(UserRole), HttpContext.User.Claims
                .First(c => c.Type == "user_role").Value);

            if (role != UserRole.Manager) {
                return StatusCode(403, "Only managers can update tasks");
            }

            Domain.Task updatedTask = taskService.UpdateTask(taskId, request.Description, 
                request.AssignedDate, request.AssignedTime);
            
            await hubContext.Clients.All.NotifyTaskUpdate(TaskDto.FromTask(updatedTask));

            return Ok();
        }
        catch (NotFoundException e) {
            return NotFound(e.Message);
        }
        catch (Exception e) {
            return StatusCode(500, e.Message);
        }
    }
    
    [HttpDelete("api/tasks/{taskId}")]
    [Authorize]
    public async Task<IActionResult> DeleteTask(int taskId) {
        if (HttpContext.User.Identity is not { IsAuthenticated: true }) {
            return Unauthorized();
        }

        try {
            UserRole role = (UserRole)Enum.Parse(typeof(UserRole), HttpContext.User.Claims
                .First(c => c.Type == "user_role").Value);

            if (role != UserRole.Manager) {
                return StatusCode(403, "Only managers can delete tasks");
            }

            taskService.DeleteTask(taskId);
            
            await hubContext.Clients.All.NotifyTaskDeletion(taskId);

            return Ok();
        }
        catch (NotFoundException e) {
            return NotFound(e.Message);
        }
        catch (Exception e) {
            return StatusCode(500, e.Message);
        }
    }
    
    
    [HttpPost("api/tasks/{taskId}/complete")]
    [Authorize]
    public IActionResult MarkTaskAsCompleted(int taskId) {
        if (HttpContext.User.Identity is not { IsAuthenticated: true }) {
            return Unauthorized();
        }

        try {
            UserRole role = (UserRole)Enum.Parse(typeof(UserRole), HttpContext.User.Claims
                .First(c => c.Type == "user_role").Value);

            if (role != UserRole.Employee) {
                return StatusCode(403, "Only employees can mark tasks as completed");
            }

            taskService.MarkTaskAsCompleted(taskId);

            return Ok();
        }
        catch (NotFoundException e) {
            return NotFound(e.Message);
        }
        catch (Exception e) {
            return StatusCode(500, e.Message);
        }
    }
}