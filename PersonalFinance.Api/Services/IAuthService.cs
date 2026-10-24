using PersonalFinance.Api.DTOs;
using PersonalFinance.Api.Models;

namespace PersonalFinance.Api.Services;

public interface IAuthService
{
    Task<User> RegisterAsync(RegisterDto dto);
    Task<User?> LoginAsync(LoginDto dto);
    string GenerateJwtToken(User user);
}