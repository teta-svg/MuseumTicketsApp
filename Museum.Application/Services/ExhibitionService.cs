using Microsoft.EntityFrameworkCore;
using Museum.Application.DTOs;
using Museum.Application.Interfaces;
using Museum.Domain;

namespace Museum.Application.Services;

public class ExhibitionService : IExhibitionService
{
    private readonly IExhibitionRepository _repository;

    public ExhibitionService(IExhibitionRepository repository)
    {
        _repository = repository;
    }

    private PublicExhibitionDTO ToDto(Exhibition e) => new PublicExhibitionDTO
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
    };

    public async Task<List<PublicExhibitionDTO>> GetAllAsync()
    {
        var exhibitions = await _repository.GetAllAsync();
        return exhibitions.Select(ToDto).ToList();
    }

    public async Task<List<PublicExhibitionDTO>> GetFilteredAsync(ExhibitionFilterDto filter)
    {
        var exhibitions = await _repository.GetFilteredAsync(filter);
        return exhibitions.Select(ToDto).ToList();
    }

    public async Task<ExhibitionDetailsDTO?> GetByIdAsync(int id)
    {
        var exhibition = await _repository.GetByIdAsync(id);
        if (exhibition == null) return null;

        return new ExhibitionDetailsDTO
        {
            ExhibitionID = exhibition.ExhibitionId,
            Name = exhibition.Name,
            Photo = exhibition.Photo,
            MuseumName = exhibition.MuseumExhibitions.FirstOrDefault()?.Museum.Name ?? "",
            StartDate = exhibition.MuseumExhibitions.Any()
                ? exhibition.MuseumExhibitions.Min(me => me.StartDate).ToDateTime(TimeOnly.MinValue)
                : DateTime.MinValue,
            EndDate = exhibition.MuseumExhibitions.Any()
                ? exhibition.MuseumExhibitions.Max(me => me.EndDate).ToDateTime(TimeOnly.MinValue)
                : DateTime.MinValue,
            MinPrice = exhibition.Tickets.Where(t => t.TicketPrices.Any()).Any()
                ? exhibition.Tickets.Where(t => t.TicketPrices.Any()).Min(t => t.TicketPrices.Min(tp => tp.Price))
                : 0m,
            Tickets = exhibition.Tickets.SelectMany(t => t.TicketPrices.Select(tp => new TicketInfoDTO
            {
                Type = t.Type,
                Price = tp.Price
            })).ToList()
        };
    }
}