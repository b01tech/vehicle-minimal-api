namespace VehicleControl.API.Domain.Interfaces;

public interface IMapper
{
    T Map<T>(object source);
}
