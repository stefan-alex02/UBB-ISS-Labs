using Microsoft.IdentityModel.Tokens;

namespace WebApp.Authentication;

public class JwtOptions {
    public string Key { get; set; }
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public TokenValidationParameters TokenValidationParameters { get; set; }

    public JwtOptions() {
        TokenValidationParameters = new TokenValidationParameters {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
        };
    }
}