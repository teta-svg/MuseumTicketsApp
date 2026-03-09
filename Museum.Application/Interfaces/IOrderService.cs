using Museum.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Museum.Application.Interfaces
{
    public interface IOrderService
    {
        Task<OrderDTO> CreateOrderAsync(int userId, CreateOrderDTO dto);
        Task<List<OrderDTO>> GetUserOrdersAsync(int userId);
    }
}
