using FluentAssertions;
using VehicleControl.API.Domain.Entities;

namespace VehicleControl.Test;

public class VehicleEntityTests
{
    [Fact]
    public void Vehicle_Constructor_ShouldCreateVehicleWithCorrectProperties()
    {
        // Arrange
        var licencePlate = "ABC-1234";
        var model = "Honda Civic";
        var year = 2020;

        // Act
        var vehicle = new Vehicle(licencePlate, model, year);

        // Assert
        vehicle.Should().NotBeNull();
        vehicle.LicencePlate.Should().Be(licencePlate);
        vehicle.Model.Should().Be(model);
        vehicle.Year.Should().Be(year);
        vehicle.Active.Should().BeTrue();
        vehicle.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        vehicle.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void Vehicle_Update_ShouldUpdatePropertiesAndTimestamp()
    {
        // Arrange
        var vehicle = new Vehicle("ABC-1234", "Honda Civic", 2020);
        var originalUpdatedAt = vehicle.UpdatedAt;
        
        // Wait a small amount to ensure timestamp difference
        Thread.Sleep(10);

        // Act
        vehicle.Update("XYZ-9876", "Toyota Corolla", 2021);

        // Assert
        vehicle.LicencePlate.Should().Be("XYZ-9876");
        vehicle.Model.Should().Be("Toyota Corolla");
        vehicle.Year.Should().Be(2021);
        vehicle.UpdatedAt.Should().BeAfter(originalUpdatedAt);
    }

    [Fact]
    public void Vehicle_SetAsInactive_ShouldSetActiveToFalseAndUpdateTimestamp()
    {
        // Arrange
        var vehicle = new Vehicle("ABC-1234", "Honda Civic", 2020);
        var originalUpdatedAt = vehicle.UpdatedAt;
        
        // Wait a small amount to ensure timestamp difference
        Thread.Sleep(10);

        // Act
        vehicle.SetAsInactive();

        // Assert
        vehicle.Active.Should().BeFalse();
        vehicle.UpdatedAt.Should().BeAfter(originalUpdatedAt);
    }

    [Fact]
    public void Vehicle_SetAsActive_ShouldSetActiveToTrueAndUpdateTimestamp()
    {
        // Arrange
        var vehicle = new Vehicle("ABC-1234", "Honda Civic", 2020);
        vehicle.SetAsInactive(); // First set as inactive
        var originalUpdatedAt = vehicle.UpdatedAt;
        
        // Wait a small amount to ensure timestamp difference
        Thread.Sleep(10);

        // Act
        vehicle.SetAsActive();

        // Assert
        vehicle.Active.Should().BeTrue();
        vehicle.UpdatedAt.Should().BeAfter(originalUpdatedAt);
    }

    [Theory]
    [InlineData("ABC-1234", "Honda Civic", 2020)]
    [InlineData("XYZ-9876", "Toyota Corolla", 2021)]
    [InlineData("DEF-5555", "Ford Focus", 2019)]
    public void Vehicle_Constructor_ShouldAcceptDifferentValidInputs(string licencePlate, string model, int year)
    {
        // Act
        var vehicle = new Vehicle(licencePlate, model, year);

        // Assert
        vehicle.LicencePlate.Should().Be(licencePlate);
        vehicle.Model.Should().Be(model);
        vehicle.Year.Should().Be(year);
        vehicle.Active.Should().BeTrue();
    }
}