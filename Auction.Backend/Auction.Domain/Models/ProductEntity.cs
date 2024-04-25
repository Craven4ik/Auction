using Auction.Core.Enums;

namespace Auction.Core.Models;

public class ProductEntity
{
    public Guid Id { get; set; }
    public Guid OwnerId { get; set; }
    public UserEntity? Owner { get; set; }
    public Guid? BuyerId { get; set; }
    public UserEntity? Buyer { get; set; }
    public Guid? BetId { get; set; }
    public BetEntity? Bet { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal StartPrice { get; set; } = 0;
    public decimal? BuyOutPrice { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public ProductState State { get; set; }
}
