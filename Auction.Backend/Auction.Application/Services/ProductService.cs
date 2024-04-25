using Auction.Core.Models;
using Auction.Domain.Interfaces.Repositories;
using Auction.Domain.Interfaces.Services;

namespace Auction.Application.Services;

public class ProductService(IProductRepository productRepository) : IProductService
{
    private readonly IProductRepository _repository = productRepository;

    public async Task<List<ProductEntity>> Get()
        => await _repository.Get();

    public async Task<ProductEntity?> GetById(Guid id)
        => await _repository.GetById(id);

    public async Task Add(ProductEntity product)
        => await _repository.Add(product);

    public async Task Update(Guid id, ProductEntity product)
        => await _repository.Update(id, product);

    public async Task Delete(Guid id)
        => await _repository.Delete(id);
}
