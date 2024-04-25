using System.ComponentModel.DataAnnotations;

namespace Auction.Presentation.Contracts.Users;

public record LoginUserRequest(
    [Required] string Email,
    [Required] string Password);
