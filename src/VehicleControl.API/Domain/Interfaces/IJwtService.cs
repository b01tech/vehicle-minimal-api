using VehicleControl.API.Domain.Entities;

namespace VehicleControl.API.Domain.Interfaces;

public interface IJwtService
{
    string GenerateToken(User user);
    bool ValidateToken(string token);
}