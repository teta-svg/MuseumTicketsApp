using Museum.Domain.DTOs;

namespace Museum.Domain.Interfaces.Services;

public interface IExhibitionService
{
    Task<List<PublicExhibitionDTO>> GetAllAsync();
    Task<List<PublicExhibitionDTO>> GetFilteredAsync(ExhibitionFilterDto filter);
    Task<ExhibitionDetailsDTO?> GetByIdAsync(int id);
}