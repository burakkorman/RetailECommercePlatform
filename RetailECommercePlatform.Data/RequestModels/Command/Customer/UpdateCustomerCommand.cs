using System.Text.Json.Serialization;
using MediatR;

namespace RetailECommercePlatform.Data.RequestModels.Command.Customer;

public class UpdateCustomerCommand : IRequest<bool>
{
    [JsonIgnore] public string Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string Address { get; set; }
}