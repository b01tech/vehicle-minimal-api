
using Microsoft.EntityFrameworkCore;
using VehicleControl.API.Infra.Data;

namespace VehicleControl.API.Extensions;

public static class InfrastrutureExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        AddDbContext(services, config);
        return services;
    }

    private static void AddDbContext(IServiceCollection services, IConfiguration config)
    {
        var connectionString = config.GetConnectionString("MySqlString");
        services.AddDbContext<AppDbContext>(opt =>
            opt.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
    }
}
