namespace WebApp.Model.Task;

public class UpdateTaskRequest {
    public String Description { get; set; }
    public DateOnly AssignedDate { get; set; }
    public TimeOnly AssignedTime { get; set; }
}