using Microsoft.Extensions.Options;
using RetailECommercePlatform.Repository.Entities;
using RetailECommercePlatform.Repository.Repositories.Generic;
using RetailECommercePlatform.Repository.Repositories.Interfaces;

namespace RetailECommercePlatform.Repository.Repositories;

public class AdminRepository: GenericRepository<Admin>, IAdminRepository
{
    public AdminRepository(IOptions<MongoDbSettings> options) : base(options)
    {
    }
}