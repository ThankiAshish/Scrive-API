using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using ScriveAPI.Services;

public class AuthMiddleware
{
    private readonly RequestDelegate _next;
    private readonly UserServices _userServices;
    private readonly IConfiguration _configuration;

    public AuthMiddleware(RequestDelegate next, UserServices userServices, IConfiguration configuration)
    {
        _next = next;
        _userServices = userServices;
        _configuration = configuration;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var token = context.Request.Headers["x-auth-token"].FirstOrDefault();

        if (string.IsNullOrEmpty(token))
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("No token, authorization denied");
            return;
        }

        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSecret"])),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.FromMinutes(5)
            };

            var claimsPrincipal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);

            var userId = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Token is not valid");
                return;
            }

            var user = await _userServices.GetUser(userId);

            if (user == null)
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("User not found");
                return;
            }

            context.Items["User"] = user;

            await _next(context);
        }
        catch (Exception ex)
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("Token is not valid");
        }
    }
}