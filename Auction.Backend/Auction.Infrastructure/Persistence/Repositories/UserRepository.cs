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
}
