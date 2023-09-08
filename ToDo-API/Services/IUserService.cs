using ToDo_API.Controllers;
using ToDo_API.Models;

namespace ToDo_API.Services;

public interface IUserService
{
    public Task<AuthResponse> CreateUserWithPasswordAsync(RegisterDto registerDto);
    public Task<AuthResponse> LoginWithPasswordAsync(LoginDto loginDto);
}