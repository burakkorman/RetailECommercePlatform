using MediatR;
using RetailECommercePlatform.Data.RequestModels.Query.Cart;
using RetailECommercePlatform.Data.ResponseModels.Query.Cart;
using RetailECommercePlatform.Repository.Repositories.Interfaces;
using RetailECommercePlatform.Service.Services.Token;

namespace RetailECommercePlatform.Service.QueryHandlers.Cart;

public class GetCartQueryHandler : IRequestHandler<GetCartQuery, GetCartQueryResponse>
{
    private readonly ICartRepository _cartRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly ITokenService _tokenService;
    
    public GetCartQueryHandler(ICartRepository cartRepository, ICustomerRepository customerRepository, ITokenService tokenService)
    {
        _cartRepository = cartRepository;
        _customerRepository = customerRepository;
        _tokenService = tokenService;
    }
    
    public async Task<GetCartQueryResponse> Handle(GetCartQuery request, CancellationToken cancellationToken)
    {
        var currentUser = _tokenService.Me();

        var cartList = await _cartRepository.FilterAsync(x => x.IsActive && x.CustomerId == currentUser.Id);
        var totalPrice = cartList.Sum(c => c.Product.Price * c.Quantity);
        
        return new GetCartQueryResponse
        {
            Products = cartList.Select(p => new ProductForCartModel
            {
                Id = p.Product.Id,
                Code = p.Product.Code,
                Name = p.Product.Name,
                Price = p.Product.Price,
                Quantity = p.Quantity
            }).ToList(),
            TotalPrice = totalPrice
        };
    }
}