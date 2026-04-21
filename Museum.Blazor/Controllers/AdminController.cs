using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Museum.Domain.DTOs;
using Museum.Domain.Interfaces.Services;

namespace Museum.WebAPI.Controllers
{
    [ApiController]
    [Route("api/admin")]
    [Authorize]
    public class AdminController : ControllerBase
    {
        private readonly IExhibitionAdminService _exhibitionService;
        private readonly IUserAdminService _userService;
        private readonly ITicketAdminService _ticketService;
        private readonly IReportService _reportService;

        public AdminController(
            IExhibitionAdminService exhibitionService,
            IUserAdminService userService,
            ITicketAdminService ticketService,
            IReportService reportService)
        {
            _exhibitionService = exhibitionService;
            _userService = userService;
            _ticketService = ticketService;
            _reportService = reportService;
        }


        [HttpPost("exhibitions")]
        [Authorize(Roles = "Администратор музея,Администратор системы")]
        public async Task<IActionResult> CreateExhibition([FromBody] CreateExhibitionAdminDto dto)
        {
            var id = await _exhibitionService.CreateExhibitionAsync(dto);
            return Ok(new { ExhibitionId = id });
        }

        [HttpDelete("exhibitions/{id}")]
        [Authorize(Roles = "Администратор музея,Администратор системы")]
        public async Task<IActionResult> DeleteExhibition(int id)
        {
            await _exhibitionService.DeleteExhibitionAsync(id);
            return NoContent();
        }


        [HttpGet("exhibitions/list")]
        [Authorize(Roles = "Администратор музея,Администратор системы")]
        public async Task<IActionResult> GetExhibitionsList()
        {
            var data = await _reportService.GetExhibitionSalesAsync();
            return Ok(data);
        }

        [HttpPost("exhibitions/{id}/close-sales")]
        [Authorize(Roles = "Администратор музея,Администратор системы")]
        public async Task<IActionResult> CloseSales(int id)
        {
            await _ticketService.CloseTicketSalesAsync(id);
            return Ok(new { message = "Продажи закрыты" });
        }


        [HttpPost("users")]
        [Authorize(Roles = "Администратор системы")]
        public async Task<IActionResult> CreateUser([FromBody] RegisterRequest dto, [FromQuery] string role)
        {
            await _userService.CreateUserAsync(dto.Email, dto.Password, dto.FirstName, dto.LastName, dto.MiddleName, dto.Phone, role);
            return Ok();
        }

        [HttpDelete("users/{email}")]
        [Authorize(Roles = "Администратор системы")]
        public async Task<IActionResult> DeleteUser(string email)
        {
            await _userService.DeleteUserAsync(email);
            return NoContent();
        }


        [HttpGet("statistics")]
        [Authorize(Roles = "Администратор системы")]
        public async Task<IActionResult> GetStatistics()
        {
            var (fileContent, fileName) = await _reportService.GetStatisticsAsync();
            return File(fileContent, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }

        [HttpGet("export/users")]
        [Authorize(Roles = "Администратор системы")]
        public async Task<IActionResult> ExportUsers()
        {
            var (fileContent, fileName) = await _reportService.ExportUsersAsync();
            return File(fileContent, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }

        [HttpGet("export/exhibitions")]
        [Authorize(Roles = "Администратор системы")]
        public async Task<IActionResult> ExportExhibitions()
        {
            var (fileContent, fileName) = await _reportService.ExportExhibitionsAsync();
            return File(fileContent, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }
    }
}
