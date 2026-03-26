using Museum.Domain;

namespace Museum.Application.Interfaces
{
    public interface IScheduleRepository
    {
        Task AddAsync(MuseumSchedule schedule);
        Task SaveChangesAsync();
        Task<List<MuseumSchedule>> GetByMuseumIdAsync(int museumId);
        Task DeleteAsync(MuseumSchedule schedule);
    }
}
