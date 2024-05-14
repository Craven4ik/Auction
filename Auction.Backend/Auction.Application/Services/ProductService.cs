using Auction.Application.Auth;
using Auction.Core.Models;
using Auction.Domain.Interfaces.Repositories;
using Auction.Domain.Interfaces.Services;
using Auction.Presentation.Contracts.Products;

namespace Auction.Application.Services;

public class ProductService(IProductRepository productRepository, IJwtProvider jwtProvider) : IProductService
{
    private readonly IProductRepository _repository = productRepository;
    private readonly IJwtProvider _jwtProvider = jwtProvider;

    public async Task<List<ProductEntity>> Get()
        => await _repository.Get();

    public async Task<ProductEntity?> GetById(Guid id)
        => await _repository.GetById(id);

    public async Task<List<ProductEntity>> GetByFilter(ProductFilters filters)
        => await _repository.GetByFilter(filters);

    public async Task<List<ProductEntity>> GetByOwner(string token)
    {
        var userId = _jwtProvider.GetUserIdFromToken(token);

        return await _repository.GetByOwner(userId);
    }

    public async Task<List<ProductEntity>> GetByBuyer(string token)
    {
        var userId = _jwtProvider.GetUserIdFromToken(token);

        return await _repository.GetByBuyer(userId);
    }

    public async Task<List<ProductEntity>> GetByBetOwner(string token)
    {
        var ownerId = _jwtProvider.GetUserIdFromToken(token);

        return await _repository.GetByBetOwner(ownerId);
    }

    public async Task Add(string token, ProductEntity product)
    {
        var ownerId = _jwtProvider.GetUserIdFromToken(token);

        product.OwnerId = ownerId;

        await _repository.Add(product);
    }

    public async Task Update(Guid id, ProductEntity product)
        => await _repository.Update(id, product);

    public async Task Delete(Guid id)
        => await _repository.Delete(id);

    public async Task DoBetForProduct(Guid id, string token, decimal offer)
    {
        var userId = _jwtProvider.GetUserIdFromToken(token);

        await _repository.DoBetForProduct(id, userId, offer);
    }

    public async Task BuyOutProduct(Guid productId, string token)
    {
        var userId = _jwtProvider.GetUserIdFromToken(token);

        await _repository.BuyOutProduct(productId, userId);
    }
}
