using Museum.Application.Interfaces;
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
        var orders = await _orderRepo.GetOrdersByUserIdAsync(orderId.ToString());
        var existingOrder = await _orderRepo.GetOrderByIdAsync(orderId);
        if (existingOrder == null)
            throw new InvalidOperationException($"Заказ {orderId} не найден");

        if (existingOrder.Status == "Оплачен")
            throw new InvalidOperationException("Заказ уже оплачен");

        var totalPrice = existingOrder.Tickets.Sum(t => t.Price * t.Quantity);
        if (amount != totalPrice)
            throw new InvalidOperationException($"Сумма оплаты {amount} не совпадает с итоговой стоимостью заказа {totalPrice}");

        await Task.Delay(_rnd.Next(1000, 2000));
        bool success = _rnd.NextDouble() < 0.8;

        var payment = new Payment
        {
            OrderId = orderId,
            Amount = amount,
            PaymentDate = DateTime.UtcNow,
            Success = success
        };
        await _paymentRepo.AddAsync(payment);

        await _orderRepo.UpdateOrderStatusAsync(orderId, success ? "Оплачен" : "Отменён");

        return success;
    }
}