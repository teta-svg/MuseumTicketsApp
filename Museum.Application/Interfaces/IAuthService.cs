using System.Threading.Tasks;

namespace Museum.Application.Interfaces;

public interface IAuthService
{
    Task<string> Login(string username, string password);
}
