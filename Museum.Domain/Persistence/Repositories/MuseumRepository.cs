using Microsoft.EntityFrameworkCore;
using Museum.Domain.Entities;

namespace Museum.Domain.Persistence.Repositories
{
    public class MuseumRepository : IMuseumRepository
    {
        private readonly MuseumTicketsDBContext _context;

        public MuseumRepository(MuseumTicketsDBContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Domain.Entities.Museum museum)
        {
            await _context.Museums.AddAsync(museum);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<Domain.Entities.Museum?> GetByAddressAsync(string name, string city, string street, string house)
        {
            return await _context.Museums
                .FirstOrDefaultAsync(m => m.Name == name
                                       && m.City == city
                                       && m.Street == street
                                       && m.House == house);
        }

        public async Task<MuseumComplex?> GetComplexByNameAsync(string name)
        {
            return await _context.MuseumComplexes
                .FirstOrDefaultAsync(c => c.Name.ToLower() == name.ToLower());
        }

        public async Task AddComplexAsync(MuseumComplex complex)
        {
            await _context.MuseumComplexes.AddAsync(complex);
        }


    }
}