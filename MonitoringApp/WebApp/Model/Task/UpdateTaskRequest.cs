namespace WebApp.Model.Task;

public class UpdateTaskRequest {
    public String Description { get; set; }
    public string AssignedDate { get; set; }
    public string AssignedTime { get; set; }
}