using System.IdentityModel.Tokens.Jwt;
using Domain.Users;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace WebApp.Authentication;

public class RefreshTokenMiddleware : IMiddleware {
    private readonly JwtService _jwtService;
    private readonly TokenValidationParameters _tokenValidationParameters;

    public RefreshTokenMiddleware(JwtService jwtService, IOptions<JwtOptions> options) {
        _jwtService = jwtService;
        _tokenValidationParameters = options.Value.TokenValidationParameters;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next) {
        var authHeader = context.Request.Headers["Authorization"].ToString();
    
        // Check if the Authorization header contains a Bearer token and it is not null
        if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer ") &&
            !authHeader
                .Substring("Bearer ".Length).Trim()
                .Equals("null", StringComparison.OrdinalIgnoreCase)) {
            // Extract the token from the header
            var token = authHeader.Substring("Bearer ".Length).Trim();

            // Initialize the JwtSecurityTokenHandler
            var tokenHandler = new JwtSecurityTokenHandler();
            try {
                // Validate the token using the provided TokenValidationParameters
                var principal = tokenHandler.ValidateToken(token, _tokenValidationParameters, out var validatedToken);
            
                // Check if the validated token is a JWT token
                if (validatedToken is JwtSecurityToken jwtToken) {
                    // Calculate the remaining lifetime of the token
                    var remainingLifetime = jwtToken.ValidTo - DateTime.UtcNow;
                
                    // If the token is close to expiration (within 5 minutes), refresh it
                    if (remainingLifetime < TimeSpan.FromMinutes(5)) {
                        // Generate a new token using the existing claims
                        int userId = int.Parse(principal.FindFirst("user_id").Value);
                        string username = principal.FindFirst("username").Value;
                        string name = principal.FindFirst("name").Value;
                        UserRole userRole = (UserRole)int.Parse(principal.FindFirst("user_role").Value);
                        
                        var newToken = _jwtService.GenerateToken(userId, username, name, userRole);

                        // Set the new token in the response header
                        context.Response.Headers["Authorization"] = "Bearer " + newToken;
                    }
                }
            }
            catch (SecurityTokenException) {
                // If token validation fails, handle it as unauthorized
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return;
            }
        }

        // Continue the request through the pipeline
        await next(context);
    }
}