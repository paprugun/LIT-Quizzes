using BlazorApp.Common.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorApp.Shared.Models.RequestModels.User
{
    public class AdminLoginRequest
    {
        [CustomRegularExpression("([a-zA-Z0-9]+([+=#-._][a-zA-Z0-9]+)*@([a-zA-Z0-9]+(-[a-zA-Z0-9]+)*)+(([.][a-zA-Z0-9]{2,4})*)?)", ErrorMessage = "Email address is not in valid format")]
        [CustomRegularExpression("(^.{1,64}@.{1,64}$)", ErrorMessage = "Email address is not in valid format")]
        [Required(ErrorMessage = "Email address field is empty")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Email address is not in valid format")]
        [StringLength(129, ErrorMessage = "Email address is not in valid format", MinimumLength = 3)]
        [JsonProperty("email")]
        public string Email { get; set; }

        [CustomRegularExpression("^(?=.*[0-9])(?=.*[a-zA-Z])([a-zA-Z0-9_-]+)", ErrorMessage = "Password should contain at least one letter and one digit")]
        [CustomRegularExpression("(^(?!\\s+$).+)", ErrorMessage = "Password can’t contain spaces only")]
        [StringLength(50, ErrorMessage = "Password should be from 6 to 50 characters", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "password is required")]
        [JsonProperty("password")]
        public string Password { get; set; }

        [JsonProperty("accessTokenLifetime")]
        public int? AccessTokenLifetime { get; set; }
    }
}
