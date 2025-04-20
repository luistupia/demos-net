using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShoppingCart.Api.Shared.Domain.Entities;

namespace ShoppingCart.Api.Shared.Persistence.Configurations;

internal sealed class CartConfiguration : IEntityTypeConfiguration<Cart>
{
    public void Configure(EntityTypeBuilder<Cart> builder)
    {
        builder.ToTable("shopping_carts");
        builder.HasKey(x => x.Id);
        builder.HasMany(x=>x.Items)
            .WithOne(x=>x.Cart)
            .HasForeignKey(x=>x.CartId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}