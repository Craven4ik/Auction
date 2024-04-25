using Auction.Core.Enums;
using Auction.Core.Models;

namespace Auction.Domain.Interfaces.Repositories;

public interface IProductRepository
{
    Task<List<ProductEntity>> Get();
    Task<ProductEntity?> GetById(Guid id);
    Task<List<ProductEntity>> GetByFilter(ProductState state);
    Task Add(ProductEntity product);
    Task Update(Guid id, ProductEntity product);
    Task Delete(Guid id);
}
