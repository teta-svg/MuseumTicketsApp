using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Museum.Application.DTOs;
using Museum.Application.Interfaces;
using System.Security.Claims;

namespace Museum.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] 
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        // POST /api/orders
        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto createOrderDto)
        {
            try
            {
                string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                    return Unauthorized(new { message = "Пользователь не авторизован" });

                var order = await _orderService.CreateOrderAsync(userId, createOrderDto);
                return Ok(order);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Внутренняя ошибка сервера", detail = ex.Message });
            }
        }

        // GET /api/orders/my
        [HttpGet("my")]
        public async Task<IActionResult> GetMyOrders()
        {
            try
            {
                string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                    return Unauthorized(new { message = "Пользователь не авторизован" });

                var orders = await _orderService.GetUserOrdersAsync(userId);
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Внутренняя ошибка сервера", detail = ex.Message });
            }
        }

        [HttpGet("{orderId}/download")]
        public async Task<IActionResult> DownloadTicket(int orderId)
        {
            var order = await _orderService.GetOrderByIdAsync(orderId);
            if (order == null || order.Status != "Оплачен") return BadRequest();

            string htmlTemplate = $@"
    <html>
    <head>
        <style>
            body {{ font-family: sans-serif; padding: 40px; text-align: center; border: 5px solid #000000; border-radius: 15px; color: #212126; }}
            .header {{ color: #212126; font-size: 24px; font-weight: bold; }}
            .details {{ margin: 20px 0; text-align: left; background: #f5f5f5; padding: 20px; border-radius: 10px; border-left: 5px solid #4b4b47; }}
            .qr-placeholder {{ margin-top: 30px; border: 2px dashed #4b4b47; width: 150px; height: 150px; line-height: 150px; display: inline-block; }}
            .footer {{ margin-top: 20px; font-size: 12px; color: #4b4b47; }}
        </style>
    </head>
    <body>
        <div class='header'>ЭЛЕКТРОННЫЙ БИЛЕT №{orderId}</div>
        <div class='details'>
            <p><strong>Статус:</strong> Оплачен</p>
            <p><strong>Дата заказа:</strong> {order.CreatedAt:dd.MM.yyyy HH:mm}</p>
            <hr>
            <p><strong>Билеты:</strong></p>
            <ul>";

            foreach (var t in order.Tickets)
            {
                htmlTemplate += $"<li>{t.TicketType} — {t.Quantity} шт.</li>";
            }

            htmlTemplate += $@"
            </ul>
        </div>
        <img src='https://dummyimage.com/200X200/000/fff&text=+qr+code+{orderId}' style='width:150px;' />
        <p><i>Предъявите этот код на входе</i></p>
        <div class='footer'>Спасибо, что посещаете наш музей!</div>
    </body>
    </html>";

            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(htmlTemplate);

            return Ok(new
            {
                fileName = $"Ticket_{orderId}.html",
                base64 = Convert.ToBase64String(bytes)
            });
        }

    }
}