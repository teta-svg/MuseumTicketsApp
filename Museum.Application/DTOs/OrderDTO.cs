using System;
using System.Collections.Generic;
using System.Text;

namespace Museum.Application.DTOs
{
    public class OrderDTO
    {
        public int OrderID { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }
        public List<OrderItemDTO> Items { get; set; } = new();
    }
}
