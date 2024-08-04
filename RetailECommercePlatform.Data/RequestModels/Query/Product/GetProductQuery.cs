using MediatR;
using RetailECommercePlatform.Data.ResponseModels.Query.Product;

namespace RetailECommercePlatform.Data.RequestModels.Query.Product;

public class GetProductQuery : IRequest<List<GetProductQueryResponse>>
{
    public string? Name { get; set; }
    public string? Code { get; set; }
}