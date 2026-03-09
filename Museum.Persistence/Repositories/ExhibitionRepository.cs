using Microsoft.EntityFrameworkCore;
using Museum.Application.Interfaces;
using Museum.Domain;
using System;
using System.Collections.Generic;
using System.Text;

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
    }
}
