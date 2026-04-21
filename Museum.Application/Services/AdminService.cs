using ClosedXML.Excel;
using Museum.Application.DTOs;
using Museum.Application.Interfaces;
using Museum.Application.Interfaces.Repositories;
using Museum.Domain;

public class AdminService : IAdminService
{
    private readonly IExhibitionRepository _exhibitionRepo;
    private readonly ITicketRepository _ticketRepo;
    private readonly IUserRepository _userRepo;
    private readonly IOrderRepository _orderRepo;
    private readonly IMuseumRepository _museumRepo;
    private readonly IPasswordHasher _passwordHasher;

    public AdminService(
        IExhibitionRepository exhibitionRepo,
        ITicketRepository ticketRepo,
        IUserRepository userRepo,
        IOrderRepository orderRepo,
        IMuseumRepository museumRepo,
        IPasswordHasher passwordHasher)
    {
        _exhibitionRepo = exhibitionRepo;
        _ticketRepo = ticketRepo;
        _userRepo = userRepo;
        _orderRepo = orderRepo;
        _museumRepo = museumRepo;
        _passwordHasher = passwordHasher;
    }

    private async Task<Museum.Domain.Museum> GetOrCreateMuseumAsync(CreateExhibitionAdminDto dto)
    {
        var complex = await _museumRepo.GetComplexByNameAsync(dto.MuseumName);

        if (complex == null)
        {
            complex = new Museum.Domain.MuseumComplex { Name = dto.MuseumName };
            await _museumRepo.AddComplexAsync(complex);
            await _museumRepo.SaveChangesAsync();
        }

        var museum = await _museumRepo.GetByAddressAsync(dto.MuseumName, dto.City, dto.Street, dto.House);

        if (museum == null)
        {
            museum = new Museum.Domain.Museum
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
        else
        {
            museum.MuseumComplexId = complex.MuseumComplexId;
        }

        return museum;
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

        await _exhibitionRepo.AddAsync(exhibition);

        foreach (var t in dto.Tickets)
        {
            var ticket = new Ticket
            {
                Type = t.Type,
                AvailableQuantity = t.Quantity,
                Status = "Доступен",
                TicketPrices = new List<TicketPrice>
                {
                    new TicketPrice { Price = t.Price, StartDate = DateOnly.FromDateTime(DateTime.Now), EndDate = DateOnly.MaxValue }
                }
            };
            exhibition.Tickets.Add(ticket);
        }

        await _exhibitionRepo.SaveChangesAsync();
        return exhibition.ExhibitionId;
    }

    public async Task DeleteExhibitionAsync(int id)
    {
        var exhibition = await _exhibitionRepo.GetByIdAsync(id);
        if (exhibition == null) return;

        exhibition.IsDeleted = true;
        await _exhibitionRepo.SaveChangesAsync();
    }

    public async Task CreateUserAsync(string email, string password, string firstName, string lastName, string? middleName, string? phone, string role)
    {
        if (await _userRepo.GetByEmailAsync(email) != null)
            throw new Exception("Пользователь уже существует");

        var hashedPassword = _passwordHasher.HashPassword(password);

        await _userRepo.AddAsync(new User
        {
            Email = email,
            Password = hashedPassword,
            FirstName = firstName,
            LastName = lastName,
            MiddleName = middleName,
            Phone = phone,
            Role = role
        });

        await _userRepo.SaveChangesAsync();
    }

    public async Task DeleteUserAsync(string email)
    {
        var user = await _userRepo.GetByEmailAsync(email);
        if (user == null) throw new Exception("Пользователь не найден");

        _userRepo.Delete(user);
        await _userRepo.SaveChangesAsync();
    }

    public async Task CloseTicketSalesAsync(int exhibitionId)
    {
        var tickets = await _ticketRepo.GetTicketsByExhibitionAsync(exhibitionId);
        foreach (var ticket in tickets) ticket.Status = "Отменён";

        await _ticketRepo.SaveChangesAsync();
    }

    //эксель
    public async Task<(byte[] FileContent, string FileName)> GetStatisticsAsync()
    {
        var allOrders = await _orderRepo.GetAllAsync();
        var users = await _userRepo.GetAllAsync();
        var tickets = await _ticketRepo.GetAllAsync();
        var exhibitions = await _exhibitionRepo.GetAllAsync();

        var activeExhibitions = exhibitions.Where(e => !e.IsDeleted).ToList();

        int totalPhysicalTickets = tickets
            .Where(t => t.Exhibition != null && !t.Exhibition.IsDeleted)
            .Sum(t => t.AvailableQuantity);

        var paidOrders = allOrders.Where(o => o.Status == "Оплачен").ToList();

        decimal revenue = paidOrders.Sum(o => o.Tickets.Sum(t => t.Price * t.Quantity));

        using var workbook = new XLWorkbook();
        var ws = workbook.Worksheets.Add("Статистика");

        ws.Cell(1, 1).Value = "Показатель";
        ws.Cell(1, 2).Value = "Значение";
        ws.Range("A1:B1").Style.Font.Bold = true;
        ws.Range("A1:B1").Style.Fill.BackgroundColor = XLColor.LightGray;

        ws.Cell(2, 1).Value = "Всего активных выставок";
        ws.Cell(2, 2).Value = activeExhibitions.Count;

        ws.Cell(3, 1).Value = "Общее кол-во билетов (остаток)";
        ws.Cell(3, 2).Value = totalPhysicalTickets;

        ws.Cell(4, 1).Value = "Зарегистрировано пользователей";
        ws.Cell(4, 2).Value = users.Count();

        ws.Cell(5, 1).Value = "Успешных заказов (Оплачено)";
        ws.Cell(5, 2).Value = paidOrders.Count;

        ws.Cell(6, 1).Value = "Итоговая чистая выручка";
        ws.Cell(6, 2).Value = revenue;
        ws.Cell(6, 2).Style.NumberFormat.Format = "#,##0.00 ₽";

        ws.Columns().AdjustToContents();

        using var ms = new MemoryStream();
        workbook.SaveAs(ms);
        return (ms.ToArray(), $"Summary_Report_{DateTime.Now:dd_MM_yyyy}.xlsx");
    }

    public async Task<(byte[] FileContent, string FileName)> ExportUsersAsync()
    {
        using var workbook = new ClosedXML.Excel.XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Users");

        worksheet.Cell(1, 1).Value = "Email";
        worksheet.Cell(1, 2).Value = "Имя";
        worksheet.Cell(1, 3).Value = "Фамилия";
        worksheet.Cell(1, 4).Value = "Отчество";
        worksheet.Cell(1, 5).Value = "Телефон";
        worksheet.Cell(1, 6).Value = "Роль";
        worksheet.Range(1, 1, 1, 7).Style.Font.SetBold();

        var users = await _userRepo.GetAllAsync();
        int row = 2;

        foreach (var user in users)
        {
            worksheet.Cell(row, 1).Value = user.Email;
            worksheet.Cell(row, 2).Value = user.FirstName;
            worksheet.Cell(row, 3).Value = user.LastName;
            worksheet.Cell(row, 4).Value = user.MiddleName;
            worksheet.Cell(row, 5).Value = user.Phone;
            worksheet.Cell(row, 6).Value = user.Role;
            row++;
        }

        worksheet.Columns().AdjustToContents();

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return (stream.ToArray(), "Users_Export.xlsx");
    }
    public async Task<(byte[] FileContent, string FileName)> ExportExhibitionsAsync()
    {
        using var workbook = new ClosedXML.Excel.XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Exhibitions");

        worksheet.Cell(1, 1).Value = "Название выставки";
        worksheet.Cell(1, 2).Value = "Фото (URL)";
        worksheet.Cell(1, 3).Value = "Музей";
        worksheet.Cell(1, 4).Value = "Город";
        worksheet.Cell(1, 5).Value = "Улица";
        worksheet.Cell(1, 6).Value = "Дом";

        worksheet.Range(1, 1, 1, 8).Style.Font.SetBold();

        var exhibitions = await _exhibitionRepo.GetAllAsync();
        var activeExhibitions = exhibitions.Where(ex => !ex.IsDeleted).ToList();
        int row = 2;

        foreach (var ex in activeExhibitions)
        {
            if (ex.MuseumExhibitions != null && ex.MuseumExhibitions.Any())
            {
                foreach (var me in ex.MuseumExhibitions)
                {
                    worksheet.Cell(row, 1).Value = ex.Name;
                    worksheet.Cell(row, 2).Value = ex.Photo;
                    worksheet.Cell(row, 3).Value = me.Museum?.Name;
                    worksheet.Cell(row, 4).Value = me.Museum?.City;
                    worksheet.Cell(row, 5).Value = me.Museum?.Street;
                    worksheet.Cell(row, 6).Value = me.Museum?.House;

                    row++;
                }
            }
            else
            {
                worksheet.Cell(row, 1).Value = ex.Name;
                worksheet.Cell(row, 2).Value = ex.Photo;
                row++;
            }
        }

        worksheet.Columns().AdjustToContents();

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return (stream.ToArray(), "Exhibitions_Export.xlsx");
    }

    public async Task<IEnumerable<ExhibitionSalesDto>> GetExhibitionSalesAsync()
    {
        var exhibitions = await _exhibitionRepo.GetAllAsync();

        return exhibitions.Select(e => new ExhibitionSalesDto
        {
            ExhibitionId = e.ExhibitionId,
            ExhibitionName = e.Name,

            TicketsSold = e.Tickets
                .SelectMany(t => t.OrderItems ?? new List<OrderItem>())
                .Where(oi => oi.Order != null && oi.Order.Status == "Оплачен")
                .Sum(oi => oi.Quantity)
        }).ToList();
    }
}