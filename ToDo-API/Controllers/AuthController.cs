using Microsoft.AspNetCore.Mvc;
using ToDo_API.Services;

namespace ToDo_API.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService;
    private IConfiguration _configuration;
    private IHttpClientFactory _httpClientFactory;

    public AuthController(IUserService userService, IConfiguration configuration, IHttpClientFactory httpClientFactory)
    {
        _userService = userService;
        _configuration = configuration;
        _httpClientFactory = httpClientFactory;
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterWithPassword([FromBody] RegisterDto registerDto)
    {
        if (registerDto == null) throw new ArgumentNullException(nameof(registerDto));

        var response =  await _userService.CreateUserWithPasswordAsync(registerDto);
        
        return Ok(response);
    }

    [HttpPost("login")]
    public async Task<IActionResult> LoginWithPassword([FromBody] LoginDto loginDto)
    {
        if (loginDto == null) throw new ArgumentNullException(nameof(loginDto));

        var response = await _userService.LoginWithPasswordAsync(loginDto);
        
        return Ok(response);
    }
    
}

public class RegisterDto
{
    public required string Email { get; set; }
    public required string Password { get; set; }
}

public class LoginDto
{
    public required string Email { get; set; }
    public required string Password { get; set; }
}

public class AuthResponse
{
    public required string UserId { get; set; }
    public required string JwtToken { get; set; }
}