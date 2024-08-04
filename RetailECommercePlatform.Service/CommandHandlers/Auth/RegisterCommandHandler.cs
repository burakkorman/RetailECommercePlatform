using AutoMapper;
using MediatR;
using RetailECommercePlatform.Data.RequestModels.Command.Auth;
using RetailECommercePlatform.Repository.Repositories.Interfaces;

namespace RetailECommercePlatform.Service.CommandHandlers.Auth;

public class RegisterCommandHandler(ICustomerRepository customerRepository, IMapper mapper)
    : IRequestHandler<RegisterCommand, bool>
{
    public async Task<bool> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var customer = mapper.Map<Repository.Entities.Customer>(request);
            await customerRepository.AddAsync(customer);
            
            return true;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }
}