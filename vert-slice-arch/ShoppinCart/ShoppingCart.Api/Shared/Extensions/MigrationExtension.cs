using Microsoft.EntityFrameworkCore;
using ShoppingCart.Api.Shared.Persistence;

namespace ShoppingCart.Api.Shared.Extensions;

public static class MigrationExtension
{
    public static async void ApplyMigrations(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var services = scope.ServiceProvider;
        var loggerFactory = services.GetRequiredService<ILoggerFactory>();
        try
        {
            var context = services.GetRequiredService<ShoppingDbContext>();
            await context.Database.MigrateAsync();
        }
        catch (Exception e)
        {
            var logger = loggerFactory.CreateLogger<Program>();
            logger.LogError(e, "Error en la migracion");
        }
    }
}