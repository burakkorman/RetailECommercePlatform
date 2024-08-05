using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RetailECommercePlatform.Data.RequestModels.Command.Product;
using RetailECommercePlatform.Data.RequestModels.Query.Product;

namespace RetailECommercePlatform.Api.Controllers;

[Route("api/product")]
[ApiController]
public class ProductController(IMediator mediator) : ControllerBase
{
    [Authorize(Roles = "Admin,Customer")]
    [HttpPost("filter")]
    public async Task<IActionResult> Get([FromBody] GetProductQuery request)
    {
        var result = await mediator.Send(request);
        return Ok(result);
    }
    
    [Authorize(Roles = "Admin,Customer")]
    [HttpGet("getById")]
    public async Task<IActionResult> Get([FromQuery] GetProductByIdQuery request)
    {
        var result = await mediator.Send(request);
        return Ok(result);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CreateProductCommand request)
    {
        var result = await mediator.Send(request);
        return Ok(result);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(string id, [FromBody] UpdateProductCommand request)
    {
        request.Id = id;
        var result = await mediator.Send(request);
        return Ok(result);
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete]
    public async Task<IActionResult> Delete([FromQuery] DeleteProductCommand request)
    {
        var result = await mediator.Send(request);
        return Ok(result);
    }
}