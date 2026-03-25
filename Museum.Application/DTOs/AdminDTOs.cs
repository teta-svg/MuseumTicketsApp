using System;
using System.Collections.Generic;
using System.Text;

namespace Museum.Application.DTOs
{
    public class CreateExhibitionAdminDto
    {
        public string Name { get; set; }
        public string? Photo { get; set; }

        public string MuseumName { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string House { get; set; }
        public int MuseumComplexId { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public List<CreateTicketAdminDto> Tickets { get; set; } = new();
        public List<CreateScheduleAdminDto> Schedules { get; set; } = new();
    }

    public class CreateTicketAdminDto
    {
        public string Type { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public int ExhibitionId { get; set; }
    }

    public class CreateScheduleAdminDto
    {
        public int MuseumId { get; set; } 
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public TimeOnly Open { get; set; }
        public TimeOnly Close { get; set; }
    }


    public class UpdateTicketAdminDto
    {
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }

    public class ExhibitionSalesDto
    {
        public int ExhibitionId { get; set; }
        public string ExhibitionName { get; set; }
        public int TicketsSold { get; set; }
    }


}
