using MediatR;
using RetailECommercePlatform.Data.ResponseModels.Query.Order;

namespace RetailECommercePlatform.Data.RequestModels.Query.Order;

public class GetAllOrdersQuery : IRequest<List<GetOrderQueryResponse>>
{
    
}