using RetailECommercePlatform.Data.Contract;
using RetailECommercePlatform.Data.Contract.Token;
using RetailECommercePlatform.Repository.Entities;

namespace RetailECommercePlatform.Service.Services.Token;

public interface ITokenService
{
    public Task<GenerateTokenResponse> GenerateToken(GenerateTokenRequest request);
    CurrentUserDto Me();
    Task<CurrentUserDto> ValidateToken(string token);
}