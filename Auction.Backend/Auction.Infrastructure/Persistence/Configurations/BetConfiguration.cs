using Auction.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Auction.Infrastructure.Persistence.Configurations;

public class BetConfiguration : IEntityTypeConfiguration<BetEntity>
{
    public void Configure(EntityTypeBuilder<BetEntity> builder)
    {
        builder.HasKey(b => b.Id);

        builder
            .HasOne(b => b.User)
            .WithMany(u => u.Bets)
            .HasForeignKey(b => b.UserId);

        builder
            .HasOne(b => b.Product)
            .WithOne(p => p.Bet)
            .HasForeignKey<BetEntity>(b => b.ProductId);
    }
}
