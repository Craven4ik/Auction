namespace Auction.Core.Models;

public class BetEntity
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public UserEntity? User { get; set; }
    public Guid ProductId { get; set; }
    public ProductEntity? Product { get; set; }
    public decimal Offer { get; set; } = 0;
}
