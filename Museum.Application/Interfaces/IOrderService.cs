using Museum.Application.DTOs;

namespace Museum.Application.Interfaces
{
    public interface IOrderService
    {
        Task<OrderDto> CreateOrderAsync(string userId, CreateOrderDto createOrderDto);

        Task<List<OrderDto>> GetUserOrdersAsync(string userId);

        Task<bool> CheckTicketAvailabilityAsync(int ticketId, int quantity);
        Task<OrderDto?> GetOrderByIdAsync(int orderId);
    }
}