namespace VehicleControl.API.DTOs.Requests;

public record RequestLoginDTO(
    string Email,
    string Password
);