using Business.Exceptions;
using Persistence.UnitOfWork;

namespace Business.Services;

public class TaskService(IUnitOfWork unitOfWork) {
    public IEnumerable<Domain.Task> GetOngoingTasksOf(int userId) {
        return unitOfWork.TaskRepository
            .Find(task => task.AssignedTo.Id == userId && !task.IsComplete);
    }
    
    public void AssignTask(Domain.Task task) {
        unitOfWork.TaskRepository.Add(task);
        unitOfWork.SaveChanges();
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