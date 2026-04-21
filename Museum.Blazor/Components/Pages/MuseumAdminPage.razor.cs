using Microsoft.JSInterop;
using Museum.Domain.DTOs;
using System.Net.Http.Headers;

namespace Museum.Blazor.Components.Pages
{
    public partial class MuseumAdminPage
    {
        private List<ExhibitionSalesDto> salesReports;
        private ExhibitionSalesDto selectedReport;
        private bool showModal = false;
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

        private void OpenModal()
        {
            newExhibition = new CreateExhibitionAdminDto
            {
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddMonths(1),
                Tickets = new() { new CreateTicketAdminDto { Type = "Взрослый", Quantity = 100, Price = 500 } }
            };
            showModal = true;
        }

        private void CloseModal() => showModal = false;

        private void AddTicket() => newExhibition.Tickets.Add(new CreateTicketAdminDto { Type = "Новый", Quantity = 50, Price = 300 });

        private async Task SaveExhibition()
        {
            try
            {
                await SetAuthHeader();
                var res = await Http.PostAsJsonAsync("api/admin/exhibitions", newExhibition);

                if (res.IsSuccessStatusCode)
                {
                    showModal = false;
                    await LoadData();
                    await JS.InvokeVoidAsync("alert", "Выставка успешно опубликована!");
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
            if (!await JS.InvokeAsync<bool>("confirm", "Удалить объект? Это действие нельзя отменить.")) return;

            await SetAuthHeader();
            var res = await Http.DeleteAsync($"api/admin/exhibitions/{selectedReport.ExhibitionId}");

            if (res.IsSuccessStatusCode)
            {
                await LoadData();
                selectedReport = null;
            }
            else
            {
                await JS.InvokeVoidAsync("alert", "Не удалось удалить. Возможно, есть активные заказы.");
            }
        }
    }
}