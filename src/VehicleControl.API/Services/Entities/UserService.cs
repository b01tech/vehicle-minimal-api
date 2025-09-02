using VehicleControl.API.Domain.Enums;
using VehicleControl.API.Domain.Interfaces;
using VehicleControl.API.DTOs.Requests;
using VehicleControl.API.DTOs.Responses;
using VehicleControl.API.Exceptions;

namespace VehicleControl.API.Services.Entities;

internal class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unit;
    private readonly IEncrypter _encrypter;
    private readonly IUserMapper _mapper;
    private readonly IJwtService _jwtService;

    public UserService(IUserRepository userRepository, IUnitOfWork unit, IEncrypter encrypter, IUserMapper mapper, IJwtService jwtService)
    {
        _userRepository = userRepository;
        _unit = unit;
        _encrypter = encrypter;
        _mapper = mapper;
        _jwtService = jwtService;
    }

    public async Task<string> DoLogin(RequestLoginDTO request)
    {
        var passwordHash = _encrypter.Encrypt(request.Password);
        var loginSuccess = await _userRepository.DoLogin(request.Email, passwordHash);

        if (!loginSuccess)
            throw new UnauthorizedException(ResourceErrorMessages.INVALID_CREDENTIALS);

        var user = await _userRepository.GetByEmail(request.Email);
        return _jwtService.GenerateToken(user);
    }
    public async Task<ResponseDataUserDTO> GetById(long id)
    {
        var user = await _userRepository.GetById(id) ?? throw new NotFoundException(ResourceErrorMessages.USER_NOT_FOUND);
        var response = _mapper.ToDataResponse(user);
        return response;
    }
    public async Task<ResponseUserDTO> Create(RequestUserDTO request)
    {
        if (await _userRepository.EmailExists(request.Email))
            throw new InputInvalidException(ResourceErrorMessages.EMAIL_ALREADY_REGISTERED);

        if (await _userRepository.UsernameExists(request.Name))
            throw new InputInvalidException(ResourceErrorMessages.NAME_ALREADY_REGISTERED);

        var requestHashed = HashPassword(request);

        var user = _mapper.ToEntity(requestHashed);
        await _userRepository.Create(user);
        await _unit.CommitAsync();

        return _mapper.ToCreatedResponse(user);
    }
    public async Task<ResponseDataUserDTO> Update(long id, RequestUpdateUserDTO request)
    {
        var user = await _userRepository.GetById(id) ?? throw new NotFoundException(ResourceErrorMessages.USER_NOT_FOUND);
        if (await _userRepository.EmailExists(request.Email))
            throw new InputInvalidException(ResourceErrorMessages.EMAIL_ALREADY_REGISTERED);

        if (await _userRepository.UsernameExists(request.Name))
            throw new InputInvalidException(ResourceErrorMessages.NAME_ALREADY_REGISTERED);

        var requestHashed = HashPassword(request);

        user = await _userRepository.Update(id, requestHashed.Name, requestHashed.Email, requestHashed.Password);
        await _unit.CommitAsync();

        return _mapper.ToDataResponse(user);
    }

    public async Task ChangeRole(long id, UserRole role)
    {
        var user = await _userRepository.GetById(id) ?? throw new NotFoundException(ResourceErrorMessages.USER_NOT_FOUND);

        await _userRepository.ChangeRole(user.Id, role);
        await _unit.CommitAsync();
    }

    public async Task Delete(long id)
    {
        var user = await _userRepository.GetById(id) ?? throw new NotFoundException(ResourceErrorMessages.USER_NOT_FOUND);
        await _userRepository.Delete(user.Id);
        await _unit.CommitAsync();
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
