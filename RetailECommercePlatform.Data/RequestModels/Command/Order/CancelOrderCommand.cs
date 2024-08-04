using MediatR;

namespace RetailECommercePlatform.Data.RequestModels.Command.Order;

public class CancelOrderCommand : IRequest<bool>
{
    public string OrderId { get; set; }
}