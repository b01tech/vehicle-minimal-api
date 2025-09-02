using VehicleControl.API.Domain.Entities;
using VehicleControl.API.Domain.Interfaces;
using VehicleControl.API.DTOs.Requests;
using VehicleControl.API.DTOs.Responses;

namespace VehicleControl.API.Services.Mapping;

internal class VehicleMapper : IVehicleMapper
{
    public Vehicle ToEntity(RequestVehicleDTO dto)
    {
        return new Vehicle(dto.LicencePlate, dto.Model, dto.Year);
    }

    public ResponseVehicleDTO ToResponse(Vehicle vehicle)
    {
        return new ResponseVehicleDTO(vehicle.Id, vehicle.Active, vehicle.LicencePlate, vehicle.Model, vehicle.Year);
    }
}