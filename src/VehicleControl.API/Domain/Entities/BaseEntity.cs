namespace VehicleControl.API.Domain.Entities;

public abstract class BaseEntity
{
    public long Id { get; private set; }
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; private set; } = DateTime.UtcNow;
    public bool Active { get; private set; } = true;

    public void UpdateTimeStamp()
    {
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetAsActive()
    {
        Active = true;
        UpdateTimeStamp();
    }
    public void SetAsInactive()
    {
        Active = false;
        UpdateTimeStamp();
    }
}
