namespace WebApp.Model.DTO;

public class TaskDto(int id, string description, DateOnly assignedDate, TimeOnly assignedTime) {
    public int Id { get; set; } = id;
    public String Description { get; set; } = description;
    public DateOnly AssignedDate { get; set; } = assignedDate;
    public TimeOnly AssignedTime { get; set; } = assignedTime;

    public static TaskDto FromTask(Domain.Task task) {
        return new TaskDto(task.Id, task.Description, task.AssignedDate, task.AssignedTime);
    }
}