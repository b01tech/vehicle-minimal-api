using FluentAssertions;
using VehicleControl.API.Domain.Entities;
using VehicleControl.API.DTOs.Requests;
using VehicleControl.API.Services.Mapping;

namespace VehicleControl.Test;

public class VehicleMapperTests
{
    private readonly VehicleMapper _mapper;

    public VehicleMapperTests()
    {
        _mapper = new VehicleMapper();
    }

    [Fact]
    public void ToEntity_ShouldMapRequestDTOToVehicleEntity()
    {
        // Arrange
        var request = new RequestVehicleDTO("ABC-1234", "Honda Civic", 2020);

        // Act
        var result = _mapper.ToEntity(request);

        // Assert
        result.Should().NotBeNull();
        result.LicencePlate.Should().Be("ABC-1234");
        result.Model.Should().Be("Honda Civic");
        result.Year.Should().Be(2020);
        result.Active.Should().BeTrue();
    }

    [Fact]
    public void ToResponse_ShouldMapVehicleEntityToResponseDTO()
    {
        // Arrange
        var vehicle = new Vehicle("XYZ-9876", "Toyota Corolla", 2021);

        // Act
        var result = _mapper.ToResponse(vehicle);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(vehicle.Id);
        result.Active.Should().Be(vehicle.Active);
        result.LicencePlate.Should().Be("XYZ-9876");
        result.Model.Should().Be("Toyota Corolla");
        result.Year.Should().Be(2021);
    }

    [Theory]
    [InlineData("ABC-1234", "Honda Civic", 2020)]
    [InlineData("XYZ-9876", "Toyota Corolla", 2021)]
    [InlineData("DEF-5555", "Ford Focus", 2019)]
    public void ToEntity_ShouldMapDifferentVehicleData(string licencePlate, string model, int year)
    {
        // Arrange
        var request = new RequestVehicleDTO(licencePlate, model, year);

        // Act
        var result = _mapper.ToEntity(request);

        // Assert
        result.LicencePlate.Should().Be(licencePlate);
        result.Model.Should().Be(model);
        result.Year.Should().Be(year);
    }
}