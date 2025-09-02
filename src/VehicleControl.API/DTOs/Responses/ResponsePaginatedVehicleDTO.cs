namespace VehicleControl.API.DTOs.Responses;

public record ResponsePaginatedVehicleDTO(
    IList<ResponseVehicleDTO> Vehicles,
    int CurrentPage,
    int TotalPages,
    int TotalItems
);