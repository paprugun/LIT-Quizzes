using BlazorApp.Common.Attributes;
using BlazorApp.Models;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace BlazorApp.Shared.Models.RequestModels.User
{
    public class RegistrationRequest
    {
        [Required(ErrorMessage = "Пошта не може бути пустою")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Пошта не в корректному форматі")]
        [CustomRegularExpression("([a-zA-Z0-9]+([+=#-._][a-zA-Z0-9]+)*@([a-zA-Z0-9]+(-[a-zA-Z0-9]+)*)+(([.][a-zA-Z0-9]{2,4})*)?)", ErrorMessage = "Пошта не в корректному форматі")]
        [CustomRegularExpression("(^.{1,64}@.{1,64}$)", ErrorMessage = "Пошта не в корректному форматі")]
        [JsonProperty("email")]
        public string Email { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Пароль повинен містити від 6 до 50 символів", MinimumLength = 6)]
        [CustomRegularExpression("^(?=.*[0-9])(?=.*[a-zA-Z])([a-zA-Z0-9_-]+)", ErrorMessage = "Пароль повинен містити хочаб одну літеру та одну цифру")]
        [CustomRegularExpression("(^(?!\\s+$).+)", ErrorMessage = "Пароль не може містити одні пробіли")]
        [DataType(DataType.Password)]
        [JsonProperty("password")]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "Підтвердети пароль не сходиться із паролем")]
        [DataType(DataType.Password)]
        [JsonProperty("confirmPassword")]
        public string ConfirmPassword{get; set;}

        [Required]
        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [Required]
        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [JsonProperty("isAdmin")]
        public bool isAdmin { get; set; } = false;

    }
}
