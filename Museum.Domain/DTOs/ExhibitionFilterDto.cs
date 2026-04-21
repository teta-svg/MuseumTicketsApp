namespace Museum.Domain.DTOs
{
    public class ExhibitionFilterDto
    {
        public string? Name { get; set; }
        public string? MuseumName { get; set; }
        public string? MuseumComplexName { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public DateOnly? StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
    }
}