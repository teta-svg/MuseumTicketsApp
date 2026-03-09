using Microsoft.EntityFrameworkCore;
using Museum.Application.Interfaces;
using Museum.Domain;
using Museum.Persistence;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Museum.Persistence.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly MuseumTicketsDBContext _context;

        public OrderRepository(MuseumTicketsDBContext context)
        {
            _context = context;
        }

        public Task<Ticket?> GetTicketWithPricesAsync(int ticketId)
        {
            return _context.Tickets
                .Include(t => t.TicketPrices)
                .Include(t => t.Exhibition)
                .FirstOrDefaultAsync(t => t.TicketId == ticketId);
        }

        public async Task AddOrderAsync(Order order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
        }

        public Task<List<Order>> GetOrdersByUserAsync(int userId)
        {
            return _context.Orders
                .Where(o => o.UserId == userId)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Ticket)
                        .ThenInclude(t => t.Exhibition)
                .ToListAsync();
        }
    }
}