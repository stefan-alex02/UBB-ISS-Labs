using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Domain;

namespace WebApp.Authentication;

public class JwtService {
    private readonly string _issuer;
    private readonly string _audience;
    private readonly SymmetricSecurityKey _signingKey;
    private readonly TimeSpan _tokenLifetime;

    public JwtService(string issuer, string audience, string key, TimeSpan tokenLifetime) {
        _issuer = issuer;
        _audience = audience;
        _signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        _tokenLifetime = tokenLifetime;
    }

    public string GenerateToken(User user) {
        var claims = new[] {
            new Claim("user_id", user.Id.ToString()),
            new Claim("username", user.Username!),
            new Claim("user_role", ((int)user.UserRole).ToString())
        };

        return GenerateToken(claims);
    }
    
    public string GenerateToken(Claim[] claims) {
        var signingCredentials = new SigningCredentials(_signingKey, SecurityAlgorithms.HmacSha256);
        var jwtToken = new JwtSecurityToken(
            issuer: _issuer,
            audience: _audience,
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: DateTime.UtcNow.Add(_tokenLifetime),
            signingCredentials: signingCredentials
        );

        return new JwtSecurityTokenHandler().WriteToken(jwtToken);
    }
}