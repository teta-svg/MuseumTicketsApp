using Museum.Domain;

namespace Museum.Application.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetByEmailAsync(string email);
        Task AddAsync(User user);
        Task SaveChangesAsync();
        void Delete(User user);
        Task<List<User>> GetAllAsync();
    }
}