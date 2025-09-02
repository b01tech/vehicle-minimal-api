using VehicleControl.API.DTOs.Requests;
using VehicleControl.API.DTOs.Responses;

namespace VehicleControl.API.Domain.Interfaces;

public interface IVehicleService
{
    Task<ResponsePaginatedVehicleDTO> GetAll(int page);
    Task<ResponseVehicleDTO> GetById(long id);
    Task<ResponseVehicleDTO> Create(RequestVehicleDTO request);
    Task<ResponseVehicleDTO> Update(long id, RequestVehicleDTO request);
    Task Delete(long id);
}