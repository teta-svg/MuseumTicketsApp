using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Museum.Application.Interfaces;
using Museum.Domain;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepo;
    private readonly IConfiguration _configuration;
    private readonly IPasswordHasher _passwordHasher;

    public AuthService(
        IUserRepository userRepo,
        IConfiguration configuration,
        IPasswordHasher passwordHasher)
    {
        _userRepo = userRepo;
        _configuration = configuration;
        _passwordHasher = passwordHasher;
    }

    public async Task<string?> LoginAsync(string email, string password)
    {
        var user = await _userRepo.GetByEmailAsync(email);

        if (user == null || !_passwordHasher.VerifyPassword(password, user.Password))
            return null;

        return GenerateJwtToken(user);
    }

    public async Task RegisterAsync(RegisterRequest request)
    {
        if (await _userRepo.GetByEmailAsync(request.Email) != null)
            throw new InvalidOperationException("Пользователь с такой почтой уже существует");

        var user = new User
        {
            Email = request.Email,
            Password = _passwordHasher.HashPassword(request.Password),
            FirstName = request.FirstName,
            LastName = request.LastName,
            MiddleName = request.MiddleName,
            Phone = request.Phone,
            Role = "Посетитель"
        };

        await _userRepo.AddAsync(user);
        await _userRepo.SaveChangesAsync();
    }

    private string GenerateJwtToken(User user)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new Claim(JwtRegisteredClaimNames.Sub, user.Email),
            new Claim(ClaimTypes.Role, user.Role),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? throw new InvalidOperationException("Jwt:Key not configured"))
        );

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}