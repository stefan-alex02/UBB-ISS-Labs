using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Domain.Users;

namespace WebApp.Authentication;

public class JwtService(string issuer, string audience, string key, JwtSettings jwtSettings) {
    private readonly SymmetricSecurityKey _signingKey = new(Encoding.UTF8.GetBytes(key));

    public JwtSettings JwtSettings => jwtSettings;

    public string GenerateToken(int userId, string username, string name, UserRole userRole) {
        var claims = new[] {
            new Claim("user_id", userId.ToString()),
            new Claim("username", username),
            new Claim("name", name),
            new Claim("user_role", ((int)userRole).ToString())
        };
        
        DateTime expireAt;
        if (userRole == UserRole.Manager) {
            expireAt = DateTime.UtcNow.Add(jwtSettings.ManagerTokenLifetime);
        } else {
            var now = DateTime.UtcNow;
            // expireAt = now.Date.AddHours(jwtSettings.EmployeeTokenEndOfDay.TotalHours);
            // if (now > expireAt) {
            //     expireAt = expireAt.AddDays(1); // 6:00 PM of the next day
            // }
            // else if (expireAt - now < jwtSettings.EmployeeTokenLifetime) {
                expireAt = now.Add(jwtSettings.EmployeeTokenLifetime);
            // }
        }

        var signingCredentials = new SigningCredentials(_signingKey, SecurityAlgorithms.HmacSha256);
        var jwtToken = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: expireAt,
            signingCredentials: signingCredentials
        );

        return new JwtSecurityTokenHandler().WriteToken(jwtToken);
    }
}