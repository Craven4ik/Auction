using Auction.Core.Enums;

namespace Auction.Presentation.Contracts.Products;

public record GetProductResponse
{
    public Guid Id { get; init; }
    public Guid OwnerId { get; init; }
    public Guid? BuyerId { get; init; }
    public Guid? BetId { get; init; }
    public Guid? MaxBetOwnerId { get; init; }
    public ProductState State { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public decimal BetStep { get; init; }
    public decimal? MaxBet { get; init; }
    public decimal StartPrice { get; init; }
    public decimal? BuyOutPrice { get; init; }
    public DateOnly StartTime { get; init; }
    public DateOnly EndTime { get; init; }
}
