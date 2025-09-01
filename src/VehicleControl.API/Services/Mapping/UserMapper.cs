using VehicleControl.API.Domain.Entities;
using VehicleControl.API.Domain.Interfaces;
using VehicleControl.API.DTOs.Requests;
using VehicleControl.API.DTOs.Responses;

namespace VehicleControl.API.Services.Mapping;

public class UserMapper : IUserMapper
{
    public User ToEntity(RequestCreateUserDTO dto) =>
         new(dto.Name.Trim(), dto.Email.Trim().ToLower(), dto.Password, dto.Role);

    public ResponseCreatedUserDTO ToCreatedResponse(User entity) =>
        new(entity.Id, entity.Name, entity.Email, entity.Role.ToString(), entity.CreatedAt);

    public ResponseDataUserDTO ToDataResponse(User entity) =>
        new(entity.Id, entity.Active, entity.Name, entity.Email, entity.Role.ToString(), entity.CreatedAt, entity.UpdatedAt);    
}
