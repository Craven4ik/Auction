using Auction.Core.Enums;
using Auction.Core.Models;
using Auction.Domain.Interfaces.Repositories;
using Auction.Presentation.Contracts.Products;
using Microsoft.EntityFrameworkCore;

namespace Auction.Infrastructure.Persistence.Repositories;

public class ProductRepository(AuctionDbContext auctionDbContext) : IProductRepository
{
    private readonly AuctionDbContext _context = auctionDbContext;

    public async Task<List<ProductEntity>> Get()
        => await _context.Products.AsNoTracking().ToListAsync();

    public async Task<ProductEntity?> GetById(Guid id)
        => await _context.Products.AsNoTracking()
            .Include(x => x.Bet).FirstOrDefaultAsync(x => x.Id.Equals(id));

    public async Task<List<ProductEntity>> GetByFilter(ProductFilters filters)
    {
        var query = _context.Products.AsNoTracking().Include(x => x.Bet).AsQueryable();

        if (filters.Status is not null)
            query = query.Where(x => x.State == filters.Status);

        if (filters.StartPrice is not null)
            query = query.Where(x => x.StartPrice <= filters.StartPrice);

        if (filters.MaxBuyout is not null)
            query = query.Where(x => x.BuyOutPrice <= filters.MaxBuyout);

        if (filters.StartDate is not null)
            query = query.Where(x => x.StartTime >= filters.StartDate);

        if (filters.EndDate is not null)
            query = query.Where(x => x.EndTime <= filters.EndDate);

        if (filters.MaxBid is not null)
            query = query.Where(x => x.Bet != null && x.Bet.Offer <= filters.MaxBid);

        return await query.ToListAsync();
    }

    public async Task<List<ProductEntity>> GetByOwner(Guid id)
        => await _context.Products.AsNoTracking().Include(x => x.Bet)
            .Where(x => x.OwnerId.Equals(id)).ToListAsync();
    
    public async Task<List<ProductEntity>> GetByBuyer(Guid id)
        => await _context.Products.AsNoTracking().Include(x => x.Bet)
            .Where(x => x.BuyerId.Equals(id)).ToListAsync();

    public async Task<List<ProductEntity>> GetByBetOwner(Guid id)
        => await _context.Products.AsNoTracking().Include(x => x.Bet)
            .Where(x => x.Bet != null && x.Bet.UserId.Equals(id)).ToListAsync();
    public async Task Add(ProductEntity product)
    {
        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();
    }

    public async Task Update(Guid id, ProductEntity product)
        => await _context.Products
            .Where(x => x.Id.Equals(id))
            .ExecuteUpdateAsync(s => s
                .SetProperty(c => c.Name, product.Name)
                .SetProperty(c => c.StartPrice, product.StartPrice)
                .SetProperty(c => c.Description, product.Description)
                .SetProperty(c => c.BetStep, product.BetStep)
                .SetProperty(c => c.BuyOutPrice, product.BuyOutPrice)
                .SetProperty(c => c.StartTime, product.StartTime)
                .SetProperty(c => c.EndTime, product.EndTime));

    public async Task Delete(Guid id)
        => await _context.Products
            .Where(x => x.Id.Equals(id))
            .ExecuteDeleteAsync();

    public async Task DoBetForProduct(Guid id, Guid userId, decimal offer)
    {
        var product = await _context.Products
            .Include(x => x.Bet).FirstOrDefaultAsync(x => x.Id.Equals(id));

        if (product is null)
            return;

        if (product.Bet is null)
        {
            var bet = new BetEntity()
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Offer = offer
            };

            await _context.Bets.AddAsync(bet);

            product.BetId = bet.Id;
            product.Bet = bet;
        }
        else
        {
            product.Bet.UserId = userId;
            product.Bet.Offer = offer;
        }

        await _context.SaveChangesAsync();
    }

    public async Task BuyOutProduct(Guid productId, Guid userId)
    {
        var product = await _context.Products.FirstOrDefaultAsync(x => x.Id.Equals(productId));

        var user = await _context.Users.FirstOrDefaultAsync(x => x.Id.Equals(userId));

        if (product is not null && user is not null && product.BuyOutPrice is not null)
        {
            if (product.BetId is not null)
            {
                product.BetId = null;
                await _context.Bets.Where(x => x.Id.Equals(product.BetId)).ExecuteDeleteAsync();
            }

            user.Balance -= (decimal)product.BuyOutPrice;

            product.BuyerId = userId;
            product.State = ProductState.Sold;

            await _context.SaveChangesAsync();
        }
    }
}
