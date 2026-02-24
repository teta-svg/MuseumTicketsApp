using Museum.Application.DTOs;

namespace Museum.Application.Interfaces;

public interface IExhibitionService
{
    Task<IEnumerable<ExhibitionDto>> GetAllExhibitionsAsync();
}
