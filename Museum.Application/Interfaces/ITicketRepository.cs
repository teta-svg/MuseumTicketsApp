using Museum.Domain;

namespace Museum.Application.Interfaces
{
    public interface ITicketRepository
    {
        Task AddAsync(Ticket ticket);
        Task<Ticket?> GetByIdAsync(int id);
        Task SaveChangesAsync();
        Task<List<Ticket>> GetTicketsByExhibitionAsync(int exhibitionId);
        Task<List<Ticket>> GetAllAsync();
    }
}
