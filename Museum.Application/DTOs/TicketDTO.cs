using System;
using System.Collections.Generic;
using System.Text;

namespace Museum.Application.DTOs
{
    public class TicketDTO
    {
        public int TicketID { get; set; }
        public string Type { get; set; }
        public int AvailableQuantity { get; set; }
        public decimal Price { get; set; }
    }
}
