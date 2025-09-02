namespace VehicleControl.API.Exceptions;

public abstract class CustomAppException : System.Exception
{
    public IList<string> ErrorMessages { get; set; }
    public CustomAppException(IList<string> errorMessages)
    {
        ErrorMessages = errorMessages;
    }

    public CustomAppException(string errorMessage)
    : this([errorMessage]) { }
}
