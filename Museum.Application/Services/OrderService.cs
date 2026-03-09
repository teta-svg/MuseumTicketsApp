using Museum.Application.DTOs;
using Museum.Application.Interfaces;
using Museum.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Museum.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _repository;

        public OrderService(IOrderRepository repository)
        {
            _repository = repository;
        }

        public async Task<OrderDTO> CreateOrderAsync(int userId, CreateOrderDTO dto)
        {
            var order = new Order
            {
                UserId = userId,
                Status = "В ожидании",
                OrderDate = DateTime.UtcNow
            };

            foreach (var item in dto.Items)
            {
                var ticket = await _repository.GetTicketWithPricesAsync(item.TicketID);
                if (ticket == null || ticket.AvailableQuantity < item.Quantity)
                    throw new InvalidOperationException($"Недостаточно билетов для {item.TicketID}");

                ticket.AvailableQuantity -= item.Quantity;

                order.OrderItems.Add(new OrderItem
                {
                    TicketId = item.TicketID,
                    Quantity = item.Quantity,
                    PriceAtPurchase = ticket.TicketPrices.OrderBy(tp => tp.StartDate).First().Price
                });
            }

            await _repository.AddOrderAsync(order);

            return new OrderDTO
            {
                OrderID = order.OrderId,
                OrderDate = order.OrderDate,
                Status = order.Status,
                Items = order.OrderItems.Select(oi => new OrderItemDTO
                {
                    ExhibitionName = oi.Ticket.Exhibition.Name,
                    TicketType = oi.Ticket.Type,
                    Quantity = oi.Quantity,
                    PriceAtPurchase = oi.PriceAtPurchase
                }).ToList()
            };
        }

        public async Task<List<OrderDTO>> GetUserOrdersAsync(int userId)
        {
            var orders = await _repository.GetOrdersByUserAsync(userId);

            return orders.Select(o => new OrderDTO
            {
                OrderID = o.OrderId,
                OrderDate = o.OrderDate,
                Status = o.Status,
                Items = o.OrderItems.Select(oi => new OrderItemDTO
                {
                    ExhibitionName = oi.Ticket.Exhibition.Name,
                    TicketType = oi.Ticket.Type,
                    Quantity = oi.Quantity,
                    PriceAtPurchase = oi.PriceAtPurchase
                }).ToList()
            }).ToList();
        }
    }
}