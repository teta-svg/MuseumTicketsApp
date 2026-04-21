using Microsoft.JSInterop;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;

namespace Museum.Blazor.Components.Pages
{
    public partial class SystemAdminPage
    {
        private string activeTab = "users";
        private string message = "";
        private string selectedRole = "Администратор музея";
        private string emailToDelete = "";
        private RegisterRequest newUser = new();

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
                    }
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

        private async Task ShowMessage(string text)
        {
            message = text;
            StateHasChanged();
            await Task.Delay(3000);
            message = "";
            StateHasChanged();
        }

        private async Task CreateUser()
        {
            await SetAuthHeader();
            var resp = await Http.PostAsJsonAsync($"api/admin/users?role={selectedRole}", newUser);

            if (resp.IsSuccessStatusCode)
            {
                newUser = new();
                _ = ShowMessage("Сотрудник успешно создан!");
            }
            else
            {
                _ = ShowMessage("Ошибка создания");
            }
        }

        private async Task DeleteUser()
        {
            if (string.IsNullOrEmpty(emailToDelete)) return;

            await SetAuthHeader();
            var resp = await Http.DeleteAsync($"api/admin/users/{emailToDelete}");

            if (resp.IsSuccessStatusCode)
            {
                emailToDelete = "";
                _ = ShowMessage("Пользователь удален");
            }
            else
            {
                _ = ShowMessage("Ошибка удаления");
            }
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
    }
}