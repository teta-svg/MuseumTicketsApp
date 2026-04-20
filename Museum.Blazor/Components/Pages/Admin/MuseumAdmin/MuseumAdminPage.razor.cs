using Microsoft.JSInterop;
using Museum.Application.DTOs;
using System.Net.Http.Headers;

namespace Museum.Blazor.Components.Pages.Admin.MuseumAdmin
{
    public partial class MuseumAdminPage
    {
        private List<ExhibitionSalesDto> salesReports;
        private ExhibitionSalesDto selectedReport;
        private bool showModal = false;
        private bool isEditMode = false;
        private int currentId = 0;
        private CreateExhibitionAdminDto newExhibition = new();

        protected override async Task OnInitializedAsync() => await LoadData();

        private async Task LoadData()
        {
            try
            {
                await SetAuthHeader();
                salesReports = await Http.GetFromJsonAsync<List<ExhibitionSalesDto>>("api/admin/exhibitions/list");
            }
            catch { salesReports = new(); }
        }

        private async Task SetAuthHeader()
        {
            var token = await JS.InvokeAsync<string>("auth.getToken");
            if (!string.IsNullOrEmpty(token))
                Http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        private async Task OpenModal(int? id = null)
        {
            if (id.HasValue)
            {
                isEditMode = true;
                currentId = id.Value;
                var selected = salesReports.FirstOrDefault(x => x.ExhibitionId == id.Value);
                newExhibition = new CreateExhibitionAdminDto
                {
                    Name = selected?.ExhibitionName,
                    StartDate = DateTime.Today,
                    EndDate = DateTime.Today.AddMonths(1),
                    Tickets = new(),
                    Schedules = new()
                };
            }
            else
            {
                isEditMode = false;
                newExhibition = new CreateExhibitionAdminDto { StartDate = DateTime.Today, EndDate = DateTime.Today.AddMonths(1), Tickets = new() { new CreateTicketAdminDto { Type = "Взрослый", Quantity = 100, Price = 500 } }, Schedules = new() };
            }
            showModal = true;
        }

        private void CloseModal() => showModal = false;
        private void AddTicket() => newExhibition.Tickets.Add(new CreateTicketAdminDto { Type = "Новый", Quantity = 50, Price = 300 });
        private void AddSchedule() => newExhibition.Schedules.Add(new CreateScheduleAdminDto { StartDate = DateOnly.FromDateTime(DateTime.Today), EndDate = DateOnly.FromDateTime(DateTime.Today), Open = new TimeOnly(10, 0), Close = new TimeOnly(19, 0) });

        private async Task SaveExhibition()
        {
            try
            {
                await SetAuthHeader();
                HttpResponseMessage res = isEditMode
                    ? await Http.PutAsJsonAsync($"api/admin/exhibitions/{currentId}", newExhibition)
                    : await Http.PostAsJsonAsync("api/admin/exhibitions", newExhibition);

                if (res.IsSuccessStatusCode)
                {
                    showModal = false;
                    await LoadData();
                    await JS.InvokeVoidAsync("alert", "Успешно сохранено!");
                }
                else
                {
                    var err = await res.Content.ReadAsStringAsync();
                    await JS.InvokeVoidAsync("alert", $"Ошибка: {err}");
                }
            }
            catch (Exception ex) { await JS.InvokeVoidAsync("alert", $"Критическая ошибка: {ex.Message}"); }
        }

        private async Task CloseSales()
        {
            if (selectedReport == null) return;
            await SetAuthHeader();
            var res = await Http.PostAsync($"api/admin/exhibitions/{selectedReport.ExhibitionId}/close-sales", null);
            if (res.IsSuccessStatusCode) await JS.InvokeVoidAsync("alert", "Продажи закрыты");
        }

        private async Task DeleteExhibition()
        {
            if (selectedReport == null) return;
            if (!await JS.InvokeAsync<bool>("confirm", "Вы уверены?")) return;
            await SetAuthHeader();
            var res = await Http.DeleteAsync($"api/admin/exhibitions/{selectedReport.ExhibitionId}");
            if (res.IsSuccessStatusCode) { await LoadData(); selectedReport = null; }
        }
    }
}
