using Museum.Application.DTOs;

namespace Museum.Application.Interfaces;

public interface IExhibitionService
{
    Task<List<PublicExhibitionDTO>> GetAllAsync();
    Task<PublicExhibitionDTO?> GetByIdAsync(int exhibitionId);
}