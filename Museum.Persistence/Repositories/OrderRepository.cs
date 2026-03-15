using Microsoft.EntityFrameworkCore;
using Museum.Application.DTOs;
using Museum.Application.Interfaces;
using Museum.Domain;

namespace Museum.Persistence.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly MuseumTicketsDBContext _context;

        public OrderRepository(MuseumTicketsDBContext context)
        {
            _context = context;
        }

        public async Task<bool> CheckTicketAvailabilityAsync(int ticketId, int quantity)
        {
            var ticket = await _context.Tickets.FindAsync(ticketId);
            if (ticket == null) return false;
            return ticket.AvailableQuantity >= quantity;
        }

        public async Task<OrderDto> CreateOrderAsync(string userIdStr, CreateOrderDto createOrderDto)
        {
            if (!int.TryParse(userIdStr, out int userId))
                throw new ArgumentException("Invalid user ID");

            var validTickets = createOrderDto.Tickets
                .Where(t => t.Quantity > 0)
                .ToList();

            if (!validTickets.Any())
                throw new ArgumentException("Невозможно создать заказ без билетов");

            var order = new Order
            {
                UserId = userId,
                OrderDate = DateTime.UtcNow,
                Status = "В ожидании"
            };

            foreach (var t in validTickets)
            {
                var ticket = await _context.Tickets
                    .Include(ti => ti.TicketPrices)
                    .FirstOrDefaultAsync(ti => ti.TicketId == t.TicketId);

                if (ticket == null)
                    throw new InvalidOperationException($"Ticket {t.TicketId} не найден");

                if (ticket.AvailableQuantity < t.Quantity)
                    throw new InvalidOperationException($"Недостаточно билетов для TicketID {t.TicketId}");

                var price = ticket.TicketPrices.OrderBy(tp => tp.StartDate)
                    .FirstOrDefault()?.Price ?? 0m;

                order.OrderItems.Add(new OrderItem
                {
                    TicketId = ticket.TicketId,
                    Quantity = t.Quantity,
                    PriceAtPurchase = price
                });

                ticket.AvailableQuantity -= t.Quantity;
                if (ticket.AvailableQuantity == 0)
                {
                    ticket.Status = "Продан";
                }
            }

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return new OrderDto
            {
                OrderId = order.OrderId,
                CreatedAt = order.OrderDate,
                Status = order.Status,
                Tickets = order.OrderItems.Select(oi => new OrderTicketDto
                {
                    TicketId = oi.TicketId,
                    Quantity = oi.Quantity,
                    Price = oi.PriceAtPurchase,
                    TicketType = oi.Ticket.Type
                }).ToList()
            };
        }

        public async Task<List<OrderDto>> GetOrdersByUserIdAsync(string userIdStr)
        {
            if (!int.TryParse(userIdStr, out int userId))
                throw new ArgumentException("Invalid user ID");

            var orders = await _context.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Ticket)
                .Where(o => o.UserId == userId)
                .ToListAsync();

            return orders.Select(o => new OrderDto
            {
                OrderId = o.OrderId,
                CreatedAt = o.OrderDate,
                Status = o.Status,
                Tickets = o.OrderItems.Select(oi => new OrderTicketDto
                {
                    TicketId = oi.TicketId,
                    Quantity = oi.Quantity,
                    Price = oi.PriceAtPurchase,
                    TicketType = oi.Ticket.Type
                }).ToList()
            }).ToList();
        }

        public async Task UpdateOrderStatusAsync(int orderId, string status)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null)
                throw new InvalidOperationException("Заказ не найден");

            order.Status = status;
            await _context.SaveChangesAsync();
        }

        public async Task<OrderDto?> GetOrderByIdAsync(int orderId)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Ticket)
                .FirstOrDefaultAsync(o => o.OrderId == orderId);

            if (order == null) return null;

            return new OrderDto
            {
                OrderId = order.OrderId,
                CreatedAt = order.OrderDate,
                Status = order.Status,
                Tickets = order.OrderItems.Select(oi => new OrderTicketDto
                {
                    TicketId = oi.TicketId,
                    Quantity = oi.Quantity,
                    Price = oi.PriceAtPurchase,
                    TicketType = oi.Ticket.Type
                }).ToList()
            };
        }
    }
}