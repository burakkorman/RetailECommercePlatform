using System.Linq.Expressions;
using RetailECommercePlatform.Data.ResponseModels.Query.Order;

namespace RetailECommercePlatform.Service.Services.Order;

public interface IOrderService
{
    Task<List<GetOrderQueryResponse>> GetOrderListResponse(Expression<Func<Repository.Entities.Order, bool>> filter);
}