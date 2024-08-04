using Microsoft.Extensions.Options;
using RetailECommercePlatform.Repository.Entities;
using RetailECommercePlatform.Repository.Repositories.Generic;
using RetailECommercePlatform.Repository.Repositories.Interfaces;

namespace RetailECommercePlatform.Repository.Repositories;

public class CartRepository: GenericRepository<Cart>, ICartRepository
{
    public CartRepository(IOptions<MongoDbSettings> options) : base(options)
    {
    }
}