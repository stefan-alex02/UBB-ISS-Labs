namespace WebApp.Model.User;

public class LoginResponse(string token) {
    public string Token { get; set; } = token;
}