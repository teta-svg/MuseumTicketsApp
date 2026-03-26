using Microsoft.AspNetCore.Mvc;
using Museum.Persistence;

[ApiController]
[Route("api/[controller]")]
public class CheckConnectionController : ControllerBase
{
    private readonly MuseumTicketsDBContext _context;

    public CheckConnectionController(MuseumTicketsDBContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        try
        {
            var canConnect = await _context.Database.CanConnectAsync();
            if (canConnect)
            {
                return Ok("База данных подключена и работает.");
            }
            return BadRequest("База не отвечает. Проверить строку подключения.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Ошибка подключения: {ex.Message}");
        }
    }
}
