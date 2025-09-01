using VehicleControl.API.Domain.Entities;
using VehicleControl.API.Domain.Enums;
using VehicleControl.API.Domain.Interfaces;
using VehicleControl.API.DTOs.Requests;
using VehicleControl.API.DTOs.Responses;

namespace VehicleControl.API.Services;

internal class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unit;
    private readonly IMapper _mapper;
    private readonly IEncrypter _encrypter;

    public UserService(IUserRepository userRepository, IUnitOfWork unit, IMapper mapper, IEncrypter encrypter)
    {
        _userRepository = userRepository;
        _unit = unit;
        _mapper = mapper;
        _encrypter = encrypter;
    }

    public Task<string> DoLogin(RequestUserLoginDTO request)
    {
        throw new NotImplementedException();
    }
    public async Task<ResponseCreatedUserDTO> Create(RequestCreateUserDTO request)
    {
        if (await _userRepository.EmailExists(request.Email))
            throw new Exception("Email já está em uso.");

        if (await _userRepository.UsernameExists(request.Name))
            throw new Exception("Nome de usuário já está em uso.");
       
        var requestHashed = HashPassword(request);

        var user = _mapper.Map<User>(requestHashed);
        await _userRepository.Create(user);
        await _unit.CommitAsync();

        return _mapper.Map<ResponseCreatedUserDTO>(user);
    }
    public Task ChangeRole(long id, UserRole role)
    {
        throw new NotImplementedException();
    }

    public Task Delete(long id)
    {
        throw new NotImplementedException();
    }



    public Task<User> GetById(long id)
    {
        throw new NotImplementedException();
    }

    public Task<User> Update(long id, string name, string email, string password)
    {
        throw new NotImplementedException();
    }

    private RequestCreateUserDTO HashPassword(RequestCreateUserDTO request)
    {
        var passwordHash = _encrypter.Encrypt(request.Password);
        return new RequestCreateUserDTO(
            request.Name,
            request.Email,
            passwordHash,
            request.Role
        );
    }
}
