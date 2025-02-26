using AutoMapper;
using MediatR;
using RetailECommercePlatform.Data.Errors;
using RetailECommercePlatform.Data.RequestModels.Command.Auth;
using RetailECommercePlatform.Repository.Entities;
using RetailECommercePlatform.Repository.Repositories.Interfaces;

namespace RetailECommercePlatform.Service.CommandHandlers.Auth;

public class RegisterCommandHandler(ICustomerRepository customerRepository, IMapper mapper)
    : IRequestHandler<RegisterCommand, bool>
{
    public async Task<bool> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var isUsernameExist = await customerRepository.AnyAsync(c => c.IsActive && c.Username == request.Username);

            if (isUsernameExist)
            {
                throw new RetailECommerceApiException(CustomError.E_101);
            }
            
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