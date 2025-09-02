using VehicleControl.API.Domain.Interfaces;
using VehicleControl.API.Services.Authentication;
using VehicleControl.API.Services.Cryptography;
using VehicleControl.API.Services.Entities;
using VehicleControl.API.Services.Mapping;

namespace VehicleControl.API.Extensions;

public static class ServicesExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration config)
    {
        RegisterMapping(services);
        RegisterEncryptService(services, config);
        RegisterAuthenticationServices(services);
        RegisterEntityServices(services);
        return services;
    }
    private static void RegisterEntityServices(IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IVehicleService, VehicleService>();
    }
    private static void RegisterMapping(IServiceCollection services)
    {
        services.AddScoped<IUserMapper, UserMapper>();
        services.AddScoped<IVehicleMapper, VehicleMapper>();
    }
    private static void RegisterEncryptService(IServiceCollection services, IConfiguration config)
    {
        services.AddScoped<IEncrypter>(_ => new EncryptService(config));
    }
    
    private static void RegisterAuthenticationServices(IServiceCollection services)
    {
        services.AddScoped<IJwtService, JwtService>();
    }
}
