namespace VehicleControl.API.Exceptions;

public class InputInvalidException : CustomAppException
{
    public InputInvalidException(string errorMessage) : base(errorMessage)
    {
    }
}
