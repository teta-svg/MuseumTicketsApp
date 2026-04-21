using Microsoft.JSInterop;
using Museum.Application.DTOs;
using System.IdentityModel.Tokens.Jwt;

namespace Museum.Blazor.Components.Pages
{
    public partial class LoginRegister
    {
        private string activeTab = "login";
        private string message;
        public class AuthResponse { public string Token { get; set; } }

        private LoginRequest loginModel { get; set; } = new();
        private RegisterRequest regModel { get; set; } = new();

        private async Task ShowMessage(string text)
        {
            message = text;
            StateHasChanged();
            await Task.Delay(4000);
            message = "";
            StateHasChanged();
        }

        private void SetTab(string tab)
        {
            activeTab = tab;
            message = "";
        }

        private async Task SubmitLogin()
        {
            try
            {
                var resp = await Http.PostAsJsonAsync("api/Auth/login", loginModel);
                if (resp.IsSuccessStatusCode)
                {
                    var authData = await resp.Content.ReadFromJsonAsync<AuthResponse>();
                    var token = authData?.Token ?? (await resp.Content.ReadAsStringAsync()).Trim('"');

                    await JS.InvokeVoidAsync("localStorage.setItem", "authToken", token);

                    var handler = new JwtSecurityTokenHandler();
                    var jwt = handler.ReadJwtToken(token);
                    var roles = jwt.Claims
                        .Where(c => c.Type == "role" || c.Type.Contains("role"))
                        .Select(c => c.Value.ToLower()).ToList();

                    if (roles.Contains("администратор системы"))
                        Navigation.NavigateTo("/admin/system", true);
                    else if (roles.Contains("администратор музея"))
                        Navigation.NavigateTo("/admin/museum", true);
                    else
                        Navigation.NavigateTo("/", true);
                }
                else { _ = ShowMessage("Неверный логин или пароль"); }
            }
            catch (Exception ex) { _ = ShowMessage("Ошибка соединения с сервером"); }
        }

        private async Task SubmitRegister()
        {
            var resp = await Http.PostAsJsonAsync("api/Auth/register", regModel);
            if (resp.IsSuccessStatusCode)
            {
                _ = ShowMessage("Регистрация успешна! Войдите в аккаунт.");
                activeTab = "login";
            }
            else
            {
                _ = ShowMessage("Ошибка регистрации. Проверьте данные.");
            }
        }
    }
}
