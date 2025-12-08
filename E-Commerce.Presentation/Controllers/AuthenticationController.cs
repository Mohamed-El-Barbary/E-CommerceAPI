using System.Security.Claims;
using E_Commerce.Services_Abstraction;
using E_Commerce.Shared.CommonResult;
using E_Commerce.Shared.DTOs.IdentityDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Presentation.Controllers;

public class AuthenticationController : ApiBaseController
{
    private readonly IAuthenticationService _authenticationService;

    public AuthenticationController(IAuthenticationService  authenticationService)
    {
        _authenticationService = authenticationService;
    }
    
    // Login
    // POST : baseUrl/api/Authentication/Login
    [HttpPost("Login")]
    public async Task<ActionResult<UserDTO>> Login(LoginDTO loginDto)
    {
        var result = await _authenticationService.LoginAsync(loginDto);
        return HandleResult(result);
    }
    
    // Register
    // POST : baseUrl/api/Authentication/Register
    [HttpPost("Register")]
    public async Task<ActionResult<UserDTO>> Register(RegisterDTO registerDto)
    {
        var result = await _authenticationService.RegisterAsync(registerDto);
        return HandleResult(result);
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
        var email = User.FindFirstValue(ClaimTypes.Email);
        var result = await _authenticationService.GetUserByEmailAsync(email!);
        return HandleResult(result);
    }

}