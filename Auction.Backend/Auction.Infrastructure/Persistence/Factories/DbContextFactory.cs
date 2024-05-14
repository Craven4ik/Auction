using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Auction.Infrastructure.Persistence.Factory;

public class DbContextFactory(IConfiguration configuration)
{
    private readonly string _connectionString = configuration.GetConnectionString(nameof(AuctionDbContext));

    public AuctionDbContext CreateDbContext()
    {
        var optionsBuilder = new DbContextOptionsBuilder<AuctionDbContext>();
        optionsBuilder.UseNpgsql(_connectionString);

        return new AuctionDbContext(optionsBuilder.Options);
    }
}
