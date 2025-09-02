using VehicleControl.API.Domain.Interfaces;
using VehicleControl.API.DTOs.Requests;
using VehicleControl.API.DTOs.Responses;
using VehicleControl.API.Exceptions;

namespace VehicleControl.API.Services.Entities;

internal class VehicleService : IVehicleService
{
    private readonly IVehicleRepository _vehicleRepository;
    private readonly IUnitOfWork _unit;
    private readonly IVehicleMapper _mapper;

    public VehicleService(IVehicleRepository vehicleRepository, IUnitOfWork unit, IVehicleMapper mapper)
    {
        _vehicleRepository = vehicleRepository;
        _unit = unit;
        _mapper = mapper;
    }

    public async Task<ResponsePaginatedVehicleDTO> GetAll(int page)
    {
        return await _vehicleRepository.GetAll(page);
    }

    public async Task<ResponseVehicleDTO> GetById(long id)
    {
        var vehicle = await _vehicleRepository.GetById(id) ?? throw new NotFoundException(ResourceErrorMessages.VEHICLE_NOT_FOUND);
        return _mapper.ToResponse(vehicle);
    }

    public async Task<ResponseVehicleDTO> Create(RequestVehicleDTO request)
    {
        if (await _vehicleRepository.LicencePlateExists(request.LicencePlate))
            throw new InputInvalidException(ResourceErrorMessages.LICENCE_PLATE_ALREADY_REGISTERED);

        var vehicle = _mapper.ToEntity(request);
        await _vehicleRepository.Create(vehicle);
        await _unit.CommitAsync();

        return _mapper.ToResponse(vehicle);
    }

    public async Task<ResponseVehicleDTO> Update(long id, RequestVehicleDTO request)
    {
        var vehicle = await _vehicleRepository.GetById(id) ?? throw new NotFoundException(ResourceErrorMessages.VEHICLE_NOT_FOUND);
        
        if (await _vehicleRepository.LicencePlateExistsForUpdate(request.LicencePlate, id))
            throw new InputInvalidException(ResourceErrorMessages.LICENCE_PLATE_ALREADY_REGISTERED);

        var updatedVehicle = await _vehicleRepository.Update(id, request.LicencePlate, request.Model, request.Year);
        await _unit.CommitAsync();

        return _mapper.ToResponse(updatedVehicle);
    }

    public async Task Delete(long id)
    {
        var vehicle = await _vehicleRepository.GetById(id) ?? throw new NotFoundException(ResourceErrorMessages.VEHICLE_NOT_FOUND);
        await _vehicleRepository.Delete(id);
        await _unit.CommitAsync();
    }
}