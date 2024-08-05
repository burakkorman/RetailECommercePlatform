using System.Linq.Expressions;
using MediatR;
using RetailECommercePlatform.Data.Enum;
using RetailECommercePlatform.Data.RequestModels.Query.Order;
using RetailECommercePlatform.Data.ResponseModels.Query.Order;
using RetailECommercePlatform.Service.Services.Order;
using RetailECommercePlatform.Service.Services.Token;

namespace RetailECommercePlatform.Service.QueryHandlers.Order;

public class GetOrdersQueryHandler : IRequestHandler<GetOrdersQuery, List<GetOrderQueryResponse>>
{
    private readonly IOrderService _orderService;
    private readonly ITokenService _tokenService;

    public GetOrdersQueryHandler(
        IOrderService orderService,
        ITokenService tokenService)
    {
        _orderService = orderService;
        _tokenService = tokenService;
    }

    public async Task<List<GetOrderQueryResponse>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
    {
        var currentUser = _tokenService.Me();

        Expression<Func<Repository.Entities.Order, bool>> filter = order =>
            order.IsActive && order.CustomerId == currentUser.Id && order.State != (int)OrderState.Cancelled;

        return await _orderService.GetOrderListResponse(filter);
    }
}
