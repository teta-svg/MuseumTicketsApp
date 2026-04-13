namespace Museum.Application.DTOs
{
    public class CreateOrderDto
    {
        public int ExhibitionId { get; set; }
        public List<OrderTicketRequestDto> Tickets { get; set; } = new();
    }

    public class OrderTicketRequestDto
    {
        public int TicketId { get; set; } 
        public int Quantity { get; set; }
    }

    public class OrderTicketDto
    {
        public int TicketId { get; set; }
        public string TicketType { get; set; } = string.Empty; 
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }

    public class OrderDto
    {
        public int OrderId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Status { get; set; } = string.Empty; 
        public List<OrderTicketDto> Tickets { get; set; } = new List<OrderTicketDto>();
    }
}