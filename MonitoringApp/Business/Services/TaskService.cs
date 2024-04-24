using Persistence.UnitOfWork;

namespace Business.Services;

public class TaskService(IUnitOfWork unitOfWork) {
    
    public void AddTask(Domain.Task task) {
        unitOfWork.TaskRepository.Add(task);
        unitOfWork.SaveChanges();
    }
}