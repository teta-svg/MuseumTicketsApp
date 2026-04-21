using Moq;
using Museum.Domain.Entities;
using Museum.Domain.Interfaces.Repositories;
using Museum.Domain.DTOs;

namespace Museum.Tests
{
    public class FakePaymentServiceTests
    {
        private readonly Mock<IOrderRepository> _orderRepoMock = new();
        private readonly Mock<IPaymentRepository> _paymentRepoMock = new();
        private readonly FakePaymentService _service;

        public FakePaymentServiceTests()
        {
            _service = new FakePaymentService(_orderRepoMock.Object, _paymentRepoMock.Object);
        }

        [Fact]
        public async Task PayOrder_ThrowException_WhenAmountIsIncorrect()
        {
            var orderId = 123;
            var orderDto = new OrderDto
            {
                OrderId = orderId,
                Status = "Ожидает оплаты",
                Tickets = new List<OrderTicketDto>
                {
                    new OrderTicketDto { Price = 100, Quantity = 2 }
                }
            };

            _orderRepoMock.Setup(r => r.GetOrderByIdAsync(orderId)).ReturnsAsync(orderDto);

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _service.PayOrderAsync(orderId, 150));

            Assert.Equal("Сумма оплаты не совпадает", exception.Message);
        }

        [Fact]
        public async Task PayOrder_ProcessPayment_RegardlessOfRandomOutcome()
        {
            var orderId = 1;
            var amount = 500m;
            var orderDto = new OrderDto
            {
                OrderId = orderId,
                Status = "Ожидает оплаты",
                Tickets = new List<OrderTicketDto> { new OrderTicketDto { Price = 500, Quantity = 1 } }
            };

            _orderRepoMock.Setup(r => r.GetOrderByIdAsync(orderId)).ReturnsAsync(orderDto);

            bool result = await _service.PayOrderAsync(orderId, amount);

            if (result)
            {
                _orderRepoMock.Verify(r => r.ConfirmPaymentAndDeductTicketsAsync(orderId), Times.Once);
            }
            else
            {
                _orderRepoMock.Verify(r => r.UpdateOrderStatusAsync(orderId, "Отменён"), Times.Once);
            }

            _paymentRepoMock.Verify(r => r.AddAsync(It.IsAny<Payment>()), Times.Once);
        }
    }
}
