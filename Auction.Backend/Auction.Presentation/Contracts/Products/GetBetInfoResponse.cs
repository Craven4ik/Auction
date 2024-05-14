namespace Auction.Presentation.Contracts.Products;

public class GetBetInfoResponse
{
    public Guid ProductId { get; init; }
    public string Name { get; init; } = string.Empty;
    public decimal Offer { get; init; }
    public DateOnly StartTime { get; init; }
    public DateOnly EndTime { get; init; }
}
