using System.ComponentModel.DataAnnotations;

namespace BlazorApp.Models.RequestModels
{
    public class RefreshTokenRequestModel
    {
        [Required(ErrorMessage = "Refresh Token is empty")]
        public string RefreshToken { get; set; }
    }
}
