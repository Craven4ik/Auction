using Auction.Core.Models;
using Auction.Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Auction.Infrastructure.Persistence;

public class AuctionDbContext(DbContextOptions<AuctionDbContext> options)
    : DbContext(options)
{
    public DbSet<UserEntity> Users { get; set; }
    public DbSet<BetEntity> Bets { get; set; }
    public DbSet<ProductEntity> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new ProductConfiguration());
        modelBuilder.ApplyConfiguration(new BetConfiguration());

        base.OnModelCreating(modelBuilder);
    }
}
