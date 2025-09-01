using VehicleControl.API.Domain.Interfaces;
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
        RegisterEntityServices(services);
        return services;
    }
    private static void RegisterEntityServices(IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
    }
    private static void RegisterMapping(IServiceCollection services)
    {
        services.AddScoped<IUserMapper, UserMapper>();
    }
    private static void RegisterEncryptService(IServiceCollection services, IConfiguration config)
    {
        services.AddScoped<IEncrypter>(_ => new EncryptService(config));
    }
}
