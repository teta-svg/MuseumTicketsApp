using Moq;
using Museum.Application.Services;
using Museum.Domain.Entities;
using Museum.Domain.Interfaces.Repositories;

namespace Museum.Tests
{
    public class ExhibitionServiceTests
    {
        private readonly Mock<IExhibitionRepository> _repoMock = new();
        private readonly ExhibitionService _service;

        public ExhibitionServiceTests()
        {
            _service = new ExhibitionService(_repoMock.Object);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnCorrectDetails_WhenExhibitionExists()
        {
            var startDate = DateOnly.FromDateTime(DateTime.Now.AddDays(-5));
            var endDate = DateOnly.FromDateTime(DateTime.Now.AddDays(5));

            var exhibition = new Exhibition
            {
                ExhibitionId = 1,
                Name = "Золото скифов",
                MuseumExhibitions = new List<MuseumExhibition>
                {
                    new MuseumExhibition
                    {
                        StartDate = startDate,
                        EndDate = endDate,
                        Museum = new Museum.Domain.Entities.Museum { Name = "Эрмитаж" }
                    }
                },
                Tickets = new List<Ticket>
                {
                    new Ticket
                    {
                        TicketId = 1,
                        TicketPrices = new List<TicketPrice> { new TicketPrice { Price = 1000 } }
                    },
                    new Ticket
                    {
                        TicketId = 2,
                        TicketPrices = new List<TicketPrice> { new TicketPrice { Price = 500 } }
                    }
                }
            };

            _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(exhibition);

            var result = await _service.GetByIdAsync(1);

            Assert.NotNull(result);
            Assert.Equal("Золото скифов", result.Name);
            Assert.Equal(500, result.MinPrice);
            Assert.Equal(10, result.DurationDays); 
        }

        [Fact]
        public async Task GetByIdAsync_ReturnFinishedStatus_WhenDateIsPast()
        {
            var pastDateStart = DateOnly.FromDateTime(DateTime.Now.AddDays(-20));
            var pastDateEnd = DateOnly.FromDateTime(DateTime.Now.AddDays(-10));

            var exhibition = new Exhibition
            {
                ExhibitionId = 2,
                MuseumExhibitions = new List<MuseumExhibition>
                {
                    new MuseumExhibition { StartDate = pastDateStart, EndDate = pastDateEnd }
                }
            };

            _repoMock.Setup(r => r.GetByIdAsync(2)).ReturnsAsync(exhibition);

            var result = await _service.GetByIdAsync(2);

            Assert.Equal("Выставка завершена", result?.StatusMessage);
        }
    }
}
