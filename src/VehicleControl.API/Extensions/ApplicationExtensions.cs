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
        MapVehiclesEndpoints(app);
        return app;
    }
    private static void MapAuthEndpoints(WebApplication app)
    {
        var group = app.MapGroup("/auth")
            .WithTags("Authentication")
            .WithOpenApi();

        group.MapPost("/", async (RequestLoginDTO request, IUserService userService) =>
        {
            var token = await userService.DoLogin(request);
            var expiresAt = DateTime.UtcNow.AddMinutes(60);
            var response = new ResponseLoginDTO(expiresAt, token);

            return Results.Ok(response);

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
    private static void MapVehiclesEndpoints(WebApplication app)
    {
        var group = app.MapGroup("/vehicles")
            .WithTags("Vehicles")
            .WithOpenApi();

        group.MapGet("/list/{page:int}", async (IVehicleService vehicleService, int page = 1) =>
        {
            var vehicles = await vehicleService.GetAll(page);
            return Results.Ok(vehicles);
        }).WithName("GetAllVehicles")
            .WithSummary("Get all vehicles with pagination")
            .WithDescription("Endpoint to retrieve a paginated list of vehicles.")
            .RequireAuthorization()
            .Produces(StatusCodes.Status200OK);
        group.MapGet("/{id:long}", async (long id, IVehicleService vehicleService) =>
        {
            var vehicle = await vehicleService.GetById(id);
            return Results.Ok(vehicle);
        }).WithName("GetVehicleById")
            .WithSummary("Get vehicle by ID")
            .WithDescription("Endpoint to retrieve a vehicle by its unique ID.")
            .RequireAuthorization()
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);
        group.MapPost("/", async (RequestVehicleDTO dto, IVehicleService vehicleService) =>
        {
            var vehicle = await vehicleService.Create(dto);
            return Results.Created(string.Empty, vehicle);
        }).WithName("CreateVehicle")
            .WithSummary("Create a new vehicle")
            .WithDescription("Endpoint to create a new vehicle in the system.")
            .RequireAuthorization(policy => policy.RequireRole("Admin", "User"))
            .Produces(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden);
        group.MapPut("/{id:long}", async (long id, RequestVehicleDTO request, IVehicleService vehicleService) =>
        {
            var result = await vehicleService.Update(id, request);
            return Results.Ok(result);
        }).WithName("UpdateVehicle")
            .WithSummary("Update an existing vehicle")
            .WithDescription("Endpoint to update the details of an existing vehicle.")
            .RequireAuthorization(policy => policy.RequireRole("Admin", "User"))
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden);
        group.MapDelete("/{id:long}", async (long id, IVehicleService vehicleService) =>
        {
            await vehicleService.Delete(id);
            return Results.NoContent();
        }).WithName("DeleteVehicle")
            .WithSummary("Delete a vehicle")
            .WithDescription("Endpoint to delete a vehicle from the system.")
            .RequireAuthorization(policy => policy.RequireRole("Admin"))
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden);
    }
}
