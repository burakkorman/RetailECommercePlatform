using MediatR;
using RetailECommercePlatform.Data.Contract.Token;
using RetailECommercePlatform.Data.Errors;
using RetailECommercePlatform.Data.RequestModels.Command.Auth;
using RetailECommercePlatform.Data.ResponseModels.Command.Auth;
using RetailECommercePlatform.Repository.Repositories.Generic;
using RetailECommercePlatform.Repository.Repositories.Interfaces;
using RetailECommercePlatform.Service.Services.Token;

namespace RetailECommercePlatform.Service.CommandHandlers.Auth;

public class LoginCommandHandler(ICustomerRepository customerRepository, IAdminRepository adminRepository, ITokenService tokenService) : IRequestHandler<LoginCommand, LoginCommandResponse>
{
    public async Task<LoginCommandResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
        {
            throw new ArgumentNullException(nameof(request));
        }
        
        var role = request.IsAdmin ? "Admin" : "Customer";

        var userExist = request.IsAdmin
            ? await adminRepository.AnyAsync(x => x.IsActive && x.Username == request.Username && x.Password == request.Password)
            : await customerRepository.AnyAsync(x => x.IsActive && x.Username == request.Username && x.Password == request.Password);
        
        if (!userExist)
        {
            throw new RetailECommerceApiException(CustomError.E_100);
        }
        
        var generatedTokenInformation = await tokenService.GenerateToken(new GenerateTokenRequest
        {
            Username = request.Username,
            Role = role
        });

        return new LoginCommandResponse
        {
            AuthenticateResult = true,
            AuthToken = generatedTokenInformation.Token,
            AccessTokenExpireDate = generatedTokenInformation.TokenExpireDate
        };
    }
}