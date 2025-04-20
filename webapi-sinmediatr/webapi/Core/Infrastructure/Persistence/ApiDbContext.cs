using Microsoft.EntityFrameworkCore;
using webapi.Core.Domain.Entities;

namespace webapi.Core.Infrastructure.Persistence;

public class ApiDbContext(DbContextOptions<ApiDbContext> opts) : DbContext(opts)
{
    public DbSet<Product> Products => Set<Product>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>().ToTable("Products");
        base.OnModelCreating(modelBuilder);
    }
}