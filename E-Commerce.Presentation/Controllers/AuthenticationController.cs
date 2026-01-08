using E_Commerce.Services_Abstraction;
using E_Commerce.Shared.CommonResult;
using E_Commerce.Shared.DTOs.IdentityDTOs;
using E_Commerce.Shared.DTOs.OrderDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace E_Commerce.Presentation.Controllers;

public class AuthenticationController : ApiBaseController
{
    private readonly IAuthenticationService _authenticationService;

    public AuthenticationController(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    [HttpPost("Login")]
    public async Task<ActionResult<UserDTO>> Login(LoginDTO loginDto)
    {
        var result = await _authenticationService.LoginAsync(loginDto);

        if (!result.IsSuccess)
            return HandleResult(result);

        SetRefreshTokenCookie(result.Value.RefershToken, result.Value.RefershTokenExpired);

        return Ok(result.Value);
    }

    [HttpPost("Register")]
    public async Task<ActionResult<UserDTO>> Register(RegisterDTO registerDto)
    {
        var result = await _authenticationService.RegisterAsync(registerDto);

        if (!result.IsSuccess)
            return HandleResult(result);

        SetRefreshTokenCookie(result.Value.RefershToken, result.Value.RefershTokenExpired);

        return Ok(result.Value);
    }

    [HttpGet("emailExists")]
    public async Task<ActionResult<bool>> CheckEmail(string email)
    {
        var result = await _authenticationService.CheckEmailAsync(email);
        return Ok(result);
    }

    [Authorize]
    [HttpGet("currentUser")]
    public async Task<ActionResult<UserDTO>> GetCurrentUser()
    {
        var email = GetEmailFromToken();
        var result = await _authenticationService.GetUserByEmailAsync(email);
        return HandleResult(result);
    }

    [ProducesResponseType<AddressDTO>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
    [Authorize]
    [HttpGet("Address")]
    public async Task<ActionResult<AddressDTO>> GetAddress()
    {
        var email = GetEmailFromToken();
        var result = await _authenticationService.GetUserAddressAsync(email);
        return HandleResult(result);
    }

    [Authorize]
    [HttpPut("Address")]
    public async Task<ActionResult<AddressDTO>> UpdateAddress(AddressDTO addressDTO)
    {
        var email = GetEmailFromToken();
        var result = await _authenticationService.UpdateUserAddressAsync(addressDTO, email);
        return HandleResult(result);
    }

    [HttpGet("refreshToken")]
    public async Task<ActionResult<UserDTO>> RefreshToken()
    {
        var oldToken = Request.Cookies["refreshToken"];
        var result = await _authenticationService.RefreshTokenAsync(oldToken!);

        if (!result.IsSuccess) return Unauthorized();

        SetRefreshTokenCookie(result.Value.RefershToken, result.Value.RefershTokenExpired);

        return Ok(result.Value);
    }



}