namespace Museum.Application.DTOs
{
    public class PublicExhibitionDTO
    {
        public int ExhibitionID { get; set; }
        public string Name { get; set; }
        public string? Photo { get; set; }
        public string MuseumName { get; set; }
        public decimal MinPrice { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
