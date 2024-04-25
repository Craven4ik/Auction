using System.ComponentModel.DataAnnotations;

namespace Auction.Presentation.Contracts.Bets;

public record AddBetRequest(
    [Required] Guid UserId,
    [Required] Guid ProductId,
    [Required] decimal Offer);
