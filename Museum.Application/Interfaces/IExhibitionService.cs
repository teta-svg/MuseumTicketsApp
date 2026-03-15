using Museum.Application.DTOs;

namespace Museum.Application.Interfaces;

public interface IExhibitionService
{
    Task<List<PublicExhibitionDTO>> GetAllAsync();
    Task<List<PublicExhibitionDTO>> GetFilteredAsync(ExhibitionFilterDto filter);
    Task<ExhibitionDetailsDTO?> GetByIdAsync(int id);
}