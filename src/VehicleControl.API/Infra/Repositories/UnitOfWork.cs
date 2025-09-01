using VehicleControl.API.Domain.Interfaces;
using VehicleControl.API.Infra.Data;

namespace VehicleControl.API.Infra.Repositories;

internal class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
    }

    public async Task CommitAsync()
    {
        await _context.SaveChangesAsync();
    }
}
