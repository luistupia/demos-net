using Catalogo.Domain.Categories;
using Catalogo.Domain.Products;
using Microsoft.EntityFrameworkCore;

namespace Catalogo.Infraestructure.Configurations;

internal sealed class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("products");
        builder.HasKey(p => p.Id);
        
        builder.HasOne<Category>()
            .WithMany()
            .HasForeignKey(p => p.CategoryId);    
    }
}
