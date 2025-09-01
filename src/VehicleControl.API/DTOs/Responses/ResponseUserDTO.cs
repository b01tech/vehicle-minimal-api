namespace VehicleControl.API.DTOs.Responses;

public record ResponseUserDTO(long Id, string Name, string Email, string Role, DateTime CreatedAt);
public record ResponseDataUserDTO(long Id, bool Active, string Name, string Email, string Role, DateTime CreatedAt, DateTime UpdatedAt);
