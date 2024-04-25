using Auction.Core.Models;
using Auction.Domain.Interfaces.Repositories;
using Auction.Domain.Interfaces.Services;

namespace Auction.Application.Services;

public class BetService(IBetRepository betRepository) : IBetService
{
    private readonly IBetRepository _betRepository = betRepository;

    public async Task<List<BetEntity>> Get()
        => await _betRepository.Get();

    public async Task<BetEntity?> GetById(Guid id)
        => await _betRepository.GetById(id);

    public async Task Add(BetEntity bet)
        => await _betRepository.Add(bet);

    public async Task Update(Guid id, Guid userId, decimal offer)
        => await _betRepository.Update(id, userId, offer);

    public async Task Delete(Guid id)
        => await _betRepository.Delete(id);
}
