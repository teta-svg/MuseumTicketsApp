using System.ComponentModel.DataAnnotations;

public class RegisterRequest
{

    [Required(ErrorMessage = "Email обязателен")]
    [EmailAddress(ErrorMessage = "Некорректный формат Email")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Пароль обязателен")]
    [MinLength(6, ErrorMessage = "Минимум 6 символов")]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "Имя обязательно")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Фамилия обязательна")]
    public string LastName { get; set; } = string.Empty;

    public string? MiddleName { get; set; }
    public string? Phone { get; set; }
}

