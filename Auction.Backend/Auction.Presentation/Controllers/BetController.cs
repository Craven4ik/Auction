using Auction.Core.Models;
using Auction.Domain.Interfaces.Services;
using Auction.Presentation.Contracts.Bets;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace Auction.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class BetController(IBetService betService) : ControllerBase
{
    private readonly IBetService _betService = betService;

    [HttpGet]
    public async Task<IActionResult> GetBets()
    {
        var bets = await _betService.Get();

        var response = bets.Select(b => new GetBetResponse(b.Id, b.UserId, b.ProductId, b.Offer));

        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetBetById([FromRoute] Guid id)
    {
        var bet = await _betService.GetById(id);

        var response = new GetBetResponse(bet.Id, bet.UserId, bet.ProductId, bet.Offer);

        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> CreateBet([FromBody] AddBetRequest request)
    {
        var bet = new BetEntity()
        {
            Id = Guid.NewGuid(),
            UserId = request.UserId,
            ProductId = request.ProductId,
            Offer = request.Offer
        };

        await _betService.Add(bet);

        return Ok();
    }

    [HttpPut]
    public async Task<IActionResult> UpdateBet([FromRoute] Guid id, [FromBody] UpdateBetRequest request)
    {
        await _betService.Update(id, request.UserId, request.Offer);

        return Ok();
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteBet([FromRoute] Guid id)
    {
        await _betService.Delete(id);

        return Ok();
    }
}
