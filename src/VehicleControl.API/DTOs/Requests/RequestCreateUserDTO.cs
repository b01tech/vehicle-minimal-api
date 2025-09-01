using VehicleControl.API.Domain.Enums;

namespace VehicleControl.API.DTOs.Requests;

public record RequestCreateUserDTO(string Name, string Email, string Password, UserRole Role);
