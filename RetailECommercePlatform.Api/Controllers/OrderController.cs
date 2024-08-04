using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RetailECommercePlatform.Data.RequestModels.Command.Order;
using RetailECommercePlatform.Data.RequestModels.Query.Order;

namespace RetailECommercePlatform.Api.Controllers;

[Authorize]
[Route("api/order")]
[ApiController]
public class OrderController(IMediator mediator) : ControllerBase
{
    [HttpGet()]
    public async Task<IActionResult> Get()
    {
        var result = await mediator.Send(new GetOrdersQuery());
        return Ok(result);
    }
    
    [HttpGet("getById")]
    public async Task<IActionResult> Get([FromQuery] GetOrderByIdQuery request)
    {
        var result = await mediator.Send(request);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Post()
    {
        var result = await mediator.Send(new CreateOrderCommand());
        return Ok(result);
    }


    [HttpPut("cancel")]
    public async Task<IActionResult> Delete(CancelOrderCommand request)
    {
        var result = await mediator.Send(request);
        return Ok(result);
    }
}