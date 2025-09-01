using VehicleControl.API.Domain.Entities;
using VehicleControl.API.Domain.Enums;
using VehicleControl.API.Domain.Interfaces;
using VehicleControl.API.DTOs.Requests;
using VehicleControl.API.DTOs.Responses;

namespace VehicleControl.API.Services.Entities;

internal class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unit;
    private readonly IEncrypter _encrypter;
    private readonly IUserMapper _mapper;

    public UserService(IUserRepository userRepository, IUnitOfWork unit, IEncrypter encrypter, IUserMapper mapper)
    {
        _userRepository = userRepository;
        _unit = unit;
        _encrypter = encrypter;
        _mapper = mapper;
    }

    public Task<string> DoLogin(RequestUserLoginDTO request)
    {
        throw new NotImplementedException();
    }
    public async Task<ResponseDataUserDTO> GetById(long id)
    {
        var user = await _userRepository.GetById(id);
        if (user == null)
            throw new Exception("Usuário não encontrado.");
        var response = _mapper.ToDataResponse(user);
        return response;
    }
    public async Task<ResponseUserDTO> Create(RequestUserDTO request)
    {
        if (await _userRepository.EmailExists(request.Email))
            throw new Exception("Email já está em uso.");

        if (await _userRepository.UsernameExists(request.Name))
            throw new Exception("Nome de usuário já está em uso.");

        var requestHashed = HashPassword(request);

        var user = _mapper.ToEntity(requestHashed);
        await _userRepository.Create(user);
        await _unit.CommitAsync();

        return _mapper.ToCreatedResponse(user);
    }
    public async Task<ResponseDataUserDTO> Update(long id, RequestUpdateUserDTO request)
    {
        var user = await _userRepository.GetById(id);
        if (user == null)
            throw new Exception("Usuário não encontrado.");

        if (await _userRepository.EmailExists(request.Email))
            throw new Exception("Email já está em uso.");

        if (await _userRepository.UsernameExists(request.Name))
            throw new Exception("Nome de usuário já está em uso.");

        var requestHashed = HashPassword(request);

        user = await _userRepository.Update(id, requestHashed.Name, requestHashed.Email, requestHashed.Password);
        await _unit.CommitAsync();

        return _mapper.ToDataResponse(user);
    }

    public Task ChangeRole(long id, UserRole role)
    {
        throw new NotImplementedException();
    }

    public Task Delete(long id)
    {
        throw new NotImplementedException();
    }



    private RequestUserDTO HashPassword(RequestUserDTO request)
    {
        var passwordHash = _encrypter.Encrypt(request.Password);
        return new RequestUserDTO(
            request.Name,
            request.Email,
            passwordHash,
            request.Role
        );
    }
    private RequestUpdateUserDTO HashPassword(RequestUpdateUserDTO request)
    {
        var passwordHash = _encrypter.Encrypt(request.Password);
        return new RequestUpdateUserDTO(
            request.Name,
            request.Email,
            passwordHash
        );
    }
}
