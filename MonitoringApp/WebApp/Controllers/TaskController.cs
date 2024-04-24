using Business.Services;
using Microsoft.AspNetCore.Mvc;
using WebApp.Models;
using Task = Domain.Task;

namespace WebApp.Controllers;

public class TaskController(TaskService taskService) : Controller {
    
    [HttpPost("api/task")]
    public IActionResult AddTask([FromBody] TaskModel task) {
        // if (HttpContext.Request.Cookies.TryGetValue("user", out string managerIdString))
        // {
        //     if (int.TryParse(managerIdString, out int managerId))
        //     {
                taskService.AddTask(new Task
                {
                    Description = task.Description,
                    IsComplete = false,
                    AssignedDate = DateOnly.FromDateTime(DateTime.Today),
                    AssignedTime = TimeOnly.FromDateTime(DateTime.Now),
                    AssignedToId = task.AssignedToId,
                    CreatedById = task.CreatedById,
                });
                return Ok();
        //     }
        //     else
        //     {
        //         return BadRequest("Invalid ManagerId in cookie");
        //     }
        // }
        // else
        // {
        //     return BadRequest("ManagerId not found in cookie");
        // }
    }
}