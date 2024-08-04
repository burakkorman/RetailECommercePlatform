using MediatR;
using RetailECommercePlatform.Data.ResponseModels.Query.Order;

namespace RetailECommercePlatform.Data.RequestModels.Query.Order;

public class GetOrderByIdQuery : IRequest<GetOrderQueryResponse>
{
    public string Id { get; set; }
}