using RetailECommercePlatform.Repository.Entities;
using RetailECommercePlatform.Repository.Repositories.Generic;

namespace RetailECommercePlatform.Repository.Repositories.Interfaces;

public interface ICustomerRepository: IGenericRepository<Customer>
{
    Task<Customer> GetByUsername(string username);
}