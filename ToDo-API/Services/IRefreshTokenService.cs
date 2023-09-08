using System.Security.Claims;

namespace ToDo_API.Services;

public interface IRefreshTokenService
{
    Task<bool> ValidateRefreshTokenAsync(ClaimsPrincipal principal, string refreshToken);
    Task StoreRefreshTokenAsync(string userId, string newRefreshToken);
}