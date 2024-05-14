namespace Auction.Presentation.Contracts.Products;

public record AddBetForProductRequest
{
    public decimal Offer {  get; init; }
}
