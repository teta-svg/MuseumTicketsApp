using Microsoft.JSInterop;
using Museum.Application.DTOs;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;

namespace Museum.Blazor.Components.Pages.Admin.SystemAdmin
{
    public partial class SystemAdminPage
    {
        private string activeTab = "users";
        private string message = "";
        private string adminEmail = "";
        private string selectedRole = "Администратор музея";
        private string emailToDelete = "";
        private bool isInitialized = false;

        private RegisterRequest newUser = new();
        private List<OrderDto>? orders;

        private async Task ShowMessage(string text, int duration = 3000)
        {
            message = text;
            StateHasChanged();
            await Task.Delay(duration);
            message = "";
            StateHasChanged();
        }

        private async Task LoadSales()
        {
            activeTab = "stats";
            message = "";

            if (orders == null)
            {
                await LoadOrders();
            }
            StateHasChanged();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                var token = await JS.InvokeAsync<string>("localStorage.getItem", "authToken");

                if (string.IsNullOrEmpty(token))
                {
                    Nav.NavigateTo("/login");
                    return;
                }

                try
                {
                    var handler = new JwtSecurityTokenHandler();
                    var jwt = handler.ReadJwtToken(token.Trim('"'));

                    var roles = jwt.Claims
                        .Where(c => c.Type == "role" || c.Type.Contains("role"))
                        .Select(c => c.Value.ToLower()).ToList();

                    if (!roles.Contains("администратор системы"))
                    {
                        Nav.NavigateTo("/");
                        return;
                    }

                    adminEmail = jwt.Claims.FirstOrDefault(c => c.Type == "email" || c.Type == "sub")?.Value ?? "";
                    isInitialized = true;
                    StateHasChanged();
                }
                catch
                {
                    Nav.NavigateTo("/login");
                }
            }
        }

        private async Task SetAuthHeader()
        {
            var token = await JS.InvokeAsync<string>("localStorage.getItem", "authToken");
            if (!string.IsNullOrEmpty(token))
                Http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Trim('"'));
        }

        private void SetTab(string tab)
        {
            activeTab = tab;
            message = "";
        }

        private async Task LoadOrders()
        {
            SetTab("orders");
            await SetAuthHeader();
            try
            {
                orders = await Http.GetFromJsonAsync<List<OrderDto>>("api/admin/orders");
            }
            catch (Exception ex)
            {
                orders = new();
                _ = ShowMessage("Ошибка загрузки: " + ex.Message);
            }
        }

        private async Task UpdateStatus(int id, string status)
        {
            await SetAuthHeader();
            var resp = await Http.PatchAsync($"api/admin/orders/{id}/status?status={status}", null);
            if (resp.IsSuccessStatusCode)
            {
                _ = ShowMessage("Статус обновлен!");
                await LoadOrders();
            }
            else { _ = ShowMessage("Не удалось обновить статус"); }
        }

        private async Task CreateUser()
        {
            await SetAuthHeader();
            var resp = await Http.PostAsJsonAsync($"api/admin/users?role={selectedRole}", newUser);
            _ = ShowMessage(resp.IsSuccessStatusCode ? "Сотрудник успешно создан!" : "Ошибка создания");
            if (resp.IsSuccessStatusCode) newUser = new();
        }

        private async Task DeleteUser()
        {
            if (string.IsNullOrEmpty(emailToDelete)) return;
            await SetAuthHeader();
            var resp = await Http.DeleteAsync($"api/admin/users/{emailToDelete}");
            _ = ShowMessage(resp.IsSuccessStatusCode ? "Пользователь удален" : "Ошибка удаления");
        }

        private async Task ExportFile(string url, string fileName)
        {
            message = "Генерация файла...";
            StateHasChanged();
            await SetAuthHeader();
            var resp = await Http.GetAsync(url);
            if (resp.IsSuccessStatusCode)
            {
                var content = await resp.Content.ReadAsByteArrayAsync();
                await JS.InvokeVoidAsync("saveAsFile", fileName, Convert.ToBase64String(content));
                _ = ShowMessage("Загрузка завершена");
            }
            else
            {
                _ = ShowMessage("Ошибка при скачивании");
            }
        }

        private async Task DownloadExcel() { await ExportFile("api/admin/statistics", "Report.xlsx"); }
    }
}
