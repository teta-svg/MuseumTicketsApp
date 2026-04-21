using Museum.Domain.DTOs;
using Museum.Domain.Entities;

namespace Museum.Domain.Interfaces.Repositories
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
