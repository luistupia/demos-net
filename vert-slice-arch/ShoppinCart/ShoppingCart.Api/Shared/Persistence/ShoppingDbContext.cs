using Microsoft.EntityFrameworkCore;
using ShoppingCart.Api.Shared.Domain.Entities;

namespace ShoppingCart.Api.Shared.Persistence;

public sealed class ShoppingDbContext(DbContextOptions<ShoppingDbContext> options) : DbContext(options)
{
    public DbSet<Item> Items => Set<Item>();
    public DbSet<Cart> Carts => Set<Cart>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var cart = Cart.Create("INIT_CODE", "SYSTEM");
        var items = new List<Item>
        {
            Item.Create("ITEM_CODE","",10,15.00M,"PHONE X","MODERNO",cart.Id),
            Item.Create("ITEM_AM","",1,13.00M,"MACBOOK","XXXXXX",cart.Id),
        };
        
        var cartEmpty = Cart.Create("INIT_PASS", "SYSTEM");
        
        modelBuilder.Entity<Cart>().HasData(cart);
        modelBuilder.Entity<Item>().HasData(items);
        modelBuilder.Entity<Cart>().HasData(cartEmpty);
        
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ShoppingDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}