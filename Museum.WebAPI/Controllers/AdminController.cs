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

        [HttpPut("exhibitions/{id}")]
        [Authorize(Roles = "Администратор музея,Администратор системы")]
        public async Task<IActionResult> UpdateExhibition(int id, [FromBody] CreateExhibitionAdminDto dto)
        {
            await _adminService.UpdateExhibitionAsync(id, dto);
            return NoContent();
        }

        [HttpDelete("exhibitions/{id}")]
        [Authorize(Roles = "Администратор музея,Администратор системы")]
        public async Task<IActionResult> DeleteExhibition(int id)
        {
            await _adminService.DeleteExhibitionAsync(id);
            return NoContent();
        }



        [HttpPost("tickets")]
        [Authorize(Roles = "Администратор музея,Администратор системы")]
        public async Task<IActionResult> AddTicket([FromBody] CreateTicketAdminDto dto)
        {
            await _adminService.AddTicketAsync(dto); 
            return Ok();
        }


        [HttpPut("tickets/{ticketId}")]
        [Authorize(Roles = "Администратор музея,Администратор системы")]
        public async Task<IActionResult> UpdateTicket(int ticketId, [FromBody] UpdateTicketAdminDto dto)
        {
            await _adminService.UpdateTicketAsync(ticketId, dto);
            return NoContent();
        }


        [HttpPost("schedules")]
        [Authorize(Roles = "Администратор музея,Администратор системы")]
        public async Task<IActionResult> AddSchedule([FromBody] CreateScheduleAdminDto dto)
        {
            await _adminService.AddScheduleAsync(dto);
            return Ok();
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

            return File(fileContent,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        fileName);
        }
    }
}