using MediatR;

namespace RetailECommercePlatform.Data.RequestModels.Command.Cart;

public class AddItemCommand : IRequest<bool>
{
    public string ProductId { get; set; }
    public int Quantity { get; set; }
}