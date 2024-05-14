using Auction.Application.Auth;
using Auction.Application.Services;
using Auction.Core.Models;
using Auction.Domain.Interfaces.Repositories;
using Moq;

namespace Auction.Tests.Services;

public class ProductServiceTests
{
    private Mock<IProductRepository> _productRepositoryMock;
    private Mock<IJwtProvider> _jwtProviderMock;
    private ProductService _productService;

    public ProductServiceTests()
    {
        _productRepositoryMock = new Mock<IProductRepository>();
        _jwtProviderMock = new Mock<IJwtProvider>();
        _productService = new ProductService(_productRepositoryMock.Object, _jwtProviderMock.Object);
    }

    [Fact]
    public async Task Get_ReturnsExpectedProducts()
    {
        var expectedProducts = new List<ProductEntity> { new ProductEntity(), new ProductEntity() };
        _productRepositoryMock.Setup(x => x.Get()).ReturnsAsync(expectedProducts);

        var result = await _productService.Get();

        Assert.Equal(expectedProducts, result);
    }

    [Fact]
    public async Task GetById_ReturnsExpectedProduct()
    {
        var expectedProduct = new ProductEntity();
        var id = Guid.NewGuid();
        _productRepositoryMock.Setup(x => x.GetById(id)).ReturnsAsync(expectedProduct);

        var result = await _productService.GetById(id);

        Assert.Equal(expectedProduct, result);
    }

    [Fact]
    public async Task GetByOwner_ReturnsExpectedProducts()
    {
        var expectedProducts = new List<ProductEntity> { new ProductEntity(), new ProductEntity() };
        var token = "test_token";
        var userId = Guid.NewGuid();
        _jwtProviderMock.Setup(x => x.GetUserIdFromToken(token)).Returns(userId);
        _productRepositoryMock.Setup(x => x.GetByOwner(userId)).ReturnsAsync(expectedProducts);

        var result = await _productService.GetByOwner(token);

        Assert.Equal(expectedProducts, result);
    }

    [Fact]
    public async Task Add_AddsProductWithExpectedOwnerId()
    {
        var token = "test_token";
        var ownerId = Guid.NewGuid();
        var product = new ProductEntity();
        _jwtProviderMock.Setup(x => x.GetUserIdFromToken(token)).Returns(ownerId);
        _productRepositoryMock.Setup(x => x.Add(It.Is<ProductEntity>(p => p.OwnerId == ownerId))).Returns(Task.CompletedTask);

        await _productService.Add(token, product);

        _productRepositoryMock.Verify(x => x.Add(It.Is<ProductEntity>(p => p.OwnerId == ownerId)), Times.Once);
    }

    [Fact]
    public async Task GetByBuyer_ReturnsExpectedProducts()
    {
        var expectedProducts = new List<ProductEntity> { new ProductEntity(), new ProductEntity() };
        var token = "test_token";
        var userId = Guid.NewGuid();
        _jwtProviderMock.Setup(x => x.GetUserIdFromToken(token)).Returns(userId);
        _productRepositoryMock.Setup(x => x.GetByBuyer(userId)).ReturnsAsync(expectedProducts);

        var result = await _productService.GetByBuyer(token);

        Assert.Equal(expectedProducts, result);
    }

    [Fact]
    public async Task GetByBetOwner_ReturnsExpectedProducts()
    {
        var expectedProducts = new List<ProductEntity> { new ProductEntity(), new ProductEntity() };
        var token = "test_token";
        var ownerId = Guid.NewGuid();
        _jwtProviderMock.Setup(x => x.GetUserIdFromToken(token)).Returns(ownerId);
        _productRepositoryMock.Setup(x => x.GetByBetOwner(ownerId)).ReturnsAsync(expectedProducts);

        var result = await _productService.GetByBetOwner(token);

        Assert.Equal(expectedProducts, result);
    }

    [Fact]
    public async Task Update_UpdatesProduct()
    {
        var id = Guid.NewGuid();
        var product = new ProductEntity();
        _productRepositoryMock.Setup(x => x.Update(id, product)).Returns(Task.CompletedTask);

        await _productService.Update(id, product);

        _productRepositoryMock.Verify(x => x.Update(id, product), Times.Once);
    }

    [Fact]
    public async Task Delete_DeletesProduct()
    {
        var id = Guid.NewGuid();
        _productRepositoryMock.Setup(x => x.Delete(id)).Returns(Task.CompletedTask);

        await _productService.Delete(id);

        _productRepositoryMock.Verify(x => x.Delete(id), Times.Once);
    }

    [Fact]
    public async Task DoBetForProduct_MakesBetWithExpectedUserIdAndOffer()
    {
        var id = Guid.NewGuid();
        var token = "test_token";
        var userId = Guid.NewGuid();
        var offer = 100m;
        _jwtProviderMock.Setup(x => x.GetUserIdFromToken(token)).Returns(userId);
        _productRepositoryMock.Setup(x => x.DoBetForProduct(id, userId, offer)).Returns(Task.CompletedTask);

        await _productService.DoBetForProduct(id, token, offer);

        _productRepositoryMock.Verify(x => x.DoBetForProduct(id, userId, offer), Times.Once);
    }

    [Fact]
    public async Task BuyOutProduct_BuysProductWithExpectedUserId()
    {
        var productId = Guid.NewGuid();
        var token = "test_token";
        var userId = Guid.NewGuid();
        _jwtProviderMock.Setup(x => x.GetUserIdFromToken(token)).Returns(userId);
        _productRepositoryMock.Setup(x => x.BuyOutProduct(productId, userId)).Returns(Task.CompletedTask);

        await _productService.BuyOutProduct(productId, token);

        _productRepositoryMock.Verify(x => x.BuyOutProduct(productId, userId), Times.Once);
    }
}
