using Microsoft.AspNetCore.Identity;
using VehicleControl.API.Domain.Enums;

namespace VehicleControl.API.Domain.Entities;

public class User: BaseEntity
{
    public string Name { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public UserRole Role { get; private set; } = UserRole.User;

    protected User() { }

    public User(string name, string email, string passwordHash, UserRole role)
    {
        Name = name;
        Email = email;
        PasswordHash = passwordHash;
        Role = role;
        Validate();
    }

    public void Update(string name, string email, string passwordHash)
    {
        Name = name;
        Email = email;
        PasswordHash = passwordHash;
        Validate();
        UpdateTimeStamp();
    }
    public void ChangeRole(UserRole role)
    {
        Role = role;
        UpdateTimeStamp();
    }

    private void Validate()
    {

    }
}
