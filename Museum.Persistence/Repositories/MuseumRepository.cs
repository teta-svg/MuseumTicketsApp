using Microsoft.EntityFrameworkCore;
using Museum.Application.Interfaces;
using Museum.Domain;

namespace Museum.Persistence.Repositories
{
    public class MuseumRepository : IMuseumRepository
    {
        private readonly MuseumTicketsDBContext _context;

        public MuseumRepository(MuseumTicketsDBContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Museum.Domain.Museum museum)
        {
            await _context.Museums.AddAsync(museum);
        }

        public async Task<Museum.Domain.Museum?> GetByNameAsync(string name)
        {
            return await _context.Museums
                .FirstOrDefaultAsync(m => m.Name == name);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<Museum.Domain.Museum?> GetByAddressAsync(string name, string city, string street, string house)
        {
            return await _context.Museums
                .FirstOrDefaultAsync(m => m.Name == name
                                       && m.City == city
                                       && m.Street == street
                                       && m.House == house);
        }

    }
}