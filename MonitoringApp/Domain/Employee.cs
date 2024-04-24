namespace Domain;

public class Employee(int id, string username, string name, string password) : 
    User(id, username, name, password, UserRole.Employee) {
    public IEnumerable<Attendance> Attendances { get; set; } = new List<Attendance>();
    public IEnumerable<Task> Tasks { get; set; } = new List<Task>();
    
    public Employee(string username, string name, string password) 
        : this(default, username, name, password) { }
    
    public override string ToString() {
        return $"Employee: {Name} ({Username})";
    }
}