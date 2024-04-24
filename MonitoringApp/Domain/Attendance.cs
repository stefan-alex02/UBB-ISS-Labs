using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices.JavaScript;

namespace Domain;

public class Attendance : Entity<int> {
    public DateOnly? Day { get; set; }
    public TimeOnly? Start { get; set; }
    public TimeOnly? End { get; set; }
    
    
    [ForeignKey("MarkedBy")]
    public int MarkedById { get; set; }
    public Employee MarkedBy { get; set; }

    // Parameterless constructor for EF
    public Attendance() : base(0) { }

    public Attendance(int id, DateOnly? day, TimeOnly? start, TimeOnly? end, Employee markedBy) : base(id) {
        Day = day;
        Start = start;
        End = end;
        MarkedBy = markedBy;
        MarkedById = markedBy.Id;
    }
}