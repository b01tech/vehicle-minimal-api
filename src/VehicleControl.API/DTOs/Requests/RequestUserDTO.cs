using VehicleControl.API.Domain.Enums;

namespace VehicleControl.API.DTOs.Requests;

public record RequestUserDTO(string Name, string Email, string Password, UserRole Role);
public record RequestUpdateUserDTO(string Name, string Email, string Password);
