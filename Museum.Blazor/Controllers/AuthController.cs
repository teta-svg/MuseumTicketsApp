using Microsoft.AspNetCore.Mvc;
using Museum.Application.DTOs;
using Museum.Application.Interfaces;
using Museum.Application.Interfaces.Repositories;

namespace Museum.WebAPI.Controllers;

[ApiController]
[IgnoreAntiforgeryToken]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    private readonly IUserRepository _userRepo;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IAuthService authService, IUserRepository userRepo, IPasswordHasher passwordHasher, ILogger<AuthController> logger)
    {
        _authService = authService;
        _userRepo = userRepo;
        _passwordHasher = passwordHasher;
        _logger = logger;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var user = await _userRepo.GetByEmailAsync(request.Email);
        if (user != null && _passwordHasher.VerifyPassword(request.Password, user.Password))
        {
            var token = await _authService.LoginAsync(request.Email, request.Password);
            if (token != null)
            {
                return Ok(new { token });
            }
        }

        _logger.LogWarning("Login failed for {Email}", request.Email);
        return Unauthorized(new { message = "Неверный email или пароль" });
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        try
        {
            await _authService.RegisterAsync(request);
            return Ok(new { message = "Регистрация успешна!" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}