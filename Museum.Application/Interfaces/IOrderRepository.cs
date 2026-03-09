using Museum.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Museum.Application.Interfaces
{
    public interface IOrderRepository
    {
        Task<Ticket?> GetTicketWithPricesAsync(int ticketId);
        Task AddOrderAsync(Order order);
        Task<List<Order>> GetOrdersByUserAsync(int userId);
    }
}