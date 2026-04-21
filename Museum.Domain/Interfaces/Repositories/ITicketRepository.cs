using Museum.Domain.Entities;

namespace Museum.Domain.Interfaces.Repositories
{
    public interface ITicketRepository
    {
        Task SaveChangesAsync();
        Task<List<Ticket>> GetTicketsByExhibitionAsync(int exhibitionId);
        Task<List<Ticket>> GetAllAsync();
    }
}
