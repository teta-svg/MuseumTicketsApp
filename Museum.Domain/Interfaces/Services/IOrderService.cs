using Museum.Domain.DTOs;

namespace Museum.Domain.Interfaces.Services
{
    public interface IOrderService
    {
        Task<OrderDto> CreateOrderAsync(string userId, CreateOrderDto createOrderDto);

        Task<List<OrderDto>> GetUserOrdersAsync(string userId);

        Task<bool> CheckTicketAvailabilityAsync(int ticketId, int quantity);
        Task<OrderDto?> GetOrderByIdAsync(int orderId);
    }
}