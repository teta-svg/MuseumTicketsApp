using Museum.Domain.Entities;

public interface IPaymentRepository
{
    Task AddAsync(Payment payment);
}