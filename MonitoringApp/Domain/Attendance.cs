using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices.JavaScript;
using Domain.Users;

namespace Domain;

public class Attendance : Entity<int> {
    public DateOnly? Day { get; set; }
    public TimeOnly? Start { get; set; }
    public TimeOnly? End { get; set; }
    public Employee MarkedBy { get; set; } = null!;

    public Attendance() : base(default) { }

    public Attendance(int id, DateOnly? day, TimeOnly? start, TimeOnly? end, Employee markedBy) : base(id) {
        Day = day;
        Start = start;
        End = end;
        MarkedBy = markedBy;
    }
}