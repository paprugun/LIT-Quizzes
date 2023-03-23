using System.ComponentModel.DataAnnotations;

namespace BlazorApp.Models.RequestModels
{
    public class ConfirmEmailRequestModel
    {
        [Required(ErrorMessage = "Token field is empty")]
        public string Token { get; set; }
    }

}
