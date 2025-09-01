namespace VehicleControl.API.DTOs.Responses;

public record ResponseCreatedUserDTO(long Id, string Name, string Email, string Role, DateTime CreatedAt);
