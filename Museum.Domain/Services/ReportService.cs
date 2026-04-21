using ClosedXML.Excel;
using Museum.Domain.DTOs;
using Museum.Domain.Entities;
using Museum.Domain.Interfaces.Repositories;
using Museum.Domain.Interfaces.Services;


namespace Museum.Domain.Services
{

    public class ReportService: IReportService
    {
        private readonly IOrderRepository _orderRepo;
        private readonly IUserRepository _userRepo;
        private readonly ITicketRepository _ticketRepo;
        private readonly IExhibitionRepository _exhibitionRepo;

        public ReportService(
            IOrderRepository orderRepo,
            IUserRepository userRepo,
            ITicketRepository ticketRepo,
            IExhibitionRepository exhibitionRepo)
        {
            _orderRepo = orderRepo;
            _userRepo = userRepo;
            _ticketRepo = ticketRepo;
            _exhibitionRepo = exhibitionRepo;
        }

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
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Users");

            string[] headers = { "Email", "Имя", "Фамилия", "Отчество", "Телефон", "Роль" };
            for (int i = 0; i < headers.Length; i++)
            {
                worksheet.Cell(1, i + 1).Value = headers[i];
            }
            worksheet.Range(1, 1, 1, 6).Style.Font.SetBold();

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
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Exhibitions");

            worksheet.Cell(1, 1).Value = "Название выставки";
            worksheet.Cell(1, 2).Value = "Фото (URL)";
            worksheet.Cell(1, 3).Value = "Музей";
            worksheet.Cell(1, 4).Value = "Город";
            worksheet.Cell(1, 5).Value = "Улица";
            worksheet.Cell(1, 6).Value = "Дом";
            worksheet.Range(1, 1, 1, 6).Style.Font.SetBold();

            var exhibitions = await _exhibitionRepo.GetAllAsync();
            var activeExhibitions = exhibitions.Where(ex => !ex.IsDeleted).ToList();
            int row = 2;

            foreach (var ex in activeExhibitions)
            {
                if (ex.MuseumExhibitions?.Any() == true)
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
                    .Where(oi => oi.Order?.Status == "Оплачен")
                    .Sum(oi => oi.Quantity)
            }).ToList();
        }
    }

}
