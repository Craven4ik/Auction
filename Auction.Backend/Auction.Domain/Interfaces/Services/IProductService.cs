using Auction.Core.Models;

namespace Auction.Domain.Interfaces.Services;

public interface IProductService
{
    Task<List<ProductEntity>> Get();
    Task<ProductEntity?> GetById(Guid id);
    Task Add(ProductEntity product);
    Task Update(Guid id, ProductEntity product);
    Task Delete(Guid id);
}
