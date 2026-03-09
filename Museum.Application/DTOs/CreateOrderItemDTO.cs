using System;
using System.Collections.Generic;
using System.Text;

namespace Museum.Application.DTOs
{
    public class CreateOrderItemDTO
    {
        public int TicketID { get; set; }
        public int Quantity { get; set; }
    }
}
