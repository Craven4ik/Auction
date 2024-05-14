using Auction.Core.Models;

namespace Auction.Domain.Interfaces.Repositories;

public interface IUserRepository
{
    Task<List<UserEntity>> Get();
    Task<UserEntity?> GetById(Guid id);
    Task<UserEntity?> GetByEmail(string email);
    Task Add(string name, string email, string passwordHash);
    Task UpdateBalance(Guid id, decimal amount);
    Task Update(Guid id, string name, string email, string passwordHash, decimal balance);
    Task Delete(Guid id);
    Task<bool> IsFreeEmail(string email);
    Task<decimal> GetUserBalance(Guid id);
    Task<decimal> GetFreeUserBalance(Guid id);
    Task<string> GetUserName(Guid id);
}
