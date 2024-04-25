using System.ComponentModel.DataAnnotations;

namespace Auction.Presentation.Contracts.Products;

public record AddProductRequest(
    [Required] Guid OwnerId,
    [Required] string Name,
    [Required] string Description,
    [Required] decimal StartPrice,
    decimal? BuyOutPrice,
    [Required] DateTime StartTime,
    [Required] DateTime EndTime);
