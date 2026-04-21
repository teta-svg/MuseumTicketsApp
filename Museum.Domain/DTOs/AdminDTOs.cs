namespace Museum.Domain.DTOs
{
    public class CreateExhibitionAdminDto
    {
        public string Name { get; set; }

        public string MuseumName { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string House { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public List<CreateTicketAdminDto> Tickets { get; set; } = new();
    }

    public class CreateTicketAdminDto
    {
        public string Type { get; set; }
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
