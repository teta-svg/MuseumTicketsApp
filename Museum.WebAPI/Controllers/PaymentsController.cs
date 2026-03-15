using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Museum.Application.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

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

        // Тестовый метод оплаты заказа
        //curl -k -X POST https://localhost:7258/api/payments/test-pay/{orderId} -H "Content-Type: application/json" -d "amount" проверка
        [AllowAnonymous]
        [HttpPost("test-pay/{orderId}")]
        public async Task<IActionResult> PayOrderTest(int orderId, [FromBody] decimal amount)
        {
            string testJwt = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjEwMDAyIiwic3ViIjoic3RyaW5nIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoi0J_QvtGB0LXRgtC40YLQtdC70YwiLCJqdGkiOiIyOWRmY2M4Yi05MThmLTQxZmQtOGNhNC1hYzJjNzc1ZGY5NjUiLCJleHAiOjE3NzM2MDI3MTUsImlzcyI6Ik11c2V1bUFwaSIsImF1ZCI6Ik11c2V1bUNsaWVudCJ9.zBoKy4kcSDhpie9M_2l2YYt6Xzqwtc1nnqbVAhaaPw8";

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(testJwt);

            var claim = jwtToken.Claims.FirstOrDefault(c =>
                c.Type == ClaimTypes.NameIdentifier || c.Type == "sub");

            if (claim == null)
                return Unauthorized(new { message = "Невозможно извлечь userId из токена" });

            string userId = claim.Value;

            try
            {
                bool success = await _paymentService.PayOrderAsync(orderId, amount);

                return Ok(new
                {
                    orderId,
                    status = success ? "Оплачен" : "Отменён",
                    payment = new
                    {
                        amount,
                        status = success ? "Успешно" : "Неуспешно",
                        timestamp = System.DateTime.UtcNow
                    },
                    userId
                });
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { message = "Ошибка сервера", detail = ex.Message });
            }
        }
    }
}