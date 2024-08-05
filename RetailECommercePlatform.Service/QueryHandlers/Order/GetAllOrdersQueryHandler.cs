using System.Linq.Expressions;
using MediatR;
using RetailECommercePlatform.Data.RequestModels.Query.Order;
using RetailECommercePlatform.Data.ResponseModels.Query.Order;
using RetailECommercePlatform.Repository.Repositories.Interfaces;
using RetailECommercePlatform.Service.Services.Order;
using RetailECommercePlatform.Service.Services.Token;

namespace RetailECommercePlatform.Service.QueryHandlers.Order;

public class GetAllOrdersQueryHandler : IRequestHandler<GetAllOrdersQuery, List<GetOrderQueryResponse>>
{
    private readonly IOrderService _orderService;

    public GetAllOrdersQueryHandler(
        IOrderService orderService)
    {
        _orderService = orderService;
    }
    
    public async Task<List<GetOrderQueryResponse>> Handle(GetAllOrdersQuery request, CancellationToken cancellationToken)
    {
        Expression<Func<Repository.Entities.Order, bool>> filter = order => order.IsActive;

        return await _orderService.GetOrderListResponse(filter);
    }
}