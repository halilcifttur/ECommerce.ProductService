using ECommerce.ProductService.Application.Features.Products.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.ProductService.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ProductController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpPost]
    public async Task<IActionResult> AddProduct(AddProductCommand command)
    {
        await _mediator.Send(command);
        return Ok("Product Added");
    }
}
