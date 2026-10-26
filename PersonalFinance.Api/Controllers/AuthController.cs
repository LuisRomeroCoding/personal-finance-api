using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalFinance.Api.DTOs;
using PersonalFinance.Api.Extensions;
using PersonalFinance.Api.Models;
using PersonalFinance.Api.Services;

namespace PersonalFinance.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto)
    {
        try
        {
            User user = await _authService.RegisterAsync(dto);
            return Ok(new { message = "User registered succesfully", userId = user.Id });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var user = await _authService.LoginAsync(dto);
        if (user == null)
        {
            return Unauthorized(new { message = "Invalid email or password" });
        }

        string token = _authService.GenerateJwtToken(user);

        /*For local development before you configure HTTPS certificates, 
         * you can temporarily set Secure = false in the cookie options 
         * so your browser accepts it over http://localhost, 
         * then flip it back to true when deploying
        */
        Response.Cookies.Append("accessToken", token, new CookieOptions
        {
            HttpOnly = false,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTime.UtcNow.AddMinutes(15)
        });

        return Ok(new { message = "Logged in successfully" });
    }

    [HttpPost("logout")]
    public IActionResult Logout()
    {
        Response.Cookies.Delete("accessToken");
        return Ok(new { message = "Logged out successfully" });
    }

    [HttpGet("me")]
    [Authorize]
    public IActionResult GetCurrentUser()
    {
        try
        {
            int userId = User.GetUserId();
            string email = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value ?? string.Empty;

            return Ok(new { userId, email });
        }
        catch (Exception ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
    }
}