namespace Museum.Application.Interfaces
{
    public interface IAuthService
    {
        Task<string?> LoginAsync(string email, string password);

        Task RegisterAsync(RegisterRequest request);
    }
}