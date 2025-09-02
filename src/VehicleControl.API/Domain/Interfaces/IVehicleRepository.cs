using VehicleControl.API.Domain.Entities;
using VehicleControl.API.DTOs.Responses;

namespace VehicleControl.API.Domain.Interfaces;

public interface IVehicleRepository
{
    Task<ResponsePaginatedVehicleDTO> GetAll(int page);
    Task<Vehicle> GetById(long id);
    Task<Vehicle> Create(Vehicle vehicle);
    Task<Vehicle> Update(long id, string licencePlate, string model, int year);
    Task Delete(long id);
    Task<bool> LicencePlateExists(string licencePlate);
    Task<bool> LicencePlateExistsForUpdate(string licencePlate, long vehicleId);
}