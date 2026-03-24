using Museum.Domain;
using System;
using System.Collections.Generic;
using System.Text;

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
