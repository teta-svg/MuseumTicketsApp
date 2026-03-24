using Museum.Domain;

public interface IMuseumRepository
{
    Task AddAsync(Museum.Domain.Museum museum);
    Task<Museum.Domain.Museum?> GetByNameAsync(string name);

    Task<Museum.Domain.Museum?> GetByAddressAsync(string name, string city, string street, string house);
    Task SaveChangesAsync();
}