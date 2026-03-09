using Microsoft.EntityFrameworkCore;
using Museum.Application.Interfaces;
using Museum.Application.DTOs;

namespace Museum.Application.Services;

public class ExhibitionService : IExhibitionService
{
    private readonly IExhibitionRepository _repository;

    public ExhibitionService(IExhibitionRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<PublicExhibitionDTO>> GetAllAsync()
    {
        var exhibitions = await _repository.GetAllAsync();

        return exhibitions.Select(e => new PublicExhibitionDTO
        {
            ExhibitionID = e.ExhibitionId,
            Name = e.Name,
            Photo = e.Photo,
            MuseumName = e.MuseumExhibitions.FirstOrDefault()?.Museum.Name ?? "",
            StartDate = e.MuseumExhibitions?.Any() == true
                ? e.MuseumExhibitions.Min(me => me.StartDate).ToDateTime(TimeOnly.MinValue)
                : DateTime.MinValue,
            EndDate = e.MuseumExhibitions?.Any() == true
                ? e.MuseumExhibitions.Max(me => me.EndDate).ToDateTime(TimeOnly.MinValue)
                : DateTime.MinValue,
            MinPrice = e.Tickets?.Where(t => t.TicketPrices.Any()).Any() == true
                ? e.Tickets.Where(t => t.TicketPrices.Any()).Min(t => t.TicketPrices.Min(tp => tp.Price))
                : 0m
        }).ToList();
    }

    public async Task<PublicExhibitionDTO?> GetByIdAsync(int exhibitionId)
    {
        var e = await _repository.GetByIdAsync(exhibitionId);
        if (e == null) return null;

        return new PublicExhibitionDTO
        {
            ExhibitionID = e.ExhibitionId,
            Name = e.Name,
            Photo = e.Photo,
            MuseumName = e.MuseumExhibitions.First().Museum.Name,
            StartDate = e.MuseumExhibitions?.Any() == true
                ? e.MuseumExhibitions.Min(me => me.StartDate).ToDateTime(TimeOnly.MinValue)
                : DateTime.MinValue,
            EndDate = e.MuseumExhibitions?.Any() == true
                ? e.MuseumExhibitions.Max(me => me.EndDate).ToDateTime(TimeOnly.MinValue)
                : DateTime.MinValue,
            MinPrice = e.Tickets?.Where(t => t.TicketPrices.Any()).Any() == true
                ? e.Tickets.Where(t => t.TicketPrices.Any()).Min(t => t.TicketPrices.Min(tp => tp.Price))
                : 0m
        };
    }
}