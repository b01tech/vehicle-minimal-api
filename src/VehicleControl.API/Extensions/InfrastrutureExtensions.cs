
using Microsoft.EntityFrameworkCore;
using VehicleControl.API.Domain.Interfaces;
using VehicleControl.API.Infra.Data;
using VehicleControl.API.Infra.Repositories;

namespace VehicleControl.API.Extensions;

public static class InfrastrutureExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        AddDbContext(services, config);
        AddRepositories(services);
        return services;
    }
    private static void AddDbContext(IServiceCollection services, IConfiguration config)
    {
        var connectionString = config.GetConnectionString("MySqlString");
        services.AddDbContext<AppDbContext>(opt =>
            opt.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
    }
    private static void AddRepositories(IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IVehicleRepository, VehicleRepository>();
    }
}
