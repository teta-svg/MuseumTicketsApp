using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Museum.Application.DTOs;
using Museum.Application.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Museum.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize] 
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

        // Тестовый метод для POST /api/orders, использует хардкод токена
        // токен меняется каждые 2 часа
        [HttpPost("test-create")]
        public async Task<IActionResult> CreateOrderTest([FromBody] CreateOrderDto createOrderDto)
        {
            string testJwt = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjEwMDAyIiwic3ViIjoic3RyaW5nIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoi0J_QvtGB0LXRgtC40YLQtdC70YwiLCJqdGkiOiIxMTQ4NmE5Zi1lYzljLTQwNDAtYmU1Mi1hYTgxNjY5ZjEzOWEiLCJleHAiOjE3NzM1OTQ1NTAsImlzcyI6Ik11c2V1bUFwaSIsImF1ZCI6Ik11c2V1bUNsaWVudCJ9.uQg1I-S8yHGyYuSzsPdhiiCSJQB0z6rH7j88MdunLSA";

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(testJwt);

            var claim = jwtToken.Claims.FirstOrDefault(c =>
                c.Type == ClaimTypes.NameIdentifier || c.Type == "sub");

            if (claim == null)
                return Unauthorized(new { message = "Невозможно извлечь userId из токена" });

            string userId = claim.Value;

            try
            {
                var order = await _orderService.CreateOrderAsync(userId, createOrderDto);
                return Ok(order);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ошибка сервера", detail = ex.Message });
            }
        }

        // Тестовый метод для GET /api/orders/my
        [HttpGet("test-my-orders")]
        public async Task<IActionResult> GetMyOrdersTest()
        {
            string testJwt = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjEwMDAyIiwic3ViIjoic3RyaW5nIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoi0J_QvtGB0LXRgtC40YLQtdC70YwiLCJqdGkiOiIxMTQ4NmE5Zi1lYzljLTQwNDAtYmU1Mi1hYTgxNjY5ZjEzOWEiLCJleHAiOjE3NzM1OTQ1NTAsImlzcyI6Ik11c2V1bUFwaSIsImF1ZCI6Ik11c2V1bUNsaWVudCJ9.uQg1I-S8yHGyYuSzsPdhiiCSJQB0z6rH7j88MdunLSA";

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(testJwt);

            var claim = jwtToken.Claims.FirstOrDefault(c =>
                c.Type == ClaimTypes.NameIdentifier || c.Type == "sub");

            if (claim == null)
                return Unauthorized(new { message = "Невозможно извлечь userId из токена" });

            string userId = claim.Value;

            try
            {
                var orders = await _orderService.GetUserOrdersAsync(userId);
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ошибка сервера", detail = ex.Message });
            }
        }
    }
}