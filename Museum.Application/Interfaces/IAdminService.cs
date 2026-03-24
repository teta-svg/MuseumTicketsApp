using Museum.Application.DTOs;

public interface IAdminService
{
    Task<int> CreateExhibitionAsync(CreateExhibitionAdminDto dto);
    Task UpdateExhibitionAsync(int id, CreateExhibitionAdminDto dto);
    Task DeleteExhibitionAsync(int id);

    Task AddTicketAsync(CreateTicketAdminDto dto);

    Task UpdateTicketAsync(int ticketId, UpdateTicketAdminDto dto);

    Task AddScheduleAsync(CreateScheduleAdminDto dto);

    Task CreateUserAsync(string email, string password, string firstName, string lastName, string? middleName, string? phone, string role);
    Task DeleteUserAsync(string email);

    Task<(byte[] FileContent, string FileName)> GetStatisticsAsync();
}