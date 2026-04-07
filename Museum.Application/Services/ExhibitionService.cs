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

        var museumEx = exhibition.MuseumExhibitions.FirstOrDefault();
        var museum = museumEx?.Museum;

        var startDate = exhibition.MuseumExhibitions.Any()
            ? exhibition.MuseumExhibitions.Min(me => me.StartDate).ToDateTime(TimeOnly.MinValue)
            : DateTime.MinValue;

        var endDate = exhibition.MuseumExhibitions.Any()
            ? exhibition.MuseumExhibitions.Max(me => me.EndDate).ToDateTime(TimeOnly.MinValue)
            : DateTime.MinValue;

        var now = DateTime.UtcNow;

        string statusMessage;

        if (startDate == DateTime.MinValue || endDate == DateTime.MinValue)
            statusMessage = "Даты проведения не указаны";
        else if (now < startDate)
            statusMessage = "Скоро откроется";
        else if (now > endDate)
            statusMessage = "Выставка завершена";
        else
            statusMessage = "Идёт сейчас";

        var schedule = museum?.MuseumSchedules
            .SelectMany(ms => ms.ScheduleDays.Select(sd => new ScheduleDto
            {
                WeekDay = sd.WeekDay,
                Open = ms.OpenTime,
                Close = ms.CloseTime
            }))
            .ToList() ?? new List<ScheduleDto>();

        var tickets = exhibition.Tickets
            .SelectMany(t => t.TicketPrices.Select(tp => new TicketInfoDTO
            {
                TicketID = t.TicketId,
                Type = t.Type,
                Price = tp.Price,
                Quantity = t.AvailableQuantity,
                Status = t.Status
            }))
            .ToList();



        var minPrice = tickets.Any() ? tickets.Min(t => t.Price) : 0;

        return new ExhibitionDetailsDTO
        {
            ExhibitionID = exhibition.ExhibitionId,
            Name = exhibition.Name,
            Photo = exhibition.Photo,

            MuseumName = museum?.Name ?? "",
            MuseumComplex = museum?.MuseumComplex?.Name ?? "",
            Address = museum != null
                ? $"{museum.City}, {museum.Street} {museum.House}"
                : "",

            StartDate = startDate,
            EndDate = endDate,
            DurationDays = (endDate - startDate).Days,

            StatusMessage = statusMessage,

            Schedule = schedule,
            Tickets = tickets,
            MinPrice = minPrice
        };
    }
}