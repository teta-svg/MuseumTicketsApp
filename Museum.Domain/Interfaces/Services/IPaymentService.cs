namespace Museum.Domain.Interfaces.Services
{
    public interface IPaymentService
    {
        Task<bool> PayOrderAsync(int orderId, decimal amount);
    }
}
