using MediatR;

namespace RetailECommercePlatform.Data.RequestModels.Command.Cart;

public class DeleteItemCommand : IRequest<bool>
{
    public string ProductId { get; set; }
}