using Auction.Core.Enums;
using Auction.Core.Models;
using Auction.Infrastructure.Persistence;
using Auction.Infrastructure.Persistence.Schedulers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Auction.Tests;
public class CheckProductStateServiceTests
{
    private ServiceProvider _serviceProvider;
    private CheckProductStateService _checkProductStateService;

    public CheckProductStateServiceTests()
    {
        var services = new ServiceCollection();

        services.AddDbContext<AuctionDbContext>(options =>
            options.UseInMemoryDatabase("TestDatabase"), ServiceLifetime.Transient);

        services.AddTransient<AuctionDbContext>();

        _serviceProvider = services.BuildServiceProvider();
        _checkProductStateService = new CheckProductStateService(_serviceProvider);
    }

    [Fact]
    public async Task ExecuteAsync_UpdatesProductStates()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AuctionDbContext>();

        var products = new List<ProductEntity>
        {
            new() { Id = Guid.NewGuid(), State = ProductState.Preparing, StartTime = DateOnly.FromDateTime(DateTime.UtcNow.AddHours(1)), EndTime = DateOnly.FromDateTime(DateTime.UtcNow.AddHours(2)), Bet = null },
            new() { Id = Guid.NewGuid(), State = ProductState.Preparing, StartTime = DateOnly.FromDateTime(DateTime.UtcNow.AddHours(-1)), EndTime = DateOnly.FromDateTime(DateTime.UtcNow), Bet = new BetEntity { Id = Guid.NewGuid(), UserId = Guid.Parse("123e4567-e89b-12d3-a456-426614174000"), Offer = 100 } },
            new() { Id = Guid.NewGuid(), State = ProductState.ForSale, StartTime = DateOnly.FromDateTime(DateTime.UtcNow.AddHours(-2)), EndTime = DateOnly.FromDateTime(DateTime.UtcNow.AddHours(-1)), Bet = null }
        };

        context.Products.AddRange(products);
        await context.SaveChangesAsync();

        _checkProductStateService.RunAsync(CancellationToken.None);

        await Task.Delay(TimeSpan.FromSeconds(10));

        await _checkProductStateService.StopAsync(CancellationToken.None);

        foreach (var product in products)
        {
            var updatedProduct = await context.Products.FindAsync(product.Id);
            if (product.StartTime > DateOnly.FromDateTime(DateTime.UtcNow))
            {
                Assert.Equal(ProductState.Preparing, updatedProduct.State);
            }
        }
    }
}