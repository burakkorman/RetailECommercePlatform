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
    /// <summary>
    /// Login
    /// </summary>
    [HttpPost("login")]
    public async Task<IActionResult> Post(LoginCommand request)
    {
        var result = await mediator.Send(request);
        return Ok(result);
    }
    
    /// <summary>
    /// Register for customer
    /// </summary>
    [HttpPost("register")]
    public async Task<IActionResult> Post(RegisterCommand request)
    {
        var result = await mediator.Send(request);
        return Ok(result);
    }
    
    /// <summary>
    /// Register for admin
    /// </summary>
    [HttpPost("admin/register")]
    public async Task<IActionResult> Post(RegisterForAdminCommand request)
    {
        var result = await mediator.Send(request);
        return Ok(result);
    }
}