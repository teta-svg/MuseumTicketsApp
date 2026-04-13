using Museum.Application.DTOs;
using Museum.Application.Interfaces;
namespace Museum.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public Task<OrderDto> CreateOrderAsync(string userIdStr, CreateOrderDto createOrderDto)
            => _orderRepository.CreateOrderAsync(userIdStr, createOrderDto);

        public Task<List<OrderDto>> GetUserOrdersAsync(string userIdStr)
            => _orderRepository.GetOrdersByUserIdAsync(userIdStr);

        public Task<bool> CheckTicketAvailabilityAsync(int ticketId, int quantity)
            => _orderRepository.CheckTicketAvailabilityAsync(ticketId, quantity);

        public async Task<OrderDto?> GetOrderByIdAsync(int orderId)
        {
            return await _orderRepository.GetOrderByIdAsync(orderId);
        }

    }
}