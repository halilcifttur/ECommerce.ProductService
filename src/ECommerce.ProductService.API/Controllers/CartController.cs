using ECommerce.ProductService.Application.Features.Carts.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.ProductService.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class CartController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpPost("add")]
    public async Task<IActionResult> AddToCart([FromBody] AddToCartCommand command)
    {
        try
        {
            return Ok(await _mediator.Send(command));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
