using Bogus;
using Catalogo.Domain.Categories;
using Catalogo.Domain.Products;
using Catalogo.Infraestructure;
using Microsoft.EntityFrameworkCore;

namespace Catalogo.Api.Extensions;

public static class DataSeed
{
    public static async Task SeedCatalogoProduct(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var service = scope.ServiceProvider;
        var loggerFactory = service.GetRequiredService<ILoggerFactory>();

        try
        {
            var context = service.GetRequiredService<CatalogoDbContext>();
            if (!context.Set<Category>().Any())
            {
                var categoryComputers = Category.Create("Computadora");
                var categoryTelefono = Category.Create("Telefono");
                context.Add(categoryComputers);
                context.Add(categoryTelefono);
                context.AddRange(new List<Category> {categoryTelefono,categoryComputers});
                
                await context.SaveChangesAsync();
            }

            if (!context.Set<Product>().Any())
            {
                var computadora = await context.Set<Category>()
                    .Where(x => x.Name == "Computadora")
                    .FirstOrDefaultAsync();
                
                var telefono = await context.Set<Category>()
                    .Where(x => x.Name == "Telefono")
                    .FirstOrDefaultAsync();

                var faker = new Faker();
                List<Product> products = new();

                for (var i = 0; i <= 10; i++)
                {
                    products.Add(
                        Product.Create(
                            faker.Commerce.Product(),
                            100.00m,
                            faker.Commerce.ProductDescription(),
                            $"img_{i}.jpg",
                            faker.Hashids.Encode(10000,i*1000),
                            i > 5 ? computadora!.Id : telefono!.Id)
                    );
                }
                context.AddRange(products);
                await context.SaveChangesAsync();
            }
        }
        catch (Exception e)
        {
            var logger = loggerFactory.CreateLogger<CatalogoDbContext>();
            logger.LogError(e, "An error occurred while seeding the database.");
        }
    }
}