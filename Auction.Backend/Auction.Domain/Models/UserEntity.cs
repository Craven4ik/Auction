using Auction.Core.Enums;

namespace Auction.Core.Models;

public class UserEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public decimal Balance { get; set; } = 0;
    public List<BetEntity> Bets { get; set; } = [];
    public List<ProductEntity> Products { get; set; } = [];
    public List<ProductEntity> Purchases { get; set; } = [];
    public UserRole Role { get; set; } = 0;
}
