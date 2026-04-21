using Microsoft.AspNetCore.Mvc;
using Museum.Domain.DTOs;
using Museum.Domain.Interfaces.Services;

namespace Museum.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExhibitionsController : ControllerBase
    {
        private readonly IExhibitionService _service;
        private readonly ILogger<ExhibitionsController> _logger;

        public ExhibitionsController(IExhibitionService service, ILogger<ExhibitionsController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<List<PublicExhibitionDTO>>> GetAll() =>
            Ok(await _service.GetAllAsync());

        [HttpGet("filter")]
        public async Task<IActionResult> GetFiltered([FromQuery] ExhibitionFilterDto filter)
        {
            _logger.LogInformation("Received filter: Name={Name}, MuseumName={MuseumName}, MinPrice={MinPrice}, MaxPrice={MaxPrice}, StartDate={StartDate}, EndDate={EndDate}",
                filter.Name, filter.MuseumName, filter.MinPrice, filter.MaxPrice,
                filter.StartDate?.ToString(), filter.EndDate?.ToString());

            var exhibitions = await _service.GetFilteredAsync(filter);
            return Ok(exhibitions);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var exhibition = await _service.GetByIdAsync(id);
            if (exhibition == null) return NotFound(new { message = "Exhibition not found" });

            return Ok(exhibition);
        }
    }
}