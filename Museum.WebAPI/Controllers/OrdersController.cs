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
    }
}