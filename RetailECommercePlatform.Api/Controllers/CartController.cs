using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RetailECommercePlatform.Data.RequestModels.Command.Cart;
using RetailECommercePlatform.Data.RequestModels.Query.Cart;

namespace RetailECommercePlatform.Api.Controllers;

[Authorize]
[Route("api/cart")]
[ApiController]
public class CartController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var result = await mediator.Send(new GetCartQuery());
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> AddItem([FromBody] AddItemCommand request)
    {
        var result = await mediator.Send(request);
        return Ok(result);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteItem(DeleteItemCommand request)
    {
        var result = await mediator.Send(request);
        return Ok(result);
    }
}