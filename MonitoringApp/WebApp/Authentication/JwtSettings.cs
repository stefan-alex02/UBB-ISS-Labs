namespace WebApp.Authentication;

public class JwtSettings {
    public TimeSpan ManagerTokenLifetime { get; set; }
    public TimeSpan EmployeeTokenLifetime { get; set; }
    public TimeSpan RefreshWindow { get; set; }
    public TimeSpan EmployeeTokenEndOfDay { get; set; }
}