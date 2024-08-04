using MediatR;

namespace RetailECommercePlatform.Data.RequestModels.Command.Auth;

public class RegisterCommand : IRequest<bool>
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string Address { get; set; }
}