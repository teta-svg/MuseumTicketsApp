using System;
using System.Collections.Generic;
using System.Text;

namespace Museum.Application.Interfaces
{
    public interface IPaymentService
    {
        Task<bool> PayOrderAsync(int orderId, decimal amount);
    }
}
