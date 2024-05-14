using System.ComponentModel.DataAnnotations.Schema;
using Domain.Users;

namespace Domain;

public class Task : Entity<int> {
    public string Description { get; set; } = null!;
    public bool IsComplete { get; set; }
    public DateOnly AssignedDate { get; set; }
    public TimeOnly AssignedTime { get; set; }
    public Manager CreatedBy { get; set; } = null!;
    public Employee AssignedTo { get; set; } = null!;

    public Task() : base(default) { }
    
    public Task(int id, string description, bool isComplete, 
        DateOnly assignedDate, TimeOnly assignedTime, 
        Manager createdBy, Employee assignedTo) 
        : base(id) {
        Description = description;
        IsComplete = isComplete;
        AssignedDate = assignedDate;
        AssignedTime = assignedTime;
        CreatedBy = createdBy;
        AssignedTo = assignedTo;
    }
}