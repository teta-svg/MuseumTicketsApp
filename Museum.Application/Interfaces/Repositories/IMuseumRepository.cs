public interface IMuseumRepository
{
    Task AddAsync(Museum.Domain.Museum museum);
    Task<Museum.Domain.MuseumComplex?> GetComplexByNameAsync(string name);
    Task AddComplexAsync(Museum.Domain.MuseumComplex complex);

    Task<Museum.Domain.Museum?> GetByAddressAsync(string name, string city, string street, string house);
    Task SaveChangesAsync();
}