namespace VehicleControl.API.DTOs.Responses;

public record ResponseVehicleDTO(long Id, bool Active, string LicencePlate, string Model, int Year);