using AutoMapper;
using RetailECommercePlatform.Data.Contract;
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
        CreateMap<RegisterForAdminCommand, Admin>();
        CreateMap<CreateProductCommand, Product>();
        CreateMap<UpdateProductCommand, Product>();
        CreateMap<Product, GetProductQueryResponse>();
        CreateMap<Admin, CurrentUserDto>();
        CreateMap<Customer, CurrentUserDto>();
    }
}