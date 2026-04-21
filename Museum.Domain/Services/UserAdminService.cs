using Museum.Domain.Entities;
using Museum.Domain.Interfaces.Repositories;
using Museum.Domain.Interfaces.Services;

namespace Museum.Domain.Services
{
    public class UserAdminService: IUserAdminService
    {
        private readonly IUserRepository _userRepo;
        private readonly IPasswordHasher _passwordHasher;

        public UserAdminService(IUserRepository userRepo, IPasswordHasher passwordHasher)
        {
            _userRepo = userRepo;
            _passwordHasher = passwordHasher;
        }

        public async Task CreateUserAsync(string email, string password, string firstName, string lastName, string? middleName, string? phone, string role)
        {
            if (await _userRepo.GetByEmailAsync(email) != null)
                throw new Exception("Пользователь уже существует");

            await _userRepo.AddAsync(new User
            {
                Email = email,
                Password = _passwordHasher.HashPassword(password),
                FirstName = firstName,
                LastName = lastName,
                MiddleName = middleName,
                Phone = phone,
                Role = role
            });
            await _userRepo.SaveChangesAsync();
        }

        public async Task DeleteUserAsync(string email)
        {
            var user = await _userRepo.GetByEmailAsync(email) ?? throw new Exception("Пользователь не найден");
            _userRepo.Delete(user);
            await _userRepo.SaveChangesAsync();
        }
    }

}
