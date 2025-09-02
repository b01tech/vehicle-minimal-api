using Microsoft.EntityFrameworkCore;
using VehicleControl.API.Domain.Entities;
using VehicleControl.API.Domain.Interfaces;
using VehicleControl.API.DTOs.Responses;
using VehicleControl.API.Infra.Data;

namespace VehicleControl.API.Infra.Repositories;

internal class VehicleRepository : IVehicleRepository
{
    private readonly AppDbContext _context;
    private const int PageSize = 10;

    public VehicleRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ResponsePaginatedVehicleDTO> GetAll(int page)
    {
        var skip = (page - 1) * PageSize;
        
        var totalItems = await _context.Vehicles
            .Where(v => v.Active)
            .CountAsync();
            
        var vehicles = await _context.Vehicles
            .AsNoTracking()
            .Where(v => v.Active)
            .OrderBy(v => v.Id)
            .Skip(skip)
            .Take(PageSize)
            .Select(v => new ResponseVehicleDTO(v.Id, v.Active, v.LicencePlate, v.Model, v.Year))
            .ToListAsync();
            
        var totalPages = (int)Math.Ceiling((double)totalItems / PageSize);
        
        return new ResponsePaginatedVehicleDTO(vehicles, page, totalPages, totalItems);
    }

    public async Task<Vehicle> GetById(long id)
    {
        var vehicle = await _context.Vehicles.FindAsync(id);
        return vehicle;
    }

    public async Task<Vehicle> Create(Vehicle vehicle)
    {
        var vehicleCreated = await _context.Vehicles.AddAsync(vehicle);
        return vehicleCreated.Entity;
    }

    public async Task<Vehicle> Update(long id, string licencePlate, string model, int year)
    {
        var vehicle = await GetById(id);
        vehicle.Update(licencePlate, model, year);
        _context.Vehicles.Update(vehicle);
        return vehicle;
    }

    public async Task Delete(long id)
    {
        var vehicle = await GetById(id);
        vehicle.SetAsInactive();
        _context.Vehicles.Update(vehicle);
    }

    public Task<bool> LicencePlateExists(string licencePlate)
    {
        return _context.Vehicles.AnyAsync(v => v.LicencePlate == licencePlate && v.Active);
    }

    public Task<bool> LicencePlateExistsForUpdate(string licencePlate, long vehicleId)
    {
        return _context.Vehicles.AnyAsync(v => v.LicencePlate == licencePlate && v.Id != vehicleId && v.Active);
    }
}