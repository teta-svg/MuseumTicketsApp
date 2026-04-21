using Museum.Application.DTOs;

namespace Museum.Application.Interfaces;

public interface IAdminService
{
    Task<int> CreateExhibitionAsync(CreateExhibitionAdminDto dto);
    Task DeleteExhibitionAsync(int id);
    Task CloseTicketSalesAsync(int exhibitionId);

    Task CreateUserAsync(string email, string password, string firstName, string lastName, string? middleName, string? phone, string role);
    Task DeleteUserAsync(string email);
    Task<(byte[] FileContent, string FileName)> GetStatisticsAsync();
    Task<(byte[] FileContent, string FileName)> ExportUsersAsync();
    Task<(byte[] FileContent, string FileName)> ExportExhibitionsAsync();
    Task<IEnumerable<ExhibitionSalesDto>> GetExhibitionSalesAsync();
}