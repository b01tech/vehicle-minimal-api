using VehicleControl.API.Domain.Entities;
using VehicleControl.API.Domain.Interfaces;
using VehicleControl.API.DTOs.Requests;
using VehicleControl.API.DTOs.Responses;

namespace VehicleControl.API.Services;

internal class MappingService : IMapper
{
    public T Map<T>(object source)
    {
        if (typeof(T) == typeof(User) && source is RequestCreateUserDTO dto)
        {
            var user = new User(dto.Name.Trim(), dto.Email.Trim().ToLower(), dto.Password, dto.Role);
            return (T)(object)user;
        }
        else if (typeof(T) == typeof(ResponseCreatedUserDTO) && source is User entity)
        {
            var result = new ResponseCreatedUserDTO(entity.Id, entity.Name, entity.Email, entity.Role.ToString(), entity.CreatedAt);
            return (T)(object)result;
        }

        throw new InvalidOperationException($"Mapeamento não suportado: {source.GetType()} -> {typeof(T)}");
    }
}
