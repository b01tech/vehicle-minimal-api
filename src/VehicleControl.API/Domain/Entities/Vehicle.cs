namespace VehicleControl.API.Domain.Entities;

public class Vehicle: BaseEntity
{
    public string LicencePlate { get; private set; } = string.Empty;
    public string Model { get; private set; } = string.Empty;
    public int Year { get; private set; }

    protected Vehicle() { }

    public Vehicle(string licencePlate, string model, int year)
    {
        LicencePlate = licencePlate;
        Model = model;
        Year = year;
        Validate();
    }

    public void Update(string licencePlate, string model, int year)
    {
        LicencePlate = licencePlate;
        Model = model;
        Year = year;
        Validate();
        UpdateTimeStamp();
    }

    private void Validate()
    {

    }
}
