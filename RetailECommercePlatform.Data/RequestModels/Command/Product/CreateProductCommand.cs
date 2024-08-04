using MediatR;

namespace RetailECommercePlatform.Data.RequestModels.Command.Product;

public class CreateProductCommand : IRequest<bool>
{
    public string Code { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public int StockCount { get; set; }
}