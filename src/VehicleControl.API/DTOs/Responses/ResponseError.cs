namespace VehicleControl.API.DTOs.Responses;

public class ResponseError
{
    public IList<string> ErrorsMessages { get; set; }
    public ResponseError(IList<string> errors) => ErrorsMessages = errors;

    public ResponseError(string errorMessage)
    {
        ErrorsMessages = [errorMessage];
    }
}