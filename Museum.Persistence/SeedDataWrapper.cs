using System.Collections.Generic;
using Museum.Domain;

public class SeedDataWrapper
{
    public List<MuseumComplex> MuseumComplexes { get; set; } = new();
    public List<Museum.Domain.Museum> Museums { get; set; } = new();
    public List<Exhibition> Exhibitions { get; set; } = new();
    public List<Ticket> Tickets { get; set; } = new();
    public List<TicketPrice> TicketPrices { get; set; } = new();
}
