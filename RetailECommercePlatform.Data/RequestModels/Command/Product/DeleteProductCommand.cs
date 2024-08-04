using MediatR;

namespace RetailECommercePlatform.Data.RequestModels.Command.Product;

public class DeleteProductCommand : IRequest<bool>
{
    public string Id { get; set; }
}