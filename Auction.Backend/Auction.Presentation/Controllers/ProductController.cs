using Auction.Core.Models;
using Auction.Domain.Interfaces.Services;
using Auction.Presentation.Contracts.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Auction.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ProductController(IProductService productService) : ControllerBase
{
    private readonly IProductService _productService = productService;

    [HttpGet]
    public async Task<IActionResult> GetProducts()
    {
        var products = await _productService.Get();

        var response = products
            .Select(p => new GetProductResponse(p.Id, p.OwnerId, p.BuyerId, p.BetId, p.State,
                p.Name, p.Description, p.StartPrice, p.BuyOutPrice, p.StartTime, p.EndTime));

        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProductById([FromRoute] Guid id)
    {
        var product = await _productService.GetById(id);

        var response = new GetProductResponse(product.Id, product.OwnerId, product.BuyerId, product.BetId,
            product.State, product.Name, product.Description, product.StartPrice, product.BuyOutPrice,
            product.StartTime, product.EndTime);

        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> CreateProduct([FromBody] AddProductRequest request)
    {
        var product = new ProductEntity()
        {
            Id = Guid.NewGuid(),
            OwnerId = request.OwnerId,
            Name = request.Name,
            Description = request.Description,
            StartPrice = request.StartPrice,
            BuyOutPrice = request.BuyOutPrice,
            StartTime = request.StartTime,
            EndTime = request.EndTime
        };

        await _productService.Add(product);

        return Ok();
    }

    [HttpPut]
    public async Task<IActionResult> UpdateProduct([FromRoute] Guid id, [FromBody] UpdateProductRequest request)
    {
        var product = new ProductEntity()
        {
            Id = id,
            OwnerId = request.OwnerId,
            BuyerId = request.BuyerId,
            BetId = request.BetId,
            Name = request.Name,
            Description = request.Description,
            StartPrice = request.StartPrice,
            BuyOutPrice = request.BuyOutPrice,
            StartTime = request.StartTime,
            EndTime = request.EndTime
        };

        await _productService.Update(id, product);

        return Ok();
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteProduct([FromRoute] Guid id)
    {
        await _productService.Delete(id);

        return Ok();
    }
}
