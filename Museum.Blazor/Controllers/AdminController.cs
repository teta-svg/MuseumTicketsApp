using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Museum.Application.DTOs;
using Museum.Application.Interfaces;

namespace Museum.WebAPI.Controllers
{
    [ApiController]
    [Route("api/admin")]
    [Authorize]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpPost("exhibitions")]
        [Authorize(Roles = "Администратор музея,Администратор системы")]
        public async Task<IActionResult> CreateExhibition([FromBody] CreateExhibitionAdminDto dto)
        {
            var id = await _adminService.CreateExhibitionAsync(dto);
            return Ok(new { ExhibitionId = id });
        }

        [HttpDelete("exhibitions/{id}")]
        [Authorize(Roles = "Администратор музея,Администратор системы")]
        public async Task<IActionResult> DeleteExhibition(int id)
        {
            await _adminService.DeleteExhibitionAsync(id);
            return NoContent();
        }
        [HttpGet("exhibitions/list")]
        [Authorize(Roles = "Администратор музея,Администратор системы")]
        public async Task<IActionResult> GetExhibitionsList()
        {
            var data = await _adminService.GetExhibitionSalesAsync();
            return Ok(data);
        }

        [HttpPost("exhibitions/{id}/close-sales")]
        [Authorize(Roles = "Администратор музея,Администратор системы")]
        public async Task<IActionResult> CloseSales(int id)
        {
            await _adminService.CloseTicketSalesAsync(id);
            return Ok(new { message = "Продажи закрыты" });
        }

        [HttpPost("users")]
        [Authorize(Roles = "Администратор системы")]
        public async Task<IActionResult> CreateUser([FromBody] RegisterRequest dto, [FromQuery] string role)
        {
            await _adminService.CreateUserAsync(dto.Email, dto.Password, dto.FirstName, dto.LastName, dto.MiddleName, dto.Phone, role);
            return Ok();
        }

        [HttpDelete("users/{email}")]
        [Authorize(Roles = "Администратор системы")]
        public async Task<IActionResult> DeleteUser(string email)
        {
            await _adminService.DeleteUserAsync(email);
            return NoContent();
        }

        [HttpGet("statistics")]
        [Authorize(Roles = "Администратор системы")]
        public async Task<IActionResult> GetStatistics()
        {
            var (fileContent, fileName) = await _adminService.GetStatisticsAsync();
            return File(fileContent, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }

        [HttpGet("export/users")]
        [Authorize(Roles = "Администратор системы")]
        public async Task<IActionResult> ExportUsers()
        {
            var (fileContent, fileName) = await _adminService.ExportUsersAsync();
            return File(fileContent, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }

        [HttpGet("export/exhibitions")]
        [Authorize(Roles = "Администратор системы")]
        public async Task<IActionResult> ExportExhibitions()
        {
            var (fileContent, fileName) = await _adminService.ExportExhibitionsAsync();
            return File(fileContent, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }
    }
}