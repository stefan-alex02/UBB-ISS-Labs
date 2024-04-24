namespace Domain;

public class Manager(int id, string username, string name, string password) : 
    User(id, username, name, password, UserRole.Manager) {
    public IEnumerable<Task> CreatedTasks { get; set; } = new List<Task>();

    public Manager(string username, string name, string password) 
        : this(default, username, name, password) { }

    public override string ToString() {
        return $"Manager: {Name} ({Username})";
    }
}