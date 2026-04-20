using Microsoft.JSInterop;
using Museum.Application.DTOs;

namespace Museum.Blazor.Components.Pages.Orders
{
    public partial class MyOrders
    {
        private List<OrderDto>? orders;
        private bool isRefreshing = false;

        protected override async Task OnInitializedAsync() => await LoadOrders();

        private async Task LoadOrders()
        {
            try
            {
                isRefreshing = true;
                var token = await JS.InvokeAsync<string>("localStorage.getItem", "authToken");
                if (string.IsNullOrEmpty(token))
                {
                    Navigation.NavigateTo("login");
                    return;
                }

                Http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                orders = await Http.GetFromJsonAsync<List<OrderDto>>("api/orders/my");
            }
            catch (Exception ex)
            {
                orders = new List<OrderDto>();
                Console.WriteLine("Ошибка загрузки заказов: " + ex.Message);
            }
            finally
            {
                isRefreshing = false;
            }
        }

        private string GetStatusClass(string status) => status switch
        {
            "Оплачен" => "bg-success-subtle text-success",
            "В ожидании" => "bg-warning-subtle text-warning border border-warning",
            "Отменён" => "bg-danger-subtle text-danger",
            _ => "bg-light text-muted"
        };

        private async Task PayPendingOrder(OrderDto order)
        {
            var token = await JS.InvokeAsync<string>("localStorage.getItem", "authToken");
            Http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            decimal total = order.Tickets.Sum(t => t.Price * t.Quantity);
            var response = await Http.PostAsJsonAsync($"api/payments/{order.OrderId}", total);

            if (response.IsSuccessStatusCode)
            {
                await LoadOrders();
            }
        }

        private async Task DownloadTicket(OrderDto order)
        {
            try
            {
                isRefreshing = true;
                var token = await JS.InvokeAsync<string>("localStorage.getItem", "authToken");
                Http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var response = await Http.GetAsync($"api/orders/{order.OrderId}/download");

                if (response.IsSuccessStatusCode)
                {
                    var fileData = await response.Content.ReadFromJsonAsync<FileDownloadDto>();
                    if (fileData != null)
                    {
                        await JS.InvokeVoidAsync("saveAsFile", fileData.FileName, fileData.Base64);
                    }
                }
                else
                {
                    await JS.InvokeVoidAsync("alert", "Ошибка при генерации билета.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                isRefreshing = false;
                StateHasChanged();
            }
        }

        public class FileDownloadDto
        {
            public string FileName { get; set; } = string.Empty;
            public string Base64 { get; set; } = string.Empty;
        }
    }
}
