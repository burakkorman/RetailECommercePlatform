using MediatR;
using RetailECommercePlatform.Data.ResponseModels.Query.Product;

namespace RetailECommercePlatform.Data.RequestModels.Query.Product;

public class GetProductByIdQuery : IRequest<GetProductQueryResponse>
{
    public string Id { get; set; }
}