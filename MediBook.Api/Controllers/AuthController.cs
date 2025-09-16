using MediBook.Application.Dtos;
using MediBook.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MediBook.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(IAuthService authService) : ControllerBase
{
    private readonly IAuthService _authService = authService;

    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login([FromBody] LoginRequestDto dto, CancellationToken cancellationToken)
    {
        var user = await _authService.LoginAsync(dto, cancellationToken);
        return user is null ? Unauthorized(new { message = "Invalid credentials." }) : Ok(user);
    }

    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> Register([FromBody] CreateUserDto dto, CancellationToken cancellationToken)
    {
        var user = await _authService.RegisterAsync(dto, cancellationToken);
        if (user is null)
        {
            return Conflict(new { message = "Email already exists." });
        }

        return CreatedAtAction(nameof(Login), new { email = user.Email }, user);
    }
}
