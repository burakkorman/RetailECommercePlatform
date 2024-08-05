using AutoMapper;
using MediatR;
using RetailECommercePlatform.Data.Errors;
using RetailECommercePlatform.Data.RequestModels.Command.Auth;
using RetailECommercePlatform.Repository.Repositories.Interfaces;

namespace RetailECommercePlatform.Service.CommandHandlers.Auth;

public class RegisterForAdminCommandHandler(IAdminRepository adminRepository, IMapper mapper)
    : IRequestHandler<RegisterForAdminCommand, bool>
{
    public async Task<bool> Handle(RegisterForAdminCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var isUsernameExist = await adminRepository.AnyAsync(a => a.IsActive && a.Username == request.Username);

            if (isUsernameExist)
            {
                throw new RetailECommerceApiException(CustomError.E_101);
            }
            
            var admin = mapper.Map<Repository.Entities.Admin>(request);
            await adminRepository.AddAsync(admin);
            
            return true;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }
}