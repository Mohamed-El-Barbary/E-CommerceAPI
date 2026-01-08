using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using E_Commerce.Domain.Entities.IdentityModule;
using E_Commerce.Services_Abstraction;
using E_Commerce.Shared.CommonResult;
using E_Commerce.Shared.DTOs.IdentityDTOs;
using E_Commerce.Shared.DTOs.OrderDTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace E_Commerce.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IConfiguration _configuration;
    private readonly IMapper _mapper;

    public AuthenticationService(
        UserManager<ApplicationUser> userManager,
        IConfiguration configuration,
        IMapper mapper)
    {
        _userManager = userManager;
        _configuration = configuration;
        _mapper = mapper;
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

        bool emailExists = await _userManager.FindByEmailAsync(registerDTO.Email) is not null;
        bool phoneExists = !string.IsNullOrEmpty(registerDTO.PhoneNumber) &&
                           _userManager.Users.Any(u => u.PhoneNumber == registerDTO.PhoneNumber);

        if (emailExists || phoneExists)
            return Error.Validation("EmailOrPhone.Exists", "Email or Phone number already exists.");


        var user = new ApplicationUser()
        {
            Email = registerDTO.Email,
            DisplayName = registerDTO.DisplayName,
            PhoneNumber = registerDTO.PhoneNumber,
            UserName = registerDTO.UserName
        };

        var identityResult = await _userManager.CreateAsync(user, registerDTO.Password);
        if (identityResult.Succeeded)
        {
            var token = await CreateTokenAsync(user);
            return new UserDTO(user.Email!, user.DisplayName, token);
        }

        return identityResult.Errors.Select(e => Error.Validation(e.Code, e.Description)).ToList();
    }

    public async Task<bool> CheckEmailAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        return user != null;
    }

    public async Task<Result<UserDTO>> GetUserByEmailAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user is null)
            return Error.NotFound("User.NotFound", $"No User with Email {email} Was Found");

        return new UserDTO(user.Email!, user.DisplayName, await CreateTokenAsync(user));
    }

    public async Task<Result<AddressDTO>> GetUserAddressAsync(string email)
    {
        var user = await _userManager.Users.Include(x => x.Address)
                                           .FirstOrDefaultAsync(x => x.Email == email);

        if (user is null)
            return Error.NotFound("User.NotFound", $"User with this email:{email} was not found");

        if (user.Address is null)
            return Error.NotFound("Address.NotFound", $"Address for user with this email:{email} was not found");

        return _mapper.Map<AddressDTO>(user.Address);

    }

    public async Task<Result<AddressDTO>> UpdateUserAddressAsync(AddressDTO addressDTO, string email)
    {
        var user = await _userManager.Users.Include(x => x.Address)
                                    .FirstOrDefaultAsync(x => x.Email == email);

        if (user is null)
            return Error.NotFound("User.NotFound", $"User with this email:{email} was not found");

        if (user.Address is not null)
        {
            user.Address.FirstName = addressDTO.FirstName;
            user.Address.LastName = addressDTO.LastName;
            user.Address.Street = addressDTO.Street;
            user.Address.City = addressDTO.City;
            user.Address.Country = addressDTO.Country;
        }
        else
        {
            user.Address = _mapper.Map<AddressDTO, Address>(addressDTO);
        }

        await _userManager.UpdateAsync(user);

        return _mapper.Map<Address, AddressDTO>(user.Address!);
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