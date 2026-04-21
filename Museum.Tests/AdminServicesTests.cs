using Moq;
using Museum.Domain.Entities;
using Museum.Domain.Interfaces.Repositories;
using Museum.Domain.Interfaces.Services;
using Museum.Domain.Services;

namespace Museum.Tests;

public class AdminServicesTests
{
    [Fact]
    public async Task GetExhibitionSales_ReturnCorrectCount_WhenOrderIsPaid()
    {
        var mockExhibitionRepo = new Mock<IExhibitionRepository>();

        var exhibitions = new List<Exhibition>
        {
            new Exhibition
            {
                ExhibitionId = 1,
                Name = "Test Art",
                Tickets = new List<Ticket>
                {
                    new Ticket
                    {
                        OrderItems = new List<OrderItem>
                        {
                            new OrderItem { Quantity = 5, Order = new Order { Status = "Оплачен" } }
                        }
                    }
                }
            }
        };

        mockExhibitionRepo.Setup(repo => repo.GetAllAsync()).ReturnsAsync(exhibitions);

        var service = new ReportService(null, null, null, mockExhibitionRepo.Object);

        var result = await service.GetExhibitionSalesAsync();

        var sales = Assert.Single(result);
        Assert.Equal(5, sales.TicketsSold);
    }

    [Fact]
    public async Task CreateUser_ThrowException_WhenUserAlreadyExists()
    {
        var mockUserRepo = new Mock<IUserRepository>();
        var mockHasher = new Mock<IPasswordHasher>();

        mockUserRepo.Setup(repo => repo.GetByEmailAsync("test@mail.com"))
                    .ReturnsAsync(new User());

        var service = new UserAdminService(mockUserRepo.Object, mockHasher.Object);

        var exception = await Assert.ThrowsAsync<Exception>(() =>
            service.CreateUserAsync("test@mail.com", "123", "Ivan", "Ivanov", null, null, "User"));

        Assert.Equal("Пользователь уже существует", exception.Message);
    }

    [Fact]
    public async Task DeleteExhibition_SetIsDeletedToTrue()
    {
        var mockExhibitionRepo = new Mock<IExhibitionRepository>();
        var mockMuseumRepo = new Mock<IMuseumRepository>();

        var testExhibition = new Exhibition { ExhibitionId = 1, Name = "Картины", IsDeleted = false };

        mockExhibitionRepo.Setup(repo => repo.GetByIdAsync(1))
                          .ReturnsAsync(testExhibition);

        var service = new ExhibitionAdminService(mockExhibitionRepo.Object, mockMuseumRepo.Object);

        await service.DeleteExhibitionAsync(1);

        Assert.True(testExhibition.IsDeleted);
        mockExhibitionRepo.Verify(repo => repo.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task CloseTicketSales_ChangeStatusToCancelled()
    {
        var mockTicketRepo = new Mock<ITicketRepository>();

        var tickets = new List<Ticket>
            {
                new Ticket { TicketId = 1, Status = "Доступен" },
                new Ticket { TicketId = 2, Status = "Доступен" }
            };

        mockTicketRepo.Setup(repo => repo.GetTicketsByExhibitionAsync(100))
                      .ReturnsAsync(tickets);

        var service = new TicketAdminService(mockTicketRepo.Object);

        await service.CloseTicketSalesAsync(100);

        Assert.All(tickets, t => Assert.Equal("Отменён", t.Status));
        mockTicketRepo.Verify(repo => repo.SaveChangesAsync(), Times.Once);
    }
}
