namespace Auction.Presentation.Contracts.Users;

public record UpdateBalanceRequest
{
    public decimal Amount { get; init; }
}
