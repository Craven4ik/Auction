using Auction.Core.Enums;

namespace Auction.Presentation.Contracts.Products;

public record GetProductResponse(
    Guid id,
    Guid OwnerId,
    Guid? BuyerId,
    Guid? BetId,
    ProductState State,
    string Name,
    string Description,
    decimal StartPrice,
    decimal? BuyOutPrice,
    DateTime StartTime,
    DateTime EndTime);
