using System.ComponentModel.DataAnnotations;

namespace Auction.Presentation.Contracts.Bets;

public record UpdateBetRequest(
    [Required] Guid UserId,
    [Required] decimal Offer);
