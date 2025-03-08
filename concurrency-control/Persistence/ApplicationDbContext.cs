using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)  
    {

    }
    
    public DbSet<BankAccount> BankAccounts { get; set; }   

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BankAccount>()
            .Property(x => x.RowVersion).IsRowVersion();

        modelBuilder.Entity<BankAccount>()
            .Property(x => x.Balance)
            .HasColumnType("decimal(18,2)");
        
        base.OnModelCreating(modelBuilder);
    }
}