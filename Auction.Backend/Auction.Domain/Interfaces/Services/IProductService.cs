using Auction.Core.Models;
using Auction.Presentation.Contracts.Products;

namespace Auction.Domain.Interfaces.Services;

public interface IProductService
{
    Task<List<ProductEntity>> Get();
    Task<ProductEntity?> GetById(Guid id);
    Task<List<ProductEntity>> GetByFilter(ProductFilters filters);
    Task<List<ProductEntity>> GetByOwner(string token);
    Task<List<ProductEntity>> GetByBuyer(string token);
    Task<List<ProductEntity>> GetByBetOwner(string token);
    Task Add(string token, ProductEntity product);
    Task Update(Guid id, ProductEntity product);
    Task Delete(Guid id);
    Task DoBetForProduct(Guid id, string token, decimal offer);
    Task BuyOutProduct(Guid productId, string token);
}
