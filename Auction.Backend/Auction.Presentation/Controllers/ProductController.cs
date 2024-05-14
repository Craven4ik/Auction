using Auction.Core.Enums;
using Auction.Core.Models;
using Auction.Domain.Interfaces.Services;
using Auction.Presentation.Contracts.Products;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Auction.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ProductController(IProductService productService, IMapper mapper, ILogger<ProductController> logger) : ControllerBase
{
    private readonly IProductService _productService = productService;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<ProductController> _logger = logger;

    [HttpGet]
    public async Task<IActionResult> GetProducts([FromQuery] ProductFilters filters)
    {
        _logger.LogInformation("Получен запрос на получение товаров по фильтру");

        var products = await _productService.GetByFilter(filters);

        var response = products.Select(_mapper.Map<GetProductResponse>).ToList();

        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProductById([FromRoute] Guid id)
    {
        _logger.LogInformation($"Получен запрос на получения товара по идентификатору {id}");

        var product = await _productService.GetById(id);

        var response = _mapper.Map<GetProductResponse>(product);

        return Ok(response);
    }

    [HttpGet("getUserProducts")]
    public async Task<IActionResult> GetUserProducts()
    {
        _logger.LogInformation("Получен запрос на получение товаров пользователя");

        var jwtToken = Request.Cookies["some-cookies"];

        if (jwtToken is null)
            return Unauthorized();

        var products = await _productService.GetByOwner(jwtToken);

        var response = products.Select(_mapper.Map<GetProductResponse>).ToList();

        return Ok(response);
    }
    
    [HttpGet("getUserPurchases")]
    public async Task<IActionResult> GetUserPurchases()
    {
        _logger.LogInformation("Получен запрос на получение покупок пользователя");

        var jwtToken = Request.Cookies["some-cookies"];

        if (jwtToken is null)
            return Unauthorized();

        var products = await _productService.GetByBuyer(jwtToken);

        var response = products.Select(_mapper.Map<GetProductResponse>).ToList();

        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> CreateProduct([FromBody] AddProductRequest request)
    {
        _logger.LogInformation("Получен запрос на создание товара");

        var jwtToken = Request.Cookies["some-cookies"];

        if (jwtToken is null)
            return Unauthorized();

        try
        {
            var product = _mapper.Map<ProductEntity>(request);
            product.EndTime = request.EndTime <= request.StartTime ? request.EndTime.AddDays(3) : request.EndTime;
            await _productService.Add(jwtToken, product);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return BadRequest(ex.Message);
        }

        return Ok();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct([FromRoute] Guid id, [FromBody] UpdateProductRequest request)
    {
        _logger.LogInformation("Получен запрос на обновление товара");

        try
        {
            var product = _mapper.Map<ProductEntity>(request);
            await _productService.Update(id, product);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return BadRequest(ex.Message);
        }

        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct([FromRoute] Guid id)
    {
        _logger.LogInformation($"Получен запрос на удаление товара по идентификатору {id}");

        try
        {
            await _productService.Delete(id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return BadRequest(ex.Message);
        }

        return Ok();
    }

    [HttpPost("doBet/{id}")]
    public async Task<IActionResult> AddBetForProduct([FromRoute] Guid id, [FromBody] AddBetForProductRequest request)
    {
        _logger.LogInformation($"Получен запрос на добавление ставки по лоту {id}");

        var jwtToken = Request.Cookies["some-cookies"];

        if (jwtToken is null)
            return Unauthorized();

        try
        {
            await _productService.DoBetForProduct(id, jwtToken, request.Offer);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return BadRequest(ex.Message);
        }

        return Ok();
    }

    [HttpGet("getUserBets")]
    public async Task<IActionResult> GetUserBets()
    {
        _logger.LogInformation("Получен запрос на получение информации о ставках пользователя");

        var jwtToken = Request.Cookies["some-cookies"];

        if (jwtToken is null)
            return Unauthorized();

        var products = await _productService.GetByBetOwner(jwtToken);

        var response = products.Select(_mapper.Map<GetBetInfoResponse>).ToList();

        return Ok(response);
    }

    [HttpPost("buyOutProduct/{productId}")]
    public async Task<IActionResult> BuyOutProduct([FromRoute] Guid productId)
    {
        _logger.LogInformation($"Получен запрос на выкуп лота {productId}");

        var jwtToken = Request.Cookies["some-cookies"];

        if (jwtToken is null)
            return Unauthorized();

        try
        {
            await _productService.BuyOutProduct(productId, jwtToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return BadRequest(ex.Message);
        }

        return Ok();
    }
}
