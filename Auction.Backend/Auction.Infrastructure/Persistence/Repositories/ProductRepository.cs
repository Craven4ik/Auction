using Auction.Core.Enums;
using Auction.Core.Models;
using Auction.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Auction.Infrastructure.Persistence.Repositories;

public class ProductRepository(AuctionDbContext auctionDbContext) : IProductRepository
{
    private readonly AuctionDbContext _context = auctionDbContext;

    public async Task<List<ProductEntity>> Get()
        => await _context.Products.AsNoTracking().ToListAsync();

    public async Task<ProductEntity?> GetById(Guid id)
        => await _context.Products.AsNoTracking().FirstOrDefaultAsync(x => x.Id.Equals(id));

    public async Task<List<ProductEntity>> GetByFilter(ProductState state)
        => await _context.Products.AsNoTracking().Where(x => x.State.Equals(state)).ToListAsync();

    public async Task Add(ProductEntity product)
    {
        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();
    }

    public async Task Update(Guid id, ProductEntity product)
        => await _context.Products
            .Where(x => x.Id.Equals(id))
            .ExecuteUpdateAsync(s => s
                .SetProperty(c => c.OwnerId, product.OwnerId)
                .SetProperty(c => c.Name, product.Name)
                .SetProperty(c => c.Description, product.Description)
                .SetProperty(c => c.StartPrice, product.StartPrice)
                .SetProperty(c => c.BuyOutPrice, product.BuyOutPrice)
                .SetProperty(c => c.StartTime, product.StartTime)
                .SetProperty(c => c.EndTime, product.EndTime));

    public async Task Delete(Guid id)
        => await _context.Products
            .Where(x => x.Id.Equals(id))
            .ExecuteDeleteAsync();
}
