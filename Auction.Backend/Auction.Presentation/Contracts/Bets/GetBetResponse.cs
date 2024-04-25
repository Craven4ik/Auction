namespace Auction.Presentation.Contracts.Bets;

public record GetBetResponse(
    Guid Id,
    Guid UserId,
    Guid ProductId,
    decimal Offer);
