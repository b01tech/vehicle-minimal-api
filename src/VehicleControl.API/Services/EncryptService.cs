using System.Security.Cryptography;
using System.Text;
using VehicleControl.API.Domain.Interfaces;

namespace VehicleControl.API.Services;

internal class EncryptService : IEncrypter
{
    private readonly string _secretKey;

    public EncryptService(IConfiguration config)
    {
        _secretKey = config["Settings:SecretKey"]!;
    }

    public string Encrypt(string input)
    {
        var password = $"{input} + {_secretKey}";
        var bytes = Encoding.UTF8.GetBytes(password);
        var hash = SHA512.HashData(bytes);
        return Convert.ToBase64String(hash);
    }

    public bool Validate(string input, string hash)
    {
        var hashOfInput = Encrypt(input);
        return hashOfInput.Equals(hash);
    }
}
