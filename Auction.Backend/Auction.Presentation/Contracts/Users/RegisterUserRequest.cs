using System.ComponentModel.DataAnnotations;

namespace Auction.Presentation.Contracts.Users;

public record RegisterUserRequest(
    [Required] string UserName,
    [Required] string Email,
    [Required] string Password);
