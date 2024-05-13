namespace WebApp.Models;

public class TaskModel {
    public string Description {get; set;}
    public int AssignedToId { get; set; }
    public int CreatedById { get; set; }
}