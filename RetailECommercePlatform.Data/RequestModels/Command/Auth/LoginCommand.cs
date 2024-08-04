using MediatR;
using RetailECommercePlatform.Data.ResponseModels.Command.Auth;

namespace RetailECommercePlatform.Data.RequestModels.Command.Auth;

public class LoginCommand : IRequest<LoginCommandResponse>
{
    public string Username { get; set; }
    public string Password { get; set; }
}