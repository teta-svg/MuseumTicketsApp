using Microsoft.EntityFrameworkCore;
using Museum.Application.DTOs;
using Museum.Application.Interfaces;
using Museum.Domain;

namespace Museum.Persistence.Repositories
{
    public class ExhibitionRepository : IExhibitionRepository
    {
        private readonly MuseumTicketsDBContext _context;

        public ExhibitionRepository(MuseumTicketsDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Exhibition>> GetAllAsync()
        {
            return await _context.Exhibitions
                .Include(e => e.MuseumExhibitions)
                    .ThenInclude(me => me.Museum)
                .Include(e => e.Tickets)
                    .ThenInclude(t => t.TicketPrices)
                .ToListAsync();
        }

        public async Task<IEnumerable<Exhibition>> GetFilteredAsync(ExhibitionFilterDto filter)
        {
            var query = _context.Exhibitions
                .Include(e => e.MuseumExhibitions)
                    .ThenInclude(me => me.Museum)
                        .ThenInclude(m => m.MuseumComplex)
                .Include(e => e.Tickets)
                    .ThenInclude(t => t.TicketPrices)
                .AsSplitQuery()
                .AsQueryable();


            //по названию выставки
            if (!string.IsNullOrEmpty(filter.Name))
                query = query.Where(e => e.Name.Contains(filter.Name));

            //по названию музея
            if (!string.IsNullOrEmpty(filter.MuseumName))
                query = query.Where(e => e.MuseumExhibitions
                                          .Any(me => me.Museum.Name.Contains(filter.MuseumName)));

            //по названию музейного комплекса
            if (!string.IsNullOrEmpty(filter.MuseumComplexName))
                query = query.Where(e => e.MuseumExhibitions
                                          .Any(me => me.Museum.MuseumComplex.Name
                                                      .Contains(filter.MuseumComplexName)));

            //по минимальной цене
            if (filter.MinPrice.HasValue)
                query = query.Where(e => e.Tickets
                                         .Where(t => t.TicketPrices.Any())
                                         .Min(t => t.TicketPrices.Min(tp => tp.Price)) >= filter.MinPrice.Value);

            //по максимальной цене
            if (filter.MaxPrice.HasValue)
                query = query.Where(e => e.Tickets
                                         .Where(t => t.TicketPrices.Any())
                                         .Min(t => t.TicketPrices.Min(tp => tp.Price)) <= filter.MaxPrice.Value);

            //по датам проведения
            if (filter.StartDate.HasValue)
            {
                var start = DateOnly.FromDateTime(filter.StartDate.Value);
                query = query.Where(e => e.MuseumExhibitions.Any(me => me.StartDate >= start));
            }

            if (filter.EndDate.HasValue)
            {
                var end = DateOnly.FromDateTime(filter.EndDate.Value);
                query = query.Where(e => e.MuseumExhibitions.Any(me => me.EndDate <= end));
            }

            return await query.ToListAsync();
        }

        public async Task<Exhibition?> GetByIdAsync(int id)
        {
            return await _context.Exhibitions
                .Include(e => e.MuseumExhibitions)
                    .ThenInclude(me => me.Museum)
                        .ThenInclude(m => m.MuseumComplex)
                .Include(e => e.MuseumExhibitions)
                    .ThenInclude(me => me.Museum)
                        .ThenInclude(m => m.MuseumSchedules)
                            .ThenInclude(ms => ms.ScheduleDays)
                .Include(e => e.Tickets)
                    .ThenInclude(t => t.TicketPrices)
                .FirstOrDefaultAsync(e => e.ExhibitionId == id);
        }

        public async Task AddAsync(Exhibition exhibition)
        {
            await _context.Exhibitions.AddAsync(exhibition);
        }

        public Task UpdateAsync(Exhibition exhibition)
        {
            _context.Exhibitions.Update(exhibition);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(Exhibition exhibition)
        {
            _context.Exhibitions.Remove(exhibition);
            return Task.CompletedTask;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}