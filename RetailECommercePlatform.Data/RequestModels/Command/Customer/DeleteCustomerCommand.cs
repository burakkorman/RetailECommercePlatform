using System.Text.Json.Serialization;
using MediatR;

namespace RetailECommercePlatform.Data.RequestModels.Command.Customer;

public class DeleteCustomerCommand : IRequest<bool>
{
    public Guid Id { get; set; }
}