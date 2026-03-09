using System;
using System.Collections.Generic;
using System.Text;

namespace Museum.Application.DTOs
{
    public class OrderItemDTO
    {
        public string ExhibitionName { get; set; }
        public string TicketType { get; set; }
        public int Quantity { get; set; }
        public decimal PriceAtPurchase { get; set; }
    }
}
