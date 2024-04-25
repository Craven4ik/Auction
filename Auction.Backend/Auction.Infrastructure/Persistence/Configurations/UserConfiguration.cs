using Auction.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Auction.Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<UserEntity>
{
    public void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        builder.HasKey(u => u.Id);

        builder
            .HasMany(u => u.Bets)
            .WithOne(b => b.User)
            .HasForeignKey(b => b.UserId);

        builder
            .HasMany(u => u.Products)
            .WithOne(p => p.Owner)
            .HasForeignKey(p => p.OwnerId);

        builder
            .HasMany(u => u.Purchases)
            .WithOne(p => p.Buyer)
            .HasForeignKey(p => p.BuyerId);
    }
}
