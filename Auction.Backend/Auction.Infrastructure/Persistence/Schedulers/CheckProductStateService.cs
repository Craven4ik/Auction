using Auction.Core.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Auction.Infrastructure.Persistence.Schedulers;

public class CheckProductStateService(IServiceProvider serviceProvider) : BackgroundService
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    public async Task RunAsync(CancellationToken stoppingToken)
    {
        await ExecuteAsync(stoppingToken);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await using var scope = _serviceProvider.CreateAsyncScope();
        var context = scope.ServiceProvider.GetRequiredService<AuctionDbContext>();

        while (!stoppingToken.IsCancellationRequested)
        {
            var expiredProducts = context.Products
                .Where(x => x.EndTime <= DateOnly.FromDateTime(DateTime.UtcNow)
                    && x.State != ProductState.Sold && x.State != ProductState.Refused).Include(x => x.Bet);

            foreach (var product in expiredProducts)
            {
                if (product.Bet is not null)
                {
                    var user = await context.Users.FirstOrDefaultAsync(x => x.Id.Equals(product.BuyerId), cancellationToken: stoppingToken);

                    if (user is not null)
                        user.Balance -= product.Bet.Offer;

                    product.State = ProductState.Sold;
                    product.BuyerId = product.Bet.UserId;
                }
                else
                {
                    product.State = ProductState.Refused;
                    product.BuyerId = null;
                }
            }

            var preparingProducts = context.Products
                .Where(x => x.StartTime > DateOnly.FromDateTime(DateTime.UtcNow)
                    && x.State != ProductState.Preparing && x.State != ProductState.Refused);

            foreach (var product in preparingProducts)
            {
                product.State = ProductState.Preparing;
            }

            var sellingProducts = context.Products
                .Where(x => x.StartTime <= DateOnly.FromDateTime(DateTime.UtcNow)
                    && x.EndTime > DateOnly.FromDateTime(DateTime.UtcNow));

            foreach (var product in sellingProducts)
            {
                if (product.State != ProductState.Sold)
                    product.State = ProductState.ForSale;
            }

            await context.SaveChangesAsync();
            await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
        }
    }
}
