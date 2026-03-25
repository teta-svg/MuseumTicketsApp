using Museum.Application.DTOs;
using Museum.Application.Interfaces;
using Museum.Domain;

public class AdminService : IAdminService
{
    private readonly IExhibitionRepository _exhibitionRepo;
    private readonly ITicketRepository _ticketRepo;
    private readonly IScheduleRepository _scheduleRepo;
    private readonly IUserRepository _userRepo;
    private readonly IOrderRepository _orderRepo;
    private readonly IMuseumRepository _museumRepo;

    public AdminService(
        IExhibitionRepository exhibitionRepo,
        ITicketRepository ticketRepo,
        IScheduleRepository scheduleRepo,
        IUserRepository userRepo,
        IOrderRepository orderRepo,
        IMuseumRepository museumRepo)
    {
        _exhibitionRepo = exhibitionRepo;
        _ticketRepo = ticketRepo;
        _scheduleRepo = scheduleRepo;
        _userRepo = userRepo;
        _orderRepo = orderRepo;
        _museumRepo = museumRepo;
    }

    private async Task<Museum.Domain.Museum> GetOrCreateMuseumAsync(CreateExhibitionAdminDto dto)
    {
        var museum = await _museumRepo.GetByAddressAsync(dto.MuseumName, dto.City, dto.Street, dto.House);

        if (museum != null)
            return museum;

        museum = new Museum.Domain.Museum
        {
            Name = dto.MuseumName,
            City = dto.City,
            Street = dto.Street,
            House = dto.House,
            MuseumComplexId = dto.MuseumComplexId
        };

        await _museumRepo.AddAsync(museum);
        await _museumRepo.SaveChangesAsync();

        return museum;
    }

    public async Task<int> CreateExhibitionAsync(CreateExhibitionAdminDto dto)
    {
        var museum = await GetOrCreateMuseumAsync(dto);

        var exhibition = new Exhibition
        {
            Name = dto.Name,
            Photo = dto.Photo,
            MuseumExhibitions =
        {
            new MuseumExhibition
            {
                MuseumId = museum.MuseumId,
                StartDate = DateOnly.FromDateTime(dto.StartDate),
                EndDate = DateOnly.FromDateTime(dto.EndDate)
            }
        }
        };

        await _exhibitionRepo.AddAsync(exhibition);
        await _exhibitionRepo.SaveChangesAsync();

        foreach (var t in dto.Tickets)
        {
            var ticket = new Ticket
            {
                ExhibitionId = exhibition.ExhibitionId,
                Type = t.Type,
                AvailableQuantity = t.Quantity,
                Status = "Доступен",
                TicketPrices =
            {
                new TicketPrice
                {
                    Price = t.Price,
                    StartDate = DateOnly.FromDateTime(DateTime.Now),
                    EndDate = DateOnly.MaxValue
                }
            }
            };

            await _ticketRepo.AddAsync(ticket);
        }

        foreach (var s in dto.Schedules)
        {
            await _scheduleRepo.AddAsync(new MuseumSchedule
            {
                MuseumId = museum.MuseumId,
                StartDate = s.StartDate,
                EndDate = s.EndDate,
                OpenTime = s.Open,
                CloseTime = s.Close
            });
        }

        await _ticketRepo.SaveChangesAsync();
        await _scheduleRepo.SaveChangesAsync();

        return exhibition.ExhibitionId;
    }

    public async Task UpdateExhibitionAsync(int id, CreateExhibitionAdminDto dto)
    {
        var exhibition = await _exhibitionRepo.GetByIdAsync(id)
            ?? throw new Exception("Выставка не найдена");

        exhibition.Name = dto.Name;
        exhibition.Photo = dto.Photo;

        var museum = await GetOrCreateMuseumAsync(dto);

        var relation = exhibition.MuseumExhibitions.FirstOrDefault();

        if (relation == null)
        {
            exhibition.MuseumExhibitions.Add(new MuseumExhibition
            {
                MuseumId = museum.MuseumId,
                StartDate = DateOnly.FromDateTime(dto.StartDate),
                EndDate = DateOnly.FromDateTime(dto.EndDate)
            });
        }
        else
        {
            relation.MuseumId = museum.MuseumId;
            relation.StartDate = DateOnly.FromDateTime(dto.StartDate);
            relation.EndDate = DateOnly.FromDateTime(dto.EndDate);
        }

        await _exhibitionRepo.SaveChangesAsync();
    }

    public async Task DeleteExhibitionAsync(int id)
    {
        var exhibition = await _exhibitionRepo.GetByIdAsync(id);
        if (exhibition == null) return;

        await _exhibitionRepo.DeleteAsync(exhibition);
        await _exhibitionRepo.SaveChangesAsync();
    }

    public async Task AddTicketAsync(CreateTicketAdminDto dto)
    {
        var tickets = await _ticketRepo.GetTicketsByExhibitionAsync(dto.ExhibitionId);

        if (tickets.Any(t => t.Type == dto.Type))
            throw new Exception("Такой тип билета уже существует");

        var ticket = new Ticket
        {
            ExhibitionId = dto.ExhibitionId,
            Type = dto.Type,
            AvailableQuantity = dto.Quantity,
            Status = "Доступен"
        };

        ticket.TicketPrices.Add(new TicketPrice
        {
            Price = dto.Price,
            StartDate = DateOnly.FromDateTime(DateTime.Now),
            EndDate = DateOnly.MaxValue
        });

        await _ticketRepo.AddAsync(ticket);
        await _ticketRepo.SaveChangesAsync();
    }

    public async Task UpdateTicketAsync(int ticketId, UpdateTicketAdminDto dto)
    {
        var ticket = await _ticketRepo.GetByIdAsync(ticketId)
            ?? throw new Exception("Билет не найден");

        ticket.AvailableQuantity = dto.Quantity;

        ticket.TicketPrices.Add(new TicketPrice
        {
            Price = dto.Price,
            StartDate = DateOnly.FromDateTime(DateTime.Now),
            EndDate = DateOnly.MaxValue
        });

        await _ticketRepo.SaveChangesAsync();
    }

    public async Task AddScheduleAsync(CreateScheduleAdminDto dto)
    {
        var schedule = new MuseumSchedule
        {
            MuseumId = dto.MuseumId,
            StartDate = dto.StartDate,
            EndDate = dto.EndDate,
            OpenTime = dto.Open,
            CloseTime = dto.Close
        };

        await _scheduleRepo.AddAsync(schedule);
        await _scheduleRepo.SaveChangesAsync();
    }


    public async Task CreateUserAsync(string email, string password, string firstName, string lastName, string? middleName, string? phone, string role)
    {
        if (await _userRepo.GetByEmailAsync(email) != null)
            throw new Exception("Пользователь уже существует");

        var user = new User
        {
            Email = email,
            Password = password,
            FirstName = firstName,
            LastName = lastName,
            MiddleName = middleName,
            Phone = phone,
            Role = role
        };

        await _userRepo.AddAsync(user);
        await _userRepo.SaveChangesAsync();
    }

    public async Task DeleteUserAsync(string email)
    {
        var user = await _userRepo.GetByEmailAsync(email);
        if (user == null)
            throw new Exception("Пользователь не найден");

        _userRepo.Delete(user);
        await _userRepo.SaveChangesAsync();
    }

    public async Task CloseTicketSalesAsync(int exhibitionId)
    {
        var tickets = await _ticketRepo.GetTicketsByExhibitionAsync(exhibitionId);

        foreach (var ticket in tickets)
            ticket.Status = "Отменён";

        await _ticketRepo.SaveChangesAsync();
    }

    public async Task<List<OrderDto>> GetAllOrdersAsync()
    {
        return await _orderRepo.GetAllAsync();
    }


    public async Task UpdateOrderStatusAsync(int orderId, string status)
    {
        if (status != "Продан" && status != "Отменён" && status != "Доступен")
            throw new Exception("Недопустимый статус");

        await _orderRepo.UpdateOrderStatusAsync(orderId, status);
    }

    public async Task<IEnumerable<ExhibitionSalesDto>> GetExhibitionSalesAsync()
    {
        var orders = await _orderRepo.GetAllAsync();
        var tickets = await _ticketRepo.GetAllAsync();
        var exhibitions = await _exhibitionRepo.GetAllAsync();

        var ticketsById = tickets.ToDictionary(t => t.TicketId);

        return orders
            .SelectMany(o => o.Tickets)
            .Where(t => ticketsById.ContainsKey(t.TicketId))
            .GroupBy(t => ticketsById[t.TicketId].ExhibitionId)
            .Select(g =>
            {
                var exhibition = exhibitions.FirstOrDefault(e => e.ExhibitionId == g.Key);

                return new ExhibitionSalesDto
                {
                    ExhibitionId = g.Key,
                    ExhibitionName = exhibition?.Name ?? "Неизвестная выставка",
                    TicketsSold = g.Sum(x => x.Quantity)
                };
            })
            .ToList();
    }


    public async Task<(byte[] FileContent, string FileName)> GetStatisticsAsync()
    {
        var orders = await _orderRepo.GetAllAsync();
        var users = await _userRepo.GetAllAsync();
        var tickets = await _ticketRepo.GetAllAsync();
        var exhibitions = await _exhibitionRepo.GetAllAsync();


        decimal revenue = orders.Sum(o => o.Tickets.Sum(t => t.Price * t.Quantity));

        using var workbook = new ClosedXML.Excel.XLWorkbook();
        var ws = workbook.Worksheets.Add("Статистика");

        ws.Cell(1, 1).Value = "Показатель";
        ws.Cell(1, 2).Value = "Значение";

        ws.Range("A1:B1").Style.Font.Bold = true;
        ws.Range("A1:B1").Style.Fill.BackgroundColor = ClosedXML.Excel.XLColor.LightGray;

        ws.Cell(2, 1).Value = "Всего выставок";
        ws.Cell(2, 2).Value = exhibitions.Count();

        ws.Cell(3, 1).Value = "Всего билетов";
        ws.Cell(3, 2).Value = tickets.Count;

        ws.Cell(4, 1).Value = "Пользователей";
        ws.Cell(4, 2).Value = users.Count();

        ws.Cell(5, 1).Value = "Всего заказов";
        ws.Cell(5, 2).Value = orders.Count();

        ws.Cell(6, 1).Value = "Общая выручка";
        ws.Cell(6, 2).Value = revenue;
        ws.Cell(6, 2).Style.NumberFormat.Format = "#,##0.00 ₽";

        ws.Columns().AdjustToContents();

        using var ms = new MemoryStream();
        workbook.SaveAs(ms);

        return (ms.ToArray(), $"Statistics_{DateTime.Now:yyyy-MM-dd}.xlsx");
    }

}