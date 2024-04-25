using Auction.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Auction.Infrastructure.Persistence.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<ProductEntity>
{
    public void Configure(EntityTypeBuilder<ProductEntity> builder)
    {
        builder.HasKey(p => p.Id);

        builder
            .HasOne(p => p.Owner)
            .WithMany(u => u.Products)
            .HasForeignKey(p => p.OwnerId);

        builder
            .HasOne(p => p.Buyer)
            .WithMany(u => u.Purchases)
            .HasForeignKey(p => p.BuyerId);

        builder
            .HasOne(p => p.Bet)
            .WithOne(b => b.Product)
            .HasForeignKey<BetEntity>(b => b.ProductId);

        builder.Property(p => p.BuyerId).IsRequired(false);

        builder.Property(p => p.BetId).IsRequired(false);

        builder.Property(p => p.BuyOutPrice).IsRequired(false);
    }
}
