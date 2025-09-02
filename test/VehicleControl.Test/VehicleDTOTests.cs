using FluentAssertions;
using VehicleControl.API.DTOs.Requests;
using VehicleControl.API.DTOs.Responses;

namespace VehicleControl.Test;

public class VehicleDTOTests
{
    [Fact]
    public void RequestVehicleDTO_ShouldCreateWithCorrectProperties()
    {
        // Arrange
        var licencePlate = "ABC-1234";
        var model = "Honda Civic";
        var year = 2020;

        // Act
        var dto = new RequestVehicleDTO(licencePlate, model, year);

        // Assert
        dto.Should().NotBeNull();
        dto.LicencePlate.Should().Be(licencePlate);
        dto.Model.Should().Be(model);
        dto.Year.Should().Be(year);
    }

    [Fact]
    public void ResponseVehicleDTO_ShouldCreateWithCorrectProperties()
    {
        // Arrange
        var id = 1L;
        var active = true;
        var licencePlate = "ABC-1234";
        var model = "Honda Civic";
        var year = 2020;

        // Act
        var dto = new ResponseVehicleDTO(id, active, licencePlate, model, year);

        // Assert
        dto.Should().NotBeNull();
        dto.Id.Should().Be(id);
        dto.Active.Should().Be(active);
        dto.LicencePlate.Should().Be(licencePlate);
        dto.Model.Should().Be(model);
        dto.Year.Should().Be(year);
    }

    [Fact]
    public void ResponsePaginatedVehicleDTO_ShouldCreateWithCorrectProperties()
    {
        // Arrange
        var vehicles = new List<ResponseVehicleDTO>
        {
            new(1L, true, "ABC-1234", "Honda Civic", 2020),
            new(2L, true, "XYZ-9876", "Toyota Corolla", 2021)
        };
        var currentPage = 1;
        var totalPages = 5;
        var totalItems = 50;

        // Act
        var dto = new ResponsePaginatedVehicleDTO(vehicles, currentPage, totalPages, totalItems);

        // Assert
        dto.Should().NotBeNull();
        dto.Vehicles.Should().HaveCount(2);
        dto.Vehicles.Should().BeEquivalentTo(vehicles);
        dto.CurrentPage.Should().Be(currentPage);
        dto.TotalPages.Should().Be(totalPages);
        dto.TotalItems.Should().Be(totalItems);
    }

    [Theory]
    [InlineData("ABC-1234", "Honda Civic", 2020)]
    [InlineData("XYZ-9876", "Toyota Corolla", 2021)]
    [InlineData("DEF-5555", "Ford Focus", 2019)]
    public void RequestVehicleDTO_ShouldAcceptDifferentValidInputs(string licencePlate, string model, int year)
    {
        // Act
        var dto = new RequestVehicleDTO(licencePlate, model, year);

        // Assert
        dto.LicencePlate.Should().Be(licencePlate);
        dto.Model.Should().Be(model);
        dto.Year.Should().Be(year);
    }

    [Fact]
    public void ResponsePaginatedVehicleDTO_ShouldHandleEmptyVehiclesList()
    {
        // Arrange
        var vehicles = new List<ResponseVehicleDTO>();
        var currentPage = 1;
        var totalPages = 0;
        var totalItems = 0;

        // Act
        var dto = new ResponsePaginatedVehicleDTO(vehicles, currentPage, totalPages, totalItems);

        // Assert
        dto.Should().NotBeNull();
        dto.Vehicles.Should().BeEmpty();
        dto.CurrentPage.Should().Be(currentPage);
        dto.TotalPages.Should().Be(totalPages);
        dto.TotalItems.Should().Be(totalItems);
    }
}