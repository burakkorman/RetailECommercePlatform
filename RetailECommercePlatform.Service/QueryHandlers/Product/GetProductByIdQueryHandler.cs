using AutoMapper;
using MediatR;
using RetailECommercePlatform.Data.Errors;
using RetailECommercePlatform.Data.RequestModels.Query.Product;
using RetailECommercePlatform.Data.ResponseModels.Query.Product;
using RetailECommercePlatform.Repository.Repositories.Interfaces;

namespace RetailECommercePlatform.Service.QueryHandlers.Product;

public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, GetProductQueryResponse>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public GetProductByIdQueryHandler(IProductRepository productRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }
    
    public async Task<GetProductQueryResponse> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(request.Id);

        if (product is null)
        {
            throw new RetailECommerceApiException(CustomError.E_200);
        }

        return _mapper.Map<GetProductQueryResponse>(product);
    }
}