namespace WebApp.Model;

public class RegisterRequest(string username, string name, string password) {
    public string Username { get; set; } = username;
    public string Name { get; set; } = name;
    public string Password { get; set; } = password;
}