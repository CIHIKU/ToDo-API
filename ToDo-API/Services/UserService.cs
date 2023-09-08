using MongoDB.Driver.Linq;
using ToDo_API.Controllers;
using ToDo_API.Models;
using ToDo_API.Repositories;
using ToDo_API.Utilities;

namespace ToDo_API.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenUtility _tokenUtility;
    private readonly IPasswordUtility _passwordUtility;
    
    public UserService(IUserRepository userRepository, ITokenUtility tokenUtility, IPasswordUtility passwordUtility)
    {
        _userRepository = userRepository;
        _tokenUtility = tokenUtility;
        _passwordUtility = passwordUtility;
    }

    public async Task<AuthResponse> CreateUserWithPasswordAsync(RegisterDto registerDto)
    {
        if (registerDto == null)
            throw new ArgumentNullException(nameof(registerDto));

        if (string.IsNullOrWhiteSpace(registerDto.Email) || !IsValidEmail(registerDto.Email))
            throw new ArgumentException("Invalid email address");

        var passwordValidate = _passwordUtility.Validate(registerDto.Password);
        
        if (!passwordValidate.IsValid)
            throw new ArgumentException(passwordValidate.ErrorMessage);
        
        var existingUser = await _userRepository.GetUserByEmailAsync(registerDto.Email);
        if (existingUser != null)
            throw new InvalidOperationException("A user with this email address already exists.");

        _passwordUtility.CreatePasswordHash(registerDto.Password, out var passwordHash,out var passwordSalt);
        
        var user = new User
        {
            Email = registerDto.Email,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt,
            PasswordResetToken = _tokenUtility.GenerateRandomToken(),
            JwtToken = _tokenUtility.GenerateJwtToken(null!),
            JwtTokenExpiration = DateTime.UtcNow.AddHours(1),
            RefreshToken = _tokenUtility.GenerateRandomToken()
        };
        
        await _userRepository.CreateUserAsync(user);

        var response = await _userRepository.GetUserByEmailAsync(user.Email);

        return new AuthResponse
        {
            UserId = response.Id,
            JwtToken = response.JwtToken
        };
    }

    public async Task<AuthResponse> LoginWithPasswordAsync(LoginDto loginDto)
    {
        if (loginDto == null) 
            throw new ArgumentNullException(nameof(loginDto));
        
        if (!IsValidEmail(loginDto.Email))
            throw new ArgumentException("Invalid email address");
        
        if (string.IsNullOrWhiteSpace(loginDto.Password))
            throw new ArgumentException("Password cannot be empty or whitespace.");
        
        var user = await _userRepository.GetUserByEmailAsync(loginDto.Email);
        
        if (user.PasswordSalt == null || user.PasswordHash == null)
            throw new ArgumentException("Password doesn't exists.");
        
        var isVerified = _passwordUtility.VerifyPasswordHash(loginDto.Password, user.PasswordHash!, user.PasswordSalt!);

        if (!isVerified)
            throw new ArgumentException("Email or password doesn't match.");
        
        return new AuthResponse
        {
            UserId = user.Id,
            JwtToken = user.JwtToken
        };
    }
    
    private bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }
}