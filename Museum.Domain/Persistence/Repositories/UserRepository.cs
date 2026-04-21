using Microsoft.EntityFrameworkCore;
using Museum.Domain.Interfaces.Repositories;
using Museum.Domain.Entities;

namespace Museum.Domain.Persistence.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly MuseumTicketsDBContext _context;

        public UserRepository(MuseumTicketsDBContext context)
        {
            _context = context;
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Delete(User user)
        {
            _context.Users.Remove(user);
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }
    }
}