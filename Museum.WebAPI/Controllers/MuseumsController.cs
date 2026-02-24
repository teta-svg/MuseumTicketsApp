using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Museum.Application.Interfaces;

[ApiController]
[Route("api/[controller]")]
public class ExhibitionsController : ControllerBase
{
    private readonly IExhibitionService _exhibitionService;

    public ExhibitionsController(IExhibitionService exhibitionService)
    {
        _exhibitionService = exhibitionService;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var data = await _exhibitionService.GetAllExhibitionsAsync();
        return Ok(data);
    }
}

