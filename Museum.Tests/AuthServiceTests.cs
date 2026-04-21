using Moq;
using Microsoft.Extensions.Configuration;
using Museum.Domain.Entities;
using Museum.Domain.Interfaces.Repositories;
using Museum.Domain.Interfaces.Services;

namespace Museum.Tests
{
    public class AuthServiceTests
    {
        private readonly Mock<IUserRepository> _userRepoMock = new();
        private readonly Mock<IPasswordHasher> _hasherMock = new();
        private readonly Mock<IConfiguration> _configMock = new();
        private readonly AuthService _authService;

        public AuthServiceTests()
        {
            _configMock.Setup(c => c["Jwt:Key"]).Returns("super_secret_key_1234567890_long_enough");
            _configMock.Setup(c => c["Jwt:Issuer"]).Returns("MuseumBackend");
            _configMock.Setup(c => c["Jwt:Audience"]).Returns("MuseumClient");

            _authService = new AuthService(_userRepoMock.Object, _configMock.Object, _hasherMock.Object);
        }

        [Fact]
        public async Task Login_ReturnToken_WhenCredentialsAreValid()
        {
            var user = new User { Email = "test@mail.com", Password = "hashed_password", Role = "Посетитель" };
            _userRepoMock.Setup(r => r.GetByEmailAsync(user.Email)).ReturnsAsync(user);
            _hasherMock.Setup(h => h.VerifyPassword("raw_password", "hashed_password")).Returns(true);

            var token = await _authService.LoginAsync("test@mail.com", "raw_password");

            Assert.NotNull(token);
            Assert.NotEmpty(token);
        }

        [Fact]
        public async Task Register_ThrowException_WhenEmailExists()
        {
            var request = new RegisterRequest { Email = "exists@mail.com", Password = "123" };
            _userRepoMock.Setup(r => r.GetByEmailAsync(request.Email)).ReturnsAsync(new User());

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _authService.RegisterAsync(request));

            Assert.Equal("Пользователь с такой почтой уже существует", exception.Message);
        }
    }
}
