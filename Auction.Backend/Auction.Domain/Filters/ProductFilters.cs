using Auction.Core.Enums;

namespace Auction.Presentation.Contracts.Products;

public record ProductFilters(
    ProductState? Status,
    decimal? StartPrice,
    decimal? MaxBid,
    decimal? MaxBuyout,
    DateOnly? StartDate,
    DateOnly? EndDate);
