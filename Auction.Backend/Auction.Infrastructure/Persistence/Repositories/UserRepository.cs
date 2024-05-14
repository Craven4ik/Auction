using Auction.Core.Models;
using Auction.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Auction.Infrastructure.Persistence.Repositories;

public class UserRepository(AuctionDbContext auctionDbContext) : IUserRepository
{
    private readonly AuctionDbContext _context = auctionDbContext;

    public async Task<List<UserEntity>> Get()
        => await _context.Users.AsNoTracking().ToListAsync();

    public async Task<UserEntity?> GetById(Guid id)
        => await _context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id.Equals(id));

    public async Task<UserEntity?> GetByEmail(string email)
        => await _context.Users.AsNoTracking()
        .FirstOrDefaultAsync(x => x.Email.Equals(email)) ?? throw new Exception("User not found");

    public async Task Add(string name, string email, string passwordHash)
    {
        var userEntity = new UserEntity()
        {
            Id = Guid.NewGuid(),
            Name = name,
            Email = email,
            PasswordHash = passwordHash,
            Balance = 0,
            Role = 0
        };

        await _context.Users.AddAsync(userEntity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateBalance(Guid id, decimal amount)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Id.Equals(id));

        if (user is not null)
        {
            user.Balance += amount;
            await _context.SaveChangesAsync();
        }
    }

    public async Task Update(Guid id, string name, string email, string passwordHash, decimal balance)
        => await _context.Users
            .Where(x => x.Id.Equals(id))
            .ExecuteUpdateAsync(s => s
                .SetProperty(c => c.Name, name)
                .SetProperty(c => c.Email, email)
                .SetProperty(c => c.PasswordHash, passwordHash)
                .SetProperty(c => c.Balance, balance));

    public async Task Delete(Guid id)
        => await _context.Users
            .Where(x => x.Id.Equals(id))
            .ExecuteDeleteAsync();

    public async Task<bool> IsFreeEmail(string email)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Email.Equals(email));

        if (user is not null)
            return true;

        return false;
    }

    public async Task<decimal> GetUserBalance(Guid id)
    {
        var user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id.Equals(id));

        if (user is not null)
            return user.Balance;

        return 0;
    }

    public async Task<decimal> GetFreeUserBalance(Guid id)
    {
        var user = await _context.Users.AsNoTracking().Include(x => x.Bets)
            .FirstOrDefaultAsync(x => x.Id.Equals(id));

        if (user is null)
            return 0;

        if (user.Bets.Count == 0)
            return user.Balance;

        return user.Balance - user.Bets.Sum(b => b.Offer);
    }

    public async Task<string> GetUserName(Guid id)
    {
        var user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id.Equals(id));

        if (user is not null)
            return user.Name;

        return string.Empty;
    }
}
