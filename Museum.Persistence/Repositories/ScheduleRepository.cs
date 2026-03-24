using Museum.Application.Interfaces;
using Museum.Domain;
using Museum.Persistence;
using Microsoft.EntityFrameworkCore;


public class ScheduleRepository : IScheduleRepository
{
    private readonly MuseumTicketsDBContext _context;

    public ScheduleRepository(MuseumTicketsDBContext context)
    {
        _context = context;
    }

    public async Task AddAsync(MuseumSchedule schedule)
    {
        await _context.MuseumSchedules.AddAsync(schedule);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task<List<MuseumSchedule>> GetByMuseumIdAsync(int museumId)
    {
        return await _context.MuseumSchedules
            .Where(s => s.MuseumId == museumId)
            .ToListAsync();
    }

    public async Task DeleteAsync(MuseumSchedule schedule)
    {
        _context.MuseumSchedules.Remove(schedule);
    }
}