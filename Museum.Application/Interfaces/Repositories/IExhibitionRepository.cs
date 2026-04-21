using Museum.Application.DTOs;
using Museum.Domain;

namespace Museum.Application.Interfaces.Repositories
{
    public interface IExhibitionRepository
    {
        Task<IEnumerable<Exhibition>> GetAllAsync();
        Task<IEnumerable<Exhibition>> GetFilteredAsync(ExhibitionFilterDto filter);
        Task<Exhibition?> GetByIdAsync(int id);

        Task AddAsync(Exhibition exhibition);
        Task SaveChangesAsync();

    }
}
