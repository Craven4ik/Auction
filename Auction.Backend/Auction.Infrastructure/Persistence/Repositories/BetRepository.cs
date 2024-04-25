using Auction.Core.Models;
using Auction.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Auction.Infrastructure.Persistence.Repositories;

public class BetRepository(AuctionDbContext auctionDbContext) : IBetRepository
{
    private readonly AuctionDbContext _context = auctionDbContext;

    public async Task<List<BetEntity>> Get()
        => await _context.Bets.AsNoTracking().ToListAsync();

    public async Task<BetEntity?> GetById(Guid id)
        => await _context.Bets.AsNoTracking().FirstOrDefaultAsync(x => x.Id.Equals(id));

    public async Task Add(BetEntity bet)
    {
        await _context.Bets.AddAsync(bet);
        await _context.SaveChangesAsync();
    }

    public async Task Update(Guid id, Guid userId, decimal offer)
        => await _context.Bets
            .Where(x => x.Id.Equals(id))
            .ExecuteUpdateAsync(s => s
                .SetProperty(c => c.UserId, userId)
                .SetProperty(c => c.Offer, offer));

    public async Task Delete(Guid id)
        => await _context.Bets
            .Where(x => x.Id.Equals(id))
            .ExecuteDeleteAsync();
}
