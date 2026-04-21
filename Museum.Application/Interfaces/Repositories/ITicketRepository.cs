using Museum.Domain;

namespace Museum.Application.Interfaces.Repositories
{
    public interface ITicketRepository
    {
        Task SaveChangesAsync();
        Task<List<Ticket>> GetTicketsByExhibitionAsync(int exhibitionId);
        Task<List<Ticket>> GetAllAsync();
    }
}
