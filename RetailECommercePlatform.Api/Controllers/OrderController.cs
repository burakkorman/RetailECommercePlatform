using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace RetailECommercePlatform.Api.Controllers;

public class OrderController(IMediator mediator) : ControllerBase
{
    [HttpGet()]
    public async Task<IActionResult> GetAll()
    {
        var result = await mediator.Send(new object());
        return Ok(result);
    }
    
    [HttpGet("getById")]
    public async Task<IActionResult> Get(object request)
    {
        var result = await mediator.Send(request);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Post(object request)
    {
        var result = await mediator.Send(request);
        return Ok(result);
    }

    [HttpPut]
    public async Task<IActionResult> Put(object request)
    {
        var result = await mediator.Send(request);
        return Ok(result);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(object request)
    {
        var result = await mediator.Send(request);
        return Ok(result);
    }
}