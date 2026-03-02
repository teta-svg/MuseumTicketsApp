using Museum.Application.Interfaces;
using BC = BCrypt.Net.BCrypt; //BCrypt.Net-Next

namespace Museum.Application.Services;

public class PasswordHasher : IPasswordHasher
{
    public string HashPassword(string password) => BC.HashPassword(password);
    public bool VerifyPassword(string password, string hashedPassword) => BC.Verify(password, hashedPassword);

}