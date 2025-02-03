using ECommerce.ProductService.Application.Features.Users.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.ProductService.API.Controllers;

[ApiController]
[AllowAnonymous]
[Route("api/[controller]")]
public class UserController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpPost]
    public async Task<IActionResult> AddUser(AddUserCommand command)
    {
        await _mediator.Send(command);
        return Ok("User Registered");
    }
}