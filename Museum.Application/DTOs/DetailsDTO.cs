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
        public string MuseumComplex { get; set; }
        public string Address { get; set; }

        public decimal MinPrice { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int DurationDays { get; set; }

        public string StatusMessage { get; set; }

        public List<ScheduleDto> Schedule { get; set; } = new();
        public List<TicketInfoDTO> Tickets { get; set; } = new();
    }

    public class ScheduleDto
    {
        public int WeekDay { get; set; }
        public DateOnly StartDate { get; set; }  
        public DateOnly EndDate { get; set; }   
        public TimeOnly Open { get; set; }
        public TimeOnly Close { get; set; }
    }

    public class TicketInfoDTO
    {
        public string Type { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}

