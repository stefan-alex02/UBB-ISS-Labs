namespace Domain;

public abstract class User(int id, string username, string name, string password, UserRole userRole)
    : Entity<int>(id) {
    public string Username { get; set; } = username;
    public string Name { get; set; } = name;
    public string Password { get; set; } = password;
    public UserRole UserRole { get; set; } = userRole;

    protected User(string username, string name, string password, UserRole userRole) 
        : this(default, username, name, password, userRole) { }

    public override string ToString() {
        return $"User: {Name} ({Username})";
    }
}