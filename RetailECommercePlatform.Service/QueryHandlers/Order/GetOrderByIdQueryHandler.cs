using MediatR;
using RetailECommercePlatform.Data.Enum;
using RetailECommercePlatform.Data.Helper;
using RetailECommercePlatform.Data.RequestModels.Query.Order;
using RetailECommercePlatform.Data.ResponseModels.Query.Order;
using RetailECommercePlatform.Repository.Repositories.Interfaces;

namespace RetailECommercePlatform.Service.QueryHandlers.Order;

public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, GetOrderQueryResponse>
{
    private readonly IOrderRepository _orderRepository;
    private readonly ICustomerRepository _customerRepository;

    public GetOrderByIdQueryHandler(
        IOrderRepository orderRepository,
        ICustomerRepository customerRepository)
    {
        _orderRepository = orderRepository;
        _customerRepository = customerRepository;
    }

    public async Task<GetOrderQueryResponse> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetAsync(o => o.IsActive && o.Id == request.Id);
        
        ArgumentNullException.ThrowIfNull(order);
        
        var customer = await _customerRepository.GetAsync(c => c.IsActive && c.Id == order.CustomerId);
        
        ArgumentNullException.ThrowIfNull(customer);

        return new GetOrderQueryResponse
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
        };
    }
}