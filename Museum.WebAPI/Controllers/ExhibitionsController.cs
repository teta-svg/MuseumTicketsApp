using Microsoft.AspNetCore.Mvc;
using Museum.Application.DTOs;
using Museum.Application.Interfaces;

namespace Museum.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExhibitionsController : ControllerBase
    {
        private readonly IExhibitionService _service;

        public ExhibitionsController(IExhibitionService service) => _service = service;

        [HttpGet]
        public async Task<ActionResult<List<PublicExhibitionDTO>>> GetAll() =>
            Ok(await _service.GetAllAsync());

        [HttpGet("filter")]
        public async Task<IActionResult> GetFiltered([FromQuery] ExhibitionFilterDto filter)
        {
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