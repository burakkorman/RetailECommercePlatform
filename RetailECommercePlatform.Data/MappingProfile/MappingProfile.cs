using AutoMapper;
using RetailECommercePlatform.Data.RequestModels.Command.Auth;
using RetailECommercePlatform.Data.RequestModels.Command.Product;
using RetailECommercePlatform.Data.ResponseModels.Query.Product;
using RetailECommercePlatform.Repository.Entities;

namespace RetailECommercePlatform.Data.MappingProfile;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Create mappings here
        CreateMap<RegisterCommand, Customer>();
        CreateMap<CreateProductCommand, Product>();
        CreateMap<UpdateProductCommand, Product>();
        CreateMap<Product, GetProductQueryResponse>();
    }
}