using Museum.Application.Services;

namespace Museum.Tests
{
    public class PasswordHasherTests
    {
        private readonly PasswordHasher _hasher = new();

        [Fact]
        public void HashPassword_ReturnNotEmptyString()
        {
            string password = "MySecurePassword123";

            string hash = _hasher.HashPassword(password);

            Assert.False(string.IsNullOrWhiteSpace(hash));
            Assert.NotEqual(password, hash);
        }

        [Fact]
        public void VerifyPassword_ReturnTrue_WhenPasswordIsCorrect()
        {
            string password = "CorrectPassword";
            string hash = _hasher.HashPassword(password);

            bool result = _hasher.VerifyPassword(password, hash);

            Assert.True(result);
        }

        [Fact]
        public void VerifyPassword_ReturnFalse_WhenPasswordIsIncorrect()
        {
            string password = "CorrectPassword";
            string wrongPassword = "WrongPassword";
            string hash = _hasher.HashPassword(password);

            bool result = _hasher.VerifyPassword(wrongPassword, hash);

            Assert.False(result);
        }
    }
}
