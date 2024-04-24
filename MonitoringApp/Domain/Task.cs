using System.ComponentModel.DataAnnotations.Schema;

namespace Domain;

public class Task : Entity<int> {
    public string Description { get; set; }
    public bool IsComplete { get; set; }
    public DateOnly AssignedDate { get; set; }
    public TimeOnly AssignedTime { get; set; }

    [ForeignKey("CreatedBy")]
    public int CreatedById { get; set; }

    public Manager CreatedBy { get; set; }

    [ForeignKey("AssignedTo")]
    public int AssignedToId { get; set; }

    public Employee AssignedTo { get; set; }

    // Parameterless constructor for EF
    public Task() : base(0) { }
    
    public Task(int id, string description, bool isComplete, 
        DateOnly assignedDate, TimeOnly assignedTime, 
        int createdById, Manager createdBy, 
        int assignedToId, Employee assignedTo) 
        : base(id) {
        Description = description;
        IsComplete = isComplete;
        AssignedDate = assignedDate;
        AssignedTime = assignedTime;
        CreatedById = createdById;
        CreatedBy = createdBy;
        AssignedToId = assignedToId;
        AssignedTo = assignedTo;
    }
}