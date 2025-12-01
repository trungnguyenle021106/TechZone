using Carter;
using Identity.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Identity.API.Endpoints
{
    public record RegisterRequest(string Email, string Password, string FirstName, string LastName);
    public record LoginRequest(string Email, string Password);
    public record AuthResponse(string Token);

    public class AuthEndpoints : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/auth");

            // Register
            group.MapPost("/register", async (RegisterRequest request, IIdentityService identityService) =>
            {
                var result = await identityService.RegisterAsync(request.Email, request.Password, request.FirstName, request.LastName);

                if (!result.Success)
                    return Results.BadRequest(result.Errors);

                return Results.Ok("User registered successfully");
            });

            // Login
            group.MapPost("/login", async (LoginRequest request, IIdentityService identityService) =>
            {
                var result = await identityService.LoginAsync(request.Email, request.Password);

                if (!result.Success)
                    return Results.BadRequest(result.Errors);

                return Results.Ok(new AuthResponse(result.Token));
            });
        }
    }
}