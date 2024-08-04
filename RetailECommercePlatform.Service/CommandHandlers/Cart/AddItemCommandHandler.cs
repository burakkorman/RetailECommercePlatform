using MediatR;
using RetailECommercePlatform.Data.Errors;
using RetailECommercePlatform.Data.RequestModels.Command.Cart;
using RetailECommercePlatform.Repository.Repositories.Interfaces;
using RetailECommercePlatform.Service.Services.Token;

namespace RetailECommercePlatform.Service.CommandHandlers.Cart;

public class AddItemCommandHandler : IRequestHandler<AddItemCommand, bool>
{
    private readonly ICartRepository _cartRepository;
    private readonly IProductRepository _productRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly ITokenService _tokenService;

    public AddItemCommandHandler(ICartRepository cartRepository, IProductRepository productRepository, ITokenService tokenService, ICustomerRepository customerRepository)
    {
        _cartRepository = cartRepository;
        _productRepository = productRepository;
        _tokenService = tokenService;
        _customerRepository = customerRepository;
    }
    
    public async Task<bool> Handle(AddItemCommand request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetAsync(p => p.IsActive && p.Id == request.ProductId);

        if (product is null)
            throw new RetailECommerceApiException(CustomError.E_200);
        
        var customer = _tokenService.Me();

        var cart = await _cartRepository.GetAsync(x =>
            x.IsActive && x.CustomerId == customer.Id && x.Product.Id == request.ProductId);

        if (cart is null)
        {
            var newCart = new Repository.Entities.Cart
            {
                CustomerId = customer.Id,
                ProductId = request.ProductId,
                Product = product,
                Quantity = request.Quantity,
            };

            await _cartRepository.AddAsync(newCart);
        }
        else
        {
            cart.Quantity += request.Quantity;
            await _cartRepository.UpdateAsync(cart.Id, cart);
        }
        
        return true;
    }
}