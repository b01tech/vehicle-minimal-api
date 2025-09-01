namespace VehicleControl.API.Domain.Interfaces;

public interface IEncrypter
{
    string Encrypt(string input);
    bool Validate(string input, string hash);
}
