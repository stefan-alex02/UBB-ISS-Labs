using Business.Exceptions;
using Domain.Users;
using Persistence.UnitOfWork;

namespace Business.Services;

public class TaskService(IUnitOfWork unitOfWork) {
    public IEnumerable<Domain.Task> GetOngoingTasksFor(int employeeId) {
        return unitOfWork.TaskRepository
            .Find(task => task.AssignedTo.Id == employeeId && !task.IsComplete);
    }
    
    public Domain.Task UpdateTask(int taskId, string description, DateOnly assignedDate, 
        TimeOnly assignedTime) {
        Domain.Task? task = unitOfWork.TaskRepository.Get(taskId);
        
        if (task is null) {
            throw new NotFoundException("Task not found");
        }
        
        task.Description = description;
        task.AssignedDate = assignedDate;
        task.AssignedTime = assignedTime;
        
        unitOfWork.TaskRepository.Update(task);
        unitOfWork.SaveChanges();
        
        return task;
    }
    
    public void DeleteTask(int taskId) {
        Domain.Task? task = unitOfWork.TaskRepository.Get(taskId);
        
        if (task is null) {
            throw new NotFoundException("Task not found");
        }
        
        unitOfWork.TaskRepository.Remove(task);
        unitOfWork.SaveChanges();
    }
    
    public Domain.Task AssignTask(string createdByUsername, string assignedToUsername, 
        string description, DateOnly assignedDate, TimeOnly assignedTime) {
        User? createdBy = unitOfWork.UserRepository.GetByUsername(createdByUsername);
        if (createdBy is null) {
            throw new NotFoundException("User not found");
        }
        if (createdBy is not Manager createdByManager) { 
            throw new UnauthorizedException("Only managers can assign tasks");
        }
        
        User? assignedTo = unitOfWork.UserRepository.GetByUsername(assignedToUsername);
        if (assignedTo is null) {
            throw new NotFoundException("User not found");
        }
        if (assignedTo is not Employee assignedToEmployee) {
            throw new UnauthorizedException("Only employees can be assigned tasks");
        }
        
        Domain.Task task = new Domain.Task(0, description, false, assignedDate, assignedTime, 
            createdByManager, assignedToEmployee);
        unitOfWork.TaskRepository.Add(task);
        unitOfWork.SaveChanges();
        
        return task;
    }
    
    public void MarkTaskAsCompleted(int taskId) {
        Domain.Task? task = unitOfWork.TaskRepository.Get(taskId);
        
        if (task is null) {
            throw new NotFoundException("Task not found");
        }
        
        task.IsComplete = true;
        
        // No need to call unitOfWork.TaskRepository.Update(task) ?
        
        unitOfWork.SaveChanges();
    }
}