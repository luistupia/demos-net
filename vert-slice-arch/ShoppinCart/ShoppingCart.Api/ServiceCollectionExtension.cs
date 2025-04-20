using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using ShoppingCart.Api.Shared.Data;
using ShoppingCart.Api.Shared.Networking.CatalogoApi;
using ShoppingCart.Api.Shared.Persistence;

namespace ShoppingCart.Api;

public static class ServiceCollectionExtension
{
    public static IServiceCollection RegisterApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<ICatalogoApiCliente, CatalogoApiCliente>();
        return services;
    }
    
    public static IServiceCollection RegisterPersistenceServices(this IServiceCollection services
        ,IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new ArgumentNullException(nameof(configuration));

        services.AddDbContext<ShoppingDbContext>(opt =>
        {
            opt.LogTo(Console.WriteLine, [
                DbLoggerCategory.Database.Command.Name
            ],LogLevel.Information).EnableSensitiveDataLogging();
            
            opt.UseSqlite(connectionString).UseCamelCaseNamingConvention();

            opt.ConfigureWarnings(w => w.Ignore(RelationalEventId.PendingModelChangesWarning));
        });

        services.AddSingleton<ISqlConnectionFactory>( _ => new SqlConnectionFactory(connectionString));
        
        return services;
    }
}