

using VehicleControl.API.Domain.Interfaces;
using VehicleControl.API.DTOs.Requests;

namespace VehicleControl.API.Extensions;

public static class ApplicationExtensions
{    
    public static WebApplication MapEndpoints(this WebApplication app)
    {
        MapLoginEndpoints(app);
        MapUsersEndpoints(app);
        return app;
    }

    private static void MapLoginEndpoints(WebApplication app)
    {
        var group = app.MapGroup("/login")
            .WithTags("Login")
            .WithOpenApi();

        group.MapPost("/", () => { return Results.Ok(); })
            .WithName("Login")
            .WithSummary("User login")
            .WithDescription("Endpoint to authenticate a user and return a JWT token.")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized);
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
            .Produces(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest);

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

        group.MapPatch("/{id:long}/role", () => { return Results.Ok(); })
            .WithName("ChangeUserRole")
            .WithSummary("Change user role")
            .WithDescription("Endpoint to change the role of an existing user.")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound);

        group.MapDelete("/{id:long}", () => { return Results.Ok(); })
            .WithName("DeleteUser")
            .WithSummary("Delete a user")
            .WithDescription("Endpoint to delete a user from the system.")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound);
    }
}
