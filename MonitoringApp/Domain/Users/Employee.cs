namespace Domain.Users;

public class Employee : User {
    public IEnumerable<Attendance> Attendances { get; set; } = new List<Attendance>();
    public IEnumerable<Task> Tasks { get; set; } = new List<Task>();
    
    public Employee() { }
    
    public Employee(string username, string name, string password) 
        : this(default, username, name, password) { }

    public Employee(int id, string username, string name, string password) : 
        base(id, username, name, password, UserRole.Employee) {
    }

    public override string ToString() {
        return $"Employee: {Name} ({Username})";
    }
}