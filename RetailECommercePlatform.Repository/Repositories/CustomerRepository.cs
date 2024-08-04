using Microsoft.Extensions.Options;
using MongoDB.Driver;
using RetailECommercePlatform.Repository.Entities;
using RetailECommercePlatform.Repository.Repositories.Generic;
using RetailECommercePlatform.Repository.Repositories.Interfaces;

namespace RetailECommercePlatform.Repository.Repositories;

public class CustomerRepository: GenericRepository<Customer>, ICustomerRepository
{
    public CustomerRepository(IOptions<MongoDbSettings> options) : base(options)
    {
    }

    public async Task<Customer> GetByUsername(string username)
    {
        return await Collection.Find(x => x.IsActive && x.Username == username).FirstOrDefaultAsync();
    }
}