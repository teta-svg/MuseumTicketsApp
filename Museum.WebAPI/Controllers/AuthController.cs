using Microsoft.AspNetCore.Mvc;
using Museum.Application.DTOs;
using Museum.Application.Interfaces;

namespace Museum.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")] //api/auth/..
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")] //login
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var token = await _authService.Login(request.Email, request.Password);

        if (token == null)
        {
            return Unauthorized(new { message = "Неверный email или пароль" });
        }

        return Ok(new { token });
    }

    [HttpPost("register")] //register
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        try
        {
            await _authService.Register(request);
            return Ok(new { message = "Регистрация успешна!" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

}
