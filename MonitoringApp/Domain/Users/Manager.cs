namespace Domain.Users;

public class Manager : User {
    public IEnumerable<Task> CreatedTasks { get; set; } = new List<Task>();
    
    public Manager() { }

    public Manager(string username, string name, string password) 
        : this(default, username, name, password) { }

    public Manager(int id, string username, string name, string password) : 
        base(id, username, name, password, UserRole.Manager) {
    }

    public override string ToString() {
        return $"Manager: {Name} ({Username})";
    }
}