using MediatR;

namespace RetailECommercePlatform.Data.RequestModels.Command.Auth;

public class RegisterForAdminCommand : IRequest<bool>
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
}