using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
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
    private readonly IAdminRepository _adminRepository;
    private readonly IMapper _mapper;

    public TokenService(IOptions<JwtSettings> jwtSettings, ICustomerRepository customerRepository, IAdminRepository adminRepository, IMapper mapper)
    {
        _jwtSettings = jwtSettings.Value;
        _customerRepository = customerRepository;
        _adminRepository = adminRepository;
        _mapper = mapper;
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
                new Claim("username", request.Username),
                new Claim(ClaimTypes.Role, request.Role)
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
            var currentRole = jwtSecurityToken.Claims.First(claim => claim.Type == ClaimTypes.Role).Value;

            if (string.IsNullOrEmpty(currentUsername) || string.IsNullOrEmpty(currentRole))
            {
                throw new SecurityTokenException("Invalid token claims.");
            }
            
            if (currentRole == "Admin")
            {
                var admin = await _adminRepository.GetAsync(a => a.Username == currentUsername);
                if (admin == null)
                {
                    throw new KeyNotFoundException("Admin not found.");
                }
                CurrentUser = _mapper.Map<CurrentUserDto>(admin);
            }
            else
            {
                var customer = await _customerRepository.GetByUsername(currentUsername);
                if (customer == null)
                {
                    throw new KeyNotFoundException("Customer not found.");
                }
                CurrentUser = _mapper.Map<CurrentUserDto>(customer);
            }
            CurrentUser.Role = currentRole;

            return CurrentUser;
        }
        catch
        {
            return null;
        }
    }
}