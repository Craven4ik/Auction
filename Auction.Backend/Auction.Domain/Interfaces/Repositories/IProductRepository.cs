using Auction.Core.Models;
using Auction.Presentation.Contracts.Products;

namespace Auction.Domain.Interfaces.Repositories;

public interface IProductRepository
{
    Task<List<ProductEntity>> Get();
    Task<ProductEntity?> GetById(Guid id);
    Task<List<ProductEntity>> GetByFilter(ProductFilters filters);
    Task<List<ProductEntity>> GetByOwner(Guid id);
    Task<List<ProductEntity>> GetByBuyer(Guid id);
    Task<List<ProductEntity>> GetByBetOwner(Guid id);
    Task Add(ProductEntity product);
    Task Update(Guid id, ProductEntity product);
    Task Delete(Guid id);
    Task DoBetForProduct(Guid id, Guid userId, decimal offer);
    Task BuyOutProduct(Guid productId, Guid userId);
}
