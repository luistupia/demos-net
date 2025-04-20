using Microsoft.EntityFrameworkCore;
using webapi.Core.Infrastructure.Persistence;

namespace webapi.Core.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        var conn = config.GetConnectionString("DefaultConnection") ?? "Data Source=app.db";
        services.AddDbContext<ApiDbContext>(opt => opt.UseSqlite(conn));
        return services;
    }
}