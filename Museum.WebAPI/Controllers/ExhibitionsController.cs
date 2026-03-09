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
    }
}
