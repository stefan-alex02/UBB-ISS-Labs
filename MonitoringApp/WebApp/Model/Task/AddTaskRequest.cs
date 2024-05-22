namespace WebApp.Model.Task;

public class AddTaskRequest {
    public string CreatedByUsername { get; set; }
    public string AssignedToUsername { get; set; }
    public String Description { get; set; }
    public DateOnly AssignedDate { get; set; }
    public TimeOnly AssignedTime { get; set; }
}