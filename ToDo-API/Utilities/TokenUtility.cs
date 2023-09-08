using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using ToDo_API.Services;

namespace ToDo_API.Utilities;

public class TokenUtility : ITokenUtility
{
    private readonly IConfiguration _configuration;
    private readonly IRefreshTokenService _refreshTokenService;

    public TokenUtility(IConfiguration configuration, IRefreshTokenService refreshTokenService)
    {
        _configuration = configuration;
        _refreshTokenService = refreshTokenService;
    }

    public string GenerateJwtToken(Claim[] claims)
    {
        var issuer = _configuration["Jwt:Issuer"]!;
        var audience = _configuration["Jwt:Audience"]!;
        var secretKey = _configuration["Jwt:SecretKey"]!;
        
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.Now.AddHours(1),
            signingCredentials: credentials
            );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    
    public string GenerateRandomToken()
    {
        var randomNumber = new byte[32];
        using var randomNumberGenerator = RandomNumberGenerator.Create();
        randomNumberGenerator.GetBytes(randomNumber);

        return Convert.ToBase64String(randomNumber);
    }
    
    public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        var secretKey = _configuration["Jwt:SecretKey"]!;
        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = signingKey,
            ValidateLifetime = false
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);

        if (securityToken is not JwtSecurityToken jwtSecurityToken 
            || !jwtSecurityToken.Header.Alg.Equals(
                SecurityAlgorithms.HmacSha256Signature, 
                StringComparison.InvariantCultureIgnoreCase
                )
            )
            throw new SecurityTokenException("Invalid token");

        return principal;
    }

    public async Task<string> RefreshToken(string expiredToken, string refreshToken)
    {
        var principal = GetPrincipalFromExpiredToken(expiredToken);

        var isValidRefreshToken = await _refreshTokenService.ValidateRefreshTokenAsync(principal, refreshToken);
        if (!isValidRefreshToken)
        {
            throw new SecurityTokenException("Invalid refresh token");
        }

        var newJwtToken = GenerateJwtToken(principal.Claims.ToArray());
        var newRefreshToken = GenerateRandomToken();

        var userId = principal.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
        await _refreshTokenService.StoreRefreshTokenAsync(userId, newRefreshToken);

        return newJwtToken;

    }
}