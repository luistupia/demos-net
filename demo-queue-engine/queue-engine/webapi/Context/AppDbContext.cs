using webapi.Models;

namespace webapi.Context;

public class AppDbContext(DbContextOptions<AppDbContext> opts) : DbContext(opts)
{
    public DbSet<QueueMessage> QueueMessages { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<QueueMessage>(b =>
        {
            b.Property(e => e.Id)
                .HasColumnType("TEXT");
        });
        
        base.OnModelCreating(modelBuilder);
    }

}