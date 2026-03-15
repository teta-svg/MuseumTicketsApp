using System;
using System.Collections.Generic;

namespace Museum.Application.DTOs
{
    public class ExhibitionDetailsDTO
    {
        public int ExhibitionID { get; set; }
        public string Name { get; set; }
        public string? Photo { get; set; }
        public string MuseumName { get; set; }
        public decimal MinPrice { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<TicketInfoDTO> Tickets { get; set; } = new();
    }

    public class TicketInfoDTO
    {
        public string Type { get; set; }
        public decimal Price { get; set; }
    }
}