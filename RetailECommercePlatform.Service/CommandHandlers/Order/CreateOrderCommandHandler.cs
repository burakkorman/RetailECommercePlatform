using MediatR;
using RetailECommercePlatform.Data.Enum;
using RetailECommercePlatform.Data.Errors;
using RetailECommercePlatform.Data.RequestModels.Command.Order;
using RetailECommercePlatform.Repository.Repositories.Interfaces;
using RetailECommercePlatform.Service.Services.Token;

namespace RetailECommercePlatform.Service.CommandHandlers.Order;

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, bool>
{
    private readonly IOrderRepository _orderRepository;
    private readonly ICartRepository _cartRepository;
    private readonly IProductRepository _productRepository;
    private readonly ITokenService _tokenService;

    public CreateOrderCommandHandler(
        IOrderRepository orderRepository,
        ICartRepository cartRepository,
        IProductRepository productRepository,
        ITokenService tokenService)
    {
        _orderRepository = orderRepository;
        _cartRepository = cartRepository;
        _productRepository = productRepository;
        _tokenService = tokenService;
    }

    public async Task<bool> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var currentUser = _tokenService.Me();
        var cartList = await _cartRepository.FilterAsync(x => x.IsActive && x.CustomerId == currentUser.Id);

        if (cartList is null || !cartList.Any())
            throw new RetailECommerceApiException(CustomError.E_300);
        
        var productIds = cartList.Select(c => c.ProductId).ToList();
        var products = await _productRepository.FilterAsync(p => p.IsActive && productIds.Contains(p.Id));

        await ValidateCartProducts(cartList, products);

        await CreateOrder(currentUser.Id, cartList);

        await DeactivateCartProducts(cartList);

        return true;
    }

    private async Task ValidateCartProducts(IEnumerable<Repository.Entities.Cart> cartList, IEnumerable<Repository.Entities.Product> products)
    {
        foreach (var cart in cartList)
        {
            var product = products.FirstOrDefault(p => p.Id == cart.ProductId);

            if (product == null)
            {
                throw new RetailECommerceApiException($"Product with ID {cart.ProductId} not found.");
            }

            if (cart.Quantity > product.StockCount)
            {
                throw new RetailECommerceApiException($"There is not enough stock for the product {product.Name}.");
            }

            product.StockCount -= cart.Quantity;
            await _productRepository.UpdateAsync(product.Id, product);
        }
    }

    private async Task CreateOrder(string customerId, IEnumerable<Repository.Entities.Cart> cartList)
    {
        var order = new Repository.Entities.Order()
        {
            CustomerId = customerId,
            OrderItems = cartList.ToList(),
            TotalPrice = cartList.Sum(c => c.Product.Price * c.Quantity),
            State = (int)OrderState.OrderReceived
        };
        await _orderRepository.AddAsync(order);
    }

    private async Task DeactivateCartProducts(IEnumerable<Repository.Entities.Cart> cartList)
    {
        foreach (var cart in cartList)
        {
            cart.IsActive = false;
            await _cartRepository.UpdateAsync(cart.Id, cart);
        }
    }
}