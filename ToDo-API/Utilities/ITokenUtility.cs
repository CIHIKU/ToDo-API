using System.Security.Claims;

namespace ToDo_API.Utilities;

public interface ITokenUtility
{
    public string GenerateJwtToken(Claim[] claims);
    public string GenerateRandomToken();
    public ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    public Task<string> RefreshToken(string expiredToken, string refreshToken);
    
}