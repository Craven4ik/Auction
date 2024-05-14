namespace Auction.Presentation.Contracts.Users;

public record GetUserInfoResponse
{
    public string Name { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public decimal Balance { get; init; }
    public decimal FreeBalance { get; set; }
    public DateOnly RegistrationDate { get; init; }
}
