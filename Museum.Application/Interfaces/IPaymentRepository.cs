using Museum.Domain;

public interface IPaymentRepository
{
    Task AddAsync(Payment payment);
}