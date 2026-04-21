using Museum.Domain.DTOs;
using Museum.Domain.Entities;
using Museum.Domain.Interfaces.Repositories;
using Museum.Domain.Interfaces.Services;

namespace Museum.Domain.Services
{
    public class ExhibitionAdminService : IExhibitionAdminService
    {
        private readonly IExhibitionRepository _exhibitionRepo;
        private readonly IMuseumRepository _museumRepo;

        public ExhibitionAdminService(IExhibitionRepository exhibitionRepo, IMuseumRepository museumRepo)
        {
            _exhibitionRepo = exhibitionRepo;
            _museumRepo = museumRepo;
        }

        public async Task<int> CreateExhibitionAsync(CreateExhibitionAdminDto dto)
        {
            var museum = await GetOrCreateMuseumAsync(dto);
            var exhibition = new Exhibition
            {
                Name = dto.Name,
                IsDeleted = false,
                MuseumExhibitions = new List<MuseumExhibition>()
            };

            exhibition.MuseumExhibitions.Add(new MuseumExhibition
            {
                MuseumId = museum.MuseumId,
                StartDate = DateOnly.FromDateTime(dto.StartDate),
                EndDate = DateOnly.FromDateTime(dto.EndDate)
            });

            foreach (var t in dto.Tickets)
            {
                exhibition.Tickets.Add(new Ticket
                {
                    Type = t.Type,
                    AvailableQuantity = t.Quantity,
                    Status = "Доступен",
                    TicketPrices = new List<TicketPrice>
                {
                    new TicketPrice { Price = t.Price, StartDate = DateOnly.FromDateTime(DateTime.Now), EndDate = DateOnly.MaxValue }
                }
                });
            }

            await _exhibitionRepo.AddAsync(exhibition);
            await _exhibitionRepo.SaveChangesAsync();
            return exhibition.ExhibitionId;
        }

        public async Task DeleteExhibitionAsync(int id)
        {
            var exhibition = await _exhibitionRepo.GetByIdAsync(id);
            if (exhibition != null)
            {
                exhibition.IsDeleted = true;
                await _exhibitionRepo.SaveChangesAsync();
            }
        }

        private async Task<Museum.Domain.Entities.Museum> GetOrCreateMuseumAsync(CreateExhibitionAdminDto dto)
        {
            var complex = await _museumRepo.GetComplexByNameAsync(dto.MuseumName);
            if (complex == null)
            {
                complex = new MuseumComplex { Name = dto.MuseumName };
                await _museumRepo.AddComplexAsync(complex);
                await _museumRepo.SaveChangesAsync();
            }

            var museum = await _museumRepo.GetByAddressAsync(dto.MuseumName, dto.City, dto.Street, dto.House);
            if (museum == null)
            {
                museum = new Museum.Domain.Entities.Museum
                {
                    Name = dto.MuseumName,
                    City = dto.City,
                    Street = dto.Street,
                    House = dto.House,
                    MuseumComplexId = complex.MuseumComplexId
                };
                await _museumRepo.AddAsync(museum);
                await _museumRepo.SaveChangesAsync();
            }
            return museum;
        }
    }

}
