namespace VehicleControl.API.Exceptions;

public class UnauthorizedException: CustomAppException
{
    public UnauthorizedException(string errorMessage) : base(errorMessage)
    {
    }
}
