using AutoMapper;
using MediatR;
using RetailECommercePlatform.Data.RequestModels.Command.Product;
using RetailECommercePlatform.Repository.Repositories.Interfaces;

namespace RetailECommercePlatform.Service.CommandHandlers.Product;

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, bool>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public CreateProductCommandHandler(IProductRepository productRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }
    
    public async Task<bool> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var product = _mapper.Map<Repository.Entities.Product>(request);

        await _productRepository.AddAsync(product);
        
        return true;
    }
}