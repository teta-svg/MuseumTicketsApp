namespace Museum.Application.DTOs;

public class ExhibitionDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Photo { get; set; }
    public string MuseumName { get; set; } = null!;
    public decimal Price { get; set; }
}
