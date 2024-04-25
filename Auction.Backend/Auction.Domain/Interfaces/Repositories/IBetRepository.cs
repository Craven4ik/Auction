using Auction.Core.Models;

namespace Auction.Domain.Interfaces.Repositories;

public interface IBetRepository
{
    Task<List<BetEntity>> Get();
    Task<BetEntity?> GetById(Guid id);
    Task Add(BetEntity bet);
    Task Update(Guid Id, Guid userId, decimal offer);
    Task Delete(Guid id);
}
