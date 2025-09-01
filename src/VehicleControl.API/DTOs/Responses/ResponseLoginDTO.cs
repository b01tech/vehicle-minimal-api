namespace VehicleControl.API.DTOs.Responses;

public record ResponseLoginDTO(
    DateTime ExpiresAt,
    string Token
);