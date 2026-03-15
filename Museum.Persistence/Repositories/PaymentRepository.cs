using Microsoft.EntityFrameworkCore;
using Museum.Application.Interfaces;
using Museum.Domain;
using Museum.Persistence;

public class PaymentRepository : IPaymentRepository
{
    private readonly MuseumTicketsDBContext _context;

    public PaymentRepository(MuseumTicketsDBContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Payment payment)
    {
        await _context.Payments.AddAsync(payment);
        await _context.SaveChangesAsync();
    }
}