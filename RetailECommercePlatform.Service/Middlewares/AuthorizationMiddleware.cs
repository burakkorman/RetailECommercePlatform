using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Configuration;
using RetailECommercePlatform.Service.Services.Token;

namespace RetailECommercePlatform.Service.Middlewares;

public class AuthorizationMiddleware
{
    private readonly RequestDelegate _next;

    public AuthorizationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(
        HttpContext context,
        ITokenService tokenService)
    {
        var token = context.Request.Headers["Authorization"].ToString().Replace("Bearer ","");
        var user = await tokenService.ValidateToken(token);
        
        await _next(context);
    }
}