using System;
using Catalogo.Infraestructure;
using Microsoft.EntityFrameworkCore;

namespace Catalogo.Api.Extensions;

public static class ApplicationBuilderExtensions
{
    public static async void ApplyMigrations(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var services = scope.ServiceProvider;
        var loggerFactory = services.GetRequiredService<ILoggerFactory>();
        try
        {
            var context = services.GetRequiredService<CatalogoDbContext>();
            await context.Database.MigrateAsync();
        }
        catch (Exception ex)
        {
            var logger = loggerFactory.CreateLogger<Program>();
            logger.LogError(ex, "An error occurred creating the DB.");
        }
    }
}
