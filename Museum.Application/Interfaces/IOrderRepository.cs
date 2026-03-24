using Museum.Application.DTOs;

namespace Museum.Application.Interfaces
{
    public interface IOrderRepository
    {
        Task<OrderDto> CreateOrderAsync(string userIdStr, CreateOrderDto createOrderDto);
        Task<List<OrderDto>> GetOrdersByUserIdAsync(string userIdStr);
        Task<bool> CheckTicketAvailabilityAsync(int ticketId, int quantity);
        Task UpdateOrderStatusAsync(int orderId, string status);
        Task<OrderDto?> GetOrderByIdAsync(int orderId);
        Task<List<OrderDto>> GetAllAsync();
    }
}