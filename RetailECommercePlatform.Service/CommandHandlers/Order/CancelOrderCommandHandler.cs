using MediatR;
using RetailECommercePlatform.Data.Enum;
using RetailECommercePlatform.Data.Errors;
using RetailECommercePlatform.Data.RequestModels.Command.Order;
using RetailECommercePlatform.Repository.Repositories.Interfaces;
using RetailECommercePlatform.Service.Services.Token;

namespace RetailECommercePlatform.Service.CommandHandlers.Order;

public class CancelOrderCommandHandler : IRequestHandler<CancelOrderCommand, bool>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;

    public CancelOrderCommandHandler(IOrderRepository orderRepository, IProductRepository productRepository)
    {
        _orderRepository = orderRepository;
        _productRepository = productRepository;
    }
    
    public async Task<bool> Handle(CancelOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(request.OrderId);
        
        ArgumentNullException.ThrowIfNull(order);
        
        var productIds = order.OrderItems.Select(i => i.ProductId);
        var products = await _productRepository.FilterAsync(x => x.IsActive && productIds.Contains(x.Id));
        
        UpdateProductStock(order.OrderItems, products);
        
        order.State = (int)OrderState.Cancelled;

        await _orderRepository.UpdateAsync(order.Id, order);

        return true;
    }
    
    private void UpdateProductStock(IEnumerable<Repository.Entities.Cart> cartList, IEnumerable<Repository.Entities.Product> products)
    {
        foreach (var cart in cartList)
        {
            var product = products.FirstOrDefault(p => p.Id == cart.ProductId);
            
            if (product == null)
            {
                throw new RetailECommerceApiException($"Product with ID {cart.ProductId} not found.");
            }
            
            product.StockCount += cart.Quantity;
            _productRepository.UpdateAsync(product.Id, product);
        }
    }
}