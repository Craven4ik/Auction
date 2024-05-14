using Auction.Core.Models;

namespace Auction.Domain.Interfaces.Services;

public interface IUserService
{
    Task<string> Login(string email, string password);
    Task Register(string userName, string email, string password);
    Task<UserEntity?> GetById(string token);
    Task UpdateBalance(string token, decimal amount);
    bool CheckToken(string token);
    Task<bool> IsFreeEmail(string email);
    Task<decimal> GetUserBalance(string token);
    Task<decimal> GetFreeUserBalance(string token);
    Task<string> GetUserName(Guid id);
    bool CheckProductOwner(Guid id, string token);
    Guid GetUserIdFromToken(string token);
}
