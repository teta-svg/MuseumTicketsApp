using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Museum.Application.DTOs;

namespace Museum.Blazor.Components.Pages
{
    public partial class ExhibitionDetails
    {
        [Parameter] public int id { get; set; }
        private ExhibitionDetailsDTO? details;
        private Dictionary<int, int> selectedQuantities = new();

        private bool showModal = false;
        private bool isProcessing = false;
        private bool isOrderCreated = false;
        private int createdOrderId;
        private string errorMessage = "";

        protected override async Task OnInitializedAsync() => await LoadData();

        private async Task LoadData()
        {
            try { details = await Https.GetFromJsonAsync<ExhibitionDetailsDTO>($"api/Exhibitions/{id}"); }
            catch { details = null; }
        }

        private void ChangeQuantity(int ticketId, int change)
        {
            var current = selectedQuantities.GetValueOrDefault(ticketId, 0);
            var newVal = current + change;
            if (newVal >= 0) selectedQuantities[ticketId] = newVal;
        }

        private int GetSelectedCount(int ticketId) => selectedQuantities.GetValueOrDefault(ticketId, 0);
        private int GetTotalCount() => selectedQuantities.Values.Sum();
        private decimal GetTotalPrice() => details?.Tickets.Sum(t => GetSelectedCount(t.TicketID) * t.Price) ?? 0;

        private async Task PrepareOrder()
        {
            var token = await JS.InvokeAsync<string>("localStorage.getItem", "authToken");
            if (string.IsNullOrEmpty(token))
            {
                Navigation.NavigateTo($"login?returnUrl={Uri.EscapeDataString(Navigation.Uri)}");
                return;
            }
            showModal = true;
        }

        private async Task CreateOrder()
        {
            isProcessing = true;
            errorMessage = "";
            try
            {
                var token = await JS.InvokeAsync<string>("localStorage.getItem", "authToken");
                Https.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var tickets = selectedQuantities.Where(x => x.Value > 0)
                    .Select(x => new OrderTicketRequestDto { TicketId = x.Key, Quantity = x.Value }).ToList();

                var response = await Https.PostAsJsonAsync("api/orders", new CreateOrderDto { ExhibitionId = id, Tickets = tickets });

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<OrderDto>();
                    createdOrderId = result.OrderId;
                    isOrderCreated = true;
                }
                else { errorMessage = "Не удалось создать заказ. Попробуйте обновить страницу."; }
            }
            catch (Exception ex) { errorMessage = "Ошибка: " + ex.Message; }
            finally { isProcessing = false; }
        }

        private async Task PayOrder()
        {
            isProcessing = true;
            try
            {
                var token = await JS.InvokeAsync<string>("localStorage.getItem", "authToken");
                Https.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var response = await Https.PostAsJsonAsync($"api/payments/{createdOrderId}", GetTotalPrice());
                if (response.IsSuccessStatusCode) Navigation.NavigateTo("/orders");
                else errorMessage = "Оплата отклонена платежной системой.";
            }
            catch (Exception ex) { errorMessage = "Ошибка связи: " + ex.Message; }
            finally { isProcessing = false; }
        }

        private void CloseModal() { showModal = false; isOrderCreated = false; errorMessage = ""; }

        private string GetDayName(int day) => day switch
        {
            1 => "Понедельник",
            2 => "Вторник",
            3 => "Среда",
            4 => "Четверг",
            5 => "Пятница",
            6 => "Суббота",
            7 => "Воскресенье",
            _ => "Неизвестно"
        };
    }
}
