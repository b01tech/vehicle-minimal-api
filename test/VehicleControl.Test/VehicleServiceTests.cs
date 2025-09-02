using FluentAssertions;
using Moq;
using VehicleControl.API.Domain.Entities;
using VehicleControl.API.Domain.Interfaces;
using VehicleControl.API.DTOs.Requests;
using VehicleControl.API.DTOs.Responses;
using VehicleControl.API.Exceptions;
using VehicleControl.API.Services.Entities;

namespace VehicleControl.Test;

public class VehicleServiceTests
{
    private readonly Mock<IVehicleRepository> _mockRepository;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IVehicleMapper> _mockMapper;
    private readonly VehicleService _service;

    public VehicleServiceTests()
    {
        _mockRepository = new Mock<IVehicleRepository>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockMapper = new Mock<IVehicleMapper>();
        _service = new VehicleService(_mockRepository.Object, _mockUnitOfWork.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task GetById_ShouldReturnVehicle_WhenVehicleExists()
    {
        // Arrange
        var vehicleId = 1L;
        var vehicle = new Vehicle("ABC-1234", "Honda Civic", 2020);
        var expectedResponse = new ResponseVehicleDTO(vehicleId, true, "ABC-1234", "Honda Civic", 2020);

        _mockRepository.Setup(r => r.GetById(vehicleId))
            .ReturnsAsync(vehicle);
        _mockMapper.Setup(m => m.ToResponse(vehicle))
            .Returns(expectedResponse);

        // Act
        var result = await _service.GetById(vehicleId);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expectedResponse);
        _mockRepository.Verify(r => r.GetById(vehicleId), Times.Once);
        _mockMapper.Verify(m => m.ToResponse(vehicle), Times.Once);
    }

    [Fact]
    public async Task GetById_ShouldThrowNotFoundException_WhenVehicleDoesNotExist()
    {
        // Arrange
        var vehicleId = 1L;
        _mockRepository.Setup(r => r.GetById(vehicleId))
            .ReturnsAsync((Vehicle?)null);

        // Act & Assert
        await _service.Invoking(s => s.GetById(vehicleId))
            .Should().ThrowAsync<NotFoundException>();
        
        _mockRepository.Verify(r => r.GetById(vehicleId), Times.Once);
        _mockMapper.Verify(m => m.ToResponse(It.IsAny<Vehicle>()), Times.Never);
    }

    [Fact]
    public async Task Create_ShouldCreateVehicle_WhenLicencePlateDoesNotExist()
    {
        // Arrange
        var request = new RequestVehicleDTO("ABC-1234", "Honda Civic", 2020);
        var vehicle = new Vehicle("ABC-1234", "Honda Civic", 2020);
        var expectedResponse = new ResponseVehicleDTO(1L, true, "ABC-1234", "Honda Civic", 2020);

        _mockRepository.Setup(r => r.LicencePlateExists(request.LicencePlate))
            .ReturnsAsync(false);
        _mockMapper.Setup(m => m.ToEntity(request))
            .Returns(vehicle);
        _mockRepository.Setup(r => r.Create(vehicle))
            .ReturnsAsync(vehicle);
        _mockMapper.Setup(m => m.ToResponse(vehicle))
            .Returns(expectedResponse);

        // Act
        var result = await _service.Create(request);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expectedResponse);
        _mockRepository.Verify(r => r.LicencePlateExists(request.LicencePlate), Times.Once);
        _mockRepository.Verify(r => r.Create(vehicle), Times.Once);
        _mockUnitOfWork.Verify(u => u.CommitAsync(), Times.Once);
    }

    [Fact]
    public async Task Create_ShouldThrowInputInvalidException_WhenLicencePlateAlreadyExists()
    {
        // Arrange
        var request = new RequestVehicleDTO("ABC-1234", "Honda Civic", 2020);

        _mockRepository.Setup(r => r.LicencePlateExists(request.LicencePlate))
            .ReturnsAsync(true);

        // Act & Assert
        await _service.Invoking(s => s.Create(request))
            .Should().ThrowAsync<InputInvalidException>();
        
        _mockRepository.Verify(r => r.LicencePlateExists(request.LicencePlate), Times.Once);
        _mockRepository.Verify(r => r.Create(It.IsAny<Vehicle>()), Times.Never);
        _mockUnitOfWork.Verify(u => u.CommitAsync(), Times.Never);
    }

    [Fact]
    public async Task Delete_ShouldDeleteVehicle_WhenVehicleExists()
    {
        // Arrange
        var vehicleId = 1L;
        var vehicle = new Vehicle("ABC-1234", "Honda Civic", 2020);

        _mockRepository.Setup(r => r.GetById(vehicleId))
            .ReturnsAsync(vehicle);

        // Act
        await _service.Delete(vehicleId);

        // Assert
        _mockRepository.Verify(r => r.GetById(vehicleId), Times.Once);
        _mockRepository.Verify(r => r.Delete(vehicleId), Times.Once);
        _mockUnitOfWork.Verify(u => u.CommitAsync(), Times.Once);
    }

    [Fact]
    public async Task Delete_ShouldThrowNotFoundException_WhenVehicleDoesNotExist()
    {
        // Arrange
        var vehicleId = 1L;
        _mockRepository.Setup(r => r.GetById(vehicleId))
            .ReturnsAsync((Vehicle?)null);

        // Act & Assert
        await _service.Invoking(s => s.Delete(vehicleId))
            .Should().ThrowAsync<NotFoundException>();
        
        _mockRepository.Verify(r => r.GetById(vehicleId), Times.Once);
        _mockRepository.Verify(r => r.Delete(It.IsAny<long>()), Times.Never);
        _mockUnitOfWork.Verify(u => u.CommitAsync(), Times.Never);
    }

    [Fact]
    public async Task GetAll_ShouldReturnPaginatedVehicles()
    {
        // Arrange
        var page = 1;
        var expectedResponse = new ResponsePaginatedVehicleDTO(
            new List<ResponseVehicleDTO>
            {
                new(1L, true, "ABC-1234", "Honda Civic", 2020),
                new(2L, true, "XYZ-9876", "Toyota Corolla", 2021)
            },
            1, 1, 2
        );

        _mockRepository.Setup(r => r.GetAll(page))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _service.GetAll(page);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expectedResponse);
        _mockRepository.Verify(r => r.GetAll(page), Times.Once);
    }
}