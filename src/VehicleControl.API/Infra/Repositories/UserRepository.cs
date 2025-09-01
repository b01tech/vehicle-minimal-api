using Microsoft.EntityFrameworkCore;
using VehicleControl.API.Domain.Entities;
using VehicleControl.API.Domain.Enums;
using VehicleControl.API.Domain.Interfaces;
using VehicleControl.API.Infra.Data;

namespace VehicleControl.API.Infra.Repositories;

internal class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> DoLogin(string email, string passwordHash)
    {
        return await _context.Users.AnyAsync(u =>
           u.Active
           && u.Email == email
           && u.PasswordHash == passwordHash
           );
    }
    public async Task<User> GetById(long id)
    {
        var user = await _context.Users.FindAsync(id);
        return user ?? throw new Exception();
    }
    
    public async Task<User> GetByEmail(string email)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email && u.Active);
        return user ?? throw new Exception("Usuário não encontrado.");
    }
    public async Task<User> Create(User user)
    {
        var userCreated = await _context.Users.AddAsync(user);
        return userCreated.Entity;
    }
    public async Task<User> Update(long id, string name, string email, string passwordHash)
    {
        var user = await GetById(id);
        user.Update(name, email, passwordHash);
        _context.Users.Update(user);
        return user;
    }
    public async Task ChangeRole(long id, UserRole role)
    {
        var user = await GetById(id);
        user.ChangeRole(role);
        _context.Users.Update(user);
    }
    public async Task Delete(long id)
    {
        var user = await GetById(id);
        user.SetAsInactive();
        _context.Users.Update(user);
    }
    public Task<bool> EmailExists(string email)
    {
        return _context.Users.AnyAsync(u => u.Email == email);
    }

    public Task<bool> UsernameExists(string name)
    {
        return _context.Users.AnyAsync(u => u.Name == name);
    }
}
