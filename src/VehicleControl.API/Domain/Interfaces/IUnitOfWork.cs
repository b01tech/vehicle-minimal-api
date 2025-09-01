namespace VehicleControl.API.Domain.Interfaces;

public interface IUnitOfWork
{
    Task CommitAsync();
}
