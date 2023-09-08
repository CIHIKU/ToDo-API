using System.Security.Claims;

namespace ToDo_API.Services;

public class RefreshTokenService : IRefreshTokenService
{
    public async Task<bool> ValidateRefreshTokenAsync(ClaimsPrincipal principal, string refreshToken)
    {
        throw new NotImplementedException();
    }

    public async Task StoreRefreshTokenAsync(string userId, string newRefreshToken)
    {
        throw new NotImplementedException();
    }
}