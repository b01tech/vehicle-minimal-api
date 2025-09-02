using VehicleControl.API.Domain.Entities;
using VehicleControl.API.DTOs.Requests;
using VehicleControl.API.DTOs.Responses;

namespace VehicleControl.API.Domain.Interfaces;

public interface IVehicleMapper
{
    Vehicle ToEntity(RequestVehicleDTO dto);
    ResponseVehicleDTO ToResponse(Vehicle vehicle);
}