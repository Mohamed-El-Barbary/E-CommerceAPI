using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using E_Commerce.Domain.Entities.IdentityModule;
using E_Commerce.Services_Abstraction;
using E_Commerce.Shared.CommonResult;
using E_Commerce.Shared.DTOs.IdentityDTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace E_Commerce.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IConfiguration _configuration;

    public AuthenticationService(UserManager<ApplicationUser> userManager, IConfiguration configuration)
    {
        _userManager = userManager;
        _configuration = configuration;
    }

    public async Task<Result<UserDTO>> LoginAsync(LoginDTO loginDTO)
    {
        var user = await _userManager.FindByEmailAsync(loginDTO.Email);
        if (user is null)
            return Error.InvalidCredentials("User.InvalidCredentials");

        var isPasswordValid = await _userManager.CheckPasswordAsync(user, loginDTO.Password);
        if (!isPasswordValid)
            return Error.InvalidCredentials("User.InvalidCredentials");

        var token = await CreateTokenAsync(user);
        return new UserDTO(user.Email!, user.DisplayName, token);
    }

    public async Task<Result<UserDTO>> RegisterAsync(RegisterDTO registerDTO)
    {
        var user = new ApplicationUser()
        {
            Email = registerDTO.Email,
            DisplayName = registerDTO.DisplayName,
            PhoneNumber = registerDTO.PhoneNumber,
            UserName = registerDTO.PhoneNumber
        };

        var identityResult = await _userManager.CreateAsync(user, registerDTO.Password);
        if (identityResult.Succeeded)
        {
            var token = await CreateTokenAsync(user);
            return new UserDTO(user.Email!, user.DisplayName, token);
        }

        return identityResult.Errors.Select(e => Error.Validation(e.Code, e.Description)).ToList();
    }

    private async Task<string> CreateTokenAsync(ApplicationUser user)
    {
        // Token [Issuer, Audience, Claims, Expires, SigningCredentials]
        var claims = new List<Claim>()
        {
            new Claim(JwtRegisteredClaimNames.Email, user.Email!),
            new Claim(JwtRegisteredClaimNames.Name, user.UserName!),
        };

        var roles = await _userManager.GetRolesAsync(user);
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var secretKey = _configuration["JWTOptions:SecretKey"];
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["JWTOptions:Issuer"],
            audience: _configuration["JWTOptions:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: cred
        );
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}