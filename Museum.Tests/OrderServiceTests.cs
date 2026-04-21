using Moq;
using Museum.Application.Services;
using Museum.Domain.Interfaces.Repositories;
using Museum.Domain.DTOs;

namespace Museum.Tests
{
    public class OrderServiceTests
    {
        private readonly Mock<IOrderRepository> _orderRepoMock = new();
        private readonly OrderService _service;

        public OrderServiceTests()
        {
            _service = new OrderService(_orderRepoMock.Object);
        }

        [Fact]
        public async Task CheckTicketAvailability_ReturnTrue_WhenRepoReturnsTrue()
        {
            int ticketId = 10;
            int quantity = 2;
            _orderRepoMock.Setup(r => r.CheckTicketAvailabilityAsync(ticketId, quantity))
                          .ReturnsAsync(true);

            var result = await _service.CheckTicketAvailabilityAsync(ticketId, quantity);

            Assert.True(result);
            _orderRepoMock.Verify(r => r.CheckTicketAvailabilityAsync(ticketId, quantity), Times.Once);
        }

        [Fact]
        public async Task GetOrderById_ReturnOrder_WhenExists()
        {
            int orderId = 1007;
            var expectedOrder = new OrderDto
            {
                OrderId = orderId,
                Status = "Ожидает оплаты",
                CreatedAt = DateTime.Now
            };

            _orderRepoMock.Setup(r => r.GetOrderByIdAsync(orderId))
                          .ReturnsAsync(expectedOrder);

            var result = await _service.GetOrderByIdAsync(orderId);

            Assert.NotNull(result);
            Assert.Equal(orderId, result?.OrderId);
            _orderRepoMock.Verify(r => r.GetOrderByIdAsync(orderId), Times.Once);
        }

        [Fact]
        public async Task CreateOrder_CallRepoWithCorrectParameters()
        {
            var userId = "user-123";
            var createDto = new CreateOrderDto { ExhibitionId = 1 };
            var expectedOrder = new OrderDto { OrderId = 55, Status = "Новый" };

            _orderRepoMock.Setup(r => r.CreateOrderAsync(userId, createDto))
                          .ReturnsAsync(expectedOrder);

            var result = await _service.CreateOrderAsync(userId, createDto);

            Assert.Equal(55, result.OrderId);
            _orderRepoMock.Verify(r => r.CreateOrderAsync(userId, createDto), Times.Once);
        }
    }
}
