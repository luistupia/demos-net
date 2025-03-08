using Catalogo.Domain.Abstractions;
using Catalogo.Domain.Products;
using Catalogo.Infraestructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Catalogo.Infraestructure;

public static class DependencyInjection
{
    public static void AddInfraestructure(this IServiceCollection services,IConfiguration configuration)
    {
        services.AddDbContext<CatalogoDbContext>(options =>
        {
            options.LogTo(Console.WriteLine, new[] { DbLoggerCategory.Database.Command.Name }, 
            Microsoft.Extensions.Logging.LogLevel.Information).EnableSensitiveDataLogging();

            options.UseSqlite(configuration.GetConnectionString("SQliteProduct")).UseSnakeCaseNamingConvention();
        });

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<CatalogoDbContext>());
        services.AddScoped<IProductRepository, ProductRepository>();
    }
}
