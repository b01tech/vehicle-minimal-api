using VehicleControl.API.Domain.Entities;
using VehicleControl.API.Domain.Enums;
using VehicleControl.API.DTOs.Requests;
using VehicleControl.API.DTOs.Responses;

namespace VehicleControl.API.Domain.Interfaces;

public interface IUserService
{
    Task<string> DoLogin(RequestLoginDTO request);
    Task<ResponseDataUserDTO> GetById(long id);
    Task<ResponseUserDTO> Create(RequestUserDTO request);
    Task<ResponseDataUserDTO> Update(long id, RequestUpdateUserDTO request);
    Task ChangeRole(long id, UserRole role);
    Task Delete(long id);
}
