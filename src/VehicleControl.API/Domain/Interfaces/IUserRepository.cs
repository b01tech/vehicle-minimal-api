using VehicleControl.API.Domain.Entities;
using VehicleControl.API.Domain.Enums;

namespace VehicleControl.API.Domain.Interfaces;

public interface IUserRepository
{
    Task<bool> DoLogin(string email, string passwordHash);
    Task<User> GetById(long id);
    Task<User> Create(User user);
    Task<User> Update(long id, string name, string email, string passwordHash);
    Task ChangeRole(long id, UserRole role);
    Task Delete(long id);
    Task<bool> EmailExists(string email);
    Task<bool> UsernameExists(string name);
}
