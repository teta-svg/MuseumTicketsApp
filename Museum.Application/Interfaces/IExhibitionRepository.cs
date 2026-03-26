using Museum.Application.DTOs;
using Museum.Domain;

namespace Museum.Application.Interfaces
{
    public interface IExhibitionRepository
    {
        Task<IEnumerable<Exhibition>> GetAllAsync();
        Task<IEnumerable<Exhibition>> GetFilteredAsync(ExhibitionFilterDto filter);
        Task<Exhibition?> GetByIdAsync(int id);

        Task AddAsync(Exhibition exhibition);
        Task UpdateAsync(Exhibition exhibition);
        Task DeleteAsync(Exhibition exhibition);
        Task SaveChangesAsync();

    }
}
