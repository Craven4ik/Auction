namespace Auction.Presentation.Contracts.Products;

public record AddProductRequest
{
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public decimal BetStep { get; init; }
    public decimal StartPrice { get; init; }
    public decimal? BuyOutPrice { get; init; }
    public DateOnly StartTime { get; init; }
    public DateOnly EndTime { get; init; }
}
