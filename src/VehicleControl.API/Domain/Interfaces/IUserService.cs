using VehicleControl.API.Domain.Entities;
using VehicleControl.API.Domain.Enums;
using VehicleControl.API.DTOs.Requests;
using VehicleControl.API.DTOs.Responses;

namespace VehicleControl.API.Domain.Interfaces;

public interface IUserService
{
    Task<string> DoLogin(RequestUserLoginDTO request);
    Task<User> GetById(long id);
    Task<ResponseCreatedUserDTO> Create(RequestCreateUserDTO request);
    Task<User> Update(long id, string name, string email, string password);
    Task ChangeRole(long id, UserRole role);
    Task Delete(long id);
}
