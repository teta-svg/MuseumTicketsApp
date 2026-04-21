using Museum.Domain.Interfaces.Repositories;
using Museum.Domain.Interfaces.Services;

namespace Museum.Domain.Services
{
    public class TicketAdminService: ITicketAdminService
    {
        private readonly ITicketRepository _ticketRepo;

        public TicketAdminService(ITicketRepository ticketRepo)
        {
            _ticketRepo = ticketRepo;
        }

        public async Task CloseTicketSalesAsync(int exhibitionId)
        {
            var tickets = await _ticketRepo.GetTicketsByExhibitionAsync(exhibitionId);
            foreach (var ticket in tickets) ticket.Status = "Отменён";
            await _ticketRepo.SaveChangesAsync();
        }
    }

}
