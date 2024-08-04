using MediatR;
using RetailECommercePlatform.Data.Enum;
using RetailECommercePlatform.Data.Helper;
using RetailECommercePlatform.Data.RequestModels.Query.Order;
using RetailECommercePlatform.Data.ResponseModels.Query.Order;
using RetailECommercePlatform.Repository.Repositories.Interfaces;
using RetailECommercePlatform.Service.Services.Token;

namespace RetailECommercePlatform.Service.QueryHandlers.Order;

public class GetOrdersQueryHandler : IRequestHandler<GetOrdersQuery, List<GetOrderQueryResponse>>
{
    private readonly IOrderRepository _orderRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly ITokenService _tokenService;

    public GetOrdersQueryHandler(
        IOrderRepository orderRepository,
        ICustomerRepository customerRepository,
        ITokenService tokenService)
    {
        _orderRepository = orderRepository;
        _customerRepository = customerRepository;
        _tokenService = tokenService;
    }
    
    public async Task<List<GetOrderQueryResponse>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
    {
        var response = new List<GetOrderQueryResponse>();
        var currentUser = _tokenService.Me();
        var orders =
            await _orderRepository.FilterAsync(x =>
                x.IsActive && x.CustomerId == currentUser.Id && x.State != (int)OrderState.Cancelled);

        var customerIds = orders.Select(o => o.CustomerId);
        var customers = await _customerRepository.FilterAsync(c => c.IsActive && customerIds.Contains(c.Id));

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