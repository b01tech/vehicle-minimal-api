using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VehicleControl.API.Domain.Entities;
using VehicleControl.API.Domain.Enums;
using VehicleControl.API.Domain.Interfaces;
using VehicleControl.API.DTOs.Requests;
using VehicleControl.API.DTOs.Responses;
using VehicleControl.API.Infra.Data;

namespace VehicleControl.API.Extensions;

public static class ApplicationExtensions
{
    public static WebApplication ApplyMigrations(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        if (db.Database.GetPendingMigrations().Any())
        {
            db.Database.Migrate();
        }
        SeedData(scope.ServiceProvider);

        return app;
    }

    public static void SeedData(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var encrypter = scope.ServiceProvider.GetRequiredService<IEncrypter>();

        if (!db.Users.Any(u => u.Name == "admin"))
        {
            var hasher = new PasswordHasher<User>();

            var admin = new User(
                name: "admin",
                email: "admin@admin.com",
                passwordHash: encrypter.Encrypt("admin"),
                role: UserRole.Admin
            );

            db.Users.Add(admin);
            db.SaveChanges();
        }
    }

    public static WebApplication MapEndpoints(this WebApplication app)
    {
        MapAuthEndpoints(app);
        MapUsersEndpoints(app);
        return app;
    }
    private static void MapAuthEndpoints(WebApplication app)
    {
        var group = app.MapGroup("/auth")
            .WithTags("Authentication")
            .WithOpenApi();

        group.MapPost("/", async (RequestLoginDTO request, IUserService userService) =>
        {
            try
            {
                var token = await userService.DoLogin(request);
                var expiresAt = DateTime.UtcNow.AddMinutes(60);

                var response = new ResponseLoginDTO(expiresAt, token);

                return Results.Ok(response);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Results.Unauthorized();
            }
            catch (Exception ex)
            {
                return Results.BadRequest(ex.Message);
            }
        }).WithName("Login")
            .WithSummary("User login")
            .WithDescription("Endpoint for user authentication and JWT token generation.")
            .Produces<ResponseLoginDTO>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status400BadRequest);
    }
    private static void MapUsersEndpoints(WebApplication app)
    {
        var group = app.MapGroup("/users")
            .WithTags("Users")
            .WithOpenApi();

        group.MapGet("/{id:long}", async (long id, IUserService userService) =>
        {
            var user = await userService.GetById(id);
            return Results.Ok(user);
        }).WithName("GetUserById")
            .WithSummary("Get user by ID")
            .WithDescription("Endpoint to retrieve a user by their unique ID.")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        group.MapPost("/", async (RequestUserDTO dto, IUserService userService) =>
        {
            var user = await userService.Create(dto);
            return Results.Created(string.Empty, user);
        }).WithName("CreateUser")
            .WithSummary("Create a new user")
            .WithDescription("Endpoint to create a new user in the system.")
            .RequireAuthorization(policy => policy.RequireRole("Admin"))
            .Produces(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden);

        group.MapPut("/{id:long}", async (long id, RequestUpdateUserDTO request, IUserService userService) =>
        {
            var result = await userService.Update(id, request);
            return Results.Ok(result);
        }).WithName("UpdateUser")
            .WithSummary("Update an existing user")
            .WithDescription("Endpoint to update the details of an existing user.")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound);

        group.MapPatch("/{id:long}/role", async (long id, UserRole role, IUserService userService) =>
        {
            await userService.ChangeRole(id, role);
            return Results.NoContent();
        }).WithName("ChangeUserRole")
            .WithSummary("Change user role")
            .WithDescription("Endpoint to change the role of an existing user.")
            .RequireAuthorization(policy => policy.RequireRole("Admin"))
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden);

        group.MapDelete("/{id:long}", async (long id, IUserService userService) =>
        {
            await userService.Delete(id);
            return Results.NoContent();
        }).WithName("DeleteUser")
            .WithSummary("Delete a user")
            .WithDescription("Endpoint to delete a user from the system.")
            .RequireAuthorization(policy => policy.RequireRole("Admin"))
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden);
    }
}
