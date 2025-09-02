namespace VehicleControl.API.Exceptions;

public class NotFoundException : CustomAppException
{
    public NotFoundException(string errorMessage) : base(errorMessage)
    {
    }
}
