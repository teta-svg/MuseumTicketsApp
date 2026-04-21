using Museum.Application.Interfaces;
using Museum.Application.Interfaces.Repositories;
using Museum.Domain;

public class FakePaymentService : IPaymentService
{
    private readonly IOrderRepository _orderRepo;
    private readonly IPaymentRepository _paymentRepo;
    private readonly Random _rnd = new Random();

    public FakePaymentService(IOrderRepository orderRepo, IPaymentRepository paymentRepo)
    {
        _orderRepo = orderRepo;
        _paymentRepo = paymentRepo;
    }

    public async Task<bool> PayOrderAsync(int orderId, decimal amount)
    {
        var existingOrderDto = await _orderRepo.GetOrderByIdAsync(orderId);
        if (existingOrderDto == null)
            throw new InvalidOperationException($"Заказ {orderId} не найден");

        if (existingOrderDto.Status == "Оплачен")
            throw new InvalidOperationException("Заказ уже оплачен");

        var totalPrice = existingOrderDto.Tickets.Sum(t => t.Price * t.Quantity);
        if (amount != totalPrice)
            throw new InvalidOperationException("Сумма оплаты не совпадает");

        await Task.Delay(_rnd.Next(1000, 2000));
        bool success = _rnd.NextDouble() < 0.8;

        if (success)
        {

            await _orderRepo.ConfirmPaymentAndDeductTicketsAsync(orderId);
        }
        else
        {
            await _orderRepo.UpdateOrderStatusAsync(orderId, "Отменён");
        }

        var payment = new Payment
        {
            OrderId = orderId,
            Amount = amount,
            PaymentDate = DateTime.UtcNow,
            Success = success
        };
        await _paymentRepo.AddAsync(payment);

        return success;
    }

}