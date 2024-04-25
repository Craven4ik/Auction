using Auction.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace Auction.Presentation.Contracts.Products;

public record UpdateProductRequest(
    [Required] Guid OwnerId,
    Guid? BuyerId,
    Guid? BetId,
    ProductState State,
    [Required] string Name,
    [Required] string Description,
    [Required] decimal StartPrice,
    decimal? BuyOutPrice,
    [Required] DateTime StartTime,
    [Required] DateTime EndTime);
