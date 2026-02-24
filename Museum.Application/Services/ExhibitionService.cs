using Microsoft.EntityFrameworkCore;
using Museum.Application.Interfaces;
using Museum.Application.DTOs;
using Museum.Persistence;

namespace Museum.Application.Services;

public class ExhibitionService : IExhibitionService
{
    private readonly MuseumTicketsDBContext _context;

    public ExhibitionService(MuseumTicketsDBContext context) => _context = context;

    public async Task<IEnumerable<ExhibitionDto>> GetAllExhibitionsAsync()
    {
        var exhibitions = await _context.Exhibitions
            .Include(e => e.MuseumExhibitions).ThenInclude(me => me.Museum)
            .Include(e => e.Tickets).ThenInclude(t => t.TicketPrices)
            .ToListAsync();

        return exhibitions.Select(e => new ExhibitionDto
        {
            Id = e.ExhibitionId,
            Name = e.Name,
            Photo = e.Photo,
            MuseumName = e.MuseumExhibitions.FirstOrDefault()?.Museum.Name ?? "Музей",
            Price = e.Tickets.FirstOrDefault()?.TicketPrices.FirstOrDefault()?.Price ?? 0
        });
    }
}

