using Museum.Domain.DTOs;

namespace Museum.Domain.Interfaces.Services;

public interface IExhibitionAdminService
{
    Task<int> CreateExhibitionAsync(CreateExhibitionAdminDto dto);
    Task DeleteExhibitionAsync(int id);
}

public interface IUserAdminService
{
    Task CreateUserAsync(string email, string password, string firstName, string lastName, string? middleName, string? phone, string role);
    Task DeleteUserAsync(string email);
}

public interface ITicketAdminService
{
    Task CloseTicketSalesAsync(int exhibitionId);
}
public interface IReportService
{
    Task<(byte[] FileContent, string FileName)> GetStatisticsAsync();
    Task<(byte[] FileContent, string FileName)> ExportUsersAsync();
    Task<(byte[] FileContent, string FileName)> ExportExhibitionsAsync();
    Task<IEnumerable<ExhibitionSalesDto>> GetExhibitionSalesAsync();
}
