using MediatR;
using Microsoft.AspNetCore.Http;
using RetailECommercePlatform.Data.Errors;
using RetailECommercePlatform.Data.RequestModels.Command.Cart;
using RetailECommercePlatform.Repository.Repositories.Interfaces;
using RetailECommercePlatform.Service.Services.Token;

namespace RetailECommercePlatform.Service.CommandHandlers.Cart;

public class DeleteItemCommandHandler : IRequestHandler<DeleteItemCommand, bool>
{
    private readonly ICartRepository _cartRepository;
    private readonly IProductRepository _productRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly ITokenService _tokenService;

    public DeleteItemCommandHandler(ICartRepository cartRepository, IProductRepository productRepository, ITokenService tokenService, ICustomerRepository customerRepository)
    {
        _cartRepository = cartRepository;
        _productRepository = productRepository;
        _tokenService = tokenService;
        _customerRepository = customerRepository;
    }
    public async Task<bool> Handle(DeleteItemCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var customer = _tokenService.Me();

            var cart = await _cartRepository.GetAsync(x =>
                x.IsActive && x.CustomerId == customer.Id && x.Product.Id == request.ProductId);

            if (cart is null)
            {
                throw new RetailECommerceApiException(CustomError.E_300);
            }

            cart.IsActive = false;

            await _cartRepository.UpdateAsync(cart.Id, cart);

            return true;
        }
        catch (Exception e)
        {
            throw new BadHttpRequestException(CustomError.E_001);
        }
    }
}