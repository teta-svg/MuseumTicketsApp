using Microsoft.EntityFrameworkCore;
using Museum.Application.Interfaces;
using Museum.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Museum.Persistence.Repositories
{
    public class TicketRepository : ITicketRepository
    {
        private readonly MuseumTicketsDBContext _context;

        public TicketRepository(MuseumTicketsDBContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Ticket ticket)
        {
            await _context.Tickets.AddAsync(ticket);
        }

        public async Task<Ticket?> GetByIdAsync(int id)
        {
            return await _context.Tickets
                .Include(t => t.TicketPrices)
                .FirstOrDefaultAsync(t => t.TicketId == id);
        }

        public async Task<List<Ticket>> GetTicketsByExhibitionAsync(int exhibitionId)
        {
            return await _context.Tickets
                .Include(t => t.TicketPrices)
                .Where(t => t.ExhibitionId == exhibitionId)
                .ToListAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
        public async Task<List<Ticket>> GetAllAsync()
        {
            return await _context.Tickets.Include(t => t.TicketPrices).ToListAsync();
        }
    }
}
