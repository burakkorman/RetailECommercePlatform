using MediatR;
using RetailECommercePlatform.Data.Contract.Token;
using RetailECommercePlatform.Data.Errors;
using RetailECommercePlatform.Data.RequestModels.Command.Auth;
using RetailECommercePlatform.Data.ResponseModels.Command.Auth;
using RetailECommercePlatform.Repository.Repositories.Generic;
using RetailECommercePlatform.Repository.Repositories.Interfaces;
using RetailECommercePlatform.Service.Services.Token;

namespace RetailECommercePlatform.Service.CommandHandlers.Auth;

public class LoginCommandHandler(ICustomerRepository customerRepository, ITokenService tokenService) : IRequestHandler<LoginCommand, LoginCommandResponse>
{
    public async Task<LoginCommandResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        LoginCommandResponse response = new();

        if (string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
        {
            throw new ArgumentNullException(nameof(request));
        }

        var customer = customerRepository.GetAsync(x => x.Username == request.Username && x.Password == request.Password);

        if (customer is null)
        {
            throw new RetailECommerceApiException(CustomError.E_100);
        }
        else
        {
            var generatedTokenInformation = await tokenService.GenerateToken(new GenerateTokenRequest { Username = request.Username });

            response.AuthenticateResult = true;
            response.AuthToken = generatedTokenInformation.Token;
            response.AccessTokenExpireDate = generatedTokenInformation.TokenExpireDate;  
        }

        return response;
    }
}