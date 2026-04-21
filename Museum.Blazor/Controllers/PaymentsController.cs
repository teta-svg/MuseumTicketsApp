using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Museum.Domain.Interfaces.Services;
using System.Security.Claims;

namespace Museum.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentsController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        // POST /api/payments/{orderId}
        [HttpPost("{orderId}")]
        public async Task<IActionResult> PayOrder(int orderId, [FromBody] decimal amount)
        {
            try
            {
                string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                    return Unauthorized(new { message = "Пользователь не авторизован" });

                bool success = await _paymentService.PayOrderAsync(orderId, amount);

                return Ok(new
                {
                    orderId,
                    status = success ? "Оплачен" : "В ожидании",
                    payment = new
                    {
                        amount,
                        status = success ? "Успешно" : "Отменён",
                        timestamp = System.DateTime.UtcNow
                    }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ошибка сервера", detail = ex.Message });
            }
        }
    }
}