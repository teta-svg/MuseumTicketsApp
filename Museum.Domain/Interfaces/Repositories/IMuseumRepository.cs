using Museum.Domain.Entities;

public interface IMuseumRepository
{
    Task AddAsync(Museum.Domain.Entities.Museum museum);
    Task<MuseumComplex?> GetComplexByNameAsync(string name);
    Task AddComplexAsync(MuseumComplex complex);

    Task<Museum.Domain.Entities.Museum?> GetByAddressAsync(string name, string city, string street, string house);
    Task SaveChangesAsync();
}