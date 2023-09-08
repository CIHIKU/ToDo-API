using Microsoft.AspNetCore.Mvc;
using ToDo_API.Services;

namespace ToDo_API.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService;

    public AuthController(IUserService userService)
    {
        _userService = userService;
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

    [HttpGet("oauth2/{provider}")]
    public async Task<IActionResult> OAuth2Login(string provider, string returnUrl = null!)
    {
        return Ok();
    }

    [HttpGet("oauth2/{provider}/callback")]
    public async Task<IActionResult> OAuth2Callback(string provider, [FromQuery] string code, [FromQuery] string state)
    {
        return Ok();
    }
}

public class RegisterDto
{
    public required string Email { get; set; }
    public required string Password { get; set; }
    // Other fields as necessary
}

public class LoginDto
{
    public required string Email { get; set; }
    public required string Password { get; set; }
    // Other fields as necessary
}

public class AuthResponse
{
    public required string UserId { get; set; }
    public required string JwtToken { get; set; }
}