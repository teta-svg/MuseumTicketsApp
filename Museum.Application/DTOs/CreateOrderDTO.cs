using System;
using System.Collections.Generic;
using System.Text;

namespace Museum.Application.DTOs
{
    public class CreateOrderDTO
    {
        public List<CreateOrderItemDTO> Items { get; set; } = new();
    }
}
