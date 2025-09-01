using VehicleControl.API.Domain.Entities;
using VehicleControl.API.DTOs.Requests;
using VehicleControl.API.DTOs.Responses;

namespace VehicleControl.API.Domain.Interfaces;

public interface IUserMapper
{
    User ToEntity(RequestUserDTO dto);
    ResponseUserDTO ToCreatedResponse(User user);
    ResponseDataUserDTO ToDataResponse(User user);
}
