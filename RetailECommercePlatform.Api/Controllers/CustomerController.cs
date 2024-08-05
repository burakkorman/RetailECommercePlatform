using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RetailECommercePlatform.Data.RequestModels.Command.Customer;
using RetailECommercePlatform.Data.RequestModels.Query.Customer;

namespace RetailECommercePlatform.Api.Controllers;

[Authorize(Roles = "Customer")]
[Route("api/customer")]
[ApiController]
public class CustomerController(IMediator mediator) : ControllerBase
{
    [HttpGet("getById")]
    public async Task<IActionResult> Get([FromQuery] GetCustomerByIdRequest request)
    {
        var result = await mediator.Send(request);
        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(string id, [FromBody] UpdateCustomerCommand request)
    {
        request.Id = id;
        var result = await mediator.Send(request);
        return Ok(result);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(DeleteCustomerCommand request)
    {
        var result = await mediator.Send(request);
        return Ok(result);
    }
}