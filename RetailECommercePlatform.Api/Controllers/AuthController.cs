using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RetailECommercePlatform.Data.RequestModels.Command.Auth;
using RetailECommercePlatform.Data.RequestModels.Command.Customer;
using RetailECommercePlatform.Data.RequestModels.Query.Customer;

namespace RetailECommercePlatform.Api.Controllers;

[AllowAnonymous]
[Route("api/auth")]
[ApiController]
public class AuthController(IMediator mediator) : ControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> Post(LoginCommand request)
    {
        var result = await mediator.Send(request);
        return Ok(result);
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> Post(RegisterCommand request)
    {
        var result = await mediator.Send(request);
        return Ok(result);
    }
}