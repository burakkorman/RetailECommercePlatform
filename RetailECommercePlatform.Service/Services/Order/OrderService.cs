using System.Linq.Expressions;
using RetailECommercePlatform.Data.Enum;
using RetailECommercePlatform.Data.Helper;
using RetailECommercePlatform.Data.ResponseModels.Query.Order;
using RetailECommercePlatform.Repository.Entities;
using RetailECommercePlatform.Repository.Repositories.Interfaces;

namespace RetailECommercePlatform.Service.Services.Order;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly ICustomerRepository _customerRepository;

    public OrderService(IOrderRepository orderRepository, ICustomerRepository customerRepository)
    {
        _orderRepository = orderRepository;
        _customerRepository = customerRepository;
    }
    
    public async Task<List<GetOrderQueryResponse>> GetOrderListResponse(Expression<Func<Repository.Entities.Order, bool>> filter)
    {
        var orders =
            await _orderRepository.FilterAsync(x => x.IsActive);
        
        var customerIds = orders.Select(o => o.CustomerId);
        var customers = await _customerRepository.FilterAsync(c => c.IsActive && customerIds.Contains(c.Id));

        var response = new List<GetOrderQueryResponse>();

        foreach (var order in orders)
        {
            var customer = customers.FirstOrDefault(c => c.Id == order.CustomerId);

            response.Add(new GetOrderQueryResponse
            {
                Id = order.Id,
                CustomerName = customer.Name + " " + customer.Surname,
                Address = customer.Address,
                Products = order.OrderItems.Select(i => new OrderProductModel
                {
                    Code = i.Product.Code,
                    Name = i.Product.Name,
                    Price = i.Product.Price
                }).ToList(),
                TotalPrice = order.TotalPrice,
                State = EnumHelper.GetDescription((OrderState)order.State)
            });
        }

        return response;
    }
}