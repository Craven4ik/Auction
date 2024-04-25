using Auction.Core.Models;

namespace Auction.Domain.Interfaces.Services;

public interface IBetService
{
    Task<List<BetEntity>> Get();
    Task<BetEntity?> GetById(Guid id);
    Task Add(BetEntity bet);
    Task Update(Guid id, Guid userId, decimal offer);
    Task Delete(Guid id);
}
