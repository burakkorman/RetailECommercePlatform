using System.Linq.Expressions;
using AutoMapper;
using MediatR;
using MongoDB.Driver;
using RetailECommercePlatform.Data.Helper;
using RetailECommercePlatform.Data.RequestModels.Query.Product;
using RetailECommercePlatform.Data.ResponseModels.Query.Product;
using RetailECommercePlatform.Repository.Repositories.Interfaces;

namespace RetailECommercePlatform.Service.QueryHandlers.Product;

public class GetProductQueryHandler : IRequestHandler<GetProductQuery, List<GetProductQueryResponse>>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public GetProductQueryHandler(IProductRepository productRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }
    
    public async Task<List<GetProductQueryResponse>> Handle(GetProductQuery request, CancellationToken cancellationToken)
    {
        var builder = Builders<Repository.Entities.Product>.Filter;
        var filter = builder.Eq(x => x.IsActive, true);

        if (!string.IsNullOrEmpty(request.Name))
        {
            filter = filter & builder.Regex(x => x.Name, new MongoDB.Bson.BsonRegularExpression(request.Name, "i"));
        }

        if (!string.IsNullOrEmpty(request.Code))
        {
            filter = filter & builder.Regex(x => x.Code, new MongoDB.Bson.BsonRegularExpression(request.Code, "i"));
        }

        var products = await _productRepository.SearchAsync(filter);
        
        ArgumentNullException.ThrowIfNull(products);

        return _mapper.Map<List<Repository.Entities.Product>, List<GetProductQueryResponse>>(products);
    }
}