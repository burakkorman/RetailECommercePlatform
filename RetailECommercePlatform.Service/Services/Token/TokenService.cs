using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RetailECommercePlatform.Data.ConfigurationModels;
using RetailECommercePlatform.Data.Contract;
using RetailECommercePlatform.Data.Contract.Token;
using RetailECommercePlatform.Repository;
using RetailECommercePlatform.Repository.Entities;
using RetailECommercePlatform.Repository.Repositories.Interfaces;

namespace RetailECommercePlatform.Service.Services.Token;

public class TokenService : ITokenService
{
    public CurrentUserDto CurrentUser { get; private set; }
    private readonly JwtSettings _jwtSettings;
    private readonly ICustomerRepository _customerRepository;

    public TokenService(IOptions<JwtSettings> jwtSettings, ICustomerRepository customerRepository)
    {
        _jwtSettings = jwtSettings.Value;
        _customerRepository = customerRepository;
    }

    public Task<GenerateTokenResponse> GenerateToken(GenerateTokenRequest request)
    {
        SymmetricSecurityKey symmetricSecurityKey =
            new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtSettings.Secret));

        var dateTimeNow = DateTime.UtcNow;

        JwtSecurityToken jwt = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: new List<Claim>
            {
                new Claim("username", request.Username)
            },
            notBefore: dateTimeNow,
            expires: DateTime.Now.AddMinutes(_jwtSettings.ExpiryMinutes),
            signingCredentials: new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256)
        );

        var response = new GenerateTokenResponse
        {
            Token = new JwtSecurityTokenHandler().WriteToken(jwt),
            TokenExpireDate = dateTimeNow.Add(TimeSpan.FromMinutes(500))
        };

        return Task.FromResult(response);
    }

    public CurrentUserDto Me()
    {
        return CurrentUser;
    }

    public async Task<CurrentUserDto> ValidateToken(string token)
    {
        try
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(token);
            var currentUsername = jwtSecurityToken.Claims.First(claim => claim.Type == "username").Value;

            var customer = await _customerRepository.GetByUsername(currentUsername);
            CurrentUser = new CurrentUserDto
            {
                Id = customer.Id,
                Name = customer.Name,
                Surname = customer.Surname
            };

            return CurrentUser;
        }
        catch
        {
            return null;
        }
    }
}