using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RetailECommercePlatform.Data.RequestModels.Command.Order;
using RetailECommercePlatform.Data.RequestModels.Query.Order;

namespace RetailECommercePlatform.Api.Controllers;

[Route("api/order")]
[ApiController]
public class OrderController(IMediator mediator) : ControllerBase
{
    [Authorize(Roles = "Admin")]
    [HttpGet("getAll")]
    public async Task<IActionResult> GetAll()
    {
        var result = await mediator.Send(new GetAllOrdersQuery());
        return Ok(result);
    }
    
    [Authorize(Roles = "Customer")]
    [HttpGet()]
    public async Task<IActionResult> Get()
    {
        var result = await mediator.Send(new GetOrdersQuery());
        return Ok(result);
    }
    
    [Authorize(Roles = "Admin,Customer")]
    [HttpGet("getById")]
    public async Task<IActionResult> Get([FromQuery] GetOrderByIdQuery request)
    {
        var result = await mediator.Send(request);
        return Ok(result);
    }

    [Authorize(Roles = "Customer")]
    [HttpPost]
    public async Task<IActionResult> Post()
    {
        var result = await mediator.Send(new CreateOrderCommand());
        return Ok(result);
    }

    [Authorize(Roles = "Customer")]
    [HttpPut("cancel")]
    public async Task<IActionResult> Delete(CancelOrderCommand request)
    {
        var result = await mediator.Send(request);
        return Ok(result);
    }
}