namespace Museum.Application.DTOs
{
    public class ExhibitionFilterDto
    {
        public string? Name { get; set; }               // Название выставки
        public string? MuseumName { get; set; }         // Название музея
        public string? MuseumComplexName { get; set; }  // Название музейного комплекса
        public decimal? MinPrice { get; set; }          // Минимальная цена билета
        public decimal? MaxPrice { get; set; }          // Максимальная цена билета
        public DateTime? StartDate { get; set; }        // Начало выставки
        public DateTime? EndDate { get; set; }          // Конец выставки
    }
}